using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class SysDocument
    {
        [Key]  
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }

        //public EgsDocumentValue DocumentValue { get; set; }

        //[ForeignKey("UserKYCID")]
        //public SysUserKycInfo UserKycInfo { get; set; }
        public DateTime DateCreated { get; set; }

        //[ForeignKey("UserID")]
        public int CreatedBy { get; set; }

        // coming back to it by emma and dunni,resolved by the two. 20-05-2021 4:52PM
        [ForeignKey("RoleID")]
        public SysRole Role { get; set; }

        [InverseProperty("Document")]
        public IEnumerable<EgsDocumentValue> DocumentValues { get; set; }

    }
}
