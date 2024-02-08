using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Models
{
    public class EgsAccountInfoViewModel 
    {

        public int AccountInfoID { get; set; }
        public string AccountNumber { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }

    public class CreateAccountInfoViewModel
    {
        public int AccountInfoID { get; set; }
        public string AccountNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateAccountInfoViewModel
    {
        public int AccountInfoID { get; set; }
        public string AccountNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class DisplayAccountInfoViewModel
    {
        public int AccountInfoID { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class HoldDisplayAccountInfoViewModel
    {
        public IEnumerable<DisplayAccountInfoViewModel> HoldAllAccountInfo { get; set; }
        public Pager Pager { get; set; }
    }
}
