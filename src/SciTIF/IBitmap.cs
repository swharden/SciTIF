namespace SciTIF;

/// <summary>
/// Describes any object which can be viewed or saved as a bitmap image
/// </summary>
public interface IBitmap : IImage
{
    void Save(string saveAs, int quality = 90);
    byte[] GetBitmapBytes();
}
