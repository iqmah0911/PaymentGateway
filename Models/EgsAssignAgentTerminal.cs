using System.ComponentModel.DataAnnotations;
using System;

namespace PaymentGateway21052021.Models
{
    public class EgsAssignAgentTerminal
    {
        [Key]
        public int AssignTerminalID { get; set; }

        public int TerminalKey { get; set; }

        public int AgentID { get; set; }

        public string Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsActive { get; set; }

        public string MainWallet { get; set; }

        public int SubWalletID { get; set; }
    }
}
