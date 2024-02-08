using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Models
{
    public class EgsGLAccountViewModel
    {
        public int GLAccountID { get; set; }
        public string GLAccountCode { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public int BankID { get; set; }
        public int CompanyID { get; set; }
        public string Bank { get; set; }
        public string Company { get; set; }
        public string BankAccount { get; set; }
    }
    public class DisplayGLAccountViewModel
    {
        public int GLAccountID { get; set; }
        public string GLAccountCode { get; set; }
        public string Bank { get; set; }
        public string Company { get; set; }
        public DateTime DateCreated { get; set; }
        public string BankAccount { get; set; }
    }
    public class HoldDisplayGLAccountViewModel
    {
        public IEnumerable<DisplayGLAccountViewModel> HoldAllGLAccounts { get; set; }
        public Pager Pager { get; set; }
    }
}
