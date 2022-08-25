using BitMiracle.LibTiff.Classic;
using System;

namespace SciTIF.IO.TiffReading;

internal class ReaderFloat32 : ReaderBase
{
    public override bool IsRGBA => false;

    public override Image ReadSlice(Tiff tif)
    {

        const int bytesPerPixel = 4;

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        Image data = new(width, height);

        byte[] lineBytes = new byte[tif.ScanlineSize()];
        byte[] pixelBytes = new byte[bytesPerPixel];
        for (int y = 0; y < height; y++)
        {
            tif.ReadScanline(lineBytes, y);
            for (int x = 0; x < width; x++)
            {
                Array.Copy(lineBytes, x * bytesPerPixel, pixelBytes, 0, bytesPerPixel);
                int i = data.GetIndex(x, y);
                data[i] = BitConverter.ToSingle(pixelBytes, 0);
            }
        }

        return data;
    }
}
