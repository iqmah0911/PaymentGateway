using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class EgsProductItemViewModel : BasicObjectsViewModel
    {
        public int ProductItemID { get; set; }

        [Display(Name = "Product Item")]
        public string ProductItemName { get; set; }
        public string ProductItemCategory { get; set; }
        public int ProductID { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; }

    }

    public class DisplayProductItem : EgsProductItemViewModel
    {
        public Pager Pager { get; set; }
        public List<object> Items { get; set; }
    }

    public class HoldDisplayProductItems
    {
        public IEnumerable<DisplayProductItem> HoldAllProductItems { get; set; }
        public Pager Pager { get; set; }
    }

}
