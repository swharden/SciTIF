using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace SciTIF.IO;

public static class SystemDrawing
{
    public static Bitmap GetBitmapGrayscale(GrayscaleImage img)
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

    public static Bitmap GetBitmapRGB(GrayscaleImage r, GrayscaleImage g, GrayscaleImage b)
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

    private static byte Clamp(double x, byte min = 0, byte max = 255)
    {
        if (x < min)
            return min;
        else if (x > max)
            return max;
        else
            return (byte)x;
    }

    public static void SavePNG(string filePath, TifFile tif, bool autoScale = false)
    {
        throw new NotImplementedException();
    }

    public static void SavePNG(string filePath, GrayscaleImage img)
    {
        using Bitmap bmp = SystemDrawing.GetBitmapGrayscale(img);
        bmp.Save(filePath, ImageFormat.Png);
    }

    public static void SavePNG(string filePath, GrayscaleImage r, GrayscaleImage g, GrayscaleImage b)
    {
        using Bitmap bmp = SystemDrawing.GetBitmapRGB(r, g, b);
        bmp.Save(filePath, ImageFormat.Png);
    }
}
