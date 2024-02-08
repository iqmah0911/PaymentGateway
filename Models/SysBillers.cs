using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysBillers
    {
        [Key]
        public int BillerID { get; set; }

        public string BillerName { get; set; }

        public string Services { get; set; }

        public double Commission { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedBy { get; set; }


    }
}
