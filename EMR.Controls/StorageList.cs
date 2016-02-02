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
    public partial class StorageList : Office2007Form
    {
        private emrController controller;
        public bool isOk = false;
        public int EmrDataID = 0;
        public StorageList(emrController _controller)
        {
            controller = _controller;
            InitializeComponent();
            begindate.Value = DateTime.Now.AddDays(-7);
            enddate.Value = DateTime.Now;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            gridList.DataSource = controller.SearchStorageList(begindate.Value, enddate.Value);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            btnConfirm_Click(null, null);
        }

        private void StorageList_Load(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }
    }
}
