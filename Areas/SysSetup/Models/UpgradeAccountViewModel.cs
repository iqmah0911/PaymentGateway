using PaymentGateway21052021.Areas.Dashboard.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class UpgradeAccountViewModel : SysCustomerViewModel
    { 
        public int UpgradeAccountID { get; set; }

        [ForeignKey("UserID")]
        public SysUsers User { get; set; }
        public string UserName { get; set; }

        public int RoleRequestId { get; set; }
        public string RoleRequestName { get; set; }
        ////public DateTime DateCreated { get; set; }
        public bool IsProcessed { get; set; }
        public int ProcessedBy { get; set; }
        public DateTime DateProcessed { get; set; }
        public int RejectedBy { get; set; }
        public DateTime DateRejected { get; set; }
        public string Comment { get; set; }

        ////public List<SDocument> documents { get; set; }

         
        //added params   
        public string Company { get; set; }

        //This was already inhearited
        ////public string Email { get; set; }
        ////public string Address { get; set; }
        ////public string FirstName { get; set; }
        ////public string MiddleName { get; set; }
        ////public string LastName { get; set; }

        //public int BankID { get; set; }
        //public string BankAccount { get; set; }  
    }

    //public class HoldUpgradeAccountViewModel
    //{ 
    //    public int RoleID { get; set; }
    //    public int UserKYCID { get; set; }
    //    public string Company { get; set; }
    //    public string Email { get; set; }
    //    public string Address { get; set; }
    //    public int BankID { get; set; }
    //    public string BankAccount { get; set; }
    //    public int IdentificationTypeID { get; set; }
    //    public string IdentificationValue { get; set; }

    //}

    public class HoldUpgradeAccountViewModel
    {
        public IEnumerable<UpgradeAccountViewModel> HoldAllRequests { get; set; }
        public Pager Pager { get; set; }
    }

}
