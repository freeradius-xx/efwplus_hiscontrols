using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EMR.Controls.Entity;

namespace EMR.Controls.Action
{
    public interface IEmrTemplateTree
    {
        void loadDeptData(DataTable dt, string defcode);
        void loadTreeData(List<EmrCatalogue> clglist,List<EMR.Controls.Entity.EmrTemplateTree> templatelist);
    }
}
