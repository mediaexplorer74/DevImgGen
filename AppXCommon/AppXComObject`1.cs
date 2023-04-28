// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.AppXComObject`1
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  public class AppXComObject<T> : IDisposable
  {
    private T _comObject;

    public AppXComObject(T comObject) => this._comObject = comObject;

    public ref T Ref() => ref this._comObject;

    public void Dispose()
    {
      if ((object) this._comObject != null)
        Marshal.ReleaseComObject((object) this._comObject);
      GC.SuppressFinalize((object) this);
    }
  }
}
