// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.PACKAGE_ID
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using System.Runtime.InteropServices;

namespace Microsoft.ImagingTools.AppX
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct PACKAGE_ID
  {
    public uint reserved;
    public uint processorArchitecture;
    public ushort Revision;
    public ushort Build;
    public ushort Minor;
    public ushort Major;
    public string name;
    public string publisher;
    public string resourceId;
    public string publisherId;
  }
}
