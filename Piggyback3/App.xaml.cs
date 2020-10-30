// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.App
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;

namespace ActivityBrowser
{
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public partial class App : Application
  {
    private bool _contentLoaded;

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      this.StartupUri = new Uri("BrowserForm.xaml", UriKind.Relative);
      Application.LoadComponent((object) this, new Uri("/Piggyback3;component/app.xaml", UriKind.Relative));
    }

    [STAThread]
    [DebuggerNonUserCode]
    public static void Main()
    {
      App app = new App();
      app.InitializeComponent();
      app.Run();
    }
  }
}
