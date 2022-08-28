namespace SciTIF.LUTs;

public class Red : ILUT
{
    public PixelColor GetColor(byte value)
    {
        return new PixelColor(value, 0, 0);
    }
}
