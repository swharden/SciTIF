using BitMiracle.LibTiff.Classic;

namespace SciTIF;

public interface ITifReader
{
    public ImageData[] ReadAllSlices(Tiff tif);
    public ImageData ReadSlice(Tiff tif, int directory);
}
