using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsUserToken
    {

        [Key]
        public int UserTokenID { get; set; }
        public int UserID { get; set; }
        public string Pin { get; set; }
        public int AuthFactor { get; set; }
        public string MainWallet { get; set; }

        //[ForeignKey("AuthFactorID")]
        //public EgsAuthFactor AuthFactors { get; set; }
    }
}
