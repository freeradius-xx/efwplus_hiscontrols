using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseControls;

namespace BedCard.Controls
{
    /// <summary>
    /// 界面显示病人数据
    /// </summary>
    public class PatientViewData
    {
        public mzPatientData mzPatientData { get; set; }

        public ZyPatientData zyPatientData { get; set; }

        private bool _finishMoneyCalculate = true;
        /// <summary>
        /// 完成金额计算
        /// </summary>
        public bool finishMoneyCalculate
        {
            get { return _finishMoneyCalculate; }
            set { _finishMoneyCalculate = value; }
        }

        private decimal _totalMoney;
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal totalMoney
        {
            get { return _totalMoney; }
            set { _totalMoney = value; }
        }

        private decimal _policyMoney;
        /// <summary>
        /// 医保金额
        /// </summary>
        public decimal policyMoney
        {
            get { return _policyMoney; }
            set { _policyMoney = value; }
        }

        private decimal _selfMoney;
        /// <summary>
        /// 自费金额
        /// </summary>
        public decimal selfMoney
        {
            get { return _selfMoney; }
            set { _selfMoney = value; }
        }

        private decimal _favorableMoney;
        /// <summary>
        /// 其他优惠
        /// </summary>
        public decimal favorableMoney
        {
            get { return _favorableMoney; }
            set { _favorableMoney = value; }
        }

        private decimal _prepaidMoney;
        /// <summary>
        /// 预交金额
        /// </summary>
        public decimal prepaidMoney
        {
            get { return _prepaidMoney; }
            set { _prepaidMoney = value; }
        }
        /// <summary>
        /// 余额=预交金额-总金额
        /// </summary>
        public decimal balanceMoney
        {
            get { return _prepaidMoney - _totalMoney; }
        }


        public event EventHandler finishMoneyCalculateHandler;

        public void GetMzPatient(int _PatLstID, int _HospitalID)
        {
            mzPatientData patient = new mzPatientData();
            mzPatientData = patient.GetMzPatient(_PatLstID, _HospitalID) as mzPatientData;
        }


        public void GetZyPatient(int _ID, int _HospitalID)
        {
            ZyPatientData patient = new ZyPatientData();
            zyPatientData = patient.GetZyPatient(_HospitalID, _ID);

            this._finishMoneyCalculate = false;
            if (this.finishMoneyCalculateHandler != null)
            {
                using (AsynProcessing asyn = new AsynProcessing(midSysName.HIS2))
                {
                    asyn.Invoke(delegate()
                 {
                     
                     decimal _prem = GetZyPrepaidMoney(_ID, _HospitalID, dbhelper);
                     decimal _totalm = GetZyTotalMoney(_ID, _HospitalID, dbhelper); ;
                     decimal _favm = GetFavorableMoney(_ID, _HospitalID, dbhelper); ;
                     decimal _polm = GetPolicyMoney(_ID, _HospitalID, dbhelper); ;
                     lock (this)
                     {
                         this._prepaidMoney = _prem;
                         this._totalMoney = _totalm;
                         this._policyMoney = _polm;
                         this._favorableMoney = _favm;

                         this._finishMoneyCalculate = true;


                         this.finishMoneyCalculateHandler(this, new EventArgs());
                     }
                 });
                }
            }
        }
        //获取预交金
        private decimal GetZyPrepaidMoney(int _ID, int _HospitalID, AsynDbHelper dbhelper)
        {
            string strsql = @"SELECT SUM(CostMoney) CostMoney FROM hisdb..Data_PayWayCost  WHERE PatientID=" + _ID;
            return GreatRHIS.BLL.Admin.Tools.ToDecimal(dbhelper.ExecuteScalar(strsql));
        }
        //获取总金额
        private decimal GetZyTotalMoney(int _ID, int _HospitalID, AsynDbHelper dbhelper)
        {
            string strsql = @"SELECT SUM(TotalPrice) TotalPrice FROM hisdb..Data_InPatientCost WHERE PatientID="+_ID;
            return GreatRHIS.BLL.Admin.Tools.ToDecimal(dbhelper.ExecuteScalar(strsql));
        }
        //获取优惠金额
        private decimal GetFavorableMoney(int _ID, int _HospitalID, AsynDbHelper dbhelper)
        {
            //System.Threading.Thread.Sleep(1000);
            return 0;
        }
        //获取医保报销金额
        private decimal GetPolicyMoney(int _ID, int _HospitalID, AsynDbHelper dbhelper)
        {
            //System.Threading.Thread.Sleep(1000);
            return 0;
        }
    }
}
