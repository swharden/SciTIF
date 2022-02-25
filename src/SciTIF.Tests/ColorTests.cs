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
        public void Test_Load_ColorImage()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "LennaRGB.tif");
            TifFile tif = new(filePath);
            Assert.AreEqual(
                expected: new double[] { 225, 136, 125 },
                actual: tif.GetPixel(0, 0));
        }
    }
}
