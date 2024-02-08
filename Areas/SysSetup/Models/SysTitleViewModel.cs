using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysTitleViewModel : BasicObjectsViewModel
    {
        public int TitleID { get; set; }
        public string TitleName { get; set; }
    }

    public class DisplayTitleViewModel : SysTitleViewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplayTitleViewModel
    {
        public IEnumerable<DisplayTitleViewModel> HoldAllTitles { get; set; }
        public Pager Pager { get; set; }
    }

    public class UpdateTitleViewModel
    {
        public int TitleID { get; set; }
        public string TitleName { get; set; }
        //public DateTime DateCreated { get; set; }
    }
}
