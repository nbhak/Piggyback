// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.MersenneTwister
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using System;

namespace ActivityBrowser
{
  public class MersenneTwister : Random
  {
    private const int N = 624;
    private const int M = 397;
    private const uint MatrixA = 2567483615;
    private const uint UpperMask = 2147483648;
    private const uint LowerMask = 2147483647;
    private const uint TemperingMaskB = 2636928640;
    private const uint TemperingMaskC = 4022730752;
    private const double FiftyThreeBitsOf1s = 9.00719925474099E+15;
    private const double Inverse53BitsOf1s = 1.11022302462516E-16;
    private const double OnePlus53BitsOf1s = 9.00719925474099E+15;
    private const double InverseOnePlus53BitsOf1s = 1.11022302462516E-16;
    private readonly uint[] _mt = new uint[624];
    private short _mti;
    private static readonly uint[] _mag01 = new uint[2]
    {
      0U,
      2567483615U
    };

    public MersenneTwister(int seed) => this.init((uint) seed);

    public MersenneTwister()
      : this(new Random().Next())
    {
    }

    public MersenneTwister(int[] initKey)
    {
      uint[] key = initKey != null ? new uint[initKey.Length] : throw new ArgumentNullException(nameof (initKey));
      for (int index = 0; index < initKey.Length; ++index)
        key[index] = (uint) initKey[index];
      this.init(key);
    }

    [CLSCompliant(false)]
    public virtual uint NextUInt32() => this.GenerateUInt32();

    [CLSCompliant(false)]
    public virtual uint NextUInt32(uint maxValue) => (uint) ((double) this.GenerateUInt32() / ((double) uint.MaxValue / (double) maxValue));

    [CLSCompliant(false)]
    public virtual uint NextUInt32(uint minValue, uint maxValue)
    {
      if (minValue >= maxValue)
        throw new ArgumentOutOfRangeException();
      return (uint) ((double) this.GenerateUInt32() / ((double) uint.MaxValue / (double) (maxValue - minValue)) + (double) minValue);
    }

    public override int Next() => this.Next(int.MaxValue);

    public override int Next(int maxValue)
    {
      if (maxValue > 1)
        return (int) (this.NextDouble() * (double) maxValue);
      if (maxValue < 0)
        throw new ArgumentOutOfRangeException();
      return 0;
    }

    public override int Next(int minValue, int maxValue)
    {
      if (maxValue <= minValue)
        throw new ArgumentOutOfRangeException();
      return maxValue == minValue ? minValue : this.Next(maxValue - minValue) + minValue;
    }

    public override void NextBytes(byte[] buffer)
    {
      int num = buffer != null ? buffer.Length : throw new ArgumentNullException();
      for (int index = 0; index < num; ++index)
        buffer[index] = (byte) this.Next(256);
    }

    public override double NextDouble() => this.compute53BitRandom(0.0, 1.11022302462516E-16);

    public double NextDouble(bool includeOne) => !includeOne ? this.NextDouble() : this.compute53BitRandom(0.0, 1.11022302462516E-16);

    public double NextDoublePositive() => this.compute53BitRandom(0.5, 1.11022302462516E-16);

    public float NextSingle() => (float) this.NextDouble();

    public float NextSingle(bool includeOne) => (float) this.NextDouble(includeOne);

    public float NextSinglePositive() => (float) this.NextDoublePositive();

    [CLSCompliant(false)]
    protected uint GenerateUInt32()
    {
      if (this._mti >= (short) 624)
      {
        short num1;
        for (num1 = (short) 0; num1 < (short) 227; ++num1)
        {
          uint num2 = (uint) ((int) this._mt[(int) num1] & int.MinValue | (int) this._mt[(int) num1 + 1] & int.MaxValue);
          this._mt[(int) num1] = this._mt[(int) num1 + 397] ^ num2 >> 1 ^ MersenneTwister._mag01[(num2 & 1U)];
        }
        for (; num1 < (short) 623; ++num1)
        {
          uint num2 = (uint) ((int) this._mt[(int) num1] & int.MinValue | (int) this._mt[(int) num1 + 1] & int.MaxValue);
          this._mt[(int) num1] = this._mt[(int) num1 - 227] ^ num2 >> 1 ^ MersenneTwister._mag01[(num2 & 1U)];
        }
        uint num3 = (uint) ((int) this._mt[623] & int.MinValue | (int) this._mt[0] & int.MaxValue);
        this._mt[623] = this._mt[396] ^ num3 >> 1 ^ MersenneTwister._mag01[(num3 & 1U)];
        this._mti = (short) 0;
      }
      uint y1 = this._mt[(int) this._mti++];
      uint y2 = y1 ^ MersenneTwister.temperingShiftU(y1);
      uint y3 = y2 ^ MersenneTwister.temperingShiftS(y2) & 2636928640U;
      uint y4 = y3 ^ MersenneTwister.temperingShiftT(y3) & 4022730752U;
      return y4 ^ MersenneTwister.temperingShiftL(y4);
    }

    private static uint temperingShiftU(uint y) => y >> 11;

    private static uint temperingShiftS(uint y) => y << 7;

    private static uint temperingShiftT(uint y) => y << 15;

    private static uint temperingShiftL(uint y) => y >> 18;

    private void init(uint seed)
    {
      this._mt[0] = seed & uint.MaxValue;
      for (this._mti = (short) 1; this._mti < (short) 624; ++this._mti)
      {
        this._mt[(int) this._mti] = (uint) ((ulong) (uint) (1812433253 * ((int) this._mt[(int) this._mti - 1] ^ (int) (this._mt[(int) this._mti - 1] >> 30))) + (ulong) this._mti);
        this._mt[(int) this._mti] &= uint.MaxValue;
      }
    }

    private void init(uint[] key)
    {
      this.init(19650218U);
      int length = key.Length;
      int index1 = 1;
      int index2 = 0;
      for (int index3 = 624 > length ? 624 : length; index3 > 0; --index3)
      {
        this._mt[index1] = (uint) ((ulong) ((this._mt[index1] ^ (uint) (((int) this._mt[index1 - 1] ^ (int) (this._mt[index1 - 1] >> 30)) * 1664525)) + key[index2]) + (ulong) index2);
        this._mt[index1] &= uint.MaxValue;
        ++index1;
        ++index2;
        if (index1 >= 624)
        {
          this._mt[0] = this._mt[623];
          index1 = 1;
        }
        if (index2 >= length)
          index2 = 0;
      }
      for (int index3 = 623; index3 > 0; --index3)
      {
        this._mt[index1] = (uint) ((ulong) (this._mt[index1] ^ (uint) (((int) this._mt[index1 - 1] ^ (int) (this._mt[index1 - 1] >> 30)) * 1566083941)) - (ulong) index1);
        this._mt[index1] &= uint.MaxValue;
        ++index1;
        if (index1 >= 624)
        {
          this._mt[0] = this._mt[623];
          index1 = 1;
        }
      }
      this._mt[0] = 2147483648U;
    }

    private double compute53BitRandom(double translate, double scale) => ((double) ((ulong) this.GenerateUInt32() >> 5) * 67108864.0 + (double) ((ulong) this.GenerateUInt32() >> 6) + translate) * scale;
  }
}
