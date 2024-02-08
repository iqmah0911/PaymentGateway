using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysUserContactInfo
    {
        [Key]
        public int UserContactInfoID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string UserAddress { get; set; }

       // [ForeignKey("StateID")]
        public int StateID { get; set; }

        public string PostalCode { get; set; }

        public string LgaID { get; set; }

       // [ForeignKey("UserID")]
        public int UserID { get; set; }
    }
}
