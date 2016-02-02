namespace EMR.Controls
{
    partial class EmrTemplateTree
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmrTemplateTree));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.ckp = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.ckd = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.ckh = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cbDept = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.treeT = new DevComponents.AdvTree.AdvTree();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.rightMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新增模板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改模板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除模板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeT)).BeginInit();
            this.rightMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.ckp);
            this.panelEx1.Controls.Add(this.ckd);
            this.panelEx1.Controls.Add(this.ckh);
            this.panelEx1.Controls.Add(this.cbDept);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(247, 69);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // ckp
            // 
            // 
            // 
            // 
            this.ckp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ckp.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.ckp.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckp.Location = new System.Drawing.Point(143, 37);
            this.ckp.Name = "ckp";
            this.ckp.Size = new System.Drawing.Size(62, 23);
            this.ckp.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ckp.TabIndex = 4;
            this.ckp.Tag = "2";
            this.ckp.Text = "个人";
            this.ckp.CheckedChanged += new System.EventHandler(this.ckh_CheckedChanged);
            // 
            // ckd
            // 
            // 
            // 
            // 
            this.ckd.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ckd.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.ckd.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckd.Location = new System.Drawing.Point(93, 37);
            this.ckd.Name = "ckd";
            this.ckd.Size = new System.Drawing.Size(62, 23);
            this.ckd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ckd.TabIndex = 3;
            this.ckd.Tag = "1";
            this.ckd.Text = "本科";
            this.ckd.CheckedChanged += new System.EventHandler(this.ckh_CheckedChanged);
            // 
            // ckh
            // 
            // 
            // 
            // 
            this.ckh.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ckh.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.ckh.Checked = true;
            this.ckh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckh.CheckValue = "Y";
            this.ckh.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckh.Location = new System.Drawing.Point(45, 37);
            this.ckh.Name = "ckh";
            this.ckh.Size = new System.Drawing.Size(62, 23);
            this.ckh.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ckh.TabIndex = 2;
            this.ckh.Tag = "0";
            this.ckh.Text = "全院";
            this.ckh.CheckedChanged += new System.EventHandler(this.ckh_CheckedChanged);
            // 
            // cbDept
            // 
            this.cbDept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDept.DisplayMember = "Text";
            this.cbDept.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDept.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbDept.FormattingEnabled = true;
            this.cbDept.ItemHeight = 19;
            this.cbDept.Location = new System.Drawing.Point(43, 11);
            this.cbDept.Name = "cbDept";
            this.cbDept.Size = new System.Drawing.Size(201, 25);
            this.cbDept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbDept.TabIndex = 1;
            this.cbDept.SelectedIndexChanged += new System.EventHandler(this.cbDept_SelectedIndexChanged);
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(4, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(55, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "科室";
            // 
            // treeT
            // 
            this.treeT.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.treeT.AllowDrop = true;
            this.treeT.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.treeT.BackgroundStyle.Class = "TreeBorderKey";
            this.treeT.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.treeT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeT.DragDropEnabled = false;
            this.treeT.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeT.ImageList = this.imageList1;
            this.treeT.Location = new System.Drawing.Point(0, 69);
            this.treeT.Name = "treeT";
            this.treeT.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.treeT.NodesConnector = this.nodeConnector1;
            this.treeT.NodeStyle = this.elementStyle1;
            this.treeT.PathSeparator = ";";
            this.treeT.Size = new System.Drawing.Size(247, 392);
            this.treeT.Styles.Add(this.elementStyle1);
            this.treeT.TabIndex = 1;
            this.treeT.DoubleClick += new System.EventHandler(this.treeT_DoubleClick);
            this.treeT.AfterCollapse += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.treeT_AfterCollapse);
            this.treeT.AfterExpand += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.treeT_AfterExpand);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "病历分类.png");
            this.imageList1.Images.SetKeyName(1, "文件.png");
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // rightMenuStrip
            // 
            this.rightMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增模板ToolStripMenuItem,
            this.修改模板ToolStripMenuItem,
            this.删除模板ToolStripMenuItem});
            this.rightMenuStrip.Name = "rightMenuStrip";
            this.rightMenuStrip.Size = new System.Drawing.Size(125, 70);
            this.rightMenuStrip.Opened += new System.EventHandler(this.rightMenuStrip_Opened);
            // 
            // 新增模板ToolStripMenuItem
            // 
            this.新增模板ToolStripMenuItem.Name = "新增模板ToolStripMenuItem";
            this.新增模板ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.新增模板ToolStripMenuItem.Text = "新增模板";
            this.新增模板ToolStripMenuItem.Click += new System.EventHandler(this.新增模板ToolStripMenuItem_Click);
            // 
            // 修改模板ToolStripMenuItem
            // 
            this.修改模板ToolStripMenuItem.Name = "修改模板ToolStripMenuItem";
            this.修改模板ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.修改模板ToolStripMenuItem.Text = "修改模板";
            this.修改模板ToolStripMenuItem.Click += new System.EventHandler(this.修改模板ToolStripMenuItem_Click);
            // 
            // 删除模板ToolStripMenuItem
            // 
            this.删除模板ToolStripMenuItem.Name = "删除模板ToolStripMenuItem";
            this.删除模板ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除模板ToolStripMenuItem.Text = "删除模板";
            this.删除模板ToolStripMenuItem.Click += new System.EventHandler(this.删除模板ToolStripMenuItem_Click);
            // 
            // EmrTemplateTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeT);
            this.Controls.Add(this.panelEx1);
            this.Name = "EmrTemplateTree";
            this.Size = new System.Drawing.Size(247, 461);
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeT)).EndInit();
            this.rightMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckp;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckd;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckh;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbDept;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.AdvTree.AdvTree treeT;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private System.Windows.Forms.ContextMenuStrip rightMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 新增模板ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改模板ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除模板ToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
    }
}
