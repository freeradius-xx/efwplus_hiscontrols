namespace EMR.Controls
{
    partial class WriteDomain
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
            this.label1 = new System.Windows.Forms.Label();
            this.listdomain = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "请选择域";
            // 
            // listdomain
            // 
            this.listdomain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listdomain.FormattingEnabled = true;
            this.listdomain.ItemHeight = 12;
            this.listdomain.Items.AddRange(new object[] {
            "当前页码",
            "总页数",
            "系统时间",
            "系统日期",
            "文本时间",
            "文本日期",
            "复选框(空)",
            "复选框(True)",
            "复选框(False)"});
            this.listdomain.Location = new System.Drawing.Point(0, 14);
            this.listdomain.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.listdomain.Name = "listdomain";
            this.listdomain.Size = new System.Drawing.Size(201, 244);
            this.listdomain.TabIndex = 1;
            this.listdomain.DoubleClick += new System.EventHandler(this.listdomain_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listdomain);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(201, 268);
            this.panel1.TabIndex = 1;
            // 
            // WriteDomain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(201, 268);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WriteDomain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "插入域";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listdomain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}