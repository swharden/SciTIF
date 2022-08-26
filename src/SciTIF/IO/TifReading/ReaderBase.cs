using BitMiracle.LibTiff.Classic;
using System;

namespace SciTIF.IO.TifReading;

internal abstract class ReaderBase : ITifReader
{
    public abstract Image ReadSlice(Tiff tif);

    public abstract bool IsRGBA { get; }

    public Image[,,] Read(Tiff tif)
    {
        int width = TifInformation.GetWidth(tif);
        int height = TifInformation.GetHeight(tif);
        int frames = TifInformation.GetFrames(tif);
        int slices = TifInformation.GetSlices(tif);
        int channels = IsRGBA ? 4 : TifInformation.GetChannels(tif);

        Image[,,] images = new Image[frames, slices, channels];

        for (int frame = 0; frame < frames; frame++)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                int dirIndex = frame * (slices * channels) + slice * channels;
                if (IsRGBA)
                {
                    LoadChannelsRGBA(tif, images, frame, slice, dirIndex);
                }
                else
                {
                    LoadChannelsGraycale(tif, images, frame, slice, channels, dirIndex);
                }

                for (int channel = 0; channel < channels; channel++)
                {
                    if (images[frame, slice, channel] is null)
                        throw new NullReferenceException($"Frame={frame}, Slice={slice}, Channel={channel}, RGBA={IsRGBA}");
                }
            }
        }

        return images;
    }

    private void LoadChannelsGraycale(Tiff tif, Image[,,] image, int frame, int slice, int channels, int dirIndex)
    {
        for (int channel = 0; channel < channels; channel++)
        {
            int i = dirIndex + channel;
            tif.SetDirectory((short)i);
            Image img = ReadSlice(tif);
            if (img is null)
                throw new NullReferenceException($"Frame={frame}, Slice={slice}, Channel={channel}");
            image[frame, slice, channel] = img;
        }
    }

    private void LoadChannelsRGBA(Tiff tif, Image[,,] image, int frame, int slice, int dirIndex)
    {
        tif.SetDirectory((short)dirIndex);
        Image img = ReadSlice(tif);
        image[frame, slice, 0] = GetRgbaChannel(img, 0);
        image[frame, slice, 1] = GetRgbaChannel(img, 1);
        image[frame, slice, 2] = GetRgbaChannel(img, 2);
        image[frame, slice, 3] = GetRgbaChannel(img, 3);
    }

    private Image GetRgbaChannel(Image img1, int offset)
    {
        Image img2 = new(img1.Width, img1.Height);

        for (int i = 0; i < img1.Length; i++)
        {
            img2[i] = BitConverter.GetBytes((int)img1[i])[offset];
        }

        return img2;
    }
}
