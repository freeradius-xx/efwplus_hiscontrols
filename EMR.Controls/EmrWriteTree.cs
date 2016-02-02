using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMR.Controls.Action;

namespace EMR.Controls
{
    public partial class EmrWriteTree : UserControl,IEmrWriteTree
    {
        public emrController controller;//控制器
        public int selectRecordId = 0;//上次选中的
        private List<string> expandnodes;//展开节点

        public EmrWriteTree()
        {
            InitializeComponent();
           
        }

        private void createTreeRoot()
        {
            treeT.Nodes.Clear();
            DevComponents.AdvTree.Node node = null;
            node = new DevComponents.AdvTree.Node("我的任务");
            node.Name = "mytask";
            node.Expand();
            node.ImageIndex = 0;
            treeT.Nodes.Add(node);
            node = new DevComponents.AdvTree.Node("住院医嘱");
            node.Name = "medorder";
            node.ImageIndex = 1;
            treeT.Nodes.Add(node);
            node = new DevComponents.AdvTree.Node("住院病历");
            node.Name = "medemr";
            node.Expand();
            node.ImageIndex = 2;
            treeT.Nodes.Add(node);
            node = new DevComponents.AdvTree.Node("护理文书");
            node.Name = "medbook";
            node.ImageIndex = 3;
            treeT.Nodes.Add(node);
            node = new DevComponents.AdvTree.Node("知情文件");
            node.Name = "infofiles";
            node.ImageIndex = 4;
            treeT.Nodes.Add(node);
            node = new DevComponents.AdvTree.Node("病历首页");
            node.Name = "homepage";
            node.ImageIndex = 5;
            treeT.Nodes.Add(node);
        }

        private void createTreeMytask()
        {
            DevComponents.AdvTree.Node pnode = treeT.Nodes.Find("mytask", false)[0];
            DevComponents.AdvTree.Node node = null;

            node = new DevComponents.AdvTree.Node("待编写病历");
            node.Name = "staywrite";
            pnode.Nodes.Add(node);
            node = new DevComponents.AdvTree.Node("待签名病历");
            node.Name = "staysign";
            pnode.Nodes.Add(node);
            node = new DevComponents.AdvTree.Node("被退回病历");
            node.Name = "sendback";
            pnode.Nodes.Add(node);
            node = new DevComponents.AdvTree.Node("待签名的下级病历");
            node.Name = "stayupsign";
            pnode.Nodes.Add(node);

        }

        private void createTreeOrder(List<EMR.Controls.Entity.EmrCatalogue> clglist, List<EMR.Controls.Entity.EmrWriteRecord> recordlist)
        {
            DevComponents.AdvTree.Node pnode = treeT.Nodes.Find("medemr", false)[0];
            DevComponents.AdvTree.Node node = null;

            foreach (EMR.Controls.Entity.EmrCatalogue clg in clglist)
            {
                node = new DevComponents.AdvTree.Node(clg.NodeText);
                node.Tag = clg;
                node.ContextMenu = rightMenuStrip;
                node.ImageIndex = 6;
                node.NodeClick += new EventHandler(node_NodeClick);
                DevComponents.AdvTree.Node nodeT = null;
                foreach (EMR.Controls.Entity.EmrWriteRecord t in recordlist.FindAll(x => x.CatalogueCode == clg.NodeCode))
                {
                    nodeT = new DevComponents.AdvTree.Node(t.RecordText);
                    nodeT.Tag = t;
                    nodeT.ContextMenu = rightMenuStrip;
                    nodeT.ImageIndex = 7;
                    nodeT.NodeDoubleClick += new EventHandler(WriteRecord_NodeDoubleClick);
                    node.Nodes.Add(nodeT);

                    if (selectRecordId == t.ID)
                    {
                        //node.Expand();
                        treeT.SelectedNode = nodeT;
                    }
                }
                if (expandnodes!=null && expandnodes.FindIndex(x => x == clg.NodeCode) != -1)
                {
                    node.Expand();
                }
                pnode.Nodes.Add(node);
            }
        }

        void node_NodeClick(object sender, EventArgs e)
        {
            expandnodes = new List<string>();
            DevComponents.AdvTree.Node pnode = treeT.Nodes.Find("medemr", false)[0];
            foreach (DevComponents.AdvTree.Node node in pnode.Nodes)
            {
                if (node.Expanded)
                    expandnodes.Add((node.Tag as EMR.Controls.Entity.EmrCatalogue).NodeCode);
            }
        }

        

