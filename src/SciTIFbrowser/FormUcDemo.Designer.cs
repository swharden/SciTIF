namespace SciTIFbrowser
{
    partial class FormUcDemo
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
            this.sciTifUC1 = new SciTIFlib.SciTifUC();
            this.SuspendLayout();
            // 
            // sciTifUC1
            // 
            this.sciTifUC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.sciTifUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sciTifUC1.Location = new System.Drawing.Point(0, 0);
            this.sciTifUC1.Name = "sciTifUC1";
            this.sciTifUC1.Size = new System.Drawing.Size(504, 364);
            this.sciTifUC1.TabIndex = 0;
            // 
            // FormUcDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 364);
            this.Controls.Add(this.sciTifUC1);
            this.Name = "FormUcDemo";
            this.Text = "SciTifUC Demo";
            this.Load += new System.EventHandler(this.FormUcDemo_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private SciTIFlib.SciTifUC sciTifUC1;
    }
}