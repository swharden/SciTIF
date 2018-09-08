using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SciTIFlib;

namespace TifLibDemo
{
    public partial class Form1 : Form
    {
        public TifFile tif;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            splitDirView1.SetFont(8);

            string demoTifFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            demoTifFolder = System.IO.Path.GetFullPath(demoTifFolder + "/../../../../data/images/");
            splitDirView1.SetFolder(@"D:\demoData\tifs");
            //splitDirView1.SelectFile(4);
            splitDirView1.SelectFile(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void splitDirView1_FileHighlighted(object sender, EventArgs e)
        {
            string tifFilePath = splitDirView1.highlightedFile;
            lblFileName.Text = System.IO.Path.GetFileName(tifFilePath);
            tif = new TifFile(tifFilePath);
            rtbMeta.Text = tif.Info();
            rtbLog.Text = tif.log.logText;
        }
    }
}
