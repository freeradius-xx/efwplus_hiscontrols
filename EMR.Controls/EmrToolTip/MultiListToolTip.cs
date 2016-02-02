using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EMR.Controls
{
    internal partial class MultiListToolTip : UserControl, Itooltip
    {
        public MultiListToolTip()
        {
            InitializeComponent();
            (checkedListBox as ListBox).DisplayMember = "Text";
            (checkedListBox as ListBox).ValueMember = "Value";
        }
        public string EText { get; set; }
        public string EValue { get; set; }
        public event EventHandler ValueChanged;

        public void LoadData(List<ListItem> datasource, string value)
        {
            if (datasource != null)
            {
                checkedListBox.Items.Clear();
                checkedListBox.Items.AddRange(datasource.ToArray());
            }

            string[] vals = value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < vals.Length; i++)
            {
                for (int m = 0; m < checkedListBox.Items.Count; m++)
                {
                    if ((checkedListBox.Items[m] as ListItem).Value.ToString() == vals[i])
                    {
                        checkedListBox.SetItemChecked(m, true);
                    }
                }
            }
        }
        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void checkedListBox_Click(object sender, EventArgs e)
        {
            checkedListBox.SetItemChecked(checkedListBox.SelectedIndex, !checkedListBox.GetItemChecked(checkedListBox.SelectedIndex));
            if (ValueChanged != null)
            {
                EValue = "";
                EText = "";
                for (int i = 0; i < checkedListBox.Items.Count; i++)
                {
                    if (checkedListBox.GetItemChecked(i))
                    {
                        EValue += EValue == "" ? "" : ",";
                        EText += EText == "" ? "" : "、";
                        EValue += (checkedListBox.Items[i] as ListItem).Value.ToString();
                        EText += checkedListBox.GetItemText(checkedListBox.Items[i]);
                    }
                }
                if (EText == "")
                    EText = "{未选}";
                ValueChanged(sender, e);
            }
        }
    }
}
