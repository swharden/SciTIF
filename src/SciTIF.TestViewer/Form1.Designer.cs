namespace SciTIF.TestViewer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sliderFrame = new SciTIF.TestViewer.LabeledSlider();
            this.sliderSlice = new SciTIF.TestViewer.LabeledSlider();
            this.sliderChannel = new SciTIF.TestViewer.LabeledSlider();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbZoom = new System.Windows.Forms.CheckBox();
            this.lblFilename = new System.Windows.Forms.Label();
            this.cbAutoScale = new System.Windows.Forms.CheckBox();
            this.cbRGB = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // sliderFrame
            // 
            this.sliderFrame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderFrame.Location = new System.Drawing.Point(12, 507);
            this.sliderFrame.Name = "sliderFrame";
            this.sliderFrame.Size = new System.Drawing.Size(489, 41);
            this.sliderFrame.SliderLabel = "Frame";
            this.sliderFrame.TabIndex = 0;
            // 
            // sliderSlice
            // 
            this.sliderSlice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderSlice.Location = new System.Drawing.Point(12, 460);
            this.sliderSlice.Name = "sliderSlice";
            this.sliderSlice.Size = new System.Drawing.Size(489, 41);
            this.sliderSlice.SliderLabel = "Slice";
            this.sliderSlice.TabIndex = 1;
            // 
            // sliderChannel
            // 
            this.sliderChannel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderChannel.Location = new System.Drawing.Point(12, 413);
            this.sliderChannel.Name = "sliderChannel";
            this.sliderChannel.Size = new System.Drawing.Size(489, 41);
            this.sliderChannel.SliderLabel = "Channel";
            this.sliderChannel.TabIndex = 2;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(426, 12);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 31);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev.Location = new System.Drawing.Point(345, 12);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(75, 31);
            this.btnPrev.TabIndex = 5;
            this.btnPrev.Text = "Previous";
            this.btnPrev.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.Location = new System.Drawing.Point(12, 51);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(489, 330);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // cbZoom
            // 
            this.cbZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbZoom.AutoSize = true;
            this.cbZoom.Checked = true;
            this.cbZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbZoom.Location = new System.Drawing.Point(281, 12);
            this.cbZoom.Name = "cbZoom";
            this.cbZoom.Size = new System.Drawing.Size(58, 19);
            this.cbZoom.TabIndex = 7;
            this.cbZoom.Text = "Zoom";
            this.cbZoom.UseVisualStyleBackColor = true;
            this.cbZoom.CheckedChanged += new System.EventHandler(this.cbStretch_CheckedChanged);
            // 
            // lblFilename
            // 
            this.lblFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFilename.AutoSize = true;
            this.lblFilename.Location = new System.Drawing.Point(12, 384);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(38, 15);
            this.lblFilename.TabIndex = 8;
            this.lblFilename.Text = "label1";
            // 
            // cbAutoScale
            // 
            this.cbAutoScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAutoScale.AutoSize = true;
            this.cbAutoScale.Checked = true;
            this.cbAutoScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoScale.Location = new System.Drawing.Point(196, 12);
            this.cbAutoScale.Name = "cbAutoScale";
            this.cbAutoScale.Size = new System.Drawing.Size(79, 19);
            this.cbAutoScale.TabIndex = 10;
            this.cbAutoScale.Text = "AutoScale";
            this.cbAutoScale.UseVisualStyleBackColor = true;
            // 
            // cbRGB
            // 
            this.cbRGB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRGB.AutoSize = true;
            this.cbRGB.Checked = true;
            this.cbRGB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRGB.Location = new System.Drawing.Point(142, 12);
            this.cbRGB.Name = "cbRGB";
            this.cbRGB.Size = new System.Drawing.Size(48, 19);
            this.cbRGB.TabIndex = 11;
            this.cbRGB.Text = "RGB";
            this.cbRGB.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 560);
            this.Controls.Add(this.cbRGB);
            this.Controls.Add(this.cbAutoScale);
            this.Controls.Add(this.lblFilename);
            this.Controls.Add(this.cbZoom);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.sliderChannel);
            this.Controls.Add(this.sliderSlice);
            this.Controls.Add(this.sliderFrame);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SciTif: Test Image Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LabeledSlider sliderFrame;
        private LabeledSlider sliderSlice;
        private LabeledSlider sliderChannel;
        private Button btnNext;
        private Button btnPrev;
        private PictureBox pictureBox1;
        private CheckBox cbZoom;
        private Label lblFilename;
        private CheckBox cbAutoScale;
        private CheckBox cbRGB;
    }
}