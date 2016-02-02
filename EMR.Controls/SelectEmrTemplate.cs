using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EMR.Controls.Action;

namespace EMR.Controls
{
    public partial class SelectEmrTemplate : Office2007Form
    {
        private emrController controller;
        public bool isOk = false;
        public int EmrDataID = 0;
        public SelectEmrTemplate(emrController _controller)
        {
            InitializeComponent();
            ckh.Tag = 0;
            ckd.Tag = 1;
            ckp.Tag = 2;
            controller = _controller;
            gridList.DataSource = controller.CallTemplateData(0);
        }
        private void ckh_CheckedChanged(object sender, EventArgs e)
        {
            if (((DevComponents.DotNetBar.Controls.CheckBoxX)sender).Checked)
            {
                int level = ckh.Checked ? (int)ckh.Tag : (ckd.Checked ? (int)ckd.Tag : (int)ckp.Tag);
                gridList.DataSource = controller.CallTemplateData(level);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (gridList.CurrentCell != null)
            {
                EmrDataID = Convert.ToInt32((gridList.DataSource as DataTable).Rows[gridList.CurrentCell.RowIndex]["EmrDataID"]);
                isOk = true;
                this.Close();
            }
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            btnConfirm_Click(null, null);
        }
    }
}
