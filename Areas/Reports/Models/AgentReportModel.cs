using Microsoft.AspNetCore.Mvc;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Models
{
    public class AgentReportModel
    {
        [Display(Name = "Start Date"), Required(ErrorMessage = "Start Date Required")]
        [BindProperty, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date"), Required(ErrorMessage = "End Date Required")]
        [BindProperty, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime EndDate { get; set; } 
        public string ReferenceNumber { get; set; }
        public string ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductItemID { get; set; }
        public string ProductItemName { get; set; }
        public string ProductItemCategory { get; set; }
        public string UserID { get; set; }
        public string RPTWalletID { get; set; }
    }

  

    public class HoldDisplayInvoiceViewModel
    {
        public IEnumerable<EgsInvoice> HoldAllInvoice { get; set; }
        public Pager Pager { get; set; }
    }

}
