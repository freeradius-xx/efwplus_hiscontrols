using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMR.Controls.Action;
using EMR.Controls.Entity;

namespace EMR.Controls
{
    public partial class EmrTemplateTree : UserControl, IEmrTemplateTree
    {
        public emrController controller;//控制器
        public int selectRecordId = 0;//上次选中的
        private List<string> expandnodes;//展开节点

        public EmrTemplateTree()
        {
            InitializeComponent();

            ckh.Tag = 0;
            ckd.Tag = 1;
            ckp.Tag = 2;
        }


        #region IEmrTemplateTree 成员

        public void loadDeptData(DataTable dt,string defcode)
        {
            cbDept.SelectedIndexChanged -= new EventHandler(cbDept_SelectedIndexChanged);
            cbDept.DisplayMember = "deptname";
            cbDept.ValueMember = "deptcode";
            cbDept.DataSource = dt;
            cbDept.SelectedValue = defcode;
            cbDept.SelectedIndexChanged += new EventHandler(cbDept_SelectedIndexChanged);
        }

        public void loadTreeData(List<EmrCatalogue> clglist, List<EMR.Controls.Entity.EmrTemplateTree> templatelist)
        {
            treeT.Nodes.Clear();
            DevComponents.AdvTree.Node node=null;
            foreach (EmrCatalogue clg in clglist)
            {
                node = new DevComponents.AdvTree.Node(clg.NodeText);
                node.Tag = clg;
                node.ContextMenu = rightMenuStrip;
                node.ImageIndex = 0;
                DevComponents.AdvTree.Node nodeT = null;
                foreach (EMR.Controls.Entity.EmrTemplateTree t in templatelist.FindAll(x => x.CatalogueCode == clg.NodeCode))
                {
                    nodeT = new DevComponents.AdvTree.Node(t.TemplateText);
                    nodeT.Tag = t;
                    nodeT.ContextMenu = rightMenuStrip;
                    nodeT.ImageIndex = 1;
                    node.Nodes.Add(nodeT);
                }
                if (expandnodes != null && expandnodes.FindIndex(x => x == clg.NodeCode) != -1)
                {
                    node.Expand();
                }
                treeT.Nodes.Add(node);
            }
        }

        #endregion

        private void cbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            int level=ckh.Checked?(int)ckh.Tag:(ckd.Checked?(int)ckd.Tag:(int)ckp.Tag);
            controller.GetTemplateTreeData(cbDept.SelectedValue.ToString(), level);
        }

        private void ckh_CheckedChanged(object sender, EventArgs e)
        {
            if (((DevComponents.DotNetBar.Controls.CheckBoxX)sender).Checked)
            {
                int level = ckh.Checked ? (int)ckh.Tag : (ckd.Checked ? (int)ckd.Tag : (int)ckp.Tag);
                controller.GetTemplateTreeData(cbDept.SelectedValue.ToString(), level);
            }
        }

        private void 新增模板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string title="";
            if (DialogTitle.Show("输入模板名称", ref title) == DialogResult.OK)
            {
                EmrCatalogue clg= treeT.SelectedNode.Tag as EmrCatalogue;

                EMR.Controls.Entity.EmrTemplateTree template = new EMR.Controls.Entity.EmrTemplateTree();
                template.CatalogueCode = clg.NodeCode;
                template.DeptCode = cbDept.SelectedValue.ToString();
                template.DeptName = cbDept.SelectedText;
                template.UserCode = controller.emrView.CurrBindKeyData.DoctorCode;
                template.UserName = controller.emrView.CurrBindKeyData.DoctorName;
                template.LevelCode = ckh.Checked ? (int)ckh.Tag : (ckd.Checked ? (int)ckd.Tag : (int)ckp.Tag);
                template.TemplateText = title;

                controller.SaveEmrTemplateTree(template);
                selectRecordId = template.ID;
            }
        }

        private void 修改模板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EMR.Controls.Entity.EmrTemplateTree clg = treeT.SelectedNode.Tag as EMR.Controls.Entity.EmrTemplateTree;
            string title = clg.TemplateText;
            if (DialogTitle.Show("修改模板名称", ref title) == DialogResult.OK)
            {
                selectRecordId = clg.ID;
                clg.TemplateText = title;
                controller.SaveEmrTemplateTree(clg);
            }
        }

        private void 删除模板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确实要删除此病历模板吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                EMR.Controls.Entity.EmrTemplateTree clg = treeT.SelectedNode.Tag as EMR.Controls.Entity.EmrTemplateTree;
                controller.DeleteEmrTemplateTree(clg.ID, clg.EmrDataId,clg.DeptCode,clg.LevelCode);
            }
        }

        private void treeT_DoubleClick(object sender, EventArgs e)
        {
            if (treeT.SelectedNode != null && treeT.SelectedNode.Tag is EMR.Controls.Entity.EmrTemplateTree)
            {
                EMR.Controls.Entity.EmrTemplateTree clg = treeT.SelectedNode.Tag as EMR.Controls.Entity.EmrTemplateTree;
                controller.GetEmrTemplateTree(clg);
            }
        }

        private void rightMenuStrip_Opened(object sender, EventArgs e)
        {
            if (treeT.SelectedNode.Tag is EmrCatalogue)
            {
                新增模板ToolStripMenuItem.Enabled = true;
                修改模板ToolStripMenuItem.Enabled = false;
                删除模板ToolStripMenuItem.Enabled = false;
            }
            else
            {
                新增模板ToolStripMenuItem.Enabled = false;
                修改模板ToolStripMenuItem.Enabled = true;
                删除模板ToolStripMenuItem.Enabled = true;
            }
        }

        private void treeT_AfterExpand(object sender, DevComponents.AdvTree.AdvTreeNodeEventArgs e)
        {
            expandnodes = new List<string>();
            foreach (DevComponents.AdvTree.Node node in treeT.Nodes)
            {
                if (node.Expanded && node.Tag!=null)
                    expandnodes.Add((node.Tag as EMR.Controls.Entity.EmrCatalogue).NodeCode);
            }
        }

        private void treeT_AfterCollapse(object sender, DevComponents.AdvTree.AdvTreeNodeEventArgs e)
        {
            expandnodes = new List<string>();
            foreach (DevComponents.AdvTree.Node node in treeT.Nodes)
            {
                if (node.Expanded && node.Tag != null)
                    expandnodes.Add((node.Tag as EMR.Controls.Entity.EmrCatalogue).NodeCode);
            }
        }
    }
}
