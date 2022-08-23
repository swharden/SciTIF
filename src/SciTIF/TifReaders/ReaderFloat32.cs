using BitMiracle.LibTiff.Classic;
using System;
using System.Linq;

namespace SciTIF.TifReaders;

internal class ReaderFloat32 : ITifReader
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

        const int bytesPerPixel = 4;

        int width = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
        int height = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
        ImageData data = new(width, height);

        byte[] lineBytes = new byte[tif.ScanlineSize()];
        byte[] pixelBytes = new byte[bytesPerPixel];
        for (int y = 0; y < height; y++)
        {
            tif.ReadScanline(lineBytes, y);
            for (int x = 0; x < width; x++)
            {
                Array.Copy(lineBytes, x * bytesPerPixel, pixelBytes, 0, bytesPerPixel);
                int i = data.GetIndex(x, y);
                data.Values[i] = BitConverter.ToSingle(pixelBytes, 0);
            }
        }

        return data;
    }
}
