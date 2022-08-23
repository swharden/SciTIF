using BitMiracle.LibTiff.Classic;

namespace SciTIF.IO.TiffReading;

internal class ReaderRGB : ReaderBase
{
    public override Image ReadSlice(Tiff tif, int directory)
    {
        tif.SetDirectory((short)directory);

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        Image data = new(width, height, 3);

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
            }
        }

        return data;
    }
}
