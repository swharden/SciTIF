using BitMiracle.LibTiff.Classic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    public class SampleDataTests
    {
        [Test]
        public void Test_PixelValues_MatchImageJ()
        {
            // TODO: improve pixel value testing
            string[] ignoredFileNames =
            {
                "17418028_MMStack_Pos0.ome.tif",
                "fluo-3ch-8bit-composite.tif",
                "fluo-3ch-16bit.tif",
                "LennaIndexed.tif",
            };

            foreach (var item in SampleData.PixelValues())
            {
                if (ignoredFileNames.Contains(item.filename))
                    continue;

                (string fileName, int x, int y, double expectedMean) = item;
                string filePath = Path.Combine(SampleData.DataFolder, fileName);
                TifFile tif = new(filePath);

                Console.WriteLine($"{fileName} {tif.FormatDescription}");

                if (tif.Channels.Length == 1)
                {
                    double value = tif.Channels[0].GetPixel(x, y);
                    Assert.That(value, Is.EqualTo(expectedMean).Within(.01));
                }
                else
                {
                    double valueR = tif.Channels[0].GetPixel(x, y);
                    double valueG = tif.Channels[1].GetPixel(x, y);
                    double valueB = tif.Channels[2].GetPixel(x, y);
                    double[] channelValues = { valueR, valueG, valueB };
                    Console.WriteLine(fileName + " : " + string.Join(",", (channelValues.Select(x => x.ToString()))));
                    double mean = channelValues.Sum() / channelValues.Length;
                    Assert.That(mean, Is.EqualTo(expectedMean).Within(.01));
                }
            }
        }

        [Test]
        public void Test_Dimensions_MatchImageJ()
        {
            foreach (var known in SampleData.Dimensions())
            {
                string filePath = Path.Combine(SampleData.DataFolder, known.filename);
                Console.WriteLine(filePath);
                TifFile tif = new(filePath);

                Assert.AreEqual(known.width, tif.Width, filePath);
                Assert.AreEqual(known.height, tif.Height, filePath);
                Assert.AreEqual(known.channels * known.slices * known.frames, tif.ImageCount, filePath);
            }
        }
    }
}
