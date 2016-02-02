using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BedCard.Controls;

namespace TestControls
{
    public partial class FrmBedCard : Form
    {
        public FrmBedCard()
        {
            InitializeComponent();
            toolStripButton3_Click(null, null);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //第一步：添加床头卡要显示的内容项
            bedCardControl1.BedContextFields = new List<ContextField>();
            bedCardControl1.BedContextFields.Add(new ContextField("住院号", "PatientNum"));
            bedCardControl1.BedContextFields.Add(new ContextField("结 算", "Diet"));
            //bedCardControl1.BedContextFields.Add(new ContextField("诊 断", "Diagnosis"));
            bedCardControl1.BedContextFields.Add(new ContextField("科 室", "Dept"));
            bedCardControl1.BedContextFields.Add(new ContextField("医 生", "Doctor"));
            bedCardControl1.BedContextFields.Add(new ContextField("入 科", "EnterTime"));
            //other
            bedCardControl1.BedContextFields.Add(new ContextField("其 他", "other"));

            //第二步：获取病人数据，数据必须是List集合
            List<BedInfo> list = new List<BedInfo>();

            for (int i = 0; i < 20; i++)//前20病人显示信息
            {
                //支持自定义床头卡的内容显示
                CustomPatientInfo bed = new CustomPatientInfo();
                bed.BedNo = i.ToString();
                bed.PatientID = i + 1;
                bed.PatientNum = "0000012" + i;
                bed.PatientName = "昂三" + i;
                if (i % 3 == 0)
                    bed.Sex = "男";
                else
                    bed.Sex = "女";
                bed.Age = "11岁";
                bed.Diet = "计算";
                bed.Diagnosis = "诊断是的安定和水电费水电费和、安定和水电费水电费和";
                bed.Dept = "妇产科";
                bed.Doctor = "李医生";
                bed.Nurse = "03";
                bed.EnterTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                bed.other = "!@#$%^&";
                list.Add(bed);
            }
            //后面的显示空床位
            for (int i = 20; i < 34; i++)
            {
                BedInfo bed = new BedInfo();
                bed.BedNo = i.ToString();
                list.Add(bed);
            }
            //第三步：将病人数据绑定到数据源上显示
            bedCardControl1.DataSource = list;
        }

        private void bedCardControl1_BedDoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("双击了：" + bedCardControl1.SelectedBed.PatientName);
        }

        private void bedCardControl1_BedClick(object sender, EventArgs e)
        {
            //MessageBox.Show("单击了：" + bedCardControl1.SelectedBed.PatientName);
        }

        private void bedCardControl1_BedTitleClick(object sender, BedCardClickEventArgs e)
        {
            MessageBox.Show("点击了床号：" + bedCardControl1.SelectedBed.BedNo);
        }

        private void bedCardControl1_HeadPageClick(object sender, BedCardClickEventArgs e)
        {
            MessageBox.Show("点击了首页：" + bedCardControl1.SelectedBed.BedNo);
        }

        private void bedCardControl1_TemperatureClick(object sender, BedCardClickEventArgs e)
        {
            MessageBox.Show("点击了体温单：" + bedCardControl1.SelectedBed.BedNo);
        }

        private void bedCardControl1_AdviceClick(object sender, BedCardClickEventArgs e)
        {
            MessageBox.Show("点击了医嘱：" + bedCardControl1.SelectedBed.BedNo);
        }

        private void bedCardControl1_ApplyFormClick(object sender, BedCardClickEventArgs e)
        {
            MessageBox.Show("点击了申请单：" + bedCardControl1.SelectedBed.BedNo);
        }

        private void bedCardControl1_BedFormatStyleEvent(BedInfo bed, string dataPropertyName, ref Font contextFont, ref Brush contextBrush)
        {
            if (bed.PatientID % 3 == 1 && dataPropertyName == "PatientNum")
            {
                contextFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                contextBrush = new SolidBrush(Color.Red);
            }
        }
    }
}
