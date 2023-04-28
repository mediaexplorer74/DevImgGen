// Decompiled with JetBrains decompiler
// Type: Microsoft.DriverKit.ApiValidator.ConstantsEXE
// Assembly: ApiValidator, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E124E9BD-E446-409E-89FC-814012FE58D3
// Assembly location: C:\Users\Admin\Desktop\re\dig\apivalidator.exe

using System;

namespace Microsoft.DriverKit.ApiValidator
{
  internal class ConstantsEXE
  {
    internal const string TaskName = "ApiValidation";
    internal const string ErrorStarter = "ApiValidation: ";
    internal const string AitToolName = "aitstatic.exe";
    public static readonly char[] ParamStartChars = new char[2]
    {
      '/',
      '-'
    };
    public const string DriverPackagePath = "DriverPackagePath";
    public const string BinaryPathArg = "BinaryPath";
    public const string XmlFileArg = "SupportedApiXmlFiles";
    public const string ExceptionFileArg = "ModuleWhiteListXmlFiles";
    public const string BinaryExceptionFileArg = "BinaryExclusionListXmlFile";
    public const string ExclusionListArg = "ExclusionList";
    public const string TempOutputFolderArg = "TempOutputFolder";
    public const string ApiExtractorExeArg = "ApiExtractorExePath";
    public const string StrictCompliance = "StrictCompliance";
    public static readonly string UsageInfo = Environment.NewLine + Environment.NewLine + "ApiValidator.exe tool is used to verify that the APIs that your binaries call are valid for a Universal Windows drivers." + Environment.NewLine + "The tool returns an error if your binaries call an API that is outside the set of valid APIs for Universal Windows drivers." + Environment.NewLine + Environment.NewLine + " Usage:" + Environment.NewLine + "       ApiValidation" + Environment.NewLine + "                          -BinaryPath or DriverPackagePath:[Binary File, Binary or Driver Package Folder Path(s)]" + Environment.NewLine + "                          -SupportedApiXmlFiles:[Supported Api File Path(s)]" + Environment.NewLine + "                          [-ModuleWhiteListXmlFiles:[Module WhiteList File Path(s)]" + Environment.NewLine + "                          [-ExclusionList:[Excluded File Path(s)]" + Environment.NewLine + Environment.NewLine + Environment.NewLine + "       -BinaryPath or -DriverPackagePath - Required. Folder(s) and/or file paths(s) specifying binaries or Driver Packages to validate." + Environment.NewLine + "                                           Separate folder/file paths by a semicolon (;). " + Environment.NewLine + "                                           Folder(s) will be checked recursively." + Environment.NewLine + Environment.NewLine + "       -SupportedApiXmlFiles             - Required. Path to XML files containing supported APIs for universal binaries." + Environment.NewLine + "                                           Separate file paths by a semicolon (;). " + Environment.NewLine + Environment.NewLine + "       -ModuleWhiteListXmlFiles          - Optional. Path to XML files containing modules that should be excluded from validation." + Environment.NewLine + "                                           Separate file paths by a semicolon (;). " + Environment.NewLine + Environment.NewLine + "       -ExclusionList                    - Optional. Path to file(s) of APIs that should be excluded from validation." + Environment.NewLine + "                                           Separate file paths by a semicolon (;). " + Environment.NewLine + Environment.NewLine + "       -BinaryExclusionListXmlFile        - Optional. Path to file(s) of Binaries that should be excluded from validation." + Environment.NewLine + "                                           Separate file paths by a semicolon (;). " + Environment.NewLine + Environment.NewLine + "       -StrictCompliance                 - Optional. StrictCompliance set this value to true and IsApiSetImplemented " + Environment.NewLine + "                                           violations will be treated as errors." + Environment.NewLine + "                                           " + Environment.NewLine + Environment.NewLine + Environment.NewLine + "See https://docs.microsoft.com/en-us/windows-hardware/drivers/develop/validating-universal-drivers for more information about how to run ApiValidator." + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + "This is an example of how to call ApiValidator from a WDK command window." + Environment.NewLine + Environment.NewLine + "apivalidator.exe -DriverPackagePath:\"C:\\Program Files(x86)\\Windows Kits\\10\\src\\usb\\umdf2_fx2\\Debug\\\" -SupportedApiXmlFiles:\"c:\\Program Files(x86)\\Windows Kits\\10\build\\universalDDIs\\x64\\UniversalDDIs.xml\"" + Environment.NewLine + Environment.NewLine + "OR " + Environment.NewLine + Environment.NewLine + "apivalidator.exe -DriverPackagePath:\"C:\\Program Files(x86)\\Windows Kits\\10\\src\\usb\\umdf2_fx2\\Debug\\\" -SupportedApiXmlFiles:\"c:\\Program Files(x86)\\Windows Kits\\10\build\\universalDDIs\\x64\\UniversalDDIs.xml\" -StrictCompliance:true" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + "OR " + Environment.NewLine + Environment.NewLine + "apivalidator.exe -DriverPackagePath:\"C:\\Program Files(x86)\\Windows Kits\\10\\src\\usb\\umdf2_fx2\\Debug\\\" -SupportedApiXmlFiles:\"c:\\Program Files(x86)\\Windows Kits\\10\build\\universalDDIs\\x64\\UniversalDDIs.xml\" -BinaryExlusionListXmlFile:\"c:\\Program Files(x86)\\Windows Kits\\10\build\\universalDDIs\\x64\\BinaryExclusionlist.xml\" -StrictCompliance:true" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
  }
}
