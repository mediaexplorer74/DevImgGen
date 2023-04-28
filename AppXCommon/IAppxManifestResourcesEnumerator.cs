// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxManifestResourcesEnumerator
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("de4dfbbd-881a-48bb-858c-d6f2baeae6ed")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxManifestResourcesEnumerator
  {
    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetCurrent();

    bool GetHasCurrent();

    bool MoveNext();
  }
}
