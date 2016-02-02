using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace EMR.Controls
{
    public partial class FrmTableColRow : Office2007Form
    {
        public int RowCount
        {
            get { return txtRowCount.Value; }
        }

        public int ColumnCount
        {
            get { return txtColColumn.Value; }
        }

        public FrmTableColRow()
        {
            InitializeComponent();

            btnOk.Click += new EventHandler(btnOk_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
