using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1416 // TODO: remove system.drawing dependency

namespace SciTIF.Tests
{
    internal class SaveAndLoad
    {
        [Test]
        public void Test_SaveSampleImages()
        {
            string outputFolder = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "_SavedSampleImages"));
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);
            Console.WriteLine(outputFolder);

            foreach (string filePath in SampleData.TifFiles)
            {
                string outputPath = Path.Combine(outputFolder, Path.GetFileName(filePath) + ".png");
                TifFile tif = new(filePath);
                tif.SavePng(outputPath, autoScale: true);
                Console.WriteLine(outputPath);
            }
        }
    }
}
