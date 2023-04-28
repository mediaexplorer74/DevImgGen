// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxBundleManifestOptionalBundleInfo
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("515BF2E8-BCB0-4D69-8C48-E383147B6E12")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxBundleManifestOptionalBundleInfo
  {
    IAppxManifestPackageId GetPackageId();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetFileName();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    IAppxBundleManifestPackageInfoEnumerator GetPackageInfoItems();
  }
}
