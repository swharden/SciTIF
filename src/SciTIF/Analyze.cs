using System;

namespace SciTIF;

internal class Analyze
{
    public static (double min, double max) GetMinMax(double[] values)
    {
        double max = double.NegativeInfinity;
        double min = double.PositiveInfinity;

        for (int x = 0; x < values.Length; x++)
        {
            max = Math.Max(max, values[x]);
            min = Math.Min(min, values[x]);
        }

        return (min, max);
    }

    public static (double min, double max) GetPercentiles(double[] values, double minPercentile, double maxPercentile)
    {
        // TODO: exception if outside range
        double minFrac = minPercentile / 100;
        double maxFrac = maxPercentile / 100;

        double[] values2 = new double[values.Length];
        Array.Copy(values, values2, values.Length);
        Array.Sort(values2);

        int minIndex = (int)(values2.Length * minFrac);
        int maxIndex = (int)(values2.Length * maxFrac);

        return (values2[minIndex], values2[maxIndex]);
    }
}
