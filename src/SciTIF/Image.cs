using System;
using System.Linq;

namespace SciTIF;

/// <summary>
/// Floating-point representation of pixel intensity values
/// </summary>
public class Image
{
    public readonly double[] Values;
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
}
