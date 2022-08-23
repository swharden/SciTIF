using BitMiracle.LibTiff.Classic;

namespace SciTIF;

public interface ITifReader
{
    public ImageDataXY[] Read(Tiff tif);
    public ImageDataXY[] ReadDirectory(Tiff tif, int directory);
}
