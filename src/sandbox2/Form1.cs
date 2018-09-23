﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sandbox2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Frames imgFrames;
        private void Form1_Load(object sender, EventArgs e)
        {
            imgFrames = new Frames(@"D:\demoData\tifs\simple\rgb-test2.jpg");
            
        }

        private void UpdateImageFromCheckboxes()
        {
        }

        private void checkRed_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = imgFrames.CreateMerged();
            UpdateImageFromCheckboxes();
        }

        private void checkGreen_CheckedChanged(object sender, EventArgs e)
        {
            //pictureBox1.Image = imgFrames.frames[1].Render();
            UpdateImageFromCheckboxes();
        }

        private void checkBlue_CheckedChanged(object sender, EventArgs e)
        {
            //pictureBox1.Image = imgFrames.frames[2].Render();
            UpdateImageFromCheckboxes();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
