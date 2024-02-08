using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Models
{
    public class AggregatorRViewModel : BasicObjectsViewModel
    {
        public int WalletTransactionID { get; set; }

        public double DiscountedAmount { get; set; }
        public string TransactionReferenceNumber { get; set; }

        public DateTime SettlementDate { get; set; }

        public int ProductID { get; set; } 
        public int ProductItemID { get; set; }

        public string ProductName { get; set; }
        public string ProductItemName { get; set; }
          
        public double Commission { get; set; }

        public string Period { get; set; }


    }

     
    public class HoldSalesWViewModel
    {
        public IEnumerable<AggregatorRViewModel> HoldWAllSales { get; set; }
        public Pager Pager { get; set; }
    }
}
