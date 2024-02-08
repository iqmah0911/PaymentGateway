using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysTransactionLimitViewModel : BasicObjectsViewModel
    {
        public int TransactionLimitID { get; set; }

        public string TransactionLimitName { get; set; }

        public double TransactionLimit { get; set; }

        public double TransactionBalance { get; set; }

        public class DisplayTransactionLimitViewModel : SysTransactionLimitViewModel
        {
            public Pager Pager { get; set; }
            public IEnumerable<object> Items { get; set; }

        }
        public class HoldDisplayTransactionLimitViewModel
        {
            public IEnumerable<DisplayTransactionLimitViewModel> HoldAllTransactionLimits { get; set; }
            public Pager Pager { get; set; }
        }
    }
}
