using System;
using System.Collections.Generic;
using System.Text;

namespace SciTIF
{
    public static class Process
    {
        public static ImageDataXY ProjectMean(ImageDataXY[] frames)
        {
            ImageDataXY sum = new(frames[0].Width, frames[0].Height);

            foreach (ImageDataXY frame in frames)
                sum += frame;

            return sum;
        }
    }
}
