using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class EgsSubWallet
    {
        [Key]
        public int subWalletID { get; set; }

        public string BusinessName { get; set; }

        public string BusinessDescription { get; set; }

        public string AccountNumber { get; set; }

        public bool IsActive { get; set; }

        public int ApprovedBy { get; set; }

        public string RCNumber { get; set; }

        public string BankName { get; set; }

        public string BusinessAddress { get; set; }

        public DateTime DateApproved { get; set; }
        public int WalletID { get; set; }  // Corrected foreign key property name

        [ForeignKey("WalletID")]
        public EgsWallet Wallet { get; set; }

        public DateTime DateCreated { get; set; }

        public string MainWallet { get; set; }

        public double CurrentBalance { get; set; }

        public double OpeningBalance { get; set; }

        public double ClosingBalance { get; set; }
    }
}
