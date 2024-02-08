using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PaymentGateway21052021.Models
{
    public class SysAgentType
    {
        [Key]
        public int AgentTypeID { get; set; }

        public string TypeName { get; set; }
        //public int UserID { get; set; }  // Corrected foreign key property name

        [ForeignKey("UserID")]
        public SysUsers User { get; set; }

         
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
