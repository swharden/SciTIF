using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class SaveAndLoad
    {
        [Test]
        public void Test_SavedImageMatchesOriginal()
        {
            string outputFolder = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "_SavedSampleImages"));
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            foreach (string filePath in SampleData.TifFiles)
            {
                string outputPath = Path.Combine(outputFolder, Path.GetFileName(filePath) + ".png");
                Console.WriteLine($"Saving: {outputPath}");
                TifFile tif = new(filePath);
                Export.PNG(outputPath, tif, autoScale: true);

                // TODO: load the image we just saved and confirm it matches the TIF value
            }
        }
    }
}
