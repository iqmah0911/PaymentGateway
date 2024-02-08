using PaymentGateway21052021.Areas.EgsOperations.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Models
{
    public class SysUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string DateOfBirth { get; set; }
        public string BVN { get; set; }
        public string Image { get; set; }

        [ForeignKey("AccountTypeID")]
        public EgsAccountType AccountType { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public bool IsSignUp { get; set; }
        public bool IsVerified { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int CreatedBy { get; set; }
        public int ApprovedBy { get; set; }

        public bool Is_Approved { get; set; }
         
        public int ApprovedBy2 { get; set; }
        public string RCNumber { get; set; }

        [ForeignKey("RoleID")]
        public SysRole Role { get; set; }

        [ForeignKey("UserTypeID")]
        public SysUserType UserType { get; set; }

        //[ForeignKey("UserContactInfoID")]
        public int? UserContactInfoID { get; set; }

        [ForeignKey("StateID")]
        public SysState ResidentialState { get; set; }

        [ForeignKey("LgaID")]
        public SysLga Lga { get; set; }

        [ForeignKey("TitleID")]
        public SysTitle Title { get; set; }


        public int? KYCID { get; set; }

        [ForeignKey("WalletID")]
        public EgsWallet Wallet { get; set; }

        [ForeignKey("TransactionLimitID")]
        public SysTransactionLimit TransactionLimit { get; set; }

        public int? AggregatorID { get; set; }

        //[InverseProperty("Merchant")]
        //public IEnumerable<SysMerchantMapping> Merchants { get; set; }

        [ForeignKey("MerchantID")]
        public EgsMerchant Merchant { get; set;  } 

        [ForeignKey("AggregatorID")]
        public SysAggregator Aggregator { get; set; } 
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }

        public bool IsSpecial { get; set; }

        public string BankAccountCode { get; set; }
        public bool Is_UseFingerprint { get; set; }

        public string TerminalID { get; set; }
         

        [ForeignKey("TerminalKey")]
        public EgsTerminal Terminals { get; set; }

        public string ActivationCode { get; set; }

        public string ActivationStatus { get; set; }

        public string AgentCode { get; set; }

        //public int ParentWalletID { get; set; }

        //public int AgentTypeID { get; set; }

        [ForeignKey("AgentTypeID")]
        public SysAgentType AgentType { get; set; }


        //[InverseProperty("Merchant")]
        //public IEnumerable<EgsProductItem> MerchantProducts { get; set; }

        //[InverseProperty("User")]
        //public IEnumerable<EgsAccountsInfo> AccountsInfos { get; set; }

        //[InverseProperty("User")]
        //public IEnumerable<EgsUpgradeAccount> UpgradeAccounts { get; set; }

        //[InverseProperty("Agent")]
        //public IEnumerable<EgsAggregatorRequest> AggregatorRequests { get; set; }

        //public ICollection<EgsUserDevice> UserDevices { get; set; }

    }
}
