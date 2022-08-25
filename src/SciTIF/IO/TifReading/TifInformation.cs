using BitMiracle.LibTiff.Classic;
using System;
using System.Collections.Generic;
using System.Text;

namespace SciTIF.IO.TifReading;

/// <summary>
/// Contains methods to extract information about TIF files from their header values
/// </summary>
public static class TifInformation
{
    public static int GetWidth(Tiff tif)
    {
        return tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
    }

    public static int GetHeight(Tiff tif)
    {
        return tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
    }

    public static int GetFrames(Tiff tif)
    {
        return GetIntFromDescription(tif, "frames");
    }

    public static int GetSlices(Tiff tif)
    {
        return GetIntFromDescription(tif, "slices");
    }

    public static int GetChannels(Tiff tif)
    {
        return GetIntFromDescription(tif, "channels");
    }

    private static int GetIntFromDescription(Tiff tif, string key, int defaultValue = 1)
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
