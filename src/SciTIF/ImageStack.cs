using System;
using System.Linq;
using System.Collections.Generic;

namespace SciTIF;

/// <summary>
/// Holds a stack of single-channel images
/// </summary>
public class ImageStack
{
    private readonly Image[] Images;

    /// <summary>
    /// Number of slices in the stack
    /// </summary>
    public int Slices => Images.Length;

    /// <summary>
    /// Width of every image in the stack
    /// </summary>
    public int Width => Images[0].Width;

    /// <summary>
    /// Height of every image in the stack
    /// </summary>
    public int Height => Images[0].Height;

    public ImageStack(IEnumerable<Image> images)
    {
        if (!images.Any())
            throw new ArgumentException("a stack must have at least one image");

        Images = images.ToArray();
    }

    /// <summary>
    /// Return the <see cref="Image"/> for the given slice (starting at 0)
    /// </summary>
    public Image GetSlice(int slice)
    {
        return Images[slice];
    }

    /// <summary>
    /// Return all slices as an array of <see cref="Image"/> objects
    /// </summary>
    public Image[] GetSlices()
    {
        return Images.ToArray();
    }

    /// <summary>
    /// Return the maximum projection of the stack.
    /// Each pixel is the maximum of all pixel values at that position across all slices.
    /// </summary>
    public Image ProjectMax()
    {
        return Project((pixelValues) => pixelValues.Max());
    }

    /// <summary>
    /// Return the additive projection of the stack.
    /// Each pixel is the sum of all pixel values at that position across all slices.
    /// </summary>
    public Image ProjectSum()
    {
        return Project((pixelValues) => pixelValues.Sum());
    }

    /// <summary>
    /// Return the mean projection of the stack.
    /// Each pixel is the mean of all pixel values at that position across all slices.
    /// </summary>
    public Image ProjectMean()
    {
        return Project((pixelValues) => pixelValues.Sum() / pixelValues.Length);
    }

    /// <summary>
    /// Return the mean projection of the stack using a custom function.
    /// </summary>
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

    /// <summary>
    /// Project the stack into a single RGB image, adding colors from each image according to its LUT
    /// </summary>
    public ImageRGB Merge()
    {
        Image red = new(Width, Height);
        Image green = new(Width, Height);
        Image blue = new(Width, Height);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                PixelColor color = new();

                for (int i = 0; i < Slices; i++)
                {
                    Image slice = GetSlice(i);
                    double value = slice.GetPixel(x, y);
                    color += slice.LUT.GetColor((byte)value);
                }

                red.SetPixel(x, y, color.R);
                green.SetPixel(x, y, color.G);
                blue.SetPixel(x, y, color.B);
            }
        }

        return new ImageRGB(red, green, blue);
    }
}
