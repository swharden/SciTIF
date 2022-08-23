using BitMiracle.LibTiff.Classic;
using System.Linq;

namespace SciTIF.TifReaders;

internal class ReaderRGBA : ITifReader
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

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();

        double[] valuesR = new double[height * width];
        double[] valuesG = new double[height * width];
        double[] valuesB = new double[height * width];
        double[] valuesA = new double[height * width];

        int[] raster = new int[height * width];
        tif.ReadRGBAImage(width, height, raster, true);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int sourceY = height - y - 1;
                int offset = sourceY * width + x;
                int destOffset = y * width + x;
                valuesR[destOffset] = Tiff.GetR(raster[offset]);
                valuesG[destOffset] = Tiff.GetG(raster[offset]);
                valuesB[destOffset] = Tiff.GetB(raster[offset]);
                valuesA[destOffset] = Tiff.GetA(raster[offset]);
            }
        }

        return new ImageDataXY[] {
            new ImageDataXY(width, height, valuesR),
            new ImageDataXY(width, height, valuesG),
            new ImageDataXY(width, height, valuesB),
            new ImageDataXY(width, height, valuesA),
        };
    }
}
