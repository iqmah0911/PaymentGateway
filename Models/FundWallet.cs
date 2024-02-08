using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class FundWallet
    {
        [Key]
        public int FundWalletID { get; set; }

        [ForeignKey("WalletID")]
        public EgsWallet Wallet { get; set; }

        [ForeignKey("UserID")]
        public SysUsers Users { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }



    }
}
