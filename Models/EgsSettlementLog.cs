using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class EgsSettlementLog
    {
        [Key]
        public int SettlementLogID { get; set; }
        [ForeignKey("ProductID")]
        public EgsProduct Product { get; set; }
        [ForeignKey("ProductItemID")]
        public EgsProductItem ProductItem { get; set; }
        public double Totalcollection { get; set; }
        public int SettlementIntervalID { get; set; }
        public int SalesID { get; set; }

        [ForeignKey("MerchantID")]
        public EgsMerchant Merchant { get; set; }

        public double Amount { get; set; }
        public string MerchantAccountNo { get; set; }

        //[ForeignKey("AggregatorID")]
        //public SysAggregator Aggregator { get; set; }

        [ForeignKey("SettlementModeID")]
        public EgsSettlementMode SettlementMode { get; set; }

        [ForeignKey("SettlementTypeID")]
        public EgsSettlementType SettlementType { get; set; }
        //public int SettlementRateTypeID { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public bool IsPaid { get; set; }
        public DateTime DatePaid { get; set; }

        [ForeignKey("BankID")]
        public SysBank Bank { get; set; }




    }
}
