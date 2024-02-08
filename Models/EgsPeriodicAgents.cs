using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsPeriodicAgents
    {
        [Key]
        public int AgentID { get; set; }
        public string WalletAccountNumber { get; set; }
        public string AgentName { get; set; }

        public string Period { get; set; }

    }
}
