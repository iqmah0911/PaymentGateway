using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace PaymentGateway21052021.Models
{
    public class EgsProductItemRate
    {
        [Key]
        public int ProductItemRateID { get; set; }

        [ForeignKey("ProductItemID")]
        public EgsProductItem ProductItem { get; set; }

        public double AmountRate{ get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public SysUsers CreatedBy { get; set; }

    }
}
