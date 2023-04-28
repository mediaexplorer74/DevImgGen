// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxBundleManifestPackageInfo4
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("5DA6F13D-A8A7-4532-857C-1393D659371D")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public interface IAppxBundleManifestPackageInfo4 : 
    IAppxBundleManifestPackageInfo3,
    IAppxBundleManifestPackageInfo2,
    IAppxBundleManifestPackageInfo
  {
    bool GetIsStub();
  }
}
