using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsBettingUsers
    {
        [Key]
        public int bettingID { get; set; }
        public string customerID { get; set; }
        public string service { get; set; }
        public string response { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
