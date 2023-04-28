// Decompiled with JetBrains decompiler
// Type: DigLib.DriverCandidate
// Assembly: DigLib, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8630F1AA-3914-41FE-A6B1-9C741E0FFE01
// Assembly location: C:\Users\Admin\Desktop\re\dig\DigLib.dll

using System;
using System.Collections.Generic;

namespace DigLib
{
  internal class DriverCandidate
  {
    internal string InfPath;
    internal Version Version;
    internal IntPtr PackageHandle;
    internal List<string> HwIds;

    internal DriverCandidate(
      string infPath,
      Version version,
      IntPtr packageHandle,
      List<string> hwids)
    {
      this.InfPath = infPath;
      this.Version = version;
      this.PackageHandle = packageHandle;
      if (hwids == null)
        return;
      this.HwIds = new List<string>();
      this.HwIds.AddRange((IEnumerable<string>) hwids);
    }
  }
}
