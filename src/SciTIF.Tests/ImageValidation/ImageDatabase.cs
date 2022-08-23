using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests.ImageValidation;

public class ImageDatabase
{
    public readonly Dictionary<string, ImageInfo> Infos = new();
    public int Count => Infos.Count;

    string lastTitle = string.Empty;

    public ImageDatabase(string ijInfoFile)
    {
        System.IO.File.ReadAllLines(ijInfoFile)
            .ToList()
            .ForEach(x => ProcessLine(x));
    }

    public void ProcessLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return;

        if (line.StartsWith("title = "))
        {
            lastTitle = line.Split("=")[1].Trim();
            Infos[lastTitle] = new();
            return;
        }

        ImageInfo info = Infos[lastTitle];

        if (line.Contains("["))
        {
            if (line.StartsWith("pixel"))
            {
                (int x, int y) = line.BracketedXandY();
                double val = line.DoubleAfterEquals();
                info.Pixels.Add(new(x, y, val));
            }
            return;
        }

        if (line.StartsWith("width = ")) info.Width = line.IntAfterEquals();
        if (line.StartsWith("height = ")) info.Height = line.IntAfterEquals();
        if (line.StartsWith("channels = ")) info.Channels = line.IntAfterEquals();
        if (line.StartsWith("slices = ")) info.Slices = line.IntAfterEquals();
        if (line.StartsWith("frames = ")) info.Frames = line.IntAfterEquals();
        if (line.StartsWith("depth = ")) info.Depth = line.IntAfterEquals();
        if (line.StartsWith("grayscale = ")) info.Grayscale = line.IntAfterEquals() > 0;

    }
}

public static class ImageDatabaseExtensions
{
    public static (int x, int y) BracketedXandY(this string s)
    {
        int x = int.Parse(s.Split("[")[1].Split(",")[0]);
        int y = int.Parse(s.Split(",")[1].Split("]")[0]);
        return (x, y);
    }

    public static double DoubleAfterEquals(this string s)
    {
        return double.Parse(s.Split("=").Last());
    }

    public static int IntAfterEquals(this string s)
    {
        return int.Parse(s.Split("=").Last());
    }
}