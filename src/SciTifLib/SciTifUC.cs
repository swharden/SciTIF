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

        ///////////////////////////////////////////////////////////////////////
        // LOADING

        public SciTifUC()
        {
            InitializeComponent();
            SetBackgroundColor(Color.Black);
        }

        ///////////////////////////////////////////////////////////////////////
        // EXTERNAL COMMAND

        public void SetImage(string imageFilePath)
        {
            tif = new TifFile(imageFilePath);
            picture.BackgroundImage = tif.GetBitmapForDisplay();
            UpdateImageToFitNewPanelSize();
        }

        public void SetBackgroundColor(Color bgColor)
        {
            panelFrame.BackColor = bgColor;
            picture.BackColor = bgColor;
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

        private void UpdateImageToFitNewPanelSize()
        {
            if (tif == null)
                return;

            if (stretchImageToFitPanel)
                ResizeImageToFitPanel();
            else
                CenterImageInPanel();
        }


        ///////////////////////////////////////////////////////////////////////
        // EVENT BINDINGS

        private void SciTifUC_Resize(object sender, EventArgs e)
        {
            UpdateImageToFitNewPanelSize();
        }

        private void picture_DoubleClick(object sender, EventArgs e)
        {
            ToggleZoomFit();
        }

        private void picture_Click(object sender, EventArgs e)
        {

        }

    }
}
