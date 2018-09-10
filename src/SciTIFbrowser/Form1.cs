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
        public Form1()
        {
            InitializeComponent();
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
            lblStatus.Text = $"Loading {filePath}";
            tif = new SciTIFlib.TifFile(filePath);
            pictureBox1.Image = tif.GetBitmapForDisplay();
        }

        public void LoadAdjacentImage(bool reverse = false)
        {
            string currentBasename = System.IO.Path.GetFileName(tif.filePath);
            string imageFolder = System.IO.Path.GetDirectoryName(tif.filePath);
            string[] imageFiles = System.IO.Directory.GetFiles(imageFolder);
            Array.Sort(imageFiles);

            // not multiple files in this folder so dont do anything
            if (imageFiles.Length < 2)
                return;

            // step through all the rest
            string imageFirst = imageFiles[0];
            string imageLast = imageFiles[imageFiles.Length - 1];

            string imageNext;
            string imagePrevious;

            for (int i = 0; i < imageFiles.Length; i++)
            {
                string maybeBasename = System.IO.Path.GetFileName(imageFiles[i]);
                System.Console.WriteLine($"{currentBasename.ToUpper()} {maybeBasename.ToUpper()}");
                if (currentBasename.ToUpper() != maybeBasename.ToUpper())
                    continue;

                if (i == 0)
                   imagePrevious = imageLast;
                else
                    imagePrevious = imageFiles[i - 1];

                if (i == imageFiles.Length - 1)
                    imageNext = imageFirst;
                else
                    imageNext = imageFiles[i + 1];

                if (reverse)
                    LoadImage(imagePrevious);
                else
                    LoadImage(imageNext);
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            System.Console.WriteLine("KEYPRESS");
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Right:
                    LoadAdjacentImage();
                    break;
                case Keys.Up:
                case Keys.Left:
                    LoadAdjacentImage(true);
                    break;
            }
        }

        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Black;
        }
    }
}
