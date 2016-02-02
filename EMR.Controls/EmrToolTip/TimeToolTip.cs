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
    internal partial class TimeToolTip : UserControl, Itooltip
    {
        public TimeToolTip()
        {
            InitializeComponent();
        }

        #region Itooltip 成员
        private DateTime _value=DateTime.Now;
        public string EText
        {
            get
            {
                return _value.ToString("yyyy-MM-dd HH:mm:ss");
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
                dateTimePicker1.Value = _value;
            }
        }

        #endregion

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            _value = new DateTime(e.Start.Date.Year, e.Start.Date.Month, e.Start.Date.Day, _value.Hour, _value.Minute, _value.Second);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            _value = new DateTime(_value.Year, _value.Month, _value.Day, dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute, dateTimePicker1.Value.Second);
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(sender, e);
            }
        }
    }
}
