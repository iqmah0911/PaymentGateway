using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsTransactionMethod
    {
        [Key]
        public int TransactionMethodID { get; set; }

        public string TransactionMethod { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

    //    [InverseProperty("TransactionMethod")]
    //    public IEnumerable<EgsWalletTransaction> WalletTransactions { get; set; }

    //    [InverseProperty("TransactionMethod")]
    //    public IEnumerable<EgsStoreTransaction> StroreTransactions { get; set; }

    //    [InverseProperty("TransactionMethod")]
    //    public IEnumerable<EgsEgoleWalletTransactions> EgoleWalletTransactions { get; set; }
    }
}
