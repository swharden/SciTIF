﻿using BitMiracle.LibTiff.Classic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SciTIF.TifReaders;

internal class ReaderIndexed8 : ITifReader
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

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        ImageData data = new(width, height, 4);

        int[] rgba = new int[width * height];
        tif.ReadRGBAImage(width, height, rgba);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int sourceY = height - 1 - y;
                int destOffset = data.GetIndex(x, y);
                data.Values[destOffset] = Tiff.GetR(rgba[sourceY * width + x]);
                data.Values[destOffset + 1] = Tiff.GetG(rgba[sourceY * width + x]);
                data.Values[destOffset + 2] = Tiff.GetB(rgba[sourceY * width + x]);
                data.Values[destOffset + 3] = Tiff.GetA(rgba[sourceY * width + x]);
            }
        }

        return data;
    }
}
