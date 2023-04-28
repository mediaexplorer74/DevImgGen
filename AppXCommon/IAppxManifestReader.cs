// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.IAppxManifestReader
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.ImagingTools.AppX
{
  [Guid("4e1bd148-55a0-4480-a3d1-15544710637c")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAppxManifestReader
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
  }
}
