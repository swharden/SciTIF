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
        public string filePath;
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
            if (decoder == null)
            {
                log.Debug("LoadImageData aborting early - no decoder");
                return;
            }

            // select the frame (channel or slice) we want
            bitmapSource = decoder.Frames[frameNumber];

            // prepare variables which will be useful later
            imageDepth = bitmapSource.Format.BitsPerPixel;
            int bytesPerPixel = imageDepth / 8;
            imageWidth = bitmapSource.PixelWidth;
            imageHeight = bitmapSource.PixelHeight;
            imageSize = new Size(imageWidth, imageHeight);
            int bytesPerRow = imageWidth * bytesPerPixel;
            int imageByteCount = imageHeight * imageWidth * bytesPerPixel;
            int imagePixelCount = imageWidth * imageHeight;

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

            // predict what bit depth we have based upon pixelValueMax
            valuesDepth = 1;
            while (Math.Pow(2, valuesDepth) < valuesMax)
                valuesDepth++;
            log.Debug($"detected data depth: {valuesDepth}-bit");

            // load data into the display object
            imageDisplay = new ImageDisplay(valuesRaw, imageWidth, imageHeight, imageDepth);

            log.Debug("data loaded successfully");
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
                pal.Entries[i] = Color.FromArgb(255, i, i, i);
            return pal;
        }
        
        public Bitmap GetBitmap()
        {
            if (decoder == null || imageWidth == 0 || imageHeight == 0)
                return null;
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
