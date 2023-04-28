// Decompiled with JetBrains decompiler
// Type: DigLib.DriverStore.NativeMethods
// Assembly: DigLib, Version=2021.1.31.1000, Culture=neutral, PublicKeyToken=null
// MVID: 8630F1AA-3914-41FE-A6B1-9C741E0FFE01
// Assembly location: C:\Users\Admin\Desktop\re\dig\DigLib.dll

using System;
using System.Runtime.InteropServices;

namespace DigLib.DriverStore
{
  public class NativeMethods
  {
    [DllImport("drvstore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr DriverPackageOpenW(
      string DriverPackageFilename,
      ProcessorArchitecture ProcessorArchitecture,
      string LocaleName,
      DriverPackageOpenFlags Flags,
      IntPtr ResolveContext);

    [DllImport("drvstore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool DriverPackageGetVersionInfoW(
      IntPtr hDriverPackage,
      ref DriverPackageVersionInfo pVersionInfo);

    [DllImport("drvstore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool DriverPackageEnumDriversW(
      IntPtr hDriverPackage,
      uint Flags,
      NativeMethods.PackageEnumCallback CallbackRoutine,
      IntPtr lParam);

    [DllImport("drvstore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool DriverPackageEnumFilesW(
      IntPtr hDriverPackage,
      IntPtr EnumContext,
      DriverPackageEnumFilesFlags Flags,
      NativeMethods.PackageEnumCallback CallbackRoutine,
      IntPtr lParam);

    [DllImport("drvstore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern void DriverPackageClose(IntPtr hDriverPackage);

    [DllImport("drvstore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr DriverStoreOpenW(
      string TargetSystemPath,
      string TargetSystemDrive,
      uint Flags,
      IntPtr hTransaction);

    [DllImport("drvstore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool DriverStoreEnumW(
      IntPtr hDriverStore,
      uint Flags,
      NativeMethods.StoreEnumCallback CallbackRoutine,
      IntPtr lParam);

    [DllImport("drvstore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool DriverStoreCopyW(
      IntPtr hDriverStore,
      string DriverPackageFilename,
      ProcessorArchitecture ProcessorArchitecture,
      string LocaleName,
      uint Flags,
      string DestinationPath);

    [DllImport("drvstore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern void DriverStoreClose(IntPtr hDriverStore);

    public delegate bool PackageEnumCallback(IntPtr hDriverPackage, IntPtr dataPtr, IntPtr lParam);

    public delegate bool StoreEnumCallback(
      IntPtr hDriverPackage,
      [MarshalAs(UnmanagedType.LPWStr)] string DriverStoreFilename,
      IntPtr dataPtr,
      IntPtr lParam);
  }
}
