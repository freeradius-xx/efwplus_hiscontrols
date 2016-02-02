using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace EMR.Controls
{
    public partial class SaveAsTemplate : Office2007Form
    {
        public bool isOk = false;
        public int Level = 0;
        public string TemplateText = "";
        public SaveAsTemplate(string text)
        {
            InitializeComponent();
            ckh.Tag = 0;
            ckd.Tag = 1;
            ckp.Tag = 2;

            ckp.Checked = true;
            txtName.Text = text;
           
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnConfirm.Focus();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBoxEx.Show("模板名称不能为空。", "提示", MessageBoxButtons.OK);
                return;
            }
            Level = ckh.Checked ? (int)ckh.Tag : (ckd.Checked ? (int)ckd.Tag : (int)ckp.Tag);
            TemplateText = txtName.Text;
            isOk = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveAsTemplate_Load(object sender, EventArgs e)
        {
            txtName.Focus();
        }
    }
}
