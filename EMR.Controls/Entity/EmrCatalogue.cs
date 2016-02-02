using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMR.Controls.Entity
{
    /// <summary>
    /// 病历类型
    /// </summary>
    public class EmrCatalogue
    {
        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private string nodeCode;
        /// <summary>
        /// 病历类型
        /// </summary>
        public string NodeCode
        {
            get { return nodeCode; }
            set { nodeCode = value; }
        }

        private string nodeText;
        public string NodeText
        {
            get { return nodeText; }
            set { nodeText = value; }
        }

        private string eventName;
        /// <summary>
        /// 事件
        /// </summary>
        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

        private short mustWriteFlag;
        /// <summary>
        /// 必须编写
        /// </summary>
        public short MustWriteFlag
        {
            get { return mustWriteFlag; }
            set { mustWriteFlag = value; }
        }

        private short uniquenessFlag;
        /// <summary>
        /// 唯一
        /// </summary>
        public short UniquenessFlag
        {
            get { return uniquenessFlag; }
            set { uniquenessFlag = value; }
        }

        private int writeHours;
        /// <summary>
        /// 多少小时必须完成
        /// </summary>
        public int WriteHours
        {
            get { return writeHours; }
            set { writeHours = value; }
        }

        private int auditHours;
        /// <summary>
        /// 多少小时必须审核
        /// </summary>
        public int AuditHours
        {
            get { return auditHours; }
            set { auditHours = value; }
        }

        private int diagnoseHours;
        /// <summary>
        /// 修正诊断时间
        /// </summary>
        public int DiagnoseHours
        {
            get { return diagnoseHours; }
            set { diagnoseHours = value; }
        }

        private int generalHours;
        /// <summary>
        /// 循环周期，一般
        /// </summary>
        public int GeneralHours
        {
            get { return generalHours; }
            set { generalHours = value; }
        }

        private int seriouslyHours;
        /// <summary>
        /// 病重
        /// </summary>
        public int SeriouslyHours
        {
            get { return seriouslyHours; }
            set { seriouslyHours = value; }
        }

        private int criticalHours;
        /// <summary>
        /// 病危
        /// </summary>
        public int CriticalHours
        {
            get { return criticalHours; }
            set { criticalHours = value; }
        }

        private int deleteFlag;
        public int DeleteFlag
        {
            get { return deleteFlag; }
            set { deleteFlag = value; }
        }
    }
}
