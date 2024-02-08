using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using PaymentGateway21052021.Repositories.Reports.Interfaces;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories
{
    public interface IUnitOfWork: IDisposable
    { 
        ITitleRepository Title { get; }
        IAccountTypeRepository AccountType { get; }
        IAccountInfoRepository AccountInfo { get; }
        IMerchantRepository Merchant { get; }
        IMerchantTypeRepository MerchantType { get; }
        IMerchantMappingRepository MerchantMapping { get; }
        IAgentMappingRepository AgentMapping { get; }

        IProductsRepository Products { get; }
        IProductCategoryRepository ProductCategory { get; }
        IProductItemRepository ProductItem { get; }
        IProductItemRateRepository ProductItemRate { get; }
        IUserTypeRepository UserType { get; }
        IRoleRepository Role { get; }
        IStateRepository State { get;  }
        IUserKYCInfoRepository UserKYCInfo { get; }
        IUserContactInfoRepository UserContactInfo { get; }
        IAggregatorRepository Aggregator { get; }
        IBankRepository Bank { get; }
        IDocumentRepository Document { get; }
        ILGARepository LGA { get; }
        ITransactionTypeRepository TransactionType { get;}
        ITransactionMethodRepository TransactionMethod { get; }
        IUserRepository User { get; }
        ISysUserRepository CreateUser { get; }
        IWalletRepository Wallet { get;  }
        IWalletTransactionRepository WalletTransaction { get; }
        ISettlementBasisRepository SettlementBasis { get; }
        ISettlementTypeRepository SettlementType { get; }
        ISettlementHistoryRepository SettlementHistory { get; }
        ISettlementLogRepository SettlementLog { get; }
        ISettlementIntervalRepository SettlementInterval { get; }
        ISettlementSummaryRepository SettlementSummary { get; }
        ITransactionLimitRepository TransactionLimit { get; }
        IIdentificationTypeRepository IdentificationType { get; }
        ICompanyRepository Company { get;  }
        IGLAccountRepository GLAccount { get; }
        IInvoiceRepository Invoice { get; }
        IStoreTransactionRepository StoreTransaction { get; }
        ISalesRepository Sales { get; }
        IInvoiceModeRepository InvoiceMode { get; }
        IAgentReportRepository AgentReport { get; }
        IAggregatorReportRepository AggregatorReport { get; }
        IMerchantReportRepository MerchantReport { get; }
        IWalletHistoryRepository WalletHistory { get; }
        IRequestResponseLogRepository RequestResponseLog { get; }
        ISettlementModeRepository SettlementMode { get; }
        IUpgradeAccountRepository UpgradeAccount { get; }
        IAggregatorRequestRepository AggregatorRequest { get; }
        IDocumentValueRepository DocumentValue { get; }
        IAggregatorCommissionRepository AggregatorCommission { get; }

        IEgoleWalletTransactionsRepository EgoleWalletTransactions { get; }
        IUserTokenRepository UserToken { get; }
        IAuthFactorRepository AuthFactor { get; }

        IBeneficiaryRepository Beneficiary { get; }
        ITokenValidationRepository TokenValidation { get; }
        IAuditTrailRepository AuditTrail { get; }

        ITicketDetailsRepository TicketDetails { get; }
        ITicketRepository Ticket { get; }

        IInsuranceRemittanceRepository InsuranceRemittance { get; }
        IAggregateSumRepository AggregateSum { get; }
        IAccountManagersRepository AccountManagers { get; }
        IBettingProvidersRepository BettingProviders { get; }
        IBankBvnsRepository BankBvns { get; }   
        IBettingUsersRepository BettingUsers { get; }   
        ICabletvusersRepository Cabletvusers { get; }
        IEnergymeterusersRepository Energymeterusers { get; }   
        IIswpricebandsRepository Iswpricebands { get; }
        IPinpadsRepository Pinpads { get; } 
        IUssdTransactionsRepository UssdTransactions { get; }   
        IWebhooklogsRepository Webhooklogs { get; }

        int Complete();
        
    }

}
