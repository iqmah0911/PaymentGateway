using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class EgsSubUser
    {
        [Key]
        public int TerminalKey { get; set; }

        public string SerialNumber { get; set; }

        public string TerminalType { get; set; }

        public string AgentfirstName { get; set; }

        public string AgentlastName { get; set; }

        public string Agentemail { get; set; }

        public string AgentphoneNumber { get; set; }

        public int SubUserID { get; set; }

        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public SysUsers Users { get; set; }

    
        public int AgentID { get; set; }

        public string TerminalID { get; set; }

        public DateTime DateCreated { get; set; }

        public int IsLinkedID { get; set; }

        public bool IsProcessed { get; set; }

        public DateTime DateUnprocessed { get; set; }
    }
}
