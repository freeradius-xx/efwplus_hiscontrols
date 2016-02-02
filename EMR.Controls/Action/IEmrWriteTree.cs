using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMR.Controls.Action
{
    public interface IEmrWriteTree
    {
        void loadTreeData(List<EMR.Controls.Entity.EmrCatalogue> clglist, List<EMR.Controls.Entity.EmrWriteRecord> recordlist);
    }
}
