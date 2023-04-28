// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.AppxInfoManager
// Assembly: AppXCommon, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 51ADFE6F-E9D3-4C2B-BB15-DCB4FA790979
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXCommon.dll

using Microsoft.Composition.ToolBox;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.ImagingTools.AppX
{
  public class AppxInfoManager
  {
    private List<AppxInfo> _appxPackages = new List<AppxInfo>();
    private Dictionary<string, List<AppxInfo>> _frameworkPackages = new Dictionary<string, List<AppxInfo>>();
    private Dictionary<string, Dictionary<string, bool>> _appxDependencies = new Dictionary<string, Dictionary<string, bool>>();
    private List<AppxInfo> _targetStagedPackages = new List<AppxInfo>();
    private List<AppxInfo> _targetApplicationsPackages = new List<AppxInfo>();
    private string _cpuType;
    private string _languages;
    private string _scales = "100,140,180";
    private List<APPX_PACKAGE_ARCHITECTURE2> _applicableArchitectures;

    public AppxInfoManager(CpuArch cpuType, List<string> languages)
    {
      this._cpuType = AppxUtils.GetApplicableArchitecturesString(cpuType);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string language in languages)
      {
        stringBuilder.Append(',');
        stringBuilder.Append(language);
      }
      this._languages = stringBuilder.ToString(1, stringBuilder.Length - 1);
      this._applicableArchitectures = AppxUtils.GetApplicableArchitectures(cpuType);
    }

    public List<AppxInfo> GetDependentFrameworks(AppxInfo package)
    {
      List<AppxInfo> dependentFrameworks = new List<AppxInfo>();
      if (this._appxDependencies.ContainsKey(package.packageFamilyName))
      {
        foreach (string key in this._appxDependencies[package.packageFamilyName].Keys)
        {
          if (!this._frameworkPackages.ContainsKey(key))
            throw new Exception(string.Format("Missing framework package [{0}] in feature manifest!", (object) key));
          dependentFrameworks.AddRange((IEnumerable<AppxInfo>) this._frameworkPackages[key]);
        }
      }
      return dependentFrameworks;
    }

    public void AddDependency(string packageFamilyName, List<string> dependencyList)
    {
      if (!this._appxDependencies.ContainsKey(packageFamilyName))
        this._appxDependencies[packageFamilyName] = new Dictionary<string, bool>();
      Dictionary<string, bool> appxDependency = this._appxDependencies[packageFamilyName];
      foreach (string dependency in dependencyList)
        appxDependency[dependency] = true;
    }

    public void AddPackage(AppxInfo package)
    {
      if (package.isFramework)
      {
        if (this._frameworkPackages.ContainsKey(package.packageFamilyName))
        {
          bool flag = false;
          List<AppxInfo> frameworkPackage = this._frameworkPackages[package.packageFamilyName];
          for (int index = 0; index < frameworkPackage.Count; ++index)
          {
            AppxInfo appxInfo = frameworkPackage[index];
            if (appxInfo.architecture == package.architecture && appxInfo.version < package.version)
            {
              flag = true;
              frameworkPackage[index] = package;
              break;
            }
          }
          if (flag)
            return;
          this._frameworkPackages[package.packageFamilyName].Add(package);
        }
        else
          this._frameworkPackages[package.packageFamilyName] = new List<AppxInfo>()
          {
            package
          };
      }
      else
        this._appxPackages.Add(package);
    }

    public static bool IsBundlePackageFullName(string packageFullName) => packageFullName.Contains("~");

    public List<AppxInfo> GetStagedPackges() => this._targetStagedPackages;

    public int GetFrameworkPackagesCount() => this._frameworkPackages.Values.Count;

    public List<AppxInfo> GetApplicationsPackages() => this._targetApplicationsPackages;

    public static string GetManifestPath(string dir, bool isBundle) => isBundle ? Path.Combine(dir, "AppxMetadata\\AppxBundleManifest.xml") : Path.Combine(dir, "AppxManifest.xml");

    public static string GetManifestPathFromPreInstalledVolume(AppxInfo package) => AppxInfoManager.GetManifestPath(Path.Combine("P:\\WindowsApps", package.packageFullName), package.isBundle);

    public void ProcessPackages(IULogger logger, CpuArch primaryCPUType)
    {
      foreach (AppxInfo appxPackage1 in this._appxPackages)
      {
        if (appxPackage1.isBundle)
        {
          try
          {
            string applicablePackages = AppxUtils.GetBundleApplicablePackages(appxPackage1.manifestPath, this._cpuType, this._languages, this._scales);
            logger.LogInfo("AppxInfoManager: Adding Bundle {0}", new object[1]
            {
              (object) appxPackage1.packageFullName
            });
            this._targetApplicationsPackages.Add(appxPackage1);
            this._targetStagedPackages.Add(appxPackage1);
            string str1 = applicablePackages;
            char[] chArray = new char[1]{ ',' };
            foreach (string str2 in str1.Split(chArray))
            {
              bool flag = false;
              if (str2.Length > 0)
              {
                foreach (AppxInfo appxPackage2 in this._appxPackages)
                {
                  if (appxPackage2.packageFullName == str2)
                  {
                    flag = true;
                    logger.LogInfo("AppxInfoManager: Adding Staged Applicable package from bundle {0}", new object[1]
                    {
                      (object) appxPackage2.packageFullName
                    });
                    this._targetStagedPackages.Add(appxPackage2);
                    break;
                  }
                }
              }
              if (!flag && str2.Length > 0)
                logger.LogInfo("AppxInfoManager: Could not find Applicable package from bundle {0}", new object[1]
                {
                  (object) str2
                });
            }
          }
          catch (Exception ex)
          {
            logger.LogInfo("AppxInfoManager: GetBundleApplicablePackages failed for Bundle path {0}: {1}", new object[2]
            {
              (object) appxPackage1.manifestPath,
              (object) ex.Message
            });
          }
        }
      }
      APPX_PACKAGE_ARCHITECTURE2 applicableArchitecturesId = AppxUtils.GetApplicableArchitecturesId(primaryCPUType.ToString());
      foreach (AppxInfo appxPackage in this._appxPackages)
      {
        if (appxPackage.isMain)
        {
          bool flag = false;
          foreach (AppxInfo applicationsPackage in this._targetApplicationsPackages)
          {
            if (applicationsPackage.packageFamilyName == appxPackage.packageFamilyName)
            {
              flag = true;
              break;
            }
          }
          if (!flag && appxPackage.architecture == applicableArchitecturesId)
          {
            logger.LogInfo("AppxInfoManager: Adding Primary Arch Applicable Package {0} {1}", new object[2]
            {
              (object) appxPackage.packageFamilyName,
              (object) appxPackage.architecture
            });
            this._targetApplicationsPackages.Add(appxPackage);
            this._targetStagedPackages.Add(appxPackage);
          }
        }
      }
      foreach (AppxInfo appxPackage in this._appxPackages)
      {
        if (this.IsArchitectureApplicable(appxPackage.architecture) && appxPackage.isMain)
        {
          bool flag = false;
          foreach (AppxInfo applicationsPackage in this._targetApplicationsPackages)
          {
            if (applicationsPackage.packageFamilyName == appxPackage.packageFamilyName)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            logger.LogInfo("AppxInfoManager: Adding Other Arch Applicable Package {0} {1}", new object[2]
            {
              (object) appxPackage.packageFamilyName,
              (object) appxPackage.architecture
            });
            this._targetApplicationsPackages.Add(appxPackage);
            this._targetStagedPackages.Add(appxPackage);
          }
        }
      }
      foreach (IEnumerable<AppxInfo> collection in this._frameworkPackages.Values)
        this._targetStagedPackages.AddRange(collection);
    }

    public bool IsArchitectureApplicable(APPX_PACKAGE_ARCHITECTURE2 architecture) => this._applicableArchitectures.Contains(architecture);
  }
}
