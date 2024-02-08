using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{

    public class EgsSettlementIntervalViewModel : BasicObjectsViewModel
    {
        public int SettlementIntervalID { get; set; }
        public string SettlementIntervalName { get; set; }
    }

    public class DisplaySettlementIntervalViewModel : EgsSettlementIntervalViewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplaySettlementIntervalViewModel
    {
        public IEnumerable<DisplaySettlementIntervalViewModel> HoldAllSettlementInterval { get; set; }
        public List<DisplaySettlementIntervalViewModel> HoldAllSettlementIntervalList { get; set; }
        public Pager Pager { get; set; }
    }

}
