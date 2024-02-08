using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysIdentificationType
    {
        [Key]
        public int IdentificationTypeID { get; set; }
        public string IdentificationTypeName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }

        [InverseProperty("IdentificationType")]
        public IEnumerable<SysUserKycInfo> UserKYCInfos { get; set; }
    }
}
