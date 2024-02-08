using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class EgsSettlementType
    {
        [Key]
        public int SettlementTypeID { get; set; }
        public string SettlementType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime  DateCreated { get; set; }

        [InverseProperty("SettlementType")]
        public IEnumerable<EgsSettlementLog> SettlementLogs { get; set; }


    }
}
