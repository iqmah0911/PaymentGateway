using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Models
{
   
        public class InvoiceViewModel
        {
            public int InvoiceID { get; set; }
            public string ReferenceNo { get; set; }
            public double Amount { get; set; }
            public bool PaymentStatus { get; set; }
            public string PaymentReference { get; set; }
            public string Bank { get; set; }
            public DateTime PaymentDate { get; set; }
            public DateTime DateCreated { get; set; }
            public string TransactionMethod { get; set; }
            public int CreatedBy { get; set; }
        }

        public class UpdateInvoiceViewModel
        {
            public int InvoiceID { get; set; }
            public string ReferenceNo { get; set; }
            public double Amount { get; set; }
            public bool PaymentStatus { get; set; }
            public string PaymentReference { get; set; }
            public string Bank { get; set; }
            public DateTime PaymentDate { get; set; }
            public DateTime DateCreated { get; set; }
        }

        public class DisplayInvoiceViewModel
        {
            public int InvoiceID { get; set; }
            public string ReferenceNo { get; set; }
            public double Amount { get; set; }
            public bool PaymentStatus { get; set; }
            public string PaymentReference { get; set; }
            public string Bank { get; set; }
            public DateTime PaymentDate { get; set; }
            public DateTime DateCreated { get; set; }
            public Pager Pager { get; set; }
            public IEnumerable<object> Items { get; set; }
        }

        public class HoldDisplayInvoiceViewModel
        {
            public IEnumerable<DisplayInvoiceViewModel> HoldAllInvoice { get; set; }
            public Pager Pager { get; set; }
        }

    public class InvoiceParamsList
    {
        public int InvoiceID { get; set; }
        public string ReferenceNo { get; set; }
        public double Amount { get; set; }
        public int ProductID { get; set; }
        public int ProductItemID { get; set; }
    }

    public class InvoicesModel
    {
        public int InvoiceID { get; set; }
        public string ReferenceNo { get; set; }
        public string ServiceNumber { get; set; }
        public double Amount { get; set; }
        public bool PaymentStatus { get; set; }

        public string PaymentReference { get; set; }

        public DateTime PaymentDate { get; set; }

        public int ProductID { get; set; }
        public int ProductItemID { get; set; }

        public int CreatedBy { get; set; } 
        public DateTime DateCreated { get; set; }
        public string AlternateReferenceNo { get; set; }

        public InvoiceParamsList InvoiceListParameters { get; set; }

    }

}
