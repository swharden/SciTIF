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
        }

        //public Bitmap[] bmpFrames;
        Bitmap bmpOriginal;
        public void LoadRGBImage()
        {
            string filePath = @"D:\demoData\tifs\simple\rgb-test2.jpg";
            bmpOriginal = new Bitmap(filePath);
        }

        /// <summary>
        /// return a bitmap (of any image format) as a byte array of its data values
        /// </summary>
        public byte[] BitmapToBytes(Bitmap bmp)
        {
            int bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            byte[] bytes = new byte[bmp.Width * bmp.Height * bytesPerPixel];
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
            Marshal.Copy(bmpData.Scan0, bytes, 0, bytes.Length);
            bmp.UnlockBits(bmpData);
            return bytes;
        }

        /// <summary>
        /// create a bitmap given a byte array of raw data
        /// </summary>
        public Bitmap BitmapFromBytes(byte[] bytes, int width, int height, PixelFormat format = PixelFormat.Format8bppIndexed)
        {
            Bitmap bmp = new Bitmap(width, height, format);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            Marshal.Copy(bytes, 0, bmpData.Scan0, bytes.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// Apply red, green, blue, magenta, or gray LUT to an 8-bit indexed bitmap
        /// </summary>
        public void BitmapApplyPalette(Bitmap bmp, string color = "gray")
        {
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < 256; i++)
            {
                if (color == "red")
                    pal.Entries[i] = System.Drawing.Color.FromArgb(255, i, 0, 0);
                else if (color == "green")
                    pal.Entries[i] = System.Drawing.Color.FromArgb(255, 0, i, 0);
                else if (color == "blue")
                    pal.Entries[i] = System.Drawing.Color.FromArgb(255, 0, 0, i);
                else if (color == "magenta")
                    pal.Entries[i] = System.Drawing.Color.FromArgb(255, i, 0, i);
                else if (color == "gray")
                    pal.Entries[i] = System.Drawing.Color.FromArgb(255, i, i, i);
                else
                    Console.WriteLine($"Unknown color pallette code: {color}");
            }
            bmp.Palette = pal;
        }

        public Bitmap ExtractChannelToGrayscale(Bitmap bmpSource, int channel = 0, bool grayscale = false)
        {
            // copy our source image to a byte array (8-bits/pixel, 3 channel RGB)
            byte[] bytesRGB = BitmapToBytes(bmpSource);
            int nPixels = bytesRGB.Length / 3;

            // create a byte array for out grayscale image (16-bits/pixel, 1 channel)
            byte[] bytesGray = new byte[nPixels];

            for (int pixel = 0; pixel < nPixels; pixel++)
            {
                bytesGray[pixel] = bytesRGB[3 * pixel + (2 - channel)];
            }

            // make a 16-bit grayscale image the same size as the source
            Bitmap bmpGray = BitmapFromBytes(bytesGray, bmpSource.Width, bmpSource.Height);

            if (grayscale)
                BitmapApplyPalette(bmpGray, "gray");
            else if (channel == 0)
                BitmapApplyPalette(bmpGray, "red");
            else if (channel == 1)
                BitmapApplyPalette(bmpGray, "green");
            else if (channel == 2)
                BitmapApplyPalette(bmpGray, "blue");

            return bmpGray;
        }

        public void SetSliceAndChannel(int slice, int channel)
        {
            if (cbDisplayMode.Text == "Merge")
            {
                pictureBox1.Image = bmpOriginal;
            }
            else if (cbDisplayMode.Text == "Grayscale")
            {
                pictureBox1.Image = ExtractChannelToGrayscale(bmpOriginal, hScrollChannel.Value, true);
            }
            else if (cbDisplayMode.Text == "Color")
            {
                pictureBox1.Image = ExtractChannelToGrayscale(bmpOriginal, hScrollChannel.Value); 
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cbDisplayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSliceAndChannelFromScrollBars();
        }
    }
}
