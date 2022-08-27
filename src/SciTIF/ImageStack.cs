using System;
using System.Linq;
using System.Collections.Generic;

namespace SciTIF;

/// <summary>
/// Holds a stack of single-channel images
/// </summary>
public class ImageStack
{
    public readonly Image[] Images;
    public int SliceCount => Images.Length;
    public int Width => Images[0].Width;
    public int Height => Images[0].Height;

    public ImageStack(IEnumerable<Image> images)
    {
        if (!images.Any())
            throw new ArgumentException("a stack must have at least one image");

        Images = images.ToArray();
    }

    public Image ProjectMax()
    {
        return Project((pixelValues) => pixelValues.Max());
    }

    public Image ProjectMean()
    {
        return Project((pixelValues) => pixelValues.Sum() / pixelValues.Length);
    }

    public Image Project(Func<double[], double> func)
    {
        Image img = new(Width, Height);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                double[] values = Images.Select(img => img.GetPixel(x, y)).ToArray();
                double result = func.Invoke(values);
                img.SetPixel(x, y, result);
            }
        }
        return img;
    }
}