        #region IEmrWriteTree 成员

        public void loadTreeData(List<EMR.Controls.Entity.EmrCatalogue> clglist, List<EMR.Controls.Entity.EmrWriteRecord> recordlist)
        {
            createTreeRoot();
            createTreeMytask();
            createTreeOrder(clglist, recordlist);
        }

        #endregion

        private void rightMenuStrip_Opened(object sender, EventArgs e)
        {
            if (treeT.SelectedNode.Tag is EMR.Controls.Entity.EmrCatalogue)
            {
                新增病历ToolStripMenuItem.Enabled = true;
                修改病历ToolStripMenuItem.Enabled = false;
                删除病历ToolStripMenuItem.Enabled = false;
                签名ToolStripMenuItem.Enabled = false;
                退回ToolStripMenuItem.Enabled = false;
            }
            else if (treeT.SelectedNode.Tag is EMR.Controls.Entity.EmrWriteRecord)
            {
                新增病历ToolStripMenuItem.Enabled = false;
                修改病历ToolStripMenuItem.Enabled = true;
                删除病历ToolStripMenuItem.Enabled = true;

                签名ToolStripMenuItem.Enabled = true;
                退回ToolStripMenuItem.Enabled = true;

                EMR.Controls.Entity.EmrWriteRecord record = treeT.SelectedNode.Tag as EMR.Controls.Entity.EmrWriteRecord;
                bool _sign, _back;
                EmrBindKeyData _bindkey = new EmrBindKeyData(controller.emrView.CurrBindKeyData.HosptialId, controller.emrView.CurrBindKeyData.PatientId, controller.emrView.CurrBindKeyData.CurrDeptCode, controller.emrView.CurrBindKeyData.CurrDeptName, controller.emrView.CurrBindKeyData.UserCode, controller.emrView.CurrBindKeyData.UserName, controller.emrView.CurrBindKeyData.UserLevel, controller.emrView.CurrBindKeyData.UserLevelName);
                _bindkey.ChangeEmrData(record.ID, record.EmrDataId, record.CatalogueCode, record.DeptCode, record.DeptName, record.UserCode, record.UserName, record.FirstSignature, record.FirstDoctorLevel, record.SecondSignature, record.SecondDoctorCode, record.SecondDoctorLevel, record.ThreeSignature, record.ThreeDoctorCode, record.ThreeDoctorLevel, record.RecordText);
                controller.VerifSignNameState(_bindkey, out _sign, out _back);
                签名ToolStripMenuItem.Enabled = _sign;
                退回ToolStripMenuItem.Enabled = _back;
            }
        }

        private void 新增病历ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string title = treeT.SelectedNode.Text;
            if (DialogTitle.Show("输入病历名称", ref title) == DialogResult.OK)
            {
                EMR.Controls.Entity.EmrCatalogue clg = treeT.SelectedNode.Tag as EMR.Controls.Entity.EmrCatalogue;

                EMR.Controls.Entity.EmrWriteRecord record = new EMR.Controls.Entity.EmrWriteRecord();
                record.CatalogueCode = clg.NodeCode;
                record.RecordText = title;
                record.DeptCode = controller.emrView.CurrBindKeyData.CurrDeptCode;
                record.DeptName = controller.emrView.CurrBindKeyData.CurrDeptName;
                record.UserCode = controller.emrView.CurrBindKeyData.UserCode;
                record.UserName = controller.emrView.CurrBindKeyData.UserName;
                record.PatientId = controller.emrView.CurrBindKeyData.PatientId;
                record.OrderNum = treeT.SelectedNode.Nodes.Count + 1;
                record.HosptialId = controller.emrView.CurrBindKeyData.HosptialId;

                controller.SaveEmrWriteRecord(record);

                selectRecordId = record.ID;
            }
        }

