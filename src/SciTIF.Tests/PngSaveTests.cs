using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class PngSaveTests
    {
        [Test]
        public void Test_SavePng_FirstFrames()
        {
            foreach (string path in SampleData.TifFiles)
            {
                TifFile tif = new(path);
                Image img = tif.GetImage(0, 0, 0);
                string filename = System.IO.Path.GetFileNameWithoutExtension(path);
                string filePath = System.IO.Path.GetFullPath($"test-save-autoscale-{filename}.png");
                img.SavePng(filePath, true);
                Console.WriteLine(filePath);
            }
        }
    }
}
