using System;
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush);

            Rectangle rect = panel1.DisplayRectangle;

            Point[] points = new Point[4];
            points[0] = new Point(5, 5);
            points[1] = new Point(5, rect.Height-5);
            points[2] = new Point(rect.Width-5, rect.Height-5);
            points[3] = new Point(rect.Width-5, 5);
            gfx.DrawPolygon(pen, points);

            Font font = new Font(FontFamily.GenericMonospace, 8, FontStyle.Regular);
            Random rnd = new Random();
            double rand = rnd.NextDouble();
            gfx.DrawString($"proof of concept\n{rand}", font, brush, new Point(7, 7));
        }
    }
}
