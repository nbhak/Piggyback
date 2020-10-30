// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.ExerciseItem
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using System.Windows.Media.Imaging;

namespace ActivityBrowser
{
  public class ExerciseItem
  {
    public ExerciseItem(
      string title,
      string description,
      BitmapImage image,
      object tag,
      params ExerciseItem[] children)
    {
      this.Title = title;
      this.Description = description;
      this.Image = image;
      this.Tag = tag;
      this.Children = children;
    }

    public string Title { get; set; }

    public string Description { get; set; }

    public BitmapImage Image { get; set; }

    public object Tag { get; set; }

    public ExerciseItem[] Children { get; set; }

    public override string ToString() => this.Title;
  }
}
