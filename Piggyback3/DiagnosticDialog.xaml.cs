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
    public DiagnosticDialog() => this.InitializeComponent();

    private void btnCancel_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(false);

    private void btnOk_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(true);

    public string DiagnosticCode => this.txtDiagCode.Text;
  }
}
