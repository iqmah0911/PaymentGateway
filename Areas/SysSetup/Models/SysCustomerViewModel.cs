using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysCustomerViewModel
    { 
            public int UserID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
            public string FullNames { get; set; }
            public string BusinessName { get; set; }
            public string BusinessAddress { get; set; }
            // public int AccounTypeID { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public string Gender { get; set; }
           // public string AccountNo { get; set; }
            public string KYC { get; set; }
            public DateTime DateCreated { get; set; }
            public int CreatedBy { get; set; }
            public int ApprovedBy { get; set; }
            public int RoleID { get; set; }
            public string Role { get; set; }
            public int UserTypeID { get; set; }
            public int UserContactInfoID { get; set; }
            public int StateID { get; set; }
            public string StateName { get; set; }
            public int LgaID { get; set; }
            public string LgaName { get; set; }
            public string CompanyName { get; set; }
            //public string MerchantCode { get; set; }
            public string BVN { get; set; }
            //Please do not delete the Date DataType method, it is functioning and useful but if you insist to delete, please contact Naasiruddeen or Ehis
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime DOB { get; set; }

            public List<CustDocument> documents { get; set; } 
        }


        public class CustDocument
        {
            public int DocumentID { get; set; }
            public string DocumentName { get; set; }
            public string DocumentPath { get; set; }
            public DateTime DateCreated { get; set; }
            public int UserKYCID { get; set; }

        }

    public class HoldCustomerViewModel
    {
        public IEnumerable<SysCustomerViewModel> HoldAllCustomers { get; set; }
        public Pager Pager { get; set; }
    }





}
