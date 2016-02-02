using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMR.Controls;
using BaseControls;
using EMR.Controls.Action;
using System.Data;

namespace TestControls
{
    public class TestEmrDbHelper : IEmrDbHelper
    {
        protected SqlDbHelper oleDb;
        public TestEmrDbHelper()
        {
            oleDb = new SqlDbHelper();
            oleDb.ConnectionString = "Data Source=.;Initial Catalog=EFWDB;User ID=sa;pwd=1;";
        }

        #region IEmrDbHelper 成员

        public System.Data.DataTable SearchStorageList(DateTime begindate, DateTime enddate)
        {
            string strsql = @"SELECT EmrDataID,CreateTime,UpdateTime,Flag FROM Emr_BigData WHERE CreateTime Between '{0}' And '{1}' and Flag=0";
            strsql = string.Format(strsql, begindate.ToString("yyyy-MM-dd") + " 00:00:00", enddate.ToString("yyyy-MM-dd") + " 23:59:59");
            return oleDb.GetDataTable(strsql);
        }

        public bool SaveEmrToDatabase(ref int emrDataId, byte[] bBytes)
        {
            if (emrDataId == 0)//Add
            {
                string strsql = @"INSERT Emr_BigData(BigData,CreateTime,UpdateTime) VALUES(@blob,GETDATE(),GETDATE())";
                return oleDb.SaveBlobData(strsql, bBytes, out emrDataId);
            }
            else//Update
            {
                string strsql = @"UPDATE Emr_BigData SET BigData=@blob,UpdateTime=GETDATE() WHERE EmrDataID=" + emrDataId;
                return oleDb.SaveBlobData(strsql, bBytes);
            }
        }

        public byte[] GetEmrFormDatabase(int emrDataId)
        {
            string strsql = @"SELECT BigData FROM Emr_BigData WHERE EmrDataID=" + emrDataId;
            return oleDb.GetBlobData(strsql);
        }

        public bool DeleteEmrFormDatabase(int emrDataId)
        {
            string strsql = @"UPDATE Emr_BigData SET Flag=1 WHERE EmrDataID="+emrDataId;
            oleDb.ExecuteNoQuery(strsql);
            return true;
        }
        #endregion
    }

    public class TestEmrTemplateDbHelper : TestEmrDbHelper, IEmrTemplateDbHelper
    {

        #region IEmrTemplateDbHelper 成员

        public DataTable GetDeptData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("deptcode", typeof(String));
            dt.Columns.Add("deptname", typeof(String));

            DataRow dr = dt.NewRow();
            dr[0] = "001";
            dr[1] = "儿科";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "002";
            dr[1] = "产科";
            dt.Rows.Add(dr);

            return dt;
        }

        public List<EMR.Controls.Entity.EmrTemplateTree> GetTemplateTreeData(string deptCode, string doctorCode, int levelCode)
        {
            string strsql = @"SELECT * FROM Emr_TemplateTree WHERE DeptCode='{0}' AND LevelCode={1} AND (UserCode='{2}' OR LevelCode<>2) And Deleteflag=0";
            strsql = string.Format(strsql, deptCode, levelCode, doctorCode);
            DataTable dt = oleDb.GetDataTable(strsql);

            return ConvertDataExtend.ToList<EMR.Controls.Entity.EmrTemplateTree>(dt);
        }

        public List<EMR.Controls.Entity.EmrCatalogue> GetCatalogueData()
        {
            string strsql = @"SELECT * FROM Emr_Catalogue";
            DataTable dt = oleDb.GetDataTable(strsql);

            return ConvertDataExtend.ToList<EMR.Controls.Entity.EmrCatalogue>(dt);
        }

