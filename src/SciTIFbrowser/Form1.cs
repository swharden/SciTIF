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
            SetImageStretch(true);
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
            statusFname.Text = tif.fileBasename;
            UpdateTitleWithZoom();
        }

        public bool zoomStretch = true;
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            ToggleImageStretch();
        }

        public void ToggleImageStretch()
        {
            if (zoomStretch)
                SetImageStretchOFF();
            else
                SetImageStretchON();
        }

        public void SetImageStretch(bool stretch = false)
        {
            if (stretch)
                SetImageStretchON();
            else
                SetImageStretchOFF();
        }

        private void SetImageStretchON()
        {
            zoomStretch = true;
            panelImageHolder.AutoScroll = false;
            panelImageHolder_Resize(null, null);
        }

        private void SetImageStretchOFF()
        {
            zoomStretch = false;
            panelImageHolder.AutoScroll = true;
            panelImageHolder_Resize(null, null);
            pbImage.Location = new Point(0, 0);
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

        private void pbImage_SizeChanged(object sender, EventArgs e)
        {
            UpdateTitleWithZoom();
        }

        public void UpdateTitleWithZoom()
        {
            double zoom;
            if (pbImage.Dock == DockStyle.Fill)
            {
                double zoomX = 100 * (double)pbImage.Width / tif.width;
                double zoomY = 100 * (double)pbImage.Height / tif.height;
                zoom = Math.Min(zoomX, zoomY);
            }
            else
            {
                zoom = 100;
            }
            this.Text = string.Format("SciTIF Browser - {0} ({1:0.00}%)", tif.fileBasename, zoom);
        }

        private void panelImageHolder_Resize(object sender, EventArgs e)
        {
            if (zoomStretch)
            {
                int pnlWidth = panelImageHolder.Width;
                int pnlHeight = panelImageHolder.Height;

                double imageWidthToHeightRatio = (double)tif.width / tif.height;
                double panelWidthToHeightRatio = (double)pnlWidth / pnlHeight;

                int imageWidth = pnlWidth;
                int imageHeight = pnlHeight;

                int panelLocX = 0;
                int panelLocY = 0;

                if (imageWidthToHeightRatio > panelWidthToHeightRatio)
                {
                    // width is most constrained, so scale Y as needed
                    imageHeight = pnlWidth * (tif.height / tif.width);
                    panelLocY = pnlHeight / 2 - imageHeight / 2;
                }
                else
                {
                    // height is most constrained, so scale X as needed
                    imageWidth = pnlHeight * (tif.width / tif.height);
                    panelLocX = pnlWidth / 2 - imageWidth / 2;
                }

                // optionally pad the image to make it float a bit in the panel
                int pxPad = 0;
                imageWidth -= pxPad * 2;
                imageHeight -= pxPad * 2;
                panelLocX += pxPad;
                panelLocY += pxPad;

                // applt these measurements to the image itself
                pbImage.Location = new Point(panelLocX, panelLocY);
                pbImage.Size = new Size(imageWidth, imageHeight);
            }
            else
            {
                // size is 1:1 and just let the panel viewer handle the location
                pbImage.Size = new Size(tif.width, tif.height);
            }
        }

        private void pbImage_Paint(object sender, PaintEventArgs e)
        {
            Console.WriteLine("PAINTING BORDER");
            int width = 1;
            Color color = Color.Magenta;
            base.OnPaint(e);
            Rectangle rect = new Rectangle(new Point(0,0), pbImage.Size);
            var bs = ButtonBorderStyle.Solid;
            ControlPaint.DrawBorder(e.Graphics, rect,
                color, width, bs, color, width, bs,
                color, width, bs, color, width, bs);
        }
    }
}
