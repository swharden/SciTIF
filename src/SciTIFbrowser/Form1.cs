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
        SciTIFlib.Path SciTifPath;

        public Form1()
        {
            InitializeComponent();
            SciTifPath = new SciTIFlib.Path();
        }

        private void Form1_Load(object sender, EventArgs e)
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
                lblStatus.Text = $"Cannot load {filePath}";
                return;
            }
            else
            {
                lblStatus.Text = $"Loading image...";
            }

            string imageFileName = System.IO.Path.GetFileName(filePath);

            // load the image and create the preview
            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
            tif = new SciTIFlib.TifFile(filePath);
            pbImage.Image = tif.GetBitmapForDisplay();
            stopwatch.Stop();
            double loadTimeMS = stopwatch.ElapsedTicks * 1000.0 / System.Diagnostics.Stopwatch.Frequency;

            // update the text around the program
            lblStatus.Text = string.Format("Image loaded in {0:0.00} ms", loadTimeMS);
            lblInfoDepthData.Text = "12-bit";
            lblInfoDepthTif.Text = "16-bit";
            lblInfoFileDimension.Text = "2048x2048";
            lblInfoFileName.Text = imageFileName;
            lblInfoFileSize.Text = String.Format("{0:0.00} kb", tif.fileSize / 1000.0);
            this.Text = $"SciTIF Browser - {imageFileName}";
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (pbImage.SizeMode == PictureBoxSizeMode.Zoom)
                pbImage.SizeMode = PictureBoxSizeMode.CenterImage;
            else
                pbImage.SizeMode = PictureBoxSizeMode.Zoom;
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
            else
                lblStatus.Text = ("no adjacent images");
        }

        private void ToggleDebugLog()
        {
            if (tableLayoutMain.RowStyles[2].Height == 0)
            {
                lblStatus.Text = "showing debug log";
                tableLayoutMain.RowStyles[2].Height = 200;
            }
            else
            {
                lblStatus.Text = "hiding debug log";
                tableLayoutMain.RowStyles[2].Height = 0;
            }
        }

        private void debugLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleDebugLog();
        }
    }
}
