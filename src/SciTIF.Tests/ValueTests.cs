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
                Console.WriteLine(item);
                string filePath = Path.Combine(SampleData.DataFolder, fileName);
                var tif = new TifFile(filePath);

                double[] channelValues = tif.GetPixel(x, y);
                string debugHint = string.Join(", ", channelValues.Select(x => x.ToString()));
                double actualMean = channelValues.Sum() / channelValues.Length;
                Assert.AreEqual(expectedMean, actualMean, item.ToString() + "|" + debugHint);
            }
        }
    }
}
