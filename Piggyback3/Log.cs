// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.Log
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading;

namespace ActivityBrowser
{
  internal static class Log
  {
    private static Thread workerThread;
    private static MemoryStream memoryStore;
    private static GZipStream zipStream;
    private static StreamWriter memoryStream;
    private static StreamWriter fileStream;
    private static Queue<string> logQueue;
    private static bool _keepRunning;
    private static object _keepRunningLock = new object();

    private static bool KeepRunning
    {
      get
      {
        lock (Log._keepRunningLock)
          return Log._keepRunning;
      }
      set
      {
        lock (Log._keepRunningLock)
          Log._keepRunning = value;
      }
    }

    static Log()
    {
      Log.LogLevel = LogLevel.Error;
      Log.logQueue = new Queue<string>();
      Log.OpenLog();
      Log.KeepRunning = true;
      Log.workerThread = new Thread(new ThreadStart(Log.LogWorker_Main));
      Log.workerThread.Name = "Log Worker";
      Log.workerThread.IsBackground = true;
      Log.workerThread.Start();
    }

    private static void LogWorker_Main()
    {
      try
      {
        while (Log.KeepRunning)
        {
          string str = (string) null;
          lock (Log.logQueue)
          {
            if (Log.logQueue.Count > 0)
              str = Log.logQueue.Dequeue();
          }
          if (str == null)
            Thread.Sleep(200);
          else
            Log.memoryStream.WriteLine(str);
        }
        lock (Log.logQueue)
        {
          while (Log.logQueue.Count > 0)
            Log.memoryStream.WriteLine(Log.logQueue.Dequeue());
        }
        Log.KeepRunning = true;
      }
      catch (Exception ex)
      {
      }
    }

    private static void OpenLog()
    {
      Log.memoryStore = new MemoryStream();
      Log.zipStream = new GZipStream((Stream) Log.memoryStore, CompressionMode.Compress);
      Log.memoryStream = new StreamWriter((Stream) Log.zipStream);
      foreach (string commandLineArg in Environment.GetCommandLineArgs())
      {
        if (commandLineArg.Equals("-enableLog"))
        {
          Log.fileStream = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Piggyback 3 Log.txt");
          Log.fileStream.AutoFlush = true;
        }
        if (commandLineArg.StartsWith("-logLevel="))
        {
          int result = 0;
          Log.LogLevel = int.TryParse(commandLineArg.Substring(10), out result) ? (LogLevel) result : LogLevel.Warning;
        }
      }
    }

    public static LogLevel LogLevel { get; set; }

    public static void Write(
      LogLevel level,
      string moduleName,
      string className,
      string logMessage)
    {
      string str = string.Format("{0} (Thread '{1}')-({2} : {3}) {4}", (object) level.ToString(), (object) Thread.CurrentThread.Name, (object) moduleName, (object) className, (object) logMessage);
      lock (Log.logQueue)
        Log.logQueue.Enqueue(str);
      if (Log.fileStream == null || level < Log.LogLevel)
        return;
      lock (Log.fileStream)
        Log.fileStream.WriteLine(str);
    }

    public static void Write(LogLevel level, string className, string logMessage) => Log.Write(level, Assembly.GetExecutingAssembly().ManifestModule.Name, className, logMessage);

    public static void CloseLog()
    {
      try
      {
        Log.KeepRunning = false;
        Log.workerThread.Join();
        if (Log.fileStream != null)
        {
          Log.fileStream.Flush();
          Log.fileStream.Close();
          Log.fileStream.Dispose();
          Log.fileStream = (StreamWriter) null;
        }
        if (!Log.KeepRunning)
          return;
        Log.memoryStream.Flush();
        Log.memoryStream.Close();
        Log.zipStream.Flush();
        Log.zipStream.Close();
        Log.memoryStore.Flush();
        Log.memoryStore.Close();
        Log.memoryStream.Dispose();
        Log.zipStream.Dispose();
        Log.memoryStore.Dispose();
        Log.memoryStore = (MemoryStream) null;
        Log.zipStream = (GZipStream) null;
        Log.memoryStream = (StreamWriter) null;
      }
      catch (Exception ex)
      {
      }
    }

    public static void DumpLog(string filename)
    {
      try
      {
        Log.KeepRunning = false;
        Log.workerThread.Join();
        if (Log.fileStream != null)
        {
          Log.fileStream.Flush();
          Log.fileStream.Close();
          Log.fileStream.Dispose();
          Log.fileStream = (StreamWriter) null;
        }
        if (!Log.KeepRunning)
          return;
        Log.memoryStream.Flush();
        Log.zipStream.Close();
        Log.zipStream.Dispose();
        FileStream fileStream = new FileStream(filename, FileMode.Create);
        byte[] buffer = Log.memoryStore.GetBuffer();
        fileStream.Write(buffer, 0, buffer.Length);
        fileStream.Close();
      }
      catch (Exception ex)
      {
      }
    }
  }
}
