using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using GreatHIS.Controls.CommonMvc;

namespace GreatHIS.Controls.CommonControl
{
    public partial class BirthdayText : UserControl
    {
        [Description("出生日期")]
        public DateTime AgeBirthday
        {
            get
            {
                try
                {
                    //string strdate = birth.Text.PadRight(16, '0').Replace(' ', '0');
                    //strdate = strdate.Substring(0, 10) + " " + strdate.Substring(11, 5);
                    //return Convert.ToDateTime(strdate);
                    return birth.Value;
                }
                catch
                {
                    return DateTime.Now;
                }
            }
            set
            {
                //this.birth.Text = value.ToString("yyyyMMddHHmm");
                this.birth.Value = value;
            }
        }

        [Description("内容")]
        public string BirthText
        {
            get
            {
                try
                {
                    //string strdate = birth.Text.PadRight(16, '0').Replace(' ', '0');
                    //strdate = strdate.Substring(0, 10) + " " + strdate.Substring(11, 5);

                    //return Convert.ToDateTime(strdate).ToString("yyyy-MM-dd HH:mm");
                    return birth.Value.ToString("yyyy-MM-dd HH:mm");
                }
                catch
                {
                    return "";
                }
            }
        }

        private string _agestring;
        [Description("年龄")]
        public string AgeString
        {
            get
            {
                return AgeExtend.GetAgeValue(AgeBirthday).ReturnAgeStr_EN();
            }
        }

        public BirthdayText()
        {
            InitializeComponent();
           
            popup1.AddPopupPanel(birth, panelEx1, PopupEvent.Click, this.Width, 110);
            this.Height = 23;

            Init();
        }

        private void Init()
        {
            this.birth.Enter += new System.EventHandler(this.birth_Enter);
            this.birth.Leave += new EventHandler(birth_Leave);
            //this.birth.ButtonCustomClick += new System.EventHandler(this.birth_ButtonCustomClick);
            //this.birth.Click += new System.EventHandler(this.birth_Click);

            this.birth.ValueChanged += new System.EventHandler(this.birth_TextChanged);


            this.txtYear.Enter += new EventHandler(txtAge_Click);
            this.txtMonth.Enter += new EventHandler(txtAge_Click);
            this.txtDay.Enter += new EventHandler(txtAge_Click);
            this.txtHour.Enter += new EventHandler(txtAge_Click);

            this.txtYear.TextChanged += new EventHandler(txtAge_TextChanged);
            this.txtMonth.TextChanged += new EventHandler(txtAge_TextChanged);
            this.txtDay.TextChanged += new EventHandler(txtAge_TextChanged);
            this.txtHour.TextChanged += new EventHandler(txtAge_TextChanged);

            //this.birth.Text = DateTime.Now.AddYears(-20).ToString("yyyyMMddHHmm");

            this.birth.KeyDown += new KeyEventHandler(birth_KeyDown);
            this.txtYear.KeyDown += new KeyEventHandler(birth_KeyDown);
            this.txtMonth.KeyDown += new KeyEventHandler(birth_KeyDown);
            this.txtDay.KeyDown += new KeyEventHandler(birth_KeyDown);
            this.txtHour.KeyDown += new KeyEventHandler(birth_KeyDown);

            this.txtYear.KeyPress += new KeyPressEventHandler(age_KeyPress);
            this.txtMonth.KeyPress += new KeyPressEventHandler(age_KeyPress);
            this.txtDay.KeyPress += new KeyPressEventHandler(age_KeyPress);
            this.txtHour.KeyPress += new KeyPressEventHandler(age_KeyPress);
        }

