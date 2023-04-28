// Decompiled with JetBrains decompiler
// Type: DevImgGen.Controls.BackButton
// Assembly: DevImgGen, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8B415242-7DF5-46C1-83D1-ACA4F55A74B9
// Assembly location: C:\Users\Admin\Desktop\re\dig\DevImgGen.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevImgGen.Controls
{
  public class BackButton : Button
  {
    private Color UselessColor = Color.FromArgb(191, 191, 191);
    private Font GlyphFont = new Font("Segoe MDL2 Assets", 11.25f);
    private const string BackChromeGlyph = "\uE830";
    private BackButton.MouseState m_MouseState;
    private bool m_Useless;

    public BackButton()
    {
      this.FlatAppearance.BorderSize = 0;
      this.FlatAppearance.MouseDownBackColor = Color.Transparent;
      this.FlatAppearance.MouseOverBackColor = Color.Transparent;
      this.FlatStyle = FlatStyle.Flat;
      this.ForeColor = Color.FromArgb(128, 128, 128);
    }

    [DefaultValue(typeof (Color), "128,128,128")]
    public override Color ForeColor
    {
      get
      {
        if (this.m_Useless)
          return this.UselessColor;
        if (this.m_MouseState == BackButton.MouseState.Hovering)
          return this.HoverColor;
        return this.m_MouseState == BackButton.MouseState.Pressed ? this.PressedColor : base.ForeColor;
      }
      set => base.ForeColor = value;
    }

    public override string Text => "\uE830";

    public override Font Font => this.GlyphFont;

    public bool Useless
    {
      get => this.m_Useless;
      set
      {
        if (this.m_Useless == value)
          return;
        this.m_Useless = value;
        this.Invalidate();
      }
    }

    [DefaultValue(typeof (Color), "50,152,254")]
    public Color HoverColor { get; set; } = Color.FromArgb(50, 152, 254);

    [DefaultValue(typeof (Color), "54,116,178")]
    public Color PressedColor { get; set; } = Color.FromArgb(54, 116, 178);

    protected override void OnMouseEnter(EventArgs e)
    {
      this.m_MouseState = BackButton.MouseState.Hovering;
      base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
      this.m_MouseState = BackButton.MouseState.None;
      base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs mevent)
    {
      this.m_MouseState = BackButton.MouseState.Pressed;
      base.OnMouseDown(mevent);
    }

    protected override void OnMouseUp(MouseEventArgs mevent)
    {
      this.m_MouseState = BackButton.MouseState.None;
      base.OnMouseUp(mevent);
    }

    private enum MouseState
    {
      None,
      Hovering,
      Pressed,
    }
  }
}
