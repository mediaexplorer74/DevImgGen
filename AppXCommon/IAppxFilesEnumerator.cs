// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxFilesEnumerator
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("f007eeaf-9831-411c-9847-917cdc62d1fe")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxFilesEnumerator
  {
    IAppxFile GetCurrent();

    bool GetHasCurrent();

    bool MoveNext();
  }
}
