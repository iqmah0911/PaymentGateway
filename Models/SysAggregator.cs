using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class SysAggregator
    {
        [Key]
        public int AggregatorID { get; set; }
        public string AggregatorName { get; set; }
        public int AggregatorType { get; set; }
        public string AggregatorCode { get; set; }

        public string AccountNo { get; set; }
        public string Address { get; set; }
        public string KYC { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }

        public int AggregatorTypeID { get; set; }

        [ForeignKey("BankID")]
        public SysBank Bank { get; set; }

        [ForeignKey("UserID")]
        public SysUsers User { get; set; }

        //[InverseProperty("Aggregator")]
        //public IEnumerable<SysUsers> Users { get; set; }

       [InverseProperty("Aggregator")]
       public IEnumerable<SysAgentMapping> AgentMappings { get; set; }

        [InverseProperty("Aggregator")]
        public IEnumerable<EgsAggregatorRequest> AggregatorRequests { get; set; }

        //[InverseProperty("Aggregator")]
        //public IEnumerable<EgsProduct> Products { get; set; }

        //[InverseProperty("Aggregator")]
        //public IEnumerable<EgsSettlementLog> SettlementLogs { get; set; }

        //[InverseProperty("Aggregator")]
        //public IEnumerable<EgsSettlementSummary> SettlementSummarys { get; set; }
    }
}
