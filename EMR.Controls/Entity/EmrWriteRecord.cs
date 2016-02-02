using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMR.Controls.Entity
{
    /// <summary>
    /// 病历书写记录
    /// </summary>
    public class EmrWriteRecord
    {
        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private string recordText;
        public string RecordText
        {
            get { return recordText; }
            set { recordText = value; }
        }

        private string catalogueCode;
        public string CatalogueCode
        {
            get { return catalogueCode; }
            set { catalogueCode = value; }
        }

        private string deptCode;
        public string DeptCode
        {
            get { return deptCode; }
            set { deptCode = value; }
        }

        private string deptName;
        public string DeptName
        {
            get { return deptName; }
            set { deptName = value; }
        }

        private string userCode;
        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private int patientId;
        public int PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }

        private int emrDataId;
        public int EmrDataId
        {
            get { return emrDataId; }
            set { emrDataId = value; }
        }

        private int orderNum;
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNum
        {
            get { return orderNum; }
            set { orderNum = value; }
        }

        private int firstSignature;
        /// <summary>
        /// 一级签名
        /// </summary>
        public int FirstSignature
        {
            get { return firstSignature; }
            set { firstSignature = value; }
        }

        private DateTime firstSignTime=new DateTime(1900,1,1);
        public DateTime FirstSignTime
        {
            get { return firstSignTime; }
            set { firstSignTime = value; }
        }

        private int firstDoctorLevel;
        /// <summary>
        /// 医生级别
        /// </summary>
        public int FirstDoctorLevel
        {
            get { return firstDoctorLevel; }
            set { firstDoctorLevel = value; }
        }

        private string _firstDoctorLevelName;
        public string FirstDoctorLevelName
        {
            get { return _firstDoctorLevelName; }
            set { _firstDoctorLevelName = value; }
        }

        private int secondSignature;
        /// <summary>
        /// 二级签名
        /// </summary>
        public int SecondSignature
        {
            get { return secondSignature; }
            set { secondSignature = value; }
        }

        private DateTime secondSignTime = new DateTime(1900, 1, 1);
        public DateTime SecondSignTime
        {
            get { return secondSignTime; }
            set { secondSignTime = value; }
        }

        private string secondDoctorCode;
        public string SecondDoctorCode
        {
            get { return secondDoctorCode; }
            set { secondDoctorCode = value; }
        }

        private string secondDoctorName;
        public string SecondDoctorName
        {
            get { return secondDoctorName; }
            set { secondDoctorName = value; }
        }

        private int secondDoctorLevel;
        /// <summary>
        /// 医生级别
        /// </summary>
        public int SecondDoctorLevel
        {
            get { return secondDoctorLevel; }
            set { secondDoctorLevel = value; }
        }

        private string _secondDoctorLevelName;
        public string SecondDoctorLevelName
        {
            get { return _secondDoctorLevelName; }
            set { _secondDoctorLevelName = value; }
        }

        private int threeSignature;
        /// <summary>
        /// 三级签名
        /// </summary>
        public int ThreeSignature
        {
            get { return threeSignature; }
            set { threeSignature = value; }
        }

        private DateTime threeSignTime = new DateTime(1900, 1, 1);
        public DateTime ThreeSignTime
        {
            get { return threeSignTime; }
            set { threeSignTime = value; }
        }

        private string threeDoctorCode;
        public string ThreeDoctorCode
        {
            get { return threeDoctorCode; }
            set { threeDoctorCode = value; }
        }

        private string threeDoctorName;
        public string ThreeDoctorName
        {
            get { return threeDoctorName; }
            set { threeDoctorName = value; }
        }



        private int threeDoctorLevel;
        /// <summary>
        /// 医生级别
        /// </summary>
        public int ThreeDoctorLevel
        {
            get { return threeDoctorLevel; }
            set { threeDoctorLevel = value; }
        }

        private string _threeDoctorLevelName;
        public string ThreeDoctorLevelName
        {
            get { return _threeDoctorLevelName; }
            set { _threeDoctorLevelName = value; }
        }

        private DateTime createTime;
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        private DateTime printTime = new DateTime(1900, 1, 1);
        public DateTime PrintTime
        {
            get { return printTime; }
            set { printTime = value; }
        }

        private int deleteFlag;
        public int DeleteFlag
        {
            get { return deleteFlag; }
            set { deleteFlag = value; }
        }

        private int hosptialId;
        public int HosptialId
        {
            get { return hosptialId; }
            set { hosptialId = value; }
        }
    }
}
