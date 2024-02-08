using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class EgsInvoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [StringLength(150)]
        public string ReferenceNo { get; set; }
        public string ServiceNumber { get; set; }
        public double Amount { get; set; }
        public bool PaymentStatus { get; set; }

        [StringLength(150)]
        public string PaymentReference { get; set; }

        public DateTime PaymentDate { get; set; }

        [ForeignKey("ProductID")]
        public EgsProduct Product { get; set; }
        public int ProductItemID { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
        public string AlternateReferenceNo { get; set; }

        public string CustomerAlternateRef { get; set; }

        public bool IsAggregatorSettled { get; set; }

         //------------------------------------
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string RegistrationNo { get; set; }

        public double SurCharge { get; set; }

        public bool AggregatorPushed { get; set; }

        public string PhoneNumber { get; set; }
         
        public string TerminalID { get; set; }

        [ForeignKey("BankID")]
        public SysBank Bank { get; set; }

        [InverseProperty("Invoices")]
        public IEnumerable<EgsWalletTransaction> WalletTransactions { get; set; }

        [InverseProperty("Invoices")]
        public IEnumerable<EgsStoreTransaction> StoreTransactions { get; set; }
    }
}
