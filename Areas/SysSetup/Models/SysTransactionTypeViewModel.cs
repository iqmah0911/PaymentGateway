using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysTransactionTypeViewModel
    {
        public int TransactionTypeID { get; set; }

        public string TransactionType { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
        //public IEnumerable<EgsWalletTransaction> WalletTransactions { get; set; }
    }
   
     public class DisplayTransactionTypeViewModel
    {
        public int TransactionTypeID { get; set; }
        public string TransactionType { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<object> Items { get; set; }

    }
    public class HoldDisplayTransactionTypeViewModel
    {
        public IEnumerable<DisplayTransactionTypeViewModel> HoldAllTransactionType { get; set; }
        public Pager Pager { get; set; }
    }

}
