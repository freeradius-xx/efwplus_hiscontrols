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
    public partial class FrmDemo : Form
    {
        public FrmDemo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmEmrRecord femr = new FrmEmrRecord();
            femr.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmBedCard fbed = new FrmBedCard();
            fbed.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmPrescription fpres = new FrmPrescription();
            fpres.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmCustomDocument fcd = new FrmCustomDocument();
            fcd.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            FrmEmrRecord2 fer = new FrmEmrRecord2();
            fer.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FrmEmrRecord3 fer = new FrmEmrRecord3();
            fer.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmTemperature frm = new FrmTemperature();
            frm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FrmMRecordFirst frm = new FrmMRecordFirst();
            frm.ShowDialog();
        }
    }
}
