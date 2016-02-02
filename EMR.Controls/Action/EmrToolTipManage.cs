using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopupControl;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace EMR.Controls
{
    public class EmrToolTipManage
    {
        private static Action<string,string> valueChange = null;
        //单选控件
        private static Itooltip listToolTip;
        private static Popup listPopup;
        //多选控件
        private static Itooltip mlistToolTip;
        private static Popup mlistPopup;
        //日期控件
        private static Itooltip dateToolTip;
        private static Popup datePopup;
        //时间控件
        private static Itooltip timeToolTip;
        private static Popup timePopup;

        //private static IEmrDataSource emrDataSource = DataSourceManage._emrDataSource;
        /// <summary>
        /// 初始化提示控件
        /// </summary>
        public static void InitEmrToolTip()
        {
            //emrDataSource = _emrDataSource;

            listPopup = new Popup((Control)(listToolTip = new ListToolTip()));
            listToolTip.ValueChanged += (_sender, _e) =>
            {
                if (valueChange != null)
                {
                    valueChange(listToolTip.EText,listToolTip.EValue);
                    listPopup.Hide();
                }
            };

            mlistPopup = new Popup((Control)(mlistToolTip = new MultiListToolTip()));
            mlistToolTip.ValueChanged += (_sender, e) =>
            {
                if (valueChange != null)
                {
                    valueChange(mlistToolTip.EText,mlistToolTip.EValue);
                }
            };

            datePopup = new Popup((Control)(dateToolTip = new DateToolTip()));
            dateToolTip.ValueChanged += (_sender, _e) =>
            {
                if (valueChange != null)
                {
                    valueChange(dateToolTip.EText,dateToolTip.EValue);
                    datePopup.Hide();
                }
            };

            timePopup = new Popup((Control)(timeToolTip = new TimeToolTip()));
            timeToolTip.ValueChanged += (_sender, _e) =>
            {
                if (valueChange != null)
                {
                    valueChange(timeToolTip.EText,timeToolTip.EValue);
                    timePopup.Hide();
                }
            };
        }

        /// <summary>
        /// 双击域显示对应弹出控件
        /// </summary>
        /// <param name="mousePoint">弹出位置</param>
        /// <param name="dtype">0无 1知识库 2数据源</param>
        /// <param name="inputType">控件类型</param>
        /// <param name="elId">域ID</param>
        /// <param name="fieldvalue">域值</param>
        /// <param name="_valueChange">值改变会写病历</param>
        public static void ShowEmrToolTip(Point mousePoint, string dtype,string inputType, string elId, string fieldvalue, Action<string, string> _valueChange)
        {
            valueChange = _valueChange;
            switch (Convert.ToInt32(inputType))
            {
                case (int)InputType.List:
                    if (dtype == "1")//知识库
                    {
                        KEntry ke = KnowledgeManage.KBEntries.Find(x => x.Value == elId);
                        if (ke != null)
                        {
                            List<ListItem> olist = new List<ListItem>();
                            foreach (KListItem item in ke.ListItems)
                            {
                                olist.Add(new ListItem(item.Text,item.Value));
                            }
                            listToolTip.LoadData(olist, fieldvalue);
                            listPopup.Show(mousePoint);
                        }
                    }
                    else if (dtype == "2")//数据源
                    {
                        object obj= DataSourceManage.GetFieldDataSource(elId);
                        if (obj != null)
                        {
                            DataTable data = (DataTable)obj;
                            List<ListItem> olist = new List<ListItem>();
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                olist.Add(new ListItem(data.Rows[i][0].ToString(), data.Rows[i][1].ToString()));
                            }
                            listToolTip.LoadData(olist, fieldvalue);
                            listPopup.Show(mousePoint);
                        }
                    }
                    break;
                case (int)InputType.MultiList:
                    if (dtype == "1")//知识库
                    {
                        KEntry ke = KnowledgeManage.KBEntries.Find(x => x.Value == elId);
                        if (ke != null)
                        {
                            List<ListItem> olist = new List<ListItem>();
                            foreach (KListItem item in ke.ListItems)
                            {
                                olist.Add(new ListItem(item.Text, item.Value));
                            }
                            mlistToolTip.LoadData(olist, fieldvalue);
                            mlistPopup.Show(mousePoint);
                        }
                    }
                    else if (dtype == "2")//数据源
                    {
                         object obj= DataSourceManage.GetFieldDataSource(elId);
                         if (obj != null)
                         {
                             DataTable data = (DataTable)obj;
                             List<ListItem> olist = new List<ListItem>();
                             for (int i = 0; i < data.Rows.Count; i++)
                             {
                                 olist.Add(new ListItem(data.Rows[i][0].ToString(), data.Rows[i][1].ToString()));
                             }
                             mlistToolTip.LoadData(olist, fieldvalue);
                             mlistPopup.Show(mousePoint);
                         }
                    }
                    break;

                case (int)InputType.Date:
                    dateToolTip.LoadData(null, fieldvalue);
                    datePopup.Show(mousePoint);
                    break;

                case (int)InputType.Time:
                    timeToolTip.LoadData(null, fieldvalue);
                    timePopup.Show(mousePoint);
                    break;
            }
        }
    }

    public interface Itooltip
    {
        string EText { get; set; }
        string EValue { get; set; }
        event EventHandler ValueChanged;
        void LoadData(List<ListItem> datasource, string value);
    }

    /// <summary>
    /// 域类型
    /// </summary>
    public enum InputType : int
    {
        None = 0,
        List = 1,//单选
        MultiList = 2,//多选
        Text = 3,//获取值
        Date = 4,
        Time = 5,
        CheckBox=6//复选框
    }

    public class ListItem
    {
        private string _Text;
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        private object _Value;
        public object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        public ListItem(string text, object value)
        {
            _Text = text;
            _Value = value;
        }
    }
}
