using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsAuthFactor
    {
        [Key]
        public int AuthFactorID { get; set; } 
        public string AuthType { get; set; }

        public string AuthValue { get; set; }

        //[InverseProperty("UserToken")]
        //public IEnumerable<EgsUserToken> UserTokens { get; set; }



    }
}
