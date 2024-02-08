using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class EgsProductItem
    {
        [Key]
        public int ProductItemID { get; set; }
        public string ProductItemCode { get; set; }

        [ForeignKey("ProductID")]
        public EgsProduct Product { get; set; }


        [ForeignKey("UserID")]
        public SysUsers Merchant { get; set; }
        //public SysUsers ProductOwner { get; set; } //please add migration to modify the changes

        public string ProductItemName { get; set; }
        public double DiscountedAmount { get; set; }
        public double CommisionAmount { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public bool Istransactionfee { get; set; }
        //New column
        public string ProductItemCategory { get; set; }

        public int CostRecovery { get; set; }


        [InverseProperty("ProductItem")]
        public IEnumerable<EgsProductItemRate> ProductItemRates { get; set; }

        [InverseProperty("ProductItem")]
        public IEnumerable<EgsSettlementLog> SettlementLogs { get; set; }



        [InverseProperty("ProductItem")]
        public IEnumerable<EgsWalletTransaction> WalletTransactions { get; set; }

        [InverseProperty("ProductItem")]
        public IEnumerable<EgsStoreTransaction> StoreTransactions { get; set; }

        [InverseProperty("ProductItem")]
        public IEnumerable<EgsSales> Sales { get; set; }
    }
}
