using System;

namespace SciTIF;

/// <summary>
/// Holds floating-point representation of single-channel image data values.
/// Values inside the array are technically mutable, but no methods here mutate them.
/// </summary>
public class ImageDataXY // TODO: abstract and make 2d/3d/4d types
{
    public readonly double[] Values;
    public readonly int Height;
    public readonly int Width;

    public ImageDataXY(int width, int height)
    {
        Values = new double[width * height];
        Width = width;
        Height = height;
    }

    public ImageDataXY(int width, int height, double[] values)
    {
        if (values is null || values.Length != width * height)
            throw new ArgumentException($" {nameof(values)} length must equal {nameof(width)} * {nameof(height)}");

        Values = values;
        Width = width;
        Height = height;
    }

    public static ImageDataXY operator +(ImageDataXY a, ImageDataXY b)
    {
        if ((a.Width != b.Width) || (a.Height != b.Height) || (a.Values.Length != b.Values.Length))
            throw new ArgumentException("image operations require identical dimensions");

        double[] values = new double[a.Values.Length];

        for (int i = 0; i < values.Length; i++)
            values[i] = a.Values[i] + b.Values[i];

        return new ImageDataXY(a.Width, a.Height, values);
    }

    public double GetPixel(int x, int y)
    {
        // TODO: bound check
        return Values[y * Width + x];
    }
}
