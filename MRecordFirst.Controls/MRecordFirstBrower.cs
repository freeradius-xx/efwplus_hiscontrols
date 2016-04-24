using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Reflection;

namespace MRecordFirst.Controls
{
    public partial class MRecordFirstBrower : UserControl
    {
        string htmltemplate;
        string MRecordFirsthtml;
        TemplateHelper thelper;
        List<JsonData> jsondatapage1;
        List<JsonData> jsondatapage2;
        List<JsonData> jsondatapage3;

        public MRecordFirstBrower()
        {
            InitializeComponent();

            webBrowser.ScriptErrorsSuppressed = true;

            htmltemplate = Application.StartupPath + "\\htmltemplate";
            MRecordFirsthtml = htmltemplate + "\\MRecordFirst";
        }

        private bool _isContextMenuShow = true;
        [Description("是否显示浏览器内容菜单")]
        public bool IsContextMenuShow
        {
            get { return _isContextMenuShow; }
            set { _isContextMenuShow = value; }
        }

        public void LoadJsonData(List<JsonData> _jsondatapage1, List<JsonData> _jsondatapage2, List<JsonData> _jsondatapage3)
        {
            jsondatapage1 = _jsondatapage1;
            jsondatapage2 = _jsondatapage2;
            jsondatapage3 = _jsondatapage3;
        }

        /// <summary>
        /// 显示Web界面
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="jsondata"></param>
        private void ShowView(string filename, List<JsonData> jsondata)
        {

            string url = MRecordFirsthtml + "\\" + filename;

            thelper = new TemplateHelper(MRecordFirsthtml);
            if (jsondata != null)
            {
                foreach (JsonData key in jsondata)
                {
                    thelper.Put(key.DataId, key.JsonValue);
                }
            }
            thelper.Put("path", htmltemplate.Replace("\\", "/"));
            string html = thelper.BuildString(filename);
            webBrowser.IsWebBrowserContextMenuEnabled = _isContextMenuShow;
            webBrowser.DocumentText = html;
        }

        /// <summary>
        /// 显示Web界面
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="jsondata"></param>
        public void ShowView(List<JsonData> jsondata)
        {
            if (tabControl1.SelectedTabIndex == 0)
            {
                jsondatapage1 = jsondata;
                ShowView("Page1.html", jsondatapage1);
            }
            else if (tabControl1.SelectedTabIndex == 1)
            {
                jsondatapage2 = jsondata;
                ShowView("Page2.html", jsondatapage2);
            }
            else if (tabControl1.SelectedTabIndex == 2)
            {
                jsondatapage3 = jsondata;
                ShowView("Page3.html", jsondatapage3);
            }
        }

