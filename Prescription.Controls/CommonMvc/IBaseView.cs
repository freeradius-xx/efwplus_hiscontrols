using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prescription.Controls.CommonMvc
{
    public interface IBaseView
    {
        //BaseController controller { get; set; }
        bool PromptController(string text);

    }
}
