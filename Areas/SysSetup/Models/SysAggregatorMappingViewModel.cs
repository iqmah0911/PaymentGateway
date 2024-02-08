using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class AggregatorMappingModel : BasicObjectsViewModel
    {
        //
        public int AggregatorMappingID { get; set; }
        public int AggregatorID { get; set; }
        public int AgentID { get; set; }
        //public string[] SubMerchantListID { get; set; }

    }

    public class AgentsView
    {
        public int AgentID { get; set; }
        public string AgentName { get; set; }
        public bool IsChecked { get; set; }

    }

    public class AggregatorsView
    {
        public int AggregatorID { get; set; }
        public string AggregatorName { get; set; }
        public bool IsChecked { get; set; }

    }

    public class AgentList
    {
        public List<AgentsView> Agent { get; set; }
    }

    public class AggregatorList
    {
        public List<AggregatorsView> Aggregator { get; set; }
    }

    public class SysAggregatorMappingViewModel : BasicObjectsViewModel
    {
        //
        public int AggregatorMappingID { get; set; }
        public int AggregatorID { get; set; }

        //[Display(Name = "Aggregator")]
        public string AggregatorName { get; set; }

        public int AgentID { get; set; }

        public int[] AgentListID { get; set; }

        //[Display(Name = "Agent Name")]SubMerchantListID
        public string AgentName { get; set; }

    }

    public class DisplayAggregators : SysAggregatorMappingViewModel
    {
        public Pager Pager { get; set; }
        public List<object> Items { get; set; }
    }

    public class HoldDisplayAggregators
    {
        public IEnumerable<DisplayAggregators> HoldAllAggregators { get; set; }
        public Pager Pager { get; set; }
    }

    public class DisplayAggregatorMappingViewModel : SysAggregatorMappingViewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplayAggregatorMappingViewModel
    {
        public IEnumerable<DisplayAggregatorMappingViewModel> HoldAllAggregatorMapping { get; set; }
        public List<DisplayAggregatorMappingViewModel> HoldAllAggregatorMappingList { get; set; }
        public Pager Pager { get; set; }
    }


}
