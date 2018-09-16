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
        private double pixelValueWhite;

        private double[] values;
        private double valuesMin;
        private double valuesMax;

        private double displayMin;
        private double displayMax;

        ////////////////////////////////////////////////////////////////////////////////////////
        // DATA LOADING

        public ImageDisplay(int[] valuesRaw, int imageWidth, int imageHeight, int sourceDepth)
        {
            // load our original values in memory
            this.valuesRaw = valuesRaw;
            this.sourceDepth = sourceDepth;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            valuesRawMin = valuesRaw.Min();
            valuesRawMax = valuesRaw.Max();
            pixelValueWhite = Math.Pow(2, sourceDepth);
            CreateNormalizedValues();
        }

        private void CreateNormalizedValues()
        {
            // create a float array of all values normalized to 1 (based on pixelValueWhite)
            values = new double[valuesRaw.Length];
            for (int i = 0; i < valuesRaw.Length; i++)
            {
                values[i] = valuesRaw[i] / pixelValueWhite;
            }
            valuesMin = values.Min();
            valuesMax = values.Max();
            SetMinAndMaxAuto();
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        // CONTRAST ADJUSTMENTS

        /// <summary>
        /// manually define min and max pixel intensity
        /// </summary>
        public void SetMinMax(int displayMinValue, int displayMaxValue)
        {
            this.displayMin = displayMinValue;
            this.displayMax = displayMaxValue;
        }

        /// <summary>
        /// default contrast (0 - white pixel based on depth)
        /// </summary>
        public void SetMinAndMaxToLimits()
        {
            displayMin = valuesMin * pixelValueWhite;
            displayMax = valuesMax * pixelValueWhite;
        }

        /// <summary>
        /// auto-contrast (full bin of data)
        /// </summary>
        public void SetMinAndMaxAuto()
        {
            displayMin = valuesMin * pixelValueWhite;
            displayMax = valuesMax * pixelValueWhite;
        }

        /// <summary>
        /// a special contrast/brightness adjustment made for tracking mouse position
        /// </summary>
        public void SetMinMaxMouse(double horizontalDistance = 0, double verticalDistance = 0)
        {

            // shift min and max (brightness) based on horizontal movement
            displayMin = (valuesMin + -horizontalDistance / 500) * pixelValueWhite;
            displayMax = (valuesMax + -horizontalDistance / 500) * pixelValueWhite;

            // squeeze min and max (contrast) based on vertical movement
            double displayCenter = (displayMax + displayMin) / 2;
            double deviation = displayCenter - displayMin;
            deviation += verticalDistance/pixelValueWhite*50;
            if (deviation < 1)
                deviation = 1;
            displayMin = displayCenter - deviation;
            displayMax = displayCenter + deviation;
        }

        /// <summary>
        /// convert a data value (normalized 0-1) to a 0-255 pixel intensity byte based on the display min/max
        /// </summary>
        public byte ValueAfterContrast(double pixelValue)
        {
            pixelValue -= displayMin / pixelValueWhite;
            pixelValue /= (displayMax - displayMin) / pixelValueWhite;
            pixelValue *= 255;
            if (pixelValue < 0)
                pixelValue = 0;
            if (pixelValue > 255)
                pixelValue = 255;
            return (byte)pixelValue;
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        // BITMAP CREATION

        /// <summary>
        /// Return the loaded image data as a bitmap brightness-and-contrast-adjusted to the current settings
        /// </summary>
        public Bitmap ValuesToBitmap()
        {
            // prepare a bitmap to hold the display image
            var format = PixelFormat.Format24bppRgb;
            Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);
            Bitmap bmpDisplay = new Bitmap(imageWidth, imageHeight, format);

            // create a byte array to hold RGB values for the display image
            int bytesPerPixel = 3;
            int byteCount = imageWidth * imageHeight * bytesPerPixel;
            byte[] bmpBytes = new byte[byteCount];

            // set the display value according to the source image intensity
            for (int i = 0; i < values.Length; i++)
            {
                byte valByte = ValueAfterContrast(values[i]);
                int bytePosition = i * bytesPerPixel;
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

    }
}
