using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysUserKYCInfoViewModel
    {
    }

    public class CreateUserKYCInfoViewModel
    {
        public int UserKYCInfoID { get; set; }
        public int IdentificationTypeID { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationValue { get; set; }
        public string BankAccount { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateUserKYCInfoViewModel
    {
        public int UserKYCInfoID { get; set; }
        public int IdentificationTypeID { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationValue { get; set; }
        public string BankAccount { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class DisplayUserKYCInfoViewModel
    {
        public int UserKYCInfoID { get; set; }
        public DateTime DateCreated { get; set; }
        public int IdentificationTypeID { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationValue { get; set; }
        public string BankAccount { get; set; }
        public Pager Pager { get; set; }
        public IEnumerable<object> Items { get; set; }
    }

    public class HoldDisplayUserKYCInfoViewModel
    {
        public IEnumerable<DisplayUserKYCInfoViewModel> HoldAllUserKYCInfo { get; set; }
        public Pager Pager { get; set; }
    }
}
