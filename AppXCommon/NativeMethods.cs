// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.NativeMethods
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Microsoft.ImagingTools.AppX
{
  public class NativeMethods
  {
    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
    public static extern IStream SHCreateStreamOnFileEx(
      [In] string fileName,
      [In] STGM mode,
      [In] uint attributes,
      [In] bool create,
      [In] IStream template);

    [DllImport("kernel32.dll")]
    public static extern int PackageFamilyNameFromFullName(
      [MarshalAs(UnmanagedType.LPWStr)] string packageFullName,
      ref uint packageFamilyNameCount,
      [MarshalAs(UnmanagedType.LPWStr)] StringBuilder packageFamilyName);

    [DllImport("Kernel32.dll")]
    public static extern int PackageFamilyNameFromId(
      ref PACKAGE_ID packageId,
      ref uint packageFamilyNameLength,
      [MarshalAs(UnmanagedType.LPWStr)] StringBuilder packageFamilyName);

    [DllImport("AppXDeploymentClient.dll")]
    public static extern int GetBundleApplicablePackages(
      [MarshalAs(UnmanagedType.LPWStr)] string appxBundleManifest,
      [MarshalAs(UnmanagedType.LPWStr)] string architectures,
      [MarshalAs(UnmanagedType.LPWStr)] string langauges,
      [MarshalAs(UnmanagedType.LPWStr)] string scales,
      uint packageFullNamesLength,
      [MarshalAs(UnmanagedType.LPWStr)] StringBuilder packageFullNames);

    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern int RegLoadAppKey(
      string hiveFile,
      out int hKey,
      RegSAM samDesired,
      int options,
      int reserved);

    [DllImport("kernel32.dll")]
    public static extern bool CreateSymbolicLink(
      string lpSymlinkFileName,
      string lpTargetFileName,
      SymbolicLink dwFlags);

    [DllImport("Kernel32.dll", SetLastError = true)]
    public static extern bool CreateHardLink(
      string lpFileName,
      string lpExistingFileName,
      IntPtr lpSecurityAttributes);

    [DllImport("Kernel32.dll")]
    public static extern bool MoveFile(string lpExistingFileName, string lpNewFileName);
  }
}
