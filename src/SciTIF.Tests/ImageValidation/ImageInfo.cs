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
    public bool Grayscale;
    public List<PixelInfo> Pixels = new();
}
