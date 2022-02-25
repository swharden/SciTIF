using BitMiracle.LibTiff.Classic;
using System;

namespace SciTIF.TifReaders;

internal class ReaderFloat32 : IReadGrayscale
{
    public ImageData ReadGrayscale(Tiff tif)
    {
        const int bytesPerPixel = 4;

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        double[,] pixelValues = new double[height, width];

        byte[] lineBytes = new byte[tif.ScanlineSize()];
        byte[] pixelBytes = new byte[bytesPerPixel];
        for (int y = 0; y < height; y++)
        {
            tif.ReadScanline(lineBytes, y);
            for (int x = 0; x < width; x++)
            {
                Array.Copy(lineBytes, x * bytesPerPixel, pixelBytes, 0, bytesPerPixel);
                pixelValues[y, x] = BitConverter.ToSingle(pixelBytes, 0);
            }
        }

        return new ImageData(pixelValues);
    }
}
