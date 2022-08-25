using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SciTIF.IO;

public static class SystemDrawing
{
    public static Bitmap GetBitmapGrayscale(Image img)
    {
        int width = img.Width;
        int height = img.Height;

        int stride = (width % 4 == 0) ? width : width + 4 - width % 4;

        byte[] bytes = new byte[stride * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                bytes[y * stride + x] = img.GetPixelByte(x, y, true);

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

    public static Bitmap GetBitmapRGB(Image r, Image g, Image b)
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
                int destOffset = (y * stride + x) * bytesPerPixel;
                bytes[destOffset + 0] = b.GetPixelByte(x, y, true);
                bytes[destOffset + 1] = g.GetPixelByte(x, y, true);
                bytes[destOffset + 2] = r.GetPixelByte(x, y, true);
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

    public static void SavePNG(string filePath, Image img)
    {
        using Bitmap bmp = SystemDrawing.GetBitmapGrayscale(img);
        bmp.Save(filePath, ImageFormat.Png);
    }

    public static void SavePNG(string filePath, Image r, Image g, Image b)
    {
        using Bitmap bmp = SystemDrawing.GetBitmapRGB(r, g, b);
        bmp.Save(filePath, ImageFormat.Png);
    }
}
