using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMR.Controls.Action;
using System.Data;
using EMR.Controls.Entity;

namespace EMR.Controls
{
    public interface IEmrTemplateDbHelper : IEmrDbHelper
    {
        /// <summary>
        /// 获取病历分类
        /// </summary>
        /// <returns></returns>
        List<EmrCatalogue> GetCatalogueData();
        /// <summary>
        /// 获取科室数据
        /// </summary>
        /// <returns></returns>
        DataTable GetDeptData();
        /// <summary>
        /// 获取模板数据
        /// </summary>
        /// <param name="deptCode">科室</param>
        /// <param name="doctorCode">医生</param>
        /// <param name="levelCode">级别</param>
        /// <returns></returns>
        List<EMR.Controls.Entity.EmrTemplateTree> GetTemplateTreeData(string deptCode,string doctorCode, int levelCode);
        /// <summary>
        /// 保存病历模板
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        bool SaveEmrTemplateTree(EMR.Controls.Entity.EmrTemplateTree template);
        /// <summary>
        /// 删除病历模板
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <returns></returns>
        bool DeleteEmrTemplateTree(int templateId);
    }
}
