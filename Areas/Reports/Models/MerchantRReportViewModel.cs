using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Models
{
    public class MerchantRReportViewModel
    {
        public int InvoiceID { get; set; }

        public string ReferenceNo { get; set; }
        public string ServiceNumber { get; set; }
        public double Amount { get; set; }
        public bool PaymentStatus { get; set; }

        public string PaymentReference { get; set; }

        public DateTime PaymentDate { get; set; }

        public string ProductName { get; set; }
        public string ProductItemName { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
        public string AlternateReferenceNo { get; set; }



    }
    public class HoldInvoicesViewModel
    {
        public IEnumerable<MerchantRReportViewModel> HoldInvoices { get; set; }
        public Pager Pager { get; set; }
    }
}

