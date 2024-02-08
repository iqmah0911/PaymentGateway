using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysBankViewModel
    {
        public int BankID { get; set; }

        public string BankName { get; set; }

        public string BankCode { get; set; }

        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        // public IEnumerable<EgsAccountsInfo> AccountsInfos { get; set; }

    }
    
     
    public class DisplayBankViewModel
    {
        public int BankID { get; set; }

        public string BankName { get; set; }

        public string BankCode { get; set; }

        public DateTime DateCreated { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplayBankViewModel
    {
        public IEnumerable<DisplayBankViewModel> HoldAllBank { get; set; }
        public Pager Pager { get; set; }
    }
}
