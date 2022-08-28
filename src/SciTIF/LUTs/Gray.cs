namespace SciTIF.LUTs;

public class Gray : ILUT
{
    public PixelColor GetColor(byte value)
    {
        return new PixelColor(value);
    }
}
