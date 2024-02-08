using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysMerchantContactInfo
    {
        [Key]
        public int MerchantContactInfoID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string MerchantAddress { get; set; }

        // [ForeignKey("StateID")]
        public int StateID { get; set; }

        public string PostalCode { get; set; }

        public string LgaID { get; set; }

        // [ForeignKey("MerchantID")]
        public int MerchantID { get; set; }
    }

}
