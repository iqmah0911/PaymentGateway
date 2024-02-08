using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Models
{
    public class EgsMerchantTypeViewModel
    {
        public int MerchantTypeID { get; set; }
        public string MerchantTypeName { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

    }

    public class HoldDisplayMerchantTypeViewModel
    {
        public IEnumerable<EgsMerchantTypeViewModel> HoldAllMerchantType { get; set; }
        public Pager Pager { get; set; }
    }
}
