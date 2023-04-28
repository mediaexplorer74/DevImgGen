// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxBlockMapReader
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("5efec991-bca3-42d1-9ec2-e92d609ec22a")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxBlockMapReader
  {
    IAppxBlockMapFile GetFile([MarshalAs(UnmanagedType.LPWStr), In] string filename);

    IAppxBlockMapFilesEnumerator GetFiles();

    IUri GetHashMethod();

    IStream GetStream();
  }
}
