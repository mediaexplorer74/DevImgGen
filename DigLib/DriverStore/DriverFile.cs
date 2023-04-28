// Decompiled with JetBrains decompiler
// Type: DigLib.DriverStore.DriverFile
// Assembly: DigLib, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8630F1AA-3914-41FE-A6B1-9C741E0FFE01
// Assembly location: C:\Users\Admin\Desktop\re\dig\DigLib.dll

using System.Runtime.InteropServices;

namespace DigLib.DriverStore
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct DriverFile
  {
    public DriverFileOperation Operation;
    public string ExternalFile;
    public DriverFileType Type;
    public uint Flags;
    public string SourceFile;
    public string SourcePath;
    public string DestinationFile;
    public string DestinationPath;
    public string ArchiveFile;
    public string SecurityDescriptor;
    public string SectionName;
  }
}
