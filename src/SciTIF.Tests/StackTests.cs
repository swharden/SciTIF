using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class StackTests
    {
        [Test]
        public void Test_Stack_PixelValueCheck()
        {
            string tifFilePath = SampleData.Tif16bitStack;
            TifFile tif = new(tifFilePath);
            ImageStack stack = tif.GetImageStack();

            Assert.That(stack.Images.Length, Is.EqualTo(32));

            int x = 13;
            int y = 17;
            double[] values = stack.Images.Select(img => img.GetPixel(x, y)).ToArray();

            // obtained using ImageJ script
            double[] knownValues = {
                278, 284, 257, 249, 275, 309, 283, 321, 266, 267, 280, 324, 272, 322, 276, 272,
                276, 317, 249, 227, 273, 273, 254, 281, 244, 279, 280, 286, 363, 328, 315, 359
            };

            // note: max is 363
            // note: mean is 286

            Assert.AreEqual(knownValues, values);
        }
    }
}
