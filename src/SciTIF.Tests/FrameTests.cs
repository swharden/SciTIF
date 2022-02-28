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
    internal class FrameTests
    {
        [Test]
        public void Test_MultiFrameImage_SaveAllFrames()
        {
            string fileName = "C3Z4F5.tif";
            string filePath = Path.Combine(SampleData.DataFolder, fileName);

            TifFile tf = new(filePath);
            Console.WriteLine(tf.Channels.Length);
        }
    }
}
