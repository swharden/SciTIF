﻿namespace SciTIF;

public class ImageRGB : IBitmap
{
    public readonly Image Red;
    public readonly Image Green;
    public readonly Image Blue;
    public int Height => Red.Height;
    public int Width => Red.Width;

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

    public byte[] GetBitmapBytes()
    {
        return IO.SystemDrawing.GetBitmapBytes(Red, Green, Blue);
    }
}
