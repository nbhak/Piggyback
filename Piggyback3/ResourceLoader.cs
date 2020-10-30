// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.ResourceLoader
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using System;
using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Windows.Xps.Packaging;

namespace ActivityBrowser
{
  internal class ResourceLoader
  {
    private static ResourceLoader defaultLoader = new ResourceLoader();

    public static ResourceLoader loader => ResourceLoader.defaultLoader;

    public string[] ResourceList() => Assembly.GetExecutingAssembly().GetManifestResourceNames();

    public XpsDocument LoadXPF(string docName)
    {
      bool flag = false;
      foreach (string resource in this.ResourceList())
      {
        if (string.Equals(docName, resource))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        Log.Write(LogLevel.Warning, "ResourceLoader.LoadXPF", string.Format("Resource '{0}' not found.", (object) docName));
        throw new FileNotFoundException(string.Format("Resource '{0}' Not Found.", (object) docName));
      }
      try
      {
        string str = string.Format("memorystream://{0}", (object) docName);
        Uri uri = new Uri(str);
        Package package = PackageStore.GetPackage(uri);
        if (package == null)
        {
          Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(docName);
          int length = (int) manifestResourceStream.Length;
          MemoryStream memoryStream = new MemoryStream();
          memoryStream.Capacity = length;
          byte[] buffer = new byte[length];
          manifestResourceStream.Read(buffer, 0, length);
          memoryStream.Write(buffer, 0, length);
          package = Package.Open((Stream) memoryStream);
          PackageStore.AddPackage(uri, package);
          Log.Write(LogLevel.Information, "ResourceLoader.LoadXPF", string.Format("Added '{0}' to the package store", (object) docName));
        }
        else
          Log.Write(LogLevel.Information, "ResourceLoader.LoadXPF", string.Format("Loaded '{0}' from package store", (object) docName));
        return new XpsDocument(package, CompressionOption.Maximum, str);
      }
      catch (Exception ex)
      {
        throw new Exception("ResourceLoader.LoadXPF", ex);
      }
    }
  }
}
