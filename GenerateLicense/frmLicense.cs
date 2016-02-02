using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenerateLicense
{
    public partial class frmLicense : Form
    {
        public frmLicense()
        {
            InitializeComponent();

            DataTable dt = new DataTable();
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("key", typeof(string));

            DataRow dr = dt.NewRow();
            dr["name"] = "EmrRecord";
            dr["key"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["name"] = "PrescriptionControl";
            dr["key"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["name"] = "Advice.Controls";
            dr["key"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["name"] = "BedCardControl";
            dr["key"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["name"] = "CustomDocumentControl";
            dr["key"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["name"] = "MRecordFirst.Controls";
            dr["key"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["name"] = "Pricing.Controls";
            dr["key"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["name"] = "Temperature.Controls";
            dr["key"] = "";
            dt.Rows.Add(dr);

            dataGrid.DataSource = dt;
            dateTP.Value = DateTime.Now;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string date = dateTP.Value.ToString("yyyy-MM-dd");
            DataTable dt = (DataTable)dataGrid.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string name = dt.Rows[i]["name"].ToString();
                name = name.Length < 8 ? name.PadRight(8, '*') : name;
                BaseControls.DESEncryptor des = new BaseControls.DESEncryptor(name, name);
                des.InputString = date;
                des.DesEncrypt();
                dt.Rows[i]["key"] = des.OutString;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (dataGrid.CurrentCell != null)
            {
                string key = ((DataTable)dataGrid.DataSource).Rows[dataGrid.CurrentCell.RowIndex]["key"].ToString();
                if (key == "")
                {
                    MessageBox.Show("请先生成注册码！");
                    return;
                }
                Clipboard.SetText(key);
                MessageBox.Show("已复制注册码：" + key);
            }
        }
    }
}
