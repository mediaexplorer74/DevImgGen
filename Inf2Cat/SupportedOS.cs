// Decompiled with JetBrains decompiler
// Type: Microsoft.UniversalStore.HardwareWorkflow.Inf2Cat.SupportedOS
// Assembly: Inf2Cat, Version=3.3.0.0, Culture=neutral, PublicKeyToken=8fdc415ae438dfeb
// MVID: 52190100-9076-4EF7-B45F-77CDBE3D58DE
// Assembly location: C:\Users\Admin\Desktop\re\dig\Inf2Cat.exe

using Microsoft.UniversalStore.HardwareWorkflow.SubmissionBuilder;

namespace Microsoft.UniversalStore.HardwareWorkflow.Inf2Cat
{
  internal sealed class SupportedOS
  {
    internal string OsString;
    internal OperatingSystems OsEnum;
    internal bool OsBool;

    internal SupportedOS(string osString, OperatingSystems osEnum, bool osBool)
    {
      this.OsString = osString;
      this.OsEnum = osEnum;
      this.OsBool = osBool;
    }
  }
}
