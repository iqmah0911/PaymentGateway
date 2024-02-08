using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysUserContactInfoViewModel
    {
    }

    public class CreateContactInfoViewModel
    {
        public int UserContactInfoID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateContactInfoViewModel
    {
        public int UserContactInfoID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class DisplayContactInfoViewModel
    {
        public int UserContactInfoID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplayContactInfoViewModel
    {
        public IEnumerable<DisplayContactInfoViewModel> HoldAllContactInfo { get; set; }
        public Pager Pager { get; set; }
    }
}
