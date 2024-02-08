using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsSubMerchant
    {
        [Key]
        public int SubMerchantID { get; set; }

        public int SubMerchantType { get; set; }

        public string AccountNo { get; set; }
        public string Address { get; set; }
        public string KYC { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

        //[InverseProperty("Merchant")]
        //public IEnumerable<SysUsers> Users { get; set; }

        //[InverseProperty("Merchant")]
        //public IEnumerable<EgsProduct> Products { get; set; }
    }
}
