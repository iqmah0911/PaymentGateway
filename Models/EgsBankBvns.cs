using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsBankBvns
    {
        [Key]
        public int bankbvnsID { get; set; }
        public string bvn { get; set; }
        public string bvndetails { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
