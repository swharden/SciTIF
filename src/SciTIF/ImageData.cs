using System;

namespace SciTIF;

/// <summary>
/// Holds floating-point representation of single-channel image data values.
/// Values inside the array are technically mutable, but no methods here mutate them.
/// </summary>
public class ImageData
{
    public readonly double[,] Values;
    public readonly int Height;
    public readonly int Width;

    public ImageData(int width, int height)
    {
        Values = new double[height, width];
        Width = width;
        Height = height;
    }

    public ImageData(double[,] data)
    {
        if (data is null)
            throw new ArgumentNullException(nameof(data));

        Values = data;
        Width = Values.GetLength(1);
        Height = Values.GetLength(0);
    }

    public void SavePng(string filePath) => Export.PNG(filePath, Values);

    public static ImageData operator +(ImageData a, ImageData b)
    {
        if (a.Width != b.Width || a.Height != b.Height)
            throw new InvalidOperationException("images must be same dimensions");

        double[,] result = new double[a.Height, a.Width];

        for (int y = 0; y < a.Height; y++)
            for (int x = 0; x < a.Width; x++)
                result[y, x] = a.Values[y, x] + b.Values[y, x];

        return new ImageData(result);
    }

    public double this[int y, int x]
    {
        get => Values[y, x];
        set => Values[y, x] = value;
    }
}
