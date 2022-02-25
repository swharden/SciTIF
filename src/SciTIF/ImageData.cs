using System;

namespace SciTIF;

/// <summary>
/// Holds floating-point representation of single-channel image data values.
/// Values inside the array are technically mutable, but no methods here mutate them.
/// </summary>
public class ImageData
{
    public readonly double[,] Values;
    public int Width => Values.GetLength(1);
    public int Height => Values.GetLength(0);

    public ImageData(double[,] data)
    {
        if (data is null)
            throw new ArgumentNullException(nameof(data));

        Values = data;
    }
}
