// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.AppxUtils
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using Microsoft.Composition.ToolBox;
using Microsoft.Win32;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.ImagingTools.AppX
{
  public class AppxUtils
  {
    public const uint PACKAGE_FAMILY_NAME_MAX_LENGTH = 64;
    public const uint PACKAGE_FULL_NAME_MAX_LENGTH = 127;
    public const uint MAX_APPX_IN_BUNDLE = 10000;
    public static readonly string[] c_appxExtensions = new string[4]
    {
      ".appx",
      ".eappx",
      ".msix",
      ".emsix"
    };
    public static readonly string[] c_appxBundleExtensions = new string[4]
    {
      ".appxbundle",
      ".eappxbundle",
      ".msixbundle",
      ".emsixbundle"
    };
    public static readonly string[] c_notCompressExtensions = new string[10]
    {
      ".jpg",
      ".png",
      ".wma",
      ".m4a",
      ".mp3",
      ".mp4",
      ".zip",
      ".gif",
      ".cat",
      ".p7x"
    };

    [DllImport("kernel32.dll")]
    private static extern uint GetCompressedFileSizeW([MarshalAs(UnmanagedType.LPWStr), In] string lpFileName, [MarshalAs(UnmanagedType.U4)] out uint lpFileSizeHigh);

    [DllImport("UpdateDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    private static extern int CompressFile(string file);

    public static bool FilesAreEqual(FileInfo first, FileInfo second)
    {
      using (SHA256 shA256 = SHA256.Create())
        return CommonUtils.ByteArrayCompare(shA256.ComputeHash((Stream) first.OpenRead()), shA256.ComputeHash((Stream) second.OpenRead()));
    }

    public static void SaveSpace(IULogger logger, string targetFolder)
    {
      logger.LogInfo("AppXCommon: Saving Space in {0}...", new object[1]
      {
        (object) targetFolder
      });
      long num1 = 0;
      long num2 = 0;
      long num3 = 0;
      using (SHA256 shA256 = SHA256.Create())
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (string file in Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathDirectory.GetFiles(targetFolder, "*.*", SearchOption.AllDirectories))
        {
          long num4 = 0;
          using (FileStream inputStream = Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.OpenRead(file))
          {
            shA256.ComputeHash((Stream) inputStream);
            num4 = inputStream.Length;
          }
          bool compressed = false;
          long newFileSize = 0;
          AppxUtils.CompressFile(file, ref compressed, ref newFileSize);
          if (compressed)
          {
            ++num2;
            num3 += num4 - newFileSize;
          }
        }
      }
      logger.LogInfo("AppXCommon: Total {0} hard-links, {1} compressed files, saved {2} bytes...", new object[3]
      {
        (object) num1,
        (object) num2,
        (object) num3
      });
    }

    public static void CompressFile(string file, ref bool compressed, ref long newFileSize)
    {
      if (!AppxUtils.ShouldCompressFile(file))
      {
        compressed = false;
      }
      else
      {
        AppxUtils.CompressFile(file);
        compressed = true;
        using (Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.OpenRead(file))
        {
          uint lpFileSizeHigh;
          uint compressedFileSizeW = AppxUtils.GetCompressedFileSizeW(file, out lpFileSizeHigh);
          newFileSize = (long) lpFileSizeHigh << 32 | (long) compressedFileSizeW;
        }
      }
    }

    public static bool ShouldCompressFile(string file)
    {
      if (!file.Contains("."))
        return true;
      string extension = Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetExtension(file);
      foreach (string compressExtension in AppxUtils.c_notCompressExtensions)
      {
        if (extension.Equals(compressExtension, StringComparison.OrdinalIgnoreCase))
          return false;
      }
      return true;
    }

    public static IStream CreateFileStream(
      string fileName,
      STGM mode,
      uint attributes,
      bool create)
    {
      return NativeMethods.SHCreateStreamOnFileEx(fileName, mode, attributes, create, (IStream) null);
    }

    public static bool IsAppxBundleFile(string filename)
    {
      string extension = Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetExtension(filename);
      foreach (string appxBundleExtension in AppxUtils.c_appxBundleExtensions)
      {
        if (extension.Equals(appxBundleExtension, StringComparison.OrdinalIgnoreCase))
          return true;
      }
      return false;
    }

    public static bool IsAppxFile(string filename)
    {
      string extension = Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetExtension(filename);
      foreach (string cAppxExtension in AppxUtils.c_appxExtensions)
      {
        if (extension.Equals(cAppxExtension, StringComparison.OrdinalIgnoreCase))
          return true;
      }
      return false;
    }

    public static string GetPackageFamilyName(string packageFullName)
    {
      uint packageFamilyNameCount = 65;
      StringBuilder packageFamilyName = new StringBuilder((int) packageFamilyNameCount);
      int num = NativeMethods.PackageFamilyNameFromFullName(packageFullName, ref packageFamilyNameCount, packageFamilyName);
      if (num != 0)
        throw new Exception(string.Format("AppXUtils: Could not resolve PackageFamilyNameFullName from PackageName '{0}' : ERRORCODE: {1}", (object) packageFullName, (object) num));
      return packageFamilyName.ToString();
    }

    public static string GetPackageFamilyNameFromFile(string sourceAppx)
    {
      if (AppxUtils.IsAppxFile(sourceAppx))
      {
        using (AppXComObject<IStream> appXcomObject1 = new AppXComObject<IStream>(AppxUtils.CreateFileStream(sourceAppx, STGM.STGM_READ, 1U, false)))
        {
          using (AppXComObject<IAppxFactory> appXcomObject2 = new AppXComObject<IAppxFactory>((IAppxFactory) new AppxFactory()))
          {
            using (AppXComObject<IAppxPackageReader> appXcomObject3 = new AppXComObject<IAppxPackageReader>(appXcomObject2.Ref().CreatePackageReader(appXcomObject1.Ref())))
            {
              using (AppXComObject<IAppxManifestReader> appXcomObject4 = new AppXComObject<IAppxManifestReader>(appXcomObject3.Ref().GetManifest()))
              {
                using (AppXComObject<IAppxManifestPackageId> appXcomObject5 = new AppXComObject<IAppxManifestPackageId>(appXcomObject4.Ref().GetPackageId()))
                  return appXcomObject5.Ref().GetPackageFamilyName();
              }
            }
          }
        }
      }
      else
      {
        if (!AppxUtils.IsAppxBundleFile(sourceAppx))
          throw new Exception(string.Format("Failed to get AppX family name from '{0}' since its type is not appx or appxbundle.", (object) sourceAppx));
        using (AppXComObject<IStream> appXcomObject6 = new AppXComObject<IStream>(AppxUtils.CreateFileStream(sourceAppx, STGM.STGM_READ, 1U, false)))
        {
          using (AppXComObject<IAppxBundleFactory> appXcomObject7 = new AppXComObject<IAppxBundleFactory>((IAppxBundleFactory) new AppxBundleFactory()))
          {
            using (AppXComObject<IAppxBundleReader> appXcomObject8 = new AppXComObject<IAppxBundleReader>(appXcomObject7.Ref().CreateBundleReader(appXcomObject6.Ref())))
            {
              using (AppXComObject<IAppxBundleManifestReader> appXcomObject9 = new AppXComObject<IAppxBundleManifestReader>(appXcomObject8.Ref().GetManifest()))
              {
                using (AppXComObject<IAppxManifestPackageId> appXcomObject10 = new AppXComObject<IAppxManifestPackageId>(appXcomObject9.Ref().GetPackageId()))
                  return appXcomObject10.Ref().GetPackageFamilyName();
              }
            }
          }
        }
      }
    }

    public static PACKAGE_ID CreatePackageId(string name, string publisher)
    {
      PACKAGE_ID packageId;
      packageId.name = name;
      packageId.publisher = publisher;
      packageId.processorArchitecture = 11U;
      packageId.reserved = 0U;
      packageId.Major = (ushort) 0;
      packageId.Minor = (ushort) 0;
      packageId.Build = (ushort) 0;
      packageId.Revision = (ushort) 0;
      packageId.resourceId = "";
      packageId.publisherId = (string) null;
      return packageId;
    }

    public static string GetPackageFaimlyName(string name, string publisher)
    {
      PACKAGE_ID packageId = AppxUtils.CreatePackageId(name, publisher);
      uint packageFamilyNameLength = 65;
      StringBuilder packageFamilyName = new StringBuilder((int) packageFamilyNameLength);
      int error = NativeMethods.PackageFamilyNameFromId(ref packageId, ref packageFamilyNameLength, packageFamilyName);
      if (error != 0)
        throw new Win32Exception(error, "PackageFamilyNameFromId failed with " + name + " " + publisher);
      return packageFamilyName.ToString();
    }

    public static int RegLoadAppKey(string hiveFile)
    {
      int hKey = 0;
      int error = NativeMethods.RegLoadAppKey(hiveFile, out hKey, RegSAM.AllAccess, 1, 0);
      if (error != 0)
        throw new Win32Exception(error, "Failed during RegLoadAppKey of file " + hiveFile);
      return hKey;
    }

    public static string GetBundleApplicablePackages(
      string appxBundleManifest,
      string architectures,
      string langauges,
      string scales)
    {
      uint num = 1280000;
      StringBuilder packageFullNames = new StringBuilder((int) num);
      int applicablePackages = NativeMethods.GetBundleApplicablePackages(appxBundleManifest, architectures, langauges, scales, num, packageFullNames);
      if (applicablePackages != 0)
        throw new Win32Exception(applicablePackages, "Failed during GetBundleApplicablePackages from file " + appxBundleManifest);
      return packageFullNames.ToString();
    }

    public static Version GetPackageVersion(ulong version)
    {
      ushort[] numArray = new ushort[4];
      for (int index = 0; index < 4; ++index)
      {
        numArray[index] = (ushort) (version & (ulong) ushort.MaxValue);
        version >>= 16;
      }
      return new Version((int) numArray[3], (int) numArray[2], (int) numArray[0], (int) numArray[1]);
    }

    public static void AddApplication(
      AppxInfoManager appxInfo,
      AppxInfo package,
      RegistryKey regKey,
      string region = null)
    {
      using (RegistryKey subKey1 = regKey.CreateSubKey(string.Format("Applications\\{0}", (object) package.packageFullName)))
      {
        string preInstalledVolume = AppxInfoManager.GetManifestPathFromPreInstalledVolume(package);
        subKey1.SetValue("Path", (object) preInstalledVolume);
        if (!string.IsNullOrEmpty(region))
          subKey1.SetValue("Regions", (object) region);
        foreach (AppxInfo dependentFramework in appxInfo.GetDependentFrameworks(package))
        {
          using (RegistryKey subKey2 = subKey1.CreateSubKey(dependentFramework.packageFullName))
            subKey2.SetValue("Path", (object) AppxInfoManager.GetManifestPathFromPreInstalledVolume(dependentFramework));
        }
      }
    }

    public static void AddStaged(AppxInfoManager appxInfo, AppxInfo package, RegistryKey regKey)
    {
      string preInstalledVolume = AppxInfoManager.GetManifestPathFromPreInstalledVolume(package);
      using (RegistryKey subKey = regKey.CreateSubKey(string.Format("Staged\\{0}\\{1}", (object) package.packageFamilyName, (object) package.packageFullName)))
        subKey.SetValue("Path", (object) preInstalledVolume);
    }

    public static void AddSetupPhase(
      string packageFamilyName,
      SetupPhase setupPhase,
      RegistryKey regKey)
    {
      using (RegistryKey subKey = regKey.CreateSubKey(string.Format("Config\\{0}", (object) packageFamilyName)))
        subKey.SetValue("SetupPhase", (object) (uint) setupPhase, RegistryValueKind.DWord);
    }

    public static void CreateHardLink(string fileName, string existingFileName)
    {
      string str = fileName + ".del";
      Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.Move(fileName, str);
      if (!NativeMethods.CreateHardLink(fileName, existingFileName, IntPtr.Zero))
      {
        Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.Move(str, fileName);
        throw new Win32Exception("Failed to CreateHardLink from existing file " + existingFileName + " to new file " + fileName + " with error " + Marshal.GetLastWin32Error().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
      Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.Delete(str);
    }

    public static string GetApplicableArchitecturesString(CpuArch cpuType)
    {
      switch (cpuType)
      {
        case CpuArch.X86:
          return "x86";
        case CpuArch.ARM:
          return "arm";
        case CpuArch.ARM64:
          return "arm64,arm,x86a64";
        case CpuArch.AMD64:
          return "x64,x86";
        default:
          throw new NoSuchArgumentException("Invalid cpuType");
      }
    }

    public static List<APPX_PACKAGE_ARCHITECTURE2> GetApplicableArchitectures(
      CpuArch cpuType)
    {
      List<APPX_PACKAGE_ARCHITECTURE2> applicableArchitectures = new List<APPX_PACKAGE_ARCHITECTURE2>();
      applicableArchitectures.Add(APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_NEUTRAL);
      switch (cpuType)
      {
        case CpuArch.X86:
          applicableArchitectures.Add(APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_X86);
          break;
        case CpuArch.ARM:
          applicableArchitectures.Add(APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_ARM);
          break;
        case CpuArch.ARM64:
          applicableArchitectures.Add(APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_ARM64);
          applicableArchitectures.Add(APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_ARM);
          applicableArchitectures.Add(APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_X86_ON_ARM64);
          break;
        case CpuArch.AMD64:
          applicableArchitectures.Add(APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_X64);
          applicableArchitectures.Add(APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_X86);
          break;
        default:
          throw new NoSuchArgumentException("Invalid cpuType");
      }
      return applicableArchitectures;
    }

    public static APPX_PACKAGE_ARCHITECTURE2 GetApplicableArchitecturesId(
      string cpuType)
    {
      if (string.IsNullOrEmpty(cpuType))
        return APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_NEUTRAL;
      switch (cpuType.ToLower(CultureInfo.InvariantCulture))
      {
        case "x86":
          return APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_X86;
        case "amd64":
          return APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_X64;
        case "arm":
          return APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_ARM;
        case "arm64":
          return APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_ARM64;
        case "x86a64":
          return APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_X86_ON_ARM64;
        case "neutral":
          return APPX_PACKAGE_ARCHITECTURE2.APPX_PACKAGE_ARCHITECTURE2_NEUTRAL;
        default:
          throw new NoSuchArgumentException("Invalid cpuType");
      }
    }

    public static uint GetTotalUncompressedSizeOfBundle(IStream bundleStream)
    {
      uint uncompressedSizeOfBundle = 0;
      using (AppXComObject<IAppxBundleFactory> appXcomObject1 = new AppXComObject<IAppxBundleFactory>((IAppxBundleFactory) new AppxBundleFactory()))
      {
        using (AppXComObject<IAppxBundleReader> appXcomObject2 = new AppXComObject<IAppxBundleReader>(appXcomObject1.Ref().CreateBundleReader(bundleStream)))
        {
          using (AppXComObject<IAppxFilesEnumerator> appXcomObject3 = new AppXComObject<IAppxFilesEnumerator>(appXcomObject2.Ref().GetPayloadPackages()))
          {
            while (appXcomObject3.Ref().GetHasCurrent())
            {
              using (AppXComObject<IAppxFile> appXcomObject4 = new AppXComObject<IAppxFile>(appXcomObject3.Ref().GetCurrent()))
              {
                using (AppXComObject<IStream> appXcomObject5 = new AppXComObject<IStream>(appXcomObject4.Ref().GetStream()))
                  uncompressedSizeOfBundle += AppxUtils.GetTotalUncompressedSizeOfPackage(appXcomObject5.Ref());
              }
              appXcomObject3.Ref().MoveNext();
            }
          }
        }
      }
      return uncompressedSizeOfBundle;
    }

    public static uint GetTotalUncompressedSizeOfPackage(IStream fileStream)
    {
      uint uncompressedSizeOfPackage = 0;
      using (AppXComObject<IAppxFactory> appXcomObject1 = new AppXComObject<IAppxFactory>((IAppxFactory) new AppxFactory()))
      {
        using (AppXComObject<IAppxPackageReader> appXcomObject2 = new AppXComObject<IAppxPackageReader>(appXcomObject1.Ref().CreatePackageReader(fileStream)))
        {
          using (AppXComObject<IAppxBlockMapReader> appXcomObject3 = new AppXComObject<IAppxBlockMapReader>(appXcomObject2.Ref().GetBlockMap()))
          {
            using (AppXComObject<IAppxBlockMapFilesEnumerator> appXcomObject4 = new AppXComObject<IAppxBlockMapFilesEnumerator>(appXcomObject3.Ref().GetFiles()))
            {
              while (appXcomObject4.Ref().GetHasCurrent())
              {
                using (AppXComObject<IAppxBlockMapFile> appXcomObject5 = new AppXComObject<IAppxBlockMapFile>(appXcomObject4.Ref().GetCurrent()))
                  uncompressedSizeOfPackage += (uint) appXcomObject5.Ref().GetUncompressedSize();
                appXcomObject4.Ref().MoveNext();
              }
            }
          }
        }
      }
      return uncompressedSizeOfPackage;
    }
  }
}
