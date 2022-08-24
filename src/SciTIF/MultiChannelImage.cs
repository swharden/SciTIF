using System;
using System.Collections.Generic;
using System.Text;

namespace SciTIF;

public class MultiChannelImage
{
    public readonly List<Image> Images = new();
    public int Channels => Images.Count;

    public MultiChannelImage(Image gray)
    {
        Images.Add(gray);
    }

    public MultiChannelImage(Image r, Image g, Image b)
    {
        Images.Add(r);
        Images.Add(g);
        Images.Add(b);
    }

    public MultiChannelImage(Image r, Image g, Image b, Image a)
    {
        Images.Add(r);
        Images.Add(g);
        Images.Add(b);
        Images.Add(a);
    }

    public MultiChannelImage(Image[] channelImages)
    {
        Images.AddRange(channelImages);
    }
}
