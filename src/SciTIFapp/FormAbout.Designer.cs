namespace SciTIFapp
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.rtbGitHub = new System.Windows.Forms.RichTextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblSep = new System.Windows.Forms.Label();
            this.rtbAuthor = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbGitHub
            // 
            this.rtbGitHub.BackColor = System.Drawing.SystemColors.Control;
            this.rtbGitHub.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbGitHub.Location = new System.Drawing.Point(138, 71);
            this.rtbGitHub.Name = "rtbGitHub";
            this.rtbGitHub.ReadOnly = true;
            this.rtbGitHub.Size = new System.Drawing.Size(181, 37);
            this.rtbGitHub.TabIndex = 25;
            this.rtbGitHub.Text = "Project Page:\nhttp://github.com/swharden/SciTIF";
            this.rtbGitHub.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rtbGitHub_MouseClick);
            this.rtbGitHub.Enter += new System.EventHandler(this.rtbGitHub_Enter);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(135, 34);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(69, 13);
            this.lblVersion.TabIndex = 24;
            this.lblVersion.Text = "Version 0.0.1";
            // 
            // lblSep
            // 
            this.lblSep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSep.Location = new System.Drawing.Point(127, 12);
            this.lblSep.Name = "lblSep";
            this.lblSep.Size = new System.Drawing.Size(2, 170);
            this.lblSep.TabIndex = 23;
            // 
            // rtbAuthor
            // 
            this.rtbAuthor.BackColor = System.Drawing.SystemColors.Control;
            this.rtbAuthor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbAuthor.Location = new System.Drawing.Point(138, 129);
            this.rtbAuthor.Name = "rtbAuthor";
            this.rtbAuthor.ReadOnly = true;
            this.rtbAuthor.Size = new System.Drawing.Size(168, 65);
            this.rtbAuthor.TabIndex = 22;
            this.rtbAuthor.Text = "Scott W Harden, D.M.D., Ph.D\nPresident & CEO\nHarden Technologies, LLC\nhttp://tech" +
    ".SWHarden.com";
            this.rtbAuthor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rtbAuthor_MouseClick);
            this.rtbAuthor.Enter += new System.EventHandler(this.rtbAuthor_Enter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(109, 170);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(132, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(77, 32);
            this.lblTitle.TabIndex = 20;
            this.lblTitle.Text = "SciTIF";
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 195);
            this.Controls.Add(this.rtbGitHub);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblSep);
            this.Controls.Add(this.rtbAuthor);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormAbout";
            this.Text = "About SciTIF";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbGitHub;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblSep;
        private System.Windows.Forms.RichTextBox rtbAuthor;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblTitle;
    }
}