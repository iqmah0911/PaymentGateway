using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsCardHolder
    { 
            [Key]
            public int CardholderID { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }

            public string PAN { get; set; }

            public string Pin { get; set; }

            public string CVV { get; set; }

            public string EXP { get; set; }

            [ForeignKey("WalletID")]
            public EgsWallet Wallet { get; set; }

            public DateTime DateCreated { get; set; }


       
    }
}
