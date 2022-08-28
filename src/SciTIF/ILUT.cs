namespace SciTIF;

/// <summary>
/// Lookup table for converting grayscale values into RGB color
/// </summary>
public interface ILUT
{
    public PixelColor GetColor(byte value);
}
