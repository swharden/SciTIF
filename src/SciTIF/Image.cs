using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SciTIF;

/// <summary>
/// Floating-point representation of pixel intensity values
/// </summary>
public class Image : IEnumerable<double>
{
    private readonly double[] Values;
    private double[]? RememberedValues;

    public readonly int Height;
    public readonly int Width;
    public readonly int Length;

    public Image(int width, int height)
    {
        Width = width;
        Height = height;
        Length = Width * Height;
        Values = new double[Length];
    }

    public Image(int width, int height, double[] values)
    {
        Width = width;
        Height = height;
        Length = Width * Height;

        if (values.Length != Length)
            throw new ArgumentException("invalid length");

        Values = values;
    }

    public double this[int index]
    {
        get => Values[index];
        set => Values[index] = value;
    }

    public IEnumerator<double> GetEnumerator()
    {
        foreach (double value in Values)
        {
            yield return value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static Image operator +(Image a, Image b)
    {
        if ((a.Width != b.Width) || (a.Height != b.Height) || (a.Values.Length != b.Values.Length))
            throw new ArgumentException("image operations require identical dimensions");

        double[] values = new double[a.Values.Length];

        for (int i = 0; i < values.Length; i++)
            values[i] = a.Values[i] + b.Values[i];

        return new Image(a.Width, a.Height, values);
    }

    public static Image operator /(Image a, double b)
    {
        double[] values = new double[a.Values.Length];

        for (int i = 0; i < values.Length; i++)
            values[i] = a.Values[i] / b;

        return new Image(a.Width, a.Height, values);
    }

    public void SavePng(string filename, bool autoscale = false)
    {
        if (autoscale)
            AutoScale();
        IO.SystemDrawing.SavePNG(filename, this);
    }

    public void Remember()
    {
        if (RememberedValues is null)
            RememberedValues = new double[Values.Length];

        Array.Copy(Values, RememberedValues, Values.Length);
    }

    public void Recall()
    {
        if (RememberedValues is null)
            throw new InvalidOperationException("Recall() requires Remember() to have been called previously");
        else
            Array.Copy(RememberedValues, Values, Values.Length);
    }

    public int GetIndex(int x, int y) => y * Width + x;

    public double GetPixel(int x, int y) => Values[GetIndex(x, y)];

    public byte GetPixelByte(int x, int y, bool clamp)
    {
        double value = Values[GetIndex(x, y)];
        if (clamp)
        {
            value = Math.Max(0, value);
            value = Math.Min(255, value);
        }
        return (byte)value;
    }

    public void SetPixel(int x, int y, byte r, byte g, byte b, byte a)
    {
        Values[GetIndex(x, y)] = BitConverter.ToInt32(new byte[] { r, g, b, a }, 0);
    }

    public double Min() => Values.Min();

    public double Max() => Values.Max();

    public double Percentile(double percent)
    {
        double[] percents = { percent };
        return Percentile(percents).Single();
    }

    public double[] Percentile(double[] percents)
    {
        int[] indexes = percents.Select(x => (int)(Values.Length * x / 100)).ToArray();
        double[] sorted = new double[Values.Length];
        Array.Copy(Values, sorted, Values.Length);
        Array.Sort(sorted);
        return indexes.Select(x => sorted[x]).ToArray();
    }

    public void AutoScale(double percentileLow = 0, double percentileHigh = 100, double newMax = 255)
    {
        double min;
        double max;

        if (percentileLow == 0 && percentileHigh == 100)
        {
            min = Min();
            max = Max();
        }
        else
        {
            double[] percents = { percentileLow, percentileHigh };
            double[] percentiles = Percentile(percents);
            min = percentiles[0];
            max = percentiles[1];
        }

        double scale = newMax / (max - min);

        for (int i = 0; i < Values.Length; i++)
            Values[i] = (Values[i] - min) * scale;
    }
}
