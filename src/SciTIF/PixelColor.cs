using System;

namespace SciTIF;

public struct PixelColor
{
    public byte R;
    public byte G;
    public byte B;

    public PixelColor()
    {
        R = 0;
        G = 0;
        B = 0;
    }

    public PixelColor(byte value)
    {
        R = value;
        G = value;
        B = value;
    }

    public PixelColor(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }

    public static PixelColor operator +(PixelColor colorA, PixelColor colorB)
    {
        byte r = (byte)(colorA.R + colorB.R);
        byte g = (byte)(colorA.G + colorB.G);
        byte b = (byte)(colorA.B + colorB.B);
        return new PixelColor(r, g, b);
    }

    public static PixelColor operator -(PixelColor colorA, PixelColor colorB)
    {
        byte r = (byte)(colorA.R - colorB.R);
        byte g = (byte)(colorA.G - colorB.G);
        byte b = (byte)(colorA.B - colorB.B);
        return new PixelColor(r, g, b);
    }
}
