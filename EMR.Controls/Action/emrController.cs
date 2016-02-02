using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Drawing;

namespace EMR.Controls.Action
{
    public class emrController
    {
        public IEmrRecord emrView;
        public IEmrDbHelper emrDbHelper;
        private EmrBindKeyData currBindKeyData;


        public emrController(IEmrRecord _emrView)
        {
            emrView = _emrView;
            //emrView.emrControlType = EmrControlType.病历预览;
            //emrView.emrOperStyle = EmrOperStyle.默认;
        }

        #region 初始化

        /// <summary>
        /// 文件方式存储，调用此方法
        /// </summary>
        public void InitLoad()
        {
            EmrToolTipManage.InitEmrToolTip();//初始化提示控件
            KnowledgeManage.InitKnowledg();//初始化知识库

            emrView.emrControlType = EmrControlType.病历模板;
            emrView.emrDatastorageType = EmrDatastorageType.文件存储;
            //emrView.IsShowFileBtn = true;
            emrView.btnState();
        }

        /// <summary>
        /// 数据库方式存储，调用此方法
        /// </summary>
        public void InitLoad(IEmrDbHelper _emrDbHelper)
        {
            emrDbHelper = _emrDbHelper;
            EmrToolTipManage.InitEmrToolTip();//初始化提示控件
            KnowledgeManage.InitKnowledg();//初始化知识库

            emrView.emrControlType = EmrControlType.病历模板;
            emrView.emrDatastorageType = EmrDatastorageType.数据库存储;
            //emrView.IsShowFileBtn = true;
            emrView.btnState();
        }

        /// <summary>
        /// 模板功能调用此方法
        /// </summary>
        /// <param name="_keyData"></param>
        public void InitLoad(IEmrTemplateDbHelper _emrTemplateDbHelper, EmrBindKeyData _keyData)
        {
            emrDbHelper = _emrTemplateDbHelper;
            emrView.CurrBindKeyData = _keyData;
            currBindKeyData = _keyData;
            //DataSourceManage.InitData(emrDataSource);
            EmrToolTipManage.InitEmrToolTip();//初始化提示控件
            KnowledgeManage.InitKnowledg();//初始化知识库

            emrView.emrControlType = EmrControlType.病历模板;
            emrView.emrDatastorageType = EmrDatastorageType.数据库存储;
            emrView.IsShowFileBtn = false;
            emrView.btnState();
        }

        /// <summary>
        /// 病历书写调用此方法
        /// </summary>
        /// <param name="_keyData"></param>
        public void InitLoad(IEmrWriteDbHelper _emrWriteDbHelper, EmrBindKeyData _keyData, bool IsPreview)
        {
            emrDbHelper = _emrWriteDbHelper;
            emrView.CurrBindKeyData = _keyData;
            currBindKeyData = _keyData;
            DataSourceManage.InitData(_emrWriteDbHelper);
            EmrToolTipManage.InitEmrToolTip();//初始化提示控件
            KnowledgeManage.InitKnowledg();//初始化知识库

            if (IsPreview)
                emrView.emrControlType = EmrControlType.病历预览;
            else
                emrView.emrControlType = EmrControlType.病历编辑;
            emrView.emrDatastorageType = EmrDatastorageType.数据库存储;
            emrView.IsShowFileBtn = false;
            emrView.btnState();
        }


