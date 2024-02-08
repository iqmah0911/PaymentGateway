using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsSettlementSummary
    {
        [Key]
        public int SettlementSummaryID { get; set; }
        [ForeignKey("MerchantID")]
        public EgsMerchant Merchant { get; set; }
        public string Narration { get; set; }
        [ForeignKey("ProductID")]
        public EgsProduct Product { get; set; }

        //[ForeignKey("AggregatorID")]
        //public SysAggregator Aggregator { get; set; }

        public double AmountSettled { get; set; }
        public string AccountNo { get; set; }
        public string SettlementReference { get; set; }
        public double TotalCollection { get; set; }
        public bool IsPaid { get; set; }
        public DateTime DatePaid { get; set; }
        public DateTime DateCreated { get; set; }
        [ForeignKey("BankID")]
        public SysBank Bank { get; set; }

    }
}
