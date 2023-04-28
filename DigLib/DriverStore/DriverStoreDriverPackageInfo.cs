// Decompiled with JetBrains decompiler
// Type: DigLib.DriverStore.DriverStoreDriverPackageInfo
// Assembly: DigLib, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8630F1AA-3914-41FE-A6B1-9C741E0FFE01
// Assembly location: C:\Users\Admin\Desktop\re\dig\DigLib.dll

using System.Runtime.InteropServices;

namespace DigLib.DriverStore
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct DriverStoreDriverPackageInfo
  {
    public ushort ProcessorArchitecture;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 85)]
    public string LocaleName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string PublishedInfName;
    public uint Flags;
  }
}
