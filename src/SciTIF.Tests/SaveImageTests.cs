using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class SaveImageTests
    {
        [Test]
        public void Test_Save_FirstFrames()
        {
            foreach (string path in SampleData.TifFiles)
            {
                TifFile tif = new(path);
                Image img = tif.GetImage(0, 0, 0);
                img.AutoScale();

                string name = System.IO.Path.GetFileNameWithoutExtension(path);
                img.Save_TEST($"autoscale-{name}.png");
                img.Save_TEST($"autoscale-{name}.jpg");
            }
        }
    }
}
