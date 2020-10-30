// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.RandomTS
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using System;

namespace ActivityBrowser
{
  public class RandomTS : Random
  {
    private MersenneTwister rng;

    public RandomTS() => this.rng = new MersenneTwister();

    public RandomTS(int seed) => this.rng = new MersenneTwister(seed);

    public override int Next()
    {
      lock (this.rng)
        return this.rng.Next();
    }

    public override int Next(int maxValue)
    {
      lock (this.rng)
        return this.rng.Next(maxValue);
    }

    public override int Next(int minValue, int maxValue)
    {
      lock (this.rng)
        return this.rng.Next(minValue, maxValue);
    }

    public override void NextBytes(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException();
      lock (this.rng)
        this.rng.NextBytes(buffer);
    }

    public override double NextDouble()
    {
      lock (this.rng)
        return this.rng.NextDouble();
    }
  }
}
