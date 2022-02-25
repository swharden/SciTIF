﻿using NUnit.Framework;
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
        public void Test_Load_ColorImage_Scan()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "20220224-ucx.tif");
            TifFile tif = new(filePath);
            Assert.AreEqual(
                expected: new double[] { 131, 131, 5 },
                actual: tif.GetPixel(188, 260));

            tif.SaveGrayscalePng("scan.png");
            tif.SaveRgbPng("scan2.png");
        }
    }
}
