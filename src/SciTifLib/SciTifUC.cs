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

        private TifFile tif;

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
            Update();
        }

        ///////////////////////////////////////////////////////////////////////
        // EXTERNAL SETTINGS

        public void SetImage(string imageFilePath)
        {
            try
            {
                tif = new TifFile(imageFilePath);
                picture.BackgroundImage = tif.GetBitmap();
                ResizeImageToFitPanel();
                MouseBCreset();
                richTextBox1.Text = tif.Info();
            }
            catch
            {
                richTextBox1.Text = $"could not display: {imageFilePath}";
                picture.BackgroundImage = null;
            }
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
            double ratioX = (double)panelFrame.Width / tif.imageWidth;
            double ratioY = (double)panelFrame.Height / tif.imageHeight;
            if (ratioX > ratioY)
            {
                // match the HEIGHTS and center it HORIZONTALLY
                int imageWidth = (int)(panelFrame.Height * ((double)tif.imageWidth / tif.imageHeight));
                picture.Size = new Size(imageWidth, panelFrame.Height);
                picture.Location = new Point((panelFrame.Width - imageWidth) / 2, 0);
            }
            else
            {
                // match the WIDTHS and center it VERTICALLY
                int imageHeight = (int)(panelFrame.Width * ((double)tif.imageHeight / tif.imageWidth));
                picture.Size = new Size(panelFrame.Width, imageHeight);
                picture.Location = new Point(0, (panelFrame.Height - imageHeight) / 2);
            }
        }

        private void CenterImage()
        {
            // 1:1 image display
            picture.BackgroundImageLayout = ImageLayout.None;
            picture.Size = tif.imageSize;

            // center as needed horizontally
            int centerX = panelFrame.Width / 2 - tif.imageWidth / 2;
            picture.Location = new Point(centerX, picture.Location.Y);

            // center as needed vertically
            int centerY = panelFrame.Height / 2 - tif.imageHeight / 2;
            picture.Location = new Point(picture.Location.X, centerY);
        }

        private void ResizeImageToFitPanel()
        {
            if (tif == null)
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
            }

            if (e.Button == MouseButtons.Right)
            {
                mouseDownR = Cursor.Position;
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
                if (mouseDownTime < 200 || (dX==0 && dY==0))
                {
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
                panelFrame.VerticalScroll.Value = posY;
                panelFrame.HorizontalScroll.Value = posX;
            }

            if (e.Button == MouseButtons.Right)
            {
                int dX = Cursor.Position.X - mouseDownR.X;
                int dY = Cursor.Position.Y - mouseDownR.Y;
                int dXsum = mouseBC.X + dX;
                int dYsum = mouseBC.Y + dY;
                tif.imageDisplay.SetMinMaxMouse(dXsum, dYsum);
                MouseBCupdate();
            }
        }

        public void MouseBCreset()
        {
            mouseBC = new Point(0, 0);
            if (tif != null && tif.imageDisplay != null)
                tif.imageDisplay.SetMinMaxMouse(0, 0);
        }

        private bool mouseBCisWorking = false;
        private void MouseBCupdate(bool skipIfBusy = true)
        {
            if (skipIfBusy && mouseBCisWorking)
                return;
            mouseBCisWorking = true;
            picture.BackgroundImage = tif.GetBitmap();
            Application.DoEvents();
            mouseBCisWorking = false;
        }

        ///////////////////////////////////////////////////////////////////////
        // EVENT BINDINGS

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

    }
}
