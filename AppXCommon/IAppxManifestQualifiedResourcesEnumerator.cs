// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxManifestQualifiedResourcesEnumerator
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("8ef6adfe-3762-4a8f-9373-2fc5d444c8d2")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxManifestQualifiedResourcesEnumerator
  {
    IAppxManifestQualifiedResource GetCurrent();

    bool GetHasCurrent();

    bool MoveNext();
  }
}
