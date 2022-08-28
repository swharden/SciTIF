namespace SciTIF.LUTs;

public class Black : ILUT
{
    public PixelColor GetColor(byte value)
    {
        return new PixelColor(0, 0, 0);
    }
}
