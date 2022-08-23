using BitMiracle.LibTiff.Classic;
using System.Linq;

namespace SciTIF.TifReaders;

internal abstract class ReaderBase : ITifReader
{
    public Stack ReadAllSlices(Tiff tif)
    {
        var slices = Enumerable.Range(0, tif.NumberOfDirectories()).Select(x => ReadSlice(tif, x));
        return new Stack(slices);
    }

    public abstract Image ReadSlice(Tiff tif, int directory);
}
