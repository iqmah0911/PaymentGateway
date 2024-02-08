using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class ApplicationRole : IdentityRole<string>
    {
        public bool IsActive { get; set; }
        public int CanCreate { get; set; }

        [ForeignKey("UserID")]
        public SysUsers CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int? RoleID { get; set; }
        public ApplicationRole() : base()
        {
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
            base.Name = roleName;
        }
    
    }
}
