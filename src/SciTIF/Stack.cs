using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SciTIF;

public class Stack
{
    public readonly List<Image> Slices = new();
    public int Count => Slices.Count;
    public int Width => Slices.First().Width;
    public int Height => Slices.First().Height;

    public Stack()
    {

    }

    public Stack(IEnumerable<Image> slices)
    {
        Slices.AddRange(slices);
    }
}
