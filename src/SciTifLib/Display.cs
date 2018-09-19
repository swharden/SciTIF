/* Code here handles all image display operations.
 * 
 * Scientific data is loaded into ValuesRaw (an int array).
 * The upper/lower pixel value limits (displayMin/displayMax) determine how it's rendered.
 * A function can generate a 24-bit (RGB, 8-bit/channel) image as needed.
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SciTIFlib
{
    /// <summary>
    /// Code here handles brightness & contrast adjustments and LUTs.
    /// </summary>
    public class ImageDisplay
    {
        private int sourceDepth;

        private int[] valuesRaw;
        private int valuesRawMin;
        private int valuesRawMax;
        private int imageWidth;
        private int imageHeight;
        private int pixelValueWhite;

        public double displayMin;
        public double displayMax;
        public double displayMinDelta;
        public double displayMaxDelta;

        ////////////////////////////////////////////////////////////////////////////////////////
        // DATA LOADING

        /// <summary>
        /// Load with non-normalized values
        /// </summary>
        public ImageDisplay(int[] valuesRaw, int imageWidth, int imageHeight, int sourceDepth)
        {
            this.valuesRaw = valuesRaw;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.sourceDepth = sourceDepth;

            pixelValueWhite = (int)Math.Pow(2, sourceDepth);
            valuesRawMin = valuesRaw.Min();
            valuesRawMax = valuesRaw.Max();
            SetMinAndMaxToLimits();

        }

        ////////////////////////////////////////////////////////////////////////////////////////
        // CONTRAST ADJUSTMENTS

        /// <summary>
        /// manually define min and max pixel intensity
        /// </summary>
        public void SetMinMax(int displayMin, int displayMax)
        {
            this.displayMin = displayMin;
            this.displayMax = displayMax;
        }

        /// <summary>
        /// default contrast (0 - white pixel based on depth)
        /// </summary>
        public void SetMinAndMaxToLimits()
        {
            displayMin = 0;
            displayMax = pixelValueWhite;
        }

        /// <summary>
        /// auto-contrast (adjust brightness / contrast to tightly fit the data)
        /// </summary>
        public void SetMinAndMaxAuto()
        {
            displayMin = valuesRawMin;
            displayMax = valuesRawMax;
        }

        /// <summary>
        /// a special contrast/brightness adjustment made for tracking mouse position
        /// </summary>
        public void SetMinMaxMouse(double horizontalDistance = 0, double verticalDistance = 0)
        {
            // adjust sensitivity to match bit depth
            double currentDisplayPixelSpan = (displayMax - displayMin);
            double sensitivity = currentDisplayPixelSpan / 256;

            // prepare variables to hold our new deal min/max
            double newMin;
            double newMax;

            // shift min and max (brightness) based on horizontal movement
            newMin = displayMin + horizontalDistance * sensitivity;
            newMax = displayMax + horizontalDistance * sensitivity;

            // squeeze min and max (contrast) based on vertical movement
            double displayCenter = (newMin + newMax) / 2;
            double deviation = displayCenter - newMin;
            deviation -= verticalDistance * sensitivity;
            newMin = displayCenter - deviation;
            newMax = displayCenter + deviation;

            // change our deltas to reflect deviation from the new max/min we calculated
            displayMinDelta = displayMin - newMin;
            displayMaxDelta = displayMax - newMax;

            double valMin = displayMin + displayMinDelta;
            double valMax = displayMax + displayMaxDelta;
        }

        /// <summary>
        /// convert a data value (raw) to a 0-255 pixel intensity byte based on the display min/max
        /// </summary>
        public byte ValueAfterContrast(double pixelValue)
        {
            // prepare temporary min/max pixel values
            double valMin = displayMin + displayMinDelta;
            double valMax = displayMax + displayMaxDelta;

            // ensure contrast doesn't get inverted
            if (valMin >= valMax)
                valMin = valMax - 1;

            // subtract down to the minimum pixel value
            pixelValue -= valMin;

            // determine how to stretch it to the max pixel value
            double stretch = (valMax - valMin) / pixelValueWhite;
            pixelValue /= stretch;

            // down-sample to 8-bit if needed
            if (sourceDepth > 8)
                pixelValue = pixelValue / pixelValueWhite * 255;

            // ensure pixels stay inside the valid range
            if (pixelValue < 0)
                pixelValue = 0;
            if (pixelValue > 255)
                pixelValue = 255;

            // return a byte
            return (byte)pixelValue;
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        // BITMAP CREATION

        /// <summary>
        /// Return image data as a 24-bit (RGB) bitmap with brightness and contrast adjusted
        /// </summary>
        public Bitmap GetDisplayBitmap()
        {
            // prepare a bitmap to hold the display image
            var format = PixelFormat.Format24bppRgb;
            Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);
            Bitmap bmpDisplay = new Bitmap(imageWidth, imageHeight, format);

            // create a byte array to hold RGB values for the display image
            int bytesPerPixel = 3;
            int byteCount = imageWidth * imageHeight * bytesPerPixel;
            byte[] bmpBytes = new byte[byteCount];

            if (valuesRaw.Length * 3 == bmpBytes.Length)
            {
                // values come in as grayscale so assign the same value to R, G, and B
                for (int i = 0; i < valuesRaw.Length; i++)
                {
                    byte valByte = ValueAfterContrast(valuesRaw[i]);
                    int bytePosition = i * bytesPerPixel;
                    bmpBytes[bytePosition + 2] = valByte; // red
                    bmpBytes[bytePosition + 1] = valByte; // green
                    bmpBytes[bytePosition + 0] = valByte; // blue
                }
            }
            else if (valuesRaw.Length == bmpBytes.Length)
            {
                // values come in as RGB so assign them as RGB
                for (int i = 0; i < valuesRaw.Length; i++)
                {
                    byte valByte = ValueAfterContrast(valuesRaw[i]);
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
