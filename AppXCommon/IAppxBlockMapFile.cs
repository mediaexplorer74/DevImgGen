// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxBlockMapFile
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("277672ac-4f63-42c1-8abc-beae3600eb59")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxBlockMapFile
  {
    IAppxBlockMapBlocksEnumerator GetBlocks();

    uint GetLocalFileHeaderSize();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetName();

    ulong GetUncompressedSize();

    bool ValidateFileHash([In] IStream fileStream);
  }
}
