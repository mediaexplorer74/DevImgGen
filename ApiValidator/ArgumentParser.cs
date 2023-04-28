// Decompiled with JetBrains decompiler
// Type: Microsoft.DriverKit.ApiValidator.ArgumentParser
// Assembly: ApiValidator, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E124E9BD-E446-409E-89FC-814012FE58D3
// Assembly location: C:\Users\Admin\Desktop\re\dig\apivalidator.exe

using Microsoft.Kits.Drivers.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace Microsoft.DriverKit.ApiValidator
{
  internal class ArgumentParser
  {
    private readonly Microsoft.Kits.Drivers.Validation.ApiValidator api;

    public ArgumentParser(Microsoft.Kits.Drivers.Validation.ApiValidator apiv) => this.api = apiv;

    private static void ShowUsage() => Utility.OutputStandard(ConstantsEXE.UsageInfo);

    private void DisplayParsingError(string message, params object[] extraArgs)
    {
      ArgumentParser.ShowUsage();
      int foregroundColor = (int) Console.ForegroundColor;
      Console.ForegroundColor = ConsoleColor.Red;
      Utility.OutputError(message, extraArgs);
      Console.ForegroundColor = (ConsoleColor) foregroundColor;
    }

    private void DisplayParsingErrorBasic(string message, params object[] extraArgs)
    {
      int foregroundColor = (int) Console.ForegroundColor;
      Console.ForegroundColor = ConsoleColor.Red;
      Utility.OutputError(message, extraArgs);
      Console.ForegroundColor = (ConsoleColor) foregroundColor;
    }

    private string ParseArgumentValue(string arg)
    {
      int num = arg.IndexOf(":", StringComparison.OrdinalIgnoreCase);
      if (num <= 0)
        return (string) null;
      string str = arg.Substring(num + 1).Trim('"');
      return string.IsNullOrWhiteSpace(str) ? (string) null : str;
    }

    private List<string> ParsePaths(string arg, bool checkFile)
    {
      string argumentValue = this.ParseArgumentValue(arg);
      if (argumentValue == null)
      {
        this.DisplayParsingError("arg {0} cannot be set to NULL or empty string.\n", (object) arg);
        return (List<string>) null;
      }
      List<string> list = ((IEnumerable<string>) argumentValue.Split(new char[1]
      {
        ';'
      }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
      foreach (string path1 in list)
      {
        try
        {
          if (File.GetAttributes(path1).HasFlag((Enum) FileAttributes.Directory))
          {
            if (!Directory.Exists(path1))
            {
              this.DisplayParsingError("Directory {0} does not exist.", (object) path1);
              return (List<string>) null;
            }
          }
          else
          {
            string path2 = path1.Trim('"');
            if (checkFile)
            {
              if (!File.Exists(path2))
              {
                this.DisplayParsingError("File {0} not found", (object) path2);
                return (List<string>) null;
              }
            }
          }
        }
        catch (IOException ex)
        {
          this.DisplayParsingError("File or Directory {0} not found", (object) ex.Message);
          return (List<string>) null;
        }
      }
      return list;
    }

    private bool ParseDriverPaths(string arg)
    {
      List<string> paths = this.ParsePaths(arg, true);
      if (paths == null || !paths.Any<string>())
      {
        this.DisplayParsingErrorBasic("Invalid value specified for the \"DriverPackagePath\" argument.");
        return false;
      }
      foreach (string str in paths)
      {
        if (Directory.Exists(str))
        {
          this.api.PathSettings.DriverPaths.Add(new DriverInfo(str.TrimEnd('\\'), true));
        }
        else
        {
          if (!File.Exists(str))
            return false;
          this.api.PathSettings.DriverPaths.Add(new DriverInfo(str, false));
        }
      }
      return true;
    }

    private bool ParseSupportedApiPaths(string arg)
    {
      List<string> paths = this.ParsePaths(arg, true);
      if (paths == null || !paths.Any<string>())
      {
        this.DisplayParsingError("Invalid value specified for the \"SupportedApiXmlFiles\" argument.");
        return false;
      }
      this.api.PathSettings.SupportedApisFilePaths.AddRange((IEnumerable<string>) paths);
      return true;
    }

    private bool StrictComplianceCheck(string arg)
    {
      string str = arg;
      if (string.IsNullOrEmpty(arg))
      {
        this.DisplayParsingError("Invalid value specified for the \"SupportedApiXmlFiles\" argument.");
        return false;
      }
      if (!str.ToLower().Trim().Equals("strictcompliance:true"))
        return true;
      this.api.PathSettings.SkipIsApiSetImplementedCheck = false;
      return true;
    }

    private bool ParseExceptionFilePaths(string arg)
    {
      List<string> paths = this.ParsePaths(arg, true);
      if (paths == null || !paths.Any<string>())
      {
        this.DisplayParsingError("Invalid value specified for the \"ModuleWhiteListXmlFiles\" argument.");
        return false;
      }
      this.api.PathSettings.ExceptionFilePaths.AddRange((IEnumerable<string>) paths);
      return true;
    }

    private static Dictionary<string, string> LoadBinaryExclusionList(
      string BinaryExclusionXmlPath)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      XmlReader xmlReader = XmlReader.Create(BinaryExclusionXmlPath, new XmlReaderSettings()
      {
        ValidationType = ValidationType.DTD,
        DtdProcessing = DtdProcessing.Parse,
        ConformanceLevel = ConformanceLevel.Fragment,
        IgnoreWhitespace = true,
        IgnoreComments = true
      });
      int content = (int) xmlReader.MoveToContent();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      while (xmlReader.Read())
      {
        switch (xmlReader.Name)
        {
          case "Binary":
            if (!string.IsNullOrEmpty(xmlReader.GetAttribute("Name")))
            {
              string lower = xmlReader.GetAttribute("Name").Trim().ToLower();
              string str = xmlReader.GetAttribute("Reason") + xmlReader.GetAttribute("MSVSO");
              if (!dictionary.ContainsKey(lower))
              {
                dictionary.Add(lower, str);
                continue;
              }
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      xmlReader?.Close();
      return dictionary;
    }

    private bool ParseBinaryExceptionFilePath(string arg)
    {
      List<string> paths = this.ParsePaths(arg, true);
      if (paths == null || !paths.Any<string>())
      {
        this.DisplayParsingError("Invalid value specified for the \"BinaryExclusionListXmlFile\" argument.");
        return false;
      }
      this.api.PathSettings.BinaryExclusionList = ArgumentParser.LoadBinaryExclusionList(paths[0]);
      if (this.api.PathSettings.BinaryExclusionList.Count != 0)
        return true;
      this.DisplayParsingError("The \"BinaryExclusionListXmlFile\" file has no binary files to exclude from verification.");
      return false;
    }

    private bool ParseExclusionFilePaths(string arg)
    {
      List<string> paths = this.ParsePaths(arg, true);
      if (paths == null || !paths.Any<string>())
      {
        this.DisplayParsingError("Invalid value specified for the \"ExclusionList\" argument.");
        return false;
      }
      foreach (string key in paths)
        this.api.PathSettings.ExclusionList.Add(key, false);
      return true;
    }

    private bool ParseTempOutputFolder(string arg)
    {
      string argumentValue = this.ParseArgumentValue(arg);
      if (argumentValue == null)
      {
        this.DisplayParsingError("Invalid value specified for the \"TempOutputFolder\" argument.");
        return false;
      }
      if (!Directory.Exists(argumentValue))
      {
        this.DisplayParsingError("Temp output folder {0} doesn't exist. Please give a valid empty directory.", (object) argumentValue);
        return false;
      }
      this.api.PathSettings.TempFilePath = argumentValue;
      return true;
    }

    private bool ParseApiExtractorExePath(string arg)
    {
      string argumentValue = this.ParseArgumentValue(arg);
      if (argumentValue == null)
      {
        this.DisplayParsingError("Invalid value specified for the \"ApiExtractorExePath\" argument.");
        return false;
      }
      if (!Directory.Exists(argumentValue))
      {
        this.DisplayParsingError("Folder Path {0} to aitstatic.exe not found.", (object) argumentValue);
        return false;
      }
      if (!File.Exists(Path.Combine(argumentValue, "aitstatic.exe")))
      {
        this.DisplayParsingError("File {0} not found. Make sure that aitstatic.exe is in the folder specified.", (object) Path.Combine(argumentValue, "aitstatic.exe"));
        return false;
      }
      this.api.PathSettings.AitStaticExeFolderPath = argumentValue;
      return true;
    }

    public bool ParseParams(List<string> args)
    {
      if (!args.Any<string>())
      {
        this.DisplayParsingError("Insufficient arguments. No binary folder or file path was supplied and no file path was given for the supported APIs file.");
        return false;
      }
      foreach (string str1 in args)
      {
        string arg = str1;
        if (arg.Contains("?"))
        {
          ArgumentParser.ShowUsage();
          return false;
        }
        if (!((IEnumerable<char>) ConstantsEXE.ParamStartChars).Any<char>((Func<char, bool>) (x => arg.StartsWith(x.ToString((IFormatProvider) CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))))
        {
          this.DisplayParsingError("Invalid parameter: {0}. \nParameter needs to start with one of the following characters: {1}", (object) arg, (object) new string(ConstantsEXE.ParamStartChars));
          return false;
        }
        string str2 = arg.TrimStart(ConstantsEXE.ParamStartChars);
        if (str2.StartsWith("DriverPackagePath", StringComparison.OrdinalIgnoreCase) || str2.StartsWith("BinaryPath", StringComparison.OrdinalIgnoreCase))
        {
          if (!this.ParseDriverPaths(str2))
          {
            this.DisplayParsingErrorBasic("Please correct Parameter: -{0} and rerun.\n", (object) str2);
            return false;
          }
        }
        else if (str2.StartsWith("SupportedApiXmlFiles", StringComparison.OrdinalIgnoreCase))
        {
          if (!this.ParseSupportedApiPaths(str2))
          {
            this.DisplayParsingErrorBasic("Please correct Parameter: -{0} and rerun.\n", (object) str2);
            return false;
          }
        }
        else if (str2.StartsWith("ModuleWhiteListXmlFiles", StringComparison.OrdinalIgnoreCase))
        {
          if (!this.ParseExceptionFilePaths(str2))
          {
            this.DisplayParsingErrorBasic("Please correct Parameter: -{0} and rerun.\n", (object) str2);
            return false;
          }
        }
        else if (str2.StartsWith("ExclusionList", StringComparison.OrdinalIgnoreCase))
        {
          if (!this.ParseExclusionFilePaths(str2))
          {
            this.DisplayParsingErrorBasic("Please correct Parameter: -{0} and rerun.\n", (object) str2);
            return false;
          }
        }
        else if (str2.StartsWith("BinaryExclusionListXmlFile", StringComparison.OrdinalIgnoreCase))
        {
          if (!this.ParseBinaryExceptionFilePath(str2))
          {
            this.DisplayParsingErrorBasic("Please correct Parameter: -{0} and rerun.\n", (object) str2);
            return false;
          }
        }
        else if (str2.StartsWith("TempOutputFolder", StringComparison.OrdinalIgnoreCase))
        {
          if (!this.ParseTempOutputFolder(str2))
            return false;
        }
        else if (str2.StartsWith("ApiExtractorExePath", StringComparison.OrdinalIgnoreCase))
        {
          if (!this.ParseApiExtractorExePath(str2))
            return false;
        }
        else if (str2.StartsWith("StrictCompliance", StringComparison.OrdinalIgnoreCase))
        {
          if (!this.StrictComplianceCheck(str2))
          {
            this.DisplayParsingErrorBasic("Please correct Parameter: -{0} and rerun.\n", (object) str2);
            return false;
          }
        }
        else
        {
          this.DisplayParsingError("Invalid Parameter: {0}", (object) arg);
          return false;
        }
      }
      if (this.api.PathSettings.DriverPaths.Count == 0)
      {
        this.DisplayParsingError("No binary folder or file path was supplied.");
        return false;
      }
      if (this.api.PathSettings.SupportedApisFilePaths.Count != 0)
        return true;
      this.DisplayParsingError("No file path was given for the supported APIs file.");
      return false;
    }
  }
}
