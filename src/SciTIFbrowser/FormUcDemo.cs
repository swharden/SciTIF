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
    public partial class FormUcDemo : Form
    {
        public FormUcDemo()
        {
            InitializeComponent();
        }

        private void FormUcDemo_Load(object sender, EventArgs e)
        {
            splitDirView1.SetFolder(@"D:\demoData\tifs");
            SetImage(@"D:\demoData\tifs\color-squares-indexed.tif");
            //SetImage(@"D:\demoData\tifs\16923029b-after.tif");
            //sciTifUC1.SetZoomFit(false);
            //sciTifUC1.SetBorder(Color.Yellow);
        }

        private void splitDirView1_FileHighlighted(object sender, EventArgs e)
        {
            SetImage(splitDirView1.highlightedFile);
        }

        private void SetImage(string imageFilePath)
        {
            sciTifUC1.SetImage(imageFilePath);
        }
    }
}
