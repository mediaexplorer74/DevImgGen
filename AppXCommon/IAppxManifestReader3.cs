// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxManifestReader3
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("C43825AB-69B7-400A-9709-CC37F5A72D24")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxManifestReader3
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

    IAppxManifestCapabilitiesEnumerator GetCapabilitiesByCapabilityClass(
      APPX_CAPABILITY_CLASS_TYPE capabilityClass);

    IAppxManifestTargetDeviceFamiliesEnumerator GetTargetDeviceFamilies();
  }
}
