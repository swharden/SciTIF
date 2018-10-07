using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SciTIFapp
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            string version = Properties.Resources.ResourceManager.GetString("version");
            lblVersion.Text = version;
            pictureBox1.Focus();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://tech.swharden.com/");
        }

        private void rtbGitHub_MouseClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/swharden/SciTIF");
        }

        private void rtbAuthor_MouseClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start("https://tech.SWHarden.com");
        }

        private void rtbAuthor_Enter(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }

        private void rtbGitHub_Enter(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }
    }
}
