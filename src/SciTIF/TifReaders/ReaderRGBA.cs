using BitMiracle.LibTiff.Classic;
using System.Linq;

namespace SciTIF.TifReaders;

internal class ReaderRGBA : ITifReader
{
    public ImageData[] Read(Tiff tif)
    {
        return Enumerable.Range(0, tif.NumberOfDirectories())
            .SelectMany(x => ReadDirectory(tif, x))
            .ToArray();
    }

    public ImageData[] ReadDirectory(Tiff tif, int directory)
    {
        tif.SetDirectory((short)directory);

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();

        double[,] valuesR = new double[height, width];
        double[,] valuesG = new double[height, width];
        double[,] valuesB = new double[height, width];
        double[,] valuesA = new double[height, width];

        int[] raster = new int[height * width];
        tif.ReadRGBAImage(width, height, raster, true);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int sourceY = height - y - 1;
                int offset = sourceY * width + x;
                valuesR[y, x] = Tiff.GetR(raster[offset]);
                valuesG[y, x] = Tiff.GetG(raster[offset]);
                valuesB[y, x] = Tiff.GetB(raster[offset]);
                valuesA[y, x] = Tiff.GetA(raster[offset]);
            }
        }

        return new ImageData[] {
            new ImageData(valuesR),
            new ImageData(valuesG),
            new ImageData(valuesB),
            new ImageData(valuesA),
        };
    }
}
