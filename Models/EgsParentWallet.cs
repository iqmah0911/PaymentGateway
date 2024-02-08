using System.ComponentModel.DataAnnotations;
using System;

namespace PaymentGateway21052021.Models
{
    public class EgsParentWallet
    {
        [Key]
        public int ParentWalletID { get; set; }

        public string WalletCode { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
