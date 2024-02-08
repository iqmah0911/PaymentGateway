using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway21052021.Models
{
    public class EgsPinpads
    {
        [Key]
        public int pinpadsID { get; set; }
        public string terminalID { get; set; }
        public string reference { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
        public string pan { get; set; }
        public string cardScheme { get; set; }
        public string customerName { get; set; }
        public string statusCode { get; set; }
        public string statusDescription { get; set; }
        public string rrn { get; set; }
        public DateTime paymentDate { get; set; }
        public string stan { get; set; }
        public string cardExpiry { get; set; }
        public string cardhash { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }


    }
}
