using BitMiracle.LibTiff.Classic;

namespace SciTIF.IO.TifReading;

internal class ReaderIndexedRGB : ReaderBase
{
    public override bool IsRGBA => true;

    public override Image ReadSlice(Tiff tif)
    {
        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();

        Image img = new(width, height);

        int[] rgba = new int[width * height];
        tif.ReadRGBAImage(width, height, rgba);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int sourceY = height - 1 - y;
                byte r = (byte)Tiff.GetR(rgba[sourceY * width + x]);
                byte g = (byte)Tiff.GetG(rgba[sourceY * width + x]);
                byte b = (byte)Tiff.GetB(rgba[sourceY * width + x]);
                byte a = (byte)Tiff.GetA(rgba[sourceY * width + x]);
                double value = EncodeRGBA(r, g, b, a);
                img.SetPixel(x, y, value);
            }
        }

        return img;
    }
}
