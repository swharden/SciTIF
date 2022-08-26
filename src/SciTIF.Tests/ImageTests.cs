using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class ImageTests
    {
        [Test]
        public void Test_Image_GetAndSetPixel()
        {
            Image img = SampleData.GetBaboonImage();

            int x = 13;
            int y = 42;
            double initialValue = img.GetPixel(x, y);
            double newValue = 420.69;

            img.SetPixel(x, y, newValue);
            Assert.That(img.GetPixel(x, y), Is.Not.EqualTo(initialValue));
            Assert.That(img.GetPixel(x, y), Is.EqualTo(newValue));
        }

        [Test]
        public void Test_Image_RememberAndRecall()
        {
            Image img = SampleData.GetBaboonImage();

            int x = 13;
            int y = 42;
            double initialValue = img.GetPixel(x, y);
            double newValue = 420.69;

            img.RememberValues();
            img.SetPixel(x, y, newValue);
            Assert.That(img.GetPixel(x, y), Is.Not.EqualTo(initialValue));
            Assert.That(img.GetPixel(x, y), Is.EqualTo(newValue));

            img.RecallValues();
            Assert.That(img.GetPixel(x, y), Is.EqualTo(initialValue));
            Assert.That(img.GetPixel(x, y), Is.Not.EqualTo(newValue));
        }
    }
}
