// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.SecretAbout
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using ControlLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ActivityBrowser
{
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public partial class SecretAbout : Window, IComponentConnector
  {
    private const string ScrollingMessage = "                         -=Piggyback 3 - Coded by Heffo with lots of Blood, Sweat and NOWHERE near enuf BEERZ! Heffo would like to give SPECIAL THANKZ to Brent Watson (Pennywise) for all the hard werk translating all the menu textz and other conversion werk from PB2. Also a big THANKZ to Valena Robins for all her hard werk reformatting the work sheets and making them look all SEXY!!=-                         -=GREETZ to all those in the C64 scene, for continuing to push such a little workhorse so far beyond the limits, and for who inspired this CRAP ripoff of a hidden screen!=-";
    private TimeSpan lastRender;
    private double bobSinTime;
    private double fovSinTime;
    private Random rng;
    private double velocity = 25.0;
    private string audioPhile;
    private List<ShinyDot> shinyDots;

    public SecretAbout()
    {
      this.InitializeComponent();
      this.rng = (Random) new RandomTS();
      Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ActivityBrowser.Resources.SecretSound.mp3");
      this.audioPhile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".mp3";
      FileStream fileStream = new FileStream(this.audioPhile, FileMode.Create, FileAccess.Write);
      manifestResourceStream.CopyTo((Stream) fileStream);
      fileStream.Flush();
      fileStream.Close();
      fileStream.Dispose();
      this.mediaPlayer.Source = new Uri(this.audioPhile.Replace('\\', '/'), UriKind.Absolute);
      this.thanksScroller.Text = "                         -=Piggyback 3 - Coded by Heffo with lots of Blood, Sweat and NOWHERE near enuf BEERZ! Heffo would like to give SPECIAL THANKZ to Brent Watson (Pennywise) for all the hard werk translating all the menu textz and other conversion werk from PB2. Also a big THANKZ to Valena Robins for all her hard werk reformatting the work sheets and making them look all SEXY!!=-                         -=GREETZ to all those in the C64 scene, for continuing to push such a little workhorse so far beyond the limits, and for who inspired this CRAP ripoff of a hidden screen!=-";
      this.thanksScroller.ScrollSpeed = 100.0;
      this.thanksScroller.ScrollCompleted += new EventHandler(this.thanksScroller_ScrollCompleted);
      this.thanksScroller.Reset();
      this.thanksScroller.Start();
      this.lastRender = TimeSpan.FromTicks(DateTime.Now.Ticks);
      CompositionTarget.Rendering += new EventHandler(this.CompositionTarget_Rendering);
      this.mediaPlayer.Position = TimeSpan.Zero;
      this.mediaPlayer.Play();
    }

    private void CompositionTarget_Rendering(object sender, EventArgs e)
    {
      RenderingEventArgs renderingEventArgs = (RenderingEventArgs) e;
      double totalSeconds = (renderingEventArgs.RenderingTime - this.lastRender).TotalSeconds;
      this.lastRender = renderingEventArgs.RenderingTime;
      this.BobScroller(totalSeconds);
      this.RotateCube(totalSeconds);
      this.TweakFov(totalSeconds);
    }

    private void RotateCube(double deltaTime)
    {
      this.rotX.Angle += deltaTime * 20.0;
      this.rotY.Angle += deltaTime * 10.0;
      this.rotZ.Angle += deltaTime * 5.0;
    }

    private void BobScroller(double deltaTime)
    {
      double num = (this.ActualHeight - this.thanksScroller.ActualHeight) * 0.5 - this.thanksScroller.ActualHeight * 0.5;
      this.bobSinTime += deltaTime * 3.0;
      if (this.bobSinTime > 1.0)
        --this.bobSinTime;
      this.thanksScroller.Margin = new Thickness(0.0, num + Math.Sin(this.bobSinTime) * 50.0, 0.0, 0.0);
      this.UpdateLayout();
    }

    private void TweakFov(double deltaTime)
    {
      double num = 70.0;
      this.fovSinTime += deltaTime * 0.5;
      if (this.fovSinTime > 1.0)
        --this.fovSinTime;
      this.cameraControl.FieldOfView = num + Math.Sin(this.fovSinTime) * 20.0;
    }

    private void thanksScroller_ScrollCompleted(object sender, EventArgs e)
    {
      this.thanksScroller.Reset();
      this.thanksScroller.Start();
    }

    private void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
    {
      this.mediaPlayer.Position = TimeSpan.Zero;
      this.mediaPlayer.Play();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      this.mediaPlayer.Stop();
      File.Delete(this.audioPhile);
    }
  }
}
