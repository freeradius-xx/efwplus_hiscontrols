using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using System.IO;
using System.Xml;
using EMR.Controls.Action;
using EMR.Controls.Properties;

namespace EMR.Controls
{

    public partial class WriteHelper : Office2007Form
    {
        private emrController controller;

        public WriteHelper(emrController _controller)
        {
            InitializeComponent();

            controller = _controller;

            //知识库
            txtCollection.TextChanged += new EventHandler(txtCollection_TextChanged);
            TreeElement.NodeDoubleClick += new TreeNodeMouseEventHandler(TreeElement_NodeDoubleClick);
            //通用病历
            TreeTYBL.NodeDoubleClick += new TreeNodeMouseEventHandler(TreeTYBL_NodeDoubleClick);
            //数据源
            txtDict.TextChanged += new EventHandler(txtDict_TextChanged);
            TreeDict.NodeDoubleClick += new TreeNodeMouseEventHandler(TreeDict_NodeDoubleClick);
            //符号
            cmbSymbolClass.SelectedIndexChanged += new EventHandler(cmbSymbolClass_SelectedIndexChanged);
        }

      

        private void WriteHelper_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(Screen.AllScreens[0].Bounds.Width- this.Width - 20, 250);

            #region 知识库
            //DataView dvElement = new DataView(emrbasedata1);
            TreeElement.Nodes.Clear();
            XmlNodeList xnlist = KnowledgeManage.Knowledge_xd.DocumentElement.SelectNodes("KBEntries/Entry");
            CreateElementTree(TreeElement.Nodes, xnlist);
            #endregion

            #region 通用模板
            TreeTYBL.Nodes.Clear();
            if (rtemp1.Checked)
            {
                CreateGeneralEmrTree_ParagraphTemplate();
            }
            else
            {
                CreateGeneralEmrTree_EMRTemplate();
            }
            #endregion

            #region 绑定数据源
            TreeDict.Nodes.Clear();
            CreateDataSourceTree();
            #endregion

            #region 绑定符号分类
            XmlNodeList xnlist_sc = KnowledgeManage.SpecialCharacter_xd.DocumentElement.SelectNodes("signclass/class");
            DataTable dt_sc = new DataTable();
            dt_sc.Columns.Add("id", typeof(String));
            dt_sc.Columns.Add("name", typeof(String));
            foreach (XmlNode xn in xnlist_sc)
            {
                DataRow dr= dt_sc.NewRow();
                dr["id"] = xn.Attributes["id"].Value;
                dr["name"] = xn.Attributes["name"].Value;
                dt_sc.Rows.Add(dr);
            }

            XmlNodeList xnlist_sl = KnowledgeManage.SpecialCharacter_xd.DocumentElement.SelectNodes("signdata/sigin");
            DataTable dt_sl = new DataTable();
            dt_sl.Columns.Add("ID", typeof(String));
            dt_sl.Columns.Add("ClassID", typeof(String));
            dt_sl.Columns.Add("SymbolText", typeof(String));
            foreach (XmlNode xn in xnlist_sl)
            {
                DataRow dr = dt_sl.NewRow();
                dr["ID"] = xn.SelectSingleNode("ID").InnerText;
                dr["ClassID"] = xn.SelectSingleNode("ClassID").InnerText;
                dr["SymbolText"] = xn.SelectSingleNode("SymbolText").InnerText;
                dt_sl.Rows.Add(dr);
            }

            cmbSymbolClass.DisplayMember = "name";
            cmbSymbolClass.ValueMember = "id";
            cmbSymbolClass.Tag = dt_sl;
            cmbSymbolClass.DataSource = dt_sc;
            cmbSymbolClass.SelectedIndex = 0;
            #endregion

        }

        #region 知识库
        private void CreateElementTree(NodeCollection nodes, XmlNodeList xnlist)
        {
            foreach (XmlNode xn in xnlist)
            {
                string ID = xn.SelectSingleNode("ID") == null ? "" : xn.SelectSingleNode("ID").InnerText;
                string Text = xn.SelectSingleNode("Text") == null ? "" : xn.SelectSingleNode("Text").InnerText;
                string Value = xn.SelectSingleNode("Value") == null ? "" : xn.SelectSingleNode("Value").InnerText;
                string Style = xn.SelectSingleNode("Style") == null ? "" : xn.SelectSingleNode("Style").InnerText;
                Node node = new Node();
                node.Text = Convert.ToString(Text);
                nodes.Add(node);
                if (xn.SelectSingleNode("SubEntries") != null)
                {
                    node.Image = Resources.FolderClosed;
                    node.ImageExpanded = Resources.FolderOpen;
                    CreateElementTree(node.Nodes, xn.SelectSingleNode("SubEntries").ChildNodes);
                }
                else
                {
                    node.Image = Resources.Document;
                    node.ImageExpanded = Resources.Document;
                    node.Tag = new string[] { ID, Text, Value, Style };
                }
            }

        }

