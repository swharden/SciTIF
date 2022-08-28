using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;
using System.IO;

namespace SciTIF.IO;

public static class SystemDrawing
{
    public static Bitmap GetBitmap(Image img)
    {
        return GetBitmapGrayscale(img);
    }

    public static Bitmap GetBitmap(Image r, Image g, Image b)
    {
        return GetBitmapRGB(r, g, b);
    }

    private static Bitmap GetBitmapGrayscale(Image img)
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

    private static Bitmap GetBitmapRGB(Image r, Image g, Image b)
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

    public static void Save(string path, Image img, int quality)
    {
        if (path.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
        {
            SavePNG(path, img);
        }
        else if (path.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase))
        {
            SaveJPG(path, img, quality);
        }
        else
        {
            throw new NotImplementedException($"Unsupported extension: {Path.GetFileName(path)}");
        }
    }

    public static void Save(string path, Image red, Image green, Image blue, int quality)
    {
        if (path.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
        {
            SavePNG(path, red, green, blue);
        }
        else if (path.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase))
        {
            SaveJPG(path, red, green, blue, quality);
        }
        else
        {
            throw new NotImplementedException($"Unsupported extension: {Path.GetFileName(path)}");
        }
    }

    private static void SavePNG(string filePath, Image img)
    {
        using Bitmap bmp = GetBitmapGrayscale(img);
        bmp.SavePNG(filePath);
    }

    private static void SavePNG(string filePath, Image r, Image g, Image b)
    {
        using Bitmap bmp = GetBitmapRGB(r, g, b);
        bmp.SavePNG(filePath);
    }

    private static void SaveJPG(string filePath, Image img, int quality)
    {
        using Bitmap bmp = GetBitmapGrayscale(img);
        bmp.SaveJPG(filePath, quality);
    }

    private static void SaveJPG(string filePath, Image r, Image g, Image b, int quality)
    {
        using Bitmap bmp = GetBitmapRGB(r, g, b);
        bmp.SaveJPG(filePath, quality);
    }

    private static void SavePNG(this Bitmap bmp, string filePath)
    {
        bmp.Save(filePath, ImageFormat.Png);
    }

    private static void SaveJPG(this Bitmap bmp, string filePath, int quality)
    {
        var jpegEncoders = ImageCodecInfo.GetImageEncoders().Where(x => x.FormatID == ImageFormat.Jpeg.Guid);
        if (!jpegEncoders.Any())
            throw new InvalidOperationException("JPEG codec not found");

        ImageCodecInfo jpgEncoder = jpegEncoders.First();
        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
        EncoderParameter myEncoderParameter = new(myEncoder, quality);
        EncoderParameters myEncoderParameters = new(1);
        myEncoderParameters.Param[0] = myEncoderParameter;

        bmp.Save(filePath, jpgEncoder, myEncoderParameters);
    }
}
