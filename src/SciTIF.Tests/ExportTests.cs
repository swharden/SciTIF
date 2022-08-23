using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class ExportTests
    {
        [Test]
        public void Test_Export_AllTestImages()
        {
            string outputFolder = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "output"));
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            foreach (string tifPath in SampleData.TifFiles)
            {
                TifFile tif = new(tifPath);
                Console.WriteLine(tif);
                string outputPath = Path.Combine(outputFolder, Path.GetFileName(tifPath) + ".png");
                Console.WriteLine(outputPath);
                Adjust.AutoScale(tif.Channels[0]);
                Export.PNG(outputPath, tif.Channels[0]);
            }
        }
    }
}
