// Decompiled with JetBrains decompiler
// Type: DevImgGen.Controls.CommandButton
// Assembly: DevImgGen, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8B415242-7DF5-46C1-83D1-ACA4F55A74B9
// Assembly location: C:\Users\Admin\Desktop\re\dig\DevImgGen.exe

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevImgGen.Controls
{
  public class CommandButton : Button
  {
    private string m_Note;

    public CommandButton() => this.FlatStyle = FlatStyle.System;

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams createParams = base.CreateParams;
        createParams.Style |= 14;
        return createParams;
      }
    }

    public string Note
    {
      get => this.m_Note;
      set
      {
        if (!(this.m_Note != value))
          return;
        this.m_Note = value;
        CommandButton.SendMessage(this.Handle, 5641, IntPtr.Zero, this.m_Note);
      }
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(
      IntPtr hWnd,
      int msg,
      IntPtr wParam,
      [MarshalAs(UnmanagedType.LPWStr)] string lParam);
  }
}
