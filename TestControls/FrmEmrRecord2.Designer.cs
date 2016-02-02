namespace TestControls
{
    partial class FrmEmrRecord2
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
            this.emrTemplateTree1 = new EMR.Controls.EmrTemplateTree();
            this.emrRecord1 = new EMR.Controls.EmrRecord();
            this.SuspendLayout();
            // 
            // emrTemplateTree1
            // 
            this.emrTemplateTree1.Dock = System.Windows.Forms.DockStyle.Left;
            this.emrTemplateTree1.Location = new System.Drawing.Point(0, 0);
            this.emrTemplateTree1.Name = "emrTemplateTree1";
            this.emrTemplateTree1.Size = new System.Drawing.Size(247, 437);
            this.emrTemplateTree1.TabIndex = 0;
            // 
            // emrRecord1
            // 
            this.emrRecord1.CurrBindKeyData = null;
            this.emrRecord1.currFileInfo = null;
            this.emrRecord1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emrRecord1.emrControlType = EMR.Controls.Action.EmrControlType.病历模板;
            this.emrRecord1.emrDatastorageType = EMR.Controls.Action.EmrDatastorageType.文件存储;
            this.emrRecord1.emrOperStyle = EMR.Controls.Action.EmrOperStyle.默认;
            this.emrRecord1.EmrTemplateTree = this.emrTemplateTree1;
            this.emrRecord1.EmrWriteTree = null;
            this.emrRecord1.IsShowFileBtn = true;
            this.emrRecord1.LicenseKey = null;
            this.emrRecord1.Location = new System.Drawing.Point(247, 0);
            this.emrRecord1.Name = "emrRecord1";
            this.emrRecord1.Size = new System.Drawing.Size(579, 437);
            this.emrRecord1.TabIndex = 1;
            // 
            // FrmEmrRecord2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 437);
            this.Controls.Add(this.emrRecord1);
            this.Controls.Add(this.emrTemplateTree1);
            this.Name = "FrmEmrRecord2";
            this.Text = "病历模板";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmEmrRecord2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private EMR.Controls.EmrTemplateTree emrTemplateTree1;
        private EMR.Controls.EmrRecord emrRecord1;
    }
}