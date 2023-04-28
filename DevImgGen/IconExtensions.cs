// Decompiled with JetBrains decompiler
// Type: DevImgGen.IconExtensions
// Assembly: DevImgGen, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8B415242-7DF5-46C1-83D1-ACA4F55A74B9
// Assembly location: C:\Users\Admin\Desktop\re\dig\DevImgGen.exe

using System.Drawing;
using System.Reflection;

namespace DevImgGen
{
  public static class IconExtensions
  {
    public static Icon AsDisposableIcon(this Icon icon)
    {
      icon.GetType().GetField("ownHandle", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((object) icon, (object) true);
      return icon;
    }
  }
}
