using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysUserKycInfo
    {
        [Key]
        public int UserKYCID { get; set; }
        
        [ForeignKey("IdentificationTypeID")]
        public SysIdentificationType IdentificationType { get; set; }
        public string IdentificationValue { get; set; } // value of identification type picked ie.. passport no.
        public string BankAccount { get; set; }

        [ForeignKey("BankID")]
        public SysBank Bank { get; set; }
        public DateTime DateCreated { get; set; }


        // [ForeignKey("UserID")]
        public int Createdby { get; set; }

        [InverseProperty("KycInfo")]
        public IEnumerable<EgsDocumentValue> DocumentValues { get; set; }

    }
}
