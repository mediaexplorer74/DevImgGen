// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxManifestMainPackageDependency
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("05D0611C-BC29-46D5-97E2-84B9C79BD8AE")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxManifestMainPackageDependency
  {
    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetName();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetPublisher();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetPackageFamilyName();
  }
}
