using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

// add the "Presentation Core" assembly to access this
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExternalDemo
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            splitDirView1.SetFont(8);
            string demoTifFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            demoTifFolder = System.IO.Path.GetFullPath(demoTifFolder + "/../../../../data/images/");
            splitDirView1.SetFolder(demoTifFolder);
            splitDirView1.SelectFile(10);
            splitDirView1.Select();
        }

        public void log(string msg)
        {
            Console.WriteLine(msg);
            richTextBox1.Text += msg + "\n";
        }

        private void splitDirView1_FileHighlighted(object sender, EventArgs e)
        {
            string tifFilePath = splitDirView1.highlightedFile;
            lblFileName.Text = System.IO.Path.GetFileName(tifFilePath);

            richTextBox1.Clear();
            log($"LOADING TIF: {tifFilePath}");

            // open a file stream and keep it open until we're done reading the file
            Stream stream = new FileStream(tifFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            // carefully open the file to see if it will decode
            TiffBitmapDecoder decoder;
            try
            {
                decoder = new TiffBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            }
            catch
            {
                log($"TiffBitmapDecoder crashed");
                return;
            }

            // determine how many single images are in this TIF
            int nFrames = decoder.Frames.Count;
            log($"number of frames: {nFrames}");

            // pull the first frame and inspect it
            BitmapSource bitmapSource = decoder.Frames[0];
            log($"Image depth: {bitmapSource.Format.BitsPerPixel}");
            log($"image width: {bitmapSource.PixelWidth}");
            log($"image height: {bitmapSource.PixelHeight}");

            // CONCLUSION - EPHYS SOFTWARE OUTPUTS SINGLE CHANNEL IMAGES!
            // MULTICHANNEL MULTI SERIES ETC IS FLAKEY AND SPECIFIC
            // WRITE THIS SOFTWARE TO ONLY TAKE-IN SINGLE CHANNEL IMAGES

            //log($"meta Subject: {bitmapSource..}");
            //log($"meta Title: {bitmapSource.Decoder}");

            // determine if colormap or grayscale
            if (bitmapSource.Palette==null)
                log($"Image Format: grayscale");
            else
                log($"Image Format: {bitmapSource.Palette.Colors.Count} colors");

            stream.Dispose();

        }
    }
}
