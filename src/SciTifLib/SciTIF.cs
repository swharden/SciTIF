using System;
using System.Linq;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace SciTIFlib
{
    public class ImageFile
    {
        public readonly string filePath;
        public int imageDepth;
        public int imageWidth;
        public int imageHeight;
        public int imageFrameCount;

        public int[] valuesRaw;
        //public double[] values; // normalized to 1

        public Logger log;
        public ImageDisplay imageDisplay;

        //////////////////////////////////////////////////////////////////
        // IMAGE LOADING 

        /// <summary>
        /// Load any type of image into memory and treat pixel intensity values as data.
        /// </summary>
        public ImageFile(string filePath)
        {
            log = new Logger("SciTIF");
            log.Info($"Loading image: {System.IO.Path.GetFileName(filePath)}");

            this.filePath = System.IO.Path.GetFullPath(filePath);
            if (!File.Exists(this.filePath))
                throw new Exception("invalid file path: {this.filePath}");

            LoadImageData();
        }

        /// <summary>
        /// Read pixel data of the TIF and populate values (the double array)
        /// </summary>
        private void LoadValuesTIFF(int frameNumber = 0)
        {
            log.Debug("Loading values from image file using the TIFF-specific method");
            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            TiffBitmapDecoder decoder;
            try
            {
                decoder = new TiffBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                log.Debug($"TiffBitmapDecoder loaded successfully");
            }
            catch
            {
                log.Debug($"TiffBitmapDecoder could not load image");
                LoadValuesNONTIFF();
                return;
            }

            // select the frame (channel or slice) we want
            log.Debug($"Decoding frame: {frameNumber + 1} of {decoder.Frames.Count}");
            BitmapSource bitmapSource = decoder.Frames[frameNumber];

            // populate class variables
            imageDepth = bitmapSource.Format.BitsPerPixel;
            imageWidth = bitmapSource.PixelWidth;
            imageHeight = bitmapSource.PixelHeight;
            imageFrameCount = decoder.Frames.Count;
            log.Debug($"Image depth: {imageDepth}-Bit");
            log.Debug($"Image shape: {imageWidth} x {imageHeight}");
            log.Debug($"Image frame count: {imageFrameCount}");

            // get the original image as an array of bytes
            int bytesPerPixel = imageDepth / 8;
            byte[] imageBytes = new byte[imageWidth * imageHeight * bytesPerPixel];
            bitmapSource.CopyPixels(imageBytes, imageWidth * bytesPerPixel, 0);
            log.Debug($"Converted image into a byte array (size: {imageBytes.Length})");

            // use an int array to hold our original data
            valuesRaw = new int[imageWidth * imageHeight];
            for (int i = 0; i < valuesRaw.Length; i++)
            {
                // this loop supports any bit depth
                int bytePosition = i * bytesPerPixel;
                for (int byteNumber = 0; byteNumber < bytesPerPixel; byteNumber++)
                {
                    valuesRaw[i] += imageBytes[bytePosition + byteNumber] << (byteNumber * 8);
                }
            }
            log.Debug($"Converted image into a non-RGB int array (size: {valuesRaw.Length})");


            // guess the depth based on the data and use this to predict the value of a white pixel
            double valuesMax = valuesRaw.Max();
            int dataDepth = 1;
            while (Math.Pow(2, dataDepth) < valuesMax)
                dataDepth++;
            double whitePixelValue = Math.Pow(2, dataDepth);
            log.Debug($"Predicted camera depth: {dataDepth}-Bit");
            log.Debug($"Brightnest pixel: {valuesMax} / {whitePixelValue} ({valuesMax / whitePixelValue * 100}%)");

            /*
            // create a float array to hold image data, normalized to 1 (by whitest pixel value)
            int nChannels = 3;
            int nValues = imageWidth * imageHeight * nChannels;
            double[] values = new double[nValues];
            for (int i = 0; i < valuesRaw.Length; i++)
            {
                int pos = i * nChannels;
                double grayValue = (double)imageBytes[i] / whitePixelValue;
                values[pos + 0] = grayValue;
                values[pos + 1] = 0;
                values[pos + 2] = 0;
            }
            log.Debug($"Converted image into an RGB double array (size: {values.Length})");
            */
        }

        /// <summary>
        /// Read pixel data of the non-TIF image and populate values (the double array)
        /// </summary>
        private void LoadValuesNONTIFF()
        {
            log.Debug("Loading values from image file using the generic method");

            // pull data from the file into the local space
            Bitmap bmpOrig = new Bitmap(filePath);
            log.Debug($"Input image format: {bmpOrig.PixelFormat}");

            // convert it to 24-bit RGB
            Bitmap bmp = new Bitmap(bmpOrig.Width, bmpOrig.Height, PixelFormat.Format24bppRgb);
            Graphics gr = Graphics.FromImage(bmp);
            gr.DrawImage(bmpOrig, new Rectangle(0, 0, bmp.Width, bmp.Height));
            gr.Dispose();
            log.Debug($"Display image format: {bmp.PixelFormat}");

            // populate class variables
            imageDepth = 8;
            imageWidth = bmp.Width;
            imageHeight = bmp.Height;
            imageFrameCount = 1;
            log.Debug($"Image depth: {imageDepth}-Bit");
            log.Debug($"Image shape: {imageWidth} x {imageHeight}");
            log.Debug($"Image frame count: {imageFrameCount}");

            // get the original image as an array of bytes
            int bytesPerPixel = 3;
            byte[] imageBytes = new byte[imageWidth * imageHeight * bytesPerPixel];
            Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);
            BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            Marshal.Copy(bitmapData.Scan0, imageBytes, 0, imageBytes.Length);
            log.Debug($"Converted image into a byte array (size: {imageBytes.Length})");

            // convert the byte array to an array of pixel values (original data values)
            int nChannels = 3;
            int nValues = imageWidth * imageHeight * nChannels;
            valuesRaw = new int[nValues];
            for (int i = 0; i < nValues; i++)
                valuesRaw[i] = imageBytes[i];
            log.Debug($"Converted image into an RGB int array (size: {valuesRaw.Length})");

            // guess the depth based on the data and use this to predict the value of a white pixel
            double valuesMax = imageBytes.Max();
            int dataDepth = 1;
            while (Math.Pow(2, dataDepth) < valuesMax)
                dataDepth++;
            double whitePixelValue = Math.Pow(2, dataDepth);
            log.Debug($"Predicted camera depth: {dataDepth}-Bit");
            log.Debug($"Brightnest pixel: {valuesMax} / {whitePixelValue} ({valuesMax / whitePixelValue * 100}%)");

            /*
            // create a float array to hold image data, normalized to 1 (by whitest pixel value)
            double[] values = new double[nValues];
            for (int i = 0; i < nValues; i++)
                values[i] = (double)imageBytes[i] / whitePixelValue;
            log.Debug($"Converted image into an RGB double array (size: {values.Length})");
            */
        }

        /// <summary>
        /// Read an image and create a double array for pixel values
        /// </summary>
        /// <param name="filePath"></param>
        private void LoadImageData()
        {
            log.Debug($"Loading image file: {System.IO.Path.GetFileName(filePath)}");

            if (filePath.ToUpper().EndsWith(".TIF") || filePath.ToUpper().EndsWith(".TIFF"))
                LoadValuesTIFF();
            else
                LoadValuesNONTIFF();

            if (valuesRaw.Length == 0)
                throw new Exception("no pixel data loaded from image!");

            // load these data into the image display class
            imageDisplay = new ImageDisplay(valuesRaw, imageWidth, imageHeight, imageDepth);
            log.Debug($"Loaded normalized data into display class");

        }

        public Bitmap GetBitmap()
        {
            return imageDisplay.GetDisplayBitmap();
        }

    }
}
