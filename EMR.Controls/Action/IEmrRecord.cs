using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EMR.Controls.Action
{
    public interface IEmrRecord
    {
        EmrControlType emrControlType { get; set; }
        EmrOperStyle emrOperStyle { get; set; }
        EmrDatastorageType emrDatastorageType { get; set; }
        void btnState();
        bool IsShowFileBtn { get; set; }

        //显示病历内容
        void LoadEmrText(byte[] wordByte);
        byte[] GetEmrText();
        void ClearEmrText();
        AxTx4oleLib.AxTXTextControl EmrTextControl { get; }
        //插入普通文本，比如：医学符号等
        void EmrInsertText(string text);
        //插入域，数据动态，比如：“病人姓名”，“住院号”等 /dtype 1知识库 2数据源
        void EmrInsertDomain(string text,string value,string dtype, string elId, string inputType);
        //插入域段落，比如：病历段落标题“既往史”、“体格检查”等
        void EmrInsertDomain(string text, string elId, bool isParagraph);

        //当前操作的文件，打开、保存
        FileInfo currFileInfo { get; set; }
        //当前工作区绑定的关键数据
        EmrBindKeyData CurrBindKeyData { get; set; }

        void btnSignState();
        void btnModifyState();
    }
}
