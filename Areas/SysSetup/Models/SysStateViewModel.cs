using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysStateViewModel
    {
        
    }

    public class CreateStateViewModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateStateViewModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class DisplayStateViewModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
        public DateTime DateCreated { get; set; }
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplayStateViewModel
    {
        public IEnumerable<DisplayStateViewModel> HoldAllStates { get; set; }
        public Pager Pager { get; set; }
    }
}
