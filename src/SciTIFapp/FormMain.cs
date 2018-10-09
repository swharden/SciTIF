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
    public partial class FormMain : Form
    {
        public string imageFilePath;
        public string imageFolderPath;
        public List<string> imageFiles = new List<string>();
        public readonly string version = Properties.Resources.ResourceManager.GetString("version");

        bool stretchImageToFitWindow = true;

        // ######################################################################
        // STARTUP BEHAVIOR

        public FormMain()
        {
            InitializeComponent();

            // prepare GUI settings
            pbImage.BackColor = SystemColors.Control;
            splitContainer1.Panel1Collapsed = true;
            splitContainer2.Panel2Collapsed = true;

            // do important stuff
            Log($"SciTIF {version}");
            CheckCommandLineArguments();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void CheckCommandLineArguments()
        {
            string[] args = Environment.GetCommandLineArgs();
            string argsString = "";
            for (int i = 0; i < args.Length; i++)
                argsString += $"\n  [{i}] \"{args[i]}\"";
            Log($"Command line arguments: {argsString}");

            // the first argument is the path of the file to load
            if (args.Length >= 2)
                SetImage(args[1]);
        }

        // ######################################################################
        // CORE METHODS

        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        double lastCommandTime;
        public void Log(string message, bool breakBefore = true)
        {
            Console.WriteLine(message);

            // format this log line
            double timeSec = (double)stopwatch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
            message = string.Format("[{0:000.000}] {1}", timeSec, message);

            // add a line break by default
            if (breakBefore && rtbConsole.Text.Length > 0)
                message = "\n" + message;

            // add another line break if it's the first line in a while
            double timeDiffFromLastCommand = Math.Abs(timeSec - lastCommandTime);
            lastCommandTime = timeSec;
            if (timeDiffFromLastCommand > .25 && rtbConsole.Text.Length > 0)
                message = "\n" + message;

            // update the text area
            rtbConsole.Text += message;

            // place the caret at the end
            rtbConsole.SelectionStart = rtbConsole.Text.Length;

            // scroll to the caret
            rtbConsole.ScrollToCaret(); 
        }

        public void ScanFolder(string folderPath)
        {
            this.imageFolderPath = folderPath;
            Log($"scanning folder: {folderPath}");

            // scan the folder for image files
            imageFiles.Clear();
            string[] imageExtensions = { ".TIF", ".TIFF", ".JPG", ".JPEG", ".PNG", ".BMP" };
            foreach (string filename in System.IO.Directory.GetFiles(folderPath, "*.*"))
            {
                string filePath = System.IO.Path.Combine(folderPath, filename);
                string imageExtension = System.IO.Path.GetExtension(filePath);
                if (imageExtensions.Contains(imageExtension.ToUpper()))
                    imageFiles.Add(filePath);
            }

            // update the listbox of files
            lbFiles.Items.Clear();
            foreach (string filePath in imageFiles)
                lbFiles.Items.Add(System.IO.Path.GetFileName(filePath));
        }

        public void SetImage(string imageFilePath)
        {
            // prevent reloading the same image
            if (this.imageFilePath == imageFilePath)
            {
                Log($"Image already loaded!");
            }
            else
            {
                Log($"Loading image: {imageFilePath}");
                System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
                SimpleImageLoader img = new SimpleImageLoader(imageFilePath);
                pbImage.Image = img.bmpPreview;
                double timeMS = stopwatch.ElapsedTicks * 1000.0 / System.Diagnostics.Stopwatch.Frequency;
                System.Console.WriteLine(string.Format("Image loaded in {0:0.00} ms", timeMS));
                pbImage.Width = img.bmpPreview.Width;
                pbImage.Height = img.bmpPreview.Height;
            }

            this.imageFilePath = imageFilePath;
            string imageFilename = System.IO.Path.GetFileName(imageFilePath);

            // set picturebox size based on whether or not it should be stretched
            if (stretchImageToFitWindow)
            {
                pbImage.Dock = DockStyle.Fill;
                panelImage.AutoScroll = false;
            }
            else
            {
                pbImage.Dock = DockStyle.None;
                panelImage.AutoScroll = true;
            }

            // update the GUI to reflect the new image file name
            this.Text = $"SciTIF - {imageFilename}";

            // if this image is in a different folder, scan the new folder to aid navigation
            string thisImageFolder = System.IO.Path.GetDirectoryName(imageFilePath);
            if (this.imageFolderPath != thisImageFolder)
                ScanFolder(thisImageFolder);

            // ensure the listbox selection is accurate
            lbFiles.SelectedIndex = imageFiles.IndexOf(imageFilePath);
        }

        public void NavigateNext()
        {
            Log("Next image...");
            if (imageFiles.Count == 0)
                return;
            int thisIndex = imageFiles.IndexOf(imageFilePath);
            if (thisIndex == imageFiles.Count - 1)
                SetImage(imageFiles[0]);
            else
                SetImage(imageFiles[thisIndex + 1]);
        }

        public void NavigatePrevious()
        {
            Log("Previous image...");
            if (imageFiles.Count == 0)
                return;
            int thisIndex = imageFiles.IndexOf(imageFilePath);
            if (thisIndex == 0)
                SetImage(imageFiles[imageFiles.Count - 1]);
            else
                SetImage(imageFiles[thisIndex - 1]);
        }

        // ######################################################################
        // KEY CAPTURE ACTIONS (forward to GUI actions)

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            Log($"Keypress: {e.KeyCode}");
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up)
                nextToolStripMenuItem_Click(null, null);
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down)
                previousToolStripMenuItem_Click(null, null);
            else if (e.KeyCode == Keys.C)
                developerConsoleToolStripMenuItem_Click(null, null);
            else if (e.KeyCode == Keys.N)
                navigatorToolStripMenuItem_Click(null, null);
            else if (ModifierKeys == Keys.Control && e.KeyCode == Keys.W)
                exitToolStripMenuItem_Click(null, null);
            else if (ModifierKeys == Keys.Control && e.KeyCode == Keys.O)
                openToolStripMenuItem_Click(null, null);
            else if (ModifierKeys == Keys.Control && e.KeyCode == Keys.S)
                saveAsToolStripMenuItem_Click(null, null);
            else if (e.KeyCode == Keys.F5)
                rescanFolderToolStripMenuItem_Click(null, null);
            else
                Log("Keypress was not handled.");

            // don't let other objects process this command
            e.Handled = true;
        }

        // ######################################################################
        // GUI ACTIONS



        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void sciTIFProjectPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/swharden/SciTIF");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout frm = new FormAbout();
            frm.ShowDialog();
        }

        private void developerConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //formConsole.Visible = true;
            //formConsole.BringToFront();
            toggleDeveloperConsole();
        }

        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbImage.BackColor = Color.Black;
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbImage.BackColor = Color.White;
        }

        private void blueDarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbImage.BackColor = Color.Navy;
        }

        private void grayDarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbImage.BackColor = Color.DarkGray;
        }

        private void grayLightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbImage.BackColor = Color.LightGray;
        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbImage.BackColor = SystemColors.Control;
        }

        private void magentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbImage.BackColor = Color.Magenta;
        }

        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFilename = lbFiles.SelectedItem.ToString();
            string selectedFilePath = System.IO.Path.Combine(imageFolderPath, selectedFilename);
            Log($"listbox selected: {selectedFilePath}");
            SetImage(selectedFilePath);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "TIF files (*.tif, *.tiff)|*.tif;*.tiff";
            diag.Filter += "|JPG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg";
            diag.Filter += "|PNG Files (*.png)|*.png;*.png";
            diag.Filter += "|BMP Files (*.bmp)|*.bmp;*.bmp";
            diag.Filter += "|All files (*.*)|*.*";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                SetImage(diag.FileName);
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void navigatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = System.IO.Path.GetFileNameWithoutExtension(imageFilePath) + "_modified.jpg";
            savefile.Filter = "JPG Files (*.jpg)|*.jpg";
            savefile.Filter += "|PNG files (*.png)|*.png";
            savefile.Filter += "|TIF files (*.tif)|*.tif";
            savefile.Filter += "|All files (*.*)|*.*";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                Log($"Saving: {savefile.FileName}");
            }
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigateNext();
        }

        private void previousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigatePrevious();
        }

        private void rescanFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScanFolder(imageFolderPath);
        }

        private void pbImage_Click(object sender, EventArgs e)
        {

        }

        public void toggleStretchImageToFitWindow()
        {
            Log("Toggling image stretch mode");
            stretchImageToFitWindow = !stretchImageToFitWindow;
            fitImageToWindowToolStripMenuItem.Checked = stretchImageToFitWindow;
            SetImage(imageFilePath);
        }

        private void fitImageToWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleStretchImageToFitWindow();
        }

        private void pbImage_DoubleClick(object sender, EventArgs e)
        {
            toggleStretchImageToFitWindow();
        }

        private void toggleDeveloperConsole()
        {
            splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;
        }
    }
}
