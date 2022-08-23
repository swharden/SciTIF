using BitMiracle.LibTiff.Classic;
using System;
using System.Linq;

namespace SciTIF.TifReaders;

internal class ReaderInt8 : ITifReader
{
    public ImageData[] ReadAllSlices(Tiff tif)
    {
        return Enumerable.Range(0, tif.NumberOfDirectories())
            .Select(x => ReadSlice(tif, x))
            .ToArray();
    }

    public ImageData ReadSlice(Tiff tif, int slice)
    {
        tif.SetDirectory((short)slice);

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        ImageData data = new(width, height);

        int numberOfStrips = tif.NumberOfStrips();
        int stripSize = tif.StripSize();

        byte[] bytes = new byte[numberOfStrips * stripSize];
        for (int i = 0; i < numberOfStrips; ++i)
        {
            tif.ReadRawStrip(i, bytes, i * stripSize, stripSize);
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int offset = y * width + x;
                int i = data.GetIndex(x, y);
                data.Values[i] = bytes[offset];
            }
        }

        return data;
    }
}
