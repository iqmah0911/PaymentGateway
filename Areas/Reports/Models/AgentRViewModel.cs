using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Models
{
    public class AgentRViewModel
    { 
        public int InvoiceID { get; set; }

        public string ReferenceNo { get; set; }
        public string ServiceNumber { get; set; }
        public double Amount { get; set; }
        public double Previous { get; set; }
        public double Current { get; set; }
        public bool PaymentStatus { get; set; }
        
        public string PaymentReference { get; set; }

        public DateTime PaymentDate { get; set; }

        public string DatePaid { get; set; }

        public string TimePaid { get; set; }

        public string ProductName { get; set; }
        public string ProductItemName { get; set; }
        public string ProductItemCategory { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string BankAcctCode { get; set; }

        public DateTime DateCreated { get; set; }
        public string InvDate { get; set; }
        public string TransactionDate { get; set; }
        public string AlternateReferenceNo { get; set; }
        public string CustomerAlternateRef { get; set; }
        public int ItemCount { get; set; }

        public string ProductItemCode { get; set; }

        public double CommissionAmount { get; set; }

        public int UserID { get; set; }
        public string TotalAmt { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDescription { get; set; }

        public string TransactionReferenceNo { get; set; }
        public int TransactionMethodID { get; set; }
        public string TransactionMethodName { get; set; }
        public string Region { get; set; }

    }

    public class HoldInvoicesRViewModel
    {
        public IEnumerable<AgentRViewModel> HoldAllInvoices { get; set; }
        public Pager Pager { get; set; }
    }

    public class DisplayInvoiceTrans : AgentRViewModel
    {
        public Pager Pager { get; set; }
        public List<object> Items { get; set; }
    }

    public class WeeklyRViewModel
    {
        public int WalletID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public double Amount { get; set; }
        public int TransCount { get; set; }
        
        public string TransPeriod { get; set; }
        
    }

    public class HoldWeeklyRViewModel
    {
        public IEnumerable<WeeklyRViewModel> HoldAllInvoices { get; set; }
        public Pager Pager { get; set; }
    }

    public class DisplayWeeklyRViewModel : WeeklyRViewModel
    {
        public Pager Pager { get; set; }
        public List<object> Items { get; set; }
    }


    public class  AgentNasModel
    {
        public DateTime Period { get; set; }
        public string Agent { get; set; }
        public string BusinessName { get; set; }
        public string ItemCount { get; set; }

    }

    public class HoldAgentNasModel
    {
        public IEnumerable<AgentNasModel> HoldAllInvoices { get; set; }
        public Pager Pager { get; set; }
    }

    public class DisplayAgentNasModel : AgentNasModel
    {
        public Pager Pager { get; set; }
        public List<object> Items { get; set; }
    }








    public class HoldDisplayInvoiceTrans
    {
        public IEnumerable<AgentRViewModel> HoldRPTInvoiceTrans { get; set; }
        public Pager Pager { get; set; }
    }

    public class HoldDisplayAuditTrail
    {
        public IEnumerable<AuditTrailViewModel> HoldRPTAuditTrail { get; set; }
        public Pager Pager { get; set; }
    }

    public class SettlementRViewModel
    {
        public int InvoiceID { get; set; } 
        public string ProductName { get; set; }
        public string ProductItemName { get; set; }
        public string ProductItemCategory { get; set; } 
        public int ItemCount { get; set; } 
        public double transactioncvalue { get; set; } 
        public double Expectedvalue { get; set; } 
        public double settlementValue { get; set; }
        public double commission { get; set; }
        public DateTime PaymentDate { get; set; }

        public double Amount { get; set; }

        public string ReferenceNumber { get; set; }
        public string PaymentReference { get; set; }

        public string WalletInfo { get; set; }
        public string AccountInfo { get; set; }

        public int WalletID { get; set; }

        public int ProductItemID { get; set; }
        public string ServiceNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public bool PaymentStatus { get; set; }
        public int TransactionMethodID { get; set; }
        public int TransactionTypeID { get; set; }
        public string AlternateReferenceNo { get; set; }
        public string formattedAmount { get; set; }

        public string Outlet { get; set; }
        public string Aggregator { get; set; }
        public string Period { get; set; }
        public int AgentsCounts { get; set; }

        public string Agent { get; set; }
        public string AccountNumber { get; set; } 
        public int TransCount { get; set; }
       // public int Commission { get; set; }
        public string CommissionPeriod { get; set; }
        public string Item { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
        public double Balance { get; set; }
        public string Company { get; set; } 
        public string LastName { get; set; }


    }
     
    public class LSettlementRViewModel
    {
        public List<SettlementRViewModel>  AllSettlement { get; set; }
        
    }

    public class HoldSettlementRViewModel
    {
        public IEnumerable<SettlementRViewModel> HoldAllSettlement { get; set; }
        public Pager Pager { get; set; }
    }


    public class OpeningWalletViewModel
    {
        public double OpeningBalance { get; set; }
        public double ClosingBalance { get; set; }
        public string Agent { get; set; }
        public string BusinessName { get; set; }
        public double WalletBalance { get; set; }
    }

    public class LOpeningWalletViewModel
    {
        public List<OpeningWalletViewModel> AllSettlement { get; set; }

    }

    public class HoldOpeningWalletViewModel
    {
        public IEnumerable<OpeningWalletViewModel> HoldAllSettlement { get; set; }
        public Pager Pager { get; set; }
    }


}
