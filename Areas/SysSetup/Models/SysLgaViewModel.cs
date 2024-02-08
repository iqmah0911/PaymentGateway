using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysLgaViewModel
    {
        public int LgaID { get; set; }

        public int StateID { get; set; }

        public string LgaName { get; set; }

        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }


    public class DisplayLgaViewModel
    {
        public int LgaID { get; set; }

        public int StateID { get; set; }

        public string LgaName { get; set; }

        public DateTime DateCreated { get; set; }
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }
    public class HoldDisplayLgaViewModel
    {
        public IEnumerable<DisplayLgaViewModel> HoldAllLga { get; set; }
        public Pager Pager { get; set; }
    }
}
