using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class EgsProduct
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductParameter { get; set; }
        public string Image { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("ProductCategoryID")]
        public EgsProductCategory ProductCategories { get; set; }
        public string Notification { get; set; }
        public string Verification { get; set; }
        public double DiscountedAmount { get; set; }
        public double CommisionAmount { get; set; }

        public string ProductDescription { get; set; }
        public bool IsFixedAmount { get; set; }
        public bool IsActive { get; set; }
        public int InvoiceModeID { get; set; }
        public string IPAddress { get; set; }
        public string APIKey { get; set; }

        [ForeignKey("MerchantID")]
        public EgsMerchant Merchant { get; set; }


        [InverseProperty("Product")]
        public IEnumerable<EgsProductItem> ProductItems { get; set; }

        [InverseProperty("Product")]
        public IEnumerable<EgsInvoice> Invoices { get; set; }
        [InverseProperty("Product")]
        public IEnumerable<EgsSettlementLog> SettlementLogs { get; set; }

        [InverseProperty("Product")]
        public IEnumerable<EgsSettlementSummary> SettlementSummarys { get; set; }

        [InverseProperty("Product")]
        public IEnumerable<EgsSales> Sales { get; set; }


        public string NotificationIndicator { get; set; }

        public string VerificationIndicator { get; set; }

        public string ActionUrl { get; set; }

        public string ActionResult { get; set; }

        public string AuthenticationUrl { get; set; }

        public string AuthenticationResult { get; set; }

    }
}
