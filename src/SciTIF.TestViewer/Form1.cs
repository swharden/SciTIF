using BitMiracle.LibTiff.Classic;
using System.Diagnostics;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;

namespace SciTIF.TestViewer
{
    public partial class Form1 : Form
    {
        private TifFile? CurrentTif;

        public Form1()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            btnPrev.Enabled = false;
            btnNext.Enabled = false;
            btnPrev.Click += BtnPrev_Click;
            btnNext.Click += BtnNext_Click;

            string initialImage = @"../../../../../data/images/LennaIndexed.tif";
            LoadImage(initialImage);

            sliderFrame.ValueChanged += (s, e) => UpdateImage();
            sliderSlice.ValueChanged += (s, e) => UpdateImage();
            sliderChannel.ValueChanged += (s, e) => UpdateImage();
            cbAutoScale.CheckedChanged += (s, e) => ReloadImage();
            cbRGB.CheckedChanged += (s, e) => UpdateImage();
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                BtnNext_Click(this, EventArgs.Empty);
            else if (e.KeyCode == Keys.Left)
                BtnPrev_Click(this, EventArgs.Empty);

            e.Handled = true;
        }

        private void BtnNext_Click(object? sender, EventArgs e)
        {
            if (CurrentTif is null)
                return;

            string[] files = Directory
                .GetFiles(Path.GetDirectoryName(CurrentTif.FilePath)!)
                .Where(x => Path.GetExtension(x).Contains(".tif"))
                .ToArray();

            int i = Array.IndexOf(files, CurrentTif.FilePath);
            int next = i < files.Length - 1 ? i + 1 : 0;
            LoadImage(files[next]);
        }

        private void BtnPrev_Click(object? sender, EventArgs e)
        {
            if (CurrentTif is null)
                return;

            string[] files = Directory
                .GetFiles(Path.GetDirectoryName(CurrentTif.FilePath)!)
                .Where(x => Path.GetExtension(x).Contains(".tif"))
                .ToArray();

            int i = Array.IndexOf(files, CurrentTif.FilePath);
            int previous = i > 0 ? i - 1 : files.Length - 1;
            LoadImage(files[previous]);
        }

        private void LoadImage(string imageFilePath)
        {
            imageFilePath = Path.GetFullPath(imageFilePath);
            if (!File.Exists(imageFilePath))
                throw new FileNotFoundException(imageFilePath);

            btnNext.Enabled = true;
            btnPrev.Enabled = true;

            CurrentTif = new TifFile(imageFilePath);
            richTextBox1.Text = Path.GetFileName(CurrentTif.FilePath)
                + Environment.NewLine
                + CurrentTif.Description;

            sliderFrame.SetSize(CurrentTif.Frames);
            sliderSlice.SetSize(CurrentTif.Slices);
            sliderChannel.SetSize(CurrentTif.Channels);

            cbRGB.Visible = CurrentTif.Channels == 4;

            UpdateImage();
        }

        private void ReloadImage()
        {
            if (CurrentTif is null)
                return;

            LoadImage(CurrentTif.FilePath);
        }

        private void UpdateImage()
        {
            if (CurrentTif is null)
                return;

            int frame = sliderFrame.GetValue();
            int slice = sliderSlice.GetValue();
            int channel = sliderChannel.GetValue();

            bool isRgb = CurrentTif.Channels == 4 && cbRGB.Checked;

            sliderFrame.Visible = CurrentTif.Frames > 1;
            sliderSlice.Visible = CurrentTif.Slices > 1;
            sliderChannel.Visible = CurrentTif.Channels > 1 && !isRgb;

            Bitmap newBmp = isRgb
                ? GetRgbBitmap(CurrentTif, frame, slice, channel)
                : GetGrayscaleBitmap(CurrentTif, frame, slice, channel, cbAutoScale.Checked);

            var oldBmp = pictureBox1.Image;
            pictureBox1.Image = newBmp;
            oldBmp?.Dispose();
        }

        private static Bitmap GetGrayscaleBitmap(Image5D tif, int frame, int slice, int channel, bool autoScale)
        {
            Image img = tif.GetImage(frame, slice, channel);
            if (autoScale)
                img.AutoScale();
            return img.GetBitmap();
        }

        private static Bitmap GetRgbBitmap(Image5D tif, int frame, int slice, int channel)
        {
            Image r = tif.GetImage(frame, slice, 0);
            Image g = tif.GetImage(frame, slice, 1);
            Image b = tif.GetImage(frame, slice, 2);
            Image a = tif.GetImage(frame, slice, 3);
            return IO.SystemDrawing.GetBitmapRGB(r, g, b);
        }

        private void cbStretch_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = cbZoom.Checked
                ? PictureBoxSizeMode.Zoom
                : PictureBoxSizeMode.Normal;
        }
    }
}