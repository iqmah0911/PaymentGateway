using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Models
{
    public class EgsTransferViewModel
    { 
        public int TransactionID { get; set; } 
        public int UserID { get; set; } 
        public int BankID { get; set; } 
        public double TransferAmount { get; set; } 
        public int TransactionTypeID { get; set; } 
        public int TransactionMethodID { get; set; } 
        public int WalletID { get; set; } 
        public DateTime DateCreated { get; set; }

        public string Status { get; set; }


    }
}
