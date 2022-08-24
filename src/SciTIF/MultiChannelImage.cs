using System;
using System.Collections.Generic;
using System.Text;

namespace SciTIF;

public class MultiChannelImage
{
    public readonly List<GrayscaleImage> Images = new();
    public int Channels => Images.Count;

    public MultiChannelImage(GrayscaleImage gray)
    {
        Images.Add(gray);
    }

    public MultiChannelImage(GrayscaleImage r, GrayscaleImage g, GrayscaleImage b)
    {
        Images.Add(r);
        Images.Add(g);
        Images.Add(b);
    }

    public MultiChannelImage(GrayscaleImage r, GrayscaleImage g, GrayscaleImage b, GrayscaleImage a)
    {
        Images.Add(r);
        Images.Add(g);
        Images.Add(b);
        Images.Add(a);
    }

    public MultiChannelImage(GrayscaleImage[] channelImages)
    {
        Images.AddRange(channelImages);
    }
}
