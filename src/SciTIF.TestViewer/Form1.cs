using BitMiracle.LibTiff.Classic;
using System.Diagnostics;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;

namespace SciTIF.TestViewer
{
    public partial class Form1 : Form
    {
        private TifFile? LoadedTif;

        public Form1()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            btnPrev.Enabled = false;
            btnNext.Enabled = false;
            btnPrev.Click += BtnPrev_Click;
            btnNext.Click += BtnNext_Click;

            string initialImage = @"../../../../../data/images/baboon RGB.tif";
            LoadImage(initialImage);

            sliderFrame.ValueChanged += (s, e) => UpdateImage();
            sliderSlice.ValueChanged += (s, e) => UpdateImage();
            sliderChannel.ValueChanged += (s, e) => UpdateImage();
            cbAutoScale.CheckedChanged += (s, e) => ReloadImage();
            cbRGB.CheckedChanged += (s, e) => UpdateImage();
            cbMaxProjection.CheckedChanged += (s, e) => UpdateImage();
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
            if (LoadedTif is null)
                return;

            string[] files = Directory
                .GetFiles(Path.GetDirectoryName(LoadedTif.FilePath)!)
                .Where(x => Path.GetExtension(x).Contains(".tif"))
                .ToArray();

            int i = Array.IndexOf(files, LoadedTif.FilePath);
            int next = i < files.Length - 1 ? i + 1 : 0;
            LoadImage(files[next]);
        }

        private void BtnPrev_Click(object? sender, EventArgs e)
        {
            if (LoadedTif is null)
                return;

            string[] files = Directory
                .GetFiles(Path.GetDirectoryName(LoadedTif.FilePath)!)
                .Where(x => Path.GetExtension(x).Contains(".tif"))
                .ToArray();

            int i = Array.IndexOf(files, LoadedTif.FilePath);
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

            LoadedTif = new TifFile(imageFilePath);
            richTextBox1.Text = Path.GetFileName(LoadedTif.FilePath)
                + Environment.NewLine
                + LoadedTif.Description;

            sliderFrame.SetSize(LoadedTif.Frames);
            sliderSlice.SetSize(LoadedTif.Slices);
            sliderChannel.SetSize(LoadedTif.Channels);

            cbRGB.Visible = LoadedTif.Channels == 4;

            UpdateImage();
        }

        private void ReloadImage()
        {
            if (LoadedTif is null)
                return;

            LoadImage(LoadedTif.FilePath);
        }

        private void UpdateImage()
        {
            if (LoadedTif is null)
                return;

            int frame = sliderFrame.GetValue();
            int slice = sliderSlice.GetValue();
            int channel = sliderChannel.GetValue();

            bool isRgb = LoadedTif.Channels == 4 && cbRGB.Checked;

            cbMaxProjection.Visible = LoadedTif.Slices > 1;
            sliderFrame.Visible = LoadedTif.Frames > 1;
            sliderSlice.Visible = LoadedTif.Slices > 1 && !cbMaxProjection.Checked;
            sliderChannel.Visible = LoadedTif.Channels > 1 && !isRgb;

            var oldBmp = pictureBox1.Image;

            if (isRgb)
            {
                pictureBox1.Image = GetRgbBitmap(LoadedTif, frame, slice, channel);
            }
            else if (LoadedTif.Slices > 1 && cbMaxProjection.Checked)
            {
                ImageStack stack = LoadedTif.GetImageStack();
                Image img = stack.ProjectMax();
                if (cbAutoScale.Checked)
                    img.AutoScale();
                pictureBox1.Image = img.GetBitmap();
            }
            else
            {
                pictureBox1.Image = GetGrayscaleBitmap(LoadedTif, frame, slice, channel, cbAutoScale.Checked);
            }

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
            return IO.SystemDrawing.GetBitmap(r, g, b);
        }

        private void cbStretch_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = cbZoom.Checked
                ? PictureBoxSizeMode.Zoom
                : PictureBoxSizeMode.Normal;
        }
    }
}