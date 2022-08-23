using BitMiracle.LibTiff.Classic;

namespace SciTIF;

public interface ITifReader
{
    public ImageData[] Read(Tiff tif);
    public ImageData[] ReadDirectory(Tiff tif, int directory);
}
