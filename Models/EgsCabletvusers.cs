using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsCabletvusers
    {
        [Key]
        public int CableID { get; set; }
        public string smartcard { get; set; }
        public string service { get; set; }
        public string response { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
