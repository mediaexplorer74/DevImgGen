// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxBundleManifestPackageInfo3
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("6BA74B98-BB74-4296-80D0-5F4256A99675")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public interface IAppxBundleManifestPackageInfo3 : 
    IAppxBundleManifestPackageInfo2,
    IAppxBundleManifestPackageInfo
  {
    IAppxManifestTargetDeviceFamiliesEnumerator GetTargetDeviceFamilies();
  }
}
