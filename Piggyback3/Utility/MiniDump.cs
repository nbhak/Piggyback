// Decompiled with JetBrains decompiler
// Type: Utility.MiniDump
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Utility
{
  public static class MiniDump
  {
    private static int mDumpError;
    private static MiniDump.MakeDumpArgs mArgs;
    private static MiniDump.MinidumpExceptionInfo mMei;

    public static bool TryDump(string dmpPath, MiniDumpType dmpType)
    {
      ActivityBrowser.Log.Write(ActivityBrowser.LogLevel.Information, "MiniDump.TryDump", "Attempting to make mini-dump file '" + dmpPath + "'");
      MiniDump.mArgs.path = dmpPath;
      MiniDump.mArgs.type = dmpType;
      MiniDump.mMei.ThreadId = MiniDump.GetCurrentThreadId();
      MiniDump.mMei.ExceptionPointers = Marshal.GetExceptionPointers();
      MiniDump.mMei.ClientPointers = false;
      Thread thread = new Thread(new ThreadStart(MiniDump.MakeDump));
      thread.Name = "MakeDump";
      thread.Start();
      thread.Join();
      return MiniDump.mDumpError == 0;
    }

    private static void MakeDump()
    {
      using (FileStream fileStream = new FileStream(MiniDump.mArgs.path, FileMode.Create))
      {
        ActivityBrowser.Log.Write(ActivityBrowser.LogLevel.Information, "MiniDump.MakeDump", "Writing mini-dump");
        Process currentProcess = Process.GetCurrentProcess();
        IntPtr num = Marshal.AllocHGlobal(Marshal.SizeOf((object) MiniDump.mMei));
        Marshal.StructureToPtr((object) MiniDump.mMei, num, false);
        MiniDump.mDumpError = MiniDump.MiniDumpWriteDump(currentProcess.Handle, currentProcess.Id, fileStream.SafeFileHandle.DangerousGetHandle(), MiniDump.mArgs.type, MiniDump.mMei.ClientPointers ? num : IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) ? 0 : Marshal.GetLastWin32Error();
        Marshal.FreeHGlobal(num);
      }
    }

    [DllImport("DbgHelp.dll", SetLastError = true)]
    private static extern bool MiniDumpWriteDump(
      IntPtr hProcess,
      int processId,
      IntPtr fileHandle,
      MiniDumpType dumpType,
      IntPtr excepInfo,
      IntPtr userInfo,
      IntPtr extInfo);

    [DllImport("kernel32.dll")]
    private static extern int GetCurrentThreadId();

    private struct MakeDumpArgs
    {
      public string path;
      public MiniDumpType type;
    }

    private struct MinidumpExceptionInfo
    {
      public int ThreadId;
      public IntPtr ExceptionPointers;
      public bool ClientPointers;
    }
  }
}
