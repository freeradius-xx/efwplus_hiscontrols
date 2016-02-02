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
    public partial class FrmEmrRecord3 : Form
    {
        public FrmEmrRecord3()
        {
            InitializeComponent();
        }

        private void FrmEmrRecord3_Load(object sender, EventArgs e)
        {
            emrRecord1.InitLoad(new TestEmrWriteDbHelper(), new EMR.Controls.Action.EmrBindKeyData(1, 1, "001", "儿科", "01", "张医生",1,"住院医师"), false);
        }
    }
}
