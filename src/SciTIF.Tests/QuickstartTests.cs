using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
            string path = SampleData.Tif16bitGrayscale;
            TifFile tif = new(path); // 16-bit images with pixel values that exceed 255
            Image slice = tif.GetImage(frame: 0, slice: 0, channel: 0); // 5D images are supported
            slice.AutoScale(); // scale pixel values down to 0-255
            slice.Save_TEST("scaled.png");
        }

        [Test]
        public void Test_StackProjectMax()
        {
            string path = System.IO.Path.Combine(SampleData.Tif16bitStack);
            TifFile tif = new(path);
            ImageStack stack = tif.GetImageStack();
            Image projection = stack.ProjectMax();
            projection.AutoScale(); // required to scale 16-bit pixel values to 8-bit (0-255)
            projection.Save_TEST("maximum-projection.png");
        }

        [Test]
        public void Test_StackProjectRainbow()
        {
            string path = System.IO.Path.Combine(SampleData.Tif16bitStack);
            TifFile tif = new(path);
            ImageStack stack = tif.GetImageStack();
            stack.AutoScale(); // required to scale 16-bit pixel values to 8-bit (0-255)

            ILUT lut = new LUTs.Jet();
            ImageRGB projection = stack.Project(lut);
            projection.Save_TEST("rainbow-projection.png");
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

        [Test]
        public void Test_SingleChannelLut()
        {
            string path = SampleData.Tif16bitGrayscale;
            TifFile tif = new(path);
            Image slice = tif.GetImage(frame: 0, slice: 0, channel: 0);
            slice.AutoScale();
            slice.LUT = new LUTs.Viridis();
            slice.Save_TEST("viridis.png");
        }

        [Test]
        public void Test_MergeTwoGrayscaleImages()
        {
            // load a multi-channel image
            string path = SampleData.Tif3Channel;
            TifFile tif = new(path);

            // scale each channel (0-255) and set the color lookup table (LUT)
            Image ch1 = tif.GetImage(channel: 0);
            ch1.AutoScale();
            ch1.LUT = new LUTs.Magenta();

            Image ch2 = tif.GetImage(channel: 1);
            ch2.AutoScale();
            ch2.LUT = new LUTs.Green();

            // create a new stack containing just the channels to merge
            Image[] images = { ch1, ch2 };
            ImageStack stack = new(images);

            // project the stack by merging colors
            ImageRGB merged = stack.Merge();
            merged.Save_TEST("merge.png");
        }
    }
}
