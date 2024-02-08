using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsUssdTransactions
    {
        [Key]
        public int ussdTransactionID { get; set; }
        public string vendor { get; set; }
        public double amount { get; set; }
        public string reference { get; set; }
        public string providerReference { get; set; }
        public string agentID { get; set; }
        public string ussdString { get; set; }
        public string transactionID { get; set; }
        public string status { get; set; }
        public string statusDescription { get; set; }
        public string request { get; set; }
        public string response { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
