using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class MerchantMappingModel : BasicObjectsViewModel
    {
        //
        public int MerchantMappingID { get; set; }
        public int MerchantID { get; set; }
        public int SubMerchantID { get; set; }
        //public string[] SubMerchantListID { get; set; }

    }

    public class MerchantsView
    {
        public int MerchantID { get; set; }
        public string MerchantName { get; set; }
        public bool IsChecked { get; set; }

    }
    
    public class MerchantList
    {
       public List<MerchantsView> subMerchant { get; set; }
    }

    public class SysMerchantMappingViewModel : BasicObjectsViewModel
    {
        //
        public int MerchantMappingID { get; set; }
        public int MerchantID { get; set; }

        //[Display(Name = "Merchant")]
        public string MerchantName { get; set; }

        public int SubMerchantID { get; set; }

        public int[] SubMerchantListID { get; set; }

        //[Display(Name = "Sub Merchant")]SubMerchantListID
        public string SubMerchantName { get; set; }

    }

    public class DisplayMerchants : SysMerchantMappingViewModel
    {
        public Pager Pager { get; set; }
        public List<object> Items { get; set; }
    }

    public class HoldDisplayMerchantss
    {
        public IEnumerable<DisplayMerchants> HoldAllMerchantss { get; set; }
        public Pager Pager { get; set; }
    }

    public class DisplayMerchantMappingViewModel : SysMerchantMappingViewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplayMerchantMappingViewModel
    {
        public IEnumerable<DisplayMerchantMappingViewModel> HoldAllMerchantMapping { get; set; }
        public List<DisplayMerchantMappingViewModel> HoldAllMerchantMappingList { get; set; }
        public Pager Pager { get; set; }
    }


}
