using BitMiracle.LibTiff.Classic;

namespace SciTIF.TifReaders;

public interface IReadGrayscale
{
    public double[,] Read(Tiff tif);
}
