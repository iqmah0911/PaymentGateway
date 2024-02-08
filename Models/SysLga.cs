using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysLga

    {
        [Key]
        public int LgaID { get; set; }

        public string LgaName { get; set; }

        public DateTime DateCreated { get; set; }

        //[ForeignKey("UserID")]
        public int CreatedBy { get; set; }

        [InverseProperty("Lga")]
        public IEnumerable<SysUsers> Users { get; set; }
    }
}