        void age_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8)
                e.Handled = true;
        }

        void txtAge_TextChanged(object sender, EventArgs e)
        {
            this.birth.ValueChanged -= new EventHandler(birth_TextChanged);
            //if (sender.Equals(txtYear) == false)
            this.txtYear.TextChanged -= new EventHandler(txtAge_TextChanged);
            //if (sender.Equals(txtMonth) == false)
            this.txtMonth.TextChanged -= new EventHandler(txtAge_TextChanged);
            //if (sender.Equals(txtDay) == false)
            this.txtDay.TextChanged -= new EventHandler(txtAge_TextChanged);
            //if (sender.Equals(txtHour) == false)
            this.txtHour.TextChanged -= new EventHandler(txtAge_TextChanged);

            if (sender.Equals(txtYear))
            {
                this.txtMonth.Text = "0";
                this.txtDay.Text = "0";
                this.txtHour.Text = "0";
            }
            if (sender.Equals(txtMonth))
            {
                this.txtYear.Text = "0";
                this.txtDay.Text = "0";
                this.txtHour.Text = "0";
            }
            if (sender.Equals(txtDay))
            {
                this.txtMonth.Text = "0";
                this.txtYear.Text = "0";
                this.txtHour.Text = "0";
            }
            if (sender.Equals(txtHour))
            {
                this.txtMonth.Text = "0";
                this.txtDay.Text = "0";
                this.txtYear.Text = "0";
            }


            AgeValue value = new AgeValue();
            value.Y_num = ConvertDataExtend.ToInt32(txtYear.Text, 0);
            value.M_num = ConvertDataExtend.ToInt32(txtMonth.Text, 0);
            value.D_num = ConvertDataExtend.ToInt32(txtDay.Text, 0);
            value.H_num = ConvertDataExtend.ToInt32(txtHour.Text, 0);

            //this.birth.Text = AgeExtend.GetDateTime(value).ToString("yyyyMMddHHmm");
            this.birth.Value = AgeExtend.GetDateTime(value);

            //if (sender.Equals(txtYear)==false)
            this.txtYear.TextChanged += new EventHandler(txtAge_TextChanged);
            //if (sender.Equals(txtMonth) == false)
            this.txtMonth.TextChanged += new EventHandler(txtAge_TextChanged);
            //if (sender.Equals(txtDay) == false)
            this.txtDay.TextChanged += new EventHandler(txtAge_TextChanged);
            //if (sender.Equals(txtHour) == false)
            this.txtHour.TextChanged += new EventHandler(txtAge_TextChanged);
            this.birth.ValueChanged += new EventHandler(birth_TextChanged);
        }

        void txtAge_Click(object sender, EventArgs e)
        {
            ((DevComponents.DotNetBar.Controls.TextBoxX)sender).Focus();
            ((DevComponents.DotNetBar.Controls.TextBoxX)sender).SelectAll();
        }

        private void birth_ButtonCustomClick(object sender, EventArgs e)
        {
            popup1.Show(birth, this.Width, 110);
        }

        private void birth_TextChanged(object sender, EventArgs e)
        {
            //this.birth.TextChanged -= new EventHandler(birth_TextChanged);
            this.txtYear.TextChanged -= new EventHandler(txtAge_TextChanged);
            this.txtMonth.TextChanged -= new EventHandler(txtAge_TextChanged);
            this.txtDay.TextChanged -= new EventHandler(txtAge_TextChanged);
            this.txtHour.TextChanged -= new EventHandler(txtAge_TextChanged);

            AgeValue value = AgeExtend.GetAgeValue(AgeBirthday);
            txtYear.Text = value.Y_num.ToString();
            txtMonth.Text = value.M_num.ToString();
            txtDay.Text = value.D_num.ToString();
            txtHour.Text = value.H_num.ToString();

            this.txtYear.TextChanged += new EventHandler(txtAge_TextChanged);
            this.txtMonth.TextChanged += new EventHandler(txtAge_TextChanged);
            this.txtDay.TextChanged += new EventHandler(txtAge_TextChanged);
            this.txtHour.TextChanged += new EventHandler(txtAge_TextChanged);
            //this.birth.TextChanged += new EventHandler(birth_TextChanged);
        }

        private void birth_Enter(object sender, EventArgs e)
        {
            popup1.Show(birth, this.Width, 110);
        }
        void birth_Leave(object sender, EventArgs e)
        {
            if (txtYear.Focused || txtMonth.Focused || txtDay.Focused || txtHour.Focused)
                return;
            popup1.Hide();
        }
        private void birth_Click(object sender, EventArgs e)
        {
            birth.Focus();
            popup1.Show(birth, this.Width, 110);
        }

        void birth_KeyDown(object sender, KeyEventArgs e)
        {
            string name = ((Control)sender).Name;
            if (e.KeyCode == Keys.Up)
            {
                if (name == "birth")
                    return;
                else if (name == "txtYear")
                    birth.Focus();
                else if (name == "txtMonth")
                    txtYear.Focus();
                else if (name == "txtDay")
                    txtMonth.Focus();
                else if (name == "txtHour")
                    txtDay.Focus();
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (name == "birth")
                    txtYear.Focus();
                else if (name == "txtYear")
                    txtMonth.Focus();
                else if (name == "txtMonth")
                    txtDay.Focus();
                else if (name == "txtDay")
                    txtHour.Focus();
                else if (name == "txtHour")
                    birth.Focus();
            }
            else if (e.KeyCode == Keys.Space)
            {
                if (name == "birth")
                    txtYear.Focus();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (name != "birth")
                {
                    birth.Focus();
                    popup1.Hide();
                }
            }

         
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            birth.Height = Height;
            base.OnSizeChanged(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            //MessageBox.Show("ok");

            base.OnKeyUp(e);
        }
    }
}
