using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sandbox3
{

    class FrameSet
    {
        public Bitmap bmpOriginal;
        public Frame[] frames;
        public FrameSet(string imageFilePath)
        {
            bmpOriginal = new Bitmap(imageFilePath);

            // create int16 arrays of pixels for R, G, and B channels
            int nPixels = bmpOriginal.Width * bmpOriginal.Height;
            byte[] bytes = BitmapToBytes(bmpOriginal);
            UInt16[] pixelsR = new UInt16[nPixels];
            UInt16[] pixelsG = new UInt16[nPixels];
            UInt16[] pixelsB = new UInt16[nPixels];
            for (int pixel = 0; pixel < nPixels; pixel++)
            {
                pixelsR[pixel] = bytes[3 * pixel + 2];
                pixelsG[pixel] = bytes[3 * pixel + 1];
                pixelsB[pixel] = bytes[3 * pixel + 0];
            }

            // load these arrays into frames
            frames = new Frame[3];
            frames[0] = new Frame(pixelsR, bmpOriginal.Size, "red");
            frames[1] = new Frame(pixelsG, bmpOriginal.Size, "green");
            frames[2] = new Frame(pixelsB, bmpOriginal.Size, "blue");
        }

        /// <summary>
        /// return a bitmap (of any image format) as a byte array of its data values
        /// </summary>
        public byte[] BitmapToBytes(Bitmap bmp)
        {
            int bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            byte[] bytes = new byte[bmp.Width * bmp.Height * bytesPerPixel];
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
            Marshal.Copy(bmpData.Scan0, bytes, 0, bytes.Length);
            bmp.UnlockBits(bmpData);
            return bytes;
        }

        /// <summary>
        /// create a bitmap given a byte array of raw data
        /// </summary>
        public Bitmap BitmapFromBytes(byte[] bytes, Size size, PixelFormat format = PixelFormat.Format24bppRgb)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height, format);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            Marshal.Copy(bytes, 0, bmpData.Scan0, bytes.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        public Bitmap AddChannel(Bitmap bmp, Frame frame, string forceColor="")
        {
            if (frame.visible == false)
                return bmp;

            string color = frame.color;
            if (forceColor != "")
                color = forceColor;

            byte[] bytes = BitmapToBytes(bmp);
            for (int i = 0; i < frame.pixels.Length; i++)
            {
                byte val = (byte)frame.pixels[i];
                if (color == "gray")
                {
                    bytes[i * 3 + 2] = val; // red
                    bytes[i * 3 + 1] = val; // green
                    bytes[i * 3 + 0] = val; // blue
                }
                else if (color == "red")
                {
                    bytes[i * 3 + 2] = val; // red
                }
                else if (color == "green")
                {
                    bytes[i * 3 + 1] = val; // green
                }
                else if (color == "blue")
                {
                    bytes[i * 3 + 0] = val; // blue
                }
                else if (color == "magenta")
                {
                    bytes[i * 3 + 2] = val; // red
                    bytes[i * 3 + 0] = val; // blue
                }
            }
            bmp = BitmapFromBytes(bytes, bmp.Size);
            return bmp;
        }

        /// <summary>
        /// return a black image you can add color channels to
        /// </summary>
        public Bitmap BlankImage()
        {
            Bitmap bmp = new Bitmap(bmpOriginal.Width, bmpOriginal.Height, PixelFormat.Format24bppRgb);
            return bmp;
        }
    }
}
