using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysBank
    {
        [Key]
        public int BankID { get; set; }

        public string BankName { get; set; }

        public string BankCode { get; set; }

        public DateTime DateCreated { get; set; }

       // [ForeignKey("UserID")]
        public int CreatedBy { get; set; }

        public string Provider { get; set; }

        [InverseProperty("Bank")]
        public IEnumerable<EgsAccountsInfo> AccountsInfos { get; set; }

        [InverseProperty("Bank")]
        public IEnumerable<EgsGLAccount> Banks { get; set; }

        [InverseProperty("Bank")]
        public IEnumerable<EgsMerchant> Merchants { get; set; }



    }
}
