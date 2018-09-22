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
            button1_Click(null, null);
        }

        public double[] RandomGaussianArray(int count)
        {
            double[] data = new double[count];
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                double u1 = 1.0 - rand.NextDouble();
                double u2 = 1.0 - rand.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                data[i] = randStdNormal;
            }
            return data;
        }

        public Bitmap Histogram(Size size, double[] values)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);

            Graphics gfx = Graphics.FromImage(bmp);
            gfx.Clear(Color.White);

            // bin the data into 1px columns
            double dataMin = values.Min();
            double dataMax = values.Max();
            double dataSpan = dataMax - dataMin;
            int nBins = size.Width;
            double[] counts = new double[nBins];
            double binSize = dataSpan / (nBins - 1);
            for (int i = 0; i < values.Length; i++)
            {
                int bin = (int)((values[i] - dataMin) / binSize);
                if (bin >= counts.Length)
                    bin = counts.Length - 1;
                if (bin < 0)
                    bin = 0;
                counts[bin] = counts[bin] + 1;
            }

            // determine what to normalize it to visually
            double peakVal = counts.Max();
            double heightMult = size.Height / peakVal;

            // plot the binned data
            Pen pen = new Pen(new SolidBrush(Color.Black));
            for (int i = 0; i < nBins; i++)
            {
                int heightPx = (int)(counts[i] * heightMult);
                Point pt1 = new Point(i, size.Height - 0);
                Point pt2 = new Point(i, size.Height - heightPx);
                gfx.DrawLine(pen, pt1, pt2);
            }

            return bmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] values = RandomGaussianArray(10000);
            pictureBox1.Image = Histogram(pictureBox1.Size, values);
        }
    }
}
