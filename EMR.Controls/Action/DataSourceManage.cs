using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMR.Controls.Action;

namespace EMR.Controls
{
    public class DataSourceManage
    {
        private static EmrBindKeyData _keydata;
        public static IEmrDataSource _emrDataSource;

        public static void InitData(IEmrDataSource emrDataSource)
        {
            _emrDataSource = emrDataSource;
        }

        public static void InitData(EmrBindKeyData keydata, IEmrDataSource emrDataSource)
        {
            _keydata=keydata;
            _emrDataSource = emrDataSource;
            emrDataSource.InitData(keydata);
        }

        public static void SetValue(string elId, Action<string, string> _valueChanged)
        {
            if (_emrDataSource == null) return;
            if (_valueChanged != null)
            {
                string text;
                string value;
                if (_emrDataSource.GetValue(elId, out text, out value))
                    _valueChanged(text, value);
            }
        }

        public static void SetAllValue(Func<string> findelId, Action<string, string> _valueChanged)
        {
            if (_emrDataSource == null) return;
            string elId = null;
            while ((elId = findelId()) != null)
            {
                if (_valueChanged != null || elId != "")
                {
                    string text;
                    string value;
                    if (_emrDataSource.GetValue(elId, out text, out value))
                        _valueChanged(text, value);
                }
            }
        }

        public static Object GetFieldDataSource(string elId)
        {
            if (_emrDataSource == null) return null;
            DSValue dsvalue = KnowledgeManage.DSValues.Find(x => x.id == elId);
            if (dsvalue != null)
            {
                if (dsvalue.inputtype == (int)InputType.List || dsvalue.inputtype == (int)InputType.MultiList)
                {
                    if (dsvalue.sourcetype == 0)
                    {
                        string[] ss = dsvalue.dofun.Split('@');
                        if (ss.Length > 2)
                            return _emrDataSource.GetDataSource(ss[0], ss[1]);
                    }
                    else if (dsvalue.sourcetype == 1)
                    {
                        if (dsvalue.dosql != "")
                            return _emrDataSource.GetDataSource(dsvalue.dosql);
                    }
                }
            }
            return null;
        }
    }

    public class DSValue
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        /// <summary>
        /// InputType与 “域类型”对应
        /// </summary>
        public int inputtype { get; set; }
        /// <summary>
        /// 数据源来源类型 -1：无 ，0：dofun方式， 1：dosql方式
        /// </summary>
        public int sourcetype { get; set; }
        /// <summary>
        /// 反射C#方法获取数据源，dllname@classname 
        /// </summary>
        public string dofun { get; set; }
        /// <summary>
        /// 执行sql语句获取数据源，
        /// </summary>
        public string dosql { get; set; }
    }
}
