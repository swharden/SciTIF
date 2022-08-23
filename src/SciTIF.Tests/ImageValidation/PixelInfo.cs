namespace SciTIF.Tests.ImageValidation;

public struct PixelInfo
{
    public int X;
    public int Y;
    public double Value;

    public PixelInfo(int x, int y, double value)
    {
        X = x;
        Y = y;
        Value = value;
    }
}
