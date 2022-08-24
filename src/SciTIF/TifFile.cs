using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using BitMiracle.LibTiff.Classic;
using SciTIF.IO.TiffReading;

namespace SciTIF;

public class TifFile
{
    public readonly string FilePath;
    public readonly Image5D Data;
    public readonly string FormatDescription;
    public readonly ITifReader Reader;

    public int Width => Data.Width;
    public int Height => Data.Height;
    public int Channels => Data.Channels;
    public int Slices => Data.Slices;
    public int Frames => Data.Frames;

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
        Data = Reader.Read(tif);

    }

    public static Dictionary<string, FieldValue> GetHeaderDictionary(Tiff tif)
    {
        Dictionary<string, FieldValue> d = new();
        foreach (TiffTag tag in Enum.GetValues(typeof(TiffTag)))
        {
            FieldValue[] res = tif.GetFieldDefaulted(tag);
            if (res is null)
                continue;
            for (int i = 0; i < res.Length; i++)
            {
                d[$"{tag}[{i}]"] = res[i];
            }
        }
        return d;
    }

    private class SilentErrorHandler : TiffErrorHandler
    {
        public SilentErrorHandler() { }
        public override void ErrorHandler(Tiff tif, string module, string fmt, params object[] ap) { }
        public override void WarningHandler(Tiff tif, string module, string fmt, params object[] ap) { }
    }

    public MultiChannelImage GetSlice(int slice, int frame)
    {
        return Data.GetSlice(slice, frame);
    }
}
