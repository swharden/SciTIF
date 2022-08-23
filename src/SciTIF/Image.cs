using System;
using System.Linq;

namespace SciTIF;

/// <summary>
/// Floating-point representation of pixel intensity values
/// </summary>
public class Image
{
    public readonly double[] Values;
    private double[]? RememberedValues;

    public readonly int Height;
    public readonly int Width;
    public readonly int Channels;

    public Image(int width, int height, int channels = 1)
    {
        Width = width;
        Height = height;
        Channels = channels;
        Values = new double[width * height * channels];
    }

    public Image(int width, int height, double[] values, int channels = 1)
    {
        if (values.Length != (width * height * channels))
            throw new ArgumentException("invalid length");

        Width = width;
        Height = height;
        Channels = channels;
        Values = values;
    }

    public static Image operator +(Image a, Image b)
    {
        if ((a.Width != b.Width) || (a.Height != b.Height) || (a.Channels != b.Channels) || (a.Values.Length != b.Values.Length))
            throw new ArgumentException("image operations require identical dimensions");

        double[] values = new double[a.Values.Length];

        for (int i = 0; i < values.Length; i++)
            values[i] = a.Values[i] + b.Values[i];

        return new Image(a.Width, a.Height, values, a.Channels);
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

    public int GetIndex(int x, int y, int channel = 0)
    {
        return (y * Width + x) * Channels + channel;
    }

    public double GetPixel(int x, int y, int channel = 0)
    {
        int i = GetIndex(x, y, channel);
        return Values[i];
    }

    public double[] GetPixelValues(int x, int y)
    {
        return Enumerable.Range(GetIndex(x, y), Channels).Select(x => Values[x]).ToArray();
    }

    public double Min() => Values.Min();

    public double Max() => Values.Max();

    public double Percentile(double percent)
    {
        return Percentile(new double[] { percent }).Single();
    }

    public double[] Percentile(double[] percents)
    {
        double[] sorted = new double[Values.Length];
        Array.Copy(Values, sorted, Values.Length);
        Array.Sort(sorted);

        return percents.Select(x => (int)(sorted.Length * x / 100)).Select(x => sorted[x]).ToArray();
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
