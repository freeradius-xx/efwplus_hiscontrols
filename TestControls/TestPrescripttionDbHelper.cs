using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BaseControls;
using Prescription.Controls;
using Prescription.Controls.Entity;

namespace TestControls
{
    public class TestPrescripttionDbHelper : IPrescripttionDbHelper
    {
        private SqlDbHelper oleDb;
        public TestPrescripttionDbHelper()
        {
            oleDb = new SqlDbHelper();
            oleDb.ConnectionString = "Data Source=.;Initial Catalog=EFWDB;User ID=sa;pwd=1;";
        }

        #region IPrescripttionDbHelper 成员

        private List<CardDataSourceDrugItem> GetDrugItemToDatabase(int type, int pageNo, int pageSize, string filter)
        {
            PageInfo page = new PageInfo(pageSize, pageNo);
            page.KeyName = "StockID";

            string strsql = "";
            strsql = @"SELECT * FROM ViewBaseData_Med T WHERE  (PyCode LIKE '%{0}%' OR PyCodeT LIKE '%{0}%' OR WbCode LIKE '%{0}%' OR WbCodeT LIKE '%{0}%' OR CName LIKE '%{0}%' OR TName LIKE '%{0}%')";
            strsql = string.Format(strsql, filter);
            strsql = SqlPage.FormatSql(strsql, page, delegate(string sql)
            {
                int toltal = 0;
                SqlDataReader sdr = oleDb.GetSqlDataReader(sql);
                if (sdr.Read())
                {
                    toltal = (int)sdr.GetValue(0);
                }
                sdr.Close();
                return toltal;
            });
            DataTable dt = oleDb.GetDataTable(strsql);

            List<CardDataSourceDrugItem> list_DrugItem = new List<CardDataSourceDrugItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CardDataSourceDrugItem mDrugItem = new CardDataSourceDrugItem();
                mDrugItem.ItemId = ConvertDataExtend.ToInt32(dt.Rows[i]["StockID"], 0);
                mDrugItem.ItemName = dt.Rows[i]["CName"].ToString();
                mDrugItem.ItemName_Print = dt.Rows[i]["TName"].ToString();
                mDrugItem.Standard = dt.Rows[i]["Spec"].ToString();

                mDrugItem.Scale = "";//补偿比例
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
                mDrugItem.FloatFlag = ConvertDataExtend.ToInt32(dt.Rows[i]["RoundingMode"], 0);// dt.Rows[i][""].ToString();//按含量取整1 按剂量取整0
                //mDrugItem.VirulentFlag = dt.Rows[i][""].ToString();//剧毒标识
                //mDrugItem.NarcoticFlag = dt.Rows[i][""].ToString();//麻醉标识
                mDrugItem.SkinTestFlag = ConvertDataExtend.ToInt32(dt.Rows[i]["SkinMethod"], 0);//皮试标识
                //mDrugItem.RecipeFlag = dt.Rows[i][""].ToString();//处方标识
                //mDrugItem.LunacyFlag = dt.Rows[i][""].ToString();//精神药品标识
                //mDrugItem.CostlyFlag = dt.Rows[i][""].ToString();//贵重药品标识
                mDrugItem.default_Usage_Amount = 0;//默认用量
                mDrugItem.default_Usage_Id = 0;//默认用法
                mDrugItem.default_Usage_Name = "";
                mDrugItem.default_Frequency_Id = 0;//默认频次
                mDrugItem.default_Frequency_Name = "";

                list_DrugItem.Add(mDrugItem);
            }
            return list_DrugItem;
        }

        public List<CardDataSourceDrugItem> GetDrugItem(int type, int pageNo, int pageSize, string filter)
        {
            return GetDrugItemToDatabase(type, pageNo, pageSize, filter);
        }

