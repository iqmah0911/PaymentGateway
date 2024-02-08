using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsSettlementMode
    {
        [Key]
        public int SettlementModeID { get; set; }
        public string SettlementModeName { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

        [InverseProperty("SettlementMode")]
        public IEnumerable<EgsSettlementLog> SettlementLogs { get; set; }

    }
}
