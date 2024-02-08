using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Models
{
    public class WalletHViewModel
    {
        public int TransactionID { get; set; }
        public int InvoiceID { get; set; }
        public string ReferenceNo { get; set; }
        public string ServiceNumber { get; set; }
        public double Amount { get; set; }
        public bool PaymentStatus { get; set; }
        public string TransactionMethod { get; set; }
        public string TransactionType { get; set; }
        public string PaymentReference { get; set; }
        public string ProductName { get; set; }
        public string ProductItemName { get; set; }
        public DateTime PaymentDate { get; set; }
        public string AlternateReferenceNo { get; set; }
        public string WalletAccountNumber { get; set; }
        public string WalletHolder { get; set; }
        public string WalletEmail { get; set; }
        public string WalletPhone { get; set; }
        public string TransactionNaration { get; set; }
        public string TransactionDate { get; set; }
        public int RefCount { get; set; }
        public string Sender { get; set; } 
        //new columns
        public string ReceiverAccount { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverBank { get; set; } 
    }

    public class HoldWalletHViewModel
    {
        public IEnumerable<WalletHViewModel> HoldAllWalletHistorys { get; set; }
        public Pager Pager { get; set; }
    }
}