        /// <summary>
        /// 判断文书编辑状态（返回bool）
        /// </summary>
        /// <returns></returns>
        public bool GetEditState()
        {
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(webBrowser.Document.Body.InnerHtml);
            var editState = doc.Select("#editState input");
            if (editState.Count > 0 && editState[0].Attr("value") == "false")
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取界面的数据（返回Json数据）
        /// </summary>
        /// <returns></returns>
        public List<JsonData> GetViewData()
        {
            List<JsonData> list = new List<JsonData>();
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(webBrowser.Document.Body.InnerHtml);
            var ret = doc.Select("#datastorage input");
            for (int i = 0; i < ret.Count; i++)
            {
                if (ret[i].Attr("acceptflag") == "true")//只有属性acceptflag设置为true才保存到数据库
                {
                    JsonData data = new JsonData();
                    data.DataId = ret[i].Attr("id");
                    data.JsonValue = ret[i].Attr("value");
                    list.Add(data);
                }
            }
            return list;
        }
        /// <summary>
        /// 显示打印预览界面
        /// </summary>
        public void PrintPreview()
        {
            webBrowser.ShowPrintPreviewDialog();
        }

        /// <summary>
        /// 直接打印
        /// </summary>
        public void Print()
        {
            webBrowser.Print();
        }
        /// <summary>
        /// 页面设置
        /// </summary>
        public void PageSettings()
        {
            webBrowser.ShowPageSetupDialog();
        }

        public static List<T> ToList<T>(string json)
        {
            object data = JavaScriptConvert.DeserializeObject(json);
            if (data is JavaScriptArray)
            {
                PropertyInfo[] pros = typeof(T).GetProperties();
                List<T> list = new List<T>();
                for (int i = 0; i < (data as JavaScriptArray).Count; i++)
                {
                    T obj = (T)Activator.CreateInstance(typeof(T));
                    object _data = (data as JavaScriptArray)[i];
                    for (int k = 0; k < pros.Length; k++)
                    {
                        object val = convertVal(pros[k].PropertyType, (_data as JavaScriptObject)[pros[k].Name]);
                        pros[k].SetValue(obj, val, null);
                    }
                    list.Add(obj);
                }
                return list;
            }

            return null;
        }

        public static string ToJson(DataRow[] drs, string idName, string valueName)
        {
            if (drs.Length > 1)
            {
                char[] a = { '[', ']' };
                string JSONstring = "[";

                foreach (DataRow dr in drs)
                {
                    JSONstring += dr[valueName].ToString().TrimStart(a).TrimEnd(a) + ",";

                }

                JSONstring = JSONstring.TrimEnd(',');
                JSONstring += "]";

                return JSONstring;
            }
            else
            {
                string JSONstring = drs[0][valueName].ToString();

                return JSONstring;
            }
        }

        #region  值转换
        private static object convertVal(Type t, object data)
        {
            object val = null;
            if (t == typeof(Int32))
                val = Convert.ToInt32(data);
            else if (t == typeof(DateTime))
                val = Convert.ToDateTime(data);
            else if (t == typeof(Decimal))
                val = Convert.ToDecimal(data);
            else if (t == typeof(Boolean))
                val = Convert.ToBoolean(data);
            else if (t == typeof(String))
                val = Convert.ToString(data).Trim();
            else if (t == typeof(Guid))
                val = new Guid(data.ToString());
            else if (t == typeof(byte[]))
                if (data != null && data.ToString().Length > 0)
                {
                    val = Convert.FromBase64String(data.ToString());
                }
                else
                {
                    val = null;
                }
            else
                val = data;
            return val;
        }
        #endregion

        private void tabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            if (tabControl1.SelectedTabIndex == 0)
            {
                if (this.tabControlPanel1.Controls.Contains(this.webBrowser) == false)
                    this.tabControlPanel1.Controls.Add(this.webBrowser);
                if (this.tabControlPanel2.Controls.Contains(this.webBrowser))
                    this.tabControlPanel2.Controls.Remove(this.webBrowser);
                if (this.tabControlPanel3.Controls.Contains(this.webBrowser))
                    this.tabControlPanel3.Controls.Remove(this.webBrowser);

                ShowView("Page1.html", jsondatapage1);
            }
            else if (tabControl1.SelectedTabIndex == 1)
            {
                if (this.tabControlPanel1.Controls.Contains(this.webBrowser))
                    this.tabControlPanel1.Controls.Remove(this.webBrowser);
                if (this.tabControlPanel2.Controls.Contains(this.webBrowser) == false)
                    this.tabControlPanel2.Controls.Add(this.webBrowser);
                if (this.tabControlPanel3.Controls.Contains(this.webBrowser))
                    this.tabControlPanel3.Controls.Remove(this.webBrowser);

                ShowView("Page2.html", jsondatapage2);
            }
            else if (tabControl1.SelectedTabIndex == 2)
            {
                if (this.tabControlPanel1.Controls.Contains(this.webBrowser))
                    this.tabControlPanel1.Controls.Remove(this.webBrowser);
                if (this.tabControlPanel2.Controls.Contains(this.webBrowser))
                    this.tabControlPanel2.Controls.Remove(this.webBrowser);
                if (this.tabControlPanel3.Controls.Contains(this.webBrowser) == false)
                    this.tabControlPanel3.Controls.Add(this.webBrowser);

                ShowView("Page3.html", jsondatapage3);
            }
        }

        private void 预览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintPreview();
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void 打印设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageSettings();
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.OnMouseClick(e);
                //Control ctr = tabControl1.GetChildAtPoint(new Point(e.X, e.Y));
                //if (ctr is DevComponents.DotNetBar.TabItem)
                //    tabControl1.SelectedTab = (DevComponents.DotNetBar.TabItem)tabControl1.(new Point(e.X, e.Y));

                //实现右键选中选项卡
            }
        }
    }


    /// <summary>
    /// 数据交互对象
    /// </summary>
    public class JsonData
    {
        private string _dataId = "";
        public string DataId
        {
            get { return _dataId; }
            set { _dataId = value; }
        }

        private string _jsonValue = "";
        public string JsonValue
        {
            get { return _jsonValue; }
            set { _jsonValue = value; }
        }
    }

}
