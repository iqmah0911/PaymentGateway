using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsTokenValidation
    {
        [Key]
        public int TokenID { get; set; }
        
        public string Provider { get; set; } 
        public string Token { get; set; }
        public DateTime ValidCreated { get; set; } 
        public DateTime ValidExpired { get; set; }

       
    }
}
