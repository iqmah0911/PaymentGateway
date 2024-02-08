using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Models;
//using PaymentGateway.Models;

namespace PaymentGateway21052021.Data
{
    //public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(150000);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=102.37.109.121;Database=PaymentGateway;User Id=apps; Password=Egole@123;Trusted_Connection=False;MultipleActiveResultSets=true;Connection Lifetime=30; Max Pool Size=350;Connection Timeout=30;Connection Lifetime=0;ConnectRetryCount=3;ConnectRetryInterval=10;",
                 builder=>builder.EnableRetryOnFailure() );
            }
             
        }

        //DB Sets should go in here for Migration Purpose
          #region "DB SETS"
        public virtual DbSet<SysUsers> SYS_Users { get; set; }
        public virtual DbSet<SysRole> SYS_Role { get; set; }
        public virtual DbSet<SysAggregator> SYS_Aggregator { get; set; }
        public virtual DbSet<SysBank> SYS_Bank { get; set; }
        public virtual DbSet<SysMerchantMapping> SYS_MerchantMapping { get; set; }
        public virtual DbSet<SysUserType> SYS_UserType { get; set; }
        public virtual DbSet<SysUserKycInfo> SYS_UserKYCInfo { get; set; }
        public virtual DbSet<SysUserContactInfo> SYS_UserContactInfo { get; set; }
        public virtual DbSet<SysTransactionType> SYS_TransactionType { get; set; }
        public virtual DbSet<SysTitle> SYS_Title { get; set; }
        public virtual DbSet<SysState> SYS_States { get; set; }
        public virtual DbSet<SysLga> SYS_LGA { get; set; }
        public virtual DbSet<SysDocument> SYS_Document { get; set; }
        public virtual DbSet<EgsDocumentValue> EGS_DocumentValue { get; set; }
        public virtual DbSet<EgsAccountsInfo> EGS_AccountInfo { get; set; }
        public virtual DbSet<EgsAccountType> EGS_AccountType { get; set; }
        public virtual DbSet<EgsMerchantType> EGS_MerchantType { get; set; }
        public virtual DbSet<EgsProduct> EGS_Products { get; set; }
        public virtual DbSet<EgsProductCategory> EGS_ProductCategories { get; set; }
        public virtual DbSet<EgsProductItem> EGS_ProductItem { get; set; }
        public virtual DbSet<EgsProductItemRate> EGS_ProductItemRate { get; set; }
        public virtual DbSet<EgsSales> EGS_Sales { get; set; }
        public virtual DbSet<EgsSettlementBasis> EGS_SettlementBasis { get; set; }
        public virtual DbSet<EgsSettlementHistory> EGS_SettlementHistory { get; set; }
        public virtual DbSet<EgsSettlementMode> EGS_SettlementMode { get; set; }
        public virtual DbSet<EgsSettlementInterval> EGS_SettlementInterval { get; set; }
        public virtual DbSet<EgsSettlementType> EGS_SettlementType { get; set; }
        public virtual DbSet<EgsTransactionMethod> EGS_TransactionMethod { get; set; }
        public virtual DbSet<EgsWallet> EGS_Wallet { get; set; }
        public virtual DbSet<EgsWalletTransaction> EGS_WalletTransaction { get; set; }
        public virtual DbSet<SysTransactionLimit> SYS_TransactionLimits { get; set; }

        public virtual DbSet<SysIdentificationType> SYS_IdentificationType { get; set; }
        public virtual DbSet<EgsSettlementLog> EGS_SettlementLog { get; set; }
        public virtual DbSet<EgsSettlementSummary> EGS_SettlementSummary { get; set; }
        public virtual DbSet<SysRequestResponseLog> SYS_RequestResponseLog{ get; set; }
        public virtual DbSet<SysCompany> SYS_Company { get; set; }
        public virtual DbSet<EgsGLAccount> EGS_GLAccounts { get; set; }
        public virtual DbSet<EgsInvoice> EGS_Invoice { get; set; }
        public virtual DbSet<EgsStoreTransaction> EGS_StoreTransaction { get; set; }
        public virtual DbSet<EgsMerchant> EGS_Merchant { get; set; }
        public virtual DbSet<EgsInvoiceMode> EGS_InvoiceMode { get; set; }
        public virtual DbSet<SysAgentMapping> SYS_AgentMapping { get; set; }
        public virtual DbSet<EgsUpgradeAccount> EGS_UpgradeAccount { get; set; }
        public virtual DbSet<EgsAggregatorRequest> EGS_AggregatorRequest { get; set; }
        public virtual DbSet<EgsAggregatorCommission> EGS_AggregatorCommission { get; set; }
        public virtual DbSet<EgsAgentCommission> EGS_AgentCommission { get; set; } 
        public virtual DbSet<EgsEgoleWalletTransactions> EGS_EgoleWalletTransactions { get; set; }

        public virtual DbSet<EgsAuthFactor> EGS_AuthFactor { get; set; }

        public virtual DbSet<EgsUserToken> EGS_UserToken { get; set; }

        public virtual DbSet<EgsBeneficiary> EGS_Beneficiary { get; set; }

       public virtual DbSet<EgsTokenValidation> EGS_TokenValidation { get; set; }


        public virtual DbSet<EgsPeriodicAgents> EGS_PeriodicAgents { get; set; }

        public virtual DbSet<EgsPeriodicTransactionReport> EGS_PeriodicTransactionReport { get; set; }
        public virtual DbSet<EgsAuditTrail> EGS_AuditTrail { get; set; }

        public virtual DbSet<EgsVendor> EGS_Vendor { get; set; }


        public virtual DbSet<EgsTickets> EGS_Tickets { get; set; }

        public virtual DbSet<EgsTicketDetails> EGS_TicketDetails { get; set; }
         
        public virtual DbSet<FundWallet> FundWallets { get; set; }

        public virtual DbSet<EgsAggregateSum> EGS_AggregateSum { get; set; }

        public virtual DbSet<EgsInsuranceRemittance> EGS_InsuranceRemittance { get; set; }

        public virtual DbSet<EgsAccountLookup> EGS_AccountLookup { get; set; }


        public virtual DbSet<EgsTerminal> EGS_Terminal { get; set; }

        public virtual DbSet<SysBillers> SYS_Billers { get; set; }

        public virtual DbSet<EgsUserDevice> EGS_UserDevice { get; set; }

        public virtual DbSet<SysServiceNotification> SYS_ServiceNotification { get; set; }

        public virtual DbSet<EgsUserNotification> EGS_UserNotification { get; set; }
         
        public virtual DbSet<EgsClient> EGS_Client { get; set; }

        public virtual DbSet<EgsServiceType> EGS_ServiceType { get; set; }

        public virtual DbSet<EgsCardHolder> EGS_CardHolder { get; set; }

        public virtual DbSet<EgsChannels> EGS_Channels { get; set; }


        public virtual DbSet<EgsTerminalTracking> EGS_TerminalTracking { get; set; }

        public virtual DbSet<EgsVASProviderChannels> EGS_VASProviderChannels { get; set; }
        //-----------------
        public virtual DbSet<EgsAccountManagers> EGS_AccountManagers { get; set; }
        public virtual DbSet<EgsBettingProviders> EGS_BettingProviders { get; set; }
        public virtual DbSet<EgsBankBvns> EGS_BankBvns { get; set; }
        public virtual DbSet<EgsBettingUsers> EGS_BettingUsers { get; set; }
        public virtual DbSet<EgsCabletvusers> EGS_cabletvusers { get; set; }    
        public virtual DbSet<EgsEnergymeterusers> EGS_Energymeterusers { get; set; }    
        public virtual DbSet<EgsIswpricebands> EGS_Iswpricebands { get; set; }
        public virtual DbSet<EgsPinpads> EGS_Pinpads { get; set; }
        public virtual DbSet<EgsUssdTransactions> EGS_UssdTransactions { get; set; }
        public virtual DbSet<EgsWebhooklogs> EGS_Webhooklogs { get; set; }  
        public virtual DbSet<EgsAssignAgentTerminal> EGS_AssignAgentTerminal { get; set; }  
        public virtual DbSet<SysAgentType> SYS_AgentType { get; set; }  
        public virtual DbSet<SysAggregatorType> SYS_AggregatorType { get; set; }  
        public virtual DbSet<EgsSubAgentWallet> EGS_SubAgentWallet { get; set; }  
        public virtual DbSet<EgsSubUser> Egs_SubUser { get; set; }  
        public virtual DbSet<EgsSubWallet> Egs_SubWallet { get; set; }  
        public virtual DbSet<EgsDisbursement> EGS_Disbursement { get; set; }  
        public virtual DbSet<EgsPosRequest> EGS_PosType { get; set; }  
        public virtual DbSet<EgsPosRequest> Egs_PosRequest { get; set; }  
        public virtual DbSet<SysTarget> SYS_Target { get; set; }  



        #endregion
    }
}
