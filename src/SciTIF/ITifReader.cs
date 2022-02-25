using BitMiracle.LibTiff.Classic;

namespace SciTIF;

public interface ITifReader
{
    public ImageData[] Read(Tiff tif);
}
