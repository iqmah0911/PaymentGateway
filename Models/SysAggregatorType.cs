using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PaymentGateway21052021.Models
{
    public class SysAggregatorType
    {

        [Key]
        public int AggregatorTypeID { get; set; }

        public string TypeName { get; set; }
        public int AggregatorID { get; set; }

        [ForeignKey("AggregatorID")]
        public SysAggregator Aggregator { get; set; }

        public int TransactionMethodID { get; set; }

        [ForeignKey("TransactionMethodID")]
        public EgsTransactionMethod Services { get; set; }

        public string ServiceDetails { get; set; }

        public double Commission { get; set; }

        public double CapAtValue { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedBy { get; set; }
    }
}
