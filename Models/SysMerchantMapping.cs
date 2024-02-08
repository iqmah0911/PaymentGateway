using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace PaymentGateway21052021.Models
{
   
    public class SysMerchantMapping
    {
        [Key]
        public int MerchantMappingID { get; set; }

        public int SubMerchantID { get; set; }
        //public int MerchantID { get; set; }

        [ForeignKey("MerchantID")]
        public EgsMerchant Merchant { get; set; }

        public  DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }
}
