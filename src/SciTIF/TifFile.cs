using System;
using System.IO;
using BitMiracle.LibTiff.Classic;
using SciTIF.IO.TiffReading;

namespace SciTIF;

public class TifFile
{
    public readonly string FilePath;
    public readonly Stack Stack;
    public readonly string FormatDescription;
    public readonly ITifReader Reader;

    public int Width => Stack.Width;
    public int Height => Stack.Height;
    public int Channels => Stack.Channels;
    public int Slices => Stack.Count;

    public TifFile(string filePath)
    {
        bool isTifExtension = filePath.EndsWith(".TIF", StringComparison.OrdinalIgnoreCase) ||
            filePath.EndsWith(".TIFF", StringComparison.OrdinalIgnoreCase);

        if (!isTifExtension)
            throw new ArgumentException($"{nameof(filePath)} must end with .tif or .tiff: {filePath}");

        FilePath = Path.GetFullPath(filePath);
        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        Tiff.SetErrorHandler(new SilentErrorHandler());
        using Tiff tif = Tiff.Open(FilePath, "r");

        if (tif is null)
            throw new InvalidProgramException($"Y U NULL? {filePath}");

        (Reader, FormatDescription) = TifReaderFactory.GetBestReader(tif);
        Stack = Reader.ReadAllSlices(tif);
    }

    private class SilentErrorHandler : TiffErrorHandler
    {
        public SilentErrorHandler() { }
        public override void ErrorHandler(Tiff tif, string module, string fmt, params object[] ap) { }
        public override void WarningHandler(Tiff tif, string module, string fmt, params object[] ap) { }
    }

    public Image GetSlice(int sliceIndex)
    {
        return Stack.Slices[sliceIndex];
    }
}
