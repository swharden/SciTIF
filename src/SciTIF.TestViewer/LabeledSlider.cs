using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SciTIF.TestViewer
{
    public partial class LabeledSlider : UserControl
    {
        [Category("Appearance")]
        public string SliderLabel
        {
            get => _sliderLabel;
            set { _sliderLabel = value; UpdateText(); }
        }
        private string _sliderLabel = "Slider";

        public event EventHandler<int> ValueChanged = delegate { };

        public LabeledSlider()
        {
            InitializeComponent();

            SetSize(1);
            UpdateText();

            hScrollBar1.ValueChanged += (s, e) =>
            {
                UpdateText();
                ValueChanged?.Invoke(this, hScrollBar1.Value);
            };
        }

        private void UpdateText()
        {
            label1.Text = $"{SliderLabel} {hScrollBar1.Value + 1}/{hScrollBar1.Maximum + 1}";
        }

        public void SetValue(int position)
        {
            hScrollBar1.Value = position;
            UpdateText();
        }

        public void SetSize(int max)
        {
            hScrollBar1.Value = 0;
            hScrollBar1.Maximum = max - 1;
            UpdateText();
        }

        public int GetValue() => hScrollBar1.Value;
    }
}
