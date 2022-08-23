﻿using BitMiracle.LibTiff.Classic;
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

                double[] pixelValues = tif.Slices[0].GetPixelValues(x, y).Take(3).ToArray();
                double mean = pixelValues.Sum() / pixelValues.Length;
                Assert.That(expectedMean, Is.EqualTo(mean).Within(.01));
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
