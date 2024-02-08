
using PaymentGateway21052021.Helpers;  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class EgsSettlementTypeViewModel
    {
        public int SettlementTypeID { get; set; }

        public string SettlementType { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
      
    }

    public class DisplaySettlementTypeViewModel
    {
        public int SettlementTypeID { get; set; }
        public string SettlementType { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<object> Items { get; set; }

    }
    public class HoldDisplaySettlementTypeViewModel
    {
        public IEnumerable<DisplaySettlementTypeViewModel> HoldAllSettlementType { get; set; }
        public Pager Pager { get; set; }
    }

}
