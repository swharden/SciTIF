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
            TifFile tif = new(path);
            Image slice = tif.GetImage(0, 0, 0); // pixel values exceed 255
            slice.AutoScale(); // scale pixel values down to 0-255
            slice.SavePng("baboon-16.png");
        }
    }
}
