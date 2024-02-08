using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class SysTarget
    {
        [Key]
        public int TargetID { get; set; }

        public string TargetName { get; set; }

        public DateTime TargetPeriodFrom { get; set; }

        public DateTime TargetPeriodTo { get; set; }

        public double TargetAmount { get; set; }
         
        [ForeignKey("TransactionMethodID")]
        public EgsTransactionMethod Services { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
