// Decompiled with JetBrains decompiler
// Type: DigLib.Utils
// Assembly: DigLib, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8630F1AA-3914-41FE-A6B1-9C741E0FFE01
// Assembly location: C:\Users\Admin\Desktop\re\dig\DigLib.dll

using Microsoft.Composition.Packaging;
using System;
using System.Diagnostics;
using System.IO;

namespace DigLib
{
  public static class Utils
  {
    internal static string[] RuntimePaths = new string[9]
    {
      "$(runtime.drivers)",
      "$(runtime.system32)",
      "$(runtime.sysWow64)",
      "$(runtime.system)",
      "$(runtime.inf)",
      "$(runtime.windows)",
      "$(runtime.programFiles)",
      "$(runtime.programFilesX86)",
      "$(runtime.bootDrive)"
    };
    internal static string[] ExpandedPaths = new string[9]
    {
      "\\device\\bootdevice\\windows\\system32\\drivers",
      "\\device\\bootdevice\\windows\\system32",
      "\\device\\bootdevice\\windows\\syswow64",
      "\\device\\bootdevice\\windows\\system",
      "\\device\\bootdevice\\windows\\inf",
      "\\device\\bootdevice\\windows",
      "\\device\\bootdevice\\program files",
      "\\device\\bootdevice\\program files (x86)",
      "\\device\\bootdevice"
    };

    public static void CmdDirectoryDelete(string dirPath)
    {
      using (Process process = new Process())
      {
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = "/c rd \"" + dirPath + "\" /s /q";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
        process.WaitForExit();
      }
    }

    public static bool ProgramExistsInPath(string programName)
    {
      if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), programName)))
        return true;
      string environmentVariable = Environment.GetEnvironmentVariable("PATH");
      char[] separator = new char[1]{ ';' };
      foreach (string path1 in environmentVariable.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        if (File.Exists(Path.Combine(path1, programName)))
          return true;
      }
      return false;
    }

    public static void RecursiveCopyDirectory(string source, string target)
    {
      string fullPath = Path.GetFullPath(source);
      foreach (string fileSystemEntry in Directory.GetFileSystemEntries(fullPath, "*", SearchOption.AllDirectories))
      {
        if (!Directory.Exists(fileSystemEntry))
        {
          string str = Path.Combine(target, fileSystemEntry.Substring(fullPath.Length + 1));
          Directory.CreateDirectory(Path.GetDirectoryName(str));
          File.Copy(fileSystemEntry, str, true);
        }
      }
    }

    internal static string AliasPath(string path)
    {
      string lowerInvariant = path.ToLowerInvariant();
      for (int index = 0; index < Utils.RuntimePaths.Length; ++index)
      {
        string expandedPath = Utils.ExpandedPaths[index];
        if (lowerInvariant.StartsWith(expandedPath))
          return lowerInvariant.Length == expandedPath.Length ? (string) null : Utils.RuntimePaths[index] + path.Substring(expandedPath.Length);
      }
      return path;
    }

    internal static void CreateInfCatalog(string packagePath, bool treatAsArm64)
    {
      using (Process process = new Process())
      {
        process.StartInfo.FileName = "inf2cat.exe";
        process.StartInfo.Arguments = "/driver:\"" + packagePath + "\" /os:10_VB_" + (treatAsArm64 ? "ARM64" : "X64");
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
        process.WaitForExit();
      }
    }

    internal static void TestSign(string path)
    {
      using (Process process = new Process())
      {
        process.StartInfo.FileName = "signtool.exe";
        process.StartInfo.Arguments = "sign /f certificates\\OEM_Test_Cert_2017.pfx /fd SHA256 /t http://timestamp.digicert.com \"" + path + "\"";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
        process.WaitForExit();
      }
    }

    internal static string CreateAssemblyID(CbsPackage cbs)
    {
      string str1 = cbs.PackageName.EndsWith("-Package") ? cbs.PackageName.Substring(0, cbs.PackageName.Length - 8) : cbs.PackageName;
      string lowerInvariant1 = str1.Replace(" ", "").Replace("(", "").Replace(")", "").ToLowerInvariant();
      string lowerInvariant2 = cbs.HostArch.ToString().ToLowerInvariant();
      string str2 = lowerInvariant2 + "_";
      string str3 = lowerInvariant1.Length <= 40 ? str2 + lowerInvariant1 : str2 + lowerInvariant1.Substring(0, 19) + ".." + lowerInvariant1.Substring(lowerInvariant1.Length - 19, 19);
      string str4 = cbs.Version.ToString();
      string str5 = str3 + "_" + cbs.PublicKey + "_" + str4 + "_";
      string str6 = string.IsNullOrEmpty(cbs.Culture) || cbs.Culture.ToLowerInvariant() == "neutral" ? "none" : cbs.Culture;
      return str5 + str6 + "_" + Utils.SxsHash(new string[8]
      {
        "name",
        "culture",
        "typename",
        "type",
        "version",
        "publickeytoken",
        "processorarchitecture",
        "versionscope"
      }, new string[8]
      {
        str1,
        str6,
        "none",
        "none",
        str4,
        cbs.PublicKey,
        lowerInvariant2,
        "nonsxs"
      }).ToString("x16");
    }

    internal static ulong SxsHash(string[] attribs, string[] values)
    {
      ulong num1 = 0;
      for (int index = 0; index < values.Length; ++index)
      {
        if (!(values[index] == "none"))
        {
          values[index] = values[index].ToLower();
          ulong num2 = Utils.HashString(attribs[index]);
          num1 = (ulong) ((long) Utils.HashString(values[index]) + 8589934583L * (long) num2 + 8589934583L * (long) num1);
        }
      }
      return num1;
    }

    private static uint HashChar(uint hash, byte val) => hash * 65599U + (uint) val;

    private static ulong HashString(string str)
    {
      ulong[] numArray = new ulong[4];
      for (int index = 0; index < str.Length; ++index)
        numArray[index % 4] = (ulong) Utils.HashChar((uint) numArray[index % 4], (byte) str[index]);
      return (ulong) ((long) numArray[0] * 2087354105127L + (long) numArray[1] * -154618822575L + (long) numArray[2] * 8589934583L) + numArray[3];
    }
  }
}
