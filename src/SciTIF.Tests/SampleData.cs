using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    public class SampleData
    {
        public static string DataFolder = GetDataFolder();
        public static string DataInfoFile => Path.Combine(DataFolder, "../imagej/sample-image-info.txt");

        public static string[] TifFiles = Directory.GetFiles(DataFolder, "*.tif");

        public static string TifWithFramesSlicesAndChannels => Path.Combine(DataFolder, "C3Z4F5.tif");

        private static string GetDataFolder()
        {
            string testDirectory = Path.GetFullPath(TestContext.CurrentContext.TestDirectory);
            string repoDirectory = Path.GetFullPath(Path.Combine(testDirectory, "../../../../../"));
            string sampleDataFilePath = Path.Combine(repoDirectory, "data/images/baboon.png");
            if (File.Exists(sampleDataFilePath))
                return Path.GetDirectoryName(sampleDataFilePath)!;
            else
                throw new FileNotFoundException(sampleDataFilePath);
        }

        [Test]
        public void Test_DataFolder_Exists()
        {
            Assert.That(Directory.Exists(DataFolder));
        }

        [Test]
        public void Test_DataFolder_HasImages()
        {
            Assert.IsNotEmpty(TifFiles);
        }

        [Test]
        public void Test_SampleImageInfoFile_Exists()
        {
            Assert.IsTrue(File.Exists(DataInfoFile));
        }
    }
}
