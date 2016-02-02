using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prescription.Controls.CommonMvc
{
    public class BaseController<T>
    {
        private IBaseView _frmView;
        public T IfrmView
        {
            get { return (T)_frmView; }
        }

        public void bindView(IBaseView view)
        {
            _frmView = view;
        }

        public virtual void InitLoad()
        {

        }
    }
}
