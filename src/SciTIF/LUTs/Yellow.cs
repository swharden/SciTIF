namespace SciTIF.LUTs;

public class Yellow : ILUT
{
    public PixelColor GetColor(byte value)
    {
        return new PixelColor(value, value, 0);
    }
}
