﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prescription.Controls.Entity
{
    public class CardDataSourceExecDept
    {
        public int DeptId { get; set; }
        //执行科室名称
        public string DeptName { get; set; }

        public string Pym { get; set; }
        public string Wbm { get; set; }
        public string Szm { get; set; }
    }
}
