using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsAgentCommission
    {
        [Key]
        public int AgentCommissionID { get; set; }
        public int ProductID { get; set; }
        public int ProductItemID { get; set; }
        public int SplittingRate { get; set; }
        public int SettlementTypeID { get; set; }
        public int SettlementIntervalID { get; set; }
        public int AgentID { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }


    }
}
