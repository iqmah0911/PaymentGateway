using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsBeneficiary
    {
        [Key]
        public int BeneficaryID { get; set; }

        public int userId { get; set; }
        public string BeneficiaryBankAccount { get; set; }

        public string BeneficiaryName { get; set; }
          
        [ForeignKey("BankID")]
        public SysBank Bank { get; set; }

        public DateTime Datecreated { get; set; }

        //New added fields
        public string BankCode { get; set; }

        public string BankName { get; set; }
        public string BankClient { get; set; }

        public int WalletId { get; set; }




    }
}
