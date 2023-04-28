// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxManifestCapabilitiesEnumerator
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("11D22258-F470-42C1-B291-8361C5437E41")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxManifestCapabilitiesEnumerator
  {
    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetCurrent();

    bool GetHasCurrent();

    bool MoveNext();
  }
}
