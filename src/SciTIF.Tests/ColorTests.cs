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

            tif.SaveGrayscalePng("scan.png");
            tif.SaveRgbPng("scan2.png");
        }

        [Test]
        public void Test_Load_ColorImage_16bitGrayscale()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "calibration-20x-ruler-0.32365.tif");
            TifFile tif = new(filePath);
            Assert.AreEqual(1896, tif.Channels[0].Values[0, 0]);

            double[,] scaled = Adjust.AutoScale(tif.Channels[0].Values);
            Export.PNG("ruler.png", scaled);
        }

        [Test]
        public void Test_Load_ColorImage_Rgb8()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "LineScan-06092017-1414-623-Cycle00002-Window2-Ch1-8bit-Reference.tif");
            TifFile tif = new(filePath);
            Console.WriteLine(tif.FormatDescription);

            double[,] scaled = Adjust.AutoScale(tif.Channels[0].Values);
            Export.PNG("rgb8.png", scaled);

            Assert.AreEqual(7, tif.Channels[0].Values[0, 0]);
            Assert.AreEqual(7, tif.Channels[1].Values[0, 0]);
            Assert.AreEqual(7, tif.Channels[2].Values[0, 0]);
        }
    }
}
