using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Prescription.Controls;

namespace TestControls
{
    public partial class FrmPrescription : Form
    {
        public FrmPrescription()
        {
            InitializeComponent();
        }

        private void FrmPrescription_Load(object sender, EventArgs e)
        {
            List<ToolStripMenuItem> listmenu = new List<ToolStripMenuItem>();
            ToolStripMenuItem menu1 = new ToolStripMenuItem();
            menu1.Name = "menu1";
            menu1.Text = "本院注射（1次）";
            menu1.Click += new EventHandler(menu1_Click);
            listmenu.Add(menu1);
            ToolStripMenuItem menu2 = new ToolStripMenuItem();
            menu2.Name = "menu2";
            menu2.Text = "本院注射（2次）";
            menu2.Click += new EventHandler(menu1_Click);
            listmenu.Add(menu2);
            //自定义添加右键菜单
            ((IPrescription)this.prescriptionControl1).AddContextMenu(listmenu);

            
            prescriptionControl1.HideColName = new string[] {"Dept_Name"};
            prescriptionControl1.PrescriptionStyle = PrescriptionStyle.西药与中成药;
            //初始化处方控件并加载病人处方数据
            TestPrescripttionDbHelper presHelper = new TestPrescripttionDbHelper();
            prescriptionControl1.InitDbHelper(presHelper);
            prescriptionControl1.LoadPatData(1, 101, "儿科", 201, "李医生");
        }

        void menu1_Click(object sender, EventArgs e)
        {
            MessageBox.Show((sender as ToolStripMenuItem).Text);
        }
        //用法联动费用
        private void prescriptionControl1_Costoflinkage(int patListId)
        {

        }
        //打印处方
        private void prescriptionControl1_SinglePresPrint(int patListId, int presType, int presNo)
        {

        }
    }
}
