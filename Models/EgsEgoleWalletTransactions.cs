using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsEgoleWalletTransactions
    {
        [Key]
        public int TransactionID { get; set; }

        [ForeignKey("UserID")]
        public SysUsers User { get; set; }

        [ForeignKey("BankID")]
        public SysBank Bank { get; set; }

        public double TransferAmount { get; set; }

        [ForeignKey("TransactionTypeID")]
        public SysTransactionType TransactionType { get; set; }

        [ForeignKey("TransactionMethodID")]
        public EgsTransactionMethod TransactionMethod{ get; set; }

        [ForeignKey("WalletID")]
        public EgsWallet Wallet { get; set; }

        public DateTime DateCreated { get; set; }

        public string Status { get; set; }

        public string Narration { get; set; }
        //new columns
        public string  ReceiverAccount { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverBank { get; set; }

        //Added 290323

        [ForeignKey("WalletTransactionID")]
        public EgsWalletTransaction WalletTransaction { get; set; }

        public string SessionID { get; set; }
         
        //
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }




    }
}