        public bool SaveEmrTemplateTree(EMR.Controls.Entity.EmrTemplateTree template)
        {
            if (template.ID == 0)
            {
                string strsql = @"INSERT  Emr_TemplateTree
                                                    ( TemplateText ,
                                                      CatalogueCode ,
                                                      DeptCode ,
                                                      DeptName ,
                                                      LevelCode ,
                                                      UserCode ,
                                                      UserName ,
                                                      EmrDataId ,
                                                      CreateTime 
                                                    )
                                            VALUES  ('{0}','{1}','{2}','{3}',{4},'{5}','{6}',{7},GETDATE())";
                strsql = string.Format(strsql, template.TemplateText, template.CatalogueCode, template.DeptCode, template.DeptName, template.LevelCode, template.UserCode, template.UserName, template.EmrDataId);
                template.ID = oleDb.InsertSql(strsql);
            }
            else
            {
                string strsql = @"update Emr_TemplateTree set TemplateText='{0}' where ID={1}";
                strsql = string.Format(strsql, template.TemplateText, template.ID);
                oleDb.ExecuteNoQuery(strsql);
            }
            return true;
        }

        public bool DeleteEmrTemplateTree(int templateId)
        {
            string strsql = @"UPDATE Emr_TemplateTree SET DeleteFlag=1 WHERE ID="+templateId;
            oleDb.ExecuteNoQuery(strsql);
            return true;
        }

