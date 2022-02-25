using BitMiracle.LibTiff.Classic;

namespace SciTIF.TifReaders;

public interface IReadGrayscale
{
    public ImageData ReadGrayscale(Tiff tif);
}
