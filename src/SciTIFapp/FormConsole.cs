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
    public partial class FormConsole : Form
    {

        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        double lastCommandTime = 0;

        public FormConsole()
        {
            InitializeComponent();
            LogClear();
        }

        private void FormConsole_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = true; // cancel the close event
            this.Visible = false;
        }

        public void Log(string message, bool breakBefore = true)
        {
            // format this log line
            double timeSec = (double) stopwatch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
            message = string.Format("[{0:000.000}] {1}", timeSec, message);

            // add a line break by default
            if (breakBefore && richTextBox1.Text.Length>0)
                message = "\n" + message;

            // add another line break if it's the first line in a while
            double timeDiffFromLastCommand = Math.Abs(timeSec - lastCommandTime);
            lastCommandTime = timeSec;
            if (timeDiffFromLastCommand > .25 && richTextBox1.Text.Length > 0)
                message = "\n" + message;

            // update the text area
            richTextBox1.Text += message;            
        }

        public void LogClear()
        {
            richTextBox1.Text = "";
            stopwatch.Restart();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length; // place the caret
            richTextBox1.ScrollToCaret(); // scroll to the caret
        }
    }
}
