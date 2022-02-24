using NUnit.Framework;
using System;
using System.IO;

namespace SciTIF.Tests
{
    public class ReadTests
    {
        [Test]
        public void Test_Read_EverySampleFile()
        {
            foreach (string filePath in SampleData.TifFiles)
            {
                TifFile tif = new(filePath);
                Console.WriteLine(tif);
            }
        }
    }
}