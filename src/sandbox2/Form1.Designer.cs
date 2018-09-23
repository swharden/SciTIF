namespace sandbox2
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkRed = new System.Windows.Forms.CheckBox();
            this.checkGreen = new System.Windows.Forms.CheckBox();
            this.checkBlue = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox1.Location = new System.Drawing.Point(129, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(374, 326);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // checkRed
            // 
            this.checkRed.AutoSize = true;
            this.checkRed.Location = new System.Drawing.Point(13, 129);
            this.checkRed.Name = "checkRed";
            this.checkRed.Size = new System.Drawing.Size(46, 17);
            this.checkRed.TabIndex = 1;
            this.checkRed.Text = "Red";
            this.checkRed.UseVisualStyleBackColor = true;
            this.checkRed.CheckedChanged += new System.EventHandler(this.checkRed_CheckedChanged);
            // 
            // checkGreen
            // 
            this.checkGreen.AutoSize = true;
            this.checkGreen.Location = new System.Drawing.Point(13, 152);
            this.checkGreen.Name = "checkGreen";
            this.checkGreen.Size = new System.Drawing.Size(55, 17);
            this.checkGreen.TabIndex = 2;
            this.checkGreen.Text = "Green";
            this.checkGreen.UseVisualStyleBackColor = true;
            this.checkGreen.CheckedChanged += new System.EventHandler(this.checkGreen_CheckedChanged);
            // 
            // checkBlue
            // 
            this.checkBlue.AutoSize = true;
            this.checkBlue.Location = new System.Drawing.Point(13, 175);
            this.checkBlue.Name = "checkBlue";
            this.checkBlue.Size = new System.Drawing.Size(47, 17);
            this.checkBlue.TabIndex = 3;
            this.checkBlue.Text = "Blue";
            this.checkBlue.UseVisualStyleBackColor = true;
            this.checkBlue.CheckedChanged += new System.EventHandler(this.checkBlue_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 424);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBlue);
            this.Controls.Add(this.checkGreen);
            this.Controls.Add(this.checkRed);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox checkRed;
        private System.Windows.Forms.CheckBox checkGreen;
        private System.Windows.Forms.CheckBox checkBlue;
        private System.Windows.Forms.Button button1;
    }
}

