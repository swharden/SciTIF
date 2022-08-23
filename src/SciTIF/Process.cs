using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SciTIF
{
    public static class Process
    {
        public static Image ProjectMean(IEnumerable<Image> frames)
        {
            Image sum = new(frames.First().Width, frames.First().Height);

            foreach (Image frame in frames)
                sum += frame;

            return sum;
        }
    }
}
