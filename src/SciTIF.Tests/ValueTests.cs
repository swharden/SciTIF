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
                (string fileName, int x, int y, double value) = item;
                Console.WriteLine(item);
                string filePath = Path.Combine(SampleData.DataFolder, fileName);
                var tif = new TifFile(filePath);
                Assert.AreEqual(value, tif.Values[y, x], item.ToString());
            }
        }
    }
}
