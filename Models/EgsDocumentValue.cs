using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class EgsDocumentValue
    {
        [Key]
        public int DocumentValueID { get; set; }

        [ForeignKey("DocumentID")]
        public SysDocument Document { get; set; }

        public string DocPath { get; set; }

        [ForeignKey("UserKYCID")]
        public SysUserKycInfo KycInfo { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
