﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    internal class QuickstartTests
    {
        [Test]
        public void Test_Quickstart()
        {
            string path = SampleData.TifWithFramesSlicesAndChannels;
            Image5D img5d = new(path);
            Image slice = img5d.GetImage(0, 0, 0);
            slice.AutoScale();
            slice.SavePng("test");
        }
    }
}