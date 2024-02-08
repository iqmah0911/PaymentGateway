using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsAuditTrail
    {
        [Key]
        public int AID { get; set; }
        public string DbAction { get; set; }
        public string Description { get; set; }
        public string Page { get; set; }

        public DateTime DateCreated { get; set; }
        public string IPAddress { get; set; }
        public int CreatedBy { get; set; }
        public string Role { get; set; } 
        public string Menu { get; set; }
        public string DeviceName { get; set; }
    }
}
