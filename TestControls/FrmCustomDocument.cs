using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestControls
{
    public partial class FrmCustomDocument : Form
    {
        public FrmCustomDocument()
        {
            InitializeComponent();

            //1.customDocumentControl1的父控件的AutoScroll属性在设计界面设置为true
            //2.Dock布局必须代码设置，因为在设计界面设置的话，滚动条无法拖动
            this.customDocumentControl1.Dock = DockStyle.Fill;
        }

        private void customDocumentControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
