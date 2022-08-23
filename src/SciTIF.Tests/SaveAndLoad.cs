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
            foreach (string tifInputPath in SampleData.TifFiles)
            {
                System.Drawing.Bitmap bmp1 = new(tifInputPath);
                string outputFolder = Path.GetTempPath();
                string outputFileName = Path.GetFileName(tifInputPath) + ".png";
                string pngOutputPath = Path.Combine(outputFolder, outputFileName);

                TifFile tif = new(tifInputPath);
                Assert.AreEqual(bmp1.Size.Width, tif.Channels[0].Width, tif.ToString());

                Export.PNG(pngOutputPath, tif);
                Console.WriteLine(pngOutputPath);

                //System.Drawing.Bitmap bmp2 = new(pngOutputPath);
                //Assert.AreEqual(bmp1.Size, bmp2.Size, tif.ToString());
            }
        }
    }
}
