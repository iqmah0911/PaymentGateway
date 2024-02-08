using PaymentGateway21052021.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Models
{
    public class SysUserViewModel
    {
    }

    public class corporatedata
    {
        public string accountNo { get; set; }
        public string accountName { get; set; }
    }

    public class coporateres
    {
        public string status { get; set; }
        public string message { get; set; }
        public corporatedata data { get; set; }
    }


    public class ChangePasswordViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class VVFDResponseViewModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public DataViewModel data { get; set; }
    }

    public class DataViewModel
    {
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string bvn { get; set; }
        public string phone { get; set; }
        public string dob { get; set; }
        public string accountNo { get; set; }
    }

    public class CreateUserViewModel
    {
        public string ReferralCode { get; set; }
        //---BankAccount number for subagents is Referral code
        public int UserID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        public int AccounTypeID { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public string Gender { get; set; }
        public string AccountNo { get; set; }
        public string KYC { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public int ApprovedBy { get; set; }
        public int RoleID { get; set; }
        public string Role { get; set; }
        public int UserTypeID { get; set; }
        public int UserContactInfoID { get; set; }
        [Required]
        public int StateID { get; set; }
        public int LgaID { get; set; }
        public string CompanyName { get; set; }
        public string MerchantCode { get; set; }
        [Required]
        [StringLength(11)]
        public string BVN { get; set; }
        [Required]
        //Please do not delete the Date DataType method, it is functioning and useful but if you insist to delete, please contact Naasiruddeen or Ehis
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }
        //public int RoleID { get; set; }


        //--------Corporate Account ---

        public string rcNumber { get; set; }
        public string companyName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public DateTime incorporationDate { get; set; }
       // public string bvn { get; set; }


    }

    public class CorporateOnboardingRequest
    {
        public string username { get; set; }
        public string walletName { get; set; } 
        public string webhookUrl { get; set; }
        public string shortName { get; set; }
        public string implementation { get; set; }

    }

    public class CorporateRequest
    {
        public string rcNumber { get; set; }
        public string companyName { get; set; }
      
        public string incorporationDate { get; set; }
         public string BVN { get; set; }
    }


    public class UpdateUserViewModel {

        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int AccountTypeID { get; set; }
        public string AccountType { get; set; }
        public string Gender { get; set; }
        public string LGA{ get; set; }
        public string KYCInfo { get; set; }
        public int StateID { get; set; }
        public int AggregatorID { get; set; }
        public int MerchantID { get; set; }
        public string Aggregator { get; set; }
        public int TransactionLimitID { get; set; }
        public string TransactionLimit { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string DOB { get; set; }
        public string Image { get; set; }
        public string AccountNo { get; set; }
        public string CompanyName { get; set; }
        public string MerchantCode { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public string BVN { get; set; }

    }

    public class SystemUserViewModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int AccounTypeID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string AccountNo { get; set; }
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
        public string CompanyName { get; set; }
        public int KYCID { get; set; }
        public int AgreggatorID { get; set; }
        public bool IsActive { get; set; }
    }

   
    public class HoldSystemUserViewModel
    {
        public IEnumerable<SystemUserViewModel> HoldAllSystemUsers { get; set; }
        public Pager Pager { get; set; }
    }

    public class HoldSysUserViewModel
    {
        public IEnumerable<SysUserViewModel> HoldAllSysUsers { get; set; }
        public Pager Pager { get; set; }
    }

    public class DropDownModelView
    {
        public List<DDListView> items { get; set; }
    }

    public class DDListView
    {
        public string itemName { get; set; }
        public int itemValue { get; set; }
        public string strItemValue { get; set; }
    }



    public class RDropDownModelView
    {
        public List<RDDListView> items { get; set; }
    }

    public class RDDListView
    {
        public string itemName { get; set; }
        public string itemValue { get; set; }
    }




    public class VFDService
    {
        public string Url { get; set; }
        public string devUrl { get; set; }
        public bool IsEnabled { get; set; }
        public string token { get; set; }
        public string walletCredentials { get; set; }
    }



    public class VFDOnboardingResponse
    {
        public string status { get; set; }
        public string message { get; set; }
    }







}
