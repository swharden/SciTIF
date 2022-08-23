﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class StackTests
    {
        [Test]
        public void Test_Stack_SaveImages()
        {
            string outputFolder = Path.GetFullPath("_multiChannelFrames");
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string filePath = Path.Combine(SampleData.DataFolder, "C3Z4F5.tif");
            TifFile tif = new(filePath);
            Console.WriteLine(tif.FormatDescription);

            for (int i = 0; i < tif.Channels.Length; i++)
            {
                string outputFilePath = Path.Combine(outputFolder, $"test-stack-{i:000}.png");
                Console.WriteLine(outputFilePath);
                Export.PNG(outputFilePath, tif.Channels[i]);
            }
        }

        [Test]
        public void Test_Series()
        {
            string filePath = Path.Combine(SampleData.DataFolder, "video-gcamp.tif");
            TifFile tif = new(filePath);
            Console.WriteLine(tif.Channels.Length);
            ImageDataXY p = Process.ProjectMean(tif.Channels);
            Export.PNG("proj.png", p);
        }
    }
}
