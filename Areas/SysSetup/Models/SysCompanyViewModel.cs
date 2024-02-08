using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysCompanyViewModel
    {
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }
        public int GLAAccountID { get; set; }
        public string GLAccountName { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

    }


    public class DisplayCompanyViewModel
    {
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }
        public int GLAAccountID { get; set; }
        public string GLAccountName { get; set; }
        public int GLAAccount { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class HoldDisplayCompanyViewModel
    {
        public IEnumerable<DisplayCompanyViewModel> HoldAllCompanies { get; set; }
        public Pager Pager { get; set; }
    }
}
