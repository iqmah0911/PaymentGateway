using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsAggregatorRequest
    {
        [Key]
        public int RequestID { get; set; }

        [ForeignKey("UserID")]
        public SysUsers Agent { get; set; }

        [ForeignKey("AggregatorID")]
        public SysAggregator Aggregator { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsProcessed { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime DateApproved { get; set; }
        public int RejectedBy { get; set; }
        public DateTime DateRejected { get; set; }
        public string Comment { get; set; }
        //Added 27April2022
        public int ApprovedBy2 { get; set; }
        public DateTime DateApprovedBy2 { get; set; }
        public int RejectedBy2 { get; set; }
        public DateTime DateRejectedBy2 { get; set; }
    }
}
