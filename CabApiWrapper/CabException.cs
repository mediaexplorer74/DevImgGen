// Decompiled with JetBrains decompiler
// Type: Microsoft.WindowsPhone.ImageUpdate.Tools.CabException
// Assembly: CabApiWrapper, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 866557A1-48F3-47A0-86C4-323A9E15F833
// Assembly location: C:\Users\Admin\Desktop\re\dig\CabApiWrapper.dll

using System;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools
{
  public class CabException : ApplicationException
  {
    private const string STR_CABERROR = "Cab operation failed with hr = 0x{0:X8} [{1}], CAB Operation {2}, Params: {3}";

    public uint CabHResult { get; private set; }

    public CabException(uint cabHR)
      : base(CabErrorMapper.Instance.MapError(cabHR))
    {
      this.CabHResult = cabHR;
    }

    public CabException(uint cabHR, string cabMethod, params string[] args)
      : base(CabException.FormatMessage(cabHR, cabMethod, args))
    {
      this.CabHResult = cabHR;
    }

    public CabException(string msg)
      : base(msg)
    {
    }

    public CabException(string message, params object[] args)
      : base(string.Format(message, args))
    {
    }

    public CabException(string msg, Exception inner)
      : base(msg, inner)
    {
    }

    private static string FormatMessage(uint cabHR, string cabMethod, params string[] list)
    {
      string str = string.Join(",", list);
      return string.Format("Cab operation failed with hr = 0x{0:X8} [{1}], CAB Operation {2}, Params: {3}", (object) cabHR, (object) CabErrorMapper.Instance.MapError(cabHR), (object) cabMethod, (object) str);
    }
  }
}
