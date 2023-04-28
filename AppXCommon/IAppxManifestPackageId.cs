// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxManifestPackageId
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("283ce2d7-7153-4a91-9649-7a0f7240945f")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxManifestPackageId
  {
    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetName();

    APPX_PACKAGE_ARCHITECTURE GetArchitecture();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetPublisher();

    ulong GetVersion();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetResourceId();

    bool ComparePublisher([MarshalAs(UnmanagedType.LPWStr), In] string otherPublisher);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetPackageFullName();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetPackageFamilyName();
  }
}
