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
    public partial class WriteDomain : Office2007Form
    {
        public string domainText = "";
        public string domainValue = "";
        public string domainId = string.Empty;
        public int domainType = (int)InputType.None;

        public WriteDomain()
        {
            InitializeComponent();
            DialogResult = DialogResult.No;
        }
        //双击列表
        private void listdomain_DoubleClick(object sender, EventArgs e)
        {
            switch (listdomain.Text)
            {
                case "当前页码":
                    domainText = "{第X页}";
                    domainId = "Gpage";
                    domainType = 3;
                    break;
                case "总页数":
                    domainText = "{共X页}";
                    domainId = "Gpcount";
                    domainType = 3;
                    break;
                case "系统时间":
                    domainText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    domainId = "SysTime";
                    domainType = 5;
                    break;
                case "系统日期":
                    domainText = DateTime.Now.ToString("yyyy年MM月dd日");
                    domainId = "SysDate";
                    domainType = 4;
                    break;
                case "文本时间":
                    domainText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //domainId = "SysTime";
                    //domainType = 5;
                    break;
                case "文本日期":
                    domainText = DateTime.Now.ToString("yyyy年MM月dd日");
                    //domainId = "SysDate";
                    //domainType = 4;
                    break;
                case "打印时间":
                    break;
                case "复选框(空)":
                    domainText = "o";
                    domainId = "SysTrueOrFalse";
                    domainType = 6;
                    domainValue = "-1";
                    break;
                case "复选框(True)":
                    domainText = "R";
                    domainId = "SysTrueOrFalse";
                    domainType = 6;
                    domainValue = "1";
                    break;
                case "复选框(False)":
                    domainText = "T";
                    domainId = "SysTrueOrFalse";
                    domainType = 6;
                    domainValue = "0";
                    break;
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
