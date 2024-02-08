using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Models
{
    public class AggregatorAgentListVM
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

    }


    public class AgguserListVM
    {
        public int AggregatorID { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

    }


    public class AggregatorAgentsbalanceVM
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
        public double Balance { get; set; }
        public string Company { get; set; }
        public string Agent { get; set; }
        public string LastName { get; set; }

    }


    public class HoldAggregatorAgentsbalanceVM
    {
        public List<AggregatorAgentsbalanceVM> HoldAllBalance { get; set; }
        //public Pager Pager { get; set; }
    }


}
