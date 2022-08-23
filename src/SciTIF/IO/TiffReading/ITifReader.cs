using BitMiracle.LibTiff.Classic;

namespace SciTIF.IO.TiffReading;

public interface ITifReader
{
    public Stack ReadAllSlices(Tiff tif);
    public Image ReadSlice(Tiff tif, int directory);
}
