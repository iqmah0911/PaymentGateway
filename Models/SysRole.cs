using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysRole
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public DateTime DateCreated { get; set; }

       // [ForeignKey("UserID")]
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("Role")]
        public IEnumerable<SysUsers> Users { get; set; }

        //[InverseProperty("RoleRequest")]
        //public IEnumerable<EgsUpgradeAccount> UpgradeAccounts { get; set; }

        [InverseProperty("Role")]
        public IEnumerable<SysDocument> Documents { get; set; }

    }
}
