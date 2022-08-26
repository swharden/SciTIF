﻿using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace SciTIF;

/// <summary>
/// Stores individual grayscale images indexed by: frame, slice, channel.
/// RGB images are stored as grayscale images in 3 channels.
/// </summary>
public class Image5D
{
    protected Image[,,] Images { get; set; } = new Image[0, 0, 0];
    private Image FirstImage => Images[0, 0, 0];
    public int Width => FirstImage.Width;
    public int Height => FirstImage.Height;
    public int Frames => Images.GetLength(0);
    public int Slices => Images.GetLength(1);
    public int Channels => Images.GetLength(2);

    /// <summary>
    /// Callers must manually assign <see cref="Images"/>
    /// </summary>
    protected Image5D() { }

    public Image5D(Image[,,] images)
    {
        Images = images;
        AssertAllImagesHaveSameDimensions();
    }

    protected void AssertAllImagesHaveSameDimensions()
    {
        foreach (Image img in GetAllImages())
        {
            if (img.Width != Width || img.Height != Height)
            {
                throw new InvalidDataException("all images must have the same dimensions");
            }
        }
    }

    protected void ReplaceImages(Image[,,] images)
    {
        Images = images;
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
