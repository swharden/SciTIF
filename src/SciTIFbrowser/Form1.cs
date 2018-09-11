using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SciTIFbrowser
{
    public partial class Form1 : Form
    {
        SciTIFlib.TifFile tif;
        SciTIFlib.Path SciTifPath = new SciTIFlib.Path();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DeveloperInit();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            DeveloperInit();
        }

        private void DeveloperInit()
        {
            //ToggleDebugLog();
            string demoFile = @"D:\demoData\tifs\proj.tif";
            LoadImage(demoFile);
        }

        public void LoadImage(string filePath)
        {
            // ensure the path is real
            if (filePath == null || !System.IO.File.Exists(filePath))
            {
                //statusDepthImage.Text = $"Cannot load {filePath}";
                return;
            }
            else
            {
                //statusDepthImage.Text = $"Loading image...";
            }

            string imageFileName = System.IO.Path.GetFileName(filePath);

            // load the image and create the preview
            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
            tif = new SciTIFlib.TifFile(filePath);
            pbImage.Image = tif.GetBitmapForDisplay();
            stopwatch.Stop();
            double loadTimeMS = stopwatch.ElapsedTicks * 1000.0 / System.Diagnostics.Stopwatch.Frequency;

            // update the text around the program
            //statusDepthImage.Text = string.Format("Image loaded in {0:0.00} ms", loadTimeMS);
            statusDepthImage.Text = $"{tif.depthImage}-bit file";
            statusDepthData.Text = $"{tif.depthData}-bit data";
            statusResolution.Text = $"{tif.width}x{tif.height}";
            statusSize.Text = $"{Math.Round(tif.fileSize / 1e6, 2)} MB";
            statusMin.Text = $"Min: {tif.min}";
            double maxPercentData = tif.max / Math.Pow(2, tif.depthData) * 100;
            maxPercentData = Math.Round(maxPercentData, 1);
            double maxPercentImage = tif.max / Math.Pow(2, tif.depthImage) * 100;
            maxPercentImage = Math.Round(maxPercentImage, 1);
            statusMax.Text = $"Max: {tif.max} ({maxPercentData}%, {maxPercentImage}%)";
            statusFname.Text = System.IO.Path.GetFileName(tif.filePath);
            this.Text = $"SciTIF Browser - {imageFileName}";
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (pbImage.Dock == DockStyle.Fill)
            {
                // it's currently stretching, so make it 1:1
                pbImage.Size = new Size(tif.width, tif.height);
                pbImage.Dock = DockStyle.None;
                pbImage.SizeMode = PictureBoxSizeMode.Normal;
            }
            else
            {
                // it's currently 1:1, so make it stretch
                pbImage.Dock = DockStyle.Fill;
                pbImage.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "TIF Files (*.tif)|*.tif|All files (*.*)|*.*";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                LoadImage(diag.FileName);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            System.Console.WriteLine("KEYPRESS");
            string ImageToLoad = null;
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Right:
                    ImageToLoad = SciTifPath.AdjacentFilename(tif.filePath, ".tif", false);
                    break;
                case Keys.Up:
                case Keys.Left:
                    ImageToLoad = SciTifPath.AdjacentFilename(tif.filePath, ".tif", true);
                    break;
            }

            if (ImageToLoad != null)
                LoadImage(ImageToLoad);
            //else
            //statusDepthImage.Text = ("no adjacent images");
        }

        private void LaunchDebugLog()
        {
            // the debug log is the history for the current TIF
            Form formLog = new FormLog();
            formLog.Show();
            formLog.Activate();
        }

        private void debugLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchDebugLog();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

    }
}
