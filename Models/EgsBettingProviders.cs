using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsBettingProviders
    {
        [Key]
        public int bettingID { get; set; }
        public string customerName { get; set; }
        public string vendor { get; set; }
        public string chargeType { get; set; }
        public double charge { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
