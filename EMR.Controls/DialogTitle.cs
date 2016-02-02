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
    public partial class DialogTitle : Office2007Form
    {
        private DialogResult OResult = DialogResult.Cancel;
        //public string TitleText = "";

        public DialogTitle()
        {
            InitializeComponent();
        }

        private void btnconfirm_Click(object sender, EventArgs e)
        {
            OResult = DialogResult.OK;
            this.Close();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            OResult = DialogResult.Cancel;
            this.Close();
        }

        public static DialogResult Show(string tip, ref string titleText)
        {
            DialogTitle dlg = new DialogTitle();
            dlg.Text = tip;
            dlg.txtTitle.Text = titleText;
            dlg.ShowDialog();
            titleText = dlg.txtTitle.Text;
            return dlg.OResult;
        }

        private void txtTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnconfirm.Focus();
        }

        private void DialogTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

    }
}
