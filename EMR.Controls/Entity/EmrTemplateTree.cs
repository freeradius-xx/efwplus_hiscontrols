using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMR.Controls.Entity
{
    /// <summary>
    /// 病历模板树
    /// </summary>
    public class EmrTemplateTree
    {
        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private string templateText;
        public string TemplateText
        {
            get { return templateText; }
            set { templateText = value; }
        }

        private string catalogueCode;
        /// <summary>
        /// 病历分类EmrCatalogue表
        /// </summary>
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

        private int levelCode;
        /// <summary>
        /// 级别 0全院 1科室 2个人
        /// </summary>
        public int LevelCode
        {
            get { return levelCode; }
            set { levelCode = value; }
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

        private int emrDataId;
        public int EmrDataId
        {
            get { return emrDataId; }
            set { emrDataId = value; }
        }

        private DateTime createTime;
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        private int deleteFlag;
        public int DeleteFlag
        {
            get { return deleteFlag; }
            set { deleteFlag = value; }
        }
    }
}
