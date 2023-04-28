// Decompiled with JetBrains decompiler
// Type: Microsoft.WindowsPhone.ImageUpdate.ImageApp
// Assembly: ImageApp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: B39B358B-EDF0-49C6-A2AD-5D3866215E5E
// Assembly location: C:\Users\Admin\Desktop\re\dig\imageapp.exe

using Microsoft.Composition.ToolBox;
using Microsoft.Composition.ToolBox.IO;
using Microsoft.Composition.ToolBox.Logging;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;
using Microsoft.WindowsPhone.Imaging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsPhone.ImageUpdate
{
  public static class ImageApp
  {
    private static bool _randomizeGptIDs = true;
    private static bool _recovery = false;
    private static string _outputFile = string.Empty;
    private static string _oemInputFile = string.Empty;
    private static string _msPackagesRoot = string.Empty;
    private static string _scratchDir = string.Empty;
    private static string c_LogFileBase = "ImageApp.log";
    private static string _LogFile = string.Empty;
    private static Microsoft.CompPlat.PkgBldr.Tools.CommandLineParser _commandLineParser = (Microsoft.CompPlat.PkgBldr.Tools.CommandLineParser) null;
    private static bool _bDoingUpdate = false;
    private static bool _bSkipUpdateMain = false;
    private static string _cpuType = string.Empty;
    private static CpuArch _cpuArch = CpuArch.Invalid;
    private static Logger _pkgToolBoxLogger = new Logger();
    private static IULogger _logger = new IULogger();
    private static bool _strictSettingPolicies = false;
    private static bool _skipImaging = false;
    private static bool _showDebugMessages = false;
    private static string _chunkMappingFile;
    private static string _ErrorLogFile;
    private static bool _patchImage = false;
    private static PatchList _imagePatchList = (PatchList) null;
    private static readonly object _lock = new object();

    private static void Main()
    {
      if (!Microsoft.CompPlat.PkgBldr.Tools.CommonUtils.IsCurrentUserAdmin())
      {
        ImageApp._logger.LogError("Please run imageapp.exe in an elevated console.", new object[0]);
        Environment.ExitCode = 5;
      }
      else
      {
        Stopwatch stopwatch = new Stopwatch();
        try
        {
          ImagingTelemetryLogger.Instance.LogAppInvokedEvent();
          stopwatch.Start();
          Microsoft.WindowsPhone.Imaging.Imaging imaging = ImageApp.InitializeImaging();
          if (imaging == null)
            return;
          ImageApp.BeginImaging(imaging);
        }
        catch (Exception ex)
        {
          Console.WriteLine(string.Format("Exception occured during ImageApp execution : [{0}].", (object) ex));
          Environment.ExitCode = ex.HResult;
          ImagingTelemetryLogger.Instance.LogFailedEvent(ex, CodeSite.Imaging, ErrorCategory.Unknown);
        }
        finally
        {
          PathToolBox.CleanupTempDirectories();
          stopwatch.Stop();
          ImagingTelemetryLogger.Instance.LogAppExitEvent(Environment.ExitCode, stopwatch.ElapsedMilliseconds);
        }
      }
    }

    private static void BeginImaging(Microsoft.WindowsPhone.Imaging.Imaging imaging)
    {
      try
      {
        if (ImageApp._bDoingUpdate)
          imaging.UpdateExistingImage(ImageApp._outputFile, ImageApp._imagePatchList, ImageApp._randomizeGptIDs);
        else
          imaging.BuildNewImage(ImageApp._outputFile, ImageApp._oemInputFile, ImageApp._msPackagesRoot, ImageApp._randomizeGptIDs, ImageApp._recovery);
        ImageApp._logger.LogInfo("Successfully finished Imaging execution.", new object[0]);
      }
      catch
      {
        Console.WriteLine("ImageApp: Error log can be found at '" + ImageApp._LogFile + "'.");
        throw;
      }
      finally
      {
        ImageApp.LogServicingStackErrors(ImageApp._ErrorLogFile);
      }
    }

    private static Microsoft.WindowsPhone.Imaging.Imaging InitializeImaging()
    {
      ImageApp.SetCmdLineParams();
      try
      {
        if (!ImageApp.ParseCommandlineParams(Environment.CommandLine))
        {
          Environment.ExitCode = 1;
          ImagingTelemetryLogger.Instance.LogFailedEvent((Exception) new ArgumentException("Invalid args"), CodeSite.Imaging, ErrorCategory.BadCommandLineParams);
          ImageApp.ShowUsageString();
          return (Microsoft.WindowsPhone.Imaging.Imaging) null;
        }
        if (!string.IsNullOrEmpty(ImageApp._scratchDir))
        {
          PathToolBox.SetTempRoot(ImageApp._scratchDir);
          Environment.SetEnvironmentVariable("VHDTMP", ImageApp._scratchDir);
          Environment.SetEnvironmentVariable("TMP", ImageApp._scratchDir);
          Environment.SetEnvironmentVariable("TEMP", ImageApp._scratchDir);
        }
        ImageApp._logger = !PathToolBox.GetDirectoryName(ImageApp._outputFile).EqualsIgnoreCase(DirectoryToolBox.GetRootDirectory(ImageApp._outputFile)) ? ImageApp.CreateIULogger(ImageApp._outputFile, ImageApp.c_LogFileBase) : throw new ArgumentException("Due to ImageApp's logging requirements, the image path provided on the command line cannot be in the root of a volume. Please try again with a different path. Path provided: " + ImageApp._outputFile);
        ImageApp._logger.LogInfo("Command line: {0}", new object[1]
        {
          (object) Environment.CommandLine
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        Environment.ExitCode = 1;
        ImagingTelemetryLogger.Instance.LogFailedEvent(ex, CodeSite.Imaging, ErrorCategory.BadCommandLineParams);
        ImageApp.ShowUsageString();
        return (Microsoft.WindowsPhone.Imaging.Imaging) null;
      }
      Microsoft.WindowsPhone.Imaging.Imaging imaging = ImageApp._bDoingUpdate ? new Microsoft.WindowsPhone.Imaging.Imaging(ImageApp._logger, ImageApp._cpuArch) : new Microsoft.WindowsPhone.Imaging.Imaging(ImageApp._logger, ImageApp._skipImaging, ImageApp._strictSettingPolicies, ImageApp._bSkipUpdateMain, ImageApp._cpuArch, ImageApp._chunkMappingFile);
      Console.CancelKeyPress += new ConsoleCancelEventHandler(imaging.CleanupHandler);
      return imaging;
    }

    private static void AppendToLog(
      string prepend,
      LoggingFunc LoggingFunc,
      string format,
      params object[] list)
    {
      string format1 = format;
      if (list != null && list.Length != 0)
        format1 = string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, list);
      LoggingFunc(format1, new object[0]);
      string contents = string.Format("{{{0}}} {1} {2}{3}", (object) DateTime.FromFileTime(DateTime.Now.ToFileTime()), (object) prepend, (object) format1, (object) Environment.NewLine);
      lock (ImageApp._lock)
        FileToolBox.AppendAllText(ImageApp._LogFile, contents);
    }

    private static void LogErrorToFileAndConsole(string format, params object[] list) => ImageApp.AppendToLog("Error:", new LoggingFunc(ImageApp._pkgToolBoxLogger.LogError), format, list);

    private static void LogWarningToFileAndConsole(string format, params object[] list) => ImageApp.AppendToLog("Warning:", new LoggingFunc(ImageApp._pkgToolBoxLogger.LogWarning), format, list);

    private static void LogInfoToFileAndConsole(string format, params object[] list) => ImageApp.AppendToLog(string.Empty, new LoggingFunc(ImageApp._pkgToolBoxLogger.LogInfo), format, list);

    private static void LogDebugToFileAndConsole(string format, params object[] list) => ImageApp.AppendToLog("Debug:", new LoggingFunc(ImageApp._pkgToolBoxLogger.LogDebug), format, list);

    private static bool SetCmdLineParams()
    {
      try
      {
        ImageApp._commandLineParser = new Microsoft.CompPlat.PkgBldr.Tools.CommandLineParser();
        ImageApp._commandLineParser.SetRequiredParameterString("OutputFile", "The path to the image to be created\\modified");
        ImageApp._commandLineParser.SetOptionalParameterString("OEMInputXML", "Path to the OEM Input XML file");
        ImageApp._commandLineParser.SetOptionalParameterString("MSPackageRoot", "Path to the Microsoft Package files root. Only used when OEM Input XML");
        ImageApp._commandLineParser.SetOptionalSwitchString("MountAndInstall", "Semi-colon delimited list of items to install (files or directories). Recursively tries to install content.");
        ImageApp._commandLineParser.SetOptionalSwitchBoolean("StrictSettingPolicies", "Causes settings without policies to produce errors", ImageApp._strictSettingPolicies);
        ImageApp._commandLineParser.SetOptionalSwitchBoolean("SkipImageCreation", "Generates the OEM Customization packages without generating the full image", ImageApp._skipImaging);
        ImageApp._commandLineParser.SetOptionalSwitchBoolean("RandomizeGptIDs", "Randomizes the GPT Disk and Partiton IDs for imaging.  Needed to run ImageApp in parallel", ImageApp._randomizeGptIDs);
        ImageApp._commandLineParser.SetOptionalSwitchBoolean("Recovery", "Create a recovery FFU instead of a full FFU", ImageApp._recovery);
        ImageApp._commandLineParser.SetOptionalSwitchBoolean("ShowDebugMessages", "Show additional debug messages", ImageApp._showDebugMessages);
        ImageApp._commandLineParser.SetRequiredSwitchString("CPUType", "Specify target CPU type of x86, ARM, ARM64 or AMD64", false, "ARM", "X86", "ARM64", "AMD64", "nothing");
        ImageApp._commandLineParser.SetOptionalSwitchString("ChunkMappingFile", "Reserved");
        ImageApp._commandLineParser.SetOptionalSwitchString("ScratchDir", "Specify a scratch folder for all writes");
        ImageApp._commandLineParser.SetOptionalSwitchBoolean("Patch", "Patch an existing image with new content. Requires at least one: /Drivers, /Customizations, /TestContent.", ImageApp._patchImage);
        ImageApp._commandLineParser.SetOptionalSwitchString("Drivers", "Folder of drivers to be patched onto an existing image.");
        ImageApp._commandLineParser.SetOptionalSwitchString("Customizations", "Folder of customization packages to be patched onto an existing image.");
        ImageApp._commandLineParser.SetOptionalSwitchString("TestContent", "Folder of test content to be patched onto an existing image (copied into DATA\\TestContent).");
      }
      catch (Exception ex)
      {
        throw new Microsoft.CompPlat.PkgBldr.Tools.NoSuchArgumentException("ImageApp!SetCmdLineParams: Unable to set an option", ex);
      }
      return true;
    }

    public static IULogger CreateIULogger(string outputFile, string logFileBase)
    {
      Microsoft.CompPlat.PkgBldr.Tools.LogUtil.LogCopyright();
      IULogger iuLogger = new IULogger();
      ImageApp._pkgToolBoxLogger.SetLoggingLevel(Microsoft.Composition.ToolBox.Logging.LoggingLevel.Info);
      iuLogger.ErrorLogger = new LogString(ImageApp.LogErrorToFileAndConsole);
      iuLogger.WarningLogger = new LogString(ImageApp.LogWarningToFileAndConsole);
      iuLogger.InformationLogger = new LogString(ImageApp.LogInfoToFileAndConsole);
      iuLogger.DebugLogger = ImageApp._showDebugMessages ? new LogString(ImageApp.LogDebugToFileAndConsole) : (LogString) null;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string withoutExtension = PathToolBox.GetFileNameWithoutExtension(outputFile);
      if (withoutExtension.Length == 0)
      {
        Console.WriteLine("ImageApp!ParseCommandLineParams: The Output File cannot be empty when extension is removed.");
        return (IULogger) null;
      }
      ImageApp._LogFile = PathToolBox.Combine(Microsoft.CompPlat.PkgBldr.Tools.FileUtils.GetShortPathName(PathToolBox.GetDirectoryName(outputFile)), withoutExtension + "." + logFileBase);
      if (ImageApp._LogFile.Length > 260)
        ImageApp._LogFile = Microsoft.CompPlat.PkgBldr.Tools.FileUtils.GetShortPathName(ImageApp._LogFile);
      FileToolBox.Delete(ImageApp._LogFile);
      iuLogger.LogFile = ImageApp._LogFile;
      ImageApp._ErrorLogFile = ImageApp.ConfigureServicingStackLogging(ImageApp._LogFile);
      return iuLogger;
    }

    private static bool ParseCommandlineParams(string Commandline)
    {
      if (!ImageApp._commandLineParser.ParseString(Commandline, true))
        return false;
      ImageApp._outputFile = ImageApp._commandLineParser.GetParameterAsString("OutputFile");
      ImageApp._strictSettingPolicies = ImageApp._commandLineParser.GetSwitchAsBoolean("StrictSettingPolicies");
      ImageApp._skipImaging = ImageApp._commandLineParser.GetSwitchAsBoolean("SkipImageCreation");
      ImageApp._randomizeGptIDs = ImageApp._commandLineParser.GetSwitchAsBoolean("RandomizeGptIDs");
      ImageApp._recovery = ImageApp._commandLineParser.GetSwitchAsBoolean("Recovery");
      ImageApp._msPackagesRoot = ImageApp._commandLineParser.GetParameterAsString("MSPackageRoot");
      ImageApp._oemInputFile = ImageApp._commandLineParser.GetParameterAsString("OEMInputXML");
      ImageApp._showDebugMessages = ImageApp._commandLineParser.GetSwitchAsBoolean("ShowDebugMessages");
      ImageApp._scratchDir = ImageApp._commandLineParser.GetSwitchAsString("ScratchDir");
      ImageApp._bSkipUpdateMain = string.IsNullOrEmpty(ImageApp._oemInputFile) && !ImageApp._bDoingUpdate;
      ImageApp._cpuType = ImageApp._commandLineParser.GetSwitchAsString("CPUType");
      ImageApp._patchImage = ImageApp._commandLineParser.GetSwitchAsBoolean("Patch");
      ImageApp._imagePatchList = new PatchList(ImageApp._commandLineParser.GetSwitchAsString("Drivers"), ImageApp._commandLineParser.GetSwitchAsString("Customizations"), string.Empty, ImageApp._commandLineParser.GetSwitchAsString("TestContent"));
      string switchAsString = ImageApp._commandLineParser.GetSwitchAsString("MountAndInstall");
      if (ImageApp._patchImage && ImageApp._imagePatchList.IsEmpty())
      {
        Console.WriteLine("ImageApp!ParseCommandLineParams: At least one collateral list must be given with /Patch. (/Drivers, /Customizations, /TestContent)");
        return false;
      }
      if (!ImageApp._patchImage && !ImageApp._imagePatchList.IsEmpty())
      {
        Console.WriteLine("ImageApp!ParseCommandLineParams: Collateral lists cannot be given without the /Patch parameter. (/Drivers, /Customizations, /TestContent)");
        return false;
      }
      if (ImageApp._patchImage && !string.IsNullOrEmpty(switchAsString))
      {
        Console.WriteLine("ImageApp!ParseCommandLineParams: /Patch and /MountAndInstall cannot be used at the same time.");
        return false;
      }
      if (!string.IsNullOrEmpty(switchAsString))
        ImageApp._imagePatchList = new PatchList(switchAsString, true);
      ImageApp._bDoingUpdate = !ImageApp._imagePatchList.IsEmpty();
      if (string.Compare(ImageApp._cpuType, "nothing", StringComparison.OrdinalIgnoreCase) != 0)
      {
        try
        {
          ImageApp._cpuArch = ManifestToolBox.CpuArchParser(ImageApp._cpuType);
        }
        catch
        {
          Console.WriteLine("ImageApp!ParseCommandLineParams: The CPUType was not a recognized type.");
          return false;
        }
        Console.WriteLine("ImageApp: Setting CPU Type to '" + ImageApp._cpuType + "'.");
      }
      ImageApp._chunkMappingFile = ImageApp._commandLineParser.GetSwitchAsString("ChunkMappingFile");
      if (string.IsNullOrEmpty(ImageApp._chunkMappingFile) || FileToolBox.Exists(ImageApp._chunkMappingFile))
        return true;
      Console.WriteLine("ImageApp!ParseCommandLineParams: The ChunkMappingFile '{0}' does not exist.", (object) ImageApp._chunkMappingFile);
      return false;
    }

    private static void ShowUsageString() => Console.WriteLine(ImageApp._commandLineParser.UsageString());

    [DllImport("UpdateDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern int SetupWdscore(string LogFile, string ErrorFile);

    [DllImport("wdscore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern void WdsSetupLogDestroy();

    private static string ConfigureServicingStackLogging(string ImgUpdLogFile)
    {
      string str1 = PathToolBox.ChangeExtension(ImgUpdLogFile, ".cbs.log");
      FileToolBox.Delete(str1);
      string str2 = str1 + ".err";
      FileToolBox.Delete(str2);
      if (!Win32Exports.FAILED(ImageApp.SetupWdscore(str1, str2)))
        return str2;
      Console.WriteLine("ImageApp!ConfigureServicingStackLogging: Failed initializing WdsCore.dll.");
      return str2;
    }

    private static void LogServicingStackErrors(string ErrorLogFile)
    {
      if (ErrorLogFile == null || !FileToolBox.Exists(ErrorLogFile))
        return;
      ImageApp.WdsSetupLogDestroy();
      foreach (string readAllLine in FileToolBox.ReadAllLines(ErrorLogFile))
        Microsoft.CompPlat.PkgBldr.Tools.LogUtil.Error(readAllLine);
    }
  }
}
