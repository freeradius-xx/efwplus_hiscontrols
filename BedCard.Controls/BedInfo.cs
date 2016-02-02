﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BedCard.Controls
{
    [Serializable]
    public class BedInfo : ICloneable
    {
        #region 属性
        private int _hospitalid;
        public int HospitalID
        {
            get
            {
                return _hospitalid;
            }
            set
            {
                _hospitalid = value;
            }
        }

        #region 床位信息

        private string _bed;
        //设置或取得当前床位编号
        public string BedNo
        {
            get
            {
                return _bed;
            }
            set
            {
                _bed = value;
            }
        }



        private string _roomnum;
        //设置或取得当前床位的房号
        public string RoomNum
        {
            get
            {
                return _roomnum;
            }
            set
            {
                _roomnum = value;
            }
        }

        private string _wardcode;
        //设置或取得当前床位的病区
        public string WardCode
        {
            get
            {
                return _wardcode;
            }
            set
            {
                _wardcode = value;
            }
        }

        private string _deptcode;
        //设置或取得当前床位的科室
        public string DeptCode
        {
            get
            {
                return _deptcode;
            }
            set
            {
                _deptcode = value;
            }
        }

        private string _limitsex;
        //设置或取得当前床位的性别限制条件
        public string LimitSex
        {
            get
            {
                return _limitsex;
            }
            set
            {
                _limitsex = value;
            }
        }

        #endregion

        #region 床位上的病人信息
        private int _patientid;
        //设置或取得当前病人信息
        public int PatientID
        {
            get
            {
                return _patientid;
            }
            set
            {
                _patientid = value;
            }
        }

        private string _patientnum;
        //设置或取得当前病人住院号
        public string PatientNum
        {
            get
            {
                return _patientnum;
            }
            set
            {
                _patientnum = value;
            }
        }
        private string _patientname;
        //设置或取得当前病人姓名
        public string PatientName
        {
            get
            {
                return _patientname;
            }
            set
            {
                _patientname = value;
            }
        }

        private bool _group = false;
        //包床标志
        public bool Group
        {
            get
            {
                return _group;
            }
            set
            {
                _group = value;
            }
        }

        private int _step;
        //出院状态标志
        public int Step
        {
            get { return _step; }
            set
            {
                _step = value;
            }
        }

        private bool _IsHistory = false;
        //床位卡显示的数据是否为历史记录
        public bool IsHistory
        {
            get
            {
                return _IsHistory;
            }
            set
            {
                _IsHistory = value;
            }
        }





        //是否有病人
        public bool IsUsed
        {
            get
            {
                return _patientid > 0;
            }
        }

        //是否新入院病人
        public bool IsNewPatient
        {
            get
            {
                return false;
            }
        }



        private string _nurse;
        //设置或取得当前病人的护理级别01\02\03\04
        public string Nurse
        {
            get
            {
                return _nurse;
            }
            set
            {
                _nurse = value;
            }
        }

        private string _sex;
        //性别
        public string Sex
        {
            get
            {
                return _sex;
            }
            set
            {
                _sex = value;
            }
        }


        private string _age;
        //设置或取得当前病人的年龄
        public string Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
            }
        }
        private string _diet;
        //设置或取得当前病人的饮食情况
        public string Diet
        {
            get
            {
                return _diet;
            }
            set
            {
                _diet = value;
            }
        }
        private string _diagnosis;
        //设置或取得当前病人的临床诊断
        public string Diagnosis
        {
            get
            {
                return _diagnosis;
            }
            set
            {
                _diagnosis = value;
            }
        }
        private bool _HasAdvice;
        //设置或取得当前病人是否存在医嘱信息
        public bool HasAdvice
        {
            get
            {
                return _HasAdvice;
            }
            set
            {
                _HasAdvice = value;
            }
        }

        private string _dept;
        //设置或取得当前病人的住院科室
        public string Dept
        {
            get
            {
                return _dept;
            }
            set
            {
                _dept = value;
            }
        }
        private string _doctor;
        //设置或取得当前病人的主治医生
        public string Doctor
        {
            get
            {
                return _doctor;
            }
            set
            {
                _doctor = value;
            }
        }

        private string _situation;

        public string Situation
        {
            get
            {
                return _situation;
            }
            set
            {
                _situation = value;
            }
        }

        private string _entertime;
        //设置或取得当前病人入科时间
        public string EnterTime
        {
            get
            {
                return _entertime;
            }
            set
            {
                _entertime = value;
            }
        }




        #endregion

        #endregion

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
