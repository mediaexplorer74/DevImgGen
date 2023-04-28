// Decompiled with JetBrains decompiler
// Type: DigLib.DriverStore.DriverInfo
// Assembly: DigLib, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8630F1AA-3914-41FE-A6B1-9C741E0FFE01
// Assembly location: C:\Users\Admin\Desktop\re\dig\DigLib.dll

using System;
using System.Runtime.InteropServices;

namespace DigLib.DriverStore
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct DriverInfo
  {
    public string ManufacturerName;
    public string HardwareDescription;
    public string HardwareId;
    public string CompatibleIds;
    public string ExcludeIds;
    public string LowerFilters;
    public string UpperFilters;
    public string ServiceName;
    public string SectionName;
    public IntPtr EnumContext;
    public long DriverDate;
    public DriverVersion DriverVersion;
    public uint FeatureScore;
    public uint Flags;
    public DriverOsVersion OsVersion;
    public uint ControlFlags;
    public uint LegacyFlags;
    public uint ExternalLegacyFlags;
  }
}
