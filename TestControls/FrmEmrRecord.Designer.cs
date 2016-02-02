namespace TestControls
{
    partial class FrmEmrRecord
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.emrRecord1 = new EMR.Controls.EmrRecord();
            this.SuspendLayout();
            // 
            // emrRecord1
            // 
            this.emrRecord1.CurrBindKeyData = null;
            this.emrRecord1.currFileInfo = null;
            this.emrRecord1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emrRecord1.emrControlType = EMR.Controls.Action.EmrControlType.病历模板;
            this.emrRecord1.emrDatastorageType = EMR.Controls.Action.EmrDatastorageType.数据库存储;
            this.emrRecord1.emrOperStyle = EMR.Controls.Action.EmrOperStyle.默认;
            this.emrRecord1.IsShowFileBtn = true;
            this.emrRecord1.LicenseKey = "";
            this.emrRecord1.Location = new System.Drawing.Point(0, 0);
            this.emrRecord1.Name = "emrRecord1";
            this.emrRecord1.Size = new System.Drawing.Size(915, 385);
            this.emrRecord1.TabIndex = 0;
            // 
            // FrmEmrRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 385);
            this.Controls.Add(this.emrRecord1);
            this.Name = "FrmEmrRecord";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private EMR.Controls.EmrRecord emrRecord1;









    }
}

