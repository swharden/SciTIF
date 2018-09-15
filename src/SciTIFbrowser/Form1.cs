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

    }
}
