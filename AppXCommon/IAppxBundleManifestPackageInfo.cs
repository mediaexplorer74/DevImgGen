// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxBundleManifestPackageInfo
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("54CD06C1-268F-40BB-8ED2-757A9EBAEC8D")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public interface IAppxBundleManifestPackageInfo
  {
    APPX_BUNDLE_PAYLOAD_PACKAGE_TYPE GetPackageType();

    IAppxManifestPackageId GetPackageId();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetFileName();

    ulong GetOffset();

    ulong GetSize();

    IAppxManifestQualifiedResourcesEnumerator GetResources();
  }
}
