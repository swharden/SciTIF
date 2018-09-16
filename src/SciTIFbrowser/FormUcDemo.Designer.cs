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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitDirView1 = new SplitDirView.SplitDirView();
            this.sciTifUC1 = new SciTIFlib.SciTifUC();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitDirView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sciTifUC1);
            this.splitContainer1.Size = new System.Drawing.Size(1051, 643);
            this.splitContainer1.SplitterDistance = 346;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitDirView1
            // 
            this.splitDirView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitDirView1.Location = new System.Drawing.Point(0, 0);
            this.splitDirView1.Name = "splitDirView1";
            this.splitDirView1.Size = new System.Drawing.Size(346, 643);
            this.splitDirView1.TabIndex = 0;
            this.splitDirView1.FileHighlighted += new System.EventHandler(this.splitDirView1_FileHighlighted);
            // 
            // sciTifUC1
            // 
            this.sciTifUC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.sciTifUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sciTifUC1.Location = new System.Drawing.Point(0, 0);
            this.sciTifUC1.Name = "sciTifUC1";
            this.sciTifUC1.Size = new System.Drawing.Size(701, 643);
            this.sciTifUC1.TabIndex = 0;
            // 
            // FormUcDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 643);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormUcDemo";
            this.Text = "SciTifUC Demo";
            this.Load += new System.EventHandler(this.FormUcDemo_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private SplitDirView.SplitDirView splitDirView1;
        private SciTIFlib.SciTifUC sciTifUC1;
    }
}