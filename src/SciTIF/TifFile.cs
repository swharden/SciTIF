using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SciTIF;

public class TifFile : Image5D
{
    public string FilePath { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public TifFile(string imageFilePath) : base()
    {
        FilePath = Path.GetFullPath(imageFilePath);
        (Images, Description) = IO.TifReading.TifReader.LoadTif(FilePath);
        AssertAllImagesHaveSameDimensions();
    }
}
