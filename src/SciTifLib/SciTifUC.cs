using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SciTIFlib
{
    public partial class SciTifUC : UserControl
    {

        private ImageFile sciTifImage;

        private bool stretchImageToFitPanel = true;
        private Color backgroundColor;
        private Color borderColor;
        private int borderWidth;

        ///////////////////////////////////////////////////////////////////////
        // LOADING

        public SciTifUC()
        {
            InitializeComponent();
            ResizeImageToFitPanel();
            SetBackgroundColor(SystemColors.ControlDarkDark);
            ConsoleOff();
        }

        ///////////////////////////////////////////////////////////////////////
        // EXTERNAL SETTINGS

        public void SetImage(string imageFilePath)
        {
            sciTifImage = new ImageFile(imageFilePath);

            // for testing
            //picture.Dock = DockStyle.Fill;
            //picture.BackColor = Color.Blue;

            ResizeImageToFitPanel();
            MouseBrightnessContrastReset();
            sciTifImage.imageDisplay.SetMinAndMaxAuto();
            picture.BackgroundImage = sciTifImage.GetBitmap();
            richTextBox1.Text = sciTifImage.log.logText;
            picture.Focus();
        }

        ///////////////////////////////////////////////////////////////////////
        // INTERNAL SETTINGS

        private void SetBackgroundColor(Color backgroundColor)
        {
            this.backgroundColor = backgroundColor;
            panelFrame.BackColor = backgroundColor;
            picture.BackColor = backgroundColor;
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
            richTextBox1.Text += String.Format("[{0:0.000}] {1}\n", timestamp, msg);
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.ScrollToCaret();
        }

        ///////////////////////////////////////////////////////////////////////
        // PICTURE POSITION AND ZOOMING

        public void SetZoomFit(bool fitImageToFrame = false)
        {
            if (fitImageToFrame)
            {
                stretchImageToFitPanel = true;
            }
            else
            {
                stretchImageToFitPanel = false;
                picture.Location = new Point(0, 0);
            }
            ResizeImageToFitPanel();
        }

        public void ToggleZoomFit()
        {
            if (stretchImageToFitPanel)
                SetZoomFit(false);
            else
                SetZoomFit(true);
            ResizeImageToFitPanel();
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
            picture.BackgroundImageLayout = ImageLayout.None;
            picture.Size = new Size(sciTifImage.imageWidth, sciTifImage.imageHeight);

            // center as needed horizontally
            int centerX = panelFrame.Width / 2 - sciTifImage.imageWidth / 2;
            picture.Location = new Point(centerX, picture.Location.Y);

            // center as needed vertically
            int centerY = panelFrame.Height / 2 - sciTifImage.imageHeight / 2;
            picture.Location = new Point(picture.Location.X, centerY);
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
                Debug($"left mouse down at ({mouseImgPos.X}, {mouseImgPos.Y})");
            }

            if (e.Button == MouseButtons.Right)
            {
                mouseDownR = Cursor.Position;
                Debug($"right mouse down at ({mouseDownR.X}, {mouseDownR.Y})");
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
                if (mouseDownTime < 200 || (dX==0 && dY==0))
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
            splitContainer.Panel2Collapsed = true;
            ResizeImageToFitPanel();
        }

        private void ConsoleOn()
        {
            Debug("showing developer console");
            developerConsoleToolStripMenuItem.Checked = true;
            splitContainer.Panel2Collapsed = false;
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Debug("Keypress: " + keyData.ToString());

            if (keyData == (Keys.Control | Keys.D))
            {
                Debug("Detected: Ctrl+D - toggling debug console");
                ConsoleToggle();
            }

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

        private void picture_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            //Render rndr = new Render();
            //Bitmap overlay = rndr.GenerateHistogram(picture.Width, picture.Height);
            //e.Graphics.DrawImage(overlay, 0, 0);

            if (sciTifImage == null || sciTifImage.imageDisplay == null)
                return;

            // add contrast text
            Font font = new Font(FontFamily.GenericMonospace, 8, FontStyle.Regular);
            SolidBrush brush = new SolidBrush(Color.Yellow);
            SolidBrush brushShadow = new SolidBrush(Color.Black);
            int contrastMin = (int)(sciTifImage.imageDisplay.displayMin + sciTifImage.imageDisplay.displayMinDelta);
            int contrastMax = (int)(sciTifImage.imageDisplay.displayMax + sciTifImage.imageDisplay.displayMaxDelta);
            gfx.DrawString($"Contrast: {contrastMin} - {contrastMax}", font, brushShadow, new Point(8, 8));
            gfx.DrawString($"Contrast: {contrastMin} - {contrastMax}", font, brush, new Point(7, 7));

        }
    }
}
