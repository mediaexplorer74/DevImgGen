// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxManifestReader2
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("d06f67bc-b31d-4eba-a8af-638e73e77b4d")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxManifestReader2
  {
    IAppxManifestPackageId GetPackageId();

    IAppxManifestProperties GetProperties();

    IAppxManifestPackageDependenciesEnumerator GetPackageDependencies();

    APPX_CAPABILITIES GetCapabilities();

    IAppxManifestResourcesEnumerator GetResources();

    IAppxManifestDeviceCapabilitiesEnumerator GetDeviceCapabilities();

    ulong GetPrerequisite([MarshalAs(UnmanagedType.LPWStr), In] string name);

    IAppxManifestApplicationsEnumerator GetApplications();

    IStream GetStream();

    IAppxManifestQualifiedResourcesEnumerator GetQualifiedResources();
  }
}
