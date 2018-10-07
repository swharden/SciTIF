using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sandbox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cbDisplayMode.SelectedItem = cbDisplayMode.Items[0];
            cbLUT.SelectedItem = cbLUT.Items[0];
            LoadRGBImage();
            SetSliceAndChannelFromScrollBars();
            //tableLayoutPanel3.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private FrameSet frameSet;
        public void LoadRGBImage()
        {
            string filePath = @"D:\demoData\tifs\simple\rgb-test2.jpg";
            frameSet = new FrameSet(filePath);
        }

        public void SetSliceAndChannel(int slice, int channel)
        {
            // abort if images aren't loaded yet
            if (frameSet == null)
                return;

            // update the channel visibility checkbox
            checkVisible.Checked = frameSet.frames[channel].visible;
            cbLUT.Text = frameSet.frames[channel].color;
            lblC.Text = $"C: {channel + 1}/{frameSet.frames.Length}";
            lblZ.Text = $"Z: {slice + 1}/{22}";

            // paint the image according to the display mode
            if (cbDisplayMode.Text == "Merge")
            {
                Bitmap bmp = frameSet.BlankImage();
                foreach (Frame frame in frameSet.frames)
                {
                    bmp = frameSet.AddChannel(bmp, frame);
                }
                pictureBox1.Image = bmp;
            }
            else if (cbDisplayMode.Text == "Grayscale")
            {
                Bitmap bmp = frameSet.BlankImage();
                bmp = frameSet.AddChannel(bmp, frameSet.frames[channel], "gray");
                pictureBox1.Image = bmp;
            }
            else if (cbDisplayMode.Text == "Color")
            {
                Bitmap bmp = frameSet.BlankImage();
                bmp = frameSet.AddChannel(bmp, frameSet.frames[channel]);
                pictureBox1.Image = bmp;
            }
            else
                Console.WriteLine("Unknown display mode");

        }

        public void SetSliceAndChannelFromScrollBars()
        {
            SetSliceAndChannel(hScrollSlice.Value, hScrollChannel.Value);
        }

        private void hScrollChannel_Scroll(object sender, ScrollEventArgs e)
        {
            SetSliceAndChannelFromScrollBars();
        }

        private void hScrollChannel_ValueChanged(object sender, EventArgs e)
        {
            SetSliceAndChannelFromScrollBars();
        }

        private void hScrollSlice_ValueChanged(object sender, EventArgs e)
        {
            SetSliceAndChannelFromScrollBars();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbDisplayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSliceAndChannelFromScrollBars();
        }

        private void hScrollChannel_Scroll_1(object sender, ScrollEventArgs e)
        {

        }

        private void checkVisible_CheckedChanged(object sender, EventArgs e)
        {
            frameSet.frames[hScrollChannel.Value].visible = checkVisible.Checked;
            SetSliceAndChannelFromScrollBars();
        }
    }
}
