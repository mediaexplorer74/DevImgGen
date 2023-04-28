// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxBlockMapFilesEnumerator
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("02b856a2-4262-4070-bacb-1a8cbbc42305")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxBlockMapFilesEnumerator
  {
    IAppxBlockMapFile GetCurrent();

    bool GetHasCurrent();

    bool MoveNext();
  }
}
