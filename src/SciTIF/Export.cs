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
        if (tif.Slices.Length == 1)
        {
            ImageData img = tif.Slices[0];
            if (autoScale)
                Adjust.AutoScale(img);

            PNG(filePath, img);
            return;
        }
        else if (tif.Slices.Length >= 3)
        {
            ImageData r = tif.Slices[0];
            ImageData g = tif.Slices[1];
            ImageData b = tif.Slices[2];

            if (autoScale)
            {
                Adjust.AutoScale(r);
                Adjust.AutoScale(g);
                Adjust.AutoScale(b);
            }

            PNG(filePath, r, g, b);
            return;
        }
        else
        {
            throw new InvalidOperationException("unsupported number of channels");
        }
    }

    public static void PNG(string filePath, ImageData img)
    {
        using Bitmap bmp = GetBitmapGrayscale(img);
        bmp.Save(filePath, ImageFormat.Png);
    }

    public static void PNG(string filePath, ImageData r, ImageData g, ImageData b)
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

    private static Bitmap GetBitmapGrayscale(ImageData img)
    {
        int width = img.Width;
        int height = img.Height;
        double[] values = img.Values;

        int stride = (width % 4 == 0) ? width : width + 4 - width % 4;

        byte[] bytes = new byte[stride * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                bytes[y * stride + x] = Clamp(values[width * y + x]);

        PixelFormat formatOutput = PixelFormat.Format8bppIndexed;

        Rectangle rect = new(0, 0, width, height);
        using Bitmap bmp = new(stride, height, formatOutput);
        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, formatOutput);
        Marshal.Copy(bytes, 0, bmpData.Scan0, bytes.Length);
        bmp.UnlockBits(bmpData);

        ColorPalette pal = bmp.Palette;
        for (int i = 0; i < 256; i++)
            pal.Entries[i] = Color.FromArgb(255, i, i, i);
        bmp.Palette = pal;

        Bitmap bmp2 = new(width, height, PixelFormat.Format32bppPArgb);
        Graphics gfx2 = Graphics.FromImage(bmp2);
        gfx2.DrawImage(bmp, 0, 0);

        return bmp2;
    }

    // TODO: implement with SkiaSharp
    private static Bitmap GetBitmapRGB(ImageData r, ImageData g, ImageData b)
    {
        int width = r.Width;
        int height = r.Height;
        int stride = (width % 4 == 0) ? width : width + 4 - width % 4;
        int bytesPerPixel = 3;

        byte[] bytes = new byte[stride * height * bytesPerPixel];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int sourceOffset = y * width + x;
                int destOffset = (y * stride + x) * bytesPerPixel;
                bytes[destOffset + 0] = Clamp(b.Values[sourceOffset]);
                bytes[destOffset + 1] = Clamp(g.Values[sourceOffset]);
                bytes[destOffset + 2] = Clamp(r.Values[sourceOffset]);
            }
        }

        PixelFormat formatOutput = PixelFormat.Format24bppRgb;
        Rectangle rect = new(0, 0, width, height);
        Bitmap bmp = new(stride, height, formatOutput);
        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, formatOutput);
        Marshal.Copy(bytes, 0, bmpData.Scan0, bytes.Length);
        bmp.UnlockBits(bmpData);

        Bitmap bmp2 = new(width, height, PixelFormat.Format32bppPArgb);
        Graphics gfx2 = Graphics.FromImage(bmp2);
        gfx2.DrawImage(bmp, 0, 0);

        return bmp2;
    }
}
