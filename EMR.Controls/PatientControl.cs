using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace BedCard.Controls
{
    public partial class PatientControl : UserControl
    {
        Dictionary<string, string> paramDic;

        public PatientControl()
        {
            InitializeComponent();
            paramDic = new Dictionary<string, string>();
            DefaultParams();
        }
        
        /// <summary>
        /// 默认数据
        /// </summary>
        public void DefaultParams()
        {
            AddParams("•床位", "Bed");
            AddParams("住院号", "PatientNum");
            AddParams("姓名", "Name");
            AddParams("性别", "Sex");
            AddParams("年龄", "Age");
            AddParams("•入院科室", "EnterDeptCode");
            AddParams("入院时间", "RegisterDate");
            AddParams("主管医生", "InDoctor");
            //AddParams("主管护士", "InDoctor");
            AddParams("•总费用", "totalMoney");
            AddParams("预交押金", "prepaidMoney");
            AddParams("余额", "balanceMoney");
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="fieldname">字段名称</param>
        public void AddParams(string title, string fieldname)
        {
            paramDic.Add(title, fieldname);
        }
        /// <summary>
        /// 清空参数
        /// </summary>
        public void ClearParams()
        {
            paramDic.Clear();
        }

        public void LoadData(int _patientId, int _hospitalId)
        {
            PatientViewData pvd = new PatientViewData();
            pvd.finishMoneyCalculateHandler += new EventHandler(pvd_finishMoneyCalculateHandler);
            pvd.GetZyPatient(_patientId, _hospitalId);
            showdata(pvd);
        }

        void pvd_finishMoneyCalculateHandler(object sender, EventArgs e)
        {
            showdata(sender as PatientViewData);
        }

        private void showdata(PatientViewData pvd)
        {
            //labInfo.Text = "";
            if (pvd == null || pvd.zyPatientData == null) return;
            List<string> data = new List<string>();
            foreach (KeyValuePair<string, string> val in paramDic)
            {
                PropertyInfo proinfo = pvd.GetType().GetProperty(val.Value);
                if (proinfo != null)
                {
                    if (pvd.finishMoneyCalculate == false)
                    {
                        data.Add(val.Key + ":" + "<b>计算中...</b>");
                    }
                    else
                    {
                        data.Add(val.Key + ":" + "<b>" + proinfo.GetValue(pvd, null).ToString() + "</b>");
                    }
                    continue;
                }

                proinfo = pvd.zyPatientData.GetType().GetProperty(val.Value);
                if (proinfo != null)
                {
                    data.Add(val.Key + ":" + "<b>" + proinfo.GetValue(pvd.zyPatientData, null).ToString() + "</b>");
                    continue;
                }
            }

            this.labInfo.Invoke((MethodInvoker)delegate()
            {
                labInfo.Text =  string.Join("  ", data.ToArray());
            });
        }
    }
}
