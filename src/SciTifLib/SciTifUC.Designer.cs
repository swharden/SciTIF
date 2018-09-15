namespace SciTIFlib
{
    partial class SciTifUC
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
            this.panelFrame = new System.Windows.Forms.Panel();
            this.picture = new System.Windows.Forms.PictureBox();
            this.panelFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFrame
            // 
            this.panelFrame.AutoScroll = true;
            this.panelFrame.BackColor = System.Drawing.Color.Red;
            this.panelFrame.Controls.Add(this.picture);
            this.panelFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFrame.Location = new System.Drawing.Point(0, 0);
            this.panelFrame.Name = "panelFrame";
            this.panelFrame.Size = new System.Drawing.Size(245, 201);
            this.panelFrame.TabIndex = 0;
            // 
            // picture
            // 
            this.picture.BackColor = System.Drawing.Color.Blue;
            this.picture.Location = new System.Drawing.Point(57, 19);
            this.picture.MinimumSize = new System.Drawing.Size(50, 80);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(216, 204);
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            this.picture.Click += new System.EventHandler(this.picture_Click);
            this.picture.DoubleClick += new System.EventHandler(this.picture_DoubleClick);
            this.picture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picture_MouseDown);
            this.picture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picture_MouseMove);
            this.picture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picture_MouseUp);
            // 
            // SciTifUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Controls.Add(this.panelFrame);
            this.Name = "SciTifUC";
            this.Size = new System.Drawing.Size(245, 201);
            this.Load += new System.EventHandler(this.SciTifUC_Load);
            this.SizeChanged += new System.EventHandler(this.SciTifUC_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SciTifUC_Paint);
            this.Resize += new System.EventHandler(this.SciTifUC_Resize);
            this.panelFrame.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFrame;
        private System.Windows.Forms.PictureBox picture;
    }
}
