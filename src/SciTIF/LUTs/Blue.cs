namespace SciTIF.LUTs;

public class Blue : ILUT
{
    public PixelColor GetColor(byte value)
    {
        return new PixelColor(0, 0, value);
    }
}
