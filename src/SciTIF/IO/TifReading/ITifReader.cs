using BitMiracle.LibTiff.Classic;

namespace SciTIF.IO.TifReading;

public interface ITifReader
{
    public Image[,,] Read(Tiff tif);
    public bool IsRGBA { get; }
}
