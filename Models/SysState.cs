using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysState
    {
        [Key]
        public int StateID { get; set; }

        public string StateName { get; set; }

        public string StateCode { get; set; }
        public DateTime DateCreated { get; set; }

       // [ForeignKey("UserID")]
        public int CreatedBy { get; set; }

        [InverseProperty("ResidentialState")]
        public IEnumerable<SysUsers> Users { get; set; }
    }
}
