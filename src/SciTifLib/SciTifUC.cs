using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SciTIFlib
{
    public partial class SciTifUC : UserControl
    {

        private ImageFile sciTifImage;

        private bool stretchImageToFitPanel = true;
        private Color backgroundColor;
        private Color borderColor;
        private int borderWidth;
        private double imageZoom = 1;

        ///////////////////////////////////////////////////////////////////////
        // LOADING

        public SciTifUC()
        {
            InitializeComponent();
            ResizeImageToFitPanel();

            // set init settings
            SetBackgroundColor(SystemColors.ControlDarkDark);
            ConsoleOff();
            FileNavOff();
            ConsoleOff();
        }

        ///////////////////////////////////////////////////////////////////////
        // EXTERNAL SETTINGS

        public void SetImage(string imageFilePath)
        {
            FileNavUpdate(imageFilePath);
            FileNavSelect(System.IO.Path.GetFileName(imageFilePath));
            sciTifImage = new ImageFile(imageFilePath);
            ResizeImageToFitPanel();
            MouseBrightnessContrastReset();
            sciTifImage.imageDisplay?.SetMinAndMaxAuto();
            picture.BackgroundImage = sciTifImage.GetBitmap();
            richTextBox1.Text += "\n\n"+sciTifImage.log.logText;
            picture.Focus();
        }

        ///////////////////////////////////////////////////////////////////////
        // FILE NAVIGATION
        private string fileNavSelectedFolder;
        private string fileNavSelectedFile;
        private string[] fileNavSeenFiles;
        private int fileNavSelectedIndex = 0;

        /// <summary>
        /// update the list of adjacent images so fwd/back arrow keys work
        /// </summary>
        private void FileNavUpdate(string folder)
        {

            if (System.IO.File.Exists(folder))
            {
                Debug($"updating folder list for: {folder}");
                folder = System.IO.Path.GetDirectoryName(folder);
                Debug($"scanning: {folder}");
            }

            if (!System.IO.Directory.Exists(folder))
            {
                Debug($"folder does not exist: {folder}");
                return;
            }

            if (folder == fileNavSelectedFolder)
            {
                Debug("folder hasn't changed, not refreshing contents");
                return;
            }
            else
            {
                Debug($"scanning {folder}");
                fileNavSelectedFolder = folder;
            }

            fileNavSeenFiles = System.IO.Directory.GetFiles(folder);
            Debug($"Found {fileNavSeenFiles.Length} in this folder");

            listBox1.Items.Clear();
            foreach (string fname in fileNavSeenFiles)
                listBox1.Items.Add(System.IO.Path.GetFileName(fname));
        }

        /// <summary>
        /// manually define an image as selected
        /// </summary>
        private void FileNavSelect(string filename)
        {
            for (int i = 0; i < fileNavSeenFiles.Length; i++)
            {
                if (fileNavSeenFiles[i] == filename)
                {
                    fileNavSelectedFile = filename;
                    fileNavSelectedIndex = i;
                    listBox1.SelectedIndex = fileNavSelectedIndex;
                    Debug($"selected file: {filename}");
                    return;
                }
            }
            Debug($"could not file in list: {filename}");
            fileNavSelectedFile = null;
        }

        private void FileNavNext()
        {
            Debug("Go to next image");
            if (fileNavSelectedIndex == fileNavSeenFiles.Length - 1)
                fileNavSelectedIndex = 0;
            else
                fileNavSelectedIndex += 1;
            listBox1.SelectedIndex = fileNavSelectedIndex;
            SetImage(fileNavSeenFiles[fileNavSelectedIndex]);
        }

        private void FileNavPrevious()
        {
            Debug("Go to previous image");
            if (fileNavSelectedIndex == 0)
                fileNavSelectedIndex = fileNavSeenFiles.Length - 1;
            else
                fileNavSelectedIndex -= 1;
            listBox1.SelectedIndex = fileNavSelectedIndex;
            SetImage(fileNavSeenFiles[fileNavSelectedIndex]);
        }

        private void FileNavUpdateListbox()
        {
            
        }

        ///////////////////////////////////////////////////////////////////////
        // INTERNAL SETTINGS

        private void SetBackgroundColor(Color backgroundColor)
        {
            this.backgroundColor = backgroundColor;
            panelFrame.BackColor = backgroundColor;
            picture.BackColor = backgroundColor;
            tableFiles.BackColor = backgroundColor;
            splitContainerFilenavPic.BackColor = backgroundColor;
        }

        private void SetBorder(Color borderColor, int borderWidth = 1)
        {
            this.borderColor = borderColor;
            this.borderWidth = borderWidth;
        }

        private double dateTimeTicksAtLaunch = DateTime.Now.Ticks;
        private void Debug(string msg)
        {
            double tickSpan = DateTime.Now.Ticks - dateTimeTicksAtLaunch;
            double timestamp = tickSpan / TimeSpan.TicksPerSecond;
            timestamp = Math.Round(timestamp, 3);
            string line = String.Format("[{0:0.000}] {1}\n", timestamp, msg);
            Console.Write(line);
            richTextBox1.Text += line;
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.ScrollToCaret();
        }

        ///////////////////////////////////////////////////////////////////////
        // PICTURE POSITION AND ZOOMING

        public void SetZoomFit(bool fitImageToFrame = false)
        {
            if (fitImageToFrame)
            {
                Debug($"Enabling fit-to-window (stretch) zoom");
                stretchImageToFitPanel = true;
            }
            else
            {
                Debug($"Disabling fit-to-window (stretch) zoom");
                stretchImageToFitPanel = false;
                picture.Location = new Point(0, 0);
            }
            ResizeImageToFitPanel();
        }

        public void ToggleZoomFit()
        {
            Debug($"Toggling zoom fit");
            if (stretchImageToFitPanel)
                SetZoomFit(false);
            else
                SetZoomFit(true);
        }

        private void ZoomToFit()
        {
            // stretcy display, so scale to the most contrained axis
            picture.BackgroundImageLayout = ImageLayout.Stretch;
            double ratioX = (double)panelFrame.Width / sciTifImage.imageWidth;
            double ratioY = (double)panelFrame.Height / sciTifImage.imageHeight;
            if (ratioX > ratioY)
            {
                // match the HEIGHTS and center it HORIZONTALLY
                int imageWidth = (int)(panelFrame.Height * ((double)sciTifImage.imageWidth / sciTifImage.imageHeight));
                picture.Size = new Size(imageWidth, panelFrame.Height);
                picture.Location = new Point((panelFrame.Width - imageWidth) / 2, 0);
            }
            else
            {
                // match the WIDTHS and center it VERTICALLY
                int imageHeight = (int)(panelFrame.Width * ((double)sciTifImage.imageHeight / sciTifImage.imageWidth));
                picture.Size = new Size(panelFrame.Width, imageHeight);
                picture.Location = new Point(0, (panelFrame.Height - imageHeight) / 2);
            }
        }

        private void CenterImage()
        {
            // 1:1 image display
            picture.BackgroundImageLayout = ImageLayout.Zoom;
            picture.Size = new Size((int)(sciTifImage.imageWidth * imageZoom), (int)(sciTifImage.imageHeight * imageZoom));

            // center small pictures as needed horizontally
            if (panelFrame.Width > picture.Size.Width)
            {
                int centerX = panelFrame.Width / 2 - picture.Size.Width / 2;
                picture.Location = new Point(centerX, picture.Location.Y);
            }
            else
            {
                picture.Location = new Point(0, picture.Location.Y);
            }

            // center small pictures as needed vertically
            if (panelFrame.Height > picture.Size.Height)
            {
                int centerY = panelFrame.Height / 2 - picture.Size.Height / 2;
                picture.Location = new Point(picture.Location.X, centerY);
            }
            else
            {
                picture.Location = new Point(picture.Location.X, 0);
            }
        }

        private void ResizeImageToFitPanel()
        {
            if (sciTifImage == null)
                return;

            if (stretchImageToFitPanel)
                ZoomToFit();
            else
                CenterImage();
        }

        ///////////////////////////////////////////////////////////////////////
        // CLICK-AND-DRAG TRACKING FOR PAN AND BRIGHTNESS/CONTRAST

        private Point mouseDownL;
        private Point mouseDownR;
        private Point mouseImgPos;
        private Point mouseBC = new Point(0, 0);
        private double mouseDownTime;

        private void picture_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (e.Button == MouseButtons.Left)
            {
                mouseDownL = Cursor.Position;
                mouseImgPos = new Point(panelFrame.HorizontalScroll.Value, panelFrame.VerticalScroll.Value);
                //Debug($"left mouse down at ({mouseImgPos.X}, {mouseImgPos.Y})");
            }

            if (e.Button == MouseButtons.Right)
            {
                mouseDownR = Cursor.Position;
                //Debug($"right mouse down at ({mouseDownR.X}, {mouseDownR.Y})");
            }
        }

        private void picture_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - mouseDownTime;
            if (e.Button == MouseButtons.Right)
            {
                int dX = Cursor.Position.X - mouseDownR.X;
                int dY = Cursor.Position.Y - mouseDownR.Y;
                mouseBC = new Point(mouseBC.X + dX, mouseBC.Y + dY);
                Debug($"right-click-released, remembering last brightness and contrast: ({mouseBC.X}, {mouseBC.X})");
                if (mouseDownTime < 200 || (dX == 0 && dY == 0))
                {
                    Debug($"right button pressed and released quickly, so showing right-click menu");
                    contextMenuStrip1.Show(picture, new Point(e.X, e.Y));
                }
            }
        }

        private void picture_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dX = Cursor.Position.X - mouseDownL.X;
                int dY = -(Cursor.Position.Y - mouseDownL.Y);
                int posY = Math.Max(0, mouseImgPos.Y + dY);
                int posX = Math.Max(0, mouseImgPos.X - dX);
                posY = Math.Max(posY, 0);
                posY = Math.Min(posY, panelFrame.VerticalScroll.Maximum);
                posX = Math.Max(posX, 0);
                posX = Math.Min(posX, panelFrame.HorizontalScroll.Maximum);
                panelFrame.VerticalScroll.Value = posY;
                panelFrame.HorizontalScroll.Value = posX;
            }

            if (e.Button == MouseButtons.Right)
            {
                int dX = Cursor.Position.X - mouseDownR.X;
                int dY = Cursor.Position.Y - mouseDownR.Y;
                int dXsum = mouseBC.X + dX;
                int dYsum = mouseBC.Y + dY;
                if (sciTifImage != null && sciTifImage.imageDisplay != null)
                    sciTifImage.imageDisplay.SetMinMaxMouse(dXsum, dYsum);
                MouseBCupdate();
            }
        }

        public void MouseBrightnessContrastReset()
        {
            Debug($"Resetting previously-remembered mouse-configured brightness and contrast");
            mouseBC = new Point(0, 0);
            if (sciTifImage != null && sciTifImage.imageDisplay != null)
                sciTifImage.imageDisplay.SetMinMaxMouse(0, 0);
        }

        private bool mouseBCisWorking = false;
        private void MouseBCupdate(bool skipIfBusy = true)
        {
            if (skipIfBusy && mouseBCisWorking)
                return;
            mouseBCisWorking = true;
            picture.BackgroundImage = sciTifImage.GetBitmap();
            Application.DoEvents();
            mouseBCisWorking = false;
        }

        public void ZoomIn(double multiple = 2)
        {
            Debug($"Zooming IN by a multiple of {multiple}");
            ZoomSet(imageZoom * multiple);
        }
        public void ZoomOut(double multiple = 2)
        {
            Debug($"Zooming OUT by a multiple of {multiple}");
            ZoomSet(imageZoom / multiple);
        }
        public void ZoomSet(double zoomFraction = 1)
        {
            Debug($"Setting zoom to {zoomFraction * 100}%");
            double maxZoom = 16;
            double minZoom = 1.0 / 16.0;
            zoomFraction = Math.Min(zoomFraction, maxZoom);
            zoomFraction = Math.Max(zoomFraction, minZoom);
            imageZoom = zoomFraction;
            SetZoomFit(false);
        }

        ///////////////////////////////////////////////////////////////////////
        // EVENTS FUNCTIONS

        private void ConsoleToggle()
        {
            Debug("toggling developer console");
            if (developerConsoleToolStripMenuItem.Checked)
                ConsoleOff();
            else
                ConsoleOn();
        }

        private void ConsoleOff()
        {
            Debug("hiding developer console");
            developerConsoleToolStripMenuItem.Checked = false;
            splitContainerTopHoriz.Panel2Collapsed = true;
            ResizeImageToFitPanel();
        }

        private void ConsoleOn()
        {
            Debug("showing developer console");
            developerConsoleToolStripMenuItem.Checked = true;
            splitContainerTopHoriz.Panel2Collapsed = false;
            ResizeImageToFitPanel();
        }

        private void FileNavToggle()
        {
            Debug("toggling file navigation panel");
            if (showFileListFToolStripMenuItem.Checked)
                FileNavOff();
            else
                FileNavOn();
        }

        private void FileNavOff()
        {
            Debug("hiding file navigation panel");
            showFileListFToolStripMenuItem.Checked = false;
            splitContainerFilenavPic.Panel1Collapsed = true;
            ResizeImageToFitPanel();
        }

        private void FileNavOn()
        {
            Debug("showing file navigation panel");
            showFileListFToolStripMenuItem.Checked = true;
            splitContainerFilenavPic.Panel1Collapsed = false;
            ResizeImageToFitPanel();
        }

        ///////////////////////////////////////////////////////////////////////
        // EVENTS BINDINGS

        private void SciTifUC_Resize(object sender, EventArgs e)
        {
            ResizeImageToFitPanel();
        }

        private void SciTifUC_SizeChanged(object sender, EventArgs e)
        {
            ResizeImageToFitPanel();
        }

        private void picture_DoubleClick(object sender, EventArgs e)
        {
            ToggleZoomFit();
        }


        private bool needsToBePaintedForTheFirstTime = true;
        private void SciTifUC_Paint(object sender, PaintEventArgs e)
        {
            if (needsToBePaintedForTheFirstTime)
            {
                ResizeImageToFitPanel();
                needsToBePaintedForTheFirstTime = false;
            }
        }

        private void developerConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsoleToggle();
        }

        // THIS IS OUR KEY PRESS CAPTURE FUNCTION! ADD FEATURES HERE!
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Debug("Keypress: " + keyData.ToString());

            if (keyData == Keys.D)
                ConsoleToggle();

            if (keyData == Keys.F)
                FileNavToggle();

            if (keyData == Keys.Oemplus || keyData == Keys.Add)
                ZoomIn();

            if (keyData == Keys.OemMinus || keyData == Keys.Subtract)
                ZoomOut();

            if (keyData == Keys.D0 || keyData == Keys.NumPad0)
                ZoomSet(1);

            if (keyData == Keys.Right || keyData == Keys.Down)
                FileNavNext();

            if (keyData == Keys.Left || keyData == Keys.Up)
                FileNavPrevious();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void panelFrame_Paint(object sender, PaintEventArgs e)
        {
            if (!panelFrame.Focused)
            {
                panelFrame.Focus();
                Debug("Focusing on panel");
            }
        }

        private void picture_Click(object sender, EventArgs e)
        {
            if (!picture.Focused)
            {
                picture.Focus();
                Debug("Focusing on picture");
            }
        }

        public Bitmap Histogram(Size size, int[] values)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);

            Graphics gfx = Graphics.FromImage(bmp);
            gfx.Clear(Color.White);

            // bin the data into 1px columns
            double dataMin = values.Min();
            double dataMax = values.Max();
            double dataSpan = dataMax - dataMin;
            int nBins = size.Width;
            double[] counts = new double[nBins];
            double binSize = dataSpan / (nBins - 1);
            for (int i = 0; i < values.Length; i++)
            {
                int bin = (int)((values[i] - dataMin) / binSize);
                if (bin >= counts.Length)
                    bin = counts.Length - 1;
                if (bin < 0)
                    bin = 0;
                counts[bin] = counts[bin] + 1;
            }

            // determine what to normalize it to visually
            double peakVal = counts.Max();
            double heightMult = size.Height / peakVal;

            // plot the binned data
            Pen pen = new Pen(new SolidBrush(Color.Black));
            for (int i = 0; i < nBins; i++)
            {
                int heightPx = (int)(counts[i] * heightMult);
                Point pt1 = new Point(i, size.Height - 0);
                Point pt2 = new Point(i, size.Height - heightPx);
                gfx.DrawLine(pen, pt1, pt2);
            }

            return bmp;
        }

        private void picture_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;

            if (sciTifImage == null || sciTifImage.imageDisplay == null)
                return;

            // create message to display
            string msg = "";

            msg += $"Zoom: {imageZoom * 100}%";

            msg += $"\nDepth: {sciTifImage.imageDepth}-Bit ({Math.Pow(2, sciTifImage.imageDepth)})";

            int contrastMin = (int)(sciTifImage.imageDisplay.displayValueMin);
            int contrastMax = (int)(sciTifImage.imageDisplay.displayValueMax);
            msg += $"\nContrast: {contrastMin} - {contrastMax}";

            // draw the message
            Font font = new Font(FontFamily.GenericMonospace, 8, FontStyle.Regular);
            SolidBrush brush = new SolidBrush(Color.Yellow);
            SolidBrush brushShadow = new SolidBrush(Color.Black);
            int posX = 5;
            int posY = 5;
            gfx.DrawString(msg, font, brushShadow, new Point(posX + 1, posY + 1));
            gfx.DrawString(msg, font, brush, new Point(posY, posY));

            // draw a histogram bitmap
            Bitmap hist = Histogram(new Size(100, 40), sciTifImage.valuesRaw);
            gfx.DrawImage(hist, new Point(5, 50));

        }

        private void toggledoubleclickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleZoomFit();
        }

        private void fitstretchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetZoomFit(true);
        }

        private void originalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetZoomFit(false);
        }

        private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomSet(1);
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ResizeImageToFitPanel();
        }

        private void btnSetFolder_Click(object sender, EventArgs e)
        {
            var diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = diag.SelectedPath;
                FileNavUpdate(selectedPath);
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            panelFrame.Focus();
            Debug("Focusing on panel");
        }

        private void showFileListFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileNavToggle();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;
            Debug(">>>>>>> selected index changed");
            fileNavSelectedIndex = listBox1.SelectedIndex;
            SetImage(fileNavSeenFiles[fileNavSelectedIndex]);
        }
    }
}
