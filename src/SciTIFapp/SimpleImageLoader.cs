﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SciTIFapp
{
    class SimpleImageLoader
    {
        public Bitmap bmpPreview;
        public readonly string path;

        public SimpleImageLoader(string path)
        {
            this.path = path;
            if (path.ToUpper().EndsWith(".TIF") || path.ToUpper().EndsWith(".TIFF"))
                bmpPreview = LoadImageTiff(path);
            else
                bmpPreview = LoadImageNonTiff(path);
        }

        private Bitmap LoadImageNonTiff(string path)
        {
            return new Bitmap(path);
        }

        /// <summary>
        /// This function takes a TIFF frame of any bit depth and returns a proper 8-bit bitmap
        /// </summary>
        private Bitmap LoadImageTiff(string path, int frameNumber = 0)
        {
            //bmpPreview = new Bitmap(path);

            // open a file stream and keep it open until we're done reading the file
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            // carefully open the file to see if it will decode
            TiffBitmapDecoder decoder;
            try
            {
                decoder = new TiffBitmapDecoder(stream,
                    BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            }
            catch
            {
                Console.WriteLine("TiffBitmapDecoder crashed");
                stream.Dispose();
                return null;
            }

            // access information about the image
            int imageFrames = decoder.Frames.Count;
            BitmapSource bitmapSource = decoder.Frames[frameNumber];
            int sourceImageDepth = bitmapSource.Format.BitsPerPixel;
            int bytesPerPixel = sourceImageDepth / 8;
            Size imageSize = new Size(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
            int pixelCount = imageSize.Width * imageSize.Height;

            // fill a byte array with source data bytes from the file
            int imageByteCount = pixelCount * bytesPerPixel;
            byte[] bytesSource = new byte[imageByteCount];
            bitmapSource.CopyPixels(bytesSource, imageSize.Width * bytesPerPixel, 0);

            // we can now close the original file
            stream.Dispose();

            // now convert the byte array to an int array (with 1 int per pixel)
            int[] valuesSource = new int[pixelCount];
            for (int i = 0; i < valuesSource.Length; i++)
            {
                // this loop is great because it works on any number of bytes per pixel
                int bytePosition = i * bytesPerPixel;
                for (int byteNumber = 0; byteNumber < bytesPerPixel; byteNumber++)
                {
                    valuesSource[i] += bytesSource[bytePosition + byteNumber] << (byteNumber * 8);
                }
            }

            // determine the range of intensity data
            int pixelValueMax = valuesSource.Max();
            int pixelValueMin = valuesSource.Min();

            // predict what bit depth we have based upon pixelValueMax
            int dataDepth = 1;
            while (Math.Pow(2, dataDepth) < pixelValueMax)
                dataDepth++;

            // determine if we will use the original bit depth or our guessed bit depth
            bool use_detected_camera_depth = true; // should this be an argument?
            if (!use_detected_camera_depth)
                dataDepth = sourceImageDepth;

            // create and fill a pixel array for the 8-bit final image
            byte[] pixelsOutput = new byte[pixelCount];
            for (int i = 0; i < pixelsOutput.Length; i++)
            {
                // start by loading the pixel value of the source
                int pixelValue = valuesSource[i];

                // upshift it to the nearest byte (if using a nonstandard depth)
                pixelValue = pixelValue << (sourceImageDepth - dataDepth);

                // downshift it as needed to ensure the MSB is in the lowest 8 bytes
                pixelValue = pixelValue >> (sourceImageDepth - 8);

                // conversion to 8-bit should be now nondestructive
                pixelsOutput[i] = (byte)(pixelValue);
            }

            // create the output bitmap (8-bit indexed color)
            var format = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
            Bitmap bmp = new Bitmap(imageSize.Width, imageSize.Height, format);

            // Create a grayscale palette, although other colors and LUTs could go here
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < 256; i++)
                pal.Entries[i] = System.Drawing.Color.FromArgb(255, i, i, i);
            bmp.Palette = pal;

            // copy the new pixel data into the data of our output bitmap
            var rect = new Rectangle(0, 0, imageSize.Width, imageSize.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, format);
            Marshal.Copy(pixelsOutput, 0, bmpData.Scan0, pixelsOutput.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }
    }
}
