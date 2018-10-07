namespace sandbox3
{
    partial class FormContrast
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
            this.bandC1 = new SciTIFlib.BandC();
            this.SuspendLayout();
            // 
            // bandC1
            // 
            this.bandC1.Location = new System.Drawing.Point(12, 12);
            this.bandC1.Name = "bandC1";
            this.bandC1.Size = new System.Drawing.Size(172, 513);
            this.bandC1.TabIndex = 0;
            // 
            // FormContrast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(197, 384);
            this.Controls.Add(this.bandC1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormContrast";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Brightness and Contrast";
            this.Load += new System.EventHandler(this.FormContrast_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SciTIFlib.BandC bandC1;
    }
}