using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class ColorTests
    {
        [Test]
        public void Test_Load_ColorImage_Lenna()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "LennaRGB.tif");
            TifFile tif = new(filePath);
            Assert.AreEqual(
                expected: new double[] { 225, 136, 125 },
                actual: tif.GetPixel(0, 0));
        }

        [Test]
        public void Test_Load_ColorImage_RGB()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "20220224-ucx.tif");
            TifFile tif = new(filePath);
            Assert.AreEqual(
                expected: new double[] { 131, 131, 5 },
                actual: tif.GetPixel(188, 260));
        }

        [Test]
        public void Test_Load_ColorImage_16bitGrayscale()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "calibration-20x-ruler-0.32365.tif");
            TifFile tif = new(filePath);
            Assert.AreEqual(1896, tif.Channels[0].Values[0, 0]);

            double[,] scaled = Adjust.AutoScale(tif.Channels[0].Values);
            Assert.AreNotEqual(1896, scaled[0,0]);
        }

        [Test]
        public void Test_Load_ColorImage_RGBA()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "LineScan-06092017-1414-623-Cycle00002-Window2-Ch1-8bit-Reference.tif");
            TifFile tif = new(filePath);
            Console.WriteLine(tif.FormatDescription);

            Assert.AreEqual(
                expected: new double[] { 7, 7, 7, 255 },
                actual: tif.GetPixel(0, 0));

            Assert.AreEqual(
                expected: new double[] { 0, 255, 255, 255 },
                actual: tif.GetPixel(75, 244));
        }
    }
}
