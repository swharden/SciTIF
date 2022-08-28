using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class QuickstartTests
    {
        [Test]
        public void Test_Quickstart()
        {
            string path = System.IO.Path.Combine(SampleData.DataFolder, "baboon 16bit grayscale.tif");
            TifFile tif = new(path); // 16-bit images with pixel values that exceed 255
            Image slice = tif.GetImage(frame: 0, slice: 0, channel: 0); // 5D images are supported
            slice.AutoScale(); // scale pixel values down to 0-255
            slice.Save("baboon-16.png");
        }

        [Test]
        public void Test_StackProject()
        {
            string path = System.IO.Path.Combine(SampleData.Tif16bitStack);
            TifFile tif = new(path);
            ImageStack stack = tif.GetImageStack();
            Image projection = stack.ProjectMax();
            projection.AutoScale(max: 255);
            projection.Save_TEST("maximum-projection.png");
        }

        [Test]
        public void Test_MergeThreeChannels()
        {
            // load a 3-channel TIF and merge channels as an RGB image

            string path = System.IO.Path.Combine(SampleData.Tif3Channel);
            TifFile tif = new(path);

            Image red = tif.GetImage(channel: 0);
            Image green = tif.GetImage(channel: 1);
            Image blue = tif.GetImage(channel: 2);

            ImageRGB rgb = new(red, green, blue);
            rgb.Save_TEST("rgb-3ch-merge.png");
        }

        [Test]
        public void Test_MergeTwoChannel()
        {
            // load a 2-channel TIF and save as green and magenta

            string path = System.IO.Path.Combine(SampleData.Tif3Channel);
            TifFile tif = new(path);

            Image ch1 = tif.GetImage(channel: 0);
            Image ch2 = tif.GetImage(channel: 1);

            // megenta is red + blue
            ImageRGB rgb = new(ch1, ch2, ch1);
            rgb.Save_TEST("rgb-2ch-merge.png");
        }
    }
}
