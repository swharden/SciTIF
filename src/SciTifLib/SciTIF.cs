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
        public int valuesMin;
        public int valuesMax;
        public int[] values;
        public System.Drawing.Size size { get { return new System.Drawing.Size(width, height); } }

        public Logger log;
        private Stream stream;
        public TiffBitmapDecoder decoder;
        private BitmapSource bitmapSource;

        public TifFile(string filePath)
        {
            this.filePath = filePath;
            log = new Logger("SciTIF");
            InspectImageFile();
            LoadImage();
            LoadImageData();
        }

        private void InspectImageFile()
        {
            if (filePath == null)
                throw new Exception("filePath cannot be null");

            filePath = System.IO.Path.GetFullPath(filePath);
            fileBasename = System.IO.Path.GetFileName(filePath);
            log.Info($"Loading abf file: {filePath}");

            if (!File.Exists(filePath))
                throw new Exception($"file does not exist: {filePath}");

            fileSize = new System.IO.FileInfo(filePath).Length;
        }

        private void LoadImage()
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

        private void LoadImageData(int frameNumber = 0)
        {
            // sanity checking
            if (decoder == null || frameNumber >= decoder.Frames.Count)
            {
                log.Debug("cannot load image data");
                return;
            }

            // select the frame (channel or slice) we want
            bitmapSource = decoder.Frames[frameNumber];

            // prepare variables which will be useful later
            depthImage = bitmapSource.Format.BitsPerPixel;
            int bytesPerPixel = depthImage / 8;
            width = bitmapSource.PixelWidth;
            height = bitmapSource.PixelHeight;
            int bytesPerRow = width * bytesPerPixel;
            int imageByteCount = height * width * bytesPerPixel;
            int imagePixelCount = width * height;

            // fill a byte array with source data from the frame of interest
            byte[] bytesSource = new byte[imageByteCount];
            bitmapSource.CopyPixels(bytesSource, bytesPerRow, 0);

            // Convert the byte array to an array of integers (according to bytesPerPixel)
            values = new int[imagePixelCount];
            for (int i = 0; i < values.Length; i++)
            {
                int bytePosition = i * bytesPerPixel;
                for (int byteNumber = 0; byteNumber < bytesPerPixel; byteNumber++)
                {
                    values[i] += bytesSource[bytePosition + byteNumber] << (byteNumber * 8);
                }
            }

            // populate stats about image
            valuesMin = values.Min();
            valuesMax = values.Max();

            // predict what bit depth we have based upon pixelValueMax
            depthData = 1;
            while (Math.Pow(2, depthData) < valuesMax)
                depthData++;
            log.Debug($"detected data depth: {depthData}-bit");
        }

        public string Info()
        {
            string msg = $"File: {System.IO.Path.GetFileName(filePath)}\n";
            msg += $"Full Path: {filePath}\n";
            return msg;
        }

        private ColorPalette PaletteGrayscale(Bitmap bmp)
        {
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < 256; i++)
                pal.Entries[i] = System.Drawing.Color.FromArgb(255, i, i, i);
            return pal;
        }

        public Bitmap GetBitmapForDisplay2(double brightness = 0, double contrast = 0)
        {
            if (decoder == null)
                return null;

            // prepare a bitmap to hold the display image
            var format = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            Rectangle rect = new Rectangle(0, 0, width, height);
            Bitmap bmpDisplay = new Bitmap(width, height, format);

            // create a byte array to hold RGB values for the display image
            int bytesPerPixel = 3;
            int byteCount = width * height * bytesPerPixel;
            byte[] bmpBytes = new byte[byteCount];

            // prepare numbers now so we don't have to do it in the loop
            double pixelValueBlack = 0;
            double pixelValueWhite = Math.Pow(2, depthData);
            double brightnessSensitivity = 1000;
            double contrastSensitivity = 0.01;

            // set the display value according to the source image intensity
            for (int i = 0; i < values.Length; i++)
            {
                // use floating point numbers to convert data scale from 0 to 1
                double pixelValue = values[i];
                pixelValue = pixelValue - pixelValueBlack;
                pixelValue /= (pixelValueWhite - pixelValueBlack);

                // apply brightness as a percentage
                pixelValue += (brightness / brightnessSensitivity);

                // apply contrast as a percentage
                double diffFromCenter = .5 - pixelValue;
                diffFromCenter *= (1 + contrast * contrastSensitivity);
                pixelValue = .5 - diffFromCenter;

                // ensure values don't go over our limits before assigning them
                byte valByte = (byte)(Math.Max(0, Math.Min(255, pixelValue * 255)));
                int bytePosition = i * bytesPerPixel;

                // TODO: this is where a LUT feature could be added
                bmpBytes[bytePosition + 2] = valByte; // red
                bmpBytes[bytePosition + 1] = valByte; // green
                bmpBytes[bytePosition + 0] = valByte; // blue
            }

            // Use marshal copy as a safe (pointer-free) way to get the pixel bytes into the bitmap
            BitmapData bmpData = bmpDisplay.LockBits(rect, ImageLockMode.ReadWrite, format);
            Marshal.Copy(bmpBytes, 0, bmpData.Scan0, byteCount);
            bmpDisplay.UnlockBits(bmpData);

            return bmpDisplay;
        }

        public Bitmap GetBitmapForDisplay()
        {

            // create and fill a pixel array for the 8-bit final image
            byte[] pixelsOutput = new byte[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                // start by loading the pixel value of the source
                int pixelValue = values[i];

                // upshift it to the nearest byte (if using a nonstandard depth)
                pixelValue = pixelValue << (depthImage - depthData);

                // downshift it as needed to ensure the MSB is in the lowest 8 bytes
                pixelValue = pixelValue >> (depthImage - 8);

                // conversion to 8-bit should now be non-destructive
                pixelsOutput[i] = (byte)(pixelValue);
            }

            // create the output bitmap (8-bit indexed color)
            var format = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
            Bitmap bmpDisplay = new Bitmap(width, height, format);
            bmpDisplay.Palette = PaletteGrayscale(bmpDisplay);

            // copy the new pixel data into the data of our output bitmap
            var rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmpDisplay.LockBits(rect, ImageLockMode.ReadOnly, format);
            Marshal.Copy(pixelsOutput, 0, bmpData.Scan0, pixelsOutput.Length);
            bmpDisplay.UnlockBits(bmpData);

            // for testing add a border
            bmpDisplay = GetBitmapAsARGB(bmpDisplay);
            bmpDisplay = OutlineBitmap(bmpDisplay, System.Drawing.Color.Blue);

            // return the 8-bit preview bitmap we created
            return bmpDisplay;
        }

        public Bitmap GetBitmapAsARGB(Bitmap bmpOriginal)
        {
            var newFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            Bitmap bmpARGB = new Bitmap(width, height, newFormat);
            Graphics gfxARGB = Graphics.FromImage(bmpARGB);
            gfxARGB.DrawImage(bmpOriginal, 0, 0);
            gfxARGB.Dispose();
            return bmpARGB;
        }

        private Bitmap OutlineBitmap(Bitmap bmp, System.Drawing.Color color, int lineWidth = 1)
        {
            System.Drawing.Pen pen = new System.Drawing.Pen(color, lineWidth);
            Graphics gfx = Graphics.FromImage(bmp);
            gfx = Graphics.FromImage(bmp);
            Rectangle rect = new Rectangle(0, 0, width - 1, height - 1);
            gfx.DrawRectangle(pen, rect);
            gfx.Dispose();
            return bmp;
        }
    }
}
