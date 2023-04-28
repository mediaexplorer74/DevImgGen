// Decompiled with JetBrains decompiler
// Type: DevImgGen.Pages.BuildPage
// Assembly: DevImgGen, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8B415242-7DF5-46C1-83D1-ACA4F55A74B9
// Assembly location: C:\Users\Admin\Desktop\re\dig\DevImgGen.exe

using DevImgGen.Controls;
using DigLib;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DevImgGen.Pages
{
  public class BuildPage : BasePage
  {
    private IContainer components;
    private BackButton btnBack;
    private Label lblHeader;
    private Button btnBrowseCfg;
    private TextBox tbConfigDir;
    private Label lblConfigLocation;
    private CommandButton cmdStartBuilding;
    private Button btnBrowseOSPkg;
    private TextBox tbOSPkgDir;
    private Label lblOSPkgLocation;
    private CheckBox chkDispFall;
    private Button btnBrowseImg;
    private TextBox tbImageLocation;
    private Label lblImageLocation;
    private Label lblBuildTip;

    public BuildPage(string packageLocation = null)
    {
      this.InitializeComponent();
      if (string.IsNullOrEmpty(packageLocation))
        return;
      this.tbConfigDir.Text = packageLocation;
    }

    private void btnBack_Click(object sender, EventArgs e) => this.OnPageChangeRequested(PageEnum.Landing);

    private void btnBrowseOSPkg_Click(object sender, EventArgs e)
    {
      using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
      {
        folderBrowserDialog.Description = this.lblOSPkgLocation.Text;
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK || !Directory.Exists(folderBrowserDialog.SelectedPath))
          return;
        this.tbOSPkgDir.Text = folderBrowserDialog.SelectedPath;
      }
    }

    private void btnBrowseCfg_Click(object sender, EventArgs e)
    {
      using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
      {
        folderBrowserDialog.Description = this.lblConfigLocation.Text;
        if (Directory.Exists(this.tbConfigDir.Text))
        {
          folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
          folderBrowserDialog.SelectedPath = this.tbConfigDir.Text;
        }
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK || !Directory.Exists(folderBrowserDialog.SelectedPath))
          return;
        this.tbConfigDir.Text = folderBrowserDialog.SelectedPath;
      }
    }

    private void cmdStartBuilding_Click(object sender, EventArgs e)
    {
      if (!File.Exists("ReferenceOEMInput.xml"))
      {
        int num1 = (int) MessageBox.Show("Reference OEM input XML is missing from the toolkit's root directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!Directory.EnumerateFileSystemEntries(this.tbOSPkgDir.Text, "*ModernPCNonProductionFM*", SearchOption.AllDirectories).Any<string>())
      {
        int num2 = (int) MessageBox.Show("It appears that the OS packages you've downloaded are of ReleaseType: Production. ReleaseType: Test packages are required to build an image using this tool.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!Directory.EnumerateFileSystemEntries(this.tbOSPkgDir.Text, "appx").Any<string>() || !Directory.EnumerateFileSystemEntries(this.tbOSPkgDir.Text, "FMFiles").Any<string>() || !Directory.EnumerateFileSystemEntries(this.tbOSPkgDir.Text, "Retail").Any<string>())
      {
        int num3 = (int) MessageBox.Show("Failed to find one or more of the common package folders (such as 'appx', 'FMFiles', and 'Retail') in the OS packages directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (string.IsNullOrEmpty(this.tbConfigDir.Text) && MessageBox.Show("No configuration package directory was specified. Are you sure you want to build without drivers?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        XDocument xdocument = XDocument.Load("ReferenceOEMInput.xml");
        XElement xelement = xdocument.Element((XName) "{http://schemas.microsoft.com/embedded/2019/06/ImageUpdate}OEMInput").Element((XName) "{http://schemas.microsoft.com/embedded/2019/06/ImageUpdate}AdditionalFMs");
        foreach (XElement element in xelement.Elements())
          element.Value = Path.Combine(Path.GetFullPath(this.tbOSPkgDir.Text), element.Value);
        if (!string.IsNullOrEmpty(this.tbConfigDir.Text))
        {
          string target = Path.Combine(this.tbOSPkgDir.Text, "Volantis");
          Utils.RecursiveCopyDirectory(Path.Combine(this.tbConfigDir.Text, "Volantis"), target);
          foreach (string enumerateFileSystemEntry in Directory.EnumerateFileSystemEntries(this.tbConfigDir.Text, "*Driver*FM.xml", SearchOption.AllDirectories))
            xelement.Add((object) new XElement((XName) "{http://schemas.microsoft.com/embedded/2019/06/ImageUpdate}AdditionalFM", (object) enumerateFileSystemEntry));
        }
        if (this.chkDispFall.Checked)
          xelement.Add((object) new XElement((XName) "{http://schemas.microsoft.com/embedded/2019/06/ImageUpdate}AdditionalFM", (object) Path.Combine(this.tbOSPkgDir.Text, "Volantis\\amd64\\DisplayFallbackFM.xml")));
        string fileName = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(this.tbImageLocation.Text)), "OEMInput.xml");
        xdocument.Save(fileName);
        string currentDirectory = Directory.GetCurrentDirectory();
        using (Process process = new Process())
        {
          process.StartInfo.FileName = "cmd.exe";
          process.StartInfo.Arguments = "/k " + Path.GetPathRoot(currentDirectory).Substring(0, 2) + " && cd \"" + currentDirectory + "\" && imggen.cmd \"" + this.tbImageLocation.Text + "\" \"" + fileName + "\" \"" + this.tbOSPkgDir.Text + "\" amd64";
          process.StartInfo.UseShellExecute = true;
          process.StartInfo.Verb = "runas";
          process.Start();
        }
        this.cmdStartBuilding.Enabled = false;
      }
    }

    private void tbOSPkgDir_TextChanged(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.tbImageLocation.Text) || this.cmdStartBuilding.Enabled)
        return;
      this.cmdStartBuilding.Enabled = true;
    }

    private void tbImageLocation_TextChanged(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.tbOSPkgDir.Text) || this.cmdStartBuilding.Enabled)
        return;
      this.cmdStartBuilding.Enabled = true;
    }

    private void btnBrowseImg_Click(object sender, EventArgs e)
    {
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.Filter = "Full Flash Update (*.ffu)|*.ffu|Virtual Hard Disk (*.vhdx)|*.vhdx|Virtual Hard Disk (*.vhd)|*.vhd";
        saveFileDialog.FileName = "Flash.ffu";
        saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
        saveFileDialog.Title = this.lblImageLocation.Text;
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        this.tbImageLocation.Text = saveFileDialog.FileName;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnBack = new BackButton();
      this.lblHeader = new Label();
      this.btnBrowseCfg = new Button();
      this.tbConfigDir = new TextBox();
      this.lblConfigLocation = new Label();
      this.cmdStartBuilding = new CommandButton();
      this.btnBrowseOSPkg = new Button();
      this.tbOSPkgDir = new TextBox();
      this.lblOSPkgLocation = new Label();
      this.chkDispFall = new CheckBox();
      this.btnBrowseImg = new Button();
      this.tbImageLocation = new TextBox();
      this.lblImageLocation = new Label();
      this.lblBuildTip = new Label();
      this.SuspendLayout();
      this.btnBack.FlatAppearance.BorderSize = 0;
      this.btnBack.FlatAppearance.MouseDownBackColor = Color.Transparent;
      this.btnBack.FlatAppearance.MouseOverBackColor = Color.Transparent;
      this.btnBack.FlatStyle = FlatStyle.Flat;
      this.btnBack.ForeColor = Color.FromArgb(128, 128, 128);
      this.btnBack.Location = new Point(5, 14);
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new Size(32, 25);
      this.btnBack.TabIndex = 0;
      this.btnBack.Text = "\uE830";
      this.btnBack.Useless = false;
      this.btnBack.UseVisualStyleBackColor = true;
      this.btnBack.Click += new EventHandler(this.btnBack_Click);
      this.lblHeader.AutoSize = true;
      this.lblHeader.Font = new Font("Segoe UI", 11.5f);
      this.lblHeader.ForeColor = Color.FromArgb(0, 51, 153);
      this.lblHeader.Location = new Point(40, 14);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(113, 21);
      this.lblHeader.TabIndex = 1;
      this.lblHeader.Text = "Build an image";
      this.btnBrowseCfg.Location = new Point(462, 188);
      this.btnBrowseCfg.Name = "btnBrowseCfg";
      this.btnBrowseCfg.Size = new Size(75, 25);
      this.btnBrowseCfg.TabIndex = 10;
      this.btnBrowseCfg.Text = "Browse...";
      this.btnBrowseCfg.UseVisualStyleBackColor = true;
      this.btnBrowseCfg.Click += new EventHandler(this.btnBrowseCfg_Click);
      this.tbConfigDir.Location = new Point(44, 189);
      this.tbConfigDir.Name = "tbConfigDir";
      this.tbConfigDir.Size = new Size(412, 23);
      this.tbConfigDir.TabIndex = 9;
      this.lblConfigLocation.AutoSize = true;
      this.lblConfigLocation.Location = new Point(41, 168);
      this.lblConfigLocation.Name = "lblConfigLocation";
      this.lblConfigLocation.Size = new Size(292, 15);
      this.lblConfigLocation.TabIndex = 8;
      this.lblConfigLocation.Text = "Pick a folder containing driver configuration packages";
      this.cmdStartBuilding.Enabled = false;
      this.cmdStartBuilding.FlatStyle = FlatStyle.System;
      this.cmdStartBuilding.Location = new Point(43, 251);
      this.cmdStartBuilding.Name = "cmdStartBuilding";
      this.cmdStartBuilding.Note = "";
      this.cmdStartBuilding.Size = new Size(494, 44);
      this.cmdStartBuilding.TabIndex = 12;
      this.cmdStartBuilding.Text = "Start building";
      this.cmdStartBuilding.UseVisualStyleBackColor = true;
      this.cmdStartBuilding.Click += new EventHandler(this.cmdStartBuilding_Click);
      this.btnBrowseOSPkg.Location = new Point(462, 74);
      this.btnBrowseOSPkg.Name = "btnBrowseOSPkg";
      this.btnBrowseOSPkg.Size = new Size(75, 25);
      this.btnBrowseOSPkg.TabIndex = 4;
      this.btnBrowseOSPkg.Text = "Browse...";
      this.btnBrowseOSPkg.UseVisualStyleBackColor = true;
      this.btnBrowseOSPkg.Click += new EventHandler(this.btnBrowseOSPkg_Click);
      this.tbOSPkgDir.Location = new Point(44, 75);
      this.tbOSPkgDir.Name = "tbOSPkgDir";
      this.tbOSPkgDir.Size = new Size(412, 23);
      this.tbOSPkgDir.TabIndex = 3;
      this.tbOSPkgDir.TextChanged += new EventHandler(this.tbOSPkgDir_TextChanged);
      this.lblOSPkgLocation.AutoSize = true;
      this.lblOSPkgLocation.Location = new Point(41, 54);
      this.lblOSPkgLocation.Name = "lblOSPkgLocation";
      this.lblOSPkgLocation.Size = new Size(202, 15);
      this.lblOSPkgLocation.TabIndex = 2;
      this.lblOSPkgLocation.Text = "Pick a folder containing OS packages";
      this.chkDispFall.AutoSize = true;
      this.chkDispFall.Location = new Point(44, 219);
      this.chkDispFall.Name = "chkDispFall";
      this.chkDispFall.Size = new Size(479, 19);
      this.chkDispFall.TabIndex = 11;
      this.chkDispFall.Text = "Set up display fallback (useful if you lack a display driver or want to use a non-MS VM)";
      this.chkDispFall.UseVisualStyleBackColor = true;
      this.btnBrowseImg.Location = new Point(462, 131);
      this.btnBrowseImg.Name = "btnBrowseImg";
      this.btnBrowseImg.Size = new Size(75, 25);
      this.btnBrowseImg.TabIndex = 7;
      this.btnBrowseImg.Text = "Browse...";
      this.btnBrowseImg.UseVisualStyleBackColor = true;
      this.btnBrowseImg.Click += new EventHandler(this.btnBrowseImg_Click);
      this.tbImageLocation.Location = new Point(44, 132);
      this.tbImageLocation.Name = "tbImageLocation";
      this.tbImageLocation.Size = new Size(412, 23);
      this.tbImageLocation.TabIndex = 6;
      this.tbImageLocation.TextChanged += new EventHandler(this.tbImageLocation_TextChanged);
      this.lblImageLocation.AutoSize = true;
      this.lblImageLocation.Location = new Point(41, 111);
      this.lblImageLocation.Name = "lblImageLocation";
      this.lblImageLocation.Size = new Size(211, 15);
      this.lblImageLocation.TabIndex = 5;
      this.lblImageLocation.Text = "Where do you want to save the image?";
      this.lblBuildTip.AutoSize = true;
      this.lblBuildTip.ForeColor = SystemColors.GrayText;
      this.lblBuildTip.Location = new Point(41, 308);
      this.lblBuildTip.Name = "lblBuildTip";
      this.lblBuildTip.Size = new Size(486, 30);
      this.lblBuildTip.TabIndex = 13;
      this.lblBuildTip.Text = "Info: Building happens in a separate elevated process. Make sure to keep an eye on the new\r\nwindow to track the image's build progress.";
      this.AutoScaleDimensions = new SizeF(96f, 96f);
      this.AutoScaleMode = AutoScaleMode.Dpi;
      this.Controls.Add((Control) this.lblBuildTip);
      this.Controls.Add((Control) this.btnBrowseImg);
      this.Controls.Add((Control) this.tbImageLocation);
      this.Controls.Add((Control) this.lblImageLocation);
      this.Controls.Add((Control) this.chkDispFall);
      this.Controls.Add((Control) this.btnBrowseCfg);
      this.Controls.Add((Control) this.tbConfigDir);
      this.Controls.Add((Control) this.lblConfigLocation);
      this.Controls.Add((Control) this.cmdStartBuilding);
      this.Controls.Add((Control) this.btnBrowseOSPkg);
      this.Controls.Add((Control) this.tbOSPkgDir);
      this.Controls.Add((Control) this.lblOSPkgLocation);
      this.Controls.Add((Control) this.lblHeader);
      this.Controls.Add((Control) this.btnBack);
      this.Font = new Font("Segoe UI", 9f);
      this.Name = nameof (BuildPage);
      this.Size = new Size(550, 400);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
