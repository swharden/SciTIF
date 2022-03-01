using System;
using System.Collections.Generic;
using System.Text;

namespace SciTIF
{
    public static class Process
    {
        public static ImageData ProjectMean(ImageData[] frames)
        {
            ImageData sum = new(frames[0].Width, frames[0].Height);

            foreach (ImageData frame in frames)
                sum += frame;

            return sum;
        }
    }
}