        private void 修改病历ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EMR.Controls.Entity.EmrWriteRecord record = treeT.SelectedNode.Tag as EMR.Controls.Entity.EmrWriteRecord;
            string title = record.RecordText;
            if (DialogTitle.Show("修改病历名称", ref title) == DialogResult.OK)
            {
                selectRecordId = record.ID;  
                record.RecordText = title;
                controller.SaveEmrWriteRecord(record);
            }
        }

        private void 删除病历ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确实要删除此病历吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                EMR.Controls.Entity.EmrWriteRecord record = treeT.SelectedNode.Tag as EMR.Controls.Entity.EmrWriteRecord;
                controller.DeleteEmrWriteRecord(record.ID, record.EmrDataId);
            }
        }

        void WriteRecord_NodeDoubleClick(object sender, EventArgs e)
        {
            EMR.Controls.Entity.EmrWriteRecord record = treeT.SelectedNode.Tag as EMR.Controls.Entity.EmrWriteRecord;
            controller.GetEmrWriteRecord(record);
            selectRecordId = record.ID;
        }

        private void 签名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EMR.Controls.Entity.EmrWriteRecord record = treeT.SelectedNode.Tag as EMR.Controls.Entity.EmrWriteRecord;
            EmrBindKeyData _bindkey = new EmrBindKeyData(controller.emrView.CurrBindKeyData.HosptialId, controller.emrView.CurrBindKeyData.PatientId, controller.emrView.CurrBindKeyData.CurrDeptCode, controller.emrView.CurrBindKeyData.CurrDeptName, controller.emrView.CurrBindKeyData.UserCode, controller.emrView.CurrBindKeyData.UserName, controller.emrView.CurrBindKeyData.UserLevel, controller.emrView.CurrBindKeyData.UserLevelName);
            _bindkey.ChangeEmrData(record.ID, record.EmrDataId, record.CatalogueCode, record.DeptCode, record.DeptName, record.UserCode, record.UserName, record.FirstSignature, record.FirstDoctorLevel, record.SecondSignature, record.SecondDoctorCode, record.SecondDoctorLevel, record.ThreeSignature, record.ThreeDoctorCode, record.ThreeDoctorLevel, record.RecordText);
            selectRecordId = record.ID;    

            Signature dlg = new Signature();
            dlg.ShowDialog();
            if (dlg.retOk)
            {
                Bitmap imgSign = dlg.SavedBitmap;
                controller.VerifSignName(_bindkey,imgSign);
            }
        }

        private void 退回ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EMR.Controls.Entity.EmrWriteRecord record = treeT.SelectedNode.Tag as EMR.Controls.Entity.EmrWriteRecord;
            EmrBindKeyData _bindkey = new EmrBindKeyData(controller.emrView.CurrBindKeyData.HosptialId, controller.emrView.CurrBindKeyData.PatientId, controller.emrView.CurrBindKeyData.CurrDeptCode, controller.emrView.CurrBindKeyData.CurrDeptName, controller.emrView.CurrBindKeyData.UserCode, controller.emrView.CurrBindKeyData.UserName, controller.emrView.CurrBindKeyData.UserLevel, controller.emrView.CurrBindKeyData.UserLevelName);
            _bindkey.ChangeEmrData(record.ID, record.EmrDataId, record.CatalogueCode, record.DeptCode, record.DeptName, record.UserCode, record.UserName, record.FirstSignature, record.FirstDoctorLevel, record.SecondSignature, record.SecondDoctorCode, record.SecondDoctorLevel, record.ThreeSignature, record.ThreeDoctorCode, record.ThreeDoctorLevel, record.RecordText);
            selectRecordId = record.ID;    
            controller.VerifSignBack(_bindkey);
        }

        private void treeT_AfterExpand(object sender, DevComponents.AdvTree.AdvTreeNodeEventArgs e)
        {
            if (treeT.Nodes.Find("medemr", false).Length > 0)
            {
                expandnodes = new List<string>();
                DevComponents.AdvTree.Node pnode = treeT.Nodes.Find("medemr", false)[0];
                foreach (DevComponents.AdvTree.Node node in pnode.Nodes)
                {
                    if (node.Expanded && node.Tag != null)
                        expandnodes.Add((node.Tag as EMR.Controls.Entity.EmrCatalogue).NodeCode);
                }
            }
        }

        private void treeT_AfterCollapse(object sender, DevComponents.AdvTree.AdvTreeNodeEventArgs e)
        {
            if (treeT.Nodes.Find("medemr", false).Length > 0)
            {
                expandnodes = new List<string>();
                DevComponents.AdvTree.Node pnode = treeT.Nodes.Find("medemr", false)[0];
                foreach (DevComponents.AdvTree.Node node in pnode.Nodes)
                {
                    if (node.Expanded && node.Tag != null)
                        expandnodes.Add((node.Tag as EMR.Controls.Entity.EmrCatalogue).NodeCode);
                }
            }
        }
    }
}
