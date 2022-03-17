using BitMiracle.LibTiff.Classic;
using System;

namespace SciTIF.TiffErrorHandlers;

internal class Silent : BitMiracle.LibTiff.Classic.TiffErrorHandler
{
    public Silent()
    {
    }

    public override void ErrorHandler(Tiff tif, string module, string fmt, params object[] ap)
    {
    }

    public override void WarningHandler(Tiff tif, string module, string fmt, params object[] ap)
    {

    }
}
