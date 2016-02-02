using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMR.Controls.Action;

namespace EMR.Controls.Action
{
    public interface IEmrDataSource
    {
        void InitData(EmrBindKeyData keydata);
        bool GetValue(string elId, out string text, out string value);
        Object GetDataSource(string dllname, string classname);
        Object GetDataSource(string sql);
    }
}
