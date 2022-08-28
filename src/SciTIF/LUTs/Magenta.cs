namespace SciTIF.LUTs;

public class Magenta : ILUT
{
    public PixelColor GetColor(byte value)
    {
        return new PixelColor(value, 0, value);
    }
}
