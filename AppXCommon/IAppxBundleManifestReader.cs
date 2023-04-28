// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxBundleManifestReader
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("CF0EBBC1-CC99-4106-91EB-E67462E04FB0")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxBundleManifestReader
  {
    IAppxManifestPackageId GetPackageId();

    IAppxBundleManifestPackageInfoEnumerator GetPackageInfoItems();

    IStream GetStream();
  }
}
