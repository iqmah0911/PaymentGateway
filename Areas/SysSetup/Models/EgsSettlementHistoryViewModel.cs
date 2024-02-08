
using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class EgsSettlementHistoryViewModel
    {
        public int SettlementHistoryID { get; set; }

        public double SettlementAmount { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
      

    }


    public class DisplaySettlementHistoryViewModel
    {
        public int SettlementHistoryID { get; set; }

        public double SettlementAmount { get; set; }

        public DateTime DateCreated { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplaySettlementHistoryViewModel
    {
        public IEnumerable<DisplaySettlementHistoryViewModel> HoldAllSettlementHistory { get; set; }
        public Pager Pager { get; set; }
    }
}
