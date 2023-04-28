// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxManifestPackageDependency
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("e4946b59-733e-43f0-a724-3bde4c1285a0")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxManifestPackageDependency
  {
    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetName();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetPublisher();

    ulong GetMinVersion();
  }
}
