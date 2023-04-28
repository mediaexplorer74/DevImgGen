// Decompiled with JetBrains decompiler
// Type: DigLib.DriverStore.DriverPackageEnumFilesFlags
// Assembly: DigLib, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8630F1AA-3914-41FE-A6B1-9C741E0FFE01
// Assembly location: C:\Users\Admin\Desktop\re\dig\DigLib.dll

using System;

namespace DigLib.DriverStore
{
  [Flags]
  public enum DriverPackageEnumFilesFlags
  {
    None = 0,
    Copy = 1,
    Delete = 2,
    Rename = 4,
    Inf = 16, // 0x00000010
    Catalog = 32, // 0x00000020
    Binaries = 64, // 0x00000040
    CopyInfs = 128, // 0x00000080
    IncludeInfs = 256, // 0x00000100
    External = 4096, // 0x00001000
    UniqueSource = 8192, // 0x00002000
    UniqueDestination = 16384, // 0x00004000
  }
}
