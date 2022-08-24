using BitMiracle.LibTiff.Classic;

namespace SciTIF.IO.TiffReading;

internal class ReaderIndexed8 : ReaderBase
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

        int[] rgba = new int[width * height];
        tif.ReadRGBAImage(width, height, rgba);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int sourceY = height - 1 - y;
                int destOffset = r.GetIndex(x, y);
                r.Values[destOffset] = Tiff.GetR(rgba[sourceY * width + x]);
                g.Values[destOffset] = Tiff.GetG(rgba[sourceY * width + x]);
                b.Values[destOffset] = Tiff.GetB(rgba[sourceY * width + x]);
                a.Values[destOffset] = Tiff.GetA(rgba[sourceY * width + x]);
            }
        }

        return new MultiChannelImage(r, g, b, a);
    }
}
