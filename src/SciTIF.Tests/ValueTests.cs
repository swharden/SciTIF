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
    public class ValueTests
    {
        [Test]
        public void Test_ExpectedValues_Match()
        {
            foreach (var item in SampleData.ExpectedResults())
            {
                (string fileName, int x, int y, double expectedMean) = item;
                string filePath = Path.Combine(SampleData.DataFolder, fileName);
                TifFile tif = new(filePath);

                Console.WriteLine($"{fileName} {tif.FormatDescription}");

                double[] channelValues = tif.GetPixel(x, y).Take(3).ToArray(); // ignore alpha (last)
                Console.WriteLine(fileName + " : " + string.Join(",", (channelValues.Select(x => x.ToString()))));
                double actualMean = Math.Round(channelValues.Sum() / channelValues.Length, 4);
                Assert.AreEqual(expectedMean, actualMean, $"{fileName} X={x} Y={y} {tif.FormatDescription}");
            }
        }
    }
}
