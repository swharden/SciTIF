namespace SciTIF;

public static class Adjust
{
    public static void AutoScale(ImageData img, double percentileLow = 0, double percentileHigh = 100)
    {
        double newMax = 255;
        double min;
        double max;

        if (percentileLow == 0 && percentileHigh == 100)
        {
            (min, max) = Analyze.GetMinMax(img.Values);
        }
        else
        {
            (min, max) = Analyze.GetPercentiles(img.Values, percentileLow, percentileHigh);
        }

        double scale = newMax / (max - min);

        for (int i = 0; i < img.Values.Length; i++)
            img.Values[i] = (img.Values[i] - min) * scale;
    }
}
