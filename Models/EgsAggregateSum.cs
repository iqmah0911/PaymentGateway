using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsAggregateSum
    {
        [Key]
        public int AggregateID { get; set; }

        public double AggregatedAmount { get; set; }

        public DateTime DateModified { get; set; }



    }
}
