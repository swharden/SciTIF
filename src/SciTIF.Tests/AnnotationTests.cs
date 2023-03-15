using NUnit.Framework;

namespace SciTIF.Tests;

internal class AnnotationTests
{
    [Test]
    public void Test_Annotation_Video()
    {
        string imagePath = SampleData.Tif16bitStack;
        TifFile tif = new(imagePath);
        Image[] images = tif.GetAllImages();

        foreach(Image image in images)
            image.ScaleBy(0, 1.0 / 16);

        VideoAnnotator annotator = new(images, 10);
        annotator.SaveWebm("test.webm");
    }
}
