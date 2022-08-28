using System;
using System.Collections.Generic;
using System.Text;

namespace SciTIF;

public class ImageRGB
{
    readonly Image Red;
    readonly Image Green;
    readonly Image Blue;

    public ImageRGB(Image red, Image green, Image blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public void Save(string path)
    {
        IO.SystemDrawing.SavePNG(path, Red, Green, Blue);
    }
}
