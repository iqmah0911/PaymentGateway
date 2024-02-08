using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsTerminalTracking
    {
        [Key]
        public int TrackingID { get; set; }
        public string TerminalID { get; set; }

        public string Action { get; set; }

        public int AgentID { get; set; }

        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }

        public string Description { get; set; }
    }
}
