using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{

    public class EgsUsersViewModel
    {
        public int UserID { get; set; }
        public int WalletID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FullNames { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public int ApprovedBy { get; set; }
        public int RoleID { get; set; }
        public string Role { get; set; }
        public int UserTypeID { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string BVN { get; set; }
        
    }
}
