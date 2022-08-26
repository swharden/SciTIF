using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class QuickstartTests
    {
        [Test]
        public void Test_Quickstart()
        {
            string path = System.IO.Path.Combine(SampleData.DataFolder, "baboon 16bit grayscale.tif");
            TifFile tif = new(path); // 16-bit images with pixel values that exceed 255
            Image slice = tif.GetImage(frame: 0, slice: 0, channel: 0); // 5D images are supported
            slice.AutoScale(); // scale pixel values down to 0-255
            slice.SavePng("baboon-16.png");
        }
    }
}
