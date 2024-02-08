using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsPeriodicTransactionReport
    {
        [Key]
        public int TransID { get; set; }
        public string AgentName { get; set; }
        public string WalletAccountNumber { get; set; }
        public int Itemcount { get; set; }

        public double Amount { get; set; }

        public DateTime TransactionDate { get; set; }
         
    }
}
