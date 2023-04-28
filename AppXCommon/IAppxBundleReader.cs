// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxBundleReader
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("DD75B8C0-BA76-43B0-AE0F-68656A1DC5C8")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxBundleReader
  {
    IAppxFile GetFootprintFile([In] APPX_BUNDLE_FOOTPRINT_FILE_TYPE fileType);

    IAppxBlockMapReader GetBlockMap();

    IAppxBundleManifestReader GetManifest();

    IAppxFilesEnumerator GetPayloadPackages();

    IAppxFile GetPayloadPackage([MarshalAs(UnmanagedType.LPWStr), In] string fileName);
  }
}
