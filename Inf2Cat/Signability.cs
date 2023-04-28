// Decompiled with JetBrains decompiler
// Type: Microsoft.UniversalStore.HardwareWorkflow.Inf2Cat.Signability
// Assembly: Inf2Cat, Version=3.3.0.0, Culture=neutral, PublicKeyToken=8fdc415ae438dfeb
// MVID: 52190100-9076-4EF7-B45F-77CDBE3D58DE
// Assembly location: C:\Users\Admin\Desktop\re\dig\Inf2Cat.exe

using Microsoft.UniversalStore.HardwareWorkflow.Catalogs;
using Microsoft.UniversalStore.HardwareWorkflow.SubmissionBuilder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microsoft.UniversalStore.HardwareWorkflow.Inf2Cat
{
  internal sealed class Signability
  {
    private string _driverPath;
    private OperatingSystems _commandLineOperatingSystemsValue;
    private bool _ignoreErrors;
    private bool _useLocalTime;
    private bool _noCatalog;
    private bool _verbose;
    private readonly Collection<CatalogAttributeInfo> _globalAttributes;
    private readonly Collection<CatalogAttributeInfo> _fileAttributes;
    private readonly ListDictionary _fileNameToFileAttributes;

    internal SupportedOS[] SupportedOperatingSystems { get; set; }

    public Signability()
    {
      this.SupportedOperatingSystems = new SupportedOS[49]
      {
        new SupportedOS("2000", OperatingSystems.Windows2000, false),
        new SupportedOS("XP_X86", OperatingSystems.WindowsXPX86, false),
        new SupportedOS("XP_X64", OperatingSystems.WindowsXPX64, false),
        new SupportedOS("SERVER2003_X86", OperatingSystems.WindowsServer2003X86, false),
        new SupportedOS("SERVER2003_IA64", OperatingSystems.WindowsServer2003IA64, false),
        new SupportedOS("SERVER2003_X64", OperatingSystems.WindowsServer2003X64, false),
        new SupportedOS("VISTA_X86", OperatingSystems.WindowsVistaX86, false),
        new SupportedOS("VISTA_X64", OperatingSystems.WindowsVistaX64, false),
        new SupportedOS("SERVER2008_X86", OperatingSystems.WindowsServer2008X86, false),
        new SupportedOS("SERVER2008_IA64", OperatingSystems.WindowsServer2008IA64, false),
        new SupportedOS("SERVER2008_X64", OperatingSystems.WindowsServer2008X64, false),
        new SupportedOS("7_X86", OperatingSystems.Windows7X86, false),
        new SupportedOS("7_X64", OperatingSystems.Windows7X64, false),
        new SupportedOS("SERVER2008R2_IA64", OperatingSystems.WindowsServer2008R2IA64, false),
        new SupportedOS("SERVER2008R2_X64", OperatingSystems.WindowsServer2008R2X64, false),
        new SupportedOS("8_X86", OperatingSystems.Windows8X86, false),
        new SupportedOS("8_X64", OperatingSystems.Windows8X64, false),
        new SupportedOS("8_ARM", OperatingSystems.Windows8ARM, false),
        new SupportedOS("SERVER8_X64", OperatingSystems.WindowsServer2012X64, false),
        new SupportedOS("6_3_X86", OperatingSystems.Windows_v63, false),
        new SupportedOS("6_3_X64", OperatingSystems.Windows_v63_X64, false),
        new SupportedOS("6_3_ARM", OperatingSystems.Windows_v63_ARM, false),
        new SupportedOS("SERVER6_3_X64", OperatingSystems.Windows_v63_Server_X64, false),
        new SupportedOS("10_X86", OperatingSystems.Windows_v100, false),
        new SupportedOS("10_X64", OperatingSystems.Windows_v100_X64, false),
        new SupportedOS("SERVER10_X64", OperatingSystems.WindowsServer_v100_X64, false),
        new SupportedOS("SERVER10_ARM64", OperatingSystems.WindowsServer_v100_ARM64, false),
        new SupportedOS("10_AU_X86", OperatingSystems.Windows_v100_RS1, false),
        new SupportedOS("10_AU_X64", OperatingSystems.Windows_v100_X64_RS1, false),
        new SupportedOS("SERVER2016_X64", OperatingSystems.WindowsServer_v100_X64_RS1, false),
        new SupportedOS("10_RS2_X86", OperatingSystems.Windows_v100_RS2, false),
        new SupportedOS("10_RS2_X64", OperatingSystems.Windows_v100_X64_RS2, false),
        new SupportedOS("10_RS3_X86", OperatingSystems.Windows_v100_RS3, false),
        new SupportedOS("10_RS3_X64", OperatingSystems.Windows_v100_X64_RS3, false),
        new SupportedOS("10_RS3_ARM64", OperatingSystems.Windows_v100_ARM64_RS3, false),
        new SupportedOS("10_RS4_X86", OperatingSystems.Windows_v100_RS4, false),
        new SupportedOS("10_RS4_X64", OperatingSystems.Windows_v100_X64_RS4, false),
        new SupportedOS("10_RS4_ARM64", OperatingSystems.Windows_v100_ARM64_RS4, false),
        new SupportedOS("10_RS5_X86", OperatingSystems.Windows_v100_RS5, false),
        new SupportedOS("10_RS5_X64", OperatingSystems.Windows_v100_X64_RS5, false),
        new SupportedOS("SERVERRS5_X64", OperatingSystems.WindowsServer_v100_X64_RS5, false),
        new SupportedOS("10_RS5_ARM64", OperatingSystems.Windows_v100_ARM64_RS5, false),
        new SupportedOS("SERVERRS5_ARM64", OperatingSystems.WindowsServer_v100_ARM64_RS5, false),
        new SupportedOS("10_19H1_X86", OperatingSystems.Windows_v100_19H1, false),
        new SupportedOS("10_19H1_X64", OperatingSystems.Windows_v100_X64_19H1, false),
        new SupportedOS("10_19H1_ARM64", OperatingSystems.Windows_v100_ARM64_19H1, false),
        new SupportedOS("10_VB_X86", OperatingSystems.Windows_v100_VB, false),
        new SupportedOS("10_VB_X64", OperatingSystems.Windows_v100_X64_VB, false),
        new SupportedOS("10_VB_ARM64", OperatingSystems.Windows_v100_ARM64_VB, false)
      };
      this._driverPath = string.Empty;
      this._globalAttributes = new Collection<CatalogAttributeInfo>();
      this._fileAttributes = new Collection<CatalogAttributeInfo>();
      this._fileNameToFileAttributes = new ListDictionary();
    }

    [STAThread]
    internal static int Main(string[] args)
    {
      int num1 = 0;
      int num2;
      try
      {
        Signability signability = new Signability();
        if (Signability.IsHelpInArguments(args))
        {
          Signability.DisplayUsage();
          return num1;
        }
        num2 = signability.ParseArguments(args) ? signability.ExecuteSignabilityTests() : -1;
      }
      catch (Exception ex)
      {
        Console.WriteLine();
        Console.WriteLine(ex.Message);
        num2 = -1;
      }
      return num2;
    }

    internal static bool IsHelpInArguments(string[] args) => args == null || args.Length == 0 || ((IEnumerable<string>) args).Any<string>(new Func<string, bool>(Signability.IsHelpArgument));

    internal bool ParseArguments(string[] args)
    {
      string empty = string.Empty;
      if (args.Length == 1 && !Signability.IsHelpArgument(args[0]))
      {
        Console.WriteLine("Parameter format not correct.");
        return false;
      }
      for (int index = 0; index < args.Length; ++index)
      {
        if (args[index].StartsWith("/OS:", StringComparison.OrdinalIgnoreCase))
        {
          empty = args[index];
          break;
        }
      }
      if (!this.ParseOSParameter(empty))
        return false;
      this._commandLineOperatingSystemsValue = OperatingSystems.None;
      for (int index = 0; index < this.SupportedOperatingSystems.Length; ++index)
      {
        if (this.SupportedOperatingSystems[index].OsBool)
          this._commandLineOperatingSystemsValue |= this.SupportedOperatingSystems[index].OsEnum;
      }
      for (int index = 0; index < args.Length; ++index)
      {
        if (!args[index].StartsWith("/OS:", StringComparison.OrdinalIgnoreCase))
        {
          if (string.Equals(args[index], "/NOCAT", StringComparison.OrdinalIgnoreCase))
            this._noCatalog = true;
          else if (string.Equals(args[index], "/VERBOSE", StringComparison.OrdinalIgnoreCase) || string.Equals(args[index], "/V", StringComparison.OrdinalIgnoreCase))
            this._verbose = true;
          else if (args[index].StartsWith("/DRIVER:", StringComparison.OrdinalIgnoreCase) && this._driverPath.Length == 0)
            this._driverPath = args[index].Remove(0, "/DRIVER:".Length);
          else if (args[index].StartsWith("/DRV:", StringComparison.OrdinalIgnoreCase) && this._driverPath.Length == 0)
          {
            this._driverPath = args[index].Remove(0, "/DRV:".Length);
          }
          else
          {
            if (args[index].StartsWith("/DRM", StringComparison.OrdinalIgnoreCase))
              throw new ArgumentException("Use of DRM argument no longer supported. Remove DRM argument and try again");
            if (args[index].StartsWith("/PE", StringComparison.OrdinalIgnoreCase))
              throw new ArgumentException("Use of PETrust argument no longer supported. Remove PETrust argument and try again");
            if (args[index].StartsWith("/PAGEHASHES", StringComparison.OrdinalIgnoreCase))
            {
              if (Environment.OSVersion.Version.Major < 6)
                throw new ArgumentException("Page hashes cannot be generated when running INF2CAT on an operating system released prior to Windows Vista.");
              if (args[index].Length == "/PAGEHASHES".Length)
                Signability.InsertCatalogAttributeAtFront((IList<CatalogAttributeInfo>) this._fileAttributes, Constants.PageHashesAttribute);
              else
                this.InsertSpecificFileAttributesAtFront(Signability.ParseFileList(args[index].Remove(0, "/PAGEHASHES".Length + 1)), Constants.PageHashesAttribute);
            }
            else if (args[index].StartsWith("/HEADERATTRIBUTE", StringComparison.OrdinalIgnoreCase) || args[index].StartsWith("/HA", StringComparison.OrdinalIgnoreCase))
              this.HandleCustomCatalogAttribute(args[index], this._globalAttributes, false);
            else if (args[index].StartsWith("/FILEATTRIBUTE", StringComparison.OrdinalIgnoreCase) || args[index].StartsWith("/FA", StringComparison.OrdinalIgnoreCase))
              this.HandleCustomCatalogAttribute(args[index], this._fileAttributes, true);
            else if (string.Equals(args[index], "/IGNOREERRORS", StringComparison.OrdinalIgnoreCase))
              this._ignoreErrors = true;
            else if (string.Equals(args[index], "/USELOCALTIME", StringComparison.OrdinalIgnoreCase))
            {
              this._useLocalTime = true;
            }
            else
            {
              Console.WriteLine("Parameter format not correct.");
              return false;
            }
          }
        }
      }
      if (this._driverPath.Length == 0)
      {
        Console.WriteLine("Driver path missing.");
        return false;
      }
      if (Directory.Exists(this._driverPath))
        return true;
      Console.WriteLine(this._driverPath + " does not exist.");
      return false;
    }

    private static bool IsHelpArgument(string argument) => string.Equals(argument, "/?", StringComparison.OrdinalIgnoreCase) || string.Equals(argument, "-?", StringComparison.OrdinalIgnoreCase);

    private void HandleCustomCatalogAttribute(
      string arg,
      Collection<CatalogAttributeInfo> attributes,
      bool isFileAttr)
    {
      int num = arg.IndexOf(':', 0);
      if (num < 0)
        throw new Inf2CatException("Parameter format not correct.");
      this.ParseCustomCatalogAttribute(arg.Remove(0, num + 1), (ICollection<CatalogAttributeInfo>) attributes, isFileAttr);
    }

    private bool ParseOSParameter(string osParameter)
    {
      if (osParameter.Length == 0)
      {
        Console.WriteLine("Operating systems parameter missing.");
        return false;
      }
      osParameter = osParameter.Remove(0, "/OS:".Length);
      string[] strArray = osParameter.Split(',');
      if (strArray.Length == 0)
      {
        Console.WriteLine("Operating systems parameter invalid.");
        return false;
      }
      for (int index = 0; index < strArray.Length; ++index)
      {
        bool flag = false;
        foreach (SupportedOS supportedOperatingSystem in this.SupportedOperatingSystems)
        {
          if (string.Equals(strArray[index], supportedOperatingSystem.OsString, StringComparison.OrdinalIgnoreCase))
          {
            if (supportedOperatingSystem.OsBool)
            {
              Console.WriteLine("Operating systems parameter invalid.");
              return false;
            }
            flag = true;
            supportedOperatingSystem.OsBool = true;
            break;
          }
        }
        if (!flag)
        {
          Console.WriteLine("Operating systems parameter invalid.");
          return false;
        }
      }
      return true;
    }

    private Collection<CatalogAttributeInfo> GetSpecificFileAttributes(
      string file)
    {
      Collection<CatalogAttributeInfo> specificFileAttributes = (Collection<CatalogAttributeInfo>) this._fileNameToFileAttributes[(object) file];
      if (specificFileAttributes == null)
        this._fileNameToFileAttributes.Add((object) file, (object) (specificFileAttributes = new Collection<CatalogAttributeInfo>()));
      return specificFileAttributes;
    }

    private void InsertSpecificFileAttributesAtFront(
      StringCollection files,
      CatalogAttributeInfo attribute)
    {
      foreach (string file in files)
        Signability.InsertCatalogAttributeAtFront((IList<CatalogAttributeInfo>) this.GetSpecificFileAttributes(file), attribute);
    }

    private void AddSpecificFileAttributes(StringCollection files, CatalogAttributeInfo attribute)
    {
      foreach (string file in files)
        Signability.AddCatalogAttribute((ICollection<CatalogAttributeInfo>) this.GetSpecificFileAttributes(file), attribute);
    }

    private static void DisplayUsage()
    {
      Console.WriteLine("Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
      Console.WriteLine("Runs driver signability tests and creates the catalog(s).");
      Console.WriteLine();
      Console.WriteLine("INF2CAT /driver:path /os:operatingSystem1[,os2]...");
      Console.WriteLine("        [/nocat] [/verbose]");
      Console.WriteLine("        [/drm[:file1[,file2]...]]");
      Console.WriteLine("        [/pe[:file1[,file2]...]]");
      Console.WriteLine("        [/pageHashes[:file1][,file2]...]]");
      Console.WriteLine();
      Console.WriteLine("  /driver (/drv)    Indicates the path to the driver package follows.");
      Console.WriteLine();
      Console.WriteLine("  path              Specifies the path to the driver package.");
      Console.WriteLine();
      Console.WriteLine("  /os               Indicates the operating system(s) targeted by the driver");
      Console.WriteLine("                    package follows. The targeted operating system(s) is a");
      Console.WriteLine("                    comma separated list of the following values:");
      Console.WriteLine();
      Console.WriteLine("  operatingSystem1  2000");
      Console.WriteLine();
      Console.WriteLine("                    XP_X86           Server2003_X86");
      Console.WriteLine("                    XP_X64           Server2003_X64");
      Console.WriteLine("                                     Server2003_IA64");
      Console.WriteLine();
      Console.WriteLine("                    Vista_X86        Server2008_X86");
      Console.WriteLine("                    Vista_X64        Server2008_X64");
      Console.WriteLine("                                     Server2008_IA64");
      Console.WriteLine();
      Console.WriteLine("                    7_X86");
      Console.WriteLine("                    7_X64            Server2008R2_X64");
      Console.WriteLine("                                     Server2008R2_IA64");
      Console.WriteLine();
      Console.WriteLine("                    8_X86");
      Console.WriteLine("                    8_X64            Server8_X64");
      Console.WriteLine("                    8_ARM");
      Console.WriteLine();
      Console.WriteLine("                    6_3_X86");
      Console.WriteLine("                    6_3_X64          Server6_3_X64");
      Console.WriteLine("                    6_3_ARM");
      Console.WriteLine();
      Console.WriteLine("                    10_X86");
      Console.WriteLine("                    10_X64           Server10_X64");
      Console.WriteLine("                                     Server10_ARM64");
      Console.WriteLine();
      Console.WriteLine("                    10_AU_X86");
      Console.WriteLine("                    10_AU_X64        Server2016_X64");
      Console.WriteLine();
      Console.WriteLine("                    10_RS2_X86");
      Console.WriteLine("                    10_RS2_X64");
      Console.WriteLine();
      Console.WriteLine("                    10_RS3_X86");
      Console.WriteLine("                    10_RS3_X64");
      Console.WriteLine("                    10_RS3_ARM64");
      Console.WriteLine("                    10_RS4_X86");
      Console.WriteLine("                    10_RS4_X64");
      Console.WriteLine("                    10_RS4_ARM64");
      Console.WriteLine("                    10_RS5_X86");
      Console.WriteLine("                    10_RS5_X64       ServerRS5_X64");
      Console.WriteLine("                    10_RS5_ARM64     ServerRS5_ARM64");
      Console.WriteLine("                    10_19H1_X86");
      Console.WriteLine("                    10_19H1_X64");
      Console.WriteLine("                    10_19H1_ARM64");
      Console.WriteLine("                    10_VB_X86");
      Console.WriteLine("                    10_VB_X64");
      Console.WriteLine("                    10_VB_ARM64");
      Console.WriteLine();
      Console.WriteLine("  /uselocaltime     Use local timezone while running driver");
      Console.WriteLine("                    timestamp verification tests. By default UTC is used.");
      Console.WriteLine();
      Console.WriteLine("  /nocat            Prevents the creation of the catalog(s).");
      Console.WriteLine();
      Console.WriteLine("  /verbose (/v)     Displays detailed console output.");
      Console.WriteLine();
      Console.WriteLine("  /drm              (Deprecated command line arg. Add drm signature attribute in .inf file to add drm signature attribute)");
      Console.WriteLine();
      Console.WriteLine("  /pe               (Deprecated command line arg. Add petrust signature attribute in .inf file to add petrust signature attribute)");
      Console.WriteLine();
      Console.WriteLine("  /pageHashes       Include page hashes with files.  Optionally");
      Console.WriteLine("                    followed by a list of files.");
    }

    private int ExecuteSignabilityTests()
    {
      int num = 0;
      try
      {
        using (DriverPackageInfo driverPackageInfo = new DriverPackageInfo(this._driverPath, this._commandLineOperatingSystemsValue, false, this._useLocalTime, true, true))
        {
          driverPackageInfo.SignabilityStatus += new EventHandler<SubmissionEventArgs>(this.ConsoleUpdate);
          List<string> errors = new List<string>();
          List<string> warnings = new List<string>();
          bool flag = driverPackageInfo.TestSignability(out errors, out warnings);
          if (flag)
          {
            Console.WriteLine("\nSignability test complete.");
          }
          else
          {
            Console.WriteLine("\nSignability test failed.");
            if (!this._ignoreErrors)
              num = -2;
          }
          Console.WriteLine("\nErrors:");
          if (errors.Count == 0)
          {
            Console.WriteLine("None");
          }
          else
          {
            for (int index = 0; index < errors.Count; ++index)
            {
              Console.WriteLine(errors[index]);
              if (errors[index].StartsWith("22.9.7: DriverVer set to incorrect date (postdated DriverVer not allowed) in "))
                Console.WriteLine(string.Format("Try using {0} when DriverVer is postdated", (object) "/USELOCALTIME"));
            }
          }
          Console.WriteLine("\nWarnings:");
          if (warnings.Count == 0)
          {
            Console.WriteLine("None");
          }
          else
          {
            for (int index = 0; index < warnings.Count; ++index)
              Console.WriteLine(warnings[index]);
          }
          if ((flag || this._ignoreErrors) && !this._noCatalog)
          {
            StringCollection catalogs = driverPackageInfo.GenerateCatalogs(this._globalAttributes, this._fileAttributes, this._fileNameToFileAttributes, true);
            Console.WriteLine("\nCatalog generation complete.");
            if (catalogs != null)
            {
              for (int index = 0; index < catalogs.Count; ++index)
                Console.WriteLine(catalogs[index]);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine();
        Console.WriteLine(ex.Message);
        Console.WriteLine("Signability test failed.");
        num = -1;
      }
      return num;
    }

    private void ConsoleUpdate(object sender, SubmissionEventArgs args)
    {
      if (this._verbose)
        Console.WriteLine(args.Message);
      else
        Console.Write(".");
    }

    private static bool FindCatalogAttribute(
      IEnumerable<CatalogAttributeInfo> attributes,
      CatalogAttributeInfo attribute)
    {
      foreach (CatalogAttributeInfo attribute1 in attributes)
      {
        if (string.Compare(attribute1.Id, attribute.Id, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(attribute1.Value, attribute.Value, StringComparison.OrdinalIgnoreCase) == 0 && attribute1.Type == attribute.Type)
          return true;
      }
      return false;
    }

    private static void InsertCatalogAttributeAtFront(
      IList<CatalogAttributeInfo> attributes,
      CatalogAttributeInfo attribute)
    {
      if (Signability.FindCatalogAttribute((IEnumerable<CatalogAttributeInfo>) attributes, attribute))
        return;
      attributes.Insert(0, attribute);
    }

    private static void AddCatalogAttribute(
      ICollection<CatalogAttributeInfo> attributes,
      CatalogAttributeInfo attribute)
    {
      if (Signability.FindCatalogAttribute((IEnumerable<CatalogAttributeInfo>) attributes, attribute))
        return;
      attributes.Add(attribute);
    }

    private static StringCollection ParseFileList(string fileList)
    {
      StringCollection stringCollection = new StringCollection();
      string[] strArray = fileList.Split(',');
      for (int index = 0; index < strArray.Length; ++index)
      {
        strArray[index] = strArray[index].Trim().ToLowerInvariant();
        if (strArray[index].Length > 0 && !stringCollection.Contains(strArray[index]))
          stringCollection.Add(strArray[index]);
      }
      return stringCollection.Count != 0 ? stringCollection : throw new Inf2CatException("Parameter format not correct.");
    }

    private void ParseCustomCatalogAttribute(
      string customAttrString,
      ICollection<CatalogAttributeInfo> attributes,
      bool isFileAttr)
    {
      string[] strArray = customAttrString.Split(new char[1]
      {
        ':'
      }, 3);
      if (strArray.Length <= 2)
        throw new Inf2CatException("Parameter format not correct.");
      int int32;
      try
      {
        int32 = Convert.ToInt32(strArray[0], 16);
      }
      catch (Exception ex)
      {
        throw new Inf2CatException("Parameter format not correct.", ex);
      }
      int length = -1;
      do
      {
        length = strArray[2].IndexOf(':', length + 1);
      }
      while (length > 0 && strArray[2][length - 1] == '^');
      string str;
      string fileList;
      if (length < 0)
      {
        str = string.Concat(strArray[2].Split('^'));
        fileList = string.Empty;
      }
      else
      {
        str = string.Concat(strArray[2].Substring(0, length).Split('^'));
        fileList = strArray[2].Substring(length + 1, strArray[2].Length - (length + 1));
      }
      if (!isFileAttr && fileList != null && fileList.Length > 0)
        throw new Inf2CatException("Parameter format not correct.");
      CatalogAttributeInfo attribute = new CatalogAttributeInfo(strArray[1], str, int32);
      if (isFileAttr && fileList != null && fileList.Length > 0)
        this.AddSpecificFileAttributes(Signability.ParseFileList(fileList), attribute);
      else
        Signability.AddCatalogAttribute(attributes, attribute);
    }
  }
}
