using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsAggregatorCommission
    {
        [Key]
        public int AggregatorCommissionID { get; set; }
        public int ProductID { get; set; }
        public int ProductItemID { get; set; }
        public int SplittingRate { get; set; }
        //public double SplittingRate { get; set; }
        public int SettlementTypeID { get; set; }
        public int SettlementIntervalID { get; set; }
        public int AggregatorID { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public string TransactionReferenceNo { get; set; }
        public bool IsPushed { get; set; }
        public double CommissionAmount { get; set; }
        public DateTime DatePushed { get; set; }

    }
}
