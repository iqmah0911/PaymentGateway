using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysAggregatorViewModel
    {
        public int AggregatorID { get; set; }
        public string AggregatorName { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }
       
    public class DisplaySysAggregatorViewModel
    {
        public int AggregatorID { get; set; }
        public string AggregatorName { get; set; }
        public DateTime DateCreated { get; set; }
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }

    }
    public class HoldDisplaySysAggregatorViewModel
    {
        public IEnumerable<DisplaySysAggregatorViewModel> HoldAllAggregator { get; set; }
        public Pager Pager { get; set; }

    }
}
