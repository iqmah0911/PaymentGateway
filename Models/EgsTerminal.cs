using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsTerminal
    {

        [Key]
        public int TerminalKey { get; set; }
        public string TerminalID { get; set; }
        public string SerialNumber { get; set; }
        public string TerminalType { get; set; }
        public int? agentid { get; set; }
        public string Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsActive { get; set; }
        public int ModifiedBy { get; set; }

        public DateTime  ModifiedDate { get; set; }

        public string BatchNumber { get; set; } //BatchNumber

        [ForeignKey("ParentTerminalKey")]
        public EgsParentTerminal ParentTerminal { get; set; }

        public string AgentTerminalID { get; set; }

        public int AggregatorID { get; set; }


    }
}
