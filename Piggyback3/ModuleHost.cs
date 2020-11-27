// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.ModuleHost
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using ModuleInterface.V1;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;

namespace ActivityBrowser
{
  internal class ModuleHost
  {
    private AggregateCatalog _masterCatalog;
    private CompositionContainer _compContainer;
    private CompositionBatch _compBatch;
    private ModuleHost.ModuleExports _exports;

    [ImportMany(typeof (IModule_V1))]
    public IList<IModule_V1> Modules { get; set; }

    public ModuleHost(string moduleDirectory)
    {
      Log.Write(LogLevel.Information, "ModuleHost.ModuleHost", "Loading Modules..");
      try
      {
        this.Modules = (IList<IModule_V1>) new List<IModule_V1>();
        this._masterCatalog = new AggregateCatalog();
        this.AddModuleDirectory(moduleDirectory);
        this._compBatch = new CompositionBatch();
        this._compBatch.AddPart((object) this);
        this._compContainer = new CompositionContainer((ComposablePartCatalog) this._masterCatalog, new ExportProvider[0]);
        this._compContainer.Compose(this._compBatch);
        this._exports = new ModuleHost.ModuleExports();
        foreach (IModule_V1 moduleV1 in new List<IModule_V1>((IEnumerable<IModule_V1>) this.Modules))
        {
          Log.Write(LogLevel.Information, "ModuleHost.ModuleHost", string.Format("Initialising Module '{0}'", (object) moduleV1.Name));
          try
          {
            moduleV1.InitModule((IModuleHost_V1) this._exports);
            Log.Write(LogLevel.Information, "ModuleHost.ModuleHost", string.Format("Initialising Module '{0}' Complete", (object) moduleV1.Name));
          }
          catch (Exception ex)
          {
            Log.Write(LogLevel.Warning, "ModuleHost.ModuleHost", ex.ToString());
            this.Modules.Remove(moduleV1);
          }
        }
      }
      catch (Exception ex)
      {
        Log.Write(LogLevel.Error, "ModuleHost.ModuleHost", ex.ToString());
      }
    }

    public void AddModuleDirectory(string path)
    {
      Log.Write(LogLevel.Information, "ModuleHost.AddModuleDirectory", string.Format("Adding directory '{0}'", (object) path));
      if (Directory.Exists(path))
      {
        string[] files = Directory.GetFiles(path, "*.dll");
        if (files.Length == 0)
        {
          Log.Write(LogLevel.Warning, "ModuleHost.AddModuleDirectory", string.Format("No dll files found in directory '{0}'", (object) path));
        }
        else
        {
          foreach (object obj in files)
            Log.Write(LogLevel.Information, "ModuleHost.AddModuleDirectory", string.Format("Found dll file '{0}'", obj));
        }
        this._masterCatalog.Catalogs.Add((ComposablePartCatalog) new DirectoryCatalog(path));
      }
      else
      {
        Log.Write(LogLevel.Warning, "ModuleHost.AddModuleDirectory", string.Format("Failed to add directory '{0}', directory not found.", (object) path));
        throw new DirectoryNotFoundException();
      }
    }

    private class ModuleExports : IModuleHost_V1
    {
      private RandomTS rng;

      public void Log(
        ELogLevel_V1 logLevel,
        string moduleName,
        string className,
        string logMessage)
      {
        ActivityBrowser.Log.Write((LogLevel) logLevel, moduleName, className, logMessage);
      }

      public void ReportException(string moduleName, string className, Exception e)
      {
      }

      public bool UseHighQualityEffects => true;

      public Random RandomGenerator
      {
        get
        {
          if (this.rng == null)
            this.rng = new RandomTS();
          return (Random) this.rng;
        }
      }
    }
  }
}
