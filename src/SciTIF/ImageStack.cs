using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.Compression;

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

    /// <summary>
    /// Create a stack from a collection of single-channel images
    /// </summary>
    public ImageStack(IEnumerable<Image> images)
    {
        if (!images.Any())
            throw new ArgumentException("a stack must have at least one image");

        foreach (Image image in images)
        {
            if (image.Width != images.First().Width)
                throw new InvalidOperationException("all images must be the same width");

            if (image.Height != images.First().Height)
                throw new InvalidOperationException("all images must be the same height");
        }

        Images = images.ToArray();
    }

    /// <summary>
    /// Create a stack from a collection of single-channel tif files
    /// </summary>
    public ImageStack(IEnumerable<string> imagePaths)
    {
        if (!imagePaths.Any())
            throw new ArgumentException("a stack must have at least one image");

        IEnumerable<TifFile> tifFiles = imagePaths.Select(x => new TifFile(x));

        foreach (TifFile tifFile in tifFiles)
        {
            if (tifFile.Width != tifFiles.First().Width)
                throw new InvalidOperationException("all images must be the same width");

            if (tifFile.Height != tifFiles.First().Height)
                throw new InvalidOperationException("all images must be the same height");

            if (tifFile.Channels != 1)
                throw new InvalidOperationException("stacks cannot be created from multi-channel images");

            if (tifFile.Slices != 1)
                throw new InvalidOperationException("stacks cannot be created from multi-slice images");

            if (tifFile.Frames != 1)
                throw new InvalidOperationException("stacks cannot be created from multi-frame images");
        }

        Images = tifFiles.Select(x => x.GetImage(0, 0, 0)).ToArray();
    }

    /// <summary>
    /// Uniformly scale values across all images so the brighest pixel equals the given value
    /// </summary>
    public void AutoScale()
    {
        double maxVale = Images.Select(x => x.Max()).Max();
        Images.ToList().ForEach(x => x.ScaleBy(0, 255.0 / maxVale));
    }

    /// <summary>
    /// Return the pixel value for a specified slice
    /// </summary>
    public double GetPixel(int x, int y, int slice)
    {
        return Images[slice].GetPixel(x, y);
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
    /// Create a color-coded projection that spans the range of colors in the given LUT
    /// </summary>
    public ImageRGB Project(ILUT lut)
    {
        if (Slices < 2)
            throw new InvalidOperationException("Projection requires at least 2 slices");

        Image r = new(Width, Height);
        Image g = new(Width, Height);
        Image b = new(Width, Height);

        PixelColor[] sliceColors = Enumerable.Range(0, Slices)
            .Select(z => (byte)(255 * z / (Slices - 1)))
            .Select(x => lut.GetColor(x))
            .ToArray();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Slices; z++)
                {
                    double value = GetPixel(x, y, z);

                    r.SetPixelMax(x, y, value * sliceColors[z].R / 255);
                    g.SetPixelMax(x, y, value * sliceColors[z].G / 255);
                    b.SetPixelMax(x, y, value * sliceColors[z].B / 255);
                }
            }
        }

        ImageRGB rgb = new(r, g, b);
        return rgb;
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
