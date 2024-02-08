using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsVendor
    {
        [Key]
        public int VendorID { get; set; } 
        public string VendorCode { get; set; } 
        public string VendorName { get; set; } 
        public string APIKey { get; set; }
        public string RCNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
