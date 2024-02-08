using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsIswpricebands
    {
        [Key]
        public int IswpriceID { get; set; }
        public string band { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public string charge { get; set; }
        public string stampduty { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
