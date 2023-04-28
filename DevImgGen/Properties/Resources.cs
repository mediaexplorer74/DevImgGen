// Decompiled with JetBrains decompiler
// Type: DevImgGen.Properties.Resources
// Assembly: DevImgGen, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8B415242-7DF5-46C1-83D1-ACA4F55A74B9
// Assembly location: C:\Users\Admin\Desktop\re\dig\DevImgGen.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace DevImgGen.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (DevImgGen.Properties.Resources.resourceMan == null)
          DevImgGen.Properties.Resources.resourceMan = new ResourceManager("DevImgGen.Properties.Resources", typeof (DevImgGen.Properties.Resources).Assembly);
        return DevImgGen.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => DevImgGen.Properties.Resources.resourceCulture;
      set => DevImgGen.Properties.Resources.resourceCulture = value;
    }

    internal static Icon Icon => (Icon) DevImgGen.Properties.Resources.ResourceManager.GetObject(nameof (Icon), DevImgGen.Properties.Resources.resourceCulture);
  }
}
