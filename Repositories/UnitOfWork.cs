//using PaymentGateway.Data;
using Microsoft.AspNetCore.Identity;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using PaymentGateway21052021.Repositories.Reports;
using PaymentGateway21052021.Repositories.Reports.Interfaces;
using PaymentGateway21052021.Repositories.SysSetup;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private ApplicationDBContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        #region "Lock"
        private static readonly object _Instancelock = new object();
        #endregion

        #region "Interfaces"

        IAggregatorRepository _Aggregator;
        IBankRepository _Bank;
        IDocumentRepository _Document;
        IMerchantTypeRepository _MerchantType;
        IMerchantMappingRepository _MerchantMapping;
        IAgentMappingRepository _AgentMapping;
        ITitleRepository _Title;
        IAccountInfoRepository _AccountInfo;
        IAccountTypeRepository _AccountType;
        IUserTypeRepository _UserType;
        IProductCategoryRepository _ProductCategory;
        IProductsRepository _Products;
        IProductItemRepository _ProductItem;
        IProductItemRateRepository _ProductItemRate;
        IRoleRepository _Role;
        IStateRepository _State;
        IUserKYCInfoRepository _UserKYCInfo;
        IUserContactInfoRepository _UserContactInfo;
        ILGARepository _LGA;
        ITransactionTypeRepository _TransactionType;
        IUserRepository _User;
        ISysUserRepository _CreateUser;
        IWalletRepository _Wallet;
        IWalletTransactionRepository _WalletTransaction;
        ISettlementBasisRepository _SettlementBasis;
        ISettlementLogRepository _SettlementLog;
        ISettlementHistoryRepository settlementHistory;
        ISettlementIntervalRepository _SettlementInterval;
        ISettlementSummaryRepository _SettlementSummary;
        ITransactionLimitRepository _TransactionLimit;
        IIdentificationTypeRepository _IdentificationType;
        ICompanyRepository _Company;
        IGLAccountRepository _GLAccount;
        IInvoiceRepository _Invoice;
        IStoreTransactionRepository _StoreTransaction;
        ISalesRepository _Sales;
        IMerchantRepository _Merchant;
        ISettlementTypeRepository _SettlementType;
        private object _SettlementHistory;
        ITransactionMethodRepository _TransactionMethod;
        IInvoiceModeRepository _InvoiceMode;
        IAgentReportRepository _AgentReport;
        IAggregatorReportRepository _AggregatorReport;
        IMerchantReportRepository _MerchantReport;
        IWalletHistoryRepository _WalletHistory;
        IRequestResponseLogRepository _RequestResponseLog;
        ISettlementModeRepository _SettlementMode;
        IAggregatorRequestRepository _AggregatorRequest;
        IUpgradeAccountRepository _UpgradeAccount;
        IDocumentValueRepository _DocumentValue;
        IAggregatorCommissionRepository _AggregatorCommission;
        IEgoleWalletTransactionsRepository _EgoleWalletTransactions;
        IUserTokenRepository _UserToken;
        IAuthFactorRepository _AuthFactor;
        IBeneficiaryRepository _Beneficiary;
        ITokenValidationRepository _TokenValidation;
        IAuditTrailRepository _AuditTrail;
        ITicketDetailsRepository _TicketDetails;
        ITicketRepository _Ticket;
        IInsuranceRemittanceRepository _InsuranceRemittance;
        IAggregateSumRepository _AggregateSum;
        IAccountManagersRepository _AccountManagers;
        IBettingProvidersRepository _BettingProviders;
        IBankBvnsRepository _BankBvns;
        IBettingUsersRepository _BettingUsers;
        ICabletvusersRepository _Cabletvusers;
        IEnergymeterusersRepository _Energymeterusers;
        IIswpricebandsRepository _Iswpricebands;
        IPinpadsRepository _Pinpads;
        IUssdTransactionsRepository _UssdTransactions;
        IWebhooklogsRepository _Webhooklogs;

        #endregion

        public UnitOfWork(SignInManager<ApplicationUser> signInManager, ApplicationDBContext context)
        {
            _context = context;
            _signInManager = signInManager;
        }


        public IDocumentRepository Document

        {
            get
            {
                if (_Document == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Document == null)
                            _Document = new DocumentRepository(_context);
                    }
                }

                return _Document;
            }
        }

        public IInvoiceModeRepository InvoiceMode
        {
            get
            {
                if (_InvoiceMode == null)
                {
                    lock (_Instancelock)
                    {
                        if (_InvoiceMode == null)
                            _InvoiceMode = new InvoiceModeRepository(_context);
                    }
                }

                return _InvoiceMode;
            }
        }

        public IWalletRepository Wallet
        {
            get
            {
                if (_Wallet == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Wallet == null)
                            _Wallet = new WalletRepository(_context);
                    }
                }

                return _Wallet;
            }
        }

        public IMerchantRepository Merchant
        {
            get
            {
                if (_Merchant == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Merchant == null)
                            _Merchant = new MerchantRepository(_context);
                    }
                }

                return _Merchant;
            }
        }

        public ISalesRepository Sales
        {
            get
            {
                if (_Sales == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Sales == null)
                            _Sales = new SalesRepository(_context);
                    }
                }

                return _Sales;
            }
        }


        public IWalletTransactionRepository WalletTransaction
        {
            get
            {
                if (_WalletTransaction == null)
                {
                    lock (_Instancelock)
                    {
                        if (_WalletTransaction == null)
                            _WalletTransaction = new WalletTransactionRepository(_context);
                    }
                }

                return _WalletTransaction;
            }
        }

        public ISettlementBasisRepository SettlementBasis
        {
            get
            {
                if (_SettlementBasis == null)
                {
                    lock (_Instancelock)
                    {
                        if (_SettlementBasis == null)
                            _SettlementBasis = new SettlementBasisRepository(_context);
                    }
                }

                return _SettlementBasis;
            }
        }
        public ISettlementTypeRepository SettlementType
        {
            get
            {
                if (_SettlementType == null)
                {
                    lock (_Instancelock)
                    {
                        if (_SettlementType == null)
                            _SettlementType = new SettlementTypeRepository(_context);
                    }
                }

                return _SettlementType;
            }
        }
        public IInvoiceRepository Invoice
        {
            get
            {
                if (_Invoice == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Invoice == null)
                            _Invoice = new InvoiceRepository(_context);
                    }
                }

                return _Invoice;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_User == null)
                {
                    lock (_Instancelock)
                    {
                        if (_User == null)
                            _User = new UserRepository(_signInManager,_context);
                    }
                }

                return _User;
            }
        }

        public ISysUserRepository CreateUser
        {
            get
            {
                if (_CreateUser == null)
                {
                    lock (_Instancelock)
                    {
                        if (_CreateUser == null)
                            _CreateUser = new SysUserRepository(_context);
                    }
                }

                return _CreateUser;
            }
        }
        public IMerchantTypeRepository MerchantType
        {
            get
            {
                if (_MerchantType == null)
                {
                    lock (_Instancelock)
                    {
                        if (_MerchantType == null)
                            _MerchantType = new MerchantTypeRepository(_context);
                    }
                }

                return _MerchantType;
            }
        }
        public IMerchantMappingRepository MerchantMapping
        {
            get
            {
                if (_MerchantMapping == null)
                {
                    lock (_Instancelock)
                    {
                        if (_MerchantMapping == null)
                            _MerchantMapping = new MerchantMappingRepository(_context);
                    }
                }

                return _MerchantMapping;
            }
        }

        public IAgentMappingRepository AgentMapping
        {
            get
            {
                if (_AgentMapping == null)
                {
                    lock (_Instancelock)
                    {
                        if (_AgentMapping == null)
                            _AgentMapping = new AgentMappingRepository(_context);
                    }
                }

                return _AgentMapping;
            }
        }

        public IRoleRepository Role
        {
            get
            {
                if (_Role == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Role == null)
                            _Role = new RoleRepository(_context);
                    }
                }

                return _Role;
            }
        }

        public IStateRepository State
        {
            get
            {
                if (_State == null)
                {
                    lock (_Instancelock)
                    {
                        if (_State == null)
                            _State = new StateRepository(_context);
                    }
                }

                return _State;
            }
        }

        public IUserKYCInfoRepository UserKYCInfo
        {
            get
            {
                if (_UserKYCInfo == null)
                {
                    lock (_Instancelock)
                    {
                        if (_UserKYCInfo == null)
                            _UserKYCInfo = new UserKYCInfoRepository(_context);
                    }
                }

                return _UserKYCInfo;
            }
        }

        public IUserContactInfoRepository UserContactInfo
        {
            get
            {
                if (_UserContactInfo == null)
                {
                    lock (_Instancelock)
                    {
                        if (_UserContactInfo == null)
                            _UserContactInfo = new UserContactInfoRepository(_context);
                    }
                }

                return _UserContactInfo;
            }
        }


        public ITitleRepository Title
        {
            get
            {
                if (_Title == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Title == null)
                            _Title = new TitleRepository(_context);
                    }
                }

                return _Title;
            }
        }

        public IAccountTypeRepository AccountType

        {
            get
            {
                if (_AccountType == null)
                {
                    lock (_Instancelock)
                    {
                        if (_AccountType == null)
                            _AccountType = new AccountTypeRepository(_context);
                    }
                }

                return _AccountType;
            }
        }

        public IAccountInfoRepository AccountInfo

        {
            get
            {
                if (_AccountInfo == null)
                {
                    lock (_Instancelock)
                    {
                        if (_AccountInfo == null)
                            _AccountInfo = new AccountInfoRepository(_context);
                    }
                }

                return _AccountInfo;
            }
        }

        public IUserTypeRepository UserType
        {
            get
            {
                if (_UserType == null)
                {
                    lock (_Instancelock)
                    {
                        if (_UserType == null)
                            _UserType = new UserTypeRepository(_context);
                    }
                }

                return _UserType;
            }
        }

        public IProductsRepository Products
        {
            get
            {
                if (_Products == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Products == null)
                            _Products = new ProductsRepository(_context);
                    }
                }

                return _Products;
            }
        }
        public ITransactionTypeRepository TransactionType
        {
            get
            {
                if (_TransactionType == null)
                {
                    lock (_Instancelock)
                    {
                        if (_TransactionType == null)
                            _TransactionType = new TransactionTypeRepository(_context);
                    }
                }

                return _TransactionType;
            }
        }
        public IProductCategoryRepository ProductCategory
        {
            get
            {
                if (_ProductCategory == null)
                {
                    lock (_Instancelock)
                    {
                        if (_ProductCategory == null)
                            _ProductCategory = new ProductCategoryRepository(_context);
                    }
                }

                return _ProductCategory;
            }
        }

        public IProductItemRepository ProductItem
        {
            get
            {
                if (_ProductItem == null)
                {
                    lock (_Instancelock)
                    {
                        if (_ProductItem == null)
                            _ProductItem = new ProductItemRepository(_context);
                    }
                }

                return _ProductItem;
            }
        }

        public IProductItemRateRepository ProductItemRate
        {
            get
            {
                if (_ProductItemRate == null)
                {
                    lock (_Instancelock)
                    {
                        if (_ProductItemRate == null)
                            _ProductItemRate = new ProductItemRateRepository(_context);
                    }
                }

                return _ProductItemRate;
            }
        }

        public IBankRepository Bank

        {
            get
            {
                if (_Bank == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Bank == null)
                            _Bank = new BankRepository(_context);
                    }
                }

                return _Bank;
            }
        }
        
        public IAggregatorRepository Aggregator 
        {
            get
            {
                if (_Aggregator == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Aggregator == null)
                            _Aggregator = new AggregatorRepository(_context);
                    }
                }

                return _Aggregator;
            }
        }

        public IUpgradeAccountRepository UpgradeAccount  
        {
            get
            {
                if (_UpgradeAccount == null)
                {
                    lock (_Instancelock)
                    {
                        if (_UpgradeAccount == null)
                            _UpgradeAccount = new UpgradeAccountRepository(_context);
                    }
                } 
                return _UpgradeAccount;
            }
        }

        public IDocumentValueRepository DocumentValue   
        {
            get
            {
                if (_DocumentValue == null)
                {
                    lock (_Instancelock)
                    {
                        if (_DocumentValue == null)
                            _DocumentValue = new DocumentValueRepository(_context);
                    }
                }
                return _DocumentValue;
            }
        }

        public ILGARepository LGA 
        {
            get
            {
                if (_LGA == null)
                {
                    lock (_Instancelock)
                    {
                        if (_LGA == null)
                            _LGA = new LGARepository(_context);
                    }
                } 
                return _LGA;
            }
        }

        public ITransactionLimitRepository TransactionLimit

        {
            get
            {
                if (_TransactionLimit == null)
                {
                    lock (_Instancelock)
                    {
                        if (_TransactionLimit == null)
                            _TransactionLimit = new TransactionLimitRepository(_context);
                    }
                }

                return _TransactionLimit;
            }
        }

        public IIdentificationTypeRepository IdentificationType

        {
            get
            {
                if (_IdentificationType == null)
                {
                    lock (_Instancelock)
                    {
                        if (_IdentificationType == null)
                            _IdentificationType = new IdentificationTypeRepository(_context);
                    }
                }

                return _IdentificationType;
            }
        }

        public ICompanyRepository Company

        {
            get
            {
                if (_Company == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Company == null)
                            _Company = new CompanyRepository(_context);
                    }
                }

                return _Company;
            }
        }

        public IGLAccountRepository GLAccount

        {
            get
            {
                if (_GLAccount == null)
                {
                    lock (_Instancelock)
                    {
                        if (_GLAccount == null)
                            _GLAccount = new GLAccountRepository(_context);
                    }
                }

                return _GLAccount;
            }
        }

        public IStoreTransactionRepository StoreTransaction

        {
            get
            {
                if (_StoreTransaction == null)
                {
                    lock (_Instancelock)
                    {
                        if (_StoreTransaction == null)
                            _StoreTransaction = new StoreTransactionRepository(_context);
                    }
                }

                return _StoreTransaction;
            }
        }

        public ISettlementLogRepository SettlementLog
        {
            get
            {
                if (_SettlementLog == null)
                {
                    lock (_Instancelock)
                    {
                        if (_SettlementLog == null)
                            _SettlementLog = new SettlementLogRepository(_context);
                    }
                }

                return _SettlementLog;
            }
        }

        public ISettlementModeRepository SettlementMode
        {
            get
            {
                if (_SettlementMode == null)
                {
                    lock (_Instancelock)
                    {
                        if (_SettlementMode == null)
                            _SettlementMode = new SettlementModeRepository(_context);
                    }
                }

                return (ISettlementModeRepository)_SettlementMode;
            }
        }

        public ISettlementSummaryRepository SettlementSummary
        {
            get
            {
                if (_SettlementSummary == null)
                {
                    lock (_Instancelock)
                    {
                        if (_SettlementSummary == null)
                            _SettlementSummary = new SettlementSummaryRepository(_context);
                    }
                }

                return _SettlementSummary;
            }
        }


        public ISettlementIntervalRepository SettlementInterval
        {
            get
            {
                if (_SettlementInterval == null)
                {
                    lock (_Instancelock)
                    {
                        if (_SettlementInterval == null)
                            _SettlementInterval = new SettlementIntervalRepository(_context);
                    }
                }

                return _SettlementInterval;
            }
        }

        public ISettlementHistoryRepository SettlementHistory
        {
            get
            {
                if (_SettlementHistory == null)
                {
                    lock (_Instancelock)
                    {
                        if (_SettlementHistory == null)
                            _SettlementHistory = new SettlementHistoryRepository(_context);
                    }
                }

                return (ISettlementHistoryRepository)_SettlementHistory;
            }
        }

        public ITransactionMethodRepository TransactionMethod
        {
            get
            {
                if (_TransactionMethod == null)
                {
                    lock (_Instancelock)
                    {
                        if (_TransactionMethod == null)
                            _TransactionMethod = new TransactionMethodRepository(_context);
                    }
                }

                return _TransactionMethod;
            }
        }

        public IAgentReportRepository AgentReport
        {
            get
            {
                if (_AgentReport == null)
                {
                    lock (_Instancelock)
                    {
                        if (_AgentReport == null)
                            _AgentReport = new AgentReportRepository(_context);
                    }
                }

                return (IAgentReportRepository)_AgentReport;
            }
        }

        public IAggregatorReportRepository AggregatorReport
        {
            get
            {
                if (_AggregatorReport == null)
                {
                    lock (_Instancelock)
                    {
                        if (_AggregatorReport == null)
                            _AggregatorReport = new AggregatorReportRepository(_context);
                    }
                }

                return (IAggregatorReportRepository)_AggregatorReport;
            }
        }


        public IMerchantReportRepository MerchantReport
        {
            get
            {
                if (_MerchantReport == null)
                {
                    lock (_Instancelock)
                    {
                        if (_MerchantReport == null)
                            _MerchantReport = new MerchantReportRepository(_context);
                    }
                }

                return (IMerchantReportRepository)_MerchantReport;
            }
        }

        public IWalletHistoryRepository WalletHistory
        {
            get
            {
                if (_WalletHistory == null)
                {
                    lock (_Instancelock)
                    {
                        if (_WalletHistory == null)
                            _WalletHistory = new WalletHistoryRepository(_context);
                    }
                }

                return (IWalletHistoryRepository)_WalletHistory;
            }
        }

        public IRequestResponseLogRepository RequestResponseLog
        {
            get
            {
                if (_RequestResponseLog == null)
                {
                    lock (_Instancelock)
                    {
                        if (_RequestResponseLog == null)
                            _RequestResponseLog = new RequestResponseLogRepository(_context);
                    }
                }

                return (IRequestResponseLogRepository)_RequestResponseLog;
            }
        }

        public IAggregatorRequestRepository AggregatorRequest
        {
            get
            {
                if (_AggregatorRequest == null)
                {
                    lock (_Instancelock)
                    {
                        if (_AggregatorRequest == null)
                            _AggregatorRequest = new AggregatorRequestRepository(_context);
                    }
                }

                return _AggregatorRequest;
            }
        }

        public IAggregatorCommissionRepository AggregatorCommission
        {
            get
            {
                if (_AggregatorCommission == null)
                {
                    lock (_Instancelock)
                    {
                        if (_AggregatorCommission == null)
                            _AggregatorCommission = new AggregatorCommissionRepository(_context);
                    }
                }

                return _AggregatorCommission;
            }
        }

        public IEgoleWalletTransactionsRepository EgoleWalletTransactions
        {
            get
            {
                if (_EgoleWalletTransactions == null)
                {
                    lock (_Instancelock)
                    {
                        if (_EgoleWalletTransactions == null)
                            _EgoleWalletTransactions = new EgoleWalletTransactionsRepository(_context);
                    }
                }

                return _EgoleWalletTransactions;
            }
        }

        public IUserTokenRepository UserToken
        {
            get
            {
                if (_UserToken == null)
                {
                    lock (_Instancelock)
                    {
                        if (_UserToken == null)
                            _UserToken = new UserTokenRepository(_context);
                    }
                }

                return _UserToken;
            }
        }

        public IAuthFactorRepository AuthFactor
        {
            get
            {
                if (_AuthFactor == null)
                {
                    lock (_Instancelock)
                    {
                        if (_AuthFactor == null)
                            _AuthFactor = new AuthFactorRepository(_context);
                    }
                }

                return _AuthFactor;
            }
        }

        public IBeneficiaryRepository Beneficiary
        {
            get
            {
                if ( _Beneficiary == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Beneficiary == null)
                            _Beneficiary = new BeneficiaryRepository(_context);
                    }
                }

                return _Beneficiary;
            }
        }

        public ITokenValidationRepository TokenValidation
        {
            get
            {
                if (_TokenValidation == null)
                {
                    lock (_Instancelock)
                    {
                        if (_TokenValidation == null)
                            _TokenValidation = new TokenValidationRepository(_context);
                    }
                }

                return _TokenValidation;
            }
        }

        public IAuditTrailRepository AuditTrail
        {
            get
            {
                if (_AuditTrail == null)
                {
                    lock (_Instancelock)
                    {
                        if (_AuditTrail == null)
                            _AuditTrail = new AuditTrailRepository(_context);
                    }
                }

                return _AuditTrail;
            }
        }


        public ITicketDetailsRepository TicketDetails
        {
            get
            {
                if (_TicketDetails == null)
                {
                    lock (_Instancelock)
                    {
                        if (_TicketDetails == null)
                            _TicketDetails = new TicketDetailsRepository(_context);
                    }
                }

                return _TicketDetails;
            }
        }

        public ITicketRepository Ticket
        {
            get
            {
                if (_Ticket == null)
                {
                    lock (_Instancelock)
                    {
                        if (_Ticket == null)
                            _Ticket = new TicketRepository(_context);
                    }
                }

                return _Ticket;
            }
        }


        public IInsuranceRemittanceRepository InsuranceRemittance
        {
            get
            {
                if (_InsuranceRemittance == null)
                {
                    lock (_Instancelock)
                    {
                        if (_InsuranceRemittance == null)
                            _InsuranceRemittance = new InsuranceRemittanceRepository(_context);
                    }
                }

                return _InsuranceRemittance;
            }
        }


        public IAggregateSumRepository AggregateSum
        {
            get
            {
                if (_AggregateSum == null)
                {
                    lock (_Instancelock)
                    {
                        if (_AggregateSum == null)
                            _AggregateSum = new AggregateSumRepository(_context);
                    }
                }

                return _AggregateSum;
            }
        }

        public IAccountManagersRepository AccountManagers
        {
            get
            {
                if (AccountManagers == null)
                {
                    lock (_Instancelock)
                    { 
                        if (_AccountManagers == null)
                            _AccountManagers = new AccountManagersRepository(_context);
                    }
                }

                return AccountManagers;
            }
        }

        public IBettingProvidersRepository BettingProviders
        {
            get
            {
                if (BettingProviders == null)
                {
                    lock (_Instancelock)
                    {
                        if (_BettingProviders == null)
                            _BettingProviders = new BettingProvidersRepository(_context);
                    }
                }

                return BettingProviders;
            }
        }

        public IBankBvnsRepository BankBvns
        {
            get
            {
                lock (_Instancelock)
                {
                    if (_BankBvns == null)
                        _BankBvns = new BankBvnsRepository(_context);
                }

                return BankBvns;
            }
        }

        public IBettingUsersRepository BettingUsers
        {
            get
            {
                lock (_Instancelock)
                {
                    if (_BettingUsers == null)
                        _BettingUsers = new BettingUsersRepository(_context);
                }

                return BettingUsers;
            }
        }

        public ICabletvusersRepository Cabletvusers
        {
            get
            {
                lock (_Instancelock)
                {
                    if (_Cabletvusers == null)
                        _Cabletvusers = new CabletvusersRepository(_context);
                }

                return Cabletvusers;
            }
        }

        public IEnergymeterusersRepository Energymeterusers
        {
            get
            {
                lock (_Instancelock)
                {
                    if (_Energymeterusers == null)
                        _Energymeterusers = new EnergymeterusersRepository(_context);
                }

                return Energymeterusers;
            }
        }

        public IIswpricebandsRepository Iswpricebands
        {
            get
            {
                lock (_Instancelock)
                {
                    if (_Iswpricebands == null)
                        _Iswpricebands = new IswpricebandsRepository(_context);
                }

                return Iswpricebands;
            }
        }

        public IPinpadsRepository Pinpads
        {
            get
            {
                lock (_Instancelock)
                {
                    if (_Pinpads == null)
                        _Pinpads = new PinpadsRepository(_context);
                }

                return Pinpads;
            }
        }

        public IUssdTransactionsRepository UssdTransactions
        {
            get
            {
                lock (_Instancelock)
                {
                    if (_UssdTransactions == null)
                        _UssdTransactions = new UssdTransactionsRepository(_context);
                }

                return UssdTransactions;
            }
        }

        public IWebhooklogsRepository Webhooklogs
        {
            get
            {
                lock (_Instancelock)
                {
                    if (_Webhooklogs == null)
                        _Webhooklogs = new WebhooklogsRepository(_context);
                }

                return Webhooklogs;
            }
        }



        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }


}
