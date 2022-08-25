using System;

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

    public Image5D(int frames, int slices, int channels, int width, int height)
    {
        Width = width;
        Height = height;
        Channels = channels;
        Slices = slices;
        Frames = frames;
        Images = new Image[frames, slices, channels];
    }

    public Image GetImage(int frame, int slice, int channel)
    {
        return Images[frame, slice, channel];
    }

    public void SetImage(int frame, int slice, int channel, Image img)
    {
        if (img.Width != Width)
            throw new InvalidOperationException($"Cannot add image with Width {img.Width} into 5D image with Width {Width}");

        Images[frame, slice, channel] = img;
    }
}
