using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace SciTIF;

public class Image5D
{
    /// <summary>
    /// Stores individual grayscale images indexed by: frame, slice, channel.
    /// RGB images are stored as grayscale images in 3 channels.
    /// </summary>
    private readonly Image[,,] Images;

    public readonly int Width;
    public readonly int Height;
    public readonly int Frames;
    public readonly int Slices;
    public readonly int Channels;

    public string FilePath { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Image5D(string imageFilePath)
    {
        Image5D img = IO.TifReading.TifReader.LoadTif(imageFilePath);

        Width = img.Width;
        Height = img.Height;
        Channels = img.Channels;
        Slices = img.Slices;
        Frames = img.Frames;
        Images = img.Images;

        FilePath = img.FilePath;
        Description = img.Description;
    }

    public Image5D(Image[,,] images)
    {
        Images = images;

        Width = images[0, 0, 0].Width;
        Height = images[0, 0, 0].Height;
        Frames = images.GetLength(0);
        Slices = images.GetLength(1);
        Channels = images.GetLength(2);

        foreach (Image img in GetAllImages())
        {
            if (img.Width != Width || img.Height != Height)
            {
                throw new InvalidDataException("all images must have the same dimensions");
            }
        }
    }

    public Image GetImage(int frame, int slice, int channel)
    {
        return Images[frame, slice, channel];
    }

    public Image[] GetAllImages()
    {
        List<Image> images = new();

        for (int frame = 0; frame < Frames; frame++)
        {
            for (int slice = 0; slice < Slices; slice++)
            {
                for (int channel = 0; channel < Channels; channel++)
                {
                    images.Add(GetImage(frame, slice, channel));
                }
            }
        }

        return images.ToArray();
    }
}
