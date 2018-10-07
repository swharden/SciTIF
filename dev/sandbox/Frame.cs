using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sandbox
{
    /// <summary>
    /// A frame is a single collection of pixel values (int16 depth) representing an image to be displayed.
    /// It also contains color and brightness/contrast preferences, but always retains its original data.
    /// </summary>
    class Frame
    {
        public UInt16[] pixels;
        public Size size;
        public string color;
        public bool visible;
        public int displayMin;
        public int displayMax;

        public Frame(UInt16[] pixels, Size size, string color = "gray", bool visible = true,
                     int displayMin = 0, int displayMax = 54321)
        {
            this.pixels = pixels;
            this.size = size;
            this.color = color;
            this.visible = visible;
            this.displayMin = displayMin;
            this.displayMax = displayMax;
        }
    }
}
