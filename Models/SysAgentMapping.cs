using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysAgentMapping
    {
        [Key]
        public int AgentMappingID { get; set; }
        public int AgentID { get; set; }

        [ForeignKey("AggregatorID")]
        public SysAggregator Aggregator { get; set; }
        public int AggregatorID { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }
}
