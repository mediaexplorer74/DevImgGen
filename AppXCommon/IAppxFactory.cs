// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxFactory
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("beb94909-e451-438b-b5a7-d79e767b75d8")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxFactory
  {
    IAppxPackageWriter CreatePackageWriter(
      [In] IStream outputStream,
      [In] APPX_PACKAGE_SETTINGS settings);

    IAppxPackageReader CreatePackageReader([In] IStream inputStream);

    IAppxManifestReader CreateManifestReader([In] IStream inputStream);

    IAppxBlockMapReader CreateBlockMapReader([In] IStream inputStream);

    IAppxBlockMapReader CreateValidatedBlockMapReader(
      [In] IStream blockMapStream,
      [In] string signatureFileName);
  }
}
