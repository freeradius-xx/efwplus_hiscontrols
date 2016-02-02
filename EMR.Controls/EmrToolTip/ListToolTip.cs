using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace EMR.Controls
{
    internal partial class ListToolTip : UserControl, Itooltip
    {
        public ListToolTip()
        {
            InitializeComponent();
            listBox.DisplayMember = "Text";
            listBox.ValueMember = "Value";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush b = new LinearGradientBrush(ClientRectangle, Color.White, BackColor, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(b, ClientRectangle);
            }
            using (Pen p = new Pen(Color.FromArgb(118, 118, 118)))
            {
                Rectangle rect = ClientRectangle;
                rect.Width--;
                rect.Height--;
                e.Graphics.DrawRectangle(p, rect);
            }
        }

        public string EText { get; set; }
        public string EValue { get; set; }

        public event EventHandler ValueChanged;

        public void LoadData(List<ListItem> datasource, string value)
        {
            if (datasource != null)
            {
                listBox.Items.Clear();
                listBox.Items.AddRange(datasource.ToArray());
            }

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                if (value == ((ListItem)listBox.Items[i]).Value.ToString())
                {
                    listBox.SetSelected(i, true);
                }
            }
        }

        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            if (ValueChanged != null)
            {
                EText = listBox.Text;
                EValue = (listBox.SelectedItem as ListItem).Value.ToString();
                ValueChanged(sender, e);
            }
        }
    }

  
}