        #endregion
    }
    public class TestEmrWriteDbHelper : TestEmrDbHelper, IEmrWriteDbHelper
    {

        #region IEmrDataSource 成员

        public void InitData(EMR.Controls.Action.EmrBindKeyData keydata)
        {
            throw new NotImplementedException();
        }

        bool IEmrDataSource.GetValue(string elId, out string text, out string value)
        {
            text = "";
            value = "";
            switch (elId)
            {
                case "HospitalName":
                    text = "英城1111";
                    value = "37";
                    break;
                case "HospitalCode":
                    text = "01123";
                    value = "01";
                    break;
                case "PatientName":
                    text = "张三2222";
                    value = "张三";
                    break;
                case "PatientSex":
                    text = "男";
                    value = "1";
                    break;
            }
            return true;
        }

        public object GetDataSource(string dllname, string classname)
        {
            throw new NotImplementedException();
        }

        public object GetDataSource(string sql)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEmrWriteDbHelper 成员

        public List<EMR.Controls.Entity.EmrCatalogue> GetCatalogueData()
        {
            string strsql = @"SELECT * FROM Emr_Catalogue";
            DataTable dt = oleDb.GetDataTable(strsql);

            return ConvertDataExtend.ToList<EMR.Controls.Entity.EmrCatalogue>(dt);
        }

        public List<EMR.Controls.Entity.EmrWriteRecord> GetWriteRecordData(int PatientId)
        {
            string strsql = @"SELECT ID ,
        RecordText ,
        CatalogueCode ,
        DeptCode ,
        DeptName ,
        UserCode ,
        UserName ,
        PatientId ,
        EmrDataId ,
        OrderNum ,
        FirstSignature ,
        FirstSignTime ,
        FirstDoctorLevel ,
        FirstDoctorLevelName,
        SecondSignature ,
        SecondSignTime ,
        SecondDoctorCode ,
        SecondDoctorName ,
        SecondDoctorLevel ,
        SecondDoctorLevelName,
        ThreeSignature ,
        ThreeSignTime ,
        ThreeDoctorCode ,
        ThreeDoctorName ,
        ThreeDoctorLevel ,
        ThreeDoctorLevelName,
        CreateTime ,
        PrintTime ,
        DeleteFlag ,
        HosptialId FROM Emr_WriteRecord WHERE PatientId={0}  And Deleteflag=0";
            strsql = string.Format(strsql, PatientId);
            DataTable dt = oleDb.GetDataTable(strsql);

            return ConvertDataExtend.ToList<EMR.Controls.Entity.EmrWriteRecord>(dt);
        }

        public bool SaveEmrWriteRecord(EMR.Controls.Entity.EmrWriteRecord record)
        {
            if (record.ID == 0)
            {
                string strsql = @"INSERT  Emr_WriteRecord
                                                    ( RecordText ,
                                                      CatalogueCode ,
                                                      DeptCode ,
                                                      DeptName ,
                                                      UserCode ,
                                                      UserName ,
                                                      PatientId ,--6
                                                      EmrDataId ,--7
                                                      OrderNum ,--8
                                                      FirstSignTime , 
                                                      SecondSignTime , 
                                                      ThreeSignTime ,
                                                      HosptialId,--12
                                                      CreateTime
                                                    )
                                            VALUES  ('{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},{8},'{9}','{10}','{11}',{12},GETDATE())";
                strsql = string.Format(strsql, record.RecordText, record.CatalogueCode, record.DeptCode, record.DeptName
                    , record.UserCode, record.UserName
                    , record.PatientId, record.EmrDataId
                    , record.OrderNum
                    , record.FirstSignTime, record.SecondSignTime, record.ThreeSignTime
                    , record.HosptialId);
                record.ID = oleDb.InsertSql(strsql);
            }
            else
            {
                string strsql = @"update Emr_WriteRecord set RecordText='{0}' where ID={1}";
                strsql = string.Format(strsql, record.RecordText, record.ID);
                oleDb.ExecuteNoQuery(strsql);
            }
            return true;
        }

        public bool DeleteEmrWriteRecord(int recordId)
        {
            string strsql = @"UPDATE Emr_WriteRecord SET DeleteFlag=1 WHERE ID=" + recordId;
            oleDb.ExecuteNoQuery(strsql);
            return true;
        }

        public DataTable CallTemplateData(string deptcode, string emrtype, int levelCode)
        {
            string strsql = @"SELECT a.EmrDataId,a.TemplateText,b.NodeText,a.DeptName,a.UserName,a.CreateTime FROM Emr_TemplateTree a
LEFT JOIN Emr_Catalogue b ON a.CatalogueCode=b.NodeCode
WHERE a.DeptCode='{0}' AND a.CatalogueCode='{1}' AND LevelCode={2} AND a.DeleteFlag=0";
            strsql = string.Format(strsql, deptcode, emrtype,levelCode);
            return oleDb.GetDataTable(strsql);
        }

        public bool SaveEmrTemplateTree(EMR.Controls.Entity.EmrTemplateTree template)
        {
            if (template.ID == 0)
            {
                string strsql = @"INSERT  Emr_TemplateTree
                                                    ( TemplateText ,
                                                      CatalogueCode ,
                                                      DeptCode ,
                                                      DeptName ,
                                                      LevelCode ,
                                                      UserCode ,
                                                      UserName ,
                                                      EmrDataId ,
                                                      CreateTime 
                                                    )
                                            VALUES  ('{0}','{1}','{2}','{3}',{4},'{5}','{6}',{7},GETDATE())";
                strsql = string.Format(strsql, template.TemplateText, template.CatalogueCode, template.DeptCode, template.DeptName, template.LevelCode, template.UserCode, template.UserName, template.EmrDataId);
                template.ID = oleDb.InsertSql(strsql);
            }
            else
            {
                string strsql = @"update Emr_TemplateTree set TemplateText='{0}' where ID={1}";
                strsql = string.Format(strsql, template.TemplateText, template.ID);
                oleDb.ExecuteNoQuery(strsql);
            }
            return true;
        }

        public void SignName(int recordId, int signtype, string doctorcode, string doctorname, int doctorlevel, string levelname, byte[] signblob)
        {
            string strsql = @"";
            if (signtype == 1)
            {
                strsql = @"UPDATE Emr_WriteRecord SET FirstSignature=1
,FirstSignTime=GETDATE()
,UserCode='{1}'
,UserName='{2}'
,FirstDoctorLevel={3}
,FirstDoctorLevelName='{4}'
WHERE ID={0}";
                strsql = string.Format(strsql, recordId, doctorcode, doctorname, doctorlevel, levelname);
                oleDb.ExecuteNoQuery(strsql);

                strsql = @"UPDATE Emr_WriteRecord SET FirstSignBlob=@blob WHERE ID=" + recordId;
                oleDb.SaveBlobData(strsql, signblob);
            }
            else if (signtype == 2)
            {
                strsql = @"UPDATE Emr_WriteRecord SET SecondSignature=1
,SecondSignTime=GETDATE()
,SecondDoctorCode='{1}'
,SecondDoctorName='{2}'
,SecondDoctorLevel={3}
,SecondDoctorLevelName='{4}'
WHERE ID={0}";
                strsql = string.Format(strsql, recordId, doctorcode, doctorname, doctorlevel, levelname);
                oleDb.ExecuteNoQuery(strsql);

                strsql = @"UPDATE Emr_WriteRecord SET SecondSignBlob=@blob WHERE ID=" + recordId;
                oleDb.SaveBlobData(strsql, signblob);
            }
            else if (signtype == 3)
            {
                strsql = @"UPDATE Emr_WriteRecord SET ThreeSignature=1
,ThreeSignTime=GETDATE()
,ThreeDoctorCode='{1}'
,ThreeDoctorName='{2}'
,ThreeDoctorLevel={3}
,ThreeDoctorLevelName='{4}'
WHERE ID={0}";
                strsql = string.Format(strsql, recordId, doctorcode, doctorname, doctorlevel, levelname);
                oleDb.ExecuteNoQuery(strsql);
                strsql = @"UPDATE Emr_WriteRecord SET ThreeSignBlob=@blob WHERE ID=" + recordId;
                oleDb.SaveBlobData(strsql, signblob);
            }
        }

        public void SignBack(int recordId, int signtype)
        {
            string strsql = @"";
            if (signtype == 1)
            {
                strsql = @"UPDATE Emr_WriteRecord SET FirstSignature=0
,FirstDoctorLevel=0
,FirstDoctorLevelName=''
,FirstSignBlob=NULL
WHERE ID={0}";
            }
            else if (signtype == 2)
            {
                strsql = @"UPDATE Emr_WriteRecord SET SecondSignature=0
,SecondDoctorCode=''
,SecondDoctorName=''
,SecondDoctorLevel=0
,SecondDoctorLevelName=''
,SecondSignBlob=NULL
WHERE ID={0}";
            }
            else if (signtype == 3)
            {
                strsql = @"UPDATE Emr_WriteRecord SET ThreeSignature=0
,ThreeDoctorCode=''
,ThreeDoctorName=''
,ThreeDoctorLevel=0
,ThreeDoctorLevelName=''
,ThreeSignBlob=NULL
WHERE ID={0}";
            }

            strsql = string.Format(strsql, recordId);
            oleDb.ExecuteNoQuery(strsql);
        }

        public void GetSignData(int recordId, int signtype, out byte[] imgsign, out string levelname, out string doctorname)
        {
            imgsign = null;
            levelname = "";
            doctorname = "";
            string strsql = @"";
            if (signtype == 1)
            {
                strsql = @"SELECT UserName,FirstDoctorLevelName FROM Emr_WriteRecord WHERE ID="+recordId;
                DataTable dt= oleDb.GetDataTable(strsql);
                if (dt.Rows.Count > 0)
                {
                    levelname = dt.Rows[0]["FirstDoctorLevelName"].ToString();
                    doctorname = dt.Rows[0]["UserName"].ToString();
                }

                strsql = @"SELECT FirstSignBlob FROM Emr_WriteRecord WHERE ID="+recordId;
                imgsign =oleDb.GetBlobData(strsql);
            }
            else if (signtype == 2)
            {
                strsql = @"SELECT SecondDoctorName,SecondDoctorLevelName FROM Emr_WriteRecord WHERE ID="+recordId;
                DataTable dt = oleDb.GetDataTable(strsql);
                if (dt.Rows.Count > 0)
                {
                    levelname = dt.Rows[0]["SecondDoctorLevelName"].ToString();
                    doctorname = dt.Rows[0]["SecondDoctorName"].ToString();
                }

                strsql = @"SELECT SecondSignBlob FROM Emr_WriteRecord WHERE ID=" + recordId;
                imgsign = oleDb.GetBlobData(strsql);
            }
            else if (signtype == 3)
            {
                strsql = @"SELECT ThreeDoctorName,ThreeDoctorLevelName FROM Emr_WriteRecord WHERE ID=" + recordId;
                DataTable dt = oleDb.GetDataTable(strsql);
                if (dt.Rows.Count > 0)
                {
                    levelname = dt.Rows[0]["ThreeDoctorLevelName"].ToString();
                    doctorname = dt.Rows[0]["ThreeDoctorName"].ToString();
                }

                strsql = @"SELECT ThreeSignBlob FROM Emr_WriteRecord WHERE ID=" + recordId;
                imgsign = oleDb.GetBlobData(strsql);
            }
           
        }

        #endregion
    }
}
