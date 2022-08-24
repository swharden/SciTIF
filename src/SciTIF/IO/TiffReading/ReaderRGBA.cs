using BitMiracle.LibTiff.Classic;

namespace SciTIF.IO.TiffReading;

internal class ReaderRGBA : ReaderBase
{
    public override MultiChannelImage ReadSlice(Tiff tif, int directory)
    {
        tif.SetDirectory((short)directory);

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        GrayscaleImage r = new(width, height);
        GrayscaleImage g = new(width, height);
        GrayscaleImage b = new(width, height);
        GrayscaleImage a = new(width, height);

        int[] raster = new int[height * width];
        tif.ReadRGBAImage(width, height, raster, true);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int sourceY = height - y - 1;
                int offset = sourceY * width + x;
                int destOffset = r.GetIndex(x, y);
                r.Values[destOffset] = Tiff.GetR(raster[offset]);
                g.Values[destOffset] = Tiff.GetG(raster[offset]);
                b.Values[destOffset] = Tiff.GetB(raster[offset]);
                a.Values[destOffset] = Tiff.GetA(raster[offset]);
            }
        }

        return new MultiChannelImage(r, g, b, a);
    }
}
