using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsStoreTransaction
    {
        [Key]
        public int StoreTransactionID { get; set; }
        
        [ForeignKey("ProductItemID")]
        public EgsProductItem ProductItem { get; set; }
        
        [ForeignKey("TransactionTypeID")]
        public SysTransactionType TransactionType { get; set; }
        
        [ForeignKey("TransactionMethodID")]
        public EgsTransactionMethod TransactionMethod { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string IPAdress { get; set; }
        public string TransactionReferenceNumber { get; set; }
        
        [ForeignKey("InvoiceID")]
        public EgsInvoice Invoices { get; set; }
        public string PhoneEmail { get; set; }
        public string RegistrationNumber { get; set; } //ProductParams if a text or dropdown
    }
}