        public CardDataSourceDrugItem GetDrugItem(int ItemId)
        {
            string strsql = @"SELECT TOP 1* FROM ViewBaseData_Med T
                                        WHERE StockID={0}";
            strsql = string.Format(strsql, ItemId);

            DataTable dt = oleDb.GetDataTable(strsql);
            List<CardDataSourceDrugItem> list_DrugItem = new List<CardDataSourceDrugItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CardDataSourceDrugItem mDrugItem = new CardDataSourceDrugItem();
                mDrugItem.ItemId = ConvertDataExtend.ToInt32(dt.Rows[i]["StockID"], 0);
                mDrugItem.ItemName = dt.Rows[i]["CName"].ToString();
                mDrugItem.ItemName_Print = dt.Rows[i]["TName"].ToString();
                mDrugItem.Standard = dt.Rows[i]["Spec"].ToString();

                mDrugItem.Scale = "";//补偿比例
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
                mDrugItem.FloatFlag = ConvertDataExtend.ToInt32(dt.Rows[i]["RoundingMode"], 0);// dt.Rows[i][""].ToString();//按含量取整1 按剂量取整0
                //mDrugItem.VirulentFlag = dt.Rows[i][""].ToString();//剧毒标识
                //mDrugItem.NarcoticFlag = dt.Rows[i][""].ToString();//麻醉标识
                mDrugItem.SkinTestFlag = ConvertDataExtend.ToInt32(dt.Rows[i]["SkinMethod"], 0);//皮试标识
                //mDrugItem.RecipeFlag = dt.Rows[i][""].ToString();//处方标识
                //mDrugItem.LunacyFlag = dt.Rows[i][""].ToString();//精神药品标识
                //mDrugItem.CostlyFlag = dt.Rows[i][""].ToString();//贵重药品标识
                mDrugItem.default_Usage_Amount = 0;//默认用量
                mDrugItem.default_Usage_Id = 0;//默认用法
                mDrugItem.default_Usage_Name = "";
                mDrugItem.default_Frequency_Id = 0;//默认频次
                mDrugItem.default_Frequency_Name = "";

                list_DrugItem.Add(mDrugItem);
            }
            return list_DrugItem.Count > 0 ? list_DrugItem[0] : null;
        }

        public List<CardDataSourceUsage> GetUsage()
        {
            string strsql = @"SELECT ID,ChannelName,CName,EName,PYCode,WBCode,InputCode
		                            FROM Clinic_Channel 
		                            WHERE  DeleteFlag=0 
		                            ORDER BY OrderNum";
            DataTable dt = oleDb.GetDataTable(strsql);

            List<CardDataSourceUsage> list_Usage = new List<CardDataSourceUsage>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CardDataSourceUsage mUsage = new CardDataSourceUsage();

                mUsage.UsageId = ConvertDataExtend.ToInt32(dt.Rows[i]["ID"], 0);
                mUsage.UsageName = dt.Rows[i]["ChannelName"].ToString();
                mUsage.Pym = dt.Rows[i]["PYCode"].ToString();
                mUsage.Wbm = dt.Rows[i]["WBCode"].ToString();
                //mUsage.Szm = dt.Rows[i]["ID"].ToString();
                list_Usage.Add(mUsage);
            }
            return list_Usage;
        }

        public List<CardDataSourceFrequency> GetFrequency()
        {
            string strsql = @"SELECT ID,FrequencyName,CName,EName,NumCode,InputCode,ExecuteCode,DeleteFlag,PYCode,WBCode
		                        FROM Clinic_Frequency 
		                        WHERE  DeleteFlag = 0
		                        ORDER BY OrderNum";

            DataTable dt = oleDb.GetDataTable(strsql);

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

        public List<CardDataSourceEntrust> GetEntrust()
        {
            string strsql = @"SELECT ID, DeptCode,HelpCode, MsgContent,Uploader,WBCode,PYCode
	                            FROM Clinic_Memo ";
            DataTable dt = oleDb.GetDataTable(strsql);

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

        public List<CardDataSourceUnit> GetUnit(int stockId, int type)
        {
            string strsql = @"select MiniUnit,PackUnit,DosUnit,PackAmount,Dosage from ViewBaseData_Med WHERE StockID=" + stockId;
            DataTable dtunit = oleDb.GetDataTable(strsql);
            List<CardDataSourceUnit> list_unit = new List<CardDataSourceUnit>();
            if (type == 0)//剂量
            {
                CardDataSourceUnit munit = new CardDataSourceUnit();
                munit.UnitDicId = 0;
                munit.UnitName = dtunit.Rows[0]["DosUnit"].ToString();
                munit.Pym = "";
                munit.Wbm = "";
                munit.Factor = Convert.ToDecimal(dtunit.Rows[0]["Dosage"]);
                list_unit.Add(munit);

                munit = new CardDataSourceUnit();
                munit.UnitDicId = 0;
                munit.UnitName = dtunit.Rows[0]["MiniUnit"].ToString();
                munit.Pym = "";
                munit.Wbm = "";
                munit.Factor = 1;
                list_unit.Add(munit);
            }
            else//总量
            {
                CardDataSourceUnit munit = new CardDataSourceUnit();
                munit.UnitDicId = 0;
                munit.UnitName = dtunit.Rows[0]["MiniUnit"].ToString();
                munit.Pym = "";
                munit.Wbm = "";
                munit.Factor = 1;
                list_unit.Add(munit);

                munit = new CardDataSourceUnit();
                munit.UnitDicId = 0;
                munit.UnitName = dtunit.Rows[0]["PackUnit"].ToString();
                munit.Pym = "";
                munit.Wbm = "";
                munit.Factor = Convert.ToDecimal(dtunit.Rows[0]["PackAmount"]);
                list_unit.Add(munit);
            }

            return list_unit;
        }

        public List<Prescription.Controls.Entity.Prescription> GetPresTemplate(int type, int tplId)
        {
            string strsql = "";
            strsql = @"SELECT TOP 100 A.GroupID,
	                                A.FeeID,A.FeeName,A.FeeClass, 
	                                A.Dosage,A.DosageUnit,
	                                A.Amount,A.Unit,
	                                A.FrequencyID,C.FrequencyName AS FrequencyName,C.ExecuteCode,
	                                A.ChannelID,B.CName AS ChannelName,
	                                A.Days Num, A.Days,A.Memo
	                                ,D.ClinPrice,D.Spec,D.Dosage DoseConvertNum,D.MatClass,D.SkinMethod IsAst
	                                FROM Clinic_SetMealDetail A
		                                LEFT JOIN Clinic_Channel B ON A.ChannelID = B.ID
		                                LEFT JOIN Clinic_Frequency C ON A.FrequencyID = C.ID 
		                                LEFT JOIN ViewBaseData_Med D ON A.FeeID=D.StockID 
	                                 WHERE ListID = {0} 
	                                 ORDER BY A.GroupID,A.OrderNum";
            strsql = string.Format(strsql, tplId);

            DataTable dt = oleDb.GetDataTable(strsql);
            List<Prescription.Controls.Entity.Prescription> list_Prescription = new List<Prescription.Controls.Entity.Prescription>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Prescription.Controls.Entity.Prescription mPres = new Prescription.Controls.Entity.Prescription();

                mPres.PresListId = 0;
                mPres.PresHeadId = 0;
                //mPres.OrderNo = i + 1;//行号
                mPres.Item_Id = Convert.ToInt32(dt.Rows[i]["FeeID"]);
                mPres.Item_Name = dt.Rows[i]["FeeName"].ToString();
                mPres.Item_Type = type;//1西药 2中药 3项目材料
                mPres.StatItem_Code = "";
                mPres.Sell_Price = ConvertDataExtend.ToDecimal(dt.Rows[i]["ClinPrice"], 0);
                mPres.Buy_Price = ConvertDataExtend.ToDecimal(dt.Rows[i]["ClinPrice"], 0);
                mPres.Item_Price = ConvertDataExtend.ToDecimal(dt.Rows[i]["ClinPrice"], 0);
                mPres.Standard = dt.Rows[i]["Spec"].ToString();
                mPres.Usage_Amount = Convert.ToDecimal(dt.Rows[i]["Dosage"]);//剂量
                mPres.Usage_Unit = dt.Rows[i]["DosageUnit"].ToString();//剂量单位
                mPres.Usage_Rate = ConvertDataExtend.ToDecimal(dt.Rows[i]["DoseConvertNum"], 1);//剂量系数
                mPres.Dosage = Convert.ToInt32(dt.Rows[i]["Num"]);//付数
                mPres.Usage_Id = Convert.ToInt32(dt.Rows[i]["ChannelID"]);
                mPres.Frequency_Id = Convert.ToInt32(dt.Rows[i]["FrequencyID"]);
                mPres.Days = Convert.ToInt32(dt.Rows[i]["Days"]);

                mPres.Amount = Convert.ToDecimal(dt.Rows[i]["Amount"].ToString());//发药数量
                mPres.Unit = dt.Rows[i]["Unit"].ToString();//发药单位

                mPres.Item_Amount = Convert.ToInt32(Convert.ToDecimal(dt.Rows[i]["Amount"]));//开药数量
                mPres.Item_Unit = dt.Rows[i]["Unit"].ToString();//开药单位
                mPres.Item_Rate = 1;//系数

                mPres.Group_Id = Convert.ToInt32(dt.Rows[i]["GroupID"]);//分组组号
                mPres.SkinTest_Flag = ConvertDataExtend.ToInt32(dt.Rows[i]["IsAst"], 0);//皮试
                mPres.SelfDrug_Flag = 0;//自备
                mPres.Entrust = dt.Rows[i]["Memo"].ToString();//嘱托

                mPres.FootNote = "";
                mPres.Tc_Flag = 0;//套餐

                mPres.Usage_Name = dt.Rows[i]["ChannelName"].ToString();//用法名称
                mPres.Frequency_Name = dt.Rows[i]["FrequencyName"].ToString();//频次名称
                mPres.Frequency_Caption = dt.Rows[i]["ExecuteCode"].ToString();//频次名称

                int _execNum, _cycleDay;
                CardDataSourceFrequency.Calculate(dt.Rows[i]["ExecuteCode"].ToString(), out _execNum, out _cycleDay);
                mPres.Frequency_ExecNum = _execNum;//执行次数
                mPres.Frequency_CycleDay = _cycleDay;//执行周期


                mPres.CalculateAmount(null);

                list_Prescription.Add(mPres);
            }

            return list_Prescription;
            //throw new NotImplementedException();
        }

        public Prescription.Controls.Entity.Prescription GetPresTemplateRow(int type, int itemId)
        {
            CardDataSourceDrugItem durgItem = GetDrugItem(itemId);
            if (durgItem == null)
                throw new Exception("该项目已停用，不能开出！");
            Prescription.Controls.Entity.Prescription mPres = new Prescription.Controls.Entity.Prescription();
            mPres.Item_Id = ConvertDataExtend.ToInt32(durgItem.ItemId, -1);
            mPres.Item_Name = ConvertDataExtend.ToString(durgItem.ItemName, "");
            mPres.Item_Type = type;//Convert.ToInt32(durgItem.ItemType);//1西药 2中药 3项目材料
            mPres.StatItem_Code = ConvertDataExtend.ToString(durgItem.StatItemCode, ""); ;
            mPres.Sell_Price = durgItem.SellPrice;
            mPres.Buy_Price = durgItem.BuyPrice;
            mPres.Item_Price = durgItem.SellPrice;
            mPres.Standard = durgItem.Standard;
            mPres.Usage_Amount = durgItem.default_Usage_Id;//剂量
            mPres.Usage_Unit = durgItem.DoseUnitName;//剂量单位
            mPres.Usage_Rate = durgItem.DoseConvertNum;//剂量系数
            if (mPres.IsHerb)
                mPres.Dosage = 1;//付数
            else
                mPres.Dosage = 0;
            mPres.Usage_Id = durgItem.default_Usage_Id;
            mPres.Frequency_Id = durgItem.default_Frequency_Id;
            mPres.Days = Convert.ToInt32(1);

            //mPres.Amount = 0;//发药数量
            mPres.Unit = durgItem.UnPickUnit;//发药单位

            mPres.Item_Amount = Convert.ToInt32(1);//开药数量
            mPres.Item_Unit = durgItem.UnPickUnit;//开药单位
            mPres.Item_Rate = Convert.ToInt32(1);//系数

            mPres.SkinTest_Flag = durgItem.SkinTestFlag;//皮试
            mPres.SelfDrug_Flag = Convert.ToInt32(0);//自备
            mPres.Entrust = "";//嘱托

            mPres.FootNote = "";
            mPres.Tc_Flag = 0;//套餐

            mPres.Pres_Date = Convert.ToDateTime(DateTime.Now);

            mPres.Usage_Name = durgItem.default_Usage_Name;//用法名称
            mPres.Frequency_Name = durgItem.default_Frequency_Name;//频次名称

            string _caption = "";
            CardDataSourceFrequency freq = GetFrequency().Find(x => x.FrequencyId == mPres.Usage_Id);
            if (freq != null)
                _caption = freq.Caption;

            //string _caption = CardDataSource.Tables["frequencydic"].Select("FrequencyId=" + mPres.Usage_Id)[0]["Caption"].ToString();
            mPres.Frequency_Caption = _caption;//频次名称

            int _execNum, _cycleDay;
            CardDataSourceFrequency.Calculate(_caption, out _execNum, out _cycleDay);
            mPres.Frequency_ExecNum = _execNum;//执行次数
            mPres.Frequency_CycleDay = _cycleDay;//执行周期

            mPres.IsFloat = true;//row["RoundingMode"]

            mPres.Usage_Amount = mPres.Usage_Amount <= 0 ? 1 : mPres.Usage_Amount;
            mPres.CalculateAmount(durgItem.UnPickUnit);//计算数量 和 金额

            return mPres;
            //throw new NotImplementedException();
        }

        public bool IsCostPres(List<Prescription.Controls.Entity.Prescription> list)
        {
            if (list.Count == 0)
                return false;

            string strsql = @"select count(*) num from Clinic_PrescriptionDetail where ID IN({0}) AND IsCharged=1";
            string Ids = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (Ids == null)
                    Ids = list[i].PresListId.ToString();
                else
                    Ids += "," + list[i].PresListId.ToString();
            }

            strsql = string.Format(strsql, Ids);
            object ret = oleDb.GetDataResult(strsql);

            if (Convert.ToInt32(ret) > 0)
            {
                return true;
            }

            return false;
        }

        public bool IsDrugStore(Prescription.Controls.Entity.Prescription pres)
        {
            if (pres.Item_Id > 0 && pres.IsDrug == true)
            {
                string strsql = @"SELECT  top 1 ActualQty FROM ViewBaseData_Med WHERE StockID ={0}";
                strsql = string.Format(strsql, pres.Item_Id);
                object ret = oleDb.GetDataResult(strsql);

                decimal qty = ret == DBNull.Value ? 0 : Convert.ToDecimal(ret);
                if (pres.Amount > qty)
                    return false;
            }

            return true;
        }

        public bool IsDrugStore(List<Prescription.Controls.Entity.Prescription> list, List<Prescription.Controls.Entity.Prescription> errlist)
        {
            list = list.FindAll(x => x.IsDrug == true);
            if (list.Count == 0)
            {
                return true;
            }
            string strsql = @"SELECT  StockID,ActualQty FROM ViewBaseData_Med WHERE StockID IN ({0}) ";
            string Ids = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (Ids == null)
                    Ids = list[i].Item_Id.ToString();
                else
                    Ids += "," + list[i].Item_Id.ToString();
            }

            strsql = string.Format(strsql, Ids);
            DataTable dt = oleDb.GetDataTable(strsql);

            //errlist = new List<Entity.Prescription>();
            for (int i = 0; i < list.Count; i++)
            {
                DataRow[] drs = dt.Select("StockID=" + list[i].Item_Id);
                if (drs.Length == 0 || ConvertDataExtend.ToDecimal(drs[0]["ActualQty"], 0) < list[i].Amount)
                {
                    errlist.Add(list[i]);
                }
            }

            if (errlist.Count > 0)
                return false;

            return true;
        }

        public List<Prescription.Controls.Entity.Prescription> GetPrescriptionData(int patListId, int presType)
        {
            string strsql = @"SELECT 
                                b.ID ListID,b.PrescriptionID,b.FeeID,b.FeeName,b.Price,b.Spec,b.Dosage,b.DosageUnit,b.Factor,b.Num,b.ChannelID,b.FrequencyID,b.Days,b.Amount,b.Unit,b.PresAmount,b.PresAmountUnit,b.PresFactor ,b.GroupID,b.IsAst,b.IsTake,b.Memo 
                                ,b.ListNO,0 OrderNO,b.IsCharged,b.IsCancel
                                ,c.ChannelName,d.FrequencyName,d.ExecuteCode
                                ,1 RoundingMode
                                ,b.DoctorID,b.DeptCode,'' PresDoctorName,'' PresDeptName
                                FROM Clinic_PrescriptionList a 
                                LEFT JOIN Clinic_PrescriptionDetail b ON a.ID=b.PrescriptionID 
                                LEFT JOIN Clinic_Channel AS c ON b.ChannelID = c.ID
                                LEFT JOIN Clinic_Frequency AS d ON b.FrequencyID = d.ID
                                WHERE a.PatientID={0} AND a.OrderClass={1} AND b.ID IS NOT NULL
                                ORDER BY b.ListNO,b.GroupID";

            strsql = string.Format(strsql, patListId, presType);
            DataTable dt = oleDb.GetDataTable(strsql);

            //int _orderNo = 1;//行号
            List<Prescription.Controls.Entity.Prescription> list_Prescription = new List<Prescription.Controls.Entity.Prescription>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Prescription.Controls.Entity.Prescription mPres = new Prescription.Controls.Entity.Prescription();

                mPres.PresListId = Convert.ToInt32(dt.Rows[i]["ListID"]);
                mPres.PresHeadId = Convert.ToInt32(dt.Rows[i]["PrescriptionID"]);
                //mPres.OrderNo = i + 1;//行号
                mPres.Item_Id = Convert.ToInt32(dt.Rows[i]["FeeID"]);
                mPres.Item_Name = dt.Rows[i]["FeeName"].ToString();
                mPres.Item_Type = 1;// Convert.ToInt32(dt.Rows[i]["ItemClass"]);//1西药 2中药 3项目材料
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
                    mPres.Status = PresStatus.退费状态;
                else if (Convert.ToInt32(dt.Rows[i]["IsCharged"]) == 1)
                    mPres.Status = PresStatus.收费状态;
                else
                    mPres.Status = PresStatus.保存状态;
                //mPres.Pres_Date = Convert.ToDateTime(dt.Rows[i]["chargedTime"]);

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

        public bool SavePrescriptionData(int patListId, List<Prescription.Controls.Entity.Prescription> list, int presType)
        {
            List<string> sqllist = new List<string>();

            string strsql = @"select ID from Clinic_PrescriptionList where PatientID ={0} AND OrderClass={1}";
            strsql = string.Format(strsql, patListId, presType);
            object ret = oleDb.GetDataResult(strsql);
            int headId = ret == DBNull.Value ? 0 : Convert.ToInt32(ret);

            if (headId == 0)//先插入处方头
            {
                strsql = @"INSERT INTO Clinic_PrescriptionList
                            ( PatientID ,
                              OrderClass ,
                              OrderTime)
                    VALUES  ( {0},{1},'{2}')";
                strsql = string.Format(strsql, patListId, presType, DateTime.Now);
                headId = oleDb.InsertSql(strsql);
            }


            //先删除修改数据
            strsql = @"DELETE FROM Clinic_PrescriptionDetail WHERE ID IN({0})";
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
                strsql = @"INSERT INTO Clinic_PrescriptionDetail(PrescriptionID ,ListNO ,GroupID ,FeeID ,FeeName ,Spec ,Dosage ,DosageUnit ,Factor ,ChannelID ,FrequencyID ,Num ,Amount ,Unit ,Price ,Days   ,IsAst ,AstResult ,IsTake ,Memo ,IsCharged ,IsCancel,IsSend ,DoctorID ,DeptCode   ,CostID ,FavorFee   ,PresAmount ,PresAmountUnit ,PresFactor )
                                                                                            VALUES  ( 
                                                                                            {0} --PrescriptionID
                                                                                            ,{1}--ListNO
                                                                                            ,{2} --GroupID
                                                                                            ,{3} --FeeID
                                                                                            ,'{4}' --FeeName
                                                                                            ,'{5}'--Spec
                                                                                            ,{6}--Dosage
                                                                                            ,'{7}'--DosageUnit
                                                                                            ,{8} ,{9} ,{10},{11} ,{12} --Amount
                                                                                            ,'{13}' ,{14} ,{15} --Days
                                                                                            ,{16}--IsAst
                                                                                            ,'' --AstResult
                                                                                            ,{17} ,'{18}' ,0 ,0 ,0,{19} --DoctorID
                                                                                            ,'{20}' --DeptCode
                                                                                            ,0 --CostID
                                                                                            ,0 --FavorFee
                                                                                            ,{21} --PresAmount
                                                                                            ,'{22}'--PresAmountUnit
                                                                                            ,{23}--PresFactor
                                                                                            )";
                strsql = string.Format(strsql,
                    headId//0
                    , list[i].PresNo//1
                    , list[i].Group_Id
                    , list[i].Item_Id
                    , list[i].Item_Name
                    , list[i].Standard//5
                    , list[i].Usage_Amount, list[i].Usage_Unit, list[i].Usage_Rate//8
                    , list[i].Usage_Id, list[i].Frequency_Id, list[i].Dosage, list[i].Amount//12
                    , list[i].Unit, list[i].Sell_Price, list[i].Days//15
                    , list[i].SkinTest_Flag//16
                    , list[i].SelfDrug_Flag, list[i].Entrust, list[i].Pres_Doc//19
                    , list[i].Pres_Dept//20
                    , list[i].Item_Amount//21
                    , list[i].Item_Unit//22
                    , list[i].Item_Rate//23
                    );

                sqllist.Add(strsql);
            }

            for (int i = 0; i < sqllist.Count; i++)
            {
                oleDb.ExecuteNoQuery(sqllist[i]);
            }
            return true;
        }

        public bool DeletePrescriptionData(int presListId)
        {
            string strsql = @"DELETE FROM Clinic_PrescriptionDetail WHERE ID ={0} and IsCharged=0";
            strsql = string.Format(strsql, presListId);
            oleDb.ExecuteNoQuery(strsql);
            return true;
        }

        public bool DeletePrescriptionData(int patListId, int presType, int PresNo)
        {
            string strsql = @"DELETE FROM Clinic_PrescriptionDetail WHERE IsCharged=0 
                                AND PrescriptionID IN(SELECT ID FROM Clinic_PrescriptionList WHERE PatientID={0} AND OrderClass={1})
                                AND ListNO={2}";
            strsql = string.Format(strsql, patListId, presType, PresNo);
            oleDb.ExecuteNoQuery(strsql);
            return true;
        }

        public bool UpdatePresNoAndGroupId(List<Prescription.Controls.Entity.Prescription> data)
        {
            List<string> sqllist = new List<string>();
            for (int i = 0; i < data.Count; i++)
            {
                string strsql = @"UPDATE Clinic_PrescriptionDetail SET ListNO={1},GroupID={2} WHERE ID={0}";
                strsql = string.Format(strsql, data[i].PresListId, data[i].PresNo, data[i].Group_Id);
                sqllist.Add(strsql);
            }

            for (int i = 0; i < sqllist.Count; i++)
            {
                oleDb.ExecuteNoQuery(sqllist[i]);
            }
            return true;
        }

        public bool UpdatePresSelfDrugFlag(int presListId, int Flag)
        {
            string strsql = @"UPDATE Clinic_PrescriptionDetail SET IsTake={1} WHERE ID={0}";
            strsql = string.Format(strsql, presListId, Flag);
            oleDb.ExecuteNoQuery(strsql);
            return true;
        }

        #endregion

        #region IPrescripttionDbHelper 成员


        public DataTable LoadTemplateList(int deptId, int doctorId, int mealCls)
        {
            string strsql = @"SELECT ID,ParentID,LevelValue,NodeName,DeptCode,StaffID,PYCode,WBCode
	                                    FROM Clinic_SetMealList 
	                                    WHERE (LevelValue = 1)OR(LevelValue = 2 AND DeptCode = {0})OR(LevelValue = 3 AND StaffID = {1}) OR 1=1";
            strsql = string.Format(strsql, deptId, doctorId);
            return oleDb.GetDataTable(strsql);
        }

        public DataTable LoadTemplateDetail(int tplId)
        {
            string strsql = @"SELECT  1 ck ,A.ID,A.ListID,A.GroupID,A.OrderNum,
	                A.FeeID,A.FeeName,A.FeeClass, 
	                A.Dosage,A.DosageUnit,
	                A.Amount,A.Unit,
	                A.FrequencyID,C.CName AS FrequencyName,
	                ChannelID,B.CName AS ChannelName,
	                Days,Memo
	                FROM Clinic_SetMealDetail A
		                LEFT JOIN Clinic_Channel B ON A.ChannelID = B.ID 
		                LEFT JOIN Clinic_Frequency C ON A.FrequencyID = C.ID 
	                 WHERE ListID = {0} ORDER BY A.GroupID,A.OrderNum";

            strsql = string.Format(strsql, tplId);
            return oleDb.GetDataTable(strsql);
        }



        public List<Prescription.Controls.Entity.Prescription> GetPresTemplateRow(int type, int[] tpldetailIds)
        {
            if (tpldetailIds.Length == 0) return new List<Prescription.Controls.Entity.Prescription>();

            string strsql = "";
            strsql = @"SELECT TOP 100 A.GroupID,
	                                A.FeeID,A.FeeName,A.FeeClass, 
	                                A.Dosage,A.DosageUnit,
	                                A.Amount,A.Unit,
	                                A.FrequencyID,C.FrequencyName AS FrequencyName,C.ExecuteCode,
	                                A.ChannelID,B.CName AS ChannelName,
	                                A.Days Num, A.Days,A.Memo
	                                ,D.ClinPrice,D.Spec,D.Dosage DoseConvertNum,D.MatClass,D.SkinMethod IsAst
	                                FROM Clinic_SetMealDetail A
		                                LEFT JOIN Clinic_Channel B ON A.ChannelID = B.ID
		                                LEFT JOIN Clinic_Frequency C ON A.FrequencyID = C.ID 
		                                LEFT JOIN ViewBaseData_Med D ON A.FeeID=D.StockID 
	                                 WHERE A.ID in ({0})
	                                 ORDER BY A.GroupID,A.OrderNum";
            strsql = string.Format(strsql,
            string.Join(",", Array.ConvertAll<int, string>(tpldetailIds, delegate(int v)
            {
                return v.ToString();
            })));
            

            DataTable dt = oleDb.GetDataTable(strsql);
            List<Prescription.Controls.Entity.Prescription> list_Prescription = new List<Prescription.Controls.Entity.Prescription>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Prescription.Controls.Entity.Prescription mPres = new Prescription.Controls.Entity.Prescription();

                mPres.PresListId = 0;
                mPres.PresHeadId = 0;
                //mPres.OrderNo = i + 1;//行号
                mPres.Item_Id = Convert.ToInt32(dt.Rows[i]["FeeID"]);
                mPres.Item_Name = dt.Rows[i]["FeeName"].ToString();
                mPres.Item_Type = type;//1西药 2中药 3项目材料
                mPres.StatItem_Code = "";
                mPres.Sell_Price = ConvertDataExtend.ToDecimal(dt.Rows[i]["ClinPrice"], 0);
                mPres.Buy_Price = ConvertDataExtend.ToDecimal(dt.Rows[i]["ClinPrice"], 0);
                mPres.Item_Price = ConvertDataExtend.ToDecimal(dt.Rows[i]["ClinPrice"], 0);
                mPres.Standard = dt.Rows[i]["Spec"].ToString();
                mPres.Usage_Amount = Convert.ToDecimal(dt.Rows[i]["Dosage"]);//剂量
                mPres.Usage_Unit = dt.Rows[i]["DosageUnit"].ToString();//剂量单位
                mPres.Usage_Rate = ConvertDataExtend.ToDecimal(dt.Rows[i]["DoseConvertNum"], 1);//剂量系数
                mPres.Dosage = Convert.ToInt32(dt.Rows[i]["Num"]);//付数
                mPres.Usage_Id = Convert.ToInt32(dt.Rows[i]["ChannelID"]);
                mPres.Frequency_Id = Convert.ToInt32(dt.Rows[i]["FrequencyID"]);
                mPres.Days = Convert.ToInt32(dt.Rows[i]["Days"]);

                mPres.Amount = Convert.ToDecimal(dt.Rows[i]["Amount"].ToString());//发药数量
                mPres.Unit = dt.Rows[i]["Unit"].ToString();//发药单位

                mPres.Item_Amount = Convert.ToInt32(Convert.ToDecimal(dt.Rows[i]["Amount"]));//开药数量
                mPres.Item_Unit = dt.Rows[i]["Unit"].ToString();//开药单位
                mPres.Item_Rate = 1;//系数

                mPres.Group_Id = Convert.ToInt32(dt.Rows[i]["GroupID"]);//分组组号
                mPres.SkinTest_Flag = ConvertDataExtend.ToInt32(dt.Rows[i]["IsAst"], 0);//皮试
                mPres.SelfDrug_Flag = 0;//自备
                mPres.Entrust = dt.Rows[i]["Memo"].ToString();//嘱托

                mPres.FootNote = "";
                mPres.Tc_Flag = 0;//套餐

                mPres.Usage_Name = dt.Rows[i]["ChannelName"].ToString();//用法名称
                mPres.Frequency_Name = dt.Rows[i]["FrequencyName"].ToString();//频次名称
                mPres.Frequency_Caption = dt.Rows[i]["ExecuteCode"].ToString();//频次名称

                int _execNum, _cycleDay;
                CardDataSourceFrequency.Calculate(dt.Rows[i]["ExecuteCode"].ToString(), out _execNum, out _cycleDay);
                mPres.Frequency_ExecNum = _execNum;//执行次数
                mPres.Frequency_CycleDay = _cycleDay;//执行周期


                mPres.CalculateAmount(null);

                list_Prescription.Add(mPres);
            }

            return list_Prescription;
            //throw new NotImplementedException();
        }



        public void AsSavePresTemplate(int level, string mName, int presType, int deptId, int doctorId, List<Prescription.Controls.Entity.Prescription> data)
        {
            string strsql = @"SELECT top 1 ID FROM Clinic_SetMealList WHERE ParentID=0 and LevelValue=" + level;
            int LevelId = ConvertDataExtend.ToInt32(oleDb.GetDataResult(strsql), 0);

            strsql = @"INSERT INTO Clinic_SetMealList(ParentID,LevelValue,NodeName,DeptCode,StaffID,PYCode,WBCode)
                                Values({0},{1},'{2}','{3}',{4},'','')";
            strsql = string.Format(strsql, LevelId, level, mName, deptId, doctorId);

            int listId = oleDb.InsertSql(strsql);

            List<string> listSql = new List<string>();
            for (int i = 0; i < data.Count; i++)
            {
                strsql = @"INSERT INTO Clinic_SetMealDetail(ListID,ListNo,GroupID,OrderNum,FeeID,FeeName,FeeClass,Dosage,DosageUnit,Amount,Unit,FrequencyID,ChannelID,Days,Memo)
			                        VALUES({0},1,{1},{2},{3},'{4}',{5},{6},'{7}',{8},'{9}',{10},{11},{12},'{13}')";
                strsql = string.Format(strsql, listId, data[i].Group_Id, i, data[i].Item_Id, data[i].Item_Name, data[i].Item_Type, data[i].Usage_Amount, data[i].Usage_Unit, data[i].Amount, data[i].Unit, data[i].Frequency_Id, data[i].Usage_Id, data[i].Days, data[i].Entrust);
                //MidDbHelper.ExecuteNonQuery(MidDbHelper.EmrHandle, strsql);
                listSql.Add(strsql);
            }
            oleDb.ExecuteSqls(listSql.ToArray());
        }

        #endregion
    }
}
