using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SciTIF;

public class Image5D
{
    private readonly MultiChannelImage[] MultiChannelImages;
    public int Width => MultiChannelImages.Any() ? MultiChannelImages.First().Images.First().Width : 0;
    public int Height => MultiChannelImages.Any() ? MultiChannelImages.First().Images.First().Height : 0;
    public int Channels => MultiChannelImages.Any() ? MultiChannelImages.First().Channels : 0;

    public readonly int Slices;
    public readonly int Frames;

    public Image5D(int slices, int frames)
    {
        Slices = slices;
        Frames = frames;
        MultiChannelImages = new MultiChannelImage[slices * frames];
    }

    public int GetIndex(int slice, int frame)
    {
        return Slices * frame + slice;
    }

    public MultiChannelImage GetSlice(int slice, int frame)
    {
        return MultiChannelImages[GetIndex(slice, frame)];
    }

    public MultiChannelImage GetSlice(int index)
    {
        return MultiChannelImages[index];
    }

    public void SetSlice(int slice, int frame, MultiChannelImage img)
    {
        MultiChannelImages[GetIndex(slice, frame)] = img;
    }

    public void SetSlice(int directory, MultiChannelImage img)
    {
        MultiChannelImages[directory] = img;
    }

    public MultiChannelImage ProjectMean(int frame)
    {
        GrayscaleImage[] sum = new GrayscaleImage[Channels];
        for (int i = 0; i < Channels; i++)
        {
            sum[i] = new(Width, Height);
        }

        for (int i = 0; i < Slices; i++)
        {
            MultiChannelImage multichannel = GetSlice(i, Frames);
            for (int j = 0; j < Channels; j++)
            {
                sum[j] += multichannel.Images[j];
            }
        }

        for (int i = 0; i < Channels; i++)
        {
            sum[i] /= Channels;
        }

        return new MultiChannelImage(sum);
    }
}
