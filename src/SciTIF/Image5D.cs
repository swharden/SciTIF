using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SciTIF;

public class Image5D
{
    private readonly Image[,,] Images;
    public int Width => Images[0, 0, 0].Width;
    public int Height => Images[0, 0, 0].Height;
    public int Frames => Images.GetLength(0);
    public int Slices => Images.GetLength(1);
    public int Channels => Images.GetLength(2);

    public Image5D(int slices, int frames, int channels)
    {
        Images = new Image[frames, slices, channels];
    }

    public Image GetImage(int frame, int slice, int channel)
    {
        return Images[frame, slice, channel];
    }

    public void SetImage(int frame, int slice, int channel, Image img)
    {
        Images[frame, slice, channel] = img;
    }

    public static Image5D FromGrayscale(Image img)
    {
        Image5D img5 = new(1, 1, 1);
        img5.SetImage(0, 0, 0, img);
        return img5;
    }

    public static Image5D FromRGB(Image r, Image g, Image b)
    {
        Image5D img5 = new(1, 1, 3);
        img5.SetImage(0, 0, 0, r);
        img5.SetImage(0, 0, 0, g);
        img5.SetImage(0, 0, 0, b);
        return img5;
    }
}
