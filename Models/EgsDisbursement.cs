using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsDisbursement
    {
        [Key]
        public int DisbursementID { get; set; }
        public string Recipient { get; set; }
        public double Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public string AccountNumber { get; set; }
        public string status { get; set; }
        public string PaymentMethod { get; set; }
        public string Approver { get; set; }

    }
}
