using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SciTIF.IO
{
    public static class BmpWriter
    {
        /// <summary>
        /// Create a BMP file from an array of pixel values (Y, X, RGBA Color)
        /// </summary>
        public static byte[] GetBitmapBytes(byte[,,] pixels)
        {
            int height = pixels.GetLength(0);
            int width = pixels.GetLength(1);
            int bytesPerPixel = pixels.GetLength(2);
            if (bytesPerPixel != 4)
                throw new ArgumentException("4 color bytes required");
            int imageBytesSize = width * height * bytesPerPixel;
            const int imageHeaderSize = 54;
            const int dbHeaderSize = 40;

            byte[] bytes = new byte[imageBytesSize + imageHeaderSize];
            bytes[0] = (byte)'B';
            bytes[1] = (byte)'M';
            bytes[14] = dbHeaderSize;

            Array.Copy(BitConverter.GetBytes(bytes.Length), 0, bytes, 2, 4);
            Array.Copy(BitConverter.GetBytes(imageHeaderSize), 0, bytes, 10, 4);
            Array.Copy(BitConverter.GetBytes(width), 0, bytes, 18, 4);
            Array.Copy(BitConverter.GetBytes(height), 0, bytes, 22, 4);
            Array.Copy(BitConverter.GetBytes(bytesPerPixel * 8), 0, bytes, 28, 2);
            Array.Copy(BitConverter.GetBytes(imageBytesSize), 0, bytes, 34, 4);

            for (int y = 0; y < height; y++)
            {
                int rowOffset = (height - 1 - y) * (width * bytesPerPixel);
                for (int x = 0; x < width; x++)
                {
                    int offset = rowOffset + x * bytesPerPixel + imageHeaderSize;
                    bytes[offset + 2] = pixels[y, x, 0];
                    bytes[offset + 1] = pixels[y, x, 1];
                    bytes[offset + 0] = pixels[y, x, 2];
                    bytes[offset + 3] = pixels[y, x, 3];
                }
            }

            return bytes;
        }
    }
}
