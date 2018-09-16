using System;
using System.Linq;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace SciTIFlib
{
    public class TifFile
    {
        public readonly string filePath;
        public string fileBasename;
        public long fileSize;
        public int imageDepth;
        public int imageWidth;
        public int imageHeight;
        public Size imageSize;
        public int valuesDepth;
        public int valuesMin;
        public int valuesMax;
        public int[] valuesRaw;
        public double[] values;


        private Logger log;
        private Stream stream;
        private TiffBitmapDecoder decoder;
        private BitmapSource bitmapSource;
        public ImageDisplay imageDisplay;

        //////////////////////////////////////////////////////////////////
        // IMAGE LOADING 

        public TifFile(string filePath)
        {
            this.filePath = System.IO.Path.GetFullPath(filePath);
            if (!File.Exists(this.filePath))
                throw new Exception("invalid file path: {this.filePath}");
            log = new Logger("SciTIF");
            LoadFrame(0);
        }

        private void LoadFrame(int frameNumber = 0)
        {

            // decode the image with TiffBitmapDecoder
            log.Debug("starting file stream");
            stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            try { decoder = new TiffBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default); }
            catch { throw new Exception($"TiffBitmapDecoder crashed while loading: {filePath}"); }
            log.Debug("TiffBitmapDecoder is being used for this image");
            fileSize = new System.IO.FileInfo(filePath).Length;
            log.Debug($"File size: {fileSize + 1} bytes ({Math.Round(fileSize / 1e6, 2)} MB)");

            // select the frame (channel or slice) we want
            log.Debug($"Decoding frame: {frameNumber + 1} of {decoder.Frames.Count}");
            bitmapSource = decoder.Frames[frameNumber];

            // prepare variables which will be useful later
            imageDepth = bitmapSource.Format.BitsPerPixel;
            int bytesPerPixel = imageDepth / 8;
            log.Debug($"bytes per pixel: {bytesPerPixel}");
            imageWidth = bitmapSource.PixelWidth;
            imageHeight = bitmapSource.PixelHeight;
            imageSize = new Size(imageWidth, imageHeight);
            int bytesPerRow = imageWidth * bytesPerPixel;
            int imageByteCount = imageHeight * imageWidth * bytesPerPixel;
            int imagePixelCount = imageWidth * imageHeight;
            log.Debug($"Image dimensions: {imageWidth} x {imageHeight}");

            // fill a byte array with source data from the frame of interest
            byte[] bytesSource = new byte[imageByteCount];
            bitmapSource.CopyPixels(bytesSource, bytesPerRow, 0);

            // use an int array to hold our original data
            valuesRaw = new int[imagePixelCount];
            for (int i = 0; i < valuesRaw.Length; i++)
            {
                int bytePosition = i * bytesPerPixel;
                for (int byteNumber = 0; byteNumber < bytesPerPixel; byteNumber++)
                {
                    valuesRaw[i] += bytesSource[bytePosition + byteNumber] << (byteNumber * 8);
                }
            }

            // determine data value extremes
            valuesMin = valuesRaw.Min();
            valuesMax = valuesRaw.Max();
            log.Debug($"extreme data values (min, max): ({valuesMin}, {valuesMax})");

            // predict what bit depth we have based upon maximum pixel value
            valuesDepth = 1;
            while (Math.Pow(2, valuesDepth) < valuesMax)
                valuesDepth++;
            log.Debug($"detected data depth: {valuesDepth}-bit");

            // load data into the display object
            imageDisplay = new ImageDisplay(valuesRaw, imageWidth, imageHeight, imageDepth);

            log.Debug("data values loaded successfully");
        }

        //////////////////////////////////////////////////////////////////
        // EXTERNAL 

        public string Info()
        {
            return log.logText;
        }


        //////////////////////////////////////////////////////////////////
        // INTERNAL

        public Bitmap GetBitmap()
        {
            /*
            if (decoder == null || imageWidth == 0 || imageHeight == 0)
                return null;
                */
            return imageDisplay.ValuesToBitmap();
        }

        public Bitmap GetBitmapAsARGB(Bitmap bmpOriginal)
        {
            var newFormat = PixelFormat.Format32bppArgb;
            Bitmap bmpARGB = new Bitmap(imageWidth, imageHeight, newFormat);
            Graphics gfxARGB = Graphics.FromImage(bmpARGB);
            gfxARGB.DrawImage(bmpOriginal, 0, 0);
            gfxARGB.Dispose();
            return bmpARGB;
        }

        private Bitmap OutlineBitmap(Bitmap bmp, Color color, int lineWidth = 1)
        {
            Pen pen = new Pen(color, lineWidth);
            Graphics gfx = Graphics.FromImage(bmp);
            gfx = Graphics.FromImage(bmp);
            Rectangle rect = new Rectangle(0, 0, imageWidth - 1, imageHeight - 1);
            gfx.DrawRectangle(pen, rect);
            gfx.Dispose();
            return bmp;
        }
    }
}
