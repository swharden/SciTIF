using BitMiracle.LibTiff.Classic;
using System.Linq;

namespace SciTIF.TifReaders;

internal class ReaderRGBA : ITifReader
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

        int[] raster = new int[height * width];
        tif.ReadRGBAImage(width, height, raster, true);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int sourceY = height - y - 1;
                int offset = sourceY * width + x;
                int destOffset = data.GetIndex(x, y);
                data.Values[destOffset] = Tiff.GetR(raster[offset]);
                data.Values[destOffset + 1] = Tiff.GetG(raster[offset]);
                data.Values[destOffset + 2] = Tiff.GetB(raster[offset]);
                data.Values[destOffset + 3] = Tiff.GetA(raster[offset]);
            }
        }

        return data;
    }
}
