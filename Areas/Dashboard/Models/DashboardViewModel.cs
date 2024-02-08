using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Dashboard.Models
{
    public class DashboardViewModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int TotalAgentsCount { get; set; }
        public int TotalMerchantCount { get; set; }
        public int AgentsCount { get; set; }
        public int MerchantCount { get; set; }
        public int dAgentsCount { get; set; }
        public int dMerchantCount { get; set; }
        public int ActiveAgentCount { get; set; }
        public int InActiveAgentCount { get; set; }
        public int ActiveMerchantCount { get; set; }
        public int InActiveMerchantCount { get; set; }
        public int TransactionCount { get; set; }
        public string TotalBankCharges { get; set; }
        public string TotalTransfer { get; set; }
        public string TotalWalletBal { get; set; }
        public string TransactionValue { get; set; }
        public int CompletedTransactionCount { get; set; }
        public double CompletedTransactionValue { get; set; }
        public int UnCompletedTransactionCount { get; set; }
        public double UnCompletedTransactionValue { get; set; }
        //public ProductData ProductList { get; set; }
        public List<ProductDashData> ProductList { get; set; }
        public List<TransactionHistory> HoldAllTransactionHistory { get; set; }

        public List<ChartReporttData> chartReporttDatas { get; set; }
        public List<AgentsDashData> AgentsList { get; set; }
        public string PaidCommisionValue { get; set; }
        public string UnPaidCommisionValue { get; set; }
    }

    public class AgentsDashData
    {
        public int UserID { get; set; }
        public string AccountInfo { get; set; }
        public double TotalAmount { get; set; }
    }

    public class ProductDashData
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double TotalAmount { get; set; }
    }

    public class ProductData
    {
        public List<ProductDashData> dashProducts { get; set; }
    }

    public class ChartReporttData
    {
        public string Day { get; set; }
        public int Amount { get; set; }
        //public List<ProductDashData> dashProducts { get; set; }
    }


    public class DisplayProduct
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
    }

    public class HoldDisplayProduct 
    {
        //public int RoleID { get; set; }
        //public string RoleName { get; set; }
        //public int TransactionCount { get; set; }
        //public double TransactionValue { get; set; }
        //public int CompletedTransactionCount { get; set; }
        //public double CompletedTransactionValue { get; set; }
        //public int UnCompletedTransactionCount { get; set; }
        //public double UnCompletedTransactionValue { get; set; }
        public IEnumerable<ProductDashData> HoldProducts { get; set; }


    }
    public class TransactionHistory
    {
        public int TransactionID { get; set; } 
        public double Amount { get; set; }
        public bool PaymentStatus { get; set; }
        public string TransactionMethod { get; set; }
        public string TransactionType { get; set; }

        public string ProductName { get; set; }
        public string ProductItemName { get; set; }
        public DateTime PaymentDate { get; set; }
    }
    public class HoldTransactionHistory
    {
        public IEnumerable<TransactionHistory> HoldAllTransactionHistory { get; set; }
        public Pager Pager { get; set; }
    }
}
