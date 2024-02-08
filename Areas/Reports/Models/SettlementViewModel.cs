using Microsoft.AspNetCore.Mvc;
using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Models
{
    public class SettlementViewModel
    { 
        public int SettlementSummaryID { get; set; } 
        public int MerchantID { get; set; }
        public string Narration { get; set; }
        public double AmountSettled { get; set; }
        public string AccountNo { get; set; }
        public DateTime DateCreated { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double TotalCollection { get; set; }
        public bool PaymentStatus { get; set; }

    }
     
    public class SettlementReportModel
    {
        [Display(Name = "Start Date"), Required(ErrorMessage = "Start Date Required")]
        [BindProperty, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date"), Required(ErrorMessage = "End Date Required")]
        [BindProperty, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime EndDate { get; set; }

        public int ProductID { get; set; }


    }
      
    public class HoldSettlementViewModel
    {
        public IEnumerable<SettlementViewModel> HoldAllSettlement { get; set; }
        public Pager Pager { get; set; }
    }
     
    public class SettlementDetailsViewModel
    {
        public int SettlementLogID { get; set; } 
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductItemID { get; set; }
        public string ProductItemName { get; set; }
        public double Totalcollection { get; set; }
        public int SettlementIntervalID { get; set; }
        public string SettlementIntervalName { get; set; }
        public int SalesID { get; set; } 
        public int MerchantID { get; set; } 
        public double Amount { get; set; }
        public string MerchantAccountNo { get; set; } 
        public int SettlementModeID { get; set; }
        public string SettlementModeName { get; set; }
        public int SettlementTypeID { get; set; }
        public string SettlementTypeName { get; set; }
        public DateTime ExactSettlementDate { get; set; }
        public int CreatedBy { get; set; }
        public bool IsPaid { get; set; } 
        public int BankID { get; set; }
        public string BankName { get; set; }
    }


    public class HoldSettlementDetailsViewModel
    {
        public IEnumerable<SettlementDetailsViewModel> HoldAllSettlementDetails { get; set; }
        public Pager Pager { get; set; }
    }

    public class HoldTransactionsViewModel
    {
        public IEnumerable<TransactionsModel> HoldAllTransactionsDetails { get; set; }
        public List<TransactionsModel> HoldAllDTransactionsDetails { get; set; }
        public Pager Pager { get; set; }
    }

    public class DropDownModelViews
    {
        public List<DDListViews> items { get; set; }
    }

    public class DDListViews
    {
        public string itemName { get; set; }
        public int itemValue { get; set; }
        public string strItemValue { get; set; }
    }








}
