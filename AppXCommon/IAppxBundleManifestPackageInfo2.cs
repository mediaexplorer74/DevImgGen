// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxBundleManifestPackageInfo2
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("44C2ACBC-B2CF-4CCB-BBDB-9C6DA8C3BC9E")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public interface IAppxBundleManifestPackageInfo2 : IAppxBundleManifestPackageInfo
  {
    bool GetIsPackageReference();

    bool GetIsNonQualifiedResourcePackage();

    bool GetIsDefaultApplicablePackage();
  }
}