        private void TreeElement_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            if (TreeElement.SelectedNode != null && TreeElement.SelectedNode.Tag != null)
            {
                string[] data = (string[])TreeElement.SelectedNode.Tag;
                string inputtype = data[3] == "List" ? "1" : "2";
                controller.emrView.EmrInsertText(TreeElement.SelectedNode.Text);
                controller.emrView.EmrInsertDomain("{" + TreeElement.SelectedNode.Text + "}","", "1", data[2], inputtype);
            }
        }


        private void txtCollection_TextChanged(object sender, EventArgs e)
        {
            FindElementNode(TreeElement.Nodes);
       }

        private void FindElementNode(NodeCollection collection)
        {
            String sFilterText = txtCollection.Text.Trim().ToUpper();
            foreach (Node n in collection)
            {
                String[] sCode = n.TagString.Split('|');
                if (n.Text.Contains(sFilterText) || sFilterText == "" )
                {
                    n.Visible = true;
                    NodePatientVisable(n);
                }
                else
                    n.Visible = false;
                FindElementNode(n.Nodes);
            }
        }

        private void NodePatientVisable(Node n)
        {
            if (n != null && n.Parent != null)
            {
                n.Parent.Visible = true;
                NodePatientVisable(n.Parent);
            }
        }
        #endregion

        #region 通用病历
        void CreateGeneralEmrTree_ParagraphTemplate()
        {
            TreeTYBL.Nodes.Clear();
            NodeCollection nodes = TreeTYBL.Nodes;
            XmlNodeList xnlist = KnowledgeManage.GeneralEmr_xd.DocumentElement.SelectNodes("ParagraphTemplate/templateNode");
            if (xnlist != null)
            foreach (XmlNode xn in xnlist)
            {
                string id = xn.Attributes["id"].Value;
                string name = xn.Attributes["name"].Value;
                Node node = new Node();
                node.Text = name;
                node.Image = Resources.FolderClosed;
                node.ImageExpanded = Resources.FolderOpen;
                nodes.Add(node);
                XmlNodeList txnlist = xn.SelectNodes("templatePara");
                if (txnlist != null)
                {
                    foreach (XmlNode txn in txnlist)
                    {
                        string tid = txn.Attributes["id"].Value;
                        string tname = txn.Attributes["name"].Value;
                        string ttext = txn.InnerText;
                        Node tnode = new Node();
                        tnode.Image = Resources.Document;
                        tnode.ImageExpanded = Resources.Document;
                        tnode.Text = tname;
                        tnode.Tag = new string[] { tid, tname, ttext };
                        node.Nodes.Add(tnode);
                    }
                }
            }
        }
        void CreateGeneralEmrTree_EMRTemplate()
        {
            TreeTYBL.Nodes.Clear();
            NodeCollection nodes = TreeTYBL.Nodes;
            XmlNodeList xnlist = KnowledgeManage.GeneralEmr_xd.DocumentElement.SelectNodes("EMRTemplate/deptClass");
            if (xnlist != null)
            foreach (XmlNode xn in xnlist)
            {
                string id = xn.Attributes["id"].Value;
                string name = xn.Attributes["name"].Value;
                Node node = new Node();
                node.Text = name;
                node.Image = Resources.FolderClosed;
                node.ImageExpanded = Resources.FolderOpen;
                nodes.Add(node);
                XmlNodeList txnlist = xn.SelectNodes("templateNode");
                if (txnlist != null)
                {
                    foreach (XmlNode txn in txnlist)
                    {
                        string tid = txn.Attributes["id"].Value;
                        string tname = txn.Attributes["name"].Value;
                        Node tnode = new Node();
                        tnode.Image = Resources.FolderClosed;
                        tnode.ImageExpanded = Resources.FolderOpen;
                        tnode.Text = tname;
                        node.Nodes.Add(tnode);

                        XmlNodeList fxnlist = txn.SelectNodes("templateFull");
                        if (fxnlist != null)
                        {
                            foreach (XmlNode fxn in fxnlist)
                            {
                                string fname = fxn.Attributes["name"].Value;
                                string filepath = fxn.Attributes["filepath"].Value;
                                Node fnode = new Node();
                                fnode.Image = Resources.Document;
                                fnode.ImageExpanded = Resources.Document;
                                fnode.Text = fname;
                                fnode.Tag = filepath;
                                tnode.Nodes.Add(fnode);
                            }
                        }
                    }
                }
            }
        }
        void TreeTYBL_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            if (rtemp1.Checked)
            {
                if (TreeTYBL.SelectedNode != null && TreeTYBL.SelectedNode.Tag != null)
                {
                    string[] data = (string[])TreeTYBL.SelectedNode.Tag;
                    controller.emrView.EmrInsertDomain(data[1], data[0], true);
                    controller.emrView.EmrInsertText(data[2]);
                }
            }
            else
            {
                if (TreeTYBL.SelectedNode != null && TreeTYBL.SelectedNode.Tag != null)
                {
                    string filename = (string)TreeTYBL.SelectedNode.Tag;

                    FileStream fs = new FileStream(KnowledgeManage.emrdatapath+"\\"+filename, FileMode.OpenOrCreate, FileAccess.Read);
                    BinaryReader bReader = new BinaryReader(fs);
                    byte[] bBytes = bReader.ReadBytes((int)fs.Length);
                    bReader.Close();
                    fs.Close();

                    controller.emrView.LoadEmrText(bBytes);
                }
            }
        }
        #endregion

        #region 数据源
        void CreateDataSourceTree()
        {
            NodeCollection nodes = TreeDict.Nodes;
            XmlNodeList xnlist = KnowledgeManage.DataSource_xd.DocumentElement.SelectNodes("DataClass");
            if (xnlist != null)
            {
                foreach (XmlNode xn in xnlist)
                {
                    string id = xn.Attributes["id"].Value;
                    string name = xn.Attributes["name"].Value;
                    Node node = new Node();
                    node.Text = name;
                    node.Image = Resources.FolderClosed;
                    node.ImageExpanded = Resources.FolderOpen;
                    nodes.Add(node);
                    XmlNodeList txnlist = xn.SelectNodes("Value");
                    if (txnlist != null)
                    {
                        foreach (XmlNode txn in txnlist)
                        {
                            string tid = txn.Attributes["id"].Value;
                            string tname = txn.Attributes["name"].Value;
                            string tvalue = txn.Attributes["value"].Value;
                            string tinputtype = txn.Attributes["inputtype"].Value;

                            Node tnode = new Node();
                            tnode.Image = Resources.Document;
                            tnode.ImageExpanded = Resources.Document;
                            tnode.Text = tname;
                            tnode.Tag = new string[] { tid, tname, tvalue, tinputtype };
                            node.Nodes.Add(tnode);
                        }
                    }
                }
            }
        }
        private void txtDict_TextChanged(object sender, EventArgs e)
        {
            FindDictNode(TreeDict.Nodes);
        }

        //双击数据源
        private void TreeDict_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            if (TreeDict.SelectedNode != null && TreeDict.SelectedNode.Tag != null)
            {
                string[] data = (string[])TreeDict.SelectedNode.Tag;
                controller.emrView.EmrInsertDomain("{" + data[1] + "}","", "2", data[0], data[3]);
            }
        }

        private void FindDictNode(NodeCollection collection)
        {
            foreach (Node n in collection)
            {
                if (n.Text.Contains(txtDict.Text.Trim()) || txtDict.Text.Trim() == "")
                {
                    n.Visible = true;
                    NodePatientVisable(n);
                }
                else
                    n.Visible = false;
                FindDictNode(n.Nodes);
            }
        }
        #endregion

        #region 符号分类
        private void btnSymbol_Click(object sender, EventArgs e)
        {
            controller.emrView.EmrInsertText((sender as ButtonX).Text);
        }

        private void cmbSymbolClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSymbolClass.SelectedValue != null)
            {
                flpSymbol.Controls.Clear();
 
                DataView dictSymbol = new DataView((DataTable)cmbSymbolClass.Tag);
                dictSymbol.RowFilter = "ClassID=" + Convert.ToString(cmbSymbolClass.SelectedValue);

                foreach (DataRowView dr in dictSymbol)
                {
                    ButtonX btnSymbol = new ButtonX();
                    btnSymbol.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
                    btnSymbol.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
                    btnSymbol.Text = dr["SymbolText"].ToString();
                    btnSymbol.Tooltip = dr["SymbolText"].ToString();
                    btnSymbol.Cursor = Cursors.Hand;
                    btnSymbol.Shape = new RoundRectangleShapeDescriptor();
                    btnSymbol.Size = new Size(23, 23);
                    btnSymbol.FocusCuesEnabled = false;
                    btnSymbol.TabStop = false;
                    btnSymbol.Click += new EventHandler(btnSymbol_Click);
                    flpSymbol.Controls.Add(btnSymbol);
                }
            }
        }
        #endregion

        private void rtemp1_CheckedChanged(object sender, EventArgs e)
        {

            if (rtemp1.Checked)
                CreateGeneralEmrTree_ParagraphTemplate();
            else
                CreateGeneralEmrTree_EMRTemplate();
        }

    }
}
