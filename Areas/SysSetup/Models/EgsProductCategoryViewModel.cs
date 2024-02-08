using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class EgsProductCategoryViewModel : BasicObjectsViewModel
    {
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductCategoryCode { get; set; }
        public string FormCategoryType { get; set; }
        public string Image { get; set; }
        public string URL { get; set; }
    }

    public class DisplayProductCategoryViewModel : EgsProductCategoryViewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplayProductCategoryViewModel
    {
        public IEnumerable<DisplayProductCategoryViewModel> HoldAllProductCategories { get; set; }
        public List<DisplayProductCategoryViewModel> HoldAllProductCategoryList { get; set; }
        public Pager Pager { get; set; }
    }

}
