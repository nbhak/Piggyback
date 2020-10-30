// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.Properties.Resources
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ActivityBrowser.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) ActivityBrowser.Properties.Resources.resourceMan, (object) null))
          ActivityBrowser.Properties.Resources.resourceMan = new ResourceManager("ActivityBrowser.Properties.Resources", typeof (ActivityBrowser.Properties.Resources).Assembly);
        return ActivityBrowser.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => ActivityBrowser.Properties.Resources.resourceCulture;
      set => ActivityBrowser.Properties.Resources.resourceCulture = value;
    }
  }
}
