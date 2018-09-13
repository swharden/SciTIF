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

        ////////////////////////////////////////////////////////////
        /// STARTUP SEQUENCE
        ///

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

        ////////////////////////////////////////////////////////////
        /// DEVELOPER TOOLS

        private void DeveloperInit()
        {
            //ToggleDebugLog();
            string demoFile = @"D:\demoData\tifs\proj.tif";
            LoadImage(demoFile);
        }

        ////////////////////////////////////////////////////////////
        /// CORE FUNCTIONS

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

            // the image is done loading. Update the rest of the program.
            panelImageHolder_Resize(null, null);
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
            // this function is called when the window is resized. If zoomStretch is off, the image
            // is displayed in 1:1 at its full size and the picturebox (with scrollbars) takes total care
            // of the location. If zoomStretch is on, we have to resize the image however we can so it's
            // maximally shown in the available image area.

            if (zoomStretch)
            {
                int panelSizeX = panelImageHolder.Width;
                int panelSizeY = panelImageHolder.Height;

                // determine which way we should scale it
                double ratioX = (double)panelSizeX / tif.width;
                double ratioY = (double)panelSizeY / tif.height;
                bool panelIsWiderThanTall = ratioX > ratioY;

                Point imagePos;
                Size imageSize;

                if (panelIsWiderThanTall)
                {
                    // match the HEIGHTS and center it HORIZONTALLY
                    int imageSizeX = (int)(panelSizeY * ((double)tif.width / tif.height));
                    imageSize = new Size(imageSizeX, panelSizeY);
                    imagePos = new Point((panelSizeX - imageSizeX) / 2, 0);
                }
                else
                {
                    // match the WIDTHS and center it VERTICALLY
                    int imageSizeY = (int)(panelSizeX * ((double)tif.height / tif.width));
                    imageSize = new Size(panelSizeX, imageSizeY);
                    imagePos = new Point(0, (panelSizeY - imageSizeY) / 2);
                }

                pbImage.Location = imagePos;
                pbImage.Size = imageSize;
            }
            else
            {
                // size is 1:1 and just let the panel viewer handle the location
                pbImage.Size = new Size(tif.width, tif.height);
            }
        }

        private void pbImage_Paint(object sender, PaintEventArgs e)
        {
            // draw a colored box around the edge of the image
            int width = 1;
            Color color = Color.LightGreen;
            base.OnPaint(e);
            Rectangle rect = new Rectangle(new Point(0, 0), pbImage.Size);
            var bs = ButtonBorderStyle.Solid;
            ControlPaint.DrawBorder(e.Graphics, rect,
                color, width, bs, color, width, bs,
                color, width, bs, color, width, bs);
        }
    }
}
