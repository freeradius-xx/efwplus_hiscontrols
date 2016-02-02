using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace EMR.Controls.Action
{
    public interface IEmrDbHelper
    {
        bool SaveEmrToDatabase(ref int emrDataId, byte[] bBytes);
        byte[] GetEmrFormDatabase(int emrDataId);
        bool DeleteEmrFormDatabase(int emrDataId);

        DataTable SearchStorageList(DateTime begindate, DateTime enddate);
    }
}
