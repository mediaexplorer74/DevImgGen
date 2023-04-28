// Decompiled with JetBrains decompiler
// Type: DigLib.DriverStore.DriverPackageOpenFlags
// Assembly: DigLib, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8630F1AA-3914-41FE-A6B1-9C741E0FFE01
// Assembly location: C:\Users\Admin\Desktop\re\dig\DigLib.dll

using System;

namespace DigLib.DriverStore
{
  [Flags]
  public enum DriverPackageOpenFlags
  {
    None = 0,
    VersionOnly = 1,
    FilesOnly = 2,
    DefaultLanguage = 4,
    LocalizableStrings = 8,
    TargetOSVersion = 16, // 0x00000010
    StrictValidation = 32, // 0x00000020
    ClassSchemaOnly = 64, // 0x00000040
    LogTelemetry = 128, // 0x00000080
    PrimaryOnly = 256, // 0x00000100
  }
}
