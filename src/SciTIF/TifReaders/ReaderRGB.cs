using BitMiracle.LibTiff.Classic;
using System;
using System.Collections.Generic;
using System.Text;

namespace SciTIF.TifReaders;

internal class ReaderRGB : IReadGrayscale
{
    public ImageData ReadGrayscale(Tiff tif)
    {
        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        double[,] pixelValues = new double[height, width];

        int[] raster = new int[height * width];
        tif.ReadRGBAImage(width, height, raster, true);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int offset = y * width + x;
                int r = Tiff.GetR(raster[offset]);
                int g = Tiff.GetG(raster[offset]);
                int b = Tiff.GetB(raster[offset]);
                pixelValues[y, x] = (r + g + b) / 3;
            }
        }

        return new ImageData(pixelValues);
    }
}
