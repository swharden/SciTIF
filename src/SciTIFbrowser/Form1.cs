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
        SciTIFlib.Path SciTifPath;
        public Form1()
        {
            InitializeComponent();
            SciTifPath = new SciTIFlib.Path();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string demoFile = @"D:\demoData\tifs\proj.tif";
            LoadImage(demoFile);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (pictureBox1.SizeMode == PictureBoxSizeMode.Zoom)
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            else
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "TIF Files (*.tif)|*.tif|All files (*.*)|*.*";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                /* do something */
            }
        }

        SciTIFlib.TifFile tif;
        public void LoadImage(string filePath)
        {
            lblStatus.Text = $"Loading image...";
            tif = new SciTIFlib.TifFile(filePath);
            pictureBox1.Image = tif.GetBitmapForDisplay();
            lblStatus.Text = $"{filePath}";
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
            {
                LoadImage(ImageToLoad);
            }
            else
            {
                lblStatus.Text = ("no adjacent images");
            }

        }
    }
}
