using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prescription.Controls.Entity;
using System.Data;
using Prescription.Controls;

namespace Prescription.Controls.Action
{
    //处方数据源
    public class PrescripttionDataSource
    {
        public static IPrescripttionDbHelper PrescripttionDbHelper;
        //0-所有 1-西药 2-中药  3-处方可开的物品 4-收费项目
        public static List<CardDataSourceDrugItem> GetDrugItem(int type, int pageNo, int pageSize, string filter)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.GetDrugItem(type, pageNo, pageSize, filter);
            }
            else
            {

                PageInfo page = new PageInfo(pageSize, pageNo);
                page.KeyName = "StockID";


                string strsql = @"SELECT * FROM hisdb..View_DrugAndFeeList WHERE MatClass={1} and (PyCode LIKE '%{0}%' OR PyCodeT LIKE '%{0}%' OR WbCode LIKE '%{0}%' OR WbCodeT LIKE '%{0}%' OR CName LIKE '%{0}%')";
                strsql = string.Format(strsql, filter, type);
                DataTable dt = MidDbHelper.ExecuteDataTable(MidDbHelper.HisHandle, strsql, page);

                //DataTable dt = HIS_FeeList.GetDrugAndFeeListFromMid(0, filter);

                List<CardDataSourceDrugItem> list_DrugItem = new List<CardDataSourceDrugItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CardDataSourceDrugItem mDrugItem = new CardDataSourceDrugItem();
                    mDrugItem.ItemId = ConvertDataExtend.ToInt32(dt.Rows[i]["StockID"], 0);
                    mDrugItem.ItemName = dt.Rows[i]["CName"].ToString();
                    mDrugItem.ItemName_Print = dt.Rows[i]["TName"].ToString();
                    mDrugItem.Standard = dt.Rows[i]["Spec"].ToString();

                    mDrugItem.Scale = dt.Rows[i]["Competence"].ToString();//补偿比例
                    mDrugItem.StoreNum = ConvertDataExtend.ToDecimal(dt.Rows[i]["ActualQty"], 0);//库存量
                    mDrugItem.UnPickUnit = dt.Rows[i]["MiniUnit"].ToString();//包装单位(销售单位)
                    mDrugItem.SellPrice = ConvertDataExtend.ToDecimal(dt.Rows[i]["ClinPrice"], 0);//销售价格?
                    mDrugItem.BuyPrice = ConvertDataExtend.ToDecimal(dt.Rows[i]["WardPrice"], 0);//进价
                    //mDrugItem.ExecDeptName = dt.Rows[i][""].ToString();//执行科室?
                    mDrugItem.Pym = dt.Rows[i]["PyCode"].ToString();
                    mDrugItem.Wbm = dt.Rows[i]["WbCode"].ToString();
                    mDrugItem.Address = dt.Rows[i]["FacName"].ToString();//生产厂家
                    //mDrugItem.DoseUnitId = dt.Rows[i][""].ToString();//剂量单位
                    mDrugItem.DoseUnitName = dt.Rows[i]["DosUnit"].ToString();
                    mDrugItem.DoseConvertNum = ConvertDataExtend.ToDecimal(dt.Rows[i]["Dosage"], 0);//剂量对应包装单位系数
                    mDrugItem.ItemType = ConvertDataExtend.ToInt32(dt.Rows[i]["MatClass"], 0);//项目类型 0-所有 1-西药 2-中药  3-处方可开的物品 4-收费项目
                    //mDrugItem.StatItemCode = dt.Rows[i][""].ToString();//大项目代码
                    //mDrugItem.UnPickUnitId = dt.Rows[i][""].ToString();//包装单位ID
                    mDrugItem.ExecDeptId = ConvertDataExtend.ToInt32(dt.Rows[i]["DeptID"], 0);//执行科室ID?
                    mDrugItem.FloatFlag = 1;// dt.Rows[i][""].ToString();//按含量取整1 按剂量取整0
                    //mDrugItem.VirulentFlag = dt.Rows[i][""].ToString();//剧毒标识
                    //mDrugItem.NarcoticFlag = dt.Rows[i][""].ToString();//麻醉标识
                    mDrugItem.SkinTestFlag = ConvertDataExtend.ToInt32(dt.Rows[i]["SkinMethod"], 0);//皮试标识
                    //mDrugItem.RecipeFlag = dt.Rows[i][""].ToString();//处方标识
                    //mDrugItem.LunacyFlag = dt.Rows[i][""].ToString();//精神药品标识
                    //mDrugItem.CostlyFlag = dt.Rows[i][""].ToString();//贵重药品标识
                    mDrugItem.default_Usage_Amount = ConvertDataExtend.ToDecimal(dt.Rows[i]["BASEDos"], 0);//默认用量
                    mDrugItem.default_Usage_Id = ConvertDataExtend.ToInt32(dt.Rows[i]["UsgID"], 0);//默认用法
                    mDrugItem.default_Usage_Name = dt.Rows[i]["ChannelName"].ToString();
                    mDrugItem.default_Frequency_Id = ConvertDataExtend.ToInt32(dt.Rows[i]["FcyID"], 0);//默认频次
                    mDrugItem.default_Frequency_Name = dt.Rows[i]["FrequencyName"].ToString();

                    list_DrugItem.Add(mDrugItem);
                }
                return list_DrugItem;
            }
        }

        //根据药品ID，获取药品数据
        public static CardDataSourceDrugItem GetDrugItem(int ItemId)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.GetDrugItem(ItemId);
            }
            else
            {
                string strsql = @"SELECT top 1 * FROM hisdb..View_DrugAndFeeList WHERE StockID={0}";
                strsql = string.Format(strsql, ItemId);

                DataTable dt = MidDbHelper.ExecuteDataTable(MidDbHelper.HisHandle, strsql);

                //DataTable dt = HIS_FeeList.GetDrugAndFeeListFromMid(0, filter);

                List<CardDataSourceDrugItem> list_DrugItem = new List<CardDataSourceDrugItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CardDataSourceDrugItem mDrugItem = new CardDataSourceDrugItem();
                    mDrugItem.ItemId = Convert.ToInt32(dt.Rows[i]["StockID"]);
                    mDrugItem.ItemName = dt.Rows[i]["CName"].ToString();
                    mDrugItem.ItemName_Print = dt.Rows[i]["TName"].ToString();
                    mDrugItem.Standard = dt.Rows[i]["Spec"].ToString();

                    mDrugItem.Scale = dt.Rows[i]["Competence"].ToString();//补偿比例
                    mDrugItem.StoreNum = Convert.ToDecimal(dt.Rows[i]["ActualQty"]);//库存量
                    mDrugItem.UnPickUnit = dt.Rows[i]["MiniUnit"].ToString();//包装单位(销售单位)
                    mDrugItem.SellPrice = Convert.ToDecimal(dt.Rows[i]["ClinPrice"]);//销售价格?
                    mDrugItem.BuyPrice = Convert.ToDecimal(dt.Rows[i]["WardPrice"]);//进价
                    //mDrugItem.ExecDeptName = dt.Rows[i][""].ToString();//执行科室?
                    mDrugItem.Pym = dt.Rows[i]["PyCode"].ToString();
                    mDrugItem.Wbm = dt.Rows[i]["WbCode"].ToString();
                    mDrugItem.Address = dt.Rows[i]["FacName"].ToString();//生产厂家
                    //mDrugItem.DoseUnitId = dt.Rows[i][""].ToString();//剂量单位
                    mDrugItem.DoseUnitName = dt.Rows[i]["DosUnit"].ToString();
                    mDrugItem.DoseConvertNum = Convert.ToDecimal(dt.Rows[i]["Dosage"]);//剂量对应包装单位系数
                    mDrugItem.ItemType = Convert.ToInt32(dt.Rows[i]["MatClass"]);//项目类型 0-所有 1-西药 2-中药  3-处方可开的物品 4-收费项目
                    //mDrugItem.StatItemCode = dt.Rows[i][""].ToString();//大项目代码
                    //mDrugItem.UnPickUnitId = dt.Rows[i][""].ToString();//包装单位ID
                    mDrugItem.ExecDeptId = Convert.ToInt32(dt.Rows[i]["DeptID"]);//执行科室ID?
                    mDrugItem.FloatFlag = 1;// dt.Rows[i][""].ToString();//按含量取整1 按剂量取整0
                    //mDrugItem.VirulentFlag = dt.Rows[i][""].ToString();//剧毒标识
                    //mDrugItem.NarcoticFlag = dt.Rows[i][""].ToString();//麻醉标识
                    mDrugItem.SkinTestFlag = Convert.ToInt32(dt.Rows[i]["SkinMethod"]);//皮试标识
                    //mDrugItem.RecipeFlag = dt.Rows[i][""].ToString();//处方标识
                    //mDrugItem.LunacyFlag = dt.Rows[i][""].ToString();//精神药品标识
                    //mDrugItem.CostlyFlag = dt.Rows[i][""].ToString();//贵重药品标识
                    mDrugItem.default_Usage_Amount = Convert.ToDecimal(dt.Rows[i]["BASEDos"]);//默认用量
                    mDrugItem.default_Usage_Id = Convert.ToInt32(dt.Rows[i]["UsgID"]);//默认用法
                    mDrugItem.default_Usage_Name = dt.Rows[i]["ChannelName"].ToString();
                    mDrugItem.default_Frequency_Id = Convert.ToInt32(dt.Rows[i]["FcyID"]);//默认频次
                    mDrugItem.default_Frequency_Name = dt.Rows[i]["FrequencyName"].ToString();

                    list_DrugItem.Add(mDrugItem);
                }
                return list_DrugItem[0];
            }
        }

        public static List<CardDataSourceUsage> GetUsage()
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.GetUsage();
            }
            else
            {
                //DataTable dt = EMRCommon.GetOutChannel;

                string strsql = @"SELECT ID,ChannelName,CName,EName,PYCode,WBCode,InputCode,EnumRange
		                            FROM emrdb..BASE_Channel 
		                            WHERE HospitalID = 33  AND DeleteFlag=0 AND OutUsed='Y'
		                            ORDER BY OrderNum";
                DataTable dt = MidDbHelper.ExecuteDataTable(MidDbHelper.EmrHandle, strsql);

                List<CardDataSourceUsage> list_Usage = new List<CardDataSourceUsage>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CardDataSourceUsage mUsage = new CardDataSourceUsage();

                    mUsage.UsageId = ConvertDataExtend.ToInt32(dt.Rows[i]["ID"],0);
                    mUsage.UsageName = dt.Rows[i]["ChannelName"].ToString();
                    mUsage.Pym = dt.Rows[i]["PYCode"].ToString();
                    mUsage.Wbm = dt.Rows[i]["WBCode"].ToString();
                    //mUsage.Szm = dt.Rows[i]["ID"].ToString();
                    list_Usage.Add(mUsage);
                }
                return list_Usage;
            }
        }

        public static List<CardDataSourceFrequency> GetFrequency()
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.GetFrequency();
            }
            else
            {
                //DataTable dt = EMRCommon.dtFrequency;

                string strsql = @"SELECT ID,FrequencyName,CName,EName,NumCode,InputCode,EnumRange,ExecuteCode,DeleteFlag,PYCode,WBCode
		                        FROM emrdb..BASE_Frequency 
		                        WHERE HospitalID = 33 AND DeleteFlag = 0
		                        ORDER BY OrderNum";

                DataTable dt = MidDbHelper.ExecuteDataTable(MidDbHelper.EmrHandle, strsql);

                List<CardDataSourceFrequency> list_Frequency = new List<CardDataSourceFrequency>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CardDataSourceFrequency mFrequency = new CardDataSourceFrequency();
                    mFrequency.FrequencyId = Convert.ToInt32(dt.Rows[i]["ID"]);
                    mFrequency.Name = dt.Rows[i]["FrequencyName"].ToString();
                    mFrequency.Caption = dt.Rows[i]["ExecuteCode"].ToString();

                    int _execNum, _cycleDay;
                    CardDataSourceFrequency.Calculate(mFrequency.Caption, out _execNum, out _cycleDay);
                    mFrequency.ExecNum = _execNum;//执行次数
                    mFrequency.CycleDay = _cycleDay;//执行周期

                    mFrequency.Pym = dt.Rows[i]["InputCode"].ToString();
                    mFrequency.Wbm = dt.Rows[i]["InputCode"].ToString();
                    mFrequency.Szm = dt.Rows[i]["NumCode"].ToString();

                    list_Frequency.Add(mFrequency);
                }
                return list_Frequency;
            }
        }

        public static List<CardDataSourceEntrust> GetEntrust()
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.GetEntrust();
            }
            else
            {
                string strsql = @"SELECT ID, DeptCode, RangEnum,HelpCode, MsgContent,Uploader,WBCode,PYCode
	                            FROM emrdb..BASE_Memo 
	                            WHERE HospitalID = 33";
                DataTable dt = MidDbHelper.ExecuteDataTable(MidDbHelper.EmrHandle, strsql);

                List<CardDataSourceEntrust> list_entrust = new List<CardDataSourceEntrust>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CardDataSourceEntrust mentrust = new CardDataSourceEntrust();
                    mentrust.Id = Convert.ToInt32(dt.Rows[i]["ID"]);
                    mentrust.Name = dt.Rows[i]["MsgContent"].ToString();
                    mentrust.Pym = dt.Rows[i]["PYCode"].ToString();
                    mentrust.Wbm = dt.Rows[i]["WBCode"].ToString();
                    mentrust.Szm = "";

                    list_entrust.Add(mentrust);
                }
                return list_entrust;
            }
        }

        //stockId 药品ID  type 0.剂量单位 1.总量单位
        public static List<CardDataSourceUnit> GetUnit(int stockId, int type)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.GetUnit(stockId, type);
            }
            else
            {
                string strParam = String.Format("{0},{1},{2}", Common.CurrentHospital.HospitalID, stockId, type);
                DataTable dt = MidDbHelper.ExecuteDataTable(MidDbHelper.HisHandle, "xpGetDrugUnit", strParam);

                List<CardDataSourceUnit> list_unit = new List<CardDataSourceUnit>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CardDataSourceUnit munit = new CardDataSourceUnit();
                    munit.UnitDicId = 0;
                    munit.UnitName = dt.Rows[i]["UnitName"].ToString();
                    munit.Pym = "";
                    munit.Wbm = "";
                    munit.Factor = Convert.ToDecimal(dt.Rows[i]["UnitFactor"]);

                    list_unit.Add(munit);
                }

                return list_unit;
            }
        }

        //获取处方模板
        public static List<Entity.Prescription> GetPresTemplate(int type, int tplId)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.GetPresTemplate(type,tplId);
            }
            else
            {

                List<Entity.Prescription> list = new List<Entity.Prescription>();
                return list;
            }
        }

        //获取处方模板行
        public static Entity.Prescription GetPresTemplateRow(int type, int tpldetailId)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.GetPresTemplateRow(type,tpldetailId);
            }
            else
            {
                Entity.Prescription pres = new Entity.Prescription();
                return pres;
            }
        }

        //得到药品处方数据
        public static List<Entity.Prescription> GetPrescriptionData(int patListId, int presType)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.GetPrescriptionData(patListId, presType);
            }
            else
            {
                string strsql = @"SELECT 
                                b.ID ListID,b.PrescriptionID,b.FeeID,b.FeeName,b.ItemClass,b.Price,b.Spec,b.Dosage,b.DosageUnit,b.Factor,b.Num,b.ChannelID,b.FrequencyID,b.Days,b.Amount,b.Unit,b.PresAmount,b.PresAmountUnit,b.PresFactor ,b.GroupID,b.IsAst,b.IsTake,b.Memo 
                                ,b.ListNO,0 OrderNO,b.IsCharged,b.IsCancel,b.chargedTime
                                ,c.ChannelName,d.FrequencyName,d.ExecuteCode
                                ,(SELECT RoundingMode FROM hisdb..Dict_Medicine WHERE MedID=(SELECT MedID FROM hisdb..BASE_MedPrice WHERE StockID=b.FeeID) AND HospitalID={0}) RoundingMode
                                ,b.DoctorID,b.DeptCode,hisproc.dbo.fnGetStaffName(b.DoctorID) PresDoctorName,hisproc.dbo.fnGetDeptNameHIS({0},b.DeptCode) PresDeptName
                                FROM emrdb..DATA_PrescriptionList a 
                                LEFT JOIN emrdb..DATA_PrescriptionDetail b ON a.ID=b.PrescriptionID 
                                LEFT JOIN emrdb..BASE_Channel AS c ON b.ChannelID = c.ID
                                LEFT JOIN emrdb..BASE_Frequency AS d ON b.FrequencyID = d.ID
                                WHERE a.PatientID={1} AND a.OrderClass={2}
                                ORDER BY b.ListNO,b.GroupID";

                strsql = string.Format(strsql, 33, patListId, presType);
                DataTable dt = MidDbHelper.ExecuteDataTable(MidDbHelper.EmrHandle, strsql);

                //int _orderNo = 1;//行号
                List<Entity.Prescription> list_Prescription = new List<Entity.Prescription>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Entity.Prescription mPres = new Entity.Prescription();

                    mPres.PresListId = Convert.ToInt32(dt.Rows[i]["ListID"]);
                    mPres.PresHeadId = Convert.ToInt32(dt.Rows[i]["PrescriptionID"]);
                    //mPres.OrderNo = i + 1;//行号
                    mPres.Item_Id = Convert.ToInt32(dt.Rows[i]["FeeID"]);
                    mPres.Item_Name = dt.Rows[i]["FeeName"].ToString();
                    mPres.Item_Type = Convert.ToInt32(dt.Rows[i]["ItemClass"]);//1西药 2中药 3项目材料
                    mPres.StatItem_Code = "";
                    mPres.Sell_Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    mPres.Buy_Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    mPres.Item_Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    mPres.Standard = dt.Rows[i]["Spec"].ToString();
                    mPres.Usage_Amount = Convert.ToDecimal(dt.Rows[i]["Dosage"]);//剂量
                    mPres.Usage_Unit = dt.Rows[i]["DosageUnit"].ToString();//剂量单位
                    mPres.Usage_Rate = Convert.ToDecimal(dt.Rows[i]["Factor"]);//剂量系数
                    mPres.Dosage = Convert.ToInt32(dt.Rows[i]["Num"]); ;//付数
                    mPres.Usage_Id = Convert.ToInt32(dt.Rows[i]["ChannelID"]);
                    mPres.Frequency_Id = Convert.ToInt32(dt.Rows[i]["FrequencyID"]);
                    mPres.Days = Convert.ToInt32(dt.Rows[i]["Days"]);

                    mPres.Amount = Convert.ToDecimal(dt.Rows[i]["Amount"].ToString());//发药数量
                    mPres.Unit = dt.Rows[i]["Unit"].ToString();//发药单位

                    mPres.Item_Amount = Convert.ToInt32(Convert.ToDecimal(dt.Rows[i]["PresAmount"]));//开药数量
                    mPres.Item_Unit = dt.Rows[i]["PresAmountUnit"].ToString();//开药单位
                    mPres.Item_Rate = Convert.ToInt32(Convert.ToDecimal(dt.Rows[i]["PresFactor"]));//系数

                    mPres.Group_Id = Convert.ToInt32(dt.Rows[i]["GroupID"]);//分组组号
                    mPres.SkinTest_Flag = Convert.ToInt32(dt.Rows[i]["IsAst"]);//皮试
                    mPres.SelfDrug_Flag = Convert.ToInt32(dt.Rows[i]["IsTake"]);//自备
                    mPres.Entrust = dt.Rows[i]["Memo"].ToString();//嘱托

                    mPres.FootNote = "";
                    mPres.Tc_Flag = 0;//套餐

                    mPres.PresNo = Convert.ToInt32(dt.Rows[i]["ListNO"]);//方号
                    mPres.Dept_Id = 0;//执行科室
                    mPres.Pres_Dept = Convert.ToInt32(dt.Rows[i]["DeptCode"]);
                    mPres.Pres_DeptName = dt.Rows[i]["PresDeptName"].ToString();
                    mPres.Pres_Doc = Convert.ToInt32(dt.Rows[i]["DoctorID"]);
                    mPres.Pres_DocName = dt.Rows[i]["PresDoctorName"].ToString();
                    if (Convert.ToInt32(dt.Rows[i]["IsCancel"]) == 1)
                        mPres.Status = Prescription.Controls.Entity.PresStatus.退费状态;
                    else if (Convert.ToInt32(dt.Rows[i]["IsCharged"]) == 1)
                        mPres.Status = Prescription.Controls.Entity.PresStatus.收费状态;
                    else
                        mPres.Status = Prescription.Controls.Entity.PresStatus.保存状态;
                    mPres.Pres_Date = Convert.ToDateTime(dt.Rows[i]["chargedTime"]);

                    mPres.Usage_Name = dt.Rows[i]["ChannelName"].ToString();//用法名称
                    mPres.Frequency_Name = dt.Rows[i]["FrequencyName"].ToString();//频次名称
                    mPres.Frequency_Caption = dt.Rows[i]["ExecuteCode"].ToString();//频次名称

                    int _execNum, _cycleDay;
                    CardDataSourceFrequency.Calculate(dt.Rows[i]["ExecuteCode"].ToString(), out _execNum, out _cycleDay);
                    mPres.Frequency_ExecNum = _execNum;//执行次数
                    mPres.Frequency_CycleDay = _cycleDay;//执行周期


                    mPres.CalculateItemMoney();

                    list_Prescription.Add(mPres);
                }

                return list_Prescription;
            }
        }

        //是否收费处方
        public static bool IsCostPres(List<Entity.Prescription> list)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.IsCostPres(list);
            }
            else
            {
                if (list.Count == 0)
                    return false;

                string strsql = @"select count(*) num from emrdb..DATA_PrescriptionDetail where ID IN({0}) AND IsCharged=1";
                string Ids = null;
                for (int i = 0; i < list.Count; i++)
                {
                    if (Ids == null)
                        Ids = list[i].PresListId.ToString();
                    else
                        Ids += "," + list[i].PresListId.ToString();
                }

                strsql = string.Format(strsql, Ids);
                object ret = MidDbHelper.ExecuteScalar(MidDbHelper.EmrHandle, strsql);

                if (Convert.ToInt32(ret) > 0)
                {
                    return true;
                }

                return false;
            }
        }

        //检查药品库存是否足够
        public static bool IsDrugStore(Entity.Prescription pres)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.IsDrugStore(pres);
            }
            else
            {
                if (pres.Item_Id > 0)
                {
                    string strsql = @"SELECT  top 1 ActualQty FROM hisdb..View_DrugAndFeeList WHERE StockID ={0}";
                    strsql = string.Format(strsql, pres.Item_Id);
                    object ret = MidDbHelper.ExecuteScalar(MidDbHelper.HisHandle, strsql);

                    decimal qty = ret == DBNull.Value ? 0 : Convert.ToDecimal(ret);
                    if (pres.Amount > qty)
                        return false;
                }

                return true;
            }
        }
        //检查药品库存是否足够
        public static bool IsDrugStore(List<Entity.Prescription> list, List<Entity.Prescription> errlist)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.IsDrugStore(list, errlist);
            }
            else
            {
                if (list.Count == 0)
                {
                    return true;
                }
                string strsql = @"SELECT  StockID,ActualQty FROM hisdb..View_DrugAndFeeList WHERE StockID IN ({0})";
                string Ids = null;
                for (int i = 0; i < list.Count; i++)
                {
                    if (Ids == null)
                        Ids = list[i].Item_Id.ToString();
                    else
                        Ids += "," + list[i].Item_Id.ToString();
                }

                strsql = string.Format(strsql, Ids);
                DataTable dt = MidDbHelper.ExecuteDataTable(MidDbHelper.HisHandle, strsql);

                //errlist = new List<Entity.Prescription>();
                for (int i = 0; i < list.Count; i++)
                {
                    DataRow[] drs = dt.Select("StockID=" + list[i].Item_Id);
                    if (drs.Length == 0 || Convert.ToDecimal(drs[0]["ActualQty"]) < list[i].Amount)
                    {
                        errlist.Add(list[i]);
                    }
                }

                if (errlist.Count > 0)
                    return false;

                return true;
            }
        }

        public static bool SavePrescriptionData(int patListId, List<Entity.Prescription> list, int presType)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.SavePrescriptionData(patListId, list, presType);
            }
            else
            {
                List<string> sqllist = new List<string>();

                string strsql = @"select ID from emrdb..DATA_PrescriptionList where PatientID ={0} AND OrderClass={1}";
                strsql = string.Format(strsql, patListId, presType);
                object ret = MidDbHelper.ExecuteScalar(MidDbHelper.EmrHandle, strsql);
                int headId = ret == DBNull.Value ? 0 : Convert.ToInt32(ret);

                if (headId == 0)//先插入处方头
                {
                    strsql = @"INSERT INTO emrdb..DATA_PrescriptionList
                            ( PatientID ,
                              OrderClass ,
                              OrderTime ,
                              OrderSign ,
                              IsPriority ,
                              ApplyID ,
                              DoctorID
                            )
                    VALUES  ( {0},{1},'{2}',0 ,0 ,0 ,{3})";
                    strsql = string.Format(strsql, patListId, presType, DateTime.Now, list[0].Pres_Doc);
                    headId = MidDbHelper.ExecuteIdentify(MidDbHelper.EmrHandle, strsql);
                }


                //先删除修改数据
                strsql = @"DELETE FROM emrdb..DATA_PrescriptionDetail WHERE ID IN({0})";
                string Ids = null;
                for (int i = 0; i < list.Count; i++)
                {
                    if (Ids == null)
                        Ids = list[i].PresListId.ToString();
                    else
                        Ids += "," + list[i].PresListId.ToString();
                }
                strsql = string.Format(strsql, Ids);
                //MidDbHelper.ExecuteNonQuery(MidDbHelper.EmrHandle, strsql);
                sqllist.Add(strsql);

                for (int i = 0; i < list.Count; i++)
                {
                    strsql = @"INSERT INTO emrdb..DATA_PrescriptionDetail(PrescriptionID ,ListNO ,GroupID ,FeeID ,FeeName ,Spec ,Dosage ,DosageUnit ,Factor ,ChannelID ,FrequencyID ,Num ,Amount ,Unit ,Price ,Days ,InjectTimes ,IsOtherFee ,IsAst ,AstResult ,IsTake ,Memo ,IsCharged ,IsCancel ,IsSend ,IsExec ,DoctorID ,DeptCode ,chargedTime ,sendTime ,CostID ,FavorFee ,DosageType ,AmountType ,PresAmount ,PresAmountUnit ,PresFactor ,ItemClass)
                            VALUES  ( {0} ,{1} ,{2} ,{3} ,'{4}' ,'{5}' ,{6} ,'{7}' ,{8} ,{9} ,{10},{11} ,{12} ,'{13}' ,{14} ,{15},0 ,0 ,{16} ,'' ,{17} ,'{18}' ,0 ,0 ,0 ,0 ,{19} ,'{20}' ,NULL ,NULL ,0 ,0 ,0 ,0 ,{21} ,'{22}' ,'{23}' ,1)";
                    strsql = string.Format(strsql, headId, list[i].PresNo, list[i].Group_Id, list[i].Item_Id, list[i].Item_Name, list[i].Standard, list[i].Usage_Amount, list[i].Usage_Unit, list[i].Usage_Rate,
                        list[i].Usage_Id, list[i].Frequency_Id, list[i].Dosage, list[i].Amount, list[i].Unit, list[i].Sell_Price, list[i].Days
                        , list[i].SkinTest_Flag, list[i].SelfDrug_Flag, list[i].Entrust, list[i].Pres_Doc, list[i].Pres_Dept, list[i].Item_Amount, list[i].Item_Unit, list[i].Item_Rate);

                    sqllist.Add(strsql);
                }

                MidDbHelper.ExecuteNonQuery(MidDbHelper.EmrSysID, sqllist);
                return true;
            }
        }

        public static bool DeletePrescriptionData(int presListId)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.DeletePrescriptionData(presListId);
            }
            else
            {
                string strsql = @"DELETE FROM emrdb..DATA_PrescriptionDetail WHERE ID ={0} and IsCharged=0";
                strsql = string.Format(strsql, presListId);
                MidDbHelper.ExecuteNonQuery(MidDbHelper.EmrHandle, strsql);
                return true;
            }
        }

        public static bool DeletePrescriptionData(int patListId, int presType, int PresNo)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.DeletePrescriptionData(patListId, presType, PresNo);
            }
            else
            {
                string strsql = @"DELETE FROM emrdb..DATA_PrescriptionDetail WHERE IsCharged=0 
                                AND PrescriptionID IN(SELECT ID FROM emrdb..DATA_PrescriptionList WHERE PatientID={0} AND OrderClass={1})
                                AND ListNO={2}";
                strsql = string.Format(strsql, patListId, presType, PresNo);
                MidDbHelper.ExecuteNonQuery(MidDbHelper.EmrHandle, strsql);
                return true;
            }
        }

        public static bool UpdatePresNoAndGroupId(List<Entity.Prescription> data)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.UpdatePresNoAndGroupId(data);
            }
            else
            {
                List<string> sqllist = new List<string>();
                for (int i = 0; i < data.Count; i++)
                {
                    string strsql = @"UPDATE emrdb..DATA_PrescriptionDetail SET ListNO={1},GroupID={2} WHERE ID={0}";
                    strsql = string.Format(strsql, data[i].PresListId, data[i].PresNo, data[i].Group_Id);
                    sqllist.Add(strsql);
                }

                MidDbHelper.ExecuteNonQuery(MidDbHelper.EmrSysID, sqllist);
                return true;
            }
        }

        public static bool UpdatePresSelfDrugFlag(int presListId, int Flag)
        {
            if (PrescripttionDbHelper != null)
            {
                return PrescripttionDbHelper.UpdatePresSelfDrugFlag(presListId, Flag);
            }
            else
            {
                string strsql = @"UPDATE emrdb..DATA_PrescriptionDetail SET IsTake={1} WHERE ID={0}";
                strsql = string.Format(strsql, presListId, Flag);
                MidDbHelper.ExecuteNonQuery(MidDbHelper.EmrHandle, strsql);
                return true;
            }
        }
    }
}
