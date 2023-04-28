// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxManifestQualifiedResource
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("3b53a497-3c5c-48d1-9ea3-bb7eac8cd7d4")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxManifestQualifiedResource
  {
    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetLanguage();

    uint GetScale();

    DX_FEATURE_LEVEL GetDXFeatureLevel();
  }
}
