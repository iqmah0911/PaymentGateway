using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysTitle
    {
        [Key]
        public int TitleID { get; set; }
        public string  TitleName { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

        [InverseProperty("Title")]
        public IEnumerable<SysUsers> Users { get; set; }
    }
}
