// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.SetupPhase
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

namespace Microsoft.ImagingTools.AppX
{
  public enum SetupPhase : uint
  {
    SetupPhase_None = 0,
    SetupPhase_PreOOBE = 1,
    SetupPhase_PreShell = 2,
    SetupPhase_PostShell = 4,
    SetupPhase_PreviewTiles = 8,
    SetupPhase_PostOOBE = 16, // 0x00000010
    SetupPhase_PreRoaming = 32, // 0x00000020
    SetupPhase_OnDemand = 64, // 0x00000040
    SetupPhase_Default = 80, // 0x00000050
    SetupPhase_FullyRegisterSharedApps = 128, // 0x00000080
    SetupPhase_All = 4294967295, // 0xFFFFFFFF
  }
}
