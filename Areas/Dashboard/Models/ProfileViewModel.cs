using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Dashboard.Models
{
    public class ProfileViewModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int AccounTypeID { get; set; }
        public string AccountTypeName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        //public int UserTypeID { get; set; }
        public int UserContactInfoID { get; set; }
        public string WalletNumber { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int IdentificationTypeID { get; set; }
        public string IdentificationTypeName { get; set; }
        public string ProfileImage { get; set; }
    }

    public class DisplayProfileViewModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int AccounTypeID { get; set; }
        public string AccountTypeName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileIMG { get; set; }
        public string WalletNumber { get; set; }
        public string Address { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        //public int UserTypeID { get; set; }
        public int UserContactInfoID { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int IdentificationTypeID { get; set; }
        public string IdentificationTypeName { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public bool IsGeneral { get; set; }
        public bool IsFilled { get; set; }

        //Aggregator commsnmodel

        public HoldDisplayAggViewModel DisplayAggViewModel { get; set; }
    }

    public class HoldDisplayAggViewModel
    {
        public IEnumerable<AggregatorCommsnVModel> HoldAllCommsn { get; set; }
        public Pager Pager { get; set; }
    }



    public class AggregatorCommsnVModel
    {
        public string monthname { get; set; }
        public string Business { get; set; }
        public int SettlementDate { get; set; }
        public double Amount { get; set; }
    }


    public class SysViewModel
    {
        public int KYCID { get; set; }
        public int IdentificationTypeID { get; set; }
        public string IdentificationValue { get; set; }
        public string BankAccount { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int UserID { get; set; }
        public int BankID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }

        public List<SDocument> Documents { get; set; }
    }

    public class PostMailViewModel
    {
        public string email { get; set; }
        public string subject { get; set; }
        public string htmlMessage { get; set; }

    }


    public class PostSysViewModel
    {
        public string Email { get; set; }
        public int KYCID { get; set; }
        public int IdentificationTypeID { get; set; }
        public string IdentificationValue { get; set; }
        public string BankAccount { get; set; }
        public int RoleID { get; set; }
        public int RoleRequestID { get; set; }
        public string RoleName { get; set; }
        public int UserID { get; set; }
        public int BankID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        //To be Uploaded 
        public List<SDocument> Documents { get; set; }  
    }

    public class SDocument
    { 
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public DateTime DateCreated { get; set; } 
        public int UserKYCID { get; set; }
         
    }


    public class PostUpgradeViewModel
    {
        public int UpgradeAccountID { get; set; }
        public string Email { get; set; }  
        public int RoleRequestID { get; set; } 
        public DateTime DateCreated { get; set; }
        public bool IsProcessed { get; set; }
        public int ProcessedBy { get; set; }
        public DateTime DateProcessed { get; set; }
        public int RejectedBy { get; set; }
        public DateTime DateRejected { get; set; }
        public string Comment { get; set; }
        public int CreatedBy { get; set; }

    }


    public class UpgradeAccountResponse
    {
        public string StatusCode { get; set; }
        public string message { get; set; }
        /////////////
        //public string Email { get; set; }
        //public int KYCID { get; set; }
        //public int IdentificationTypeID { get; set; }
        //public string IdentificationValue { get; set; }
        //public string BankAccount { get; set; }
        //public int RoleID { get; set; }
        //public string RoleName { get; set; }
        //public int UserID { get; set; }
        //public int BankID { get; set; }
        //public string CompanyName { get; set; }
        //public string Address { get; set; }



          
    }




}
