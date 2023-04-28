// Decompiled with JetBrains decompiler
// Type: Microsoft.UniversalStore.HardwareWorkflow.Inf2Cat.Constants
// Assembly: Inf2Cat, Version=3.3.0.0, Culture=neutral, PublicKeyToken=8fdc415ae438dfeb
// MVID: 52190100-9076-4EF7-B45F-77CDBE3D58DE
// Assembly location: C:\Users\Admin\Desktop\re\dig\Inf2Cat.exe

using Microsoft.UniversalStore.HardwareWorkflow.Catalogs;

namespace Microsoft.UniversalStore.HardwareWorkflow.Inf2Cat
{
  internal static class Constants
  {
    internal const int ReturnSuccess = 0;
    internal const int ReturnFailure = -1;
    internal const int ReturnSignabilityErrors = -2;
    internal const string BadParameter = "Parameter format not correct.";
    internal const string BadOSParameter = "Operating systems parameter invalid.";
    internal const string ParamUseLocalTime = "/USELOCALTIME";
    internal const string ParamOS = "/OS:";
    internal const string ParamNoCatalog = "/NOCAT";
    internal const string ParamVerbose = "/VERBOSE";
    internal const string ParamVerboseShort = "/V";
    internal const string ParamDriver = "/DRIVER:";
    internal const string ParamDriverShort = "/DRV:";
    internal const string ParamDrm = "/DRM";
    internal const string ParamPE = "/PE";
    internal const string ParamPageHashes = "/PAGEHASHES";
    internal const string ParamIgnoreErrors = "/IGNOREERRORS";
    internal const string ParamHeaderAttribute = "/HEADERATTRIBUTE";
    internal const string ParamHeaderAttributeShort = "/HA";
    internal const string ParamFileAttribute = "/FILEATTRIBUTE";
    internal const string ParamFileAttributeShort = "/FA";
    internal const string ParamUsage1 = "/?";
    internal const string ParamUsage2 = "-?";
    internal const string DrmLevelAttributeName = "DRMLevel";
    internal const string DrmLevelAttributeValuePreWin7orR2 = "1200";
    internal const string DrmLevelAttributeValuePostWin7orR2 = "1300";
    internal static readonly CatalogAttributeInfo PEGlobalAttribute = new CatalogAttributeInfo("PE", "TRUSTED", 65537);
    internal static readonly CatalogAttributeInfo PEFileAttribute = new CatalogAttributeInfo("PETrusted", "1");
    internal static readonly CatalogAttributeInfo PageHashesAttribute = new CatalogAttributeInfo("PageHashes", "true");
  }
}
