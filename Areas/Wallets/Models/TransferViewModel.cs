using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Wallets.Models
{
    public class TransferViewModel
    {
        public int walletId { get; set; }
      
        public string toAccount { get; set; }
        public string toBank { get; set; }
        public double transAmount { get; set; }
        public string transferType { get; set; }

        public string transferpin { get; set; }
        public string Receiver { get; set; }

        public int BeneficiaryID { get; set; }
        [Required]
        public string Narration { get; set; }
      
    }


    public class NTransferViewModel
    {
        public int walletId { get; set; }

        public string toAccount { get; set; }
        public string toBank { get; set; }
        public double transAmount { get; set; }
        public string transferType { get; set; }
         
        public string Receiver { get; set; }
        public string ReceiverBank { get; set; }

        [Required]
        public string Narration { get; set; }
    }




    public class TransfersResponse
    {
        public string status { get; set; }
        public string message { get; set; } 
    }

    public class TransfersReceiptVM
    {
        public string Date { get; set; }
        public string OrderID { get; set; }

        public double Amount { get; set; }

        public string Fee { get; set; }
        public string Total { get; set; } 
        public string message { get; set; }

         
    }


    public class VerificationViewModel
    {
        public string toAccount { get; set; }
        public string toBank { get; set; }
    }

    public class WalletToWalletViewModel
    {
        public string WalletAccountNumber { get; set; }
        public double TransferAmount { get; set; }
        public string Receiver { get; set; } 
        public string transferpin { get; set; }
        public string Narration { get; set; }
    }



}
