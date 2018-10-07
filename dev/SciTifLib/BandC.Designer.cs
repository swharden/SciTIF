namespace SciTIFlib
{
    partial class BandC
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnPercentile = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnAutoAll = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.scrollMax = new System.Windows.Forms.HScrollBar();
            this.btnMaxAuto = new System.Windows.Forms.Button();
            this.btnMaxFull = new System.Windows.Forms.Button();
            this.nudMax = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.scrollMin = new System.Windows.Forms.HScrollBar();
            this.btnMinAuto = new System.Windows.Forms.Button();
            this.btnMinFull = new System.Windows.Forms.Button();
            this.nudMin = new System.Windows.Forms.NumericUpDown();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnPercentile);
            this.groupBox6.Controls.Add(this.btnSet);
            this.groupBox6.Controls.Add(this.btnReset);
            this.groupBox6.Controls.Add(this.btnAutoAll);
            this.groupBox6.Location = new System.Drawing.Point(3, 285);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(165, 72);
            this.groupBox6.TabIndex = 22;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Controls";
            // 
            // btnPercentile
            // 
            this.btnPercentile.Location = new System.Drawing.Point(84, 45);
            this.btnPercentile.Name = "btnPercentile";
            this.btnPercentile.Size = new System.Drawing.Size(71, 20);
            this.btnPercentile.TabIndex = 13;
            this.btnPercentile.Text = "Percentile";
            this.btnPercentile.UseVisualStyleBackColor = true;
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(7, 45);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(71, 20);
            this.btnSet.TabIndex = 12;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(84, 19);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(71, 20);
            this.btnReset.TabIndex = 11;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnAutoAll
            // 
            this.btnAutoAll.Location = new System.Drawing.Point(7, 19);
            this.btnAutoAll.Name = "btnAutoAll";
            this.btnAutoAll.Size = new System.Drawing.Size(71, 20);
            this.btnAutoAll.TabIndex = 10;
            this.btnAutoAll.Text = "Auto";
            this.btnAutoAll.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.pictureBox1);
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(165, 122);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Histogram";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.DarkGray;
            this.pictureBox1.Location = new System.Drawing.Point(6, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(153, 97);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel4);
            this.groupBox3.Controls.Add(this.btnMaxAuto);
            this.groupBox3.Controls.Add(this.btnMaxFull);
            this.groupBox3.Controls.Add(this.nudMax);
            this.groupBox3.Location = new System.Drawing.Point(3, 208);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(165, 71);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Maximum";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.scrollMax);
            this.panel4.Location = new System.Drawing.Point(6, 45);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(152, 20);
            this.panel4.TabIndex = 11;
            // 
            // scrollMax
            // 
            this.scrollMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrollMax.LargeChange = 1;
            this.scrollMax.Location = new System.Drawing.Point(0, 0);
            this.scrollMax.Maximum = 255;
            this.scrollMax.Name = "scrollMax";
            this.scrollMax.Size = new System.Drawing.Size(150, 18);
            this.scrollMax.TabIndex = 4;
            this.scrollMax.Value = 197;
            this.scrollMax.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollMax_Scroll);
            // 
            // btnMaxAuto
            // 
            this.btnMaxAuto.Location = new System.Drawing.Point(120, 19);
            this.btnMaxAuto.Name = "btnMaxAuto";
            this.btnMaxAuto.Size = new System.Drawing.Size(38, 20);
            this.btnMaxAuto.TabIndex = 10;
            this.btnMaxAuto.Text = "auto";
            this.btnMaxAuto.UseVisualStyleBackColor = true;
            // 
            // btnMaxFull
            // 
            this.btnMaxFull.Location = new System.Drawing.Point(82, 19);
            this.btnMaxFull.Name = "btnMaxFull";
            this.btnMaxFull.Size = new System.Drawing.Size(32, 20);
            this.btnMaxFull.TabIndex = 9;
            this.btnMaxFull.Text = "full";
            this.btnMaxFull.UseVisualStyleBackColor = true;
            this.btnMaxFull.Click += new System.EventHandler(this.btnMaxFull_Click);
            // 
            // nudMax
            // 
            this.nudMax.Location = new System.Drawing.Point(6, 19);
            this.nudMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMax.Name = "nudMax";
            this.nudMax.Size = new System.Drawing.Size(70, 20);
            this.nudMax.TabIndex = 2;
            this.nudMax.Value = new decimal(new int[] {
            197,
            0,
            0,
            0});
            this.nudMax.ValueChanged += new System.EventHandler(this.nudMax_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.btnMinAuto);
            this.groupBox1.Controls.Add(this.btnMinFull);
            this.groupBox1.Controls.Add(this.nudMin);
            this.groupBox1.Location = new System.Drawing.Point(3, 131);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(165, 71);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Minimum";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.scrollMin);
            this.panel2.Location = new System.Drawing.Point(6, 45);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(152, 20);
            this.panel2.TabIndex = 11;
            // 
            // scrollMin
            // 
            this.scrollMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrollMin.LargeChange = 1;
            this.scrollMin.Location = new System.Drawing.Point(0, 0);
            this.scrollMin.Maximum = 255;
            this.scrollMin.Name = "scrollMin";
            this.scrollMin.Size = new System.Drawing.Size(150, 18);
            this.scrollMin.TabIndex = 4;
            this.scrollMin.Value = 57;
            this.scrollMin.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollMin_Scroll);
            // 
            // btnMinAuto
            // 
            this.btnMinAuto.Location = new System.Drawing.Point(120, 19);
            this.btnMinAuto.Name = "btnMinAuto";
            this.btnMinAuto.Size = new System.Drawing.Size(38, 20);
            this.btnMinAuto.TabIndex = 10;
            this.btnMinAuto.Text = "auto";
            this.btnMinAuto.UseVisualStyleBackColor = true;
            // 
            // btnMinFull
            // 
            this.btnMinFull.Location = new System.Drawing.Point(82, 19);
            this.btnMinFull.Name = "btnMinFull";
            this.btnMinFull.Size = new System.Drawing.Size(32, 20);
            this.btnMinFull.TabIndex = 9;
            this.btnMinFull.Text = "full";
            this.btnMinFull.UseVisualStyleBackColor = true;
            this.btnMinFull.Click += new System.EventHandler(this.btnMinFull_Click);
            // 
            // nudMin
            // 
            this.nudMin.Location = new System.Drawing.Point(6, 19);
            this.nudMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMin.Name = "nudMin";
            this.nudMin.Size = new System.Drawing.Size(70, 20);
            this.nudMin.TabIndex = 2;
            this.nudMin.Value = new decimal(new int[] {
            57,
            0,
            0,
            0});
            this.nudMin.ValueChanged += new System.EventHandler(this.nudMin_ValueChanged);
            // 
            // BandC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "BandC";
            this.Size = new System.Drawing.Size(172, 360);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnPercentile;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnAutoAll;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.HScrollBar scrollMax;
        private System.Windows.Forms.Button btnMaxAuto;
        private System.Windows.Forms.Button btnMaxFull;
        private System.Windows.Forms.NumericUpDown nudMax;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.HScrollBar scrollMin;
        private System.Windows.Forms.Button btnMinAuto;
        private System.Windows.Forms.Button btnMinFull;
        private System.Windows.Forms.NumericUpDown nudMin;
    }
}
