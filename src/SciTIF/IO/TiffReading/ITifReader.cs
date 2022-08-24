using BitMiracle.LibTiff.Classic;

namespace SciTIF.IO.TiffReading;

public interface ITifReader
{
    public Image5D Read(Tiff tif);
    public bool IsRGBA { get; }
}
