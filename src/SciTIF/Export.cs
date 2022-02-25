using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace SciTIF;

public static class Export
{
    public static void PNG(string filePath, TifFile tif, bool autoScale = false)
    {
        if (tif.Channels.Length == 1)
        {
            double[,] gray = tif.Channels[0].Values;

            if (autoScale)
            {
                gray = Adjust.AutoScale(gray);
            }

            PNG(filePath, gray);
            return;
        }
        else if (tif.Channels.Length == 3 || tif.Channels.Length == 4)
        {
            double[,] r = tif.Channels[0].Values;
            double[,] g = tif.Channels[1].Values;
            double[,] b = tif.Channels[2].Values;

            if (autoScale)
            {
                r = Adjust.AutoScale(r);
                g = Adjust.AutoScale(g);
                b = Adjust.AutoScale(b);
            }

            PNG(filePath, r, g, b);
            return;
        }
        else
        {
            throw new InvalidOperationException("unsupported number of channels");
        }
    }

    public static void PNG(string filePath, double[,] values)
    {
        using Bitmap bmp = GetBitmapGrayscale(values);
        bmp.Save(filePath, ImageFormat.Png);
    }

    public static void PNG(string filePath, double[,] r, double[,] g, double[,] b)
    {
        using Bitmap bmp = GetBitmapRGB(r, g, b);
        bmp.Save(filePath, ImageFormat.Png);
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
    private static Bitmap GetBitmapGrayscale(double[,] values)
    {
        int width = values.GetLength(1);
        int height = values.GetLength(0);
        int stride = (width % 4 == 0) ? width : width + 4 - width % 4;

        byte[] bytes = new byte[stride * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                bytes[y * stride + x] = Clamp(values[y, x]);

        PixelFormat formatOutput = PixelFormat.Format8bppIndexed;
        Rectangle rect = new(0, 0, width, height);
        Bitmap bmp = new(stride, height, formatOutput);
        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, formatOutput);
        Marshal.Copy(bytes, 0, bmpData.Scan0, bytes.Length);
        bmp.UnlockBits(bmpData);

        System.Drawing.Imaging.ColorPalette pal = bmp.Palette;
        for (int i = 0; i < 256; i++)
            pal.Entries[i] = Color.FromArgb(255, i, i, i);
        bmp.Palette = pal;

        return bmp;
    }

    // TODO: implement with SkiaSharp
    private static Bitmap GetBitmapRGB(double[,] r, double[,] g, double[,] b)
    {
        int width = r.GetLength(1);
        int height = r.GetLength(0);
        int stride = (width % 4 == 0) ? width : width + 4 - width % 4;
        int bytesPerPixel = 3;

        byte[] bytes = new byte[stride * height * bytesPerPixel];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int offset = (y * stride + x) * bytesPerPixel;
                bytes[offset + 0] = Clamp(b[y, x]); // blue
                bytes[offset + 1] = Clamp(g[y, x]); // green
                bytes[offset + 2] = Clamp(r[y, x]); // red
            }
        }

        PixelFormat formatOutput = PixelFormat.Format24bppRgb;
        Rectangle rect = new(0, 0, width, height);
        Bitmap bmp = new(stride, height, formatOutput);
        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, formatOutput);
        Marshal.Copy(bytes, 0, bmpData.Scan0, bytes.Length);
        bmp.UnlockBits(bmpData);

        return bmp;
    }
}
