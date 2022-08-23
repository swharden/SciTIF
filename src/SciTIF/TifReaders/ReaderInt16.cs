﻿using BitMiracle.LibTiff.Classic;
using System;
using System.Linq;

namespace SciTIF.TifReaders;

internal class ReaderInt16 : ITifReader
{
    public ImageData[] ReadAllSlices(Tiff tif)
    {
        return Enumerable.Range(0, tif.NumberOfDirectories())
            .Select(x => ReadSlice(tif, x))
            .ToArray();
    }

    public ImageData ReadSlice(Tiff tif, int directory)
    {
        tif.SetDirectory((short)directory);

        const int bytesPerPixel = 2;

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

        int msbOffset = tif.IsBigEndian() ? 0 : 1;
        int lsbOffset = tif.IsBigEndian() ? 1 : 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int offset = (y * width + x) * bytesPerPixel;
                int i = data.GetIndex(x, y);
                data.Values[i] += bytes[offset + lsbOffset];
                data.Values[i] += bytes[offset + msbOffset] << 8;
            }
        }

        return data;
    }
}
