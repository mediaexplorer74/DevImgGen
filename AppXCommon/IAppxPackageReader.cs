// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxPackageReader
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("b5c49650-99bc-481c-9a34-3d53a4106708")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxPackageReader
  {
    IAppxBlockMapReader GetBlockMap();

    IAppxFile GetFootprintFile([In] APPX_FOOTPRINT_FILE_TYPE type);

    IAppxFile GetPayloadFile([MarshalAs(UnmanagedType.LPWStr), In] string fileName);

    IAppxFilesEnumerator GetPayloadFiles();

    IAppxManifestReader GetManifest();
  }
}
