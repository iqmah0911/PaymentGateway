using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Models
{
    public class EgsAccountTypeViewModel
    {
        public int AccountTypeID { get; set; }
        public string AccountTypeName { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

    }

    public class CreateAccountTypeViewModel
    {
        public int AccountTypeID { get; set; }
        public string AccountTypeName { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateAccountTypeViewModel
    {
        public int AccountTypeID { get; set; }
        public string AccountTypeName { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class DisplayAccountTypeViewModel
    {
        public int AccountTypeID { get; set; }
        public string AccountTypeName { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class HoldDisplayAccountTypeViewModel
    {
        public IEnumerable<DisplayAccountTypeViewModel> HoldAllAccountType { get; set; }
        public Pager Pager { get; set; }
    }
}
