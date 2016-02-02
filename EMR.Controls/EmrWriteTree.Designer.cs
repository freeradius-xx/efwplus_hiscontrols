namespace EMR.Controls
{
    partial class EmrWriteTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmrWriteTree));
            this.treeT = new DevComponents.AdvTree.AdvTree();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.node1 = new DevComponents.AdvTree.Node();
            this.node3 = new DevComponents.AdvTree.Node();
            this.node4 = new DevComponents.AdvTree.Node();
            this.node5 = new DevComponents.AdvTree.Node();
            this.node6 = new DevComponents.AdvTree.Node();
            this.node2 = new DevComponents.AdvTree.Node();
            this.node7 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.rightMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新增病历ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改病历ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除病历ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.签名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退回ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.treeT)).BeginInit();
            this.rightMenuStrip.SuspendLayout();
            this.SuspendLayout();
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
            this.treeT.Location = new System.Drawing.Point(0, 0);
            this.treeT.Name = "treeT";
            this.treeT.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1,
            this.node2,
            this.node7});
            this.treeT.NodesConnector = this.nodeConnector1;
            this.treeT.NodeStyle = this.elementStyle1;
            this.treeT.PathSeparator = ";";
            this.treeT.Size = new System.Drawing.Size(252, 429);
            this.treeT.Styles.Add(this.elementStyle1);
            this.treeT.TabIndex = 2;
            this.treeT.Text = "advTree1";
            this.treeT.AfterCollapse += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.treeT_AfterCollapse);
            this.treeT.AfterExpand += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.treeT_AfterExpand);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "任务.png");
            this.imageList1.Images.SetKeyName(1, "医嘱.png");
            this.imageList1.Images.SetKeyName(2, "住院病历.png");
            this.imageList1.Images.SetKeyName(3, "护理记录.png");
            this.imageList1.Images.SetKeyName(4, "知情文件.png");
            this.imageList1.Images.SetKeyName(5, "病历首页.png");
            this.imageList1.Images.SetKeyName(6, "病历分类.png");
            this.imageList1.Images.SetKeyName(7, "文件.png");
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node3,
            this.node4,
            this.node5,
            this.node6});
            this.node1.Text = "我的任务";
            // 
            // node3
            // 
            this.node3.Name = "node3";
            this.node3.Text = "待编写病历";
            // 
            // node4
            // 
            this.node4.Name = "node4";
            this.node4.Text = "待签名病历";
            // 
            // node5
            // 
            this.node5.Name = "node5";
            this.node5.Text = "被退回病历";
            // 
            // node6
            // 
            this.node6.Name = "node6";
            this.node6.Text = "待签名下级病历";
            // 
            // node2
            // 
            this.node2.Expanded = true;
            this.node2.Name = "node2";
            this.node2.Text = "住院病历";
            // 
            // node7
            // 
            this.node7.Name = "node7";
            this.node7.Text = "病案首页";
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
            this.新增病历ToolStripMenuItem,
            this.修改病历ToolStripMenuItem,
            this.删除病历ToolStripMenuItem,
            this.toolStripSeparator1,
            this.签名ToolStripMenuItem,
            this.退回ToolStripMenuItem});
            this.rightMenuStrip.Name = "rightMenuStrip";
            this.rightMenuStrip.Size = new System.Drawing.Size(125, 120);
            this.rightMenuStrip.Opened += new System.EventHandler(this.rightMenuStrip_Opened);
            // 
            // 新增病历ToolStripMenuItem
            // 
            this.新增病历ToolStripMenuItem.Name = "新增病历ToolStripMenuItem";
            this.新增病历ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.新增病历ToolStripMenuItem.Text = "新增病历";
            this.新增病历ToolStripMenuItem.Click += new System.EventHandler(this.新增病历ToolStripMenuItem_Click);
            // 
            // 修改病历ToolStripMenuItem
            // 
            this.修改病历ToolStripMenuItem.Name = "修改病历ToolStripMenuItem";
            this.修改病历ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.修改病历ToolStripMenuItem.Text = "修改标题";
            this.修改病历ToolStripMenuItem.Click += new System.EventHandler(this.修改病历ToolStripMenuItem_Click);
            // 
            // 删除病历ToolStripMenuItem
            // 
            this.删除病历ToolStripMenuItem.Name = "删除病历ToolStripMenuItem";
            this.删除病历ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除病历ToolStripMenuItem.Text = "删除病历";
            this.删除病历ToolStripMenuItem.Click += new System.EventHandler(this.删除病历ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // 签名ToolStripMenuItem
            // 
            this.签名ToolStripMenuItem.Name = "签名ToolStripMenuItem";
            this.签名ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.签名ToolStripMenuItem.Text = "签名";
            this.签名ToolStripMenuItem.Click += new System.EventHandler(this.签名ToolStripMenuItem_Click);
            // 
            // 退回ToolStripMenuItem
            // 
            this.退回ToolStripMenuItem.Name = "退回ToolStripMenuItem";
            this.退回ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.退回ToolStripMenuItem.Text = "退回";
            this.退回ToolStripMenuItem.Click += new System.EventHandler(this.退回ToolStripMenuItem_Click);
            // 
            // EmrWriteTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeT);
            this.Name = "EmrWriteTree";
            this.Size = new System.Drawing.Size(252, 429);
            ((System.ComponentModel.ISupportInitialize)(this.treeT)).EndInit();
            this.rightMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.AdvTree.AdvTree treeT;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.Node node3;
        private DevComponents.AdvTree.Node node4;
        private DevComponents.AdvTree.Node node5;
        private DevComponents.AdvTree.Node node6;
        private DevComponents.AdvTree.Node node2;
        private DevComponents.AdvTree.Node node7;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private System.Windows.Forms.ContextMenuStrip rightMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 新增病历ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改病历ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除病历ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 签名ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退回ToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;

    }
}
