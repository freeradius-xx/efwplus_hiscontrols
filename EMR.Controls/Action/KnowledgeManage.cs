using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EMR.Controls
{
    /// <summary>
    /// 知识库管理
    /// </summary>
    public class KnowledgeManage
    {
        public static XmlDocument Knowledge_xd;//知识库
        public static XmlDocument GeneralEmr_xd;//通用病历
        public static XmlDocument DataSource_xd;//数据源
        public static XmlDocument SpecialCharacter_xd;//特殊符号

        public static List<KEntry> KBEntries;//知识库对象集合
        public static List<DSValue> DSValues;//数据源值对象集合

        public static string emrdatapath;//数据路径

        /// <summary>
        /// 初始化知识库
        /// </summary>
        public static void InitKnowledg()
        {
            emrdatapath = System.Windows.Forms.Application.StartupPath;

            //先将XML字串读到xmlDocument中
            Knowledge_xd = new XmlDocument();
            Knowledge_xd.Load(emrdatapath + "\\emrdata\\Knowledge.xml");

            GeneralEmr_xd = new XmlDocument();
            GeneralEmr_xd.Load(emrdatapath + "\\emrdata\\GeneralEmr.xml");

            DataSource_xd = new XmlDocument();
            DataSource_xd.Load(emrdatapath + "\\emrdata\\DataSource.xml");

            SpecialCharacter_xd = new XmlDocument();
            SpecialCharacter_xd.Load(emrdatapath + "\\emrdata\\SpecialCharacter.xml");

            KBEntries = new List<KEntry>();
            XmlNodeList xnlist = KnowledgeManage.Knowledge_xd.DocumentElement.SelectNodes("KBEntries/Entry");
            getKEntry(xnlist);

            DSValues = new List<DSValue>();
            getDSValue();
        }

        private static void getKEntry(XmlNodeList xnlist)
        {
            foreach (XmlNode xn in xnlist)
            {
                if (xn.SelectSingleNode("ID") == null) continue;

                KEntry entry = new KEntry();
                entry.ID = xn.SelectSingleNode("ID").InnerText;
                entry.Text = xn.SelectSingleNode("Text").InnerText;
                entry.Value = xn.SelectSingleNode("Value").InnerText;
                entry.Style = xn.SelectSingleNode("Style").InnerText;
                
                if (xn.SelectSingleNode("SubEntries") == null)
                {
                    entry.ListItems = new List<KListItem>();
                    if (xn.SelectSingleNode("ListItems") != null)
                    {
                        foreach (XmlNode item in xn.SelectSingleNode("ListItems").ChildNodes)
                        {
                            KListItem li = new KListItem();
                            li.Text = item.SelectSingleNode("Text").InnerText;
                            li.Value = item.SelectSingleNode("Value").InnerText;
                            entry.ListItems.Add(li);
                        }
                    }
                }
                else
                {
                    getKEntry(xn.SelectSingleNode("SubEntries").ChildNodes);
                }

                KBEntries.Add(entry);
            }
        }

        private static void getDSValue()
        {
            XmlNodeList xnlist = KnowledgeManage.DataSource_xd.DocumentElement.SelectNodes("DataClass");
            foreach (XmlNode xn in xnlist)
            {
                 XmlNodeList txnlist = xn.SelectNodes("Value");
                 if (txnlist != null)
                 {
                     foreach (XmlNode txn in txnlist)
                     {
                         DSValue dsvalue = new DSValue();
                         dsvalue.id = txn.Attributes["id"].Value;
                         dsvalue.name = txn.Attributes["name"].Value;
                         dsvalue.value = txn.Attributes["value"].Value;
                         dsvalue.inputtype = Convert.ToInt32(txn.Attributes["inputtype"].Value);
                         dsvalue.sourcetype = Convert.ToInt32(txn.Attributes["sourcetype"].Value);
                         dsvalue.dofun = txn.Attributes["dofun"].Value;
                         dsvalue.dosql = txn.Attributes["dosql"].Value;

                         DSValues.Add(dsvalue);
                     }
                 }
            }
        }
    }

    public class KEntry
    {
        public string ID { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string Style { get; set; }
        public List<KListItem> ListItems { get; set; }
    }

    public class KListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
