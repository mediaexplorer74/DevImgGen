// Decompiled with JetBrains decompiler
// Type: Microsoft.DriverKit.ApiValidator.ApiValidatorExe
// Assembly: ApiValidator, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: E124E9BD-E446-409E-89FC-814012FE58D3
// Assembly location: C:\Users\Admin\Desktop\re\dig\apivalidator.exe

using Microsoft.Kits.Drivers.Validation.Events;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DriverKit.ApiValidator
{
  public class ApiValidatorExe
  {
    internal static int Main(string[] args)
    {
      Microsoft.Kits.Drivers.Validation.ApiValidator apiv = new Microsoft.Kits.Drivers.Validation.ApiValidator();
      apiv.Events.Debug += new ApiValidationDebug(Utility.OutputDebugEvent);
      apiv.Events.Error += new ApiValidationError(Utility.OutputErrorEvent);
      apiv.Events.Info += new ApiValidationInfo(Utility.OutputStandardEvent);
      apiv.Events.ValidationFailure += new ApiValidationFailure(Utility.OutputApiValidationFailEvent);
      apiv.Events.Warning += new ApiValidationWarning(Utility.OutputWarningEvent);
      ArgumentParser argumentParser = new ArgumentParser(apiv);
      Utility.OutputDebug("[ApiValidation::Main]");
      return !argumentParser.ParseParams(((IEnumerable<string>) args).ToList<string>()) ? 2 : (int) apiv.Run();
    }
  }
}
