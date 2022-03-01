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

                double[] channelValues = tif.GetPixel(x, y).Take(3).ToArray(); // ignore alpha (last)
                Console.WriteLine(fileName + " : " + string.Join(",", (channelValues.Select(x => x.ToString()))));
                double actualMean = Math.Round(channelValues.Sum() / channelValues.Length, 4);

                Assert.AreEqual(expectedMean, actualMean, $"{fileName} X={x} Y={y} {tif.FormatDescription}");
            }
        }

        [Test]
        public void Test_Dimensions_MatchImageJ()
        {
            foreach (var dims in SampleData.Dimensions())
            {
                string filePath = Path.Combine(SampleData.DataFolder, dims.filename);
                Console.WriteLine(filePath);
                TifFile tif = new(filePath);

                Assert.AreEqual(dims.width, tif.Width, filePath);
                Assert.AreEqual(dims.height, tif.Height, filePath);
                Assert.AreEqual(dims.channels * dims.slices * dims.frames, tif.ImageCount, filePath);
            }
        }
    }
}