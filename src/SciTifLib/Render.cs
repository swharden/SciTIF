using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIFlib
{
    class Render
    {
        public Bitmap GenerateHistogram(int width, int height)
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
    }
}