        //保存文件流为本地文件
        public bool SaveFileToLocal()
        {
            byte[] bBytes = emrView.GetEmrText();
            if (bBytes == null)
                return false;

            if (bBytes.Length <= 0)
                return false;

            Directory.CreateDirectory(emrView.currFileInfo.DirectoryName);
            try
            {
                FileStream fileStream = new FileStream(emrView.currFileInfo.FullName, FileMode.Create);
                BinaryWriter bWriter = new BinaryWriter(fileStream);
                bWriter.Write(bBytes);
                bWriter.Close();
                fileStream.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //获取文件字节
        public void OpenFileToEmr()
        {
            try
            {
                if (emrView.currFileInfo != null)
                {
                    FileStream fs = new FileStream(emrView.currFileInfo.FullName, FileMode.OpenOrCreate, FileAccess.Read);
                    BinaryReader bReader = new BinaryReader(fs);
                    byte[] bBytes = bReader.ReadBytes((int)fs.Length);
                    bReader.Close();
                    fs.Close();

                    emrView.LoadEmrText(bBytes);
                }
                else
                {
                    emrView.LoadEmrText(null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //保存到数据库
        public bool SaveDatabase()
        {
            int emrDataId = emrView.CurrBindKeyData.EmrDataId;
            byte[] bBytes = emrView.GetEmrText();
            bool val = emrDbHelper.SaveEmrToDatabase(ref emrDataId, bBytes);
            emrView.CurrBindKeyData.EmrDataId = emrDataId;
            return val;
        }

        public void OpenDatabaseToEmr()
        {
            if (emrView.CurrBindKeyData.EmrDataId == 0)
            {
                emrView.LoadEmrText(null);
            }
            else
            {
                byte[] bBytes = emrDbHelper.GetEmrFormDatabase(emrView.CurrBindKeyData.EmrDataId);
                if (bBytes.Length == 1)
                    emrView.LoadEmrText(null);
                else
                    emrView.LoadEmrText(bBytes);
            }
        }

        public void OpenDatabaseToEmr(int emrDataId)
        {
            if (emrView.CurrBindKeyData.EmrDataId == 0)
            {
                emrView.LoadEmrText(null);
            }
            else
            {
                byte[] bBytes = emrDbHelper.GetEmrFormDatabase(emrDataId);
                if (bBytes.Length == 1)
                    emrView.LoadEmrText(null);
                else
                    emrView.LoadEmrText(bBytes);
            }
        }

        public DataTable SearchStorageList(DateTime begindate, DateTime enddate)
        {
            return emrDbHelper.SearchStorageList(begindate, enddate);
        }
        #endregion

        #region 病历编写EmrWriteTree
        private IEmrWriteTree iEmrWriteTree;
        private IEmrWriteDbHelper iEmrWriteDbHelper;
        public void InitWrite(IEmrWriteTree _emrWriteTree)
        {
            iEmrWriteTree = _emrWriteTree;
            iEmrWriteDbHelper = emrDbHelper as IEmrWriteDbHelper;

            GetWriteRecordTreeData();
        }

        public void GetWriteRecordTreeData()
        {
            List<EMR.Controls.Entity.EmrWriteRecord> rlist = iEmrWriteDbHelper.GetWriteRecordData(currBindKeyData.PatientId);
            List<EMR.Controls.Entity.EmrCatalogue> clist = iEmrWriteDbHelper.GetCatalogueData();

            iEmrWriteTree.loadTreeData(clist, rlist);
        }

        public void SaveEmrWriteRecord(EMR.Controls.Entity.EmrWriteRecord record)
        {
            if (record.ID > 0)
            {
                iEmrWriteDbHelper.SaveEmrWriteRecord(record);
            }
            else
            {
                //1.保存病历内容得到EmrDataId
                byte[] bBytes = new byte[1];
                int emrDataId = 0;
                iEmrWriteDbHelper.SaveEmrToDatabase(ref emrDataId, bBytes);
                //2.保存模板数据
                record.EmrDataId = emrDataId;
                iEmrWriteDbHelper.SaveEmrWriteRecord(record);
                //3.设置当前数据
                //currBindKeyData.RecordId = template.ID;
                //currBindKeyData.EmrDataId = emrDataId;
            }
            //4.显示树
            GetWriteRecordTreeData();
        }

        public void DeleteEmrWriteRecord(int recordId, int emrDataId)
        {
            //
            iEmrWriteDbHelper.DeleteEmrWriteRecord(recordId);
            iEmrWriteDbHelper.DeleteEmrFormDatabase(emrDataId);

            GetWriteRecordTreeData();
        }

        public void GetEmrWriteRecord(EMR.Controls.Entity.EmrWriteRecord record)
        {
            //currBindKeyData.RecordId = template.ID;
            //currBindKeyData.EmrDataId = template.EmrDataId;
            currBindKeyData.ChangeEmrData(record.ID, record.EmrDataId, record.CatalogueCode,record.DeptCode,record.DeptName,record.UserCode,record.UserName, record.FirstSignature,Convert.ToInt32(record.FirstDoctorLevel), record.SecondSignature,record.SecondDoctorCode,Convert.ToInt32(record.SecondDoctorLevel), record.ThreeSignature,record.ThreeDoctorCode,Convert.ToInt32(record.ThreeDoctorLevel),record.RecordText);
            emrView.emrOperStyle = EmrOperStyle.默认;
            OpenDatabaseToEmr();
            emrView.btnState();
            emrView.btnSignState();
            emrView.btnModifyState();
        }
        //调用病历模板
        public DataTable CallTemplateData(int levelCode)
        {
            return iEmrWriteDbHelper.CallTemplateData(currBindKeyData.DeptCode, currBindKeyData.EmrType,levelCode);
        }
        //另存为病历
        public void SaveAsTemplateData(string title, int level)
        {
            EMR.Controls.Entity.EmrTemplateTree template = new EMR.Controls.Entity.EmrTemplateTree();
            template.TemplateText = title;
            template.CatalogueCode = currBindKeyData.EmrType;
            template.DeptCode = currBindKeyData.CurrDeptCode;
            template.DeptName = currBindKeyData.CurrDeptName;
            template.LevelCode = level;
            template.UserCode = currBindKeyData.UserCode;
            template.UserName = currBindKeyData.UserName;

            int emrDataId = 0;
            if (emrView.emrOperStyle == EmrOperStyle.修改)
                iEmrWriteDbHelper.SaveEmrToDatabase(ref emrDataId, emrView.GetEmrText());
            else
            {
                if (currBindKeyData.EmrDataId > 0)
                {
                    byte[] bBytes = emrDbHelper.GetEmrFormDatabase(currBindKeyData.EmrDataId);
                    iEmrWriteDbHelper.SaveEmrToDatabase(ref emrDataId, bBytes);
                }
            }
            //2.保存模板数据
            template.EmrDataId = emrDataId;
            iEmrWriteDbHelper.SaveEmrTemplateTree(template);
        }

        public void GetModifyState(out bool modify)
        {
            modify = false;
            //0.未签名
            if (currBindKeyData.FirstSignature == 0)
            {
                modify = true;
            }
            //1.一级签名已签，用户级别大于
            else if (currBindKeyData.FirstSignature == 1 && currBindKeyData.SecondSignature == 0 && currBindKeyData.UserLevel > currBindKeyData.FirstDoctorLevel)
            {
                modify = true;
            }
            //2.二级签名已签，用户级别大于
            else if (currBindKeyData.SecondSignature == 1 && currBindKeyData.ThreeSignature == 0 && currBindKeyData.UserLevel > currBindKeyData.SecondDoctorLevel)
            {
                modify = true;
            }
            //3.三级签名已签
        }
        //获取签名状态
        public void GetSignNameState(out bool sign, out bool back)
        {
            VerifSignNameState(currBindKeyData, out sign, out back);
        }
        public void VerifSignNameState(EmrBindKeyData _currBindKeyData, out bool sign, out bool back)
        {
            sign = false;
            back = false;
            //1.未签名
            if (_currBindKeyData.FirstSignature == 0)
            {
                //A.本人 1-0
                if (_currBindKeyData.UserCode == _currBindKeyData.DoctorCode)
                {
                    sign = true;
                    back = false;
                }
                //B.非   0-0
                else
                {
                    sign = false;
                    back = false;
                }
            }
            //2.一级签名已签
            if (_currBindKeyData.FirstSignature == 1 && _currBindKeyData.SecondSignature == 0)
            {
                //A.本人    0-1
                if (_currBindKeyData.UserCode == _currBindKeyData.DoctorCode)
                {
                    sign = false;
                    back = true;
                }
                //B.上级医师 1-0
                else if (_currBindKeyData.UserCode != _currBindKeyData.DoctorCode && _currBindKeyData.UserLevel > _currBindKeyData.FirstDoctorLevel)
                {
                    sign = true;
                    back = false;
                }
                //C.非   0-0
                else
                {
                    sign = false;
                    back = false;
                }
            }
            //3.二级签名已签
            if (_currBindKeyData.SecondSignature == 1 && _currBindKeyData.ThreeSignature == 0)
            {
                //A.本人    0-1
                if (_currBindKeyData.UserCode == _currBindKeyData.SecondDoctorCode)
                {
                    sign = false;
                    back = true;
                }
                //B.上级医师 1-0
                else if (_currBindKeyData.UserCode != _currBindKeyData.SecondDoctorCode && _currBindKeyData.UserLevel > _currBindKeyData.SecondDoctorLevel)
                {
                    sign = true;
                    back = false;
                }
                //C.非   0-0
                else
                {
                    sign = false;
                    back = false;
                }
            }
            //4.三级签名已签
            if (_currBindKeyData.ThreeSignature == 1)
            {
                //A.本人    0-1
                if (_currBindKeyData.UserCode == _currBindKeyData.ThreeDoctorCode)
                {
                    sign = false;
                    back = true;
                }
                //B.非   0-0
                else
                {
                    sign = false;
                    back = false;
                }
            }
        }
        //签名
        public void SignName(Bitmap imgSign)
        {
            VerifSignName(currBindKeyData, imgSign);
        }
        public void VerifSignName(EmrBindKeyData _currBindKeyData, Bitmap imgSign)
        {
            //更新签名数据
            int signtype = 0;
            if (_currBindKeyData.FirstSignature == 0)
                signtype = 1;
            else if (_currBindKeyData.SecondSignature == 0)
                signtype = 2;
            else if (_currBindKeyData.ThreeSignature == 0)
                signtype = 3;
            if (signtype > 0)
            {
                MemoryStream mstream = new MemoryStream();
                imgSign.Save(mstream, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] byData = new Byte[mstream.Length];
                mstream.Position = 0;
                mstream.Read(byData, 0, byData.Length);
                mstream.Close();

                iEmrWriteDbHelper.SignName(_currBindKeyData.RecordId, signtype, _currBindKeyData.UserCode, _currBindKeyData.UserName, _currBindKeyData.UserLevel, _currBindKeyData.UserLevelName, byData);
                _currBindKeyData.SignName(signtype, _currBindKeyData.UserCode, _currBindKeyData.UserLevel);
                //界面显示签名
            }
            //emrView.btnSignState();
            //emrView.btnModifyState();
            //GetWriteRecordTreeData();
            //OpenDatabaseToEmr();

            GetWriteRecordTreeData();
        }
        //退回
        public void SignBack()
        {
            VerifSignBack(currBindKeyData);
        }
        public void VerifSignBack(EmrBindKeyData _currBindKeyData)
        {
            int signtype = 0;
            if (_currBindKeyData.ThreeSignature == 1)
                signtype = 3;
            else if (_currBindKeyData.SecondSignature == 1)
                signtype = 2;
            else if (_currBindKeyData.FirstSignature == 1)
                signtype = 1;
            if (signtype > 0)
            {
                iEmrWriteDbHelper.SignBack(_currBindKeyData.RecordId, signtype);
                _currBindKeyData.SignBack(signtype);
            }
            //emrView.btnSignState();
            //emrView.btnModifyState();
            //GetWriteRecordTreeData();
            //OpenDatabaseToEmr();

            GetWriteRecordTreeData();
        }

        //获取签名数据
        public void GetSignData(int signtype, out Bitmap imgsign, out string levelname, out string doctorname)
        {
            byte[] _imgsign;
            iEmrWriteDbHelper.GetSignData(currBindKeyData.RecordId, signtype, out _imgsign, out levelname, out doctorname);
            if (_imgsign != null)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(_imgsign);
                imgsign = new Bitmap(ms);
            }
            else
            {
                imgsign = null;
            }
        }
        #endregion

        #region 病历模板EmrTemplateTree
        private IEmrTemplateTree iEmrTemplateTree;
        private IEmrTemplateDbHelper iEmrTemplateDbHelper;
        public void InitTemplate(IEmrTemplateTree _emrTemplateTree)
        {
            iEmrTemplateTree = _emrTemplateTree;
            iEmrTemplateDbHelper = emrDbHelper as IEmrTemplateDbHelper;
            iEmrTemplateTree.loadDeptData(iEmrTemplateDbHelper.GetDeptData(), currBindKeyData.CurrDeptCode);

            List<EMR.Controls.Entity.EmrTemplateTree> tlist = iEmrTemplateDbHelper.GetTemplateTreeData(currBindKeyData.CurrDeptCode,currBindKeyData.UserCode, 0);
            List<EMR.Controls.Entity.EmrCatalogue> clist = iEmrTemplateDbHelper.GetCatalogueData();
            iEmrTemplateTree.loadTreeData(clist, tlist);
        }

        public void GetTemplateTreeData(string deptcode, int level)
        {
            List<EMR.Controls.Entity.EmrTemplateTree> tlist = iEmrTemplateDbHelper.GetTemplateTreeData(deptcode, currBindKeyData.UserCode, level);
            List<EMR.Controls.Entity.EmrCatalogue> clist = iEmrTemplateDbHelper.GetCatalogueData();
            iEmrTemplateTree.loadTreeData(clist, tlist);
        }

        public void SaveEmrTemplateTree(EMR.Controls.Entity.EmrTemplateTree template)
        {
            if (template.ID > 0)
            {
                iEmrTemplateDbHelper.SaveEmrTemplateTree(template);
            }
            else
            {
                //1.保存病历内容得到EmrDataId
                byte[] bBytes = new byte[1];
                int emrDataId = 0;
                iEmrTemplateDbHelper.SaveEmrToDatabase(ref emrDataId, bBytes);
                //2.保存模板数据
                template.EmrDataId = emrDataId;
                iEmrTemplateDbHelper.SaveEmrTemplateTree(template);
                //3.设置当前数据
                //currBindKeyData.RecordId = template.ID;
                //currBindKeyData.EmrDataId = emrDataId;
            }
            //4.显示树
            GetTemplateTreeData(template.DeptCode, template.LevelCode);
        }

        public void DeleteEmrTemplateTree(int templateId, int emrDataId, string deptcode, int level)
        {
            //
            iEmrTemplateDbHelper.DeleteEmrTemplateTree(templateId);
            iEmrTemplateDbHelper.DeleteEmrFormDatabase(emrDataId);

            GetTemplateTreeData(deptcode, level);
        }

        public void GetEmrTemplateTree(EMR.Controls.Entity.EmrTemplateTree template)
        {
            //currBindKeyData.RecordId = template.ID;
            //currBindKeyData.EmrDataId = template.EmrDataId;
            currBindKeyData.ChangeEmrData(template.ID, template.EmrDataId, template.CatalogueCode,template.DeptCode,template.DeptName,template.UserCode,template.UserName);
            emrView.emrOperStyle = EmrOperStyle.默认;
            OpenDatabaseToEmr();
            emrView.btnState();
        }

        #endregion
    }

    public enum EmrControlType
    {
        病历模板, 病历预览, 病历编辑
    }

    public enum EmrOperStyle
    {
        默认,修改
    }

    public enum EmrDatastorageType
    {
        文件存储,数据库存储
    }

    /// <summary>
    /// 病历绑定关键数据
    /// </summary>
    public class EmrBindKeyData
    {
        /// <summary>
        /// 医院ID
        /// </summary>
        public int HosptialId { get; set; }
        /// <summary>
        /// 病人ID
        /// </summary>
        public int PatientId { get; set; }
        /// <summary>
        /// 当前科室
        /// </summary>
        public string CurrDeptCode { get; set; }
        public string CurrDeptName { get; set; }
        /// <summary>
        /// 登录当前用户
        /// </summary>
        public string UserCode { get; set; }
        public string UserName{ get; set; }
        /// <summary>
        /// 用户级别
        /// </summary>
        public int UserLevel { get; set; }
        public string UserLevelName { get; set; }

        /// <summary>
        /// 病历内容存储数据ID
        /// </summary>
        public int EmrDataId { get; set; }
        /// <summary>
        /// 病历类型：入院记录、首次病程记录等
        /// </summary>
        public string EmrType { get; set; }
        /// <summary>
        /// 病历ID，病人的病历记录
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// 病历标题
        /// </summary>
        public string EmrTitle { get; set; }
        /// <summary>
        /// 病人所在科室代码
        /// </summary>
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        /// <summary>
        /// 病人主管医生代码
        /// </summary>
        public string DoctorCode { get; set; }
        public string DoctorName { get; set; }

        /// <summary>
        /// 一级签名
        /// </summary>
        public int FirstSignature { get; set; }
        public int FirstDoctorLevel { get; set; }
        /// <summary>
        /// 二级签名
        /// </summary>
        public int SecondSignature { get; set; }
        public string SecondDoctorCode { get; set; }
        public int SecondDoctorLevel { get; set; }
        /// <summary>
        /// 三级签名
        /// </summary>
        public int ThreeSignature{get;set;}
        public string ThreeDoctorCode { get; set; }
        public int ThreeDoctorLevel { get; set; }
        //书写病历
        public EmrBindKeyData(int hosptialid, int patientId, string currdeptcode, string currdeptname, string usercode, string username,int userlevel,string levelname)
        {
            HosptialId = hosptialid;
            PatientId = patientId;
            //DeptCode = deptcode;
            //DeptName = deptname;
            CurrDeptCode = currdeptcode;
            CurrDeptName = currdeptname;
            UserCode = usercode;
            UserName = username;
            UserLevel = userlevel;
            UserLevelName = levelname;

            EmrDataId = -1;
        }
        //病历模板
        public EmrBindKeyData(int hosptialid, string currdeptcode, string currdeptname, string usercode, string username)
        {
            HosptialId = hosptialid;
            //DeptCode = deptcode;
            //DeptName = deptname;
            CurrDeptCode = currdeptcode;
            CurrDeptName = currdeptname;
            UserCode = usercode;
            UserName = username;

            EmrDataId = -1;
        }
        //测试
        public EmrBindKeyData(int emrDataId)
        {
            EmrDataId = emrDataId;
        }

        public EmrBindKeyData()
        {
        }

        //切换病历
        public void ChangeEmrData(int recordId, int emrDataId, string emrType, string deptcode, string deptname, string doctorcode, string doctorname)
        {
            ChangeEmrData(recordId, emrDataId, emrType, deptcode, deptname, doctorcode, doctorname, 0, 0, 0, "", 0, 0, "", 0, "");
        }
        //切换病历
        public void ChangeEmrData(int recordId, int emrDataId, string emrType, string deptcode, string deptname, string doctorcode, string doctorname, int firstSignature,int firstdoctorlevel, int secondSignature,string seconddoctorcode,int seconddoctorlevel, int threeSignature,string threedoctorcode,int threedoctorlevel, string title)
        {
            RecordId = recordId;
            EmrDataId = emrDataId;
            EmrType = emrType;
            DeptCode = deptcode;
            DeptName = deptname;
            DoctorCode = doctorcode;
            DoctorName = doctorname;
            FirstSignature = firstSignature;
            FirstDoctorLevel = firstdoctorlevel;
            SecondSignature = secondSignature;
            SecondDoctorCode = seconddoctorcode;
            SecondDoctorLevel = seconddoctorlevel;
            ThreeSignature = threeSignature;
            ThreeDoctorCode = threedoctorcode;
            ThreeDoctorLevel = threedoctorlevel;
            EmrTitle = title;
        }

        public void SignName(int type, string doctorcode, int doctorlevel)
        {
            if (type == 1)
            {
                FirstSignature = 1;
                FirstDoctorLevel = doctorlevel;
            }
            else if (type == 2)
            {
                SecondSignature = 1;
                SecondDoctorCode = doctorcode;
                SecondDoctorLevel = doctorlevel;
            }
            else if (type == 3)
            {
                ThreeSignature = 1;
                ThreeDoctorCode = doctorcode;
                ThreeDoctorLevel = doctorlevel;
            }
        }
        public void SignBack(int type)
        {
            if (type == 1)
            {
                FirstSignature = 0;
                FirstDoctorLevel = 0;
            }
            else if (type == 2)
            {
                SecondSignature = 0;
                SecondDoctorCode = "";
                SecondDoctorLevel = 0;
            }
            else if (type == 3)
            {
                ThreeSignature = 0;
                ThreeDoctorCode = "";
                ThreeDoctorLevel = 0;
            }
        }
    }

    public class EmrEditFieldData
    {
        private string _text;
        public string Text
        {
            get
            {
                _text = string.IsNullOrEmpty(_text) ? "{空}" : _text;
                return _text;
            }
            set { _text = value; }
        }

        private string _value;
        public string Value
        {
            get
            {
                _value = string.IsNullOrEmpty(_value) ? "<none>" : _value;
                return _value;
            }
            set { _value = value; }
        }

        private int _dtype;
        /// <summary>
        /// 0系统域   1知识库  2数据源
        /// </summary>
        public int dtype
        {
            get { return _dtype; }
            set { _dtype = value; }
        }

        private string _elId;
        public string elId
        {
            get { return _elId; }
            set { _elId = value; }
        }

        private InputType _inputType;
        public InputType inputType
        {
            get { return _inputType; }
            set { _inputType = value; }
        }

        public EmrEditFieldData()
        {
        }

        public EmrEditFieldData(string text, string value, string dtype, string elId, string inputType)
        {
            _text = text;
            _value = value;
            _dtype = Convert.ToInt32(dtype);
            _elId = elId;
            _inputType = (InputType)Convert.ToInt32(inputType);
        }

        public EmrEditFieldData(string fielddata, string fieldtext)
        {
            string[] arr = fielddata.Split(new char[]{'|'});
            if (arr.Length == 4)
            {
                _text = fieldtext;
                _value = arr[3];
                _dtype = Convert.ToInt32(arr[0]);
                _elId = arr[2];
                _inputType = (InputType)Convert.ToInt32(arr[1]);
            }
        }

        public void getdata(EmrControlType ctype,out string fielddata, out string fieldtext, ref string fontname, ref short fontbold)
        {
            //获取数据源值
            if (dtype == 2 && ctype==EmrControlType.病历编辑)
            {
                DataSourceManage.SetValue(elId, delegate(string _text, string _value)
                {
                    if (_text != "" || _value != "")
                    {
                        Text = _text;
                        Value = _value;
                    }
                });
            }
            //复选框
            if (dtype == 0 && (elId == "SysTrueOrFalse"))
            {
                if (Text == "o")
                {
                    fontname = "Wingdings";
                }
                else if (Text == "R" || Text == "T")
                {
                    fontbold = 1;
                    fontname = "Wingdings 2";
                }
            }
            fielddata= dtype + "|" + (int)inputType+ "|" + elId + "|" + Value;
            fieldtext = Text;
        }
    }
}
