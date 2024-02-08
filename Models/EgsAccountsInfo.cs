using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsAccountsInfo
    {
        [Key]
        public int AccountsInfoID { get; set; }

        [ForeignKey("UserID")]
        public SysUsers User { get; set; }

        [ForeignKey("BankID")]
        public SysBank Bank { get; set; }

        public string AccountNumber { get; set; }

        public DateTime DateCreated { get; set; }

        //[ForeignKey("UserID")]
        public int CreatedBy { get; set; }

    }
}
