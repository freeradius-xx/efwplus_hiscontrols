namespace EMR.Controls
{
    partial class FrmTableColRow
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
            this.pBottom = new DevComponents.DotNetBar.PanelEx();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.txtColColumn = new DevComponents.Editors.IntegerInput();
            this.txtRowCount = new DevComponents.Editors.IntegerInput();
            this.labelX33 = new DevComponents.DotNetBar.LabelX();
            this.labelX32 = new DevComponents.DotNetBar.LabelX();
            this.pBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtColColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRowCount)).BeginInit();
            this.SuspendLayout();
            // 
            // pBottom
            // 
            this.pBottom.CanvasColor = System.Drawing.SystemColors.Control;
            this.pBottom.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.pBottom.Controls.Add(this.btnCancel);
            this.pBottom.Controls.Add(this.btnOk);
            this.pBottom.Controls.Add(this.txtColColumn);
            this.pBottom.Controls.Add(this.txtRowCount);
            this.pBottom.Controls.Add(this.labelX33);
            this.pBottom.Controls.Add(this.labelX32);
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBottom.Location = new System.Drawing.Point(0, 0);
            this.pBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(174, 98);
            this.pBottom.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.pBottom.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.pBottom.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.pBottom.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.pBottom.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.pBottom.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.pBottom.Style.GradientAngle = 90;
            this.pBottom.TabIndex = 397;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(19, 61);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(45, 21);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(75, 61);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(69, 21);
            this.btnOk.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "确定";
            // 
            // txtColColumn
            // 
            // 
            // 
            // 
            this.txtColColumn.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtColColumn.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtColColumn.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtColColumn.Location = new System.Drawing.Point(75, 9);
            this.txtColColumn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtColColumn.MaxValue = 20;
            this.txtColColumn.MinValue = 1;
            this.txtColColumn.Name = "txtColColumn";
            this.txtColColumn.ShowUpDown = true;
            this.txtColColumn.Size = new System.Drawing.Size(69, 21);
            this.txtColColumn.TabIndex = 1;
            this.txtColColumn.Value = 1;
            // 
            // txtRowCount
            // 
            // 
            // 
            // 
            this.txtRowCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtRowCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtRowCount.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtRowCount.Location = new System.Drawing.Point(75, 30);
            this.txtRowCount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtRowCount.MaxValue = 20;
            this.txtRowCount.MinValue = 1;
            this.txtRowCount.Name = "txtRowCount";
            this.txtRowCount.ShowUpDown = true;
            this.txtRowCount.Size = new System.Drawing.Size(69, 21);
            this.txtRowCount.TabIndex = 3;
            this.txtRowCount.Value = 1;
            // 
            // labelX33
            // 
            // 
            // 
            // 
            this.labelX33.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX33.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX33.Location = new System.Drawing.Point(12, 29);
            this.labelX33.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelX33.Name = "labelX33";
            this.labelX33.Size = new System.Drawing.Size(61, 17);
            this.labelX33.TabIndex = 2;
            this.labelX33.Text = "表格行数";
            // 
            // labelX32
            // 
            // 
            // 
            // 
            this.labelX32.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX32.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX32.Location = new System.Drawing.Point(12, 8);
            this.labelX32.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelX32.Name = "labelX32";
            this.labelX32.Size = new System.Drawing.Size(61, 16);
            this.labelX32.TabIndex = 0;
            this.labelX32.Text = "表格列数";
            // 
            // FrmTableColRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(174, 98);
            this.Controls.Add(this.pBottom);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTableColRow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "插入表格";
            this.pBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtColColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRowCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx pBottom;
        private DevComponents.DotNetBar.LabelX labelX33;
        private DevComponents.DotNetBar.LabelX labelX32;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.Editors.IntegerInput txtColColumn;
        private DevComponents.Editors.IntegerInput txtRowCount;
    }
}