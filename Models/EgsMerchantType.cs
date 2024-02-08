using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsMerchantType
    {
        [Key]
        public int MerchantTypeID { get; set; }

        public string MerchantType { get; set; }

        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }
}
