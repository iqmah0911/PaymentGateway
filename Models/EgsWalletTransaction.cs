using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway21052021.Models
{
    public class EgsWalletTransaction
    {
        [Key]
        public int WalletTransactionID { get; set; }

        [ForeignKey("WalletID")]
        public EgsWallet Wallet{ get; set; }

        [ForeignKey("ProductItemID")]
        public EgsProductItem ProductItem { get; set; }
        //public int ProductItemID { get; set; }


        [ForeignKey("TransactionTypeID")]
        public SysTransactionType TransactionType { get; set; }

        [ForeignKey("TransactionMethodID")]
        public EgsTransactionMethod TransactionMethod { get; set; }

        public double Amount { get; set; }

        public string WalletReferenceNumber { get; set; }

        //public string TransactionDescription { get; set; }

        public DateTime TransactionDate { get; set; }
        public string IPAddress { get; set; }
        public string TransactionReferenceNo { get; set; }
        //Added new columns 10-08-2021
        public int CreatedBy { get; set; }
        public double Previous { get; set; }

        public double Current { get; set; } 
        public string TransactionStatus { get; set; }

        public double SurCharge { get; set; }


        [ForeignKey("TerminalKey")]
        public EgsTerminal Terminals { get; set; }
        [ForeignKey("InvoiceID")]
        public EgsInvoice Invoices { get; set; }

        [ForeignKey("TransactionID")]
        public EgsEgoleWalletTransactions EgoleWalletTransaction { get; set; }

        [ForeignKey("BillerID")]
        public SysBillers Billers { get; set; }

        // Added on 26/7/2023
        public string Narration { get; set; }
        
        public string ReceiverAccount { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverBank { get; set; }
         
        public string SessionID { get; set; }

        public string TransactionClientType { get; set; }

        public int IsCompleted { get; set; }

        public string MainAccountNumber { get; set; }

        public string PayItemRef { get; set; }

        public bool IsSettled { get; set; }

        public bool IsPushed { get; set; }

        public DateTime DateSettled { get; set; }

        public DateTime DatePushed { get; set; }

    }
}
