namespace SciTIF.LUTs;

public class Green : ILUT
{
    public PixelColor GetColor(byte value)
    {
        return new PixelColor(0, value, 0);
    }
}
