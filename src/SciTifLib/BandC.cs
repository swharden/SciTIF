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
    public partial class BandC : UserControl
    {

        public class DisplayLimits
        {
            public int limitMin { get; private set; }
            public int limitMax { get; private set; }
            public int min { get; private set; }
            public int max { get; private set; }

            public DisplayLimits(int limitMax = 255, int limitMin = 0)
            {
                this.limitMax = limitMax;
                this.limitMin = limitMin;
                max = limitMax;
                min = limitMin;
            }

            public void SetMin(int min)
            {
                this.min = min;
            }

            public void SetMax(int max)
            {
                this.max = max;
            }

        }

        DisplayLimits dispLim;
        public BandC()
        {
            InitializeComponent();
            dispLim = new DisplayLimits();
            UpdateGuiFromDisplayLimits();
        }

        public void UpdateGuiFromDisplayLimits()
        {
            // minimum
            nudMin.Maximum = dispLim.limitMax;
            nudMin.Minimum = dispLim.limitMin;
            nudMin.Value = dispLim.min;
            scrollMin.Maximum = dispLim.limitMax;
            scrollMin.Minimum = dispLim.limitMin;
            scrollMin.Value = dispLim.min;

            // maximum
            nudMax.Maximum = dispLim.limitMax;
            nudMax.Minimum = dispLim.limitMin;
            nudMax.Value = dispLim.max;
            scrollMax.Maximum = dispLim.limitMax;
            scrollMax.Minimum = dispLim.limitMin;
            scrollMax.Value = dispLim.max;

        }

        private void nudMin_ValueChanged(object sender, EventArgs e)
        {
            dispLim.SetMin((int)nudMin.Value);
            UpdateGuiFromDisplayLimits();
        }

        private void scrollMin_Scroll(object sender, ScrollEventArgs e)
        {
            dispLim.SetMin(scrollMin.Value);
            UpdateGuiFromDisplayLimits();
        }

        private void nudMax_ValueChanged(object sender, EventArgs e)
        {
            dispLim.SetMax((int)nudMax.Value);
            UpdateGuiFromDisplayLimits();
        }

        private void scrollMax_Scroll(object sender, ScrollEventArgs e)
        {
            dispLim.SetMax(scrollMax.Value);
            UpdateGuiFromDisplayLimits();
        }

        private void btnMinFull_Click(object sender, EventArgs e)
        {
            dispLim.SetMin(dispLim.limitMin);
        }

        private void btnMaxFull_Click(object sender, EventArgs e)
        {
            dispLim.SetMax(dispLim.limitMax);
        }
    }
}
