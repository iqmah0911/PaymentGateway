using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsWallet
    {
        [Key]
        public int WalletID { get; set; }

        [ForeignKey("UserID")]
        public SysUsers User { get; set; }
        public string WalletAccountNumber { get; set; }
        public string BVN { get; set; }
        public double WalletBalance { get; set; }
        public string WalletCredentials { get; set; }
        public DateTime DateCreated { get; set; }
        //Added new columns  10-08-2021
        public double OpeningBalance { get; set; }
        public double ClosingBalance { get; set; }
        public DateTime OpeningBalanceDate { get; set; }
        public DateTime ClosingBalanceDate { get; set; }

        public bool IsActive { get; set; }

        //public int ParentWalletID { get; set; }

        [InverseProperty("Wallet")]
        public IEnumerable<EgsWalletTransaction> WalletTransactions { get; set; }

        [InverseProperty("Wallet")]
        public IEnumerable<EgsEgoleWalletTransactions> EgoleWalletTransactions { get; set; }

    }
}
