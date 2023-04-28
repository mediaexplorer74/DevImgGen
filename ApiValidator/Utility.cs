// Decompiled with JetBrains decompiler
// Type: Microsoft.DriverKit.ApiValidator.Utility
// Assembly: ApiValidator, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E124E9BD-E446-409E-89FC-814012FE58D3
// Assembly location: C:\Users\Admin\Desktop\re\dig\apivalidator.exe

using Microsoft.Kits.Drivers.Validation.Events;
using System;

namespace Microsoft.DriverKit.ApiValidator
{
  internal class Utility
  {
    internal static void OutputStandard(string msg, params object[] args) => Console.Out.WriteLine("ApiValidation: " + msg, args);

    public static void OutputStandardEvent(object sender, ApiValidationEventArgs ave) => Utility.OutputStandard(ave.Message);

    internal static void OutputError(string msg, params object[] args) => Console.Error.WriteLine("ApiValidation: Error: " + msg, args);

    public static void OutputErrorEvent(object sender, ApiValidationEventArgs ave) => Utility.OutputError(ave.Message);

    internal static void OutputWarning(string msg, params object[] args) => Console.Error.WriteLine("ApiValidation: Warning: " + msg, args);

    public static void OutputWarningEvent(object sender, ApiValidationEventArgs ave) => Utility.OutputWarning(ave.Message);

    public static void OutputApiValidationFailEvent(object sender, ApiValidationEventArgs ave)
    {
      if (ave.IgnoreError)
        Utility.OutputStandard(ave.Message, (object) ave.InvalidApi, (object) ave.WindowsModule, (object) ave.ModuleName);
      else
        Utility.OutputError(ave.Message, (object) ave.InvalidApi, (object) ave.WindowsModule, (object) ave.ModuleName);
    }

    internal static void OutputDebug(string msg, params object[] args)
    {
    }

    public static void OutputDebugEvent(object sender, ApiValidationDebugEventArgs de) => Utility.OutputDebug(de.Message);
  }
}
