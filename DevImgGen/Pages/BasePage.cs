// Decompiled with JetBrains decompiler
// Type: DevImgGen.Pages.BasePage
// Assembly: DevImgGen, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8B415242-7DF5-46C1-83D1-ACA4F55A74B9
// Assembly location: C:\Users\Admin\Desktop\re\dig\DevImgGen.exe

using System;
using System.Windows.Forms;

namespace DevImgGen.Pages
{
  public class BasePage : UserControl
  {
    protected virtual void OnPageChangeRequested(PageEnum e)
    {
      EventHandler<PageEnum> pageChangeRequested = this.PageChangeRequested;
      if (pageChangeRequested == null)
        return;
      pageChangeRequested((object) this, e);
    }

    public event EventHandler<PageEnum> PageChangeRequested;
  }
}
