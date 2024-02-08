using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class EgsSettlementInterval
    {
        [Key]
        public int SettlementIntervalID { get; set; }
        public string SettlementIntervalName { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

    }
}
