using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows;
using System.Runtime.InteropServices; // for Marshal
using System.Drawing.Imaging;

namespace SciTIFlib
{
    public class TifFile
    {
        public string filePath;
        public long fileSize;

        public Logger log;
        private Stream stream;
        public TiffBitmapDecoder decoder;
        private BitmapSource bitmapSource;

        public TifFile(string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            this.filePath = filePath;
            log = new Logger("SciTIF");
            log.Info($"Loading abf file: {filePath}");

            if (!File.Exists(filePath))
                throw new Exception($"file does not exist: {filePath}");
            else
                fileSize = new System.IO.FileInfo(filePath).Length;

            LoadImage();
            if (decoder != null)
            {
                LoadImageProperties();
                log.Debug("TIF read successfully");
            }
            else
            {
                log.Debug("Could not decode TIF");
            }
        }

        public void LoadImage()
        {
            log.Debug("starting file stream");
            stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                decoder = new TiffBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                log.Debug("TIF file successfully decoded");
            }
            catch
            {
                decoder = null;
                log.Debug("TiffBitmapDecoder crashed on load");
            }

        }

        public void LoadImageProperties()
        {
            bitmapSource = decoder.Frames[0];
            log.Debug($"Image depth: {bitmapSource.Format.BitsPerPixel}-bit");
            log.Debug($"image width: {bitmapSource.PixelWidth}");
            log.Debug($"image height: {bitmapSource.PixelHeight}");
            log.Debug($"number of frames: {decoder.Frames.Count}");

            if (decoder.Frames.Count > 1)
            {
                log.Warn($"multi-frame image detected - assuming RGB for now");
            }
        }

        public string Info()
        {
            string msg = $"File: {System.IO.Path.GetFileName(filePath)}\n";
            msg += $"Full Path: {filePath}\n";
            return msg;
        }

        public Bitmap GetBitmap(int frameNumber = 0)
        {
            if (decoder == null)
                return null;
            if (frameNumber >= decoder.Frames.Count)
                return null;

            // prepare an array to hold pixel values
            int sourceImageDepth = bitmapSource.Format.BitsPerPixel;
            int bytesPerPixel = sourceImageDepth / 8;
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = width * bytesPerPixel;
            int imageByteCount = height * width * bytesPerPixel;
            int pixelCount = width * height;

            log.Debug($"image pixels: {width * height}");
            log.Debug($"image bytes: {imageByteCount}");

            // copy the source data into a byte array
            byte[] bytesSource = new byte[imageByteCount];
            bitmapSource.CopyPixels(bytesSource, stride, 0);

            // convert the byte array to an int array based on depth (bytes per pixel)
            int[] valuesSource = new int[pixelCount];
            for (int i = 0; i < valuesSource.Length; i++)
            {
                int bytePosition = i * bytesPerPixel;
                for (int byteNumber = 0; byteNumber < bytesPerPixel; byteNumber++)
                {
                    valuesSource[i] += bytesSource[bytePosition + byteNumber] << (byteNumber * 8);
                }
            }

            // sanity check the values we get
            int pixelValueMax = valuesSource.Max();
            int pixelValueMin = valuesSource.Min();
            log.Debug($"pixel value max: {pixelValueMax}");
            log.Debug($"pixel value min: {pixelValueMin}");

            // if we have a nonstandard bit-depth, try to figure that out
            int dataDepth = 1;
            while (Math.Pow(2, dataDepth) < pixelValueMax)
                dataDepth++;                
            log.Debug($"data value depth: {dataDepth}-bit");

            // determine if we will use the original bit depth or our guessed bit depth
            bool use_detected_camera_depth = true;
            if (!use_detected_camera_depth)
                dataDepth = sourceImageDepth;

            // create the 8-bit pixel array (indexed color) to hold the final output image
            byte[] pixelsOutput = new byte[height * width];
            for (int i = 0; i < pixelsOutput.Length; i++)
            {
                int pixelValue = valuesSource[i];

                // upshift it if using a nonstandard depth
                pixelValue = pixelValue << (sourceImageDepth - dataDepth);

                // downshift it if needed to ensure it ends up 8-bit
                pixelValue = pixelValue >> (sourceImageDepth-8);

                pixelsOutput[i] = (byte)(pixelValue);
            }

            // create the output bitmap (8-bit indexed color with a grayscale pallette)
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            ColorPalette paletteGrayscale = bmp.Palette;
            for (int i = 0; i < 256; i++)
                paletteGrayscale.Entries[i] = System.Drawing.Color.FromArgb(255, i, i, i);
            bmp.Palette = paletteGrayscale;

            // copy our pixels byte array into the new image
            var rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
            Marshal.Copy(pixelsOutput, 0, bitmapData.Scan0, pixelsOutput.Length);
            bmp.UnlockBits(bitmapData);

            return bmp;
        }

    }
}
