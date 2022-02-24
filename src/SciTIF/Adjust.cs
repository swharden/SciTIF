﻿using System;
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
            (min, max) = GetMinMax(values);
        }
        else
        {
            (min, max) = GetPercentiles(values, percentileLow, percentileHigh);
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

    private static (double min, double max) GetPercentiles(double[,] values, double minPercentile, double maxPercentile)
    {
        int Height = values.GetLength(0);
        int Width = values.GetLength(1);
        int i = 0;
        double[] values2 = new double[Width * Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                values2[i++] = values[y, x];
            }
        }
        Array.Sort(values2);

        double minFrac = minPercentile / 100;
        double maxFrac = maxPercentile / 100;
        int minIndex = (int)(values2.Length * minFrac);
        int maxIndex = (int)(values2.Length * maxFrac);

        return (values2[minIndex], values2[maxIndex]);
    }

    private static (double min, double max) GetMinMax(double[,] values)
    {
        double max = double.NegativeInfinity;
        double min = double.PositiveInfinity;

        int Height = values.GetLength(0);
        int Width = values.GetLength(1);
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                max = Math.Max(max, values[y, x]);
                min = Math.Min(min, values[y, x]);
            }
        }

        return (min, max);
    }
}
