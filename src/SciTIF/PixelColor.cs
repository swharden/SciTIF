namespace SciTIF;

public struct PixelColor
{
    public byte R;
    public byte G;
    public byte B;
    public byte A;

    public PixelColor(byte value)
    {
        R = value;
        G = value;
        B = value;
        A = 255;
    }

    public PixelColor(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
}
