using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysTransactionLimit
    {
        [Key]
        public int TransactionLimitID { get; set; }
        public string TransactionLimitName { get; set; }
        public double TransactionLimit { get; set; }
        public double TransactionBalance { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }


        [InverseProperty("TransactionLimit")]
        public IEnumerable<SysUsers> Users { get; set; }

    }
}
