namespace SciTIF.LUTs;

public class Cyan : ILUT
{
    public PixelColor GetColor(byte value)
    {
        return new PixelColor(0, value, value);
    }
}
