using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Wallets.Models
{
    public class PinViewModel
    { 

            [Required]
            [StringLength(4)]
            public string Pin { get; set; }
            [Required]
            [StringLength(4)]
           [Compare("Pin")]
            public string ConfirmPin { get; set; }
       
    }
}
