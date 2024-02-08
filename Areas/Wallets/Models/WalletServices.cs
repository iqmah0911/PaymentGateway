using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Wallets.Models
{
    public class WalletServices : OrdersModel
    {
        public int ProductCategoryID { get; set; }
        public string ProductCategoryCode { get; set; }
        public string ProductCategoryName { get; set; }
        public string searchText { get; set; }
        public int ProductItemRateID { get; set; }
        public string PhoneEmail { get; set; }
        public string LoginUser { get; set; }
        public bool IsFixedAmount { get; set; }
        public int CreatedBy { get; set; }
    }

  

}
