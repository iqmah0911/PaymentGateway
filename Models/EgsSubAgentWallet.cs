using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class EgsSubAgentWallet
    {
        [Key]
        public int SubAgentWalletId { get; set; }
        [ForeignKey("UserID")]
        public SysUsers User { get; set; }
        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("WalletID")]
        public EgsWallet Wallet { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int OpeningBalance { get; set; }
        public int ClosingBalance { get; set; }
    }
}
