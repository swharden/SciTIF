using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SciTIFlib
{
    public partial class BandC : UserControl
    {

        public class DisplayLimits
        {
            public int limitMin { get; private set; }
            public int limitMax { get; private set; }
            public int min { get; private set; }
            public int max { get; private set; }

            public DisplayLimits(int limitMax = 255, int limitMin = 0)
            {
                this.limitMax = limitMax;
                this.limitMin = limitMin;
                max = limitMax;
                min = limitMin;
            }

            public void SetMin(int min)
            {
                if (min > max)
                    min = max;
                this.min = min;
            }

            public void SetMax(int max)
            {
                if (max < min)
                    max = min;
                this.max = max;
            }

            public Bitmap CreateHistogram(Size size)
            {
                Bitmap bmp = new Bitmap(size.Width, size.Height);
                Graphics gfx = Graphics.FromImage(bmp);
                gfx.Clear(Color.White);

                Rectangle rect = new Rectangle(0, 0, size.Width - 1, size.Height - 1);
                gfx.DrawRectangle(Pens.Black, rect);

                int pointMin = size.Width * min / (limitMax - limitMin);
                int pointMax = size.Width * max / (limitMax - limitMin);
                gfx.DrawLine(Pens.Black, new Point(pointMin, size.Height - 1), new Point(pointMax, 0));

                return bmp;
            }

        }

        DisplayLimits dispLim;
        public BandC()
        {
            InitializeComponent();
            dispLim = new DisplayLimits();
            UpdateGuiFromDisplayLimits();
        }

        public void UpdateGuiFromDisplayLimits()
        {
            // histogram
            pictureBox1.Image = dispLim.CreateHistogram(pictureBox1.Size);

            // minimum
            nudMin.Maximum = dispLim.limitMax;
            nudMin.Minimum = dispLim.limitMin;
            nudMin.Value = dispLim.min;
            scrollMin.Maximum = dispLim.limitMax;
            scrollMin.Minimum = dispLim.limitMin;
            scrollMin.Value = dispLim.min;

            // maximum
            nudMax.Maximum = dispLim.limitMax;
            nudMax.Minimum = dispLim.limitMin;
            nudMax.Value = dispLim.max;
            scrollMax.Maximum = dispLim.limitMax;
            scrollMax.Minimum = dispLim.limitMin;
            scrollMax.Value = dispLim.max;
        }

        private void nudMin_ValueChanged(object sender, EventArgs e)
        {
            dispLim.SetMin((int)nudMin.Value);
            UpdateGuiFromDisplayLimits();
        }

        private void scrollMin_Scroll(object sender, ScrollEventArgs e)
        {
            dispLim.SetMin(scrollMin.Value);
            UpdateGuiFromDisplayLimits();
        }

        private void nudMax_ValueChanged(object sender, EventArgs e)
        {
            dispLim.SetMax((int)nudMax.Value);
            UpdateGuiFromDisplayLimits();
        }

        private void scrollMax_Scroll(object sender, ScrollEventArgs e)
        {
            dispLim.SetMax(scrollMax.Value);
            UpdateGuiFromDisplayLimits();
        }

        private void btnMinFull_Click(object sender, EventArgs e)
        {
            dispLim.SetMin(dispLim.limitMin);
            UpdateGuiFromDisplayLimits();
        }

        private void btnMaxFull_Click(object sender, EventArgs e)
        {
            dispLim.SetMax(dispLim.limitMax);
            UpdateGuiFromDisplayLimits();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dispLim.SetMin(dispLim.limitMin);
            dispLim.SetMax(dispLim.limitMax);
            UpdateGuiFromDisplayLimits();
        }
    }
}
