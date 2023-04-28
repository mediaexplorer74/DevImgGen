// Decompiled with JetBrains decompiler
// Type: DigLib.DriverOverlap
// Assembly: DigLib, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8630F1AA-3914-41FE-A6B1-9C741E0FFE01
// Assembly location: C:\Users\Admin\Desktop\re\dig\DigLib.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace DigLib
{
  public class DriverOverlap
  {
    private ManualResetEvent m_WaitEvent;

    public string Name { get; private set; }

    public Version Version { get; private set; }

    public List<string> PreviousHwIDs { get; private set; }

    public List<string> NewHwIDs { get; private set; }

    internal bool Replace { get; private set; }

    public DriverOverlap(
      string name,
      Version version,
      List<string> previousHwIDs,
      List<string> newHwIDs,
      ManualResetEvent waitEvent)
    {
      this.Name = name;
      this.Version = version;
      this.PreviousHwIDs = previousHwIDs;
      this.NewHwIDs = newHwIDs;
      this.m_WaitEvent = waitEvent;
    }

    public void Approve()
    {
      this.Replace = true;
      this.m_WaitEvent.Set();
    }

    public void Deny()
    {
      this.Replace = false;
      this.m_WaitEvent.Set();
    }
  }
}
