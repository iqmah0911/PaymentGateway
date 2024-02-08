using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysUserTypeVIewModel : BasicObjectsViewModel
    {
        public int UserTypeID { get; set; }
        public string UserType { get; set; }

    }


    public class DisplayUserTypeViewModel : SysUserTypeVIewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplayUserTypeViewModel
    {
        public IEnumerable<DisplayUserTypeViewModel> HoldAllUserTypes { get; set; }
        public Pager Pager { get; set; }
    }
}
