namespace SciTIF;

/// <summary>
/// 5D image loaded from a TIF file
/// </summary>
public class TifFile : Image5D
{
    public readonly string FilePath;
    public readonly string Description;

    public TifFile(string imageFilePath) : base()
    {
        FilePath = System.IO.Path.GetFullPath(imageFilePath);
        (Images, Description) = IO.TifReading.TifReader.LoadTif(FilePath);
        AssertAllImagesHaveSameDimensions();
    }
}
