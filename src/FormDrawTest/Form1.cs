﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormDrawTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Invalidate(); // forces repaint
        }

        private Bitmap GenerateHistogram(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics gfx = Graphics.FromImage(bmp);
            
            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush);

            Point[] points = new Point[4];
            points[0] = new Point(5, 5);
            points[1] = new Point(5, height - 5);
            points[2] = new Point(width - 5, height - 5);
            points[3] = new Point(width - 5, 5);
            gfx.DrawPolygon(pen, points);

            return bmp;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
            Graphics gfx = e.Graphics;
            e.Graphics.DrawImage(GenerateHistogram(panel1.Width, panel1.Height), 0, 0);
        }
    }
}
