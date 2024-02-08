using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class SysTransactionType
    {
        [Key]
        public int TransactionTypeID { get; set; }

        public string TransactionType { get; set; }

       // [ForeignKey("UserID")]
        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }


        [InverseProperty("TransactionType")]
        public IEnumerable<EgsWalletTransaction> WalletTransactions { get; set; }

        [InverseProperty("TransactionType")]
        public IEnumerable<EgsStoreTransaction> StroreTransactions { get; set; }

        [InverseProperty("TransactionType")]
        public IEnumerable<EgsEgoleWalletTransactions> EgoleWalletTransactions { get; set; }

    }
}
