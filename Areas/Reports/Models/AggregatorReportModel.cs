using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Models
{
    public class AggregatorReportModel
    {
        [Display(Name = "Start Date"), Required(ErrorMessage = "Start Date Required")]
        [BindProperty, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date"), Required(ErrorMessage = "End Date Required")]
        [BindProperty, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime EndDate { get; set; }

        public string UserID { get; set; }
        public string AggID { get; set; }

        public int ProductID { get; set; }

    }
}
