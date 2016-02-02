namespace TestControls
{
    partial class FrmBedCard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBedCard));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.bedCardControl1 = new BedCard.Controls.BedCardControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(477, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton1.Text = "新增";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton2.Text = "删除";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(76, 22);
            this.toolStripButton3.Text = "加载数据";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // bedCardControl1
            // 
            this.bedCardControl1.AutoScroll = true;
            this.bedCardControl1.BackColor = System.Drawing.Color.White;
            this.bedCardControl1.BedContextFields = null;
            this.bedCardControl1.BedHeight = 160;
            this.bedCardControl1.BedWidth = 162;
            this.bedCardControl1.DataSource = null;
            this.bedCardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bedCardControl1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.bedCardControl1.LicenseKey = "";
            this.bedCardControl1.Location = new System.Drawing.Point(0, 25);
            this.bedCardControl1.Name = "bedCardControl1";
            this.bedCardControl1.SelectedBed = null;
            this.bedCardControl1.SelectedBedIndex = -1;
            this.bedCardControl1.Size = new System.Drawing.Size(477, 339);
            this.bedCardControl1.TabIndex = 1;
            this.bedCardControl1.BedClick += new System.EventHandler(this.bedCardControl1_BedClick);
            this.bedCardControl1.BedFormatStyleEvent += new BedCard.Controls.BedFormatStyle(this.bedCardControl1_BedFormatStyleEvent);
            this.bedCardControl1.AdviceClick += new BedCard.Controls.OnButtonClick(this.bedCardControl1_AdviceClick);
            this.bedCardControl1.BedDoubleClick += new System.EventHandler(this.bedCardControl1_BedDoubleClick);
            this.bedCardControl1.TemperatureClick += new BedCard.Controls.OnButtonClick(this.bedCardControl1_TemperatureClick);
            this.bedCardControl1.BedTitleClick += new BedCard.Controls.OnButtonClick(this.bedCardControl1_BedTitleClick);
            this.bedCardControl1.HeadPageClick += new BedCard.Controls.OnButtonClick(this.bedCardControl1_HeadPageClick);
            this.bedCardControl1.ApplyFormClick += new BedCard.Controls.OnButtonClick(this.bedCardControl1_ApplyFormClick);
            // 
            // FrmBedCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 364);
            this.Controls.Add(this.bedCardControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FrmBedCard";
            this.Text = "Form2";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private BedCard.Controls.BedCardControl bedCardControl1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;

    }
}