using BitMiracle.LibTiff.Classic;

namespace SciTIF.IO.TifReading;

internal class ReaderInt16 : ReaderBase
{
    public override bool IsRGBA => false;

    public override Image ReadSlice(Tiff tif)
    {
        const int bytesPerPixel = 2;

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

        int msbOffset = tif.IsBigEndian() ? 0 : 1;
        int lsbOffset = tif.IsBigEndian() ? 1 : 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int offset = (y * width + x) * bytesPerPixel;
                int i = data.GetIndex(x, y);
                data[i] += bytes[offset + lsbOffset];
                data[i] += bytes[offset + msbOffset] << 8;
            }
        }

        return data;
    }
}
