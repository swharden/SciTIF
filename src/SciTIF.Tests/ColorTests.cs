using BitMiracle.LibTiff.Classic;
using Newtonsoft.Json.Linq;
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
        public void AssertPixelValues(TifFile tif, int x, int y, double r, double g, double b)
        {
            double[] actual = tif.GetSlice(0).GetPixelValues(x, y);
            Assert.That(actual.Length, Is.EqualTo(3));
            Assert.That(actual[0], Is.EqualTo(r));
            Assert.That(actual[1], Is.EqualTo(g));
            Assert.That(actual[2], Is.EqualTo(b));
        }

        public void AssertPixelValues(TifFile tif, int x, int y, double r, double g, double b, double a)
        {
            double[] actual = tif.GetSlice(0).GetPixelValues(x, y);
            Assert.That(actual.Length, Is.EqualTo(4));
            Assert.That(actual[0], Is.EqualTo(r));
            Assert.That(actual[1], Is.EqualTo(g));
            Assert.That(actual[2], Is.EqualTo(b));
            Assert.That(actual[3], Is.EqualTo(a));
        }

        [Test]
        public void Test_Load_ColorImage_RGB()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "20220224-ucx.tif");
            TifFile tif = new(filePath);
            AssertPixelValues(tif, 188, 260, 131, 131, 5);
        }

        [Test]
        public void Test_Load_ColorImage_16bitGrayscale()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "calibration-20x-ruler-0.32365.tif");
            TifFile tif = new(filePath);
            Assert.AreEqual(1896, tif.GetSlice(0).Values[0]);

            tif.GetSlice(0).AutoScale();
            Assert.AreNotEqual(1896, tif.GetSlice(0).Values[0]);
        }

        [Test]
        public void Test_Load_ColorImage_RGBA()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "LineScan-06092017-1414-623-Cycle00002-Window2-Ch1-8bit-Reference.tif");
            TifFile tif = new(filePath);
            Console.WriteLine(tif.FormatDescription);

            AssertPixelValues(tif, 0, 0, 7, 7, 7, 255);
            AssertPixelValues(tif, 75, 244, 0, 255, 255, 255);
        }

        [Test]
        public void Test_Load_ColorImage_Indexed()
        {
            string filePathRGB = Path.Combine(SampleData.DataFolder, "LennaRGB.tif");
            TifFile tifRGB = new(filePathRGB);
            AssertPixelValues(tifRGB, 0, 0, 225, 136, 125);

            string filePathIndexed = Path.Combine(SampleData.DataFolder, "LennaIndexed.tif");
            TifFile tifIndexed = new(filePathIndexed);
            AssertPixelValues(tifIndexed, 0, 0, 221, 133, 125, 255);
        }
    }
}
