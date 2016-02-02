using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMR.Controls.Action;
using System.Data;
using System.Drawing;

namespace EMR.Controls
{
    public interface IEmrWriteDbHelper : IEmrDbHelper, IEmrDataSource
    {
        /// <summary>
        /// 获取病历分类
        /// </summary>
        /// <returns></returns>
        List<EMR.Controls.Entity.EmrCatalogue> GetCatalogueData();
        /// <summary>
        /// 获取病人的病历数据
        /// </summary>
        /// <param name="PatientId">病人ID</param>
        /// <returns></returns>
        List<EMR.Controls.Entity.EmrWriteRecord> GetWriteRecordData(int PatientId);

        /// <summary>
        /// 保存病历
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        bool SaveEmrWriteRecord(EMR.Controls.Entity.EmrWriteRecord record);
        /// <summary>
        /// 删除病历
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        bool DeleteEmrWriteRecord(int recordId);

        /// <summary>
        /// 调用模板数据
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="emrtype"></param>
        /// <returns></returns>
        DataTable CallTemplateData(string deptcode, string emrtype, int levelCode);

        /// <summary>
        /// 另存为病历模板
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        bool SaveEmrTemplateTree(EMR.Controls.Entity.EmrTemplateTree template);

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="signtype">1一级签名 2二级签名 3三级签名</param>
        /// <param name="doctorcode"></param>
        /// <param name="doctorname"></param>
        /// <param name="doctorlevel"></param>
        /// <param name="levelname"></param>
        /// <param name="signblob">签名图片数据</param>
        void SignName(int recordId, int signtype, string doctorcode, string doctorname, int doctorlevel, string levelname, byte[] signblob);
        /// <summary>
        /// 回退
        /// </summary>
        /// <param name="signtype">1一级签名 2二级签名 3三级签名</param>
        void SignBack(int recordId, int signtype);
        /// <summary>
        /// 获取签名数据
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="signtype">int recordId</param>
        /// <param name="imgsign"></param>
        /// <param name="levelname"></param>
        /// <param name="doctorname"></param>
        void GetSignData(int recordId, int signtype, out byte[] imgsign, out string levelname, out string doctorname);
    }
}
