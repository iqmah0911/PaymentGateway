using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Models
{
    public class AuditTrailViewModel
    {
        [Display(Name = "Start Date"), Required(ErrorMessage = "Start Date Required")]
        [BindProperty, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date"), Required(ErrorMessage = "End Date Required")]
        [BindProperty, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime EndDate { get; set; }
        public int AID { get; set; }
        public string DbAction { get; set; }
        public string Description { get; set; }
        public string Page { get; set; }

        public DateTime DateCreated { get; set; }
        public string IPAddress { get; set; }
        public int CreatedBy { get; set; }
        public string Role { get; set; }
        public string Menu { get; set; }


    }
}
