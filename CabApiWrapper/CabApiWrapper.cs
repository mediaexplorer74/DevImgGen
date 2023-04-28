// Decompiled with JetBrains decompiler
// Type: Microsoft.WindowsPhone.ImageUpdate.Tools.CabApiWrapper
// Assembly: CabApiWrapper, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 866557A1-48F3-47A0-86C4-323A9E15F833
// Assembly location: C:\Users\Admin\Desktop\re\dig\CabApiWrapper.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools
{
  public class CabApiWrapper
  {
    private const string STR_COMMA = ",";

    private CabApiWrapper()
    {
    }

    public static void Extract(string filename, string outputDir)
    {
      filename = !string.IsNullOrEmpty(filename) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(filename) : throw new ArgumentNullException(nameof (filename));
      outputDir = !string.IsNullOrEmpty(outputDir) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(outputDir) : throw new ArgumentNullException(nameof (outputDir));
      uint cabHR = Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.Exists(filename) ? CabApiWrapper.NativeMethods.Cab_Extract(filename, outputDir) : throw new FileNotFoundException(string.Format("CAB file {0} not found", (object) filename), filename);
      if (cabHR != 0U)
        throw new CabException(cabHR, "Cab_Extract", new string[2]
        {
          filename,
          outputDir
        });
    }

    public static void ExtractOne(string filename, string outputDir, string fileToExtract)
    {
      filename = !string.IsNullOrEmpty(filename) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(filename) : throw new ArgumentNullException(nameof (filename));
      outputDir = !string.IsNullOrEmpty(outputDir) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(outputDir) : throw new ArgumentNullException(nameof (outputDir));
      if (string.IsNullOrEmpty(fileToExtract))
        throw new ArgumentNullException(nameof (fileToExtract));
      if (!Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.Exists(filename))
        throw new FileNotFoundException(string.Format("CAB file {0} not found", (object) filename), filename);
      uint one = CabApiWrapper.NativeMethods.Cab_ExtractOne(filename, outputDir, fileToExtract);
      if (one != 0U)
        throw new CabException(one, "Cab_ExtractOne", new string[3]
        {
          filename,
          outputDir,
          fileToExtract
        });
    }

    public static void ExtractSelected(
      string filename,
      string outputDir,
      IEnumerable<string> filesToExtract)
    {
      filename = !string.IsNullOrEmpty(filename) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(filename) : throw new ArgumentNullException(nameof (filename));
      outputDir = !string.IsNullOrEmpty(outputDir) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(outputDir) : throw new ArgumentNullException(nameof (outputDir));
      string[] filesToExtract1 = filesToExtract != null ? filesToExtract.ToArray<string>() : throw new ArgumentNullException(nameof (filesToExtract));
      uint length = (uint) filesToExtract1.Length;
      if (length == 0U)
        throw new ArgumentException("Parameter 'filesToExtract' cannot be empty");
      if (!Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.Exists(filename))
        throw new FileNotFoundException(string.Format("CAB file {0} not found", (object) filename), filename);
      uint selected = CabApiWrapper.NativeMethods.Cab_ExtractSelected(filename, outputDir, filesToExtract1, length);
      if (selected != 0U)
        throw new CabException(selected, "Cab_ExtractSelected", new string[3]
        {
          filename,
          outputDir,
          string.Join(",", filesToExtract1)
        });
    }

    public static void ExtractSelected(
      string filename,
      IEnumerable<string> filesToExtract,
      IEnumerable<string> targetPaths)
    {
      filename = !string.IsNullOrEmpty(filename) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(filename) : throw new ArgumentNullException(nameof (filename));
      if (filesToExtract == null)
        throw new ArgumentNullException(nameof (filesToExtract));
      if (targetPaths == null)
        throw new ArgumentNullException(nameof (targetPaths));
      string[] array1 = filesToExtract.ToArray<string>();
      string[] array2 = targetPaths.ToArray<string>();
      uint length1 = (uint) array1.Length;
      uint length2 = (uint) array2.Length;
      if (length1 == 0U)
        throw new ArgumentException("'filesToExtract' parameter cannot be empty");
      if (length2 == 0U)
        throw new ArgumentException("'targetPaths' parameter cannot be empty");
      if ((int) length1 != (int) length2)
        throw new ArgumentException("'filesToExtract' and 'targetPaths' should have the same number of elements");
      uint selected = CabApiWrapper.NativeMethods.Cab_ExtractSelected(filename, array1, length1, array2, length2);
      if (selected != 0U)
        throw new CabException(selected, "Cab_ExtractSelected", new string[3]
        {
          filename,
          string.Join(",", array1),
          string.Join(",", array2)
        });
    }

    public static string[] GetFileList(string filename, out ulong[] fileSizes)
    {
      filename = !string.IsNullOrEmpty(filename) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(filename) : throw new ArgumentNullException(nameof (filename));
      if (!Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.Exists(filename))
        throw new FileNotFoundException(string.Format("CAB file {0} not found", (object) filename), filename);
      IntPtr fileList1 = IntPtr.Zero;
      IntPtr sizeList = IntPtr.Zero;
      uint cFileList = 0;
      string[] fileList2;
      try
      {
        uint fileSizeList = CabApiWrapper.NativeMethods.Cab_GetFileSizeList(filename, out fileList1, out sizeList, out cFileList);
        if (fileSizeList != 0U)
          throw new CabException(fileSizeList, "Cab_GetFileSizeList", new string[1]
          {
            filename
          });
        fileSizes = new ulong[(int) cFileList];
        long[] destination1 = new long[(int) cFileList];
        Marshal.Copy(sizeList, destination1, 0, (int) cFileList);
        fileSizes = (ulong[]) destination1;
        IntPtr[] destination2 = new IntPtr[(int) cFileList];
        Marshal.Copy(fileList1, destination2, 0, (int) cFileList);
        fileList2 = new string[(int) cFileList];
        for (int index = 0; (long) index < (long) cFileList; ++index)
          fileList2[index] = Marshal.PtrToStringUni(destination2[index]);
      }
      finally
      {
        if (fileList1 != IntPtr.Zero && cFileList > 0U)
        {
          uint cabHR = CabApiWrapper.NativeMethods.Cab_FreeFileSizeList(fileList1, sizeList, cFileList);
          if (cabHR != 0U)
            throw new CabException(cabHR, "Cab_FreeFileSizeList", new string[1]
            {
              filename
            });
        }
      }
      return fileList2;
    }

    public static string[] GetFileList(string filename) => CabApiWrapper.GetFileList(filename, out ulong[] _);

    public static bool IsCabinet(string filename)
    {
      filename = !string.IsNullOrEmpty(filename) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(filename) : throw new ArgumentNullException(nameof (filename));
      bool isCabinet;
      uint cabHR = Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.Exists(filename) ? CabApiWrapper.NativeMethods.Cab_CheckIsCabinet(filename, out isCabinet) : throw new FileNotFoundException(string.Format("CAB file {0} not found", (object) filename), filename);
      if (cabHR != 0U)
        throw new CabException(cabHR, "Cab_CheckIsCabinet", new string[1]
        {
          filename
        });
      return isCabinet;
    }

    public static void CreateCab(
      string filename,
      string rootDirectory,
      string tempWorkingFolder,
      string filterToSelectFiles)
    {
      CabApiWrapper.CreateCab(filename, rootDirectory, tempWorkingFolder, filterToSelectFiles, CompressionType.None);
    }

    public static void CreateCab(
      string filename,
      string rootDirectory,
      string tempWorkingFolder,
      string filterToSelectFiles,
      CompressionType compressionType)
    {
      filename = !string.IsNullOrEmpty(filename) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(filename) : throw new ArgumentNullException(nameof (filename));
      rootDirectory = !string.IsNullOrEmpty(rootDirectory) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(rootDirectory) : throw new ArgumentNullException(nameof (rootDirectory));
      tempWorkingFolder = !string.IsNullOrEmpty(tempWorkingFolder) ? Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPath.GetFullPathUNC(tempWorkingFolder) : throw new ArgumentNullException(nameof (tempWorkingFolder));
      if (!Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathDirectory.Exists(rootDirectory))
        throw new DirectoryNotFoundException(string.Format("'rootDirectory' folder {0} does not exist", (object) rootDirectory));
      if (!Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathDirectory.Exists(tempWorkingFolder))
        throw new DirectoryNotFoundException(string.Format("'tempWorkingFolder' folder {0} does not exist", (object) tempWorkingFolder));
      uint cab = CabApiWrapper.NativeMethods.Cab_CreateCab(filename, rootDirectory, tempWorkingFolder, filterToSelectFiles, compressionType);
      if (cab != 0U)
        throw new CabException(cab, "Cab_CreateCab", new string[4]
        {
          filename,
          rootDirectory,
          tempWorkingFolder,
          filterToSelectFiles
        });
    }

    public static void CreateCabSelected(
      string filename,
      string[] files,
      string tempWorkingFolder,
      string prefixToTrim)
    {
      CabApiWrapper.CreateCabSelected(filename, files, tempWorkingFolder, prefixToTrim, CompressionType.None);
    }

    public static void CreateCabSelected(
      string filename,
      string[] files,
      string tempWorkingFolder,
      string prefixToTrim,
      CompressionType compressionType)
    {
      if (string.IsNullOrEmpty(filename))
        throw new ArgumentNullException(nameof (filename));
      if (files == null)
        throw new ArgumentNullException(nameof (files));
      if (string.IsNullOrEmpty(tempWorkingFolder))
        throw new ArgumentNullException(nameof (tempWorkingFolder));
      if (files.Length == 0)
        throw new ArgumentException("'files' parameter cannot be empty");
      if (!Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathDirectory.Exists(tempWorkingFolder))
        throw new DirectoryNotFoundException(string.Format("'tempWorkingFolder' folder {0} does not exist", (object) tempWorkingFolder));
      string[] array = ((IEnumerable<string>) files).Where<string>((Func<string, bool>) (x => !Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.Exists(x))).ToArray<string>();
      if (array.Length != 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("Error when adding files to cab file '{0}'. The following files specified in 'files' don't exist:", (object) filename);
        stringBuilder.AppendLine();
        foreach (string str in array)
        {
          stringBuilder.AppendFormat("\t{0}", (object) str);
          stringBuilder.AppendLine();
        }
        throw new FileNotFoundException(stringBuilder.ToString());
      }
      uint cabSelected = CabApiWrapper.NativeMethods.Cab_CreateCabSelected(filename, files, (uint) files.Length, (string[]) null, 0U, tempWorkingFolder, prefixToTrim, compressionType);
      if (cabSelected != 0U)
        throw new CabException(cabSelected, "Cab_CreateCabSelected", new string[3]
        {
          filename,
          tempWorkingFolder,
          prefixToTrim
        });
    }

    public static void CreateCabSelected(
      string filename,
      string[] sourceFiles,
      string[] targetFiles,
      string tempWorkingFolder)
    {
      CabApiWrapper.CreateCabSelected(filename, sourceFiles, targetFiles, tempWorkingFolder, CompressionType.None);
    }

    public static void CreateCabSelected(
      string filename,
      string[] sourceFiles,
      string[] targetFiles,
      string tempWorkingFolder,
      CompressionType compressionType)
    {
      if (string.IsNullOrEmpty(filename))
        throw new ArgumentNullException(nameof (filename));
      if (sourceFiles == null)
        throw new ArgumentNullException(nameof (sourceFiles));
      if (targetFiles == null)
        throw new ArgumentNullException(nameof (targetFiles));
      if (string.IsNullOrEmpty(tempWorkingFolder))
        throw new ArgumentNullException(nameof (tempWorkingFolder));
      if (sourceFiles.Length == 0)
        throw new ArgumentException("'sourceFiles' parameter cannot be empty");
      if (targetFiles.Length == 0)
        throw new ArgumentException("'targetFiles' parameter cannot be empty");
      if (sourceFiles.Length != targetFiles.Length)
        throw new ArgumentException("'sourceFiles' and 'targetFiles' should have the same number of elements");
      if (!Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathDirectory.Exists(tempWorkingFolder))
        throw new DirectoryNotFoundException(string.Format("'tempWorkingFolder' folder {0} does not exist", (object) tempWorkingFolder));
      string[] array = ((IEnumerable<string>) sourceFiles).Where<string>((Func<string, bool>) (x => !Microsoft.WindowsPhone.ImageUpdate.Tools.Common.LongPathFile.Exists(x))).ToArray<string>();
      if (array.Length != 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("Error when adding files to cab file '{0}'. The following files specified in 'sourceFiles' don't exist:", (object) filename);
        stringBuilder.AppendLine();
        foreach (string str in array)
        {
          stringBuilder.AppendFormat("\t{0}", (object) str);
          stringBuilder.AppendLine();
        }
        throw new FileNotFoundException(stringBuilder.ToString());
      }
      uint cabSelected = CabApiWrapper.NativeMethods.Cab_CreateCabSelected(filename, sourceFiles, (uint) sourceFiles.Length, targetFiles, (uint) targetFiles.Length, tempWorkingFolder, (string) null, compressionType);
      if (cabSelected != 0U)
        throw new CabException(cabSelected, "Cab_CreateCabSelected", new string[2]
        {
          filename,
          tempWorkingFolder
        });
    }

    internal sealed class NativeMethods
    {
      private const string STRING_CABAPI_DLL = "CabApi.dll";
      private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;
      private const bool SET_LAST_ERROR = true;
      private const CharSet CHAR_SET = CharSet.Auto;

      private NativeMethods()
      {
      }

      [DllImport("CabApi.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
      public static extern uint Cab_Extract(string filename, string outputDir);

      [DllImport("CabApi.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
      public static extern uint Cab_ExtractOne(
        string filename,
        string outputDir,
        string fileToExtract);

      [DllImport("CabApi.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
      public static extern uint Cab_ExtractSelected(
        string filename,
        string outputDir,
        string[] filesToExtract,
        uint cFilesToExtract);

      [DllImport("CabApi.dll", EntryPoint = "Cab_ExtractSelectedToTarget", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
      public static extern uint Cab_ExtractSelected(
        string filename,
        string[] filesToExtract,
        uint cFilesToExtract,
        string[] targetPaths,
        uint cTargetPaths);

      [DllImport("CabApi.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
      public static extern uint Cab_GetFileSizeList(
        string filename,
        out IntPtr fileList,
        out IntPtr sizeList,
        out uint cFileList);

      [DllImport("CabApi.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
      public static extern uint Cab_FreeFileSizeList(
        IntPtr fileList,
        IntPtr sizeList,
        uint cFileList);

      [DllImport("CabApi.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
      public static extern uint Cab_GetFileList(
        string filename,
        out IntPtr fileList,
        out uint cFileList);

      [DllImport("CabApi.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
      public static extern uint Cab_FreeFileList(IntPtr fileList, uint cFileList);

      [DllImport("CabApi.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
      public static extern uint Cab_CheckIsCabinet(string filename, out bool isCabinet);

      [DllImport("CabApi.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
      public static extern uint Cab_CreateCab(
        string filename,
        string rootDirectory,
        string tempWorkingFolder,
        string filterToSelectFiles,
        CompressionType compressionType);

      [DllImport("CabApi.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
      public static extern uint Cab_CreateCabSelected(
        string filename,
        string[] files,
        uint cFiles,
        string[] targetfiles,
        uint cTargetFiles,
        string tempWorkingFolder,
        string prefixToTrim,
        CompressionType compressionType);
    }
  }
}
