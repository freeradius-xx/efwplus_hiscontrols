using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseControls;

namespace EMR.Controls
{
    internal partial class DateToolTip : UserControl, Itooltip
    {
        public DateToolTip()
        {
            InitializeComponent();
        }

        #region Itooltip 成员
        private DateTime _value = DateTime.Now;
        public string EText
        {
            get
            {
                return _value.ToString("yyyy年MM月dd日");
            }
            set
            {
                _value = Convert.ToDateTime(value);
            }
        }
        
        public string EValue
        {
            get
            {
                return _value.ToString();
            }
            set
            {
                _value = Convert.ToDateTime(value);
            }
        }

        public event EventHandler ValueChanged;

        public void LoadData(List<ListItem> datasource, string value)
        {
            if (Tools.IsDateTime(value))
            {
                _value = Convert.ToDateTime(value);
                monthCalendar1.SetDate(_value);
            }
        }

        #endregion

        private void btnok_Click(object sender, EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(sender, e);
            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            _value = e.Start.Date;
        }
    }
}
