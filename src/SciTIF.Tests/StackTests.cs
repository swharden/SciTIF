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

            // measured at a known point
            double[] pixelValues = stack.Images.Select(img => img.GetPixel(13, 17)).ToArray();

            // obtained using ImageJ script
            double[] knownValues = {
                278, 284, 257, 249, 275, 309, 283, 321, 266, 267, 280, 324, 272, 322, 276, 272,
                276, 317, 249, 227, 273, 273, 254, 281, 244, 279, 280, 286, 363, 328, 315, 359
            };

            Assert.AreEqual(knownValues, pixelValues);
        }

        [Test]
        public void Test_Stack_ProjectMax()
        {
            string tifFilePath = SampleData.Tif16bitStack;
            TifFile tif = new(tifFilePath);
            ImageStack stack = tif.GetImageStack();
            Image projection = stack.ProjectMax();

            Assert.That(projection.GetPixel(13, 17), Is.EqualTo(363));
        }

        [Test]
        public void Test_Stack_ProjectMean()
        {
            string tifFilePath = SampleData.Tif16bitStack;
            TifFile tif = new(tifFilePath);
            ImageStack stack = tif.GetImageStack();
            Image projection = stack.ProjectMean();

            Assert.That(projection.GetPixel(13, 17), Is.EqualTo(286).Within(1));
        }
    }
}
