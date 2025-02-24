﻿using System;
using System.Linq;

namespace SciTIF;

// TODO: should frames and slices be 0-indexed?

/// <summary>
/// Floating-point representation of pixel intensity values from a single-channel image.
/// </summary>
public class Image : IBitmap
{
    public double[] Values;
    private double[]? RememberedValues;
    public ILUT LUT { get; set; } = new LUTs.Gray();
    public int Height { get; private set; }
    public int Width { get; private set; }

    public Image(int width, int height)
    {
        Width = width;
        Height = height;
        Values = new double[width * height];
    }

    public Image(int width, int height, double[] values)
    {
        if (values.Length != (width * height))
            throw new ArgumentException("invalid length");

        Width = width;
        Height = height;
        Values = values;
    }

    #region IMAGE EXPORT

    public void Save(string filename, int quality = 90)
    {
        IO.SystemDrawing.Save(filename, this, quality);
    }

    public byte[] GetBitmapBytes()
    {
        return IO.SystemDrawing.GetBitmapBytes(this);
    }

    #endregion

    #region GET AND SET PIXEL VALUES

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

    public void SetPixel(int x, int y, double value)
    {
        Values[GetIndex(x, y)] = value;
    }

    /// <summary>
    /// Apply the given value to the specified pixel only if it is brighter than its current value
    /// </summary>
    public void SetPixelMax(int x, int y, double value)
    {
        int i = GetIndex(x, y);
        Values[i] = Math.Max(value, Values[i]);
    }

    #endregion

    #region PIXEL VALUE ANALYSIS

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

    #endregion

    #region PIXEL VALUE MUTATION

    // IDEA: multiple states could be stored in a numbered collection

    public void RememberValues()
    {
        if (RememberedValues is null)
            RememberedValues = new double[Values.Length];

        Array.Copy(Values, RememberedValues, Values.Length);
    }

    public void RecallValues()
    {
        if (RememberedValues is null)
            throw new InvalidOperationException("Recall() requires Remember() to have been called previously");
        else
            Array.Copy(RememberedValues, Values, Values.Length);
    }

    // TODO: disable mutation
    public void AutoScale(double percentileLow = 0, double percentileHigh = 100, double max = 255)
    {

        if (percentileLow == 0 && percentileHigh == 100)
        {
            double minPixel = Values.Min();
            double maxPixel = Values.Max();
            ScaleBy(minPixel, max / (maxPixel - minPixel));
        }
        else
        {
            double[] percents = { percentileLow, percentileHigh };
            double[] percentiles = Percentile(percents);
            double minPixel = percentiles[0];
            double maxPixel = percentiles[1];
            ScaleBy(minPixel, max / (maxPixel - minPixel));
        }
    }

    public void Scale(double min, double max)
    {
        double subtract = min;
        double multiply = max / (max - min);
        ScaleBy(subtract, multiply);
    }

    public void ScaleBy(double subtract, double multiply)
    {
        for (int i = 0; i < Values.Length; i++)
            Values[i] = (Values[i] - subtract) * multiply;
    }

    #endregion

    #region OPERATOR OVERLOADS

    private static void AssertSameDimensions(Image a, Image b)
    {
        if (a.Width != b.Width)
            throw new ArgumentException($"both images must have the same width ({a.Width} vs {b.Width})");

        if (a.Height != b.Height)
            throw new ArgumentException($"both images must have the same height ({a.Height} vs {b.Height})");

        if (a.Values.Length != b.Values.Length)
            throw new ArgumentException($"both images must have the same data length ({a.Values.Length} vs {b.Values.Length})");
    }

    public static Image operator +(Image a, Image b)
    {
        AssertSameDimensions(a, b);
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

    #endregion
}
