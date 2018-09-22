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
            //splitDirView1.SetFolder(@"D:\demoData\tifs\simple");
            SetImage(@"D:\demoData\tifs\simple\rgb-test2.jpg");
        }

        private void splitDirView1_FileHighlighted(object sender, EventArgs e)
        {
            //SetImage(splitDirView1.highlightedFile);
        }

        private void SetImage(string imageFilePath)
        {
            sciTifUC1.SetImage(imageFilePath);
        }
    }
}
