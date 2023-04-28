// Decompiled with JetBrains decompiler
// Type: Microsoft.WindowsPhone.ImageUpdate.Tools.CabErrorMapper
// Assembly: CabApiWrapper, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 866557A1-48F3-47A0-86C4-323A9E15F833
// Assembly location: C:\Users\Admin\Desktop\re\dig\CabApiWrapper.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools
{
  public class CabErrorMapper
  {
    private static readonly Lazy<CabErrorMapper> _instance = new Lazy<CabErrorMapper>((Func<CabErrorMapper>) (() => new CabErrorMapper()));
    private const uint CABAPI_ERR_BASE = 2149089984;
    private const uint CABAPI_ERR_FCI_BASE = 2149090000;
    private const uint CABAPI_ERR_FDI_BASE = 2149090016;
    public const uint E_CABAPI_NOT_CABINET = 2149089985;
    public const string STR_E_CABAPI_NOT_CABINET = "Specified file is not a valid cabinet.";
    public const uint E_CABAPI_UNKNOWN_FILE = 2149089986;
    public const string STR_E_CABAPI_UNKNOWN_FILE = "CAB extraction failed: One or more files in extraction list not found in cabinet.";
    public const uint E_CABAPI_FCI_OPEN_SRC = 2149090001;
    public const string STR_E_CABAPI_FCI_OPEN_SRC = "CAB creation failed: Could not open source file.";
    public const uint E_CABAPI_FCI_READ_SRC = 2149090002;
    public const string STR_E_CABAPI_FCI_READ_SRC = "CAB creation failed: Could not read source file.";
    public const uint E_CABAPI_FCI_ALLOC_FAIL = 2149090003;
    public const string STR_E_CABAPI_FCI_ALLOC_FAIL = "CAB creation failed: FCI failed to allocate memory.";
    public const uint E_CABAPI_FCI_TEMP_FILE = 2149090004;
    public const string STR_E_CABAPI_FCI_TEMP_FILE = "CAB creation failed: FCI failed to create temporary file.";
    public const uint E_CABAPI_FCI_BAD_COMPR_TYPE = 2149090005;
    public const string STR_E_CABAPI_FCI_BAD_COMPR_TYPE = "CAB creation failed: Unknown compression type.";
    public const uint E_CABAPI_FCI_CAB_FILE = 2149090006;
    public const string STR_E_CABAPI_FCI_CAB_FILE = "CAB creation failed: FCI failed to create cabinet file.";
    public const uint E_CABAPI_FCI_USER_ABORT = 2149090007;
    public const string STR_E_CABAPI_FCI_USER_ABORT = "CAB creation failed: FCI aborted on user request.";
    public const uint E_CABAPI_FCI_MCI_FAIL = 2149090008;
    public const string STR_E_CABAPI_FCI_MCI_FAIL = "CAB creation failed: FCI failed to compress data.";
    public const uint E_CABAPI_FCI_CAB_FORMAT_LIMIT = 2149090009;
    public const string STR_E_CABAPI_FCI_CAB_FORMAT_LIMIT = "CAB creation failed: Data-size or file-count exceeded CAB format limits.";
    public const uint E_CABAPI_FCI_UNKNOWN = 2149090015;
    public const string STR_E_CABAPI_FCI_UNKNOWN = "CAB creation failed: Unknown error.";
    public const uint E_CABAPI_FDI_CABINET_NOT_FOUND = 2149090017;
    public const string STR_E_CABAPI_FDI_CABINET_NOT_FOUND = "CAB extract failed: Specified cabinet file not found.";
    public const uint E_CABAPI_FDI_NOT_A_CABINET = 2149090018;
    public const string STR_E_CABAPI_FDI_NOT_A_CABINET = "CAB extract failed: Specified file is not a valid cabinet.";
    public const uint E_CABAPI_FDI_UNKNOWN_CABINET_VERSION = 2149090019;
    public const string STR_E_CABAPI_FDI_UNKNOWN_CABINET_VERSION = "CAB extract failed: Specified cabinet has an unknown cabinet version number.";
    public const uint E_CABAPI_FDI_CORRUPT_CABINET = 2149090020;
    public const string STR_E_CABAPI_FDI_CORRUPT_CABINET = "CAB extract failed: Specified cabinet is corrupt.";
    public const uint E_CABAPI_FDI_ALLOC_FAIL = 2149090021;
    public const string STR_E_CABAPI_FDI_ALLOC_FAIL = "CAB extract failed: FDI failed to allocate memory.";
    public const uint E_CABAPI_FDI_BAD_COMPR_TYPE = 2149090022;
    public const string STR_E_CABAPI_FDI_BAD_COMPR_TYPE = "CAB extract failed: Unknown compression type used in cabinet folder.";
    public const uint E_CABAPI_FDI_MDI_FAIL = 2149090023;
    public const string STR_E_CABAPI_FDI_MDI_FAIL = "CAB extract failed: FDI failed to decompress data from cabinet file.";
    public const uint E_CABAPI_FDI_TARGET_FILE = 2149090024;
    public const string STR_E_CABAPI_FDI_TARGET_FILE = "CAB extract failed: Failure writing to target file.";
    public const uint E_CABAPI_FDI_RESERVE_MISMATCH = 2149090025;
    public const string STR_E_CABAPI_FDI_RESERVE_MISMATCH = "CAB extract failed: The cabinets within a set do not have the same RESERVE sizes.";
    public const uint E_CABAPI_FDI_WRONG_CABINET = 2149090026;
    public const string STR_E_CABAPI_FDI_WRONG_CABINET = "CAB extract failed: The cabinet returned by fdintNEXT_CABINET is incorrect.";
    public const uint E_CABAPI_FDI_USER_ABORT = 2149090027;
    public const string STR_E_CABAPI_FDI_USER_ABORT = "CAB extract failed: FDI aborted on user request.";
    public const uint E_CABAPI_FDI_UNKNOWN = 2149090031;
    public const string STR_E_CABAPI_FDI_UNKNOWN = "CAB extract failed: Unknown error.";
    public const string STR_UNKNOWN_ERROR = "CAB operation failed: Unknown error.";
    private Dictionary<uint, string> _map = new Dictionary<uint, string>();
    private const string STR_CABERROR_PREFIX = "E_CABAPI";
    private const string STR_PREFIX = "STR_";
    private const string STR_CABERRORMSG_PREFIX = "STR_E_CABAPI";

    private CabErrorMapper()
    {
      Dictionary<string, uint> dictionary1 = new Dictionary<string, uint>();
      Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
      foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
      {
        if (field.Name.StartsWith("E_CABAPI", StringComparison.OrdinalIgnoreCase))
          dictionary1[field.Name] = (uint) field.GetValue((object) this);
        else if (field.Name.StartsWith("STR_E_CABAPI", StringComparison.OrdinalIgnoreCase))
          dictionary2[field.Name] = field.GetValue((object) this) as string;
      }
      foreach (string key1 in dictionary1.Keys)
      {
        string key2 = "STR_" + key1;
        if (dictionary2.ContainsKey(key2))
          this._map[dictionary1[key1]] = dictionary2[key2];
      }
    }

    public static CabErrorMapper Instance => CabErrorMapper._instance.Value;

    public string MapError(uint hr)
    {
      string empty = string.Empty;
      return this._map == null || !this._map.ContainsKey(hr) ? string.Format("CAB operation failed: Unknown error.", (object) hr) : this._map[hr];
    }
  }
}
