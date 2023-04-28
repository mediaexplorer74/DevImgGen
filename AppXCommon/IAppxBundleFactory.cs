// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxBundleFactory
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("BBA65864-965F-4A5F-855F-F074BDBF3A7B")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxBundleFactory
  {
    IAppxBundleWriter CreateBundleWriter([In] IStream outputStream, [In] ulong bundleVersion);

    IAppxBundleReader CreateBundleReader([In] IStream inputStream);

    IAppxBundleManifestReader CreateBundleManifestReader(
      [In] IStream inputStream);
  }
}
