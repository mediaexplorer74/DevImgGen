// Decompiled with JetBrains decompiler
// Type: DevImgGen.MainForm
// Assembly: DevImgGen, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8B415242-7DF5-46C1-83D1-ACA4F55A74B9
// Assembly location: C:\Users\Admin\Desktop\re\dig\DevImgGen.exe

using DevImgGen.Pages;
using DevImgGen.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevImgGen
{
  public class MainForm : Form
  {
    private string m_DriverLocation;
    private string m_PackageLocation;
    private IContainer components;

    public MainForm()
    {
      this.InitializeComponent();
      this.Icon = Resources.Icon;
      this.PageChangeRequested((object) null, PageEnum.Landing);
    }

    private void PageChangeRequested(object sender, PageEnum e)
    {
      switch (e)
      {
        case PageEnum.Landing:
          LandingPage landingPage = new LandingPage();
          landingPage.PageChangeRequested += new EventHandler<PageEnum>(this.PageChangeRequested);
          this.Controls.Add((Control) landingPage);
          break;
        case PageEnum.Export:
          ExportPage exportPage = new ExportPage();
          exportPage.PageChangeRequested += new EventHandler<PageEnum>(this.PageChangeRequested);
          exportPage.DriverLocationChanged += new EventHandler<string>(this.DriverLocationChanged);
          this.Controls.Add((Control) exportPage);
          break;
        case PageEnum.CreateConfig:
          CreateConfigPage createConfigPage = new CreateConfigPage(this.m_DriverLocation);
          createConfigPage.PageChangeRequested += new EventHandler<PageEnum>(this.PageChangeRequested);
          createConfigPage.PackageLocationChanged += new EventHandler<string>(this.PackageLocationChanged);
          this.Controls.Add((Control) createConfigPage);
          break;
        case PageEnum.Build:
          BuildPage buildPage = new BuildPage(this.m_PackageLocation);
          buildPage.PageChangeRequested += new EventHandler<PageEnum>(this.PageChangeRequested);
          this.Controls.Add((Control) buildPage);
          break;
      }
      if (this.Controls.Count <= 1)
        return;
      this.RemovePageFromStack();
    }

    private void PackageLocationChanged(object sender, string e) => this.m_PackageLocation = e;

    private void DriverLocationChanged(object sender, string e) => this.m_DriverLocation = e;

    private void RemovePageFromStack(int distance = 1)
    {
      Control control = this.Controls[this.Controls.Count - 1 - distance];
      this.Controls.Remove(control);
      control.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(96f, 96f);
      this.AutoScaleMode = AutoScaleMode.Dpi;
      this.BackColor = SystemColors.ControlLightLight;
      this.ClientSize = new Size(581, 436);
      this.Font = new Font("Segoe UI", 9f);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = nameof (MainForm);
      this.Text = "Windows Device Image Generator";
      this.ResumeLayout(false);
    }
  }
}
