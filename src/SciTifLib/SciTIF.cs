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
        public double[] values; // normalized to 1
        
        public Logger log;
        public ImageData imageData;

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

            LoadImageData(filePath);
            imageData = new ImageData(valuesRaw, imageWidth, imageHeight, imageDepth);
        }

        /// <summary>
        /// Read an image and create a double array for pixel values
        /// </summary>
        /// <param name="filePath"></param>
        private void LoadImageData(string filePath)
        {
            log.Debug($"Loading image file: {System.IO.Path.GetFileName(filePath)}");

            // pull data from the file into the local space
            Bitmap bmpOrig = new Bitmap(filePath);
            log.Debug($"Pixel format: {bmpOrig.PixelFormat}");

            // convert it to 24-bit RGB
            Bitmap bmp = new Bitmap(bmpOrig.Width, bmpOrig.Height, PixelFormat.Format24bppRgb);
            Graphics gr = Graphics.FromImage(bmp);
            gr.DrawImage(bmpOrig, new Rectangle(0, 0, bmp.Width, bmp.Height));
            gr.Dispose();

            // populate class variables
            imageDepth = 8;
            imageWidth = bmp.Width;
            imageHeight = bmp.Height;
            imageFrameCount = 1;

            // get the original image as an array of bytes
            int bytesPerPixel = 3;
            byte[] imageBytes = new byte[imageWidth * imageHeight * bytesPerPixel];
            Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);
            BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            Marshal.Copy(bitmapData.Scan0, imageBytes, 0, imageBytes.Length);

            // normalize the values to 1 according to the white pixel value
            double whitePixelValue = Math.Pow(2, GuessDepth(imageBytes.Max()));

            // convert the byte array to an array of pixel values (raw, and normalized to 1)
            int nChannels = 3;
            int nValues = imageWidth * imageHeight * nChannels;
            valuesRaw = new int[nValues];
            values = new double[nValues];
            for (int i = 0; i < values.Length; i++)
            {
                valuesRaw[i] = imageBytes[i];
                values[i] = (double)imageBytes[i] / whitePixelValue;
            }

        }

        /// <summary>
        /// Given a maximum intensity, guess the depth
        /// </summary>
        public int GuessDepth(double whitestPixelValue)
        {
            int depth = 1;
            while (Math.Pow(2, depth) < whitestPixelValue)
                depth++;
            return depth;
        }
        
        /// <summary>
        /// Return image data as a 24-bit (RGB) bitmap with brightness and contrast adjusted
        /// </summary>
        /// <returns></returns>
        public Bitmap GetBitmap()
        {
            // prepare a bitmap to hold the display image
            var format = PixelFormat.Format24bppRgb;
            Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);
            Bitmap bmpDisplay = new Bitmap(imageWidth, imageHeight, format);

            // create a byte array to hold RGB values for the display image
            int bytesPerPixel = 3;
            int byteCount = imageWidth * imageHeight * bytesPerPixel;
            byte[] bmpBytes = new byte[byteCount];

            if (values.Length * 3 == bmpBytes.Length)
            {
                log.Debug("Assigning grayscale pixel values to output image");
                for (int i = 0; i < values.Length; i++)
                {
                    byte valByte = imageData.ValueAfterContrast(values[i]);
                    int bytePosition = i * bytesPerPixel;
                    bmpBytes[bytePosition + 2] = valByte; // red
                    bmpBytes[bytePosition + 1] = valByte; // green
                    bmpBytes[bytePosition + 0] = valByte; // blue
                }
            }
            else if (values.Length == bmpBytes.Length)
            {
                log.Debug("Assigning RGB pixel values to output image");
                // values come in as RGB so assign them as RGB
                for (int i = 0; i < values.Length; i++)
                {
                    byte valByte = imageData.ValueAfterContrast(values[i]);
                    bmpBytes[i] = valByte;
                }
            }
            else
            {
                throw new Exception("unknown image pixel value format");
            }

            // Use marshal copy as a safe (pointer-free) way to get the pixel bytes into the bitmap
            BitmapData bmpData = bmpDisplay.LockBits(rect, ImageLockMode.ReadWrite, format);
            Marshal.Copy(bmpBytes, 0, bmpData.Scan0, byteCount);
            bmpDisplay.UnlockBits(bmpData);
            return bmpDisplay;
        }

    }
}
