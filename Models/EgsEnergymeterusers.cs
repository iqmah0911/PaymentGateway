using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsEnergymeterusers
    {
        [Key]
        public int energymeterID { get; set; }
        public string meternumber { get; set; }
        public string disco { get; set; }
        public string type { get; set; }
        public string response { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
