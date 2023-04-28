// Decompiled with JetBrains decompiler
// Type: Microsoft.UniversalStore.HardwareWorkflow.Inf2Cat.Inf2CatException
// Assembly: Inf2Cat, Version=3.3.0.0, Culture=neutral, PublicKeyToken=8fdc415ae438dfeb
// MVID: 52190100-9076-4EF7-B45F-77CDBE3D58DE
// Assembly location: C:\Users\Admin\Desktop\re\dig\Inf2Cat.exe

using System;

namespace Microsoft.UniversalStore.HardwareWorkflow.Inf2Cat
{
  public class Inf2CatException : Exception
  {
    public Inf2CatException(string message)
      : base(message)
    {
    }

    public Inf2CatException(string message, Exception ex)
      : base(message, ex)
    {
    }
  }
}
