using System.ComponentModel.DataAnnotations;
using System;

namespace PaymentGateway21052021.Models
{
    public class EgsParentTerminal
    {
        [Key]
        public int ParentTerminalKey { get; set; }

        public string ParentTerminalID { get; set; }

        public string Provider { get; set; }

        public DateTime DateCreated { get; set; }

        public bool Status { get; set; }
    }
}
