using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    
    public class EgsSettlementHistory
    {
        [Key]
        public int SettlementHistoryID { get; set; }

        public double SettlementAmount { get; set; }

        //[ForeignKey("ProductID")]
        //public EgsProduct ProductID { get; set; }

        //[ForeignKey("BankID")]
        //public SysBank BankID { get; set; }

        //[ForeignKey("UserID")] 
        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
