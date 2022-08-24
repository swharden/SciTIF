using BitMiracle.LibTiff.Classic;
using System;
using System.IO;

namespace SciTIF.IO.TiffReading;

internal abstract class ReaderBase : ITifReader
{
    public abstract Image ReadSlice(Tiff tif);

    public abstract bool IsRGBA { get; }

    public Image5D Read(Tiff tif)
    {
        int frames = GetIntFromDescription(tif, "frames");
        int slices = GetIntFromDescription(tif, "slices");
        int channels = IsRGBA ? 4 : GetIntFromDescription(tif, "channels");

        Console.WriteLine();
        Console.WriteLine(Path.GetFileName(tif.FileName()));
        Console.WriteLine(this);
        Image5D image = new(slices, frames, channels);
        LoadAllImages(image, tif, IsRGBA);

        return image;
    }

    private void LoadAllImages(Image5D image, Tiff tif, bool isRGBA)
    {
        for (int frame = 0; frame < image.Frames; frame++)
        {
            for (int slice = 0; slice < image.Slices; slice++)
            {
                if (isRGBA)
                    LoadChannelsRGBA(tif, image, frame, slice);
                else
                    LoadChannelsGraycale(tif, image, frame, slice);

                for (int channel = 0; channel < image.Channels; channel++)
                {
                    if (image.GetImage(frame, slice, channel) is null)
                        throw new NullReferenceException($"Frame={frame}, Slice={slice}, Channel={channel}, RGBA={IsRGBA}");
                }
            }
        }
    }

    private void LoadChannelsGraycale(Tiff tif, Image5D image, int frame, int slice)
    {
        for (int channel = 0; channel < image.Channels; channel++)
        {
            int i = frame * (image.Slices * image.Channels) + slice * image.Channels + channel;
            tif.SetDirectory((short)i);
            Image img = ReadSlice(tif);
            if (img is null)
                throw new NullReferenceException($"Frame={frame}, Slice={slice}, Channel={channel}");
            image.SetImage(frame, slice, channel, img);
        }
    }

    private void LoadChannelsRGBA(Tiff tif, Image5D image, int frame, int slice)
    {
        int i = frame * image.Slices + slice;
        tif.SetDirectory((short)i);
        Image img = ReadSlice(tif); // TODO: break into colors

        image.SetImage(frame, slice, channel: 0, img);
        image.SetImage(frame, slice, channel: 1, img);
        image.SetImage(frame, slice, channel: 2, img);
        image.SetImage(frame, slice, channel: 3, img);
    }

    private int GetImageWidth(Tiff tif)
    {
        return int.Parse(tif.GetFieldDefaulted(TiffTag.IMAGEWIDTH)[0].ToString()!);
    }

    private int GetImageHeight(Tiff tif)
    {
        return int.Parse(tif.GetFieldDefaulted(TiffTag.IMAGELENGTH)[0].ToString()!);
    }

    private int GetIntFromDescription(Tiff tif, string key, int defaultValue = 1)
    {
        FieldValue[] desc = tif.GetField(TiffTag.IMAGEDESCRIPTION);
        if (desc is not null)
        {
            foreach (string line in desc[0].ToString()!.Split('\n'))
            {
                if (line.Contains(key))
                    return int.Parse(line.Split('=')[1]);
            }
        }
        return defaultValue;
    }
}
