using BitMiracle.LibTiff.Classic;
using System;
using System.Linq;

namespace SciTIF.TifReaders;

internal class ReaderInt16 : ITifReader
{
    public ImageDataXY[] Read(Tiff tif)
    {
        return Enumerable.Range(0, tif.NumberOfDirectories())
            .SelectMany(x => ReadDirectory(tif, x))
            .ToArray();
    }

    public ImageDataXY[] ReadDirectory(Tiff tif, int directory)
    {
        tif.SetDirectory((short)directory);

        const int bytesPerPixel = 2;

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        double[] values = new double[height * width];

        int numberOfStrips = tif.NumberOfStrips();
        int stripSize = tif.StripSize();

        byte[] bytes = new byte[numberOfStrips * stripSize];
        for (int i = 0; i < numberOfStrips; ++i)
        {
            tif.ReadRawStrip(i, bytes, i * stripSize, stripSize);
        }

        int msbOffset = tif.IsBigEndian() ? 0 : 1;
        int lsbOffset = tif.IsBigEndian() ? 1 : 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int offset = (y * width + x) * bytesPerPixel;
                values[y * width + x] += bytes[offset + lsbOffset];
                values[y * width + x] += bytes[offset + msbOffset] << 8;
            }
        }

        return new ImageDataXY[] { new ImageDataXY(width, height, values) };
    }
}
