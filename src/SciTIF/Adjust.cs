using System;
using System.Collections.Generic;
using System.Text;

namespace SciTIF;

public static class Adjust
{
    public static double[,] AutoScale(double[,] values, double percentileLow = 0, double percentileHigh = 100)
    {
        double newMax = 255;
        double min;
        double max;

        if (percentileLow == 0 && percentileHigh == 100)
        {
            (min, max) = Analyze.GetMinMax(values);
        }
        else
        {
            (min, max) = Analyze.GetPercentiles(values, percentileLow, percentileHigh);
        }

        double scale = newMax / (max - min);

        int Height = values.GetLength(0);
        int Width = values.GetLength(1);
        double[,] values2 = new double[Height, Width];

        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                values2[y, x] = (values[y, x] - min) * scale;

        return values2;
    }
}
