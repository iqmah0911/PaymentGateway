using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Models
{
    
    public class TransactionsModel
    {
        public int WalletTransactionID { get; set; }

        //public EgsWallet Wallet { get; set; }

        //public EgsProductItem ProductItem { get; set; }

        //public SysTransactionType TransactionType { get; set; }

        //public EgsTransactionMethod TransactionMethod { get; set; }

        public double Amount { get; set; }
        public double SurCharge { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AccountNumber { get; set; }

        public string WalletReferenceNumber { get; set; }
        public string TransactionType { get; set; }
        public int TransactionTypeID { get; set; }
        public string TransactionMethod { get; set; }
        public int TransactionMethodID { get; set; }

        public string TransactionDescription { get; set; }

        public DateTime TransactionDate { get; set; }
        //public string IPAddress { get; set; }
        public string TransactionReferenceNo { get; set; }
        ////Added new columns 10-08-2021
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string Region { get; set; }
        public double Previous { get; set; }

        public double Current { get; set; }
        public string TransactionStatus { get; set; }

        ////public EgsInvoice Invoices { get; set; }
        //public int InvoiceID { get; set; }

        //public string ReferenceNo { get; set; }
        public string ServiceNumber { get; set; }

        public bool PaymentStatus { get; set; }

        public string PaymentReference { get; set; }

        public DateTime PaymentDate { get; set; }
        public string strPaymentDate { get; set; }

        ////public EgsProduct Product { get; set; }
        public int ProductItemID { get; set; }
        public string ProductItem { get; set; }
        public int? ProductID { get; set; }

        public DateTime DateCreated { get; set; }
        public string AlternateReferenceNo { get; set; }

        public string CustomerAlternateRef { get; set; }

        //public bool IsAggregatorSettled { get; set; }

        ////------------------------------------
        //public string CustomerEmail { get; set; }
        //public string CustomerName { get; set; }
        //public string RegistrationNo { get; set; }

        //public SysBank Bank { get; set; }

        //public IEnumerable<EgsWalletTransaction> WalletTransactions { get; set; }

        //public EgsWalletTransaction WalletTransaction { get; set; }

        //public IEnumerable<EgsStoreTransaction> StoreTransactions { get; set; }
    }


}
