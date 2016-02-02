namespace BedCard.Controls
{
    partial class PatientControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labInfo = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // labInfo
            // 
            this.labInfo.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labInfo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labInfo.Location = new System.Drawing.Point(1, 1);
            this.labInfo.Name = "labInfo";
            this.labInfo.PaddingLeft = 10;
            this.labInfo.PaddingRight = 10;
            this.labInfo.Size = new System.Drawing.Size(526, 37);
            this.labInfo.TabIndex = 9;
            this.labInfo.Text = "床位:<b>12</b> 住院号：<b>00000008</b> 病人姓名：<b>张三</b>";
            // 
            // PatientControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.Controls.Add(this.labInfo);
            this.Name = "PatientControl";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(528, 39);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labInfo;

    }
}
