namespace TestControls
{
    partial class FrmEmrRecord3
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
            this.emrWriteTree1 = new EMR.Controls.EmrWriteTree();
            this.emrRecord1 = new EMR.Controls.EmrRecord();
            this.SuspendLayout();
            // 
            // emrWriteTree1
            // 
            this.emrWriteTree1.Dock = System.Windows.Forms.DockStyle.Left;
            this.emrWriteTree1.Location = new System.Drawing.Point(0, 0);
            this.emrWriteTree1.Name = "emrWriteTree1";
            this.emrWriteTree1.Size = new System.Drawing.Size(252, 400);
            this.emrWriteTree1.TabIndex = 0;
            // 
            // emrRecord1
            // 
            this.emrRecord1.CurrBindKeyData = null;
            this.emrRecord1.currFileInfo = null;
            this.emrRecord1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emrRecord1.emrControlType = EMR.Controls.Action.EmrControlType.病历模板;
            this.emrRecord1.emrDatastorageType = EMR.Controls.Action.EmrDatastorageType.文件存储;
            this.emrRecord1.emrOperStyle = EMR.Controls.Action.EmrOperStyle.默认;
            this.emrRecord1.EmrTemplateTree = null;
            this.emrRecord1.EmrWriteTree = this.emrWriteTree1;
            this.emrRecord1.IsShowFileBtn = true;
            this.emrRecord1.LicenseKey = null;
            this.emrRecord1.Location = new System.Drawing.Point(252, 0);
            this.emrRecord1.Name = "emrRecord1";
            this.emrRecord1.Size = new System.Drawing.Size(500, 400);
            this.emrRecord1.TabIndex = 1;
            // 
            // FrmEmrRecord3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 400);
            this.Controls.Add(this.emrRecord1);
            this.Controls.Add(this.emrWriteTree1);
            this.Name = "FrmEmrRecord3";
            this.Text = "病历书写";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmEmrRecord3_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private EMR.Controls.EmrWriteTree emrWriteTree1;
        private EMR.Controls.EmrRecord emrRecord1;
    }
}