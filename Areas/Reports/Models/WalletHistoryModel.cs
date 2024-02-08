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
    public class WalletHistoryModel
    {
        [Display(Name = "Start Date"), Required(ErrorMessage = "Start Date Required")]
        [BindProperty, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date"), Required(ErrorMessage = "End Date Required")]
        [BindProperty, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Transaction Type"), Required(ErrorMessage = "Transaction Type Required")]
        public string TransactionType { get; set; }
        public int UserID { get; set; }
        public int WalletID { get; set; }

    }

    public class HoldDisplayHistoryViewModel
    {
        public IEnumerable<EgsWalletTransaction> HoldAllWalletHistory { get; set; }
        public Pager Pager { get; set; }
    }
}
