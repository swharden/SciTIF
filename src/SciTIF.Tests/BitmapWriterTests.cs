using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class BitmapWriterTests
    {
        [Test]
        public void Test_Write_File()
        {
            int width = 17;
            int height = 15;

            byte[,,] pixels = new byte[height, width, 4];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    byte r = (byte)(x * 255 / width);
                    byte g = (byte)(y * 255 / height);
                    byte b = (byte)((r + g) / 2);
                    byte a = 255;

                    pixels[y, x, 0] = r;
                    pixels[y, x, 1] = g;
                    pixels[y, x, 2] = b;
                    pixels[y, x, 3] = a;
                }
            }

            // manually set one pixel
            pixels[5, 7, 0] = 255; // red
            pixels[5, 7, 1] = 255; // gren
            pixels[5, 7, 2] = 255; // blue
            pixels[5, 7, 3] = 255; // alpha

            byte[] bytes = IO.BmpWriter.GetBitmapBytes(pixels);
            string saveAs = Path.GetFullPath("test.bmp");
            System.IO.File.WriteAllBytes(saveAs, bytes);
            Console.WriteLine(saveAs);
        }
    }
}
