using System;
using System.Collections.Generic;
using System.Text;

namespace SciTIF;

public class ImageRGB
{
    public readonly Image Red;
    public readonly Image Green;
    public readonly Image Blue;

    public ImageRGB(Image red, Image green, Image blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public void Save(string path, int quality = 90)
    {
        IO.SystemDrawing.Save(path, Red, Green, Blue, quality);
    }

    public System.Drawing.Bitmap GetBitmap()
    {
        return IO.SystemDrawing.GetBitmap(Red, Green, Blue);
    }
}
