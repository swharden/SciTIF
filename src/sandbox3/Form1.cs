using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sandbox3
{
    public partial class Form1 : Form
    {
        FrameSet frameSet;
        public Form1()
        {
            InitializeComponent();

            string filePath = @"D:\demoData\tifs\simple\rgb-test2.jpg";
            frameSet = new FrameSet(filePath);
            pictureBox1.Image = frameSet.bmpOriginal;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void channelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formChannels = new FormChannels();
            formChannels.Show();
        }

        private void brightnessAndContrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formContrast = new FormContrast();
            formContrast.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            brightnessAndContrastToolStripMenuItem_Click(null, null);
        }
    }
}
