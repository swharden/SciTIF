using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SciTIF;

public static class Export
{
    public static void PNG(string filePath, double[,] values)
    {
        using Bitmap bmp = GetBitmap(values);
        bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
    }

    private static byte Clamp(double x, byte min = 0, byte max = 255)
    {
        if (x < min)
            return min;
        else if (x > max)
            return max;
        else
            return (byte)x;
    }

    // TODO: implement with SkiaSharp
    private static Bitmap GetBitmap(double[,] values)
    {
        int width = values.GetLength(1);
        int height = values.GetLength(0);
        int stride = (width % 4 == 0) ? width : width + 4 - width % 4;

        byte[] pixelsOutput = new byte[stride * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                pixelsOutput[y * stride + x] = Clamp(values[y, x]);

        var formatOutput = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;

        var rect = new Rectangle(0, 0, width, height);
        Bitmap bmp = new(stride, height, formatOutput);
        System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, formatOutput);
        System.Runtime.InteropServices.Marshal.Copy(pixelsOutput, 0, bmpData.Scan0, pixelsOutput.Length);
        bmp.UnlockBits(bmpData);

        System.Drawing.Imaging.ColorPalette pal = bmp.Palette;
        for (int i = 0; i < 256; i++)
            pal.Entries[i] = System.Drawing.Color.FromArgb(255, i, i, i);
        bmp.Palette = pal;

        return bmp;
    }
}
