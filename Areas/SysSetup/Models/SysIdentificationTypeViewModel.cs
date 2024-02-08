using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysIdentificationTypeViewModel : BasicObjectsViewModel
    {
        public int IdentificationTypeID { get; set; }
        public string IdentificationType { get; set; }

    }
    public class DisplayIdentificationTypeViewModel : SysIdentificationTypeViewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplayIdentificationTypeViewModel
    {
        public IEnumerable<DisplayIdentificationTypeViewModel> HoldAllIdentificationTypes { get; set; }
        public Pager Pager { get; set; }
    }
}
