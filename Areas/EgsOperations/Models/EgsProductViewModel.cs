using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Models
{
    public class EgsProductViewModel
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Prodct Name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Prodct Code is required")]
        [StringLength(60, ErrorMessage = "Prodct Code can't be longer than 60 characters")]
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string Image { get; set; }
        public string Notification { get; set; }
        public string Verification { get; set; }
        public bool IsFixedAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductCategory { get; set; }
        public int InvoiceModeID { get; set; }
        public int MerchantID { get; set; }
        public int UserID { get; set; }

    }
    public class APIProductViewModel
    {
        //public int ProductID { get; set; }
        public string ProductName { get; set; }
        //public string ProductDescription { get; set; }
        //public string Notification { get; set; }
        //public string Verification { get; set; }
        //public bool IsFixedAmount { get; set; }
        //public DateTime DateCreated { get; set; }
        //public int CreatedBy { get; set; }
        //public int ProductCategoryID { get; set; }
        //public string ProductCategory { get; set; }

    }

    public class DisplayProduct
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public string ProductDescription { get; set; }
        public string Notification { get; set; }
        public string Verification { get; set; }
        public bool IsFixedAmount { get; set; }
        public string ProductCategory { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class HoldDisplayProduct
    {
        public IEnumerable<DisplayProduct> HoldAllProduct { get; set; }
        public Pager Pager { get; set; }
    }


    public class AirtimeProducts
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }

        public int ProductModeID { get; set; }
    }

    public class AirtimeProductList
    {
        public List<AirtimeProducts> AirtimeList { get; set; }
    }


    public class DataProducts
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ServiceCode { get; set; }
        public int ProductModeID { get; set; }
    }

    public class DataProductList
    {
        public List<DataProducts> DataProdList { get; set; }
    }









}
