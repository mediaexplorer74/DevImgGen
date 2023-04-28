// Decompiled with JetBrains decompiler
// Type: Microsoft.ImagingTools.AppX.AppXImaging
// Assembly: AppXImaging, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 5C560AE0-FEEA-40D9-ACCF-3B1268C88742
// Assembly location: C:\Users\Admin\Desktop\re\dig\AppXImaging.dll

using Microsoft.Composition.ToolBox;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using Microsoft.WindowsPhone.FeatureAPI.AppX;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.ImagingTools.AppX
{
  public class AppXImaging
  {
    public static string c_AppLicensesFolder = "AppData";
    public static string c_WindowsAppsFolder = "WindowsApps";
    public static string c_AppxAllUserStoreRegHive = "AppxAllUserStore.dat";
    public static string c_StagingSubFolder = "appx";
    public static string c_MakeAppxExeName = "MakeAppx.exe";
    public static string c_BundleTempSubFolder = "Bundle";
    public static string c_BundleWithStubTempSubFolder = "BundleWithStub";
    public static string c_AppXTempSubFolder = "Processed";
    public static string c_LXP_Capability_Name = "localExperienceInternal";
    public object _Locker = new object();
    private List<string> _processedPackages = new List<string>();
    private static float c_CommitSizeEstimateMultiplier = 1.1f;

    public void StageAppXPackages(
      IULogger logger,
      List<AppXFMPkgInfo> appXPkgFiles,
      string updateStagingRoot,
      CpuArch cpuType,
      List<string> languages)
    {
      logger.LogInfo("AppXCommon: Stage started for {0}", new object[1]
      {
        (object) cpuType
      });
      string makeAppxExe = LongPath.Combine(LongPath.GetDirectoryName(Assembly.GetEntryAssembly().Location), AppXImaging.c_MakeAppxExeName);
      if (!LongPathFile.Exists(makeAppxExe))
        throw new Exception("MakeAppx.exe not found");
      List<APPX_PACKAGE_ARCHITECTURE2> applicableArchitectures = AppxUtils.GetApplicableArchitectures(cpuType);
      APPX_PACKAGE_ARCHITECTURE2 applicableArchitecturesId = AppxUtils.GetApplicableArchitecturesId(cpuType.ToString());
      AppxInfo appxInfo = new AppxInfo();
      AppxInfoManager appxInfoManager = new AppxInfoManager(cpuType, languages);
      Random random = new Random();
      string path = LongPath.Combine(updateStagingRoot, random.Next().ToString());
      string str1 = LongPath.Combine(path, AppXImaging.c_BundleTempSubFolder);
      string str2 = LongPath.Combine(path, AppXImaging.c_BundleWithStubTempSubFolder);
      string appXTempProcessedFolder = LongPath.Combine(path, AppXImaging.c_AppXTempSubFolder);
      string appsUpdateStagingRoot = LongPath.Combine(updateStagingRoot, AppXImaging.c_StagingSubFolder);
      LongPathDirectory.CreateDirectory(appsUpdateStagingRoot);
      LongPathDirectory.CreateDirectory(str1);
      LongPathDirectory.CreateDirectory(str2);
      LongPathDirectory.CreateDirectory(appXTempProcessedFolder);
      Stopwatch stopwatch1 = new Stopwatch();
      foreach (AppXFMPkgInfo appXpkgFile in appXPkgFiles)
      {
        APPX_PACKAGE_ARCHITECTURE2 packageArchitecturE2 = LongPathFile.Exists(appXpkgFile.PackagePath) ? AppxUtils.GetApplicableArchitecturesId(appXpkgFile.CpuType) : throw new Exception("Error : Appx with path '" + appXpkgFile.PackagePath + "' cannot be found.");
        if (!applicableArchitectures.Contains(packageArchitecturE2))
        {
          logger.LogInfo("AppxCommon: Skipping {0}; architecture not applicable", new object[1]
          {
            (object) appXpkgFile.Name
          });
          return;
        }
        Stopwatch stopwatch2 = new Stopwatch();
        stopwatch1.Start();
        stopwatch2.Start();
        if (AppxUtils.IsAppxBundleFile(appXpkgFile.PackagePath))
        {
          if (appXpkgFile.PreferStub)
            this.UnbundleAppxBundleContents(logger, makeAppxExe, appXpkgFile.PackagePath, str2);
          else
            this.UnbundleAppxBundleContents(logger, makeAppxExe, appXpkgFile.PackagePath, str1);
        }
        else
          this.UnpackAppx(logger, makeAppxExe, appXpkgFile.PackagePath, appXTempProcessedFolder);
        stopwatch2.Stop();
        logger.LogInfo("\tfinished {0}, took {1}", new object[2]
        {
          (object) LongPath.GetFileName(appXpkgFile.PackagePath),
          (object) stopwatch2.Elapsed
        });
      }
      Parallel.ForEach<string>(LongPathDirectory.EnumerateDirectories(str1), (Action<string>) (bundleDir =>
      {
        foreach (string str3 in this.GetRelativeFilePathsForStubPackagesInBundle(bundleDir))
          LongPathFile.Delete(LongPath.Combine(bundleDir, str3));
        foreach (string str4 in this.GetRelativeFilePathsForFullPackagesInBundle(bundleDir))
        {
          string str5 = LongPath.Combine(bundleDir, str4);
          this.UnpackAppx(logger, makeAppxExe, str5, appXTempProcessedFolder);
          LongPathFile.Delete(str5);
        }
      }));
      Parallel.ForEach<string>(LongPathDirectory.EnumerateDirectories(str2), (Action<string>) (bundleDir =>
      {
        foreach (string str6 in this.GetRelativeFilePathsForFullPackagesInBundle(bundleDir))
          LongPathFile.Delete(LongPath.Combine(bundleDir, str6));
        List<string> packagesInBundle = this.GetRelativeFilePathsForStubPackagesInBundle(bundleDir);
        if (packagesInBundle.Count == 0)
          throw new Exception("Bundle at" + bundleDir + "does not contain a stub package, but the bundle was declared with property PreferStub = true in the feature manifest");
        foreach (string str7 in packagesInBundle)
        {
          string str8 = LongPath.Combine(bundleDir, str7);
          this.UnpackAppx(logger, makeAppxExe, str8, appXTempProcessedFolder);
          LongPathFile.Delete(str8);
        }
      }));
      logger.LogInfo("AppXImaging: Copying bundle folder with no appx packages to processed: {0}", new object[1]
      {
        (object) str1
      });
      FileUtils.CopyDirectory(str1, appXTempProcessedFolder);
      FileUtils.CopyDirectory(str2, appXTempProcessedFolder);
      Stopwatch stopwatch3 = new Stopwatch();
      stopwatch3.Start();
      List<string> packagesToPopulate = new List<string>();
      logger.LogDebug("*****************  AppXImaging:   Processing {0} primary architecture packages first  *****************", new object[1]
      {
        (object) applicableArchitecturesId
      });
      List<string> populate1 = this.ProcessPackagesToPopulate(logger, packagesToPopulate, appxInfoManager, applicableArchitecturesId, appXTempProcessedFolder, true);
      logger.LogDebug("***************** AppXImaging:   Processing secondary architecture packages and exclude previous added packages *****************", new object[0]);
      List<string> populate2 = this.ProcessPackagesToPopulate(logger, populate1, appxInfoManager, applicableArchitecturesId, appXTempProcessedFolder, false);
      stopwatch3.Stop();
      logger.LogDebug("AppXImaging: Processing app manifests to get all packages took {0}", new object[1]
      {
        (object) stopwatch3.Elapsed
      });
      Parallel.ForEach<string>(populate2.GroupBy<string, string>((Func<string, string>) (package => package.ToLowerInvariant())).Select<IGrouping<string, string>, string>((Func<IGrouping<string, string>, string>) (group => group.First<string>())), (Action<string>) (appXPackage =>
      {
        string str9 = LongPath.Combine(appXTempProcessedFolder, appXPackage);
        string str10 = LongPath.Combine(appsUpdateStagingRoot, appXPackage);
        if (LongPathDirectory.Exists(str10) || NativeMethods.MoveFile(str9, str10))
          return;
        logger.LogInfo("\tfalling back to CopyDirectory as {0} is likely not on the same volume as {1}", new object[2]
        {
          (object) str9,
          (object) str10
        });
        try
        {
          LongPathDirectory.CreateDirectory(str10);
          FileUtils.CopyDirectory(str9, str10);
        }
        catch (Exception ex)
        {
          logger.LogError("AppXImaging: Creating and copying packages to apps staging directory for package {0} encountered an IO exception", new object[1]
          {
            (object) appXPackage
          });
          logger.LogException(ex);
        }
      }));
      logger.LogDebug("AppXImaging: Removing temporary working directory", new object[0]);
      LongPathDirectory.Delete(path, true);
      stopwatch1.Stop();
      logger.LogInfo("AppXImaging: Stage finished", new object[0]);
      logger.LogInfo("AppXImaging: Total MakeAppx time: " + stopwatch1.Elapsed.ToString(), new object[0]);
    }

    private void UnbundleAppxBundleContents(
      IULogger logger,
      string makeAppxExe,
      string appxBundleFile,
      string appXTempStagingFolder)
    {
      LongPath.Combine(appXTempStagingFolder, LongPath.GetFileName(appxBundleFile));
      logger.LogDebug("AppXImaging: appXTempStagingFolder {0}", new object[1]
      {
        (object) appXTempStagingFolder
      });
      string args = string.Format("unbundle /o /v /p \"{0}\" /d \"{1}\" /pfn", (object) appxBundleFile, (object) appXTempStagingFolder);
      logger.LogDebug("AppXImaging: makeappx.exe {0}", new object[1]
      {
        (object) args
      });
      string processOutput = string.Empty;
      int error = CommonUtils.RunProcess((string) null, makeAppxExe, args, true, true, out processOutput);
      if (error != 0)
      {
        logger.LogError("Command {0} {1} failed with error code {2}, process output = {3}", new object[4]
        {
          (object) makeAppxExe,
          (object) args,
          (object) error,
          (object) processOutput
        });
        throw new Win32Exception(error);
      }
    }

    private void UnpackAppx(
      IULogger logger,
      string makeAppxExe,
      string appxFile,
      string stagingRoot)
    {
      int num1 = 5;
      int num2 = 0;
      string args = string.Format("unpack /o /v /p \"{0}\" /d \"{1}\" /pfn", (object) appxFile, (object) stagingRoot);
      string processOutput = string.Empty;
      int error = 0;
      do
      {
        try
        {
          error = CommonUtils.RunProcess((string) null, makeAppxExe, args, true, true, out processOutput);
          if (error == 0)
            return;
          ++num2;
          logger.LogInfo("AppXImaging: MakeAppX returned {0}.  Retrying {1}", new object[2]
          {
            (object) error,
            (object) num2
          });
          Thread.Sleep(5000);
        }
        catch (IUException ex)
        {
          ++num2;
          logger.LogInfo("AppXImaging: MakeAppX unpack encountered an IO Exception {0}.  Retrying {1}", new object[2]
          {
            (object) ex.InnerException,
            (object) num2
          });
          Thread.Sleep(5000);
        }
        catch (Exception ex)
        {
          ++num2;
          logger.LogInfo("AppXImaging: MakeAppX unpack encountered an Exception {0}.   Retrying {1}", new object[2]
          {
            (object) ex.InnerException,
            (object) num2
          });
          Thread.Sleep(5000);
        }
      }
      while (num2 <= num1);
      logger.LogError("Command {0} {1} failed with retry count {2}, process output = {3}", new object[4]
      {
        (object) makeAppxExe,
        (object) args,
        (object) num2,
        (object) processOutput
      });
      throw new Win32Exception(error);
    }

    private List<string> ProcessPackagesToPopulate(
      IULogger logger,
      List<string> packagesToPopulate,
      AppxInfoManager appxInfoManager,
      APPX_PACKAGE_ARCHITECTURE2 AppXArch,
      string stagingDir,
      bool bFilterPrimaryArch)
    {
      foreach (string directory in LongPathDirectory.GetDirectories(stagingDir))
      {
        AppxInfo appxInfo = new AppxInfo();
        bool flag = false;
        if (AppxInfoManager.IsBundlePackageFullName(LongPath.GetFileName(directory)))
        {
          appxInfo.manifestPath = AppxInfoManager.GetManifestPath(directory, true);
          using (AppXComObject<IAppxBundleFactory> appXcomObject1 = new AppXComObject<IAppxBundleFactory>((IAppxBundleFactory) new AppxBundleFactory()))
          {
            using (AppXComObject<IStream> appXcomObject2 = new AppXComObject<IStream>(AppxUtils.CreateFileStream(appxInfo.manifestPath, STGM.STGM_READ, 1U, false)))
            {
              using (AppXComObject<IAppxBundleManifestReader> appXcomObject3 = new AppXComObject<IAppxBundleManifestReader>(appXcomObject1.Ref().CreateBundleManifestReader(appXcomObject2.Ref())))
              {
                using (AppXComObject<IAppxManifestPackageId2> appXcomObject4 = new AppXComObject<IAppxManifestPackageId2>(appXcomObject3.Ref().GetPackageId() as IAppxManifestPackageId2))
                {
                  appxInfo.architecture = appXcomObject4.Ref().GetArchitecture2();
                  appxInfo.version = appXcomObject4.Ref().GetVersion();
                  appxInfo.packageFullName = appXcomObject4.Ref().GetPackageFullName();
                  appxInfo.packageFamilyName = appXcomObject4.Ref().GetPackageFamilyName();
                }
              }
            }
          }
          logger.LogInfo("AppXImaging:  Adding bundle {0} for arch {1}", new object[2]
          {
            (object) appxInfo.packageFullName,
            (object) appxInfo.architecture
          });
          lock (this._Locker)
            packagesToPopulate.Add(appxInfo.packageFullName);
        }
        else
        {
          appxInfo.manifestPath = AppxInfoManager.GetManifestPath(directory, false);
          using (AppXComObject<IAppxFactory> appXcomObject5 = new AppXComObject<IAppxFactory>((IAppxFactory) new AppxFactory()))
          {
            using (AppXComObject<IStream> appXcomObject6 = new AppXComObject<IStream>(AppxUtils.CreateFileStream(appxInfo.manifestPath, STGM.STGM_READ, 1U, false)))
            {
              using (AppXComObject<IAppxManifestReader> appXcomObject7 = new AppXComObject<IAppxManifestReader>(appXcomObject5.Ref().CreateManifestReader(appXcomObject6.Ref())))
              {
                using (AppXComObject<IAppxManifestPackageId2> appXcomObject8 = new AppXComObject<IAppxManifestPackageId2>(appXcomObject7.Ref().GetPackageId() as IAppxManifestPackageId2))
                {
                  appxInfo.architecture = appXcomObject8.Ref().GetArchitecture2();
                  appxInfo.version = appXcomObject8.Ref().GetVersion();
                  appxInfo.packageFullName = appXcomObject8.Ref().GetPackageFullName();
                  appxInfo.packageFamilyName = appXcomObject8.Ref().GetPackageFamilyName();
                  using (AppXComObject<IAppxManifestProperties> appXcomObject9 = new AppXComObject<IAppxManifestProperties>(appXcomObject7.Ref().GetProperties()))
                  {
                    if (appXcomObject9.Ref().GetBoolValue("Framework"))
                      appxInfo.isFramework = true;
                    else if (appXcomObject9.Ref().GetBoolValue("ResourcePackage"))
                    {
                      appxInfo.isResource = true;
                    }
                    else
                    {
                      appxInfo.isMain = true;
                      if (this._processedPackages.Contains(appxInfo.packageFamilyName))
                        continue;
                    }
                  }
                  if (bFilterPrimaryArch)
                  {
                    if (appxInfo.architecture == AppXArch)
                      flag = true;
                  }
                  else if (appxInfoManager.IsArchitectureApplicable(appxInfo.architecture))
                    flag = true;
                }
              }
            }
          }
          if (flag)
          {
            lock (this._Locker)
            {
              logger.LogInfo("AppXImaging:  Adding package {0} for arch {1}", new object[2]
              {
                (object) appxInfo.packageFullName,
                (object) appxInfo.architecture
              });
              if (appxInfo.isMain)
                this._processedPackages.Add(appxInfo.packageFamilyName);
              packagesToPopulate.Add(appxInfo.packageFullName);
            }
          }
        }
      }
      return packagesToPopulate;
    }

    public uint GetCommitSizeAppX(
      IULogger logger,
      List<AppXFMPkgInfo> AppXPkgFiles,
      string updateStagingRoot,
      CpuArch cpuType,
      List<string> languages,
      List<string> resolutions)
    {
      logger.LogInfo("AppxCommon: Estimating commit size...", new object[0]);
      uint num1 = 0;
      foreach (AppXFMPkgInfo appXpkgFile in AppXPkgFiles)
      {
        if (LongPathFile.Exists(appXpkgFile.PackagePath))
        {
          using (AppXComObject<IStream> appXcomObject = new AppXComObject<IStream>(AppxUtils.CreateFileStream(appXpkgFile.PackagePath, STGM.STGM_READ, 1U, false)))
          {
            uint num2 = !AppxUtils.IsAppxBundleFile(appXpkgFile.PackagePath) ? AppxUtils.GetTotalUncompressedSizeOfPackage(appXcomObject.Ref()) : AppxUtils.GetTotalUncompressedSizeOfBundle(appXcomObject.Ref());
            logger.LogInfo("AppxCommon: {0} will be {1} bytes", new object[2]
            {
              (object) appXpkgFile.Name,
              (object) num2
            });
            num1 += num2;
          }
        }
      }
      uint commitSizeAppX = (uint) ((double) num1 * (double) AppXImaging.c_CommitSizeEstimateMultiplier);
      logger.LogInfo("AppxCommon: Total commit size approximately {0} bytes", new object[1]
      {
        (object) commitSizeAppX
      });
      return commitSizeAppX;
    }

    public void CommitAppXPackages(
      IULogger logger,
      List<AppXFMPkgInfo> appXPkgFiles,
      string updateStagingRoot,
      string preInstalledVolumePath,
      CpuArch cpuType,
      List<string> languages)
    {
      logger.LogInfo("AppXCommon: Commit started for {0}", new object[1]
      {
        (object) cpuType
      });
      object obj = new object();
      AppxInfoManager appxInfo = new AppxInfoManager(cpuType, languages);
      string appsUpdateStagingRoot = LongPath.Combine(updateStagingRoot, AppXImaging.c_StagingSubFolder);
      logger.LogInfo("AppXCommon: Scanning staging root {0}...", new object[1]
      {
        (object) appsUpdateStagingRoot
      });
      List<string> stringList = new List<string>();
      foreach (string enumerateDirectory in LongPathDirectory.EnumerateDirectories(appsUpdateStagingRoot))
      {
        string fileName = LongPath.GetFileName(enumerateDirectory);
        string packageFamilyName = AppxUtils.GetPackageFamilyName(fileName);
        bool flag = false;
        foreach (AppXFMPkgInfo appXpkgFile in appXPkgFiles)
        {
          if (appXpkgFile.ID == packageFamilyName)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          logger.LogError("AppXImaging: Found no Appx ID matching specified package family name {0}. Bailing out...", new object[1]
          {
            (object) packageFamilyName
          });
          return;
        }
        AppxInfo package = new AppxInfo();
        if (AppxInfoManager.IsBundlePackageFullName(fileName))
        {
          package.isBundle = true;
          package.manifestPath = AppxInfoManager.GetManifestPath(enumerateDirectory, true);
          using (AppXComObject<IAppxBundleFactory> appXcomObject1 = new AppXComObject<IAppxBundleFactory>((IAppxBundleFactory) new AppxBundleFactory()))
          {
            using (AppXComObject<IStream> appXcomObject2 = new AppXComObject<IStream>(AppxUtils.CreateFileStream(package.manifestPath, STGM.STGM_READ, 1U, false)))
            {
              using (AppXComObject<IAppxBundleManifestReader> appXcomObject3 = new AppXComObject<IAppxBundleManifestReader>(appXcomObject1.Ref().CreateBundleManifestReader(appXcomObject2.Ref())))
              {
                using (AppXComObject<IAppxManifestPackageId2> appXcomObject4 = new AppXComObject<IAppxManifestPackageId2>(appXcomObject3.Ref().GetPackageId() as IAppxManifestPackageId2))
                {
                  package.architecture = appXcomObject4.Ref().GetArchitecture2();
                  package.version = appXcomObject4.Ref().GetVersion();
                  package.packageFullName = appXcomObject4.Ref().GetPackageFullName();
                  package.packageFamilyName = appXcomObject4.Ref().GetPackageFamilyName();
                }
              }
            }
          }
        }
        else
        {
          package.manifestPath = AppxInfoManager.GetManifestPath(enumerateDirectory, false);
          using (AppXComObject<IAppxFactory> appXcomObject5 = new AppXComObject<IAppxFactory>((IAppxFactory) new AppxFactory()))
          {
            using (AppXComObject<IStream> appXcomObject6 = new AppXComObject<IStream>(AppxUtils.CreateFileStream(package.manifestPath, STGM.STGM_READ, 1U, false)))
            {
              using (AppXComObject<IAppxManifestReader3> appXcomObject7 = new AppXComObject<IAppxManifestReader3>(appXcomObject5.Ref().CreateManifestReader(appXcomObject6.Ref()) as IAppxManifestReader3))
              {
                using (AppXComObject<IAppxManifestPackageId2> appXcomObject8 = new AppXComObject<IAppxManifestPackageId2>(appXcomObject7.Ref().GetPackageId() as IAppxManifestPackageId2))
                {
                  package.architecture = appXcomObject8.Ref().GetArchitecture2();
                  package.version = appXcomObject8.Ref().GetVersion();
                  package.packageFullName = appXcomObject8.Ref().GetPackageFullName();
                  package.packageFamilyName = appXcomObject8.Ref().GetPackageFamilyName();
                  if (appxInfo.IsArchitectureApplicable(package.architecture))
                  {
                    using (AppXComObject<IAppxManifestProperties> appXcomObject9 = new AppXComObject<IAppxManifestProperties>(appXcomObject7.Ref().GetProperties()))
                    {
                      if (appXcomObject9.Ref().GetBoolValue("Framework"))
                        package.isFramework = true;
                      else if (appXcomObject9.Ref().GetBoolValue("ResourcePackage"))
                      {
                        package.isResource = true;
                      }
                      else
                      {
                        package.isMain = true;
                        using (AppXComObject<IAppxManifestPackageDependenciesEnumerator> appXcomObject10 = new AppXComObject<IAppxManifestPackageDependenciesEnumerator>(appXcomObject7.Ref().GetPackageDependencies()))
                        {
                          List<string> dependencyList = new List<string>();
                          while (appXcomObject10.Ref().GetHasCurrent())
                          {
                            using (AppXComObject<IAppxManifestPackageDependency> appXcomObject11 = new AppXComObject<IAppxManifestPackageDependency>(appXcomObject10.Ref().GetCurrent()))
                            {
                              lock (this._Locker)
                              {
                                dependencyList.Add(AppxUtils.GetPackageFaimlyName(appXcomObject11.Ref().GetName(), appXcomObject11.Ref().GetPublisher()));
                                logger.LogInfo("AppXImaging: Adding Dependencies {0}", new object[1]
                                {
                                  (object) AppxUtils.GetPackageFaimlyName(appXcomObject11.Ref().GetName(), appXcomObject11.Ref().GetPublisher())
                                });
                              }
                            }
                            appXcomObject10.Ref().MoveNext();
                          }
                          if (dependencyList.Count > 0)
                          {
                            try
                            {
                              logger.LogInfo("AppXImaging:  AddDependency adding {0} with a total dependency count: {1}", new object[2]
                              {
                                (object) appXcomObject8.Ref().GetPackageFamilyName(),
                                (object) dependencyList.Count
                              });
                              lock (this._Locker)
                                appxInfo.AddDependency(package.packageFamilyName, dependencyList);
                            }
                            catch (IUException ex)
                            {
                              logger.LogError("AppXImaging: Adding Depenencies encountered an exception", new object[1]
                              {
                                (object) ex
                              });
                            }
                          }
                          else
                            logger.LogDebug("************  AppXImaging:  Found no dependencies for {0}", new object[1]
                            {
                              (object) package.packageFullName
                            });
                        }
                      }
                      using (AppXComObject<IAppxManifestCapabilitiesEnumerator> appXcomObject12 = new AppXComObject<IAppxManifestCapabilitiesEnumerator>(appXcomObject7.Ref().GetCapabilitiesByCapabilityClass(APPX_CAPABILITY_CLASS_TYPE.APPX_CAPABILITY_CLASS_WINDOWS)))
                      {
                        while (appXcomObject12.Ref().GetHasCurrent())
                        {
                          if (appXcomObject12.Ref().GetCurrent().Equals(AppXImaging.c_LXP_Capability_Name, StringComparison.OrdinalIgnoreCase))
                          {
                            logger.LogInfo("AppXImaging:  Found LXP {0}", new object[1]
                            {
                              (object) package.packageFamilyName
                            });
                            stringList.Add(package.packageFamilyName);
                            break;
                          }
                          appXcomObject12.Ref().MoveNext();
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
        lock (obj)
        {
          logger.LogInfo("AppXImaging:  Adding package to appxInfoManager {0}", new object[1]
          {
            (object) package.packageFullName
          });
          appxInfo.AddPackage(package);
        }
      }
      appxInfo.ProcessPackages(logger, cpuType);
      List<AppxInfo> stagedPackges = appxInfo.GetStagedPackges();
      List<AppxInfo> applicationsPackages = appxInfo.GetApplicationsPackages();
      logger.LogInfo("AppXCommon: Found {0} apps, {1} frameworks, total {2} packages...", new object[3]
      {
        (object) applicationsPackages.Count,
        (object) appxInfo.GetFrameworkPackagesCount(),
        (object) stagedPackges.Count
      });
      logger.LogInfo("AppXCommon: Copy license XML files...", new object[0]);
      string path = LongPath.Combine(preInstalledVolumePath, AppXImaging.c_AppLicensesFolder);
      LongPathDirectory.CreateDirectory(path);
      List<APPX_PACKAGE_ARCHITECTURE2> applicableArchitectures = AppxUtils.GetApplicableArchitectures(cpuType);
      foreach (AppXFMPkgInfo appXpkgFile in appXPkgFiles)
      {
        APPX_PACKAGE_ARCHITECTURE2 applicableArchitecturesId = AppxUtils.GetApplicableArchitecturesId(appXpkgFile.CpuType);
        if (!applicableArchitectures.Contains(applicableArchitecturesId))
          logger.LogInfo("AppxCommon: Skipping {0}; architecture not applicable", new object[1]
          {
            (object) appXpkgFile.Name
          });
        else if (string.IsNullOrEmpty(appXpkgFile.LicenseFilePath))
        {
          logger.LogInfo("AppxCommon: Skipping {0}; license not specified", new object[1]
          {
            (object) appXpkgFile.Name
          });
        }
        else
        {
          string str = LongPath.Combine(path, appXpkgFile.ID + ".xml");
          if (!LongPathFile.Exists(str))
          {
            logger.LogDebug("AppXImaging: Copying License file {0}.", new object[1]
            {
              (object) str
            });
            LongPathFile.Copy(appXpkgFile.LicenseFilePath, str, true);
          }
        }
      }
      string windowsAppsFolder = LongPath.Combine(preInstalledVolumePath, AppXImaging.c_WindowsAppsFolder);
      LongPathDirectory.CreateDirectory(windowsAppsFolder);
      Parallel.ForEach<AppxInfo>(stagedPackges.GroupBy<AppxInfo, string>((Func<AppxInfo, string>) (package => package.packageFullName.ToLowerInvariant())).Select<IGrouping<string, AppxInfo>, AppxInfo>((Func<IGrouping<string, AppxInfo>, AppxInfo>) (group => group.First<AppxInfo>())), (Action<AppxInfo>) (package =>
      {
        logger.LogInfo("AppXCommon: Copy package {0} with FamilyName {1}...", new object[2]
        {
          (object) package.packageFullName,
          (object) package.packageFamilyName
        });
        string str1 = LongPath.Combine(appsUpdateStagingRoot, package.packageFullName);
        string str2 = LongPath.Combine(windowsAppsFolder, package.packageFullName);
        if (LongPathDirectory.Exists(str2) || NativeMethods.MoveFile(str1, str2))
          return;
        logger.LogInfo("\tfalling back to CopyDirectory as {0} is likely not on the same volume as {1}", new object[2]
        {
          (object) str1,
          (object) str2
        });
        FileUtils.CopyDirectory(str1, str2, true);
      }));
      AppxUtils.SaveSpace(logger, windowsAppsFolder);
      string str3 = LongPath.Combine(preInstalledVolumePath, AppXImaging.c_WindowsAppsFolder, AppXImaging.c_AppxAllUserStoreRegHive);
      LongPathFile.Delete(str3);
      using (SafeRegistryHandle handle = new SafeRegistryHandle(new IntPtr(AppxUtils.RegLoadAppKey(str3)), true))
      {
        using (RegistryKey regKey = RegistryKey.FromHandle(handle))
        {
          foreach (AppxInfo package in applicationsPackages)
          {
            string region = (string) null;
            bool flag = false;
            foreach (AppXFMPkgInfo appXpkgFile in appXPkgFiles)
            {
              if (appXpkgFile.ID == package.packageFamilyName)
              {
                flag = true;
                region = appXpkgFile.Region;
                break;
              }
            }
            if (!flag)
            {
              logger.LogError("AppXImaging: Found no Appx ID matching specified package family name {0} while writing hive. Bailing out...", new object[1]
              {
                (object) package.packageFamilyName
              });
              return;
            }
            logger.LogInfo("AppXCommon: Write to registry [AppxAllUserStore\\Applications\\{0}]...", new object[1]
            {
              (object) package.packageFullName
            });
            lock (this._Locker)
              AppxUtils.AddApplication(appxInfo, package, regKey, region);
          }
          foreach (AppxInfo package in stagedPackges)
          {
            logger.LogInfo("AppXCommon: Write to registry [AppxAllUserStore\\Staged\\{0}\\{1}]...", new object[2]
            {
              (object) package.packageFamilyName,
              (object) package.packageFullName
            });
            lock (this._Locker)
              AppxUtils.AddStaged(appxInfo, package, regKey);
          }
          foreach (string packageFamilyName in stringList)
          {
            logger.LogInfo("AppXCommon: Write to registry [AppxAllUserStore\\Config\\{0} SetupPhase {1}]...", new object[2]
            {
              (object) packageFamilyName,
              (object) 4U
            });
            lock (this._Locker)
              AppxUtils.AddSetupPhase(packageFamilyName, SetupPhase.SetupPhase_PostShell, regKey);
          }
        }
      }
      logger.LogInfo("AppXCommon: Clean up apps staging folder", new object[0]);
      FileUtils.DeleteTree(appsUpdateStagingRoot);
      logger.LogInfo("AppXCommon: GC before finish", new object[0]);
      GC.Collect();
      GC.WaitForPendingFinalizers();
      GC.Collect();
      GC.WaitForPendingFinalizers();
      logger.LogInfo("AppXCommon: Commit finished", new object[0]);
    }

    private List<string> GetRelativeFilePathsForStubPackagesInBundle(string unpackedBundleDir) => this.GetRelativeFilePathsForPackagesInBundle(unpackedBundleDir, true);

    private List<string> GetRelativeFilePathsForFullPackagesInBundle(string unpackedBundleDir) => this.GetRelativeFilePathsForPackagesInBundle(unpackedBundleDir, false);

    private List<string> GetRelativeFilePathsForPackagesInBundle(
      string unpackedBundleDir,
      bool getStubPackages)
    {
      List<string> packagesInBundle = new List<string>();
      string manifestPath = AppxInfoManager.GetManifestPath(unpackedBundleDir, true);
      using (AppXComObject<IAppxBundleFactory> appXcomObject1 = new AppXComObject<IAppxBundleFactory>((IAppxBundleFactory) new AppxBundleFactory()))
      {
        using (AppXComObject<IStream> appXcomObject2 = new AppXComObject<IStream>(AppxUtils.CreateFileStream(manifestPath, STGM.STGM_READ, 1U, false)))
        {
          using (AppXComObject<IAppxBundleManifestReader> appXcomObject3 = new AppXComObject<IAppxBundleManifestReader>(appXcomObject1.Ref().CreateBundleManifestReader(appXcomObject2.Ref())))
          {
            using (AppXComObject<IAppxBundleManifestPackageInfoEnumerator> appXcomObject4 = new AppXComObject<IAppxBundleManifestPackageInfoEnumerator>(appXcomObject3.Ref().GetPackageInfoItems()))
            {
              while (appXcomObject4.Ref().GetHasCurrent())
              {
                using (AppXComObject<IAppxBundleManifestPackageInfo4> appXcomObject5 = new AppXComObject<IAppxBundleManifestPackageInfo4>(appXcomObject4.Ref().GetCurrent() as IAppxBundleManifestPackageInfo4))
                {
                  if (getStubPackages == appXcomObject5.Ref().GetIsStub())
                  {
                    string fileName = appXcomObject5.Ref().GetFileName();
                    packagesInBundle.Add(fileName);
                  }
                }
                appXcomObject4.Ref().MoveNext();
              }
            }
          }
        }
      }
      return packagesInBundle;
    }
  }
}
