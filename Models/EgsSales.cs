using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class EgsSales
    {
        [Key]
        public int SalesID { get; set; }

        public int? WalletTransactionID { get; set; }
        public double DiscountedAmount { get; set; }
        public double CommisionAmount { get; set; }

        [ForeignKey("ProductID")]
        public EgsProduct Product { get; set; }

        [ForeignKey("ProductItemID")]
        public EgsProductItem ProductItem { get; set; }

        //[ForeignKey("ProductItemrateID")]
        //public int ProductItemRateID { get; set; }
        public bool IsSettled { get; set; }

        public string IPAddress { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public string SalesReferenceNumber { get; set; }
        //public string WalletReferenceNumber { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime SettlementDate { get; set; }
        //New Column
        //public string BankAccountID { get; set; }

    }
}
