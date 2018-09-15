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

        TifFile tif;

        private bool stretchImageToFitPanel = true;
        private Color backgroundColor;
        private Color borderColor;
        private int borderWidth;

        ///////////////////////////////////////////////////////////////////////
        // LOADING

        public SciTifUC()
        {
            InitializeComponent();
            UpdateImageToFitNewPanelSize();
            SetBackgroundColor(SystemColors.ControlDarkDark);
            Update();
        }

        ///////////////////////////////////////////////////////////////////////
        // EXTERNAL COMMANDS

        public void SetImage(string imageFilePath)
        {
            tif = new TifFile(imageFilePath);
            picture.BackgroundImage = tif.GetBitmapForDisplay();
            UpdateImageToFitNewPanelSize();
        }

        public void SetBackgroundColor(Color backgroundColor)
        {
            this.backgroundColor = backgroundColor;
            panelFrame.BackColor = backgroundColor;
            picture.BackColor = backgroundColor;
        }

        public void SetBorder(Color borderColor, int borderWidth = 1)
        {
            this.borderColor = borderColor;
            this.borderWidth = borderWidth;
            OutlineImageOnPanel();
        }

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
            UpdateImageToFitNewPanelSize();
        }

        public void ToggleZoomFit()
        {
            if (stretchImageToFitPanel)
                SetZoomFit(false);
            else
                SetZoomFit(true);
            UpdateImageToFitNewPanelSize();
        }

        ///////////////////////////////////////////////////////////////////////
        // INTERNAL FUNCTIONS

        private void ResizeImageToFitPanel()
        {
            // stretcy display, so scale to the most contrained axis
            picture.BackgroundImageLayout = ImageLayout.Stretch;
            double ratioX = (double)panelFrame.Width / tif.width;
            double ratioY = (double)panelFrame.Height / tif.height;
            if (ratioX > ratioY)
            {
                // match the HEIGHTS and center it HORIZONTALLY
                int imageWidth = (int)(panelFrame.Height * ((double)tif.width / tif.height));
                picture.Size = new Size(imageWidth, panelFrame.Height);
                picture.Location = new Point((panelFrame.Width - imageWidth) / 2, 0);
            }
            else
            {
                // match the WIDTHS and center it VERTICALLY
                int imageHeight = (int)(panelFrame.Width * ((double)tif.height / tif.width));
                picture.Size = new Size(panelFrame.Width, imageHeight);
                picture.Location = new Point(0, (panelFrame.Height - imageHeight) / 2);
            }
        }

        private void CenterImageInPanel()
        {
            // 1:1 image display
            picture.BackgroundImageLayout = ImageLayout.None;
            picture.Size = tif.size;

            // center as needed horizontally
            int centerX = panelFrame.Width / 2 - tif.width / 2;
            picture.Location = new Point(centerX, picture.Location.Y);

            // center as needed vertically
            int centerY = panelFrame.Height / 2 - tif.height / 2;
            picture.Location = new Point(picture.Location.X, centerY);
        }

        private void OutlineImageOnPanel()
        {
            if (borderWidth == 0)
                return;
            Graphics gfx = panelFrame.CreateGraphics();
            gfx.Clear(backgroundColor);
            Pen pen = new Pen(borderColor, borderWidth * 2);
            Rectangle rect = new Rectangle(picture.Location, picture.Size);
            gfx.DrawRectangle(pen, rect);
        }

        private void UpdateImageToFitNewPanelSize()
        {
            if (tif == null)
                return;

            if (stretchImageToFitPanel)
                ResizeImageToFitPanel();
            else
                CenterImageInPanel();

            OutlineImageOnPanel();
        }

        ///////////////////////////////////////////////////////////////////////
        // CLICK-AND-DRAG TRACKING


        ///////////////////////////////////////////////////////////////////////
        // EVENT BINDINGS

        private void SciTifUC_Resize(object sender, EventArgs e)
        {
            UpdateImageToFitNewPanelSize();
        }

        private void SciTifUC_SizeChanged(object sender, EventArgs e)
        {
            UpdateImageToFitNewPanelSize();
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
                UpdateImageToFitNewPanelSize();
                needsToBePaintedForTheFirstTime = false;
            }
        }

        private void SciTifUC_Load(object sender, EventArgs e)
        {

        }

        private void picture_Click(object sender, EventArgs e)
        {

        }

        private bool mouseDownLeft;
        private Point mouseDownLeftPoint;
        private Point mouseDownLeftPicturePosition;

        private void picture_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownLeft = true;
                mouseDownLeftPoint = Cursor.Position;
                Console.WriteLine($"mouse down: {mouseDownLeftPoint.X}, {mouseDownLeftPoint.Y}");
                mouseDownLeftPicturePosition = new Point(panelFrame.HorizontalScroll.Value,
                                                         panelFrame.VerticalScroll.Value);
            }
        }

        private void picture_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownLeft = false;
                mouseDownLeftPoint = Cursor.Position;
                Console.WriteLine($"mouse up: {mouseDownLeftPoint.X}, {mouseDownLeftPoint.Y}");
            }
        }

        private void picture_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownLeft)
            {
                int dX = Cursor.Position.X - mouseDownLeftPoint.X;
                int dY = -(Cursor.Position.Y - mouseDownLeftPoint.Y);
                Console.WriteLine($"mouse move: {dX}, {dY}");
                int posY = Math.Max(0, mouseDownLeftPicturePosition.Y + dY);
                int posX = Math.Max(0, mouseDownLeftPicturePosition.X - dX);
                panelFrame.VerticalScroll.Value = posY;
                panelFrame.HorizontalScroll.Value = posX;
                Application.DoEvents();
                this.Update();
            }
        }
    }
}
