using BitMiracle.LibTiff.Classic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SciTIF.TifReaders;

internal class ReaderIndexed8 : ITifReader
{
    public ImageData[] Read(Tiff tif)
    {
        return Enumerable.Range(0, tif.NumberOfDirectories())
            .SelectMany(x => ReadDirectory(tif, x))
            .ToArray();
    }

    private ImageData[] ReadDirectory(Tiff tif, int directory)
    {
        tif.SetDirectory((short)directory);

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();

        double[,] r = new double[height, width];
        double[,] g = new double[height, width];
        double[,] b = new double[height, width];
        double[,] a = new double[height, width];

        int[] rgba = new int[width * height];
        tif.ReadRGBAImage(width, height, rgba);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int sourceY = height - 1 - y;
                r[y, x] = Tiff.GetR(rgba[sourceY * width + x]);
                g[y, x] = Tiff.GetG(rgba[sourceY * width + x]);
                b[y, x] = Tiff.GetB(rgba[sourceY * width + x]);
                a[y, x] = Tiff.GetA(rgba[sourceY * width + x]);
            }
        }

        return new ImageData[] {
            new ImageData(r),
            new ImageData(g),
            new ImageData(b),
            new ImageData(a),
        };
    }

    ImageData[] ITifReader.ReadDirectory(Tiff tif, int directory)
    {
        throw new NotImplementedException();
    }
}
