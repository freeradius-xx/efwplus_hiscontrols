using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MRecordFirst.Controls;
using Newtonsoft.Json;

namespace TestControls
{
    public partial class FrmMRecordFirst : Form
    {
        public FrmMRecordFirst()
        {
            InitializeComponent();
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            if (txtJsonData.Text == "")
            {
                mRecordFirstBrower1.ShowView(null);
            }
            else
            {
                List<JsonData> list = MRecordFirstBrower.ToList<JsonData>(txtJsonData.Text);
                mRecordFirstBrower1.ShowView(list);
            }
        }

        private void btnGetJson_Click(object sender, EventArgs e)
        {
            List<JsonData> list = mRecordFirstBrower1.GetViewData();
            txtJsonData.Text = JavaScriptConvert.SerializeObject(list);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            mRecordFirstBrower1.PrintPreview();
        }
    }
}
