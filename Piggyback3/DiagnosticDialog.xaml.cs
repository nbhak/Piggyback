// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.DiagnosticDialog
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace ActivityBrowser
{
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public partial class DiagnosticDialog : Window, IComponentConnector
  {
    internal Button btnCancel;
    internal Button btnOk;
    internal Label label1;
    internal TextBox txtDiagCode;
    private bool _contentLoaded;

    public DiagnosticDialog() => this.InitializeComponent();

    private void btnCancel_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(false);

    private void btnOk_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(true);

    public string DiagnosticCode => this.txtDiagCode.Text;

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Piggyback3;component/diagnosticdialog.xaml", UriKind.Relative));
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerNonUserCode]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.btnCancel = (Button) target;
          this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
          break;
        case 2:
          this.btnOk = (Button) target;
          this.btnOk.Click += new RoutedEventHandler(this.btnOk_Click);
          break;
        case 3:
          this.label1 = (Label) target;
          break;
        case 4:
          this.txtDiagCode = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
