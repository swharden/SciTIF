using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class SampleData
    {
        public string DataFolder => GetDataFolder();

        [Test]
        public void Test_DataFolder_HasImages()
        {
            Console.WriteLine(GetDataFolder());
            string[] paths = Directory.GetFiles(DataFolder, "*.tif");
            Assert.IsNotNull(paths);
            Assert.IsNotEmpty(paths);
        }

        private string GetDataFolder()
        {
            string testDirectory = Path.GetFullPath(TestContext.CurrentContext.TestDirectory);
            string repoDirectory = Path.GetFullPath(Path.Combine(testDirectory, "../../../../../"));
            string sampleDataFilePath = Path.Combine(repoDirectory, "data/images/Lenna_(test_image).png");
            if (File.Exists(sampleDataFilePath))
                return Path.GetDirectoryName(sampleDataFilePath)!;
            else
                throw new InvalidOperationException($"expected file not found: {sampleDataFilePath}");

        }
    }
}
