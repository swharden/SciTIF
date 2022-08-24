using BitMiracle.LibTiff.Classic;

namespace SciTIF.IO.TiffReading;

internal class ReaderInt8 : ReaderBase
{
    public override bool IsRGBA => false;

    public override Image ReadSlice(Tiff tif)
    {
        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        Image data = new(width, height);

        int numberOfStrips = tif.NumberOfStrips();
        int stripSize = tif.StripSize();

        byte[] bytes = new byte[numberOfStrips * stripSize];
        for (int i = 0; i < numberOfStrips; ++i)
        {
            tif.ReadRawStrip(i, bytes, i * stripSize, stripSize);
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int offset = y * width + x;
                int i = data.GetIndex(x, y);
                data.Values[i] = bytes[offset];
            }
        }

        return data;
    }
}
