using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SciTIF;

/// <summary>
/// Holds a stack of single-channel images
/// </summary>
public class ImageStack
{
    public readonly Image[] Images;

    public ImageStack(IEnumerable<Image> images)
    {
        Images = images.ToArray();
    }
}
