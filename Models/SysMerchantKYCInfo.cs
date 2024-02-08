using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysMerchantKycInfo
    {
        [Key]
        public int MerchantKYCID { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationValue { get; set; } // value of identification type picked ie.. passport no.
        public string BankAccount { get; set; }

        [ForeignKey("BankID")]
        public SysBank Bank { get; set; }
        public DateTime DateCreated { get; set; }

        // [ForeignKey("MerchantID")]
        public int Createdby { get; set; }

        [InverseProperty("KycInfo")]
        public IEnumerable<EgsDocumentValue> DocumentValues { get; set; }

    }

}
