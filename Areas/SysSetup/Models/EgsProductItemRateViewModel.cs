using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class EgsProductItemRateViewModel : BasicObjectsViewModel
    {
        public int ProductItemRateID { get; set; }
        public int ProductID { get; set; }
        public int ProductItemID { get; set; }
        public string Product { get; set; }
        public string ProductItem { get; set; }
        public double AmountRate { get; set; }


    }

    public class DisplayProductItemRate : EgsProductItemRateViewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplayProductItemRates
    {
        public IEnumerable<DisplayProductItemRate> HoldAllProductItemRates { get; set; }
        public Pager Pager { get; set; }
    }


}
