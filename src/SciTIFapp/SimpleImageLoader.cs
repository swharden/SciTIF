using System;
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
        public string logText = "SimpleImageLoader log:";

        // image propeties 
        public int depthSource;
        public int depthDisplay;
        public int valueMin;
        public int valueMax;
        public int frameCount;
        public int frame;

        public SimpleImageLoader(string path)
        {
            this.path = path;
            if (path.ToUpper().EndsWith(".TIF") || path.ToUpper().EndsWith(".TIFF"))
                bmpPreview = LoadImageTiff(path);
            else
                bmpPreview = LoadImageNonTiff(path);
        }

        private void Log(string message)
        {
            if (logText != null)
                message = "\n  " + message;
            logText += message;
        }

        /// <summary>
        /// This function takes a TIFF frame of any bit depth and returns a proper 8-bit bitmap
        /// </summary>
        private Bitmap LoadImageTiff(string path, int frameNumber = 0)
        {
            Log("Loading image with TiffBitmapDecoder");

            // open a file stream and keep it open until we're done reading the file
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            // carefully open the file to see if it will decode
            TiffBitmapDecoder decoder;
            try
            {
                decoder = new TiffBitmapDecoder(stream,
                    BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                Log("TIFF decoder opened successfully");
            }
            catch
            {
                Log("TIFF decoder crashed");
                stream.Dispose();
                return null;
            }

            // access information about the image
            int imageFrames = decoder.Frames.Count;
            BitmapSource bitmapSource = decoder.Frames[frameNumber];
            int sourceImageDepth = bitmapSource.Format.BitsPerPixel;
            int bytesPerPixel = sourceImageDepth / 8;
            Log($"sourceImageDepth: {sourceImageDepth}");
            Size imageSize = new Size(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
            Log($"Detected {sourceImageDepth}-bit image ({imageSize.Width}x{imageSize.Height}) with {imageFrames} frames");

            // fill a byte array with source data bytes from the file
            int pixelCount = imageSize.Width * imageSize.Height;
            int imageByteCount = pixelCount * bytesPerPixel;
            byte[] bytesSource = new byte[imageByteCount];
            bitmapSource.CopyPixels(bytesSource, imageSize.Width * bytesPerPixel, 0);
            Log($"filled bytesSource with {imageByteCount} bytes about {pixelCount} pixels");

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
            Log($"Min/max data values: {pixelValueMin}, {pixelValueMax}");

            // predict what bit depth we have based upon pixelValueMax
            int dataDepth = 1;
            while (Math.Pow(2, dataDepth) < pixelValueMax)
                dataDepth++;
            Log($"Data bit depth: {dataDepth}");

            // determine if we will use the original bit depth or our guessed bit depth
            bool use_detected_camera_depth = true; // should this be an argument?
            if (!use_detected_camera_depth)
                dataDepth = sourceImageDepth;
            Log($"Display bit depth: {dataDepth}");

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

            // input bytes are padded such that stide is a multiple of 4 bytes, so trim it off
            int strideByteMultiple = 4;
            int strideOverhang = imageSize.Width % strideByteMultiple;
            Log($"Width-stride overhang: {strideOverhang} bytes");
            if (strideOverhang > 0)
            {
                int strideBytesNeededPerRow = strideByteMultiple - (strideOverhang);
                Log($"Trimming {strideBytesNeededPerRow} extra bytes from the end of each row");
                byte[] pixelsOutputOriginal = new byte[pixelCount];
                Array.Copy(pixelsOutput, pixelsOutputOriginal, pixelCount);
                pixelsOutput = new byte[pixelCount + strideBytesNeededPerRow * imageSize.Height];
                int newStrideWidth = imageSize.Width + strideBytesNeededPerRow;
                for (int row = 0; row < imageSize.Height; row++)
                    for (int col = 0; col < imageSize.Width; col++)
                        pixelsOutput[row * newStrideWidth + col] = pixelsOutputOriginal[row * imageSize.Width + col];
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
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, format);
            Log($"bmpData byte size: {bmpData.Stride * bmpData.Height}");
            Marshal.Copy(pixelsOutput, 0, bmpData.Scan0, pixelsOutput.Length);
            Log($"pixelsOutput.Length: {pixelsOutput.Length}, bytes {bmpData.Stride * bmpData.Height}");
            bmp.UnlockBits(bmpData);

            // update class-level variables with image information
            this.depthSource = sourceImageDepth;
            this.depthDisplay = dataDepth;
            this.valueMin = pixelValueMin;
            this.valueMax = pixelValueMax;
            this.frameCount = imageFrames;
            this.frame = frameNumber;

            return bmp;
        }

        private Bitmap LoadImageNonTiff(string path)
        {
            Log("Loading image with Bitmap");
            Bitmap bmp = new Bitmap(path);

            // update class-level variables with image information
            this.depthSource = 8;
            this.depthDisplay = 8;
            this.valueMin = 0; // TODO: function to get limits of BMP
            this.valueMax = 255; // TODO: function to get limits of BMP
            this.frameCount = 1;
            this.frame = 1;

            return bmp;
        }
    }
}
