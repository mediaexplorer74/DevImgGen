// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxFile
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("91df827b-94fd-468f-827b-57f41b2f6f2e")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxFile
  {
    APPX_COMPRESSION_OPTION GetCompressionOption();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetContentType();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetName();

    ulong GetSize();

    IStream GetStream();
  }
}
