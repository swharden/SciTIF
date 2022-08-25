using BitMiracle.LibTiff.Classic;

namespace SciTIF.IO.TifReading;

internal class ReaderRGB : ReaderBase
{
    public override bool IsRGBA => true;

    public override Image ReadSlice(Tiff tif)
    {
        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        Image img = new(width, height);

        int[] raster = new int[height * width];
        tif.ReadRGBAImage(width, height, raster, true);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int sourceY = height - y - 1;
                int offset = sourceY * width + x;
                byte r = (byte)Tiff.GetR(raster[offset]);
                byte g = (byte)Tiff.GetG(raster[offset]);
                byte b = (byte)Tiff.GetB(raster[offset]);
                img.SetPixel(x, y, r, g, b, 255);
            }
        }

        return img;
    }
}
