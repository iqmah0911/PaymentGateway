using System.ComponentModel.DataAnnotations;
using System;

namespace PaymentGateway21052021.Models
{
    public class EgsAgentManagers
    {
        [Key]
        public int AgentManagerID { get; set; }

        public int ManagerID { get; set; }

        public int AgentID { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
