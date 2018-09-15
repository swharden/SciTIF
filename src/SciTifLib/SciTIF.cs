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
        public string fileBasename;
        public long fileSize;
        public int depthImage;
        public int depthData;
        public int width;
        public int height;
        public int min;
        public int max;
        public System.Drawing.Size size { get { return new System.Drawing.Size(width, height); } }

        public Logger log;
        private Stream stream;
        public TiffBitmapDecoder decoder;
        private BitmapSource bitmapSource;

        public TifFile(string filePath)
        {
            if (filePath == null)
                throw new Exception("filePath cannot be null");
            filePath = System.IO.Path.GetFullPath(filePath);
            this.filePath = filePath;
            this.fileBasename = System.IO.Path.GetFileName(filePath);
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

        /// <summary>
        /// Generate an 8-bit grayscale image suitable for display.
        /// Returned data may be degraded due to quantization error.
        /// </summary>
        public Bitmap GetBitmapForDisplay(int frameNumber = 0)
        {
            // ensure a TIF is loaded
            if (decoder == null) return null;

            // select the frame (channel or slice) we want
            if (frameNumber >= decoder.Frames.Count) return null;

            // prepare variables which will be useful later
            int sourceImageDepth = bitmapSource.Format.BitsPerPixel;
            int bytesPerPixel = sourceImageDepth / 8;
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = width * bytesPerPixel;
            int imageByteCount = height * width * bytesPerPixel;
            int pixelCount = width * height;

            // fill our byte array with source data
            byte[] bytesSource = new byte[imageByteCount];
            bitmapSource.CopyPixels(bytesSource, stride, 0);

            // Fill an int array with data from the byte array according to bytesPerPixel
            int[] valuesSource = new int[pixelCount];
            for (int i = 0; i < valuesSource.Length; i++)
            {
                int bytePosition = i * bytesPerPixel;
                for (int byteNumber = 0; byteNumber < bytesPerPixel; byteNumber++)
                {
                    valuesSource[i] += bytesSource[bytePosition + byteNumber] << (byteNumber * 8);
                }
            }

            // determine the range of intensity data
            int pixelValueMin = valuesSource.Min();
            int pixelValueMax = valuesSource.Max();
            log.Debug($"pixel value min: {pixelValueMin}");
            log.Debug($"pixel value max: {pixelValueMax}");

            // predict what bit depth we have based upon pixelValueMax
            int dataDepth = 1;
            while (Math.Pow(2, dataDepth) < pixelValueMax)
                dataDepth++;
            log.Debug($"detected data depth: {dataDepth}-bit");

            // determine if we will use the original bit depth or our guessed bit depth
            bool use_detected_camera_depth = true; // should this be an argument?
            if (!use_detected_camera_depth)
                dataDepth = sourceImageDepth;

            // create and fill a pixel array for the 8-bit final image
            byte[] pixelsOutput = new byte[height * width];
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
            Bitmap bmpIndexed8 = new Bitmap(width, height, format);

            // Create a grayscale palette, although other colors and LUTs could go here
            ColorPalette pal = bmpIndexed8.Palette;
            for (int i = 0; i < 256; i++)
                pal.Entries[i] = System.Drawing.Color.FromArgb(255, i, i, i);
            bmpIndexed8.Palette = pal;

            // copy the new pixel data into the data of our output bitmap
            var rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmpIndexed8.LockBits(rect, ImageLockMode.ReadOnly, format);
            Marshal.Copy(pixelsOutput, 0, bmpData.Scan0, pixelsOutput.Length);
            bmpIndexed8.UnlockBits(bmpData);

            // update class variables with that we discovered
            this.depthData = dataDepth;
            this.depthImage = sourceImageDepth;
            this.width = width;
            this.height = height;
            this.min = pixelValueMin;
            this.max = pixelValueMax;

            // create a non-indexed version of this image
            Bitmap bmpARGB = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gfxARGB = Graphics.FromImage(bmpARGB);
            gfxARGB.DrawImage(bmpIndexed8, 0, 0);
            gfxARGB.Dispose();

            // return the 8-bit preview bitmap we created
            return bmpARGB;
        }
        
        public Bitmap OutlineBitmap(Bitmap bmp, System.Drawing.Color color)
        {
            System.Drawing.Pen blackPen = new System.Drawing.Pen(color, 5);
            Graphics gfx = Graphics.FromImage(bmp);
            gfx = Graphics.FromImage(bmp);
            gfx.DrawRectangle(blackPen, 0, 0, bmp.Width - 1, bmp.Height - 1);
            gfx.Dispose();
            return bmp;
        }
    }
}
