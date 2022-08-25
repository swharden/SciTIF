using System.Collections.Generic;

namespace SciTIF.Tests.ImageValidation;

public class ImageInfo
{
    public int Width;
    public int Height;
    public int Frames;
    public int Slices;
    public int Channels;
    public int Depth;
    public bool IsGrayscale;
    public bool IsRGB => Channels == 1 && Depth == 24;
    public bool IsSingleChannelIndexedColor => Depth == 8 && !IsGrayscale && Channels == 1;
    public List<PixelInfo> Pixels = new();
}
