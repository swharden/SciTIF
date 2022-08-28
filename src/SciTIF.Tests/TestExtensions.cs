using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests;

internal static class TestExtensions
{
    private static string GetOutputFolder()
    {
        string outputFolder = Path.Combine(Path.GetTempPath(), "SciTif-Tests");
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);
        return outputFolder;
    }

    public static void Save_TEST(this ImageRGB image, string fileName)
    {
        string saveAs = Path.Combine(GetOutputFolder(), fileName);
        image.Save(saveAs);
        Console.WriteLine(saveAs);
    }

    public static void Save_TEST(this Image image, string fileName)
    {
        string saveAs = Path.Combine(GetOutputFolder(), fileName);
        image.Save(saveAs);
        Console.WriteLine(saveAs);
    }
}
