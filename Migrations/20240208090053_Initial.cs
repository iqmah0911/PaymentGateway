using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway21052021.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    TokenExpiration = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EGS_AccountLookup",
                columns: table => new
                {
                    lookupID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountName = table.Column<string>(nullable: true),
                    BVN = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BankCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_AccountLookup", x => x.lookupID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_AccountManagers",
                columns: table => new
                {
                    accountManagerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    firstName = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true),
                    stateID = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    DateDeleted = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_AccountManagers", x => x.accountManagerID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_AgentCommission",
                columns: table => new
                {
                    AgentCommissionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductID = table.Column<int>(nullable: false),
                    ProductItemID = table.Column<int>(nullable: false),
                    SplittingRate = table.Column<int>(nullable: false),
                    SettlementTypeID = table.Column<int>(nullable: false),
                    SettlementIntervalID = table.Column<int>(nullable: false),
                    AgentID = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_AgentCommission", x => x.AgentCommissionID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_AggregateSum",
                columns: table => new
                {
                    AggregateID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AggregatedAmount = table.Column<double>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_AggregateSum", x => x.AggregateID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_AggregatorCommission",
                columns: table => new
                {
                    AggregatorCommissionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductID = table.Column<int>(nullable: false),
                    ProductItemID = table.Column<int>(nullable: false),
                    SplittingRate = table.Column<int>(nullable: false),
                    SettlementTypeID = table.Column<int>(nullable: false),
                    SettlementIntervalID = table.Column<int>(nullable: false),
                    AggregatorID = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    TransactionReferenceNo = table.Column<string>(nullable: true),
                    IsPushed = table.Column<bool>(nullable: false),
                    CommissionAmount = table.Column<double>(nullable: false),
                    DatePushed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_AggregatorCommission", x => x.AggregatorCommissionID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_AssignAgentTerminal",
                columns: table => new
                {
                    AssignTerminalID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TerminalKey = table.Column<int>(nullable: false),
                    AgentID = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    MainWallet = table.Column<string>(nullable: true),
                    SubWalletID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_AssignAgentTerminal", x => x.AssignTerminalID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_AuditTrail",
                columns: table => new
                {
                    AID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DbAction = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Page = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IPAddress = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    Menu = table.Column<string>(nullable: true),
                    DeviceName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_AuditTrail", x => x.AID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_AuthFactor",
                columns: table => new
                {
                    AuthFactorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthType = table.Column<string>(nullable: true),
                    AuthValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_AuthFactor", x => x.AuthFactorID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_BankBvns",
                columns: table => new
                {
                    bankbvnsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    bvn = table.Column<string>(nullable: true),
                    bvndetails = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_BankBvns", x => x.bankbvnsID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_BettingProviders",
                columns: table => new
                {
                    bettingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    customerName = table.Column<string>(nullable: true),
                    vendor = table.Column<string>(nullable: true),
                    chargeType = table.Column<string>(nullable: true),
                    charge = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_BettingProviders", x => x.bettingID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_BettingUsers",
                columns: table => new
                {
                    bettingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    customerID = table.Column<string>(nullable: true),
                    service = table.Column<string>(nullable: true),
                    response = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_BettingUsers", x => x.bettingID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_cabletvusers",
                columns: table => new
                {
                    CableID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    smartcard = table.Column<string>(nullable: true),
                    service = table.Column<string>(nullable: true),
                    response = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_cabletvusers", x => x.CableID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Channels",
                columns: table => new
                {
                    ChannelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Channel = table.Column<string>(nullable: true),
                    status = table.Column<int>(nullable: false),
                    priority = table.Column<int>(nullable: false),
                    upperTransactionBound = table.Column<double>(nullable: false),
                    lowerTransactionBound = table.Column<double>(nullable: false),
                    chargeValue = table.Column<double>(nullable: false),
                    chargeIsFlat = table.Column<int>(nullable: false),
                    cap = table.Column<int>(nullable: false),
                    secretKey = table.Column<string>(nullable: true),
                    EnumChannelID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Channels", x => x.ChannelID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Client",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    WalletCredentials = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    ClientID = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    WalletID = table.Column<int>(nullable: false),
                    Expiration = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Client", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Disbursement",
                columns: table => new
                {
                    DisbursementID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Recipient = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ReferenceNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    Approver = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Disbursement", x => x.DisbursementID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Energymeterusers",
                columns: table => new
                {
                    energymeterID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    meternumber = table.Column<string>(nullable: true),
                    disco = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    response = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Energymeterusers", x => x.energymeterID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_InsuranceRemittance",
                columns: table => new
                {
                    InsuranceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(nullable: true),
                    ProductCategory = table.Column<string>(nullable: true),
                    ProductItem = table.Column<string>(nullable: true),
                    Premuim = table.Column<double>(nullable: false),
                    CBS = table.Column<double>(nullable: false),
                    NIA = table.Column<double>(nullable: false),
                    EGOLEPAY = table.Column<double>(nullable: false),
                    NIAMEMBER = table.Column<double>(nullable: false),
                    BROKER = table.Column<double>(nullable: false),
                    SettlementDate = table.Column<DateTime>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    AccountAlias = table.Column<string>(nullable: true),
                    ReceivingBank = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    TransactionReference = table.Column<string>(nullable: true),
                    BankCode = table.Column<string>(nullable: true),
                    RegNumber = table.Column<string>(nullable: true),
                    PolicyNumber = table.Column<string>(nullable: true),
                    IsTransferred = table.Column<bool>(nullable: false),
                    IsSettled = table.Column<bool>(nullable: false),
                    TransferDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    ECOWAS = table.Column<double>(nullable: false),
                    LAGOS = table.Column<double>(nullable: false),
                    INSURER = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_InsuranceRemittance", x => x.InsuranceID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_InvoiceMode",
                columns: table => new
                {
                    InvoiceModeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InvoiceModeType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_InvoiceMode", x => x.InvoiceModeID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Iswpricebands",
                columns: table => new
                {
                    IswpriceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    band = table.Column<string>(nullable: true),
                    min = table.Column<string>(nullable: true),
                    max = table.Column<string>(nullable: true),
                    charge = table.Column<string>(nullable: true),
                    stampduty = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Iswpricebands", x => x.IswpriceID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_MerchantType",
                columns: table => new
                {
                    MerchantTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MerchantType = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_MerchantType", x => x.MerchantTypeID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_PeriodicAgents",
                columns: table => new
                {
                    AgentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WalletAccountNumber = table.Column<string>(nullable: true),
                    AgentName = table.Column<string>(nullable: true),
                    Period = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_PeriodicAgents", x => x.AgentID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_PeriodicTransactionReport",
                columns: table => new
                {
                    TransID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AgentName = table.Column<string>(nullable: true),
                    WalletAccountNumber = table.Column<string>(nullable: true),
                    Itemcount = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_PeriodicTransactionReport", x => x.TransID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Pinpads",
                columns: table => new
                {
                    pinpadsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    terminalID = table.Column<string>(nullable: true),
                    reference = table.Column<string>(nullable: true),
                    amount = table.Column<double>(nullable: false),
                    currency = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    pan = table.Column<string>(nullable: true),
                    cardScheme = table.Column<string>(nullable: true),
                    customerName = table.Column<string>(nullable: true),
                    statusCode = table.Column<string>(nullable: true),
                    statusDescription = table.Column<string>(nullable: true),
                    rrn = table.Column<string>(nullable: true),
                    paymentDate = table.Column<DateTime>(nullable: false),
                    stan = table.Column<string>(nullable: true),
                    cardExpiry = table.Column<string>(nullable: true),
                    cardhash = table.Column<string>(nullable: true),
                    lat = table.Column<string>(nullable: true),
                    lng = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Pinpads", x => x.pinpadsID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_ProductCategories",
                columns: table => new
                {
                    ProductCategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductCategoryCode = table.Column<string>(nullable: true),
                    ProductCategoryName = table.Column<string>(nullable: true),
                    FormCategoryType = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    MobileIcon = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_ProductCategories", x => x.ProductCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_ServiceType",
                columns: table => new
                {
                    ServiceTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ServiceType = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_ServiceType", x => x.ServiceTypeID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_SettlementBasis",
                columns: table => new
                {
                    SettlementBasisID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductID = table.Column<int>(nullable: false),
                    ProductItemID = table.Column<int>(nullable: false),
                    SplittingRate = table.Column<int>(nullable: false),
                    SettlementTypeID = table.Column<int>(nullable: false),
                    SettlementIntervalID = table.Column<int>(nullable: false),
                    MerchantID = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_SettlementBasis", x => x.SettlementBasisID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_SettlementHistory",
                columns: table => new
                {
                    SettlementHistoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SettlementAmount = table.Column<double>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_SettlementHistory", x => x.SettlementHistoryID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_SettlementInterval",
                columns: table => new
                {
                    SettlementIntervalID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SettlementIntervalName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_SettlementInterval", x => x.SettlementIntervalID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_SettlementMode",
                columns: table => new
                {
                    SettlementModeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SettlementModeName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_SettlementMode", x => x.SettlementModeID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_SettlementType",
                columns: table => new
                {
                    SettlementTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SettlementType = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_SettlementType", x => x.SettlementTypeID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_TerminalTracking",
                columns: table => new
                {
                    TrackingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TerminalID = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    AgentID = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_TerminalTracking", x => x.TrackingID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_TokenValidation",
                columns: table => new
                {
                    TokenID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Provider = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    ValidCreated = table.Column<DateTime>(nullable: false),
                    ValidExpired = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_TokenValidation", x => x.TokenID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_TransactionMethod",
                columns: table => new
                {
                    TransactionMethodID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionMethod = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_TransactionMethod", x => x.TransactionMethodID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_UserDevice",
                columns: table => new
                {
                    UserDeviceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeviceName = table.Column<string>(nullable: true),
                    DeviceType = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true),
                    DateRegistered = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsEmailSent = table.Column<bool>(nullable: false),
                    RequestID = table.Column<string>(nullable: true),
                    DateEmailSent = table.Column<DateTime>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: false),
                    TerminalID = table.Column<string>(nullable: true),
                    DateApproved = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_UserDevice", x => x.UserDeviceId);
                });

            migrationBuilder.CreateTable(
                name: "EGS_UserNotification",
                columns: table => new
                {
                    UserNotificationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Service = table.Column<string>(nullable: true),
                    IsDeposit = table.Column<bool>(nullable: false),
                    IsWithdrawal = table.Column<bool>(nullable: false),
                    IsCommission = table.Column<bool>(nullable: false),
                    IsMoved = table.Column<bool>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_UserNotification", x => x.UserNotificationID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_UserToken",
                columns: table => new
                {
                    UserTokenID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: false),
                    Pin = table.Column<string>(nullable: true),
                    AuthFactor = table.Column<int>(nullable: false),
                    MainWallet = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_UserToken", x => x.UserTokenID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_UssdTransactions",
                columns: table => new
                {
                    ussdTransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    vendor = table.Column<string>(nullable: true),
                    amount = table.Column<double>(nullable: false),
                    reference = table.Column<string>(nullable: true),
                    providerReference = table.Column<string>(nullable: true),
                    agentID = table.Column<string>(nullable: true),
                    ussdString = table.Column<string>(nullable: true),
                    transactionID = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    statusDescription = table.Column<string>(nullable: true),
                    request = table.Column<string>(nullable: true),
                    response = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_UssdTransactions", x => x.ussdTransactionID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_VASProviderChannels",
                columns: table => new
                {
                    ProviderChannelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ServiceProvider = table.Column<string>(nullable: true),
                    ServiceCategory = table.Column<string>(nullable: true),
                    ServiceDetails = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<int>(nullable: false),
                    Modifiedby = table.Column<int>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Commission = table.Column<double>(nullable: false),
                    Cap = table.Column<double>(nullable: false),
                    ApiKey = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    Notification = table.Column<string>(nullable: true),
                    Verification = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_VASProviderChannels", x => x.ProviderChannelID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Vendor",
                columns: table => new
                {
                    VendorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VendorCode = table.Column<string>(nullable: true),
                    VendorName = table.Column<string>(nullable: true),
                    APIKey = table.Column<string>(nullable: true),
                    RCNumber = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Vendor", x => x.VendorID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Webhooklogs",
                columns: table => new
                {
                    webHooklogsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    provider = table.Column<string>(nullable: true),
                    service = table.Column<string>(nullable: true),
                    payload = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Webhooklogs", x => x.webHooklogsID);
                });

            migrationBuilder.CreateTable(
                name: "EgsParentTerminal",
                columns: table => new
                {
                    ParentTerminalKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentTerminalID = table.Column<string>(nullable: true),
                    Provider = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EgsParentTerminal", x => x.ParentTerminalKey);
                });

            migrationBuilder.CreateTable(
                name: "EgsPosType",
                columns: table => new
                {
                    PosTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PosType = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    IsAvailaible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EgsPosType", x => x.PosTypeID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Bank",
                columns: table => new
                {
                    BankID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BankName = table.Column<string>(nullable: true),
                    BankCode = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    Provider = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Bank", x => x.BankID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Billers",
                columns: table => new
                {
                    BillerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillerName = table.Column<string>(nullable: true),
                    Services = table.Column<string>(nullable: true),
                    Commission = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Billers", x => x.BillerID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_IdentificationType",
                columns: table => new
                {
                    IdentificationTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdentificationTypeName = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_IdentificationType", x => x.IdentificationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_LGA",
                columns: table => new
                {
                    LgaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LgaName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_LGA", x => x.LgaID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_RequestResponseLog",
                columns: table => new
                {
                    RequestResponseID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Request = table.Column<string>(nullable: true),
                    Response = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    StatusCode = table.Column<string>(nullable: true),
                    Provider = table.Column<string>(nullable: true),
                    TransactionType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_RequestResponseLog", x => x.RequestResponseID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Role",
                columns: table => new
                {
                    RoleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_States",
                columns: table => new
                {
                    StateID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StateName = table.Column<string>(nullable: true),
                    StateCode = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_States", x => x.StateID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Title",
                columns: table => new
                {
                    TitleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TitleName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Title", x => x.TitleID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_TransactionLimits",
                columns: table => new
                {
                    TransactionLimitID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionLimitName = table.Column<string>(nullable: true),
                    TransactionLimit = table.Column<double>(nullable: false),
                    TransactionBalance = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_TransactionLimits", x => x.TransactionLimitID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_TransactionType",
                columns: table => new
                {
                    TransactionTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionType = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_TransactionType", x => x.TransactionTypeID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_UserContactInfo",
                columns: table => new
                {
                    UserContactInfoID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    UserAddress = table.Column<string>(nullable: true),
                    StateID = table.Column<int>(nullable: false),
                    PostalCode = table.Column<string>(nullable: true),
                    LgaID = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_UserContactInfo", x => x.UserContactInfoID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_UserType",
                columns: table => new
                {
                    UserTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserType = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    DailyLimit = table.Column<double>(nullable: false),
                    TransactionLimit = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_UserType", x => x.UserTypeID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Target",
                columns: table => new
                {
                    TargetID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TargetName = table.Column<string>(nullable: true),
                    TargetPeriodFrom = table.Column<DateTime>(nullable: false),
                    TargetPeriodTo = table.Column<DateTime>(nullable: false),
                    TargetAmount = table.Column<double>(nullable: false),
                    TransactionMethodID = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Target", x => x.TargetID);
                    table.ForeignKey(
                        name: "FK_SYS_Target_EGS_TransactionMethod_TransactionMethodID",
                        column: x => x.TransactionMethodID,
                        principalTable: "EGS_TransactionMethod",
                        principalColumn: "TransactionMethodID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Terminal",
                columns: table => new
                {
                    TerminalKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TerminalID = table.Column<string>(nullable: true),
                    SerialNumber = table.Column<string>(nullable: true),
                    TerminalType = table.Column<string>(nullable: true),
                    agentid = table.Column<int>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    BatchNumber = table.Column<string>(nullable: true),
                    ParentTerminalKey = table.Column<int>(nullable: true),
                    AgentTerminalID = table.Column<string>(nullable: true),
                    AggregatorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Terminal", x => x.TerminalKey);
                    table.ForeignKey(
                        name: "FK_EGS_Terminal_EgsParentTerminal_ParentTerminalKey",
                        column: x => x.ParentTerminalKey,
                        principalTable: "EgsParentTerminal",
                        principalColumn: "ParentTerminalKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EgsPosRequest",
                columns: table => new
                {
                    PosrequestID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PosTypeID = table.Column<int>(nullable: true),
                    MainWalletAccount = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Devicetype = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IsProccessed = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    Approvedby = table.Column<int>(nullable: false),
                    DateApproved = table.Column<DateTime>(nullable: false),
                    RejectedBy = table.Column<int>(nullable: false),
                    DateRejected = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EgsPosRequest", x => x.PosrequestID);
                    table.ForeignKey(
                        name: "FK_EgsPosRequest_EgsPosType_PosTypeID",
                        column: x => x.PosTypeID,
                        principalTable: "EgsPosType",
                        principalColumn: "PosTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Beneficiary",
                columns: table => new
                {
                    BeneficaryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<int>(nullable: false),
                    BeneficiaryBankAccount = table.Column<string>(nullable: true),
                    BeneficiaryName = table.Column<string>(nullable: true),
                    BankID = table.Column<int>(nullable: true),
                    Datecreated = table.Column<DateTime>(nullable: false),
                    BankCode = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BankClient = table.Column<string>(nullable: true),
                    WalletId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Beneficiary", x => x.BeneficaryID);
                    table.ForeignKey(
                        name: "FK_EGS_Beneficiary_SYS_Bank_BankID",
                        column: x => x.BankID,
                        principalTable: "SYS_Bank",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Merchant",
                columns: table => new
                {
                    MerchantID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MerchantType = table.Column<int>(nullable: false),
                    MerchantCode = table.Column<string>(nullable: true),
                    AccountNo = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    KYC = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    BankID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Merchant", x => x.MerchantID);
                    table.ForeignKey(
                        name: "FK_EGS_Merchant_SYS_Bank_BankID",
                        column: x => x.BankID,
                        principalTable: "SYS_Bank",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SYS_UserKYCInfo",
                columns: table => new
                {
                    UserKYCID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdentificationTypeID = table.Column<int>(nullable: true),
                    IdentificationValue = table.Column<string>(nullable: true),
                    BankAccount = table.Column<string>(nullable: true),
                    BankID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_UserKYCInfo", x => x.UserKYCID);
                    table.ForeignKey(
                        name: "FK_SYS_UserKYCInfo_SYS_Bank_BankID",
                        column: x => x.BankID,
                        principalTable: "SYS_Bank",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SYS_UserKYCInfo_SYS_IdentificationType_IdentificationTypeID",
                        column: x => x.IdentificationTypeID,
                        principalTable: "SYS_IdentificationType",
                        principalColumn: "IdentificationTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Document",
                columns: table => new
                {
                    DocumentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocumentName = table.Column<string>(nullable: true),
                    DocumentType = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    RoleID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Document", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_SYS_Document_SYS_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "SYS_Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ProductParameter = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ProductCategoryID = table.Column<int>(nullable: true),
                    Notification = table.Column<string>(nullable: true),
                    Verification = table.Column<string>(nullable: true),
                    DiscountedAmount = table.Column<double>(nullable: false),
                    CommisionAmount = table.Column<double>(nullable: false),
                    ProductDescription = table.Column<string>(nullable: true),
                    IsFixedAmount = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    InvoiceModeID = table.Column<int>(nullable: false),
                    IPAddress = table.Column<string>(nullable: true),
                    APIKey = table.Column<string>(nullable: true),
                    MerchantID = table.Column<int>(nullable: true),
                    NotificationIndicator = table.Column<string>(nullable: true),
                    VerificationIndicator = table.Column<string>(nullable: true),
                    ActionUrl = table.Column<string>(nullable: true),
                    ActionResult = table.Column<string>(nullable: true),
                    AuthenticationUrl = table.Column<string>(nullable: true),
                    AuthenticationResult = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_EGS_Products_EGS_Merchant_MerchantID",
                        column: x => x.MerchantID,
                        principalTable: "EGS_Merchant",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_Products_EGS_ProductCategories_ProductCategoryID",
                        column: x => x.ProductCategoryID,
                        principalTable: "EGS_ProductCategories",
                        principalColumn: "ProductCategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SYS_MerchantMapping",
                columns: table => new
                {
                    MerchantMappingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubMerchantID = table.Column<int>(nullable: false),
                    MerchantID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_MerchantMapping", x => x.MerchantMappingID);
                    table.ForeignKey(
                        name: "FK_SYS_MerchantMapping_EGS_Merchant_MerchantID",
                        column: x => x.MerchantID,
                        principalTable: "EGS_Merchant",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_DocumentValue",
                columns: table => new
                {
                    DocumentValueID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocumentID = table.Column<int>(nullable: true),
                    DocPath = table.Column<string>(nullable: true),
                    UserKYCID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_DocumentValue", x => x.DocumentValueID);
                    table.ForeignKey(
                        name: "FK_EGS_DocumentValue_SYS_Document_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "SYS_Document",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_DocumentValue_SYS_UserKYCInfo_UserKYCID",
                        column: x => x.UserKYCID,
                        principalTable: "SYS_UserKYCInfo",
                        principalColumn: "UserKYCID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Invoice",
                columns: table => new
                {
                    InvoiceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReferenceNo = table.Column<string>(maxLength: 150, nullable: true),
                    ServiceNumber = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    PaymentStatus = table.Column<bool>(nullable: false),
                    PaymentReference = table.Column<string>(maxLength: 150, nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    ProductID = table.Column<int>(nullable: true),
                    ProductItemID = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    AlternateReferenceNo = table.Column<string>(nullable: true),
                    CustomerAlternateRef = table.Column<string>(nullable: true),
                    IsAggregatorSettled = table.Column<bool>(nullable: false),
                    CustomerEmail = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    RegistrationNo = table.Column<string>(nullable: true),
                    SurCharge = table.Column<double>(nullable: false),
                    AggregatorPushed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    TerminalID = table.Column<string>(nullable: true),
                    BankID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Invoice", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_EGS_Invoice_SYS_Bank_BankID",
                        column: x => x.BankID,
                        principalTable: "SYS_Bank",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_Invoice_EGS_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "EGS_Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_SettlementSummary",
                columns: table => new
                {
                    SettlementSummaryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MerchantID = table.Column<int>(nullable: true),
                    Narration = table.Column<string>(nullable: true),
                    ProductID = table.Column<int>(nullable: true),
                    AmountSettled = table.Column<double>(nullable: false),
                    AccountNo = table.Column<string>(nullable: true),
                    SettlementReference = table.Column<string>(nullable: true),
                    TotalCollection = table.Column<double>(nullable: false),
                    IsPaid = table.Column<bool>(nullable: false),
                    DatePaid = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    BankID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_SettlementSummary", x => x.SettlementSummaryID);
                    table.ForeignKey(
                        name: "FK_EGS_SettlementSummary_SYS_Bank_BankID",
                        column: x => x.BankID,
                        principalTable: "SYS_Bank",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_SettlementSummary_EGS_Merchant_MerchantID",
                        column: x => x.MerchantID,
                        principalTable: "EGS_Merchant",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_SettlementSummary_EGS_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "EGS_Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<string>(nullable: true),
                    BVN = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    AccountTypeID = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsSignUp = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: false),
                    Is_Approved = table.Column<bool>(nullable: false),
                    ApprovedBy2 = table.Column<int>(nullable: false),
                    RCNumber = table.Column<string>(nullable: true),
                    RoleID = table.Column<int>(nullable: true),
                    UserTypeID = table.Column<int>(nullable: true),
                    UserContactInfoID = table.Column<int>(nullable: true),
                    StateID = table.Column<int>(nullable: true),
                    LgaID = table.Column<int>(nullable: true),
                    TitleID = table.Column<int>(nullable: true),
                    KYCID = table.Column<int>(nullable: true),
                    WalletID = table.Column<int>(nullable: true),
                    TransactionLimitID = table.Column<int>(nullable: true),
                    AggregatorID = table.Column<int>(nullable: true),
                    MerchantID = table.Column<int>(nullable: true),
                    BusinessName = table.Column<string>(nullable: true),
                    BusinessAddress = table.Column<string>(nullable: true),
                    IsSpecial = table.Column<bool>(nullable: false),
                    BankAccountCode = table.Column<string>(nullable: true),
                    Is_UseFingerprint = table.Column<bool>(nullable: false),
                    TerminalID = table.Column<string>(nullable: true),
                    TerminalKey = table.Column<int>(nullable: true),
                    ActivationCode = table.Column<string>(nullable: true),
                    ActivationStatus = table.Column<string>(nullable: true),
                    AgentCode = table.Column<string>(nullable: true),
                    AgentTypeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_SYS_Users_SYS_LGA_LgaID",
                        column: x => x.LgaID,
                        principalTable: "SYS_LGA",
                        principalColumn: "LgaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SYS_Users_EGS_Merchant_MerchantID",
                        column: x => x.MerchantID,
                        principalTable: "EGS_Merchant",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SYS_Users_SYS_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "SYS_Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SYS_Users_SYS_States_StateID",
                        column: x => x.StateID,
                        principalTable: "SYS_States",
                        principalColumn: "StateID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SYS_Users_EGS_Terminal_TerminalKey",
                        column: x => x.TerminalKey,
                        principalTable: "EGS_Terminal",
                        principalColumn: "TerminalKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SYS_Users_SYS_Title_TitleID",
                        column: x => x.TitleID,
                        principalTable: "SYS_Title",
                        principalColumn: "TitleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SYS_Users_SYS_TransactionLimits_TransactionLimitID",
                        column: x => x.TransactionLimitID,
                        principalTable: "SYS_TransactionLimits",
                        principalColumn: "TransactionLimitID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SYS_Users_SYS_UserType_UserTypeID",
                        column: x => x.UserTypeID,
                        principalTable: "SYS_UserType",
                        principalColumn: "UserTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CanCreate = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    RoleID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_AccountInfo",
                columns: table => new
                {
                    AccountsInfoID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: true),
                    BankID = table.Column<int>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_AccountInfo", x => x.AccountsInfoID);
                    table.ForeignKey(
                        name: "FK_EGS_AccountInfo_SYS_Bank_BankID",
                        column: x => x.BankID,
                        principalTable: "SYS_Bank",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_AccountInfo_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_AccountType",
                columns: table => new
                {
                    AccountTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountTypeName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedByUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_AccountType", x => x.AccountTypeID);
                    table.ForeignKey(
                        name: "FK_EGS_AccountType_SYS_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_ProductItem",
                columns: table => new
                {
                    ProductItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductItemCode = table.Column<string>(nullable: true),
                    ProductID = table.Column<int>(nullable: true),
                    UserID = table.Column<int>(nullable: true),
                    ProductItemName = table.Column<string>(nullable: true),
                    DiscountedAmount = table.Column<double>(nullable: false),
                    CommisionAmount = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    Istransactionfee = table.Column<bool>(nullable: false),
                    ProductItemCategory = table.Column<string>(nullable: true),
                    CostRecovery = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_ProductItem", x => x.ProductItemID);
                    table.ForeignKey(
                        name: "FK_EGS_ProductItem_EGS_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "EGS_Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_ProductItem_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Egs_SubUser",
                columns: table => new
                {
                    TerminalKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SerialNumber = table.Column<string>(nullable: true),
                    TerminalType = table.Column<string>(nullable: true),
                    AgentfirstName = table.Column<string>(nullable: true),
                    AgentlastName = table.Column<string>(nullable: true),
                    Agentemail = table.Column<string>(nullable: true),
                    AgentphoneNumber = table.Column<string>(nullable: true),
                    SubUserID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    AgentID = table.Column<int>(nullable: false),
                    TerminalID = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IsLinkedID = table.Column<int>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false),
                    DateUnprocessed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egs_SubUser", x => x.TerminalKey);
                    table.ForeignKey(
                        name: "FK_Egs_SubUser_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EGS_UpgradeAccount",
                columns: table => new
                {
                    UpgradeAccountID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: true),
                    RoleRequestID = table.Column<int>(nullable: false),
                    UserTypeRequestID = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false),
                    ProcessedBy = table.Column<int>(nullable: false),
                    DateProcessed = table.Column<DateTime>(nullable: false),
                    RejectedBy = table.Column<int>(nullable: false),
                    DateRejected = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_UpgradeAccount", x => x.UpgradeAccountID);
                    table.ForeignKey(
                        name: "FK_EGS_UpgradeAccount_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Wallet",
                columns: table => new
                {
                    WalletID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: true),
                    WalletAccountNumber = table.Column<string>(nullable: true),
                    BVN = table.Column<string>(nullable: true),
                    WalletBalance = table.Column<double>(nullable: false),
                    WalletCredentials = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    OpeningBalance = table.Column<double>(nullable: false),
                    ClosingBalance = table.Column<double>(nullable: false),
                    OpeningBalanceDate = table.Column<DateTime>(nullable: false),
                    ClosingBalanceDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Wallet", x => x.WalletID);
                    table.ForeignKey(
                        name: "FK_EGS_Wallet_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SYS_AgentType",
                columns: table => new
                {
                    AgentTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeName = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: true),
                    TransactionMethodID = table.Column<int>(nullable: false),
                    ServiceDetails = table.Column<string>(nullable: true),
                    Commission = table.Column<double>(nullable: false),
                    CapAtValue = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_AgentType", x => x.AgentTypeID);
                    table.ForeignKey(
                        name: "FK_SYS_AgentType_EGS_TransactionMethod_TransactionMethodID",
                        column: x => x.TransactionMethodID,
                        principalTable: "EGS_TransactionMethod",
                        principalColumn: "TransactionMethodID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_AgentType_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Aggregator",
                columns: table => new
                {
                    AggregatorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AggregatorName = table.Column<string>(nullable: true),
                    AggregatorType = table.Column<int>(nullable: false),
                    AggregatorCode = table.Column<string>(nullable: true),
                    AccountNo = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    KYC = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AggregatorTypeID = table.Column<int>(nullable: false),
                    BankID = table.Column<int>(nullable: true),
                    UserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Aggregator", x => x.AggregatorID);
                    table.ForeignKey(
                        name: "FK_SYS_Aggregator_SYS_Bank_BankID",
                        column: x => x.BankID,
                        principalTable: "SYS_Bank",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SYS_Aggregator_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SYS_ServiceNotification",
                columns: table => new
                {
                    ServiceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    UserID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_ServiceNotification", x => x.ServiceID);
                    table.ForeignKey(
                        name: "FK_SYS_ServiceNotification_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_ProductItemRate",
                columns: table => new
                {
                    ProductItemRateID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductItemID = table.Column<int>(nullable: true),
                    AmountRate = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedByUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_ProductItemRate", x => x.ProductItemRateID);
                    table.ForeignKey(
                        name: "FK_EGS_ProductItemRate_SYS_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_ProductItemRate_EGS_ProductItem_ProductItemID",
                        column: x => x.ProductItemID,
                        principalTable: "EGS_ProductItem",
                        principalColumn: "ProductItemID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Sales",
                columns: table => new
                {
                    SalesID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WalletTransactionID = table.Column<int>(nullable: true),
                    DiscountedAmount = table.Column<double>(nullable: false),
                    CommisionAmount = table.Column<double>(nullable: false),
                    ProductID = table.Column<int>(nullable: true),
                    ProductItemID = table.Column<int>(nullable: true),
                    IsSettled = table.Column<bool>(nullable: false),
                    IPAddress = table.Column<string>(nullable: true),
                    TransactionReferenceNumber = table.Column<string>(nullable: true),
                    SalesReferenceNumber = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    SettlementDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Sales", x => x.SalesID);
                    table.ForeignKey(
                        name: "FK_EGS_Sales_EGS_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "EGS_Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_Sales_EGS_ProductItem_ProductItemID",
                        column: x => x.ProductItemID,
                        principalTable: "EGS_ProductItem",
                        principalColumn: "ProductItemID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_SettlementLog",
                columns: table => new
                {
                    SettlementLogID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductID = table.Column<int>(nullable: true),
                    ProductItemID = table.Column<int>(nullable: true),
                    Totalcollection = table.Column<double>(nullable: false),
                    SettlementIntervalID = table.Column<int>(nullable: false),
                    SalesID = table.Column<int>(nullable: false),
                    MerchantID = table.Column<int>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    MerchantAccountNo = table.Column<string>(nullable: true),
                    SettlementModeID = table.Column<int>(nullable: true),
                    SettlementTypeID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    IsPaid = table.Column<bool>(nullable: false),
                    DatePaid = table.Column<DateTime>(nullable: false),
                    BankID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_SettlementLog", x => x.SettlementLogID);
                    table.ForeignKey(
                        name: "FK_EGS_SettlementLog_SYS_Bank_BankID",
                        column: x => x.BankID,
                        principalTable: "SYS_Bank",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_SettlementLog_EGS_Merchant_MerchantID",
                        column: x => x.MerchantID,
                        principalTable: "EGS_Merchant",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_SettlementLog_EGS_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "EGS_Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_SettlementLog_EGS_ProductItem_ProductItemID",
                        column: x => x.ProductItemID,
                        principalTable: "EGS_ProductItem",
                        principalColumn: "ProductItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_SettlementLog_EGS_SettlementMode_SettlementModeID",
                        column: x => x.SettlementModeID,
                        principalTable: "EGS_SettlementMode",
                        principalColumn: "SettlementModeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_SettlementLog_EGS_SettlementType_SettlementTypeID",
                        column: x => x.SettlementTypeID,
                        principalTable: "EGS_SettlementType",
                        principalColumn: "SettlementTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_StoreTransaction",
                columns: table => new
                {
                    StoreTransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductItemID = table.Column<int>(nullable: true),
                    TransactionTypeID = table.Column<int>(nullable: true),
                    TransactionMethodID = table.Column<int>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    IPAdress = table.Column<string>(nullable: true),
                    TransactionReferenceNumber = table.Column<string>(nullable: true),
                    InvoiceID = table.Column<int>(nullable: true),
                    PhoneEmail = table.Column<string>(nullable: true),
                    RegistrationNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_StoreTransaction", x => x.StoreTransactionID);
                    table.ForeignKey(
                        name: "FK_EGS_StoreTransaction_EGS_Invoice_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "EGS_Invoice",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_StoreTransaction_EGS_ProductItem_ProductItemID",
                        column: x => x.ProductItemID,
                        principalTable: "EGS_ProductItem",
                        principalColumn: "ProductItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_StoreTransaction_EGS_TransactionMethod_TransactionMethodID",
                        column: x => x.TransactionMethodID,
                        principalTable: "EGS_TransactionMethod",
                        principalColumn: "TransactionMethodID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_StoreTransaction_SYS_TransactionType_TransactionTypeID",
                        column: x => x.TransactionTypeID,
                        principalTable: "SYS_TransactionType",
                        principalColumn: "TransactionTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_CardHolder",
                columns: table => new
                {
                    CardholderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardHolderName = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true),
                    PAN = table.Column<string>(nullable: true),
                    Pin = table.Column<string>(nullable: true),
                    CVV = table.Column<string>(nullable: true),
                    EXP = table.Column<string>(nullable: true),
                    WalletID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_CardHolder", x => x.CardholderID);
                    table.ForeignKey(
                        name: "FK_EGS_CardHolder_EGS_Wallet_WalletID",
                        column: x => x.WalletID,
                        principalTable: "EGS_Wallet",
                        principalColumn: "WalletID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_SubAgentWallet",
                columns: table => new
                {
                    SubAgentWalletId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    WalletID = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    OpeningBalance = table.Column<int>(nullable: false),
                    ClosingBalance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_SubAgentWallet", x => x.SubAgentWalletId);
                    table.ForeignKey(
                        name: "FK_EGS_SubAgentWallet_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_SubAgentWallet_EGS_Wallet_WalletID",
                        column: x => x.WalletID,
                        principalTable: "EGS_Wallet",
                        principalColumn: "WalletID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Egs_SubWallet",
                columns: table => new
                {
                    subWalletID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BusinessName = table.Column<string>(nullable: true),
                    BusinessDescription = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: false),
                    RCNumber = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BusinessAddress = table.Column<string>(nullable: true),
                    DateApproved = table.Column<DateTime>(nullable: false),
                    WalletID = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    MainWallet = table.Column<string>(nullable: true),
                    CurrentBalance = table.Column<double>(nullable: false),
                    OpeningBalance = table.Column<double>(nullable: false),
                    ClosingBalance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egs_SubWallet", x => x.subWalletID);
                    table.ForeignKey(
                        name: "FK_Egs_SubWallet_EGS_Wallet_WalletID",
                        column: x => x.WalletID,
                        principalTable: "EGS_Wallet",
                        principalColumn: "WalletID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EGS_Tickets",
                columns: table => new
                {
                    TicketID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReferenceNo = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    WalletID = table.Column<int>(nullable: true),
                    ReceivedBy = table.Column<int>(nullable: false),
                    TreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_Tickets", x => x.TicketID);
                    table.ForeignKey(
                        name: "FK_EGS_Tickets_EGS_Wallet_WalletID",
                        column: x => x.WalletID,
                        principalTable: "EGS_Wallet",
                        principalColumn: "WalletID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FundWallets",
                columns: table => new
                {
                    FundWalletID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WalletID = table.Column<int>(nullable: true),
                    UserID = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundWallets", x => x.FundWalletID);
                    table.ForeignKey(
                        name: "FK_FundWallets_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FundWallets_EGS_Wallet_WalletID",
                        column: x => x.WalletID,
                        principalTable: "EGS_Wallet",
                        principalColumn: "WalletID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_AggregatorRequest",
                columns: table => new
                {
                    RequestID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: true),
                    AggregatorID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: false),
                    DateApproved = table.Column<DateTime>(nullable: false),
                    RejectedBy = table.Column<int>(nullable: false),
                    DateRejected = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    ApprovedBy2 = table.Column<int>(nullable: false),
                    DateApprovedBy2 = table.Column<DateTime>(nullable: false),
                    RejectedBy2 = table.Column<int>(nullable: false),
                    DateRejectedBy2 = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_AggregatorRequest", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_EGS_AggregatorRequest_SYS_Aggregator_AggregatorID",
                        column: x => x.AggregatorID,
                        principalTable: "SYS_Aggregator",
                        principalColumn: "AggregatorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_AggregatorRequest_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SYS_AgentMapping",
                columns: table => new
                {
                    AgentMappingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AgentID = table.Column<int>(nullable: false),
                    AggregatorID = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_AgentMapping", x => x.AgentMappingID);
                    table.ForeignKey(
                        name: "FK_SYS_AgentMapping_SYS_Aggregator_AggregatorID",
                        column: x => x.AggregatorID,
                        principalTable: "SYS_Aggregator",
                        principalColumn: "AggregatorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_AggregatorType",
                columns: table => new
                {
                    AggregatorTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeName = table.Column<string>(nullable: true),
                    AggregatorID = table.Column<int>(nullable: false),
                    TransactionMethodID = table.Column<int>(nullable: false),
                    ServiceDetails = table.Column<string>(nullable: true),
                    Commission = table.Column<double>(nullable: false),
                    CapAtValue = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_AggregatorType", x => x.AggregatorTypeID);
                    table.ForeignKey(
                        name: "FK_SYS_AggregatorType_SYS_Aggregator_AggregatorID",
                        column: x => x.AggregatorID,
                        principalTable: "SYS_Aggregator",
                        principalColumn: "AggregatorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_AggregatorType_EGS_TransactionMethod_TransactionMethodID",
                        column: x => x.TransactionMethodID,
                        principalTable: "EGS_TransactionMethod",
                        principalColumn: "TransactionMethodID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EGS_TicketDetails",
                columns: table => new
                {
                    TDetailsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TicketID = table.Column<int>(nullable: true),
                    Feedback = table.Column<string>(nullable: true),
                    FeedbackBy = table.Column<int>(nullable: false),
                    IsOfficial = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_TicketDetails", x => x.TDetailsID);
                    table.ForeignKey(
                        name: "FK_EGS_TicketDetails_EGS_Tickets_TicketID",
                        column: x => x.TicketID,
                        principalTable: "EGS_Tickets",
                        principalColumn: "TicketID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_WalletTransaction",
                columns: table => new
                {
                    WalletTransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WalletID = table.Column<int>(nullable: true),
                    ProductItemID = table.Column<int>(nullable: true),
                    TransactionTypeID = table.Column<int>(nullable: true),
                    TransactionMethodID = table.Column<int>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    WalletReferenceNumber = table.Column<string>(nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    IPAddress = table.Column<string>(nullable: true),
                    TransactionReferenceNo = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    Previous = table.Column<double>(nullable: false),
                    Current = table.Column<double>(nullable: false),
                    TransactionStatus = table.Column<string>(nullable: true),
                    SurCharge = table.Column<double>(nullable: false),
                    TerminalKey = table.Column<int>(nullable: true),
                    InvoiceID = table.Column<int>(nullable: true),
                    TransactionID = table.Column<int>(nullable: true),
                    BillerID = table.Column<int>(nullable: true),
                    Narration = table.Column<string>(nullable: true),
                    ReceiverAccount = table.Column<string>(nullable: true),
                    ReceiverName = table.Column<string>(nullable: true),
                    ReceiverBank = table.Column<string>(nullable: true),
                    SessionID = table.Column<string>(nullable: true),
                    TransactionClientType = table.Column<string>(nullable: true),
                    IsCompleted = table.Column<int>(nullable: false),
                    MainAccountNumber = table.Column<string>(nullable: true),
                    PayItemRef = table.Column<string>(nullable: true),
                    IsSettled = table.Column<bool>(nullable: false),
                    IsPushed = table.Column<bool>(nullable: false),
                    DateSettled = table.Column<DateTime>(nullable: false),
                    DatePushed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_WalletTransaction", x => x.WalletTransactionID);
                    table.ForeignKey(
                        name: "FK_EGS_WalletTransaction_SYS_Billers_BillerID",
                        column: x => x.BillerID,
                        principalTable: "SYS_Billers",
                        principalColumn: "BillerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_WalletTransaction_EGS_Invoice_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "EGS_Invoice",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_WalletTransaction_EGS_ProductItem_ProductItemID",
                        column: x => x.ProductItemID,
                        principalTable: "EGS_ProductItem",
                        principalColumn: "ProductItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_WalletTransaction_EGS_Terminal_TerminalKey",
                        column: x => x.TerminalKey,
                        principalTable: "EGS_Terminal",
                        principalColumn: "TerminalKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_WalletTransaction_EGS_TransactionMethod_TransactionMethodID",
                        column: x => x.TransactionMethodID,
                        principalTable: "EGS_TransactionMethod",
                        principalColumn: "TransactionMethodID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_WalletTransaction_SYS_TransactionType_TransactionTypeID",
                        column: x => x.TransactionTypeID,
                        principalTable: "SYS_TransactionType",
                        principalColumn: "TransactionTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_WalletTransaction_EGS_Wallet_WalletID",
                        column: x => x.WalletID,
                        principalTable: "EGS_Wallet",
                        principalColumn: "WalletID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EGS_EgoleWalletTransactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: true),
                    BankID = table.Column<int>(nullable: true),
                    TransferAmount = table.Column<double>(nullable: false),
                    TransactionTypeID = table.Column<int>(nullable: true),
                    TransactionMethodID = table.Column<int>(nullable: true),
                    WalletID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Narration = table.Column<string>(nullable: true),
                    ReceiverAccount = table.Column<string>(nullable: true),
                    ReceiverName = table.Column<string>(nullable: true),
                    ReceiverBank = table.Column<string>(nullable: true),
                    WalletTransactionID = table.Column<int>(nullable: true),
                    SessionID = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_EgoleWalletTransactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_EGS_EgoleWalletTransactions_SYS_Bank_BankID",
                        column: x => x.BankID,
                        principalTable: "SYS_Bank",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_EgoleWalletTransactions_EGS_TransactionMethod_TransactionMethodID",
                        column: x => x.TransactionMethodID,
                        principalTable: "EGS_TransactionMethod",
                        principalColumn: "TransactionMethodID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_EgoleWalletTransactions_SYS_TransactionType_TransactionTypeID",
                        column: x => x.TransactionTypeID,
                        principalTable: "SYS_TransactionType",
                        principalColumn: "TransactionTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_EgoleWalletTransactions_SYS_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "SYS_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_EgoleWalletTransactions_EGS_Wallet_WalletID",
                        column: x => x.WalletID,
                        principalTable: "EGS_Wallet",
                        principalColumn: "WalletID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_EgoleWalletTransactions_EGS_WalletTransaction_WalletTransactionID",
                        column: x => x.WalletTransactionID,
                        principalTable: "EGS_WalletTransaction",
                        principalColumn: "WalletTransactionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Company",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyName = table.Column<string>(nullable: true),
                    CompanyAddress = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    GLAccountID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Company", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "EGS_GLAccounts",
                columns: table => new
                {
                    GLAccountID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GLAccountCode = table.Column<string>(nullable: true),
                    BankID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsInUse = table.Column<bool>(nullable: false),
                    BankAccount = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EGS_GLAccounts", x => x.GLAccountID);
                    table.ForeignKey(
                        name: "FK_EGS_GLAccounts_SYS_Bank_BankID",
                        column: x => x.BankID,
                        principalTable: "SYS_Bank",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EGS_GLAccounts_SYS_Company_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "SYS_Company",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_UserID",
                table: "AspNetRoles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_AccountInfo_BankID",
                table: "EGS_AccountInfo",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_AccountInfo_UserID",
                table: "EGS_AccountInfo",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_AccountType_CreatedByUserID",
                table: "EGS_AccountType",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_AggregatorRequest_AggregatorID",
                table: "EGS_AggregatorRequest",
                column: "AggregatorID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_AggregatorRequest_UserID",
                table: "EGS_AggregatorRequest",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_Beneficiary_BankID",
                table: "EGS_Beneficiary",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_CardHolder_WalletID",
                table: "EGS_CardHolder",
                column: "WalletID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_DocumentValue_DocumentID",
                table: "EGS_DocumentValue",
                column: "DocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_DocumentValue_UserKYCID",
                table: "EGS_DocumentValue",
                column: "UserKYCID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_EgoleWalletTransactions_BankID",
                table: "EGS_EgoleWalletTransactions",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_EgoleWalletTransactions_TransactionMethodID",
                table: "EGS_EgoleWalletTransactions",
                column: "TransactionMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_EgoleWalletTransactions_TransactionTypeID",
                table: "EGS_EgoleWalletTransactions",
                column: "TransactionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_EgoleWalletTransactions_UserID",
                table: "EGS_EgoleWalletTransactions",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_EgoleWalletTransactions_WalletID",
                table: "EGS_EgoleWalletTransactions",
                column: "WalletID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_EgoleWalletTransactions_WalletTransactionID",
                table: "EGS_EgoleWalletTransactions",
                column: "WalletTransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_GLAccounts_BankID",
                table: "EGS_GLAccounts",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_GLAccounts_CompanyID",
                table: "EGS_GLAccounts",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_Invoice_BankID",
                table: "EGS_Invoice",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_Invoice_ProductID",
                table: "EGS_Invoice",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_Merchant_BankID",
                table: "EGS_Merchant",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_ProductItem_ProductID",
                table: "EGS_ProductItem",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_ProductItem_UserID",
                table: "EGS_ProductItem",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_ProductItemRate_CreatedByUserID",
                table: "EGS_ProductItemRate",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_ProductItemRate_ProductItemID",
                table: "EGS_ProductItemRate",
                column: "ProductItemID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_Products_MerchantID",
                table: "EGS_Products",
                column: "MerchantID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_Products_ProductCategoryID",
                table: "EGS_Products",
                column: "ProductCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_Sales_ProductID",
                table: "EGS_Sales",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_Sales_ProductItemID",
                table: "EGS_Sales",
                column: "ProductItemID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_SettlementLog_BankID",
                table: "EGS_SettlementLog",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_SettlementLog_MerchantID",
                table: "EGS_SettlementLog",
                column: "MerchantID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_SettlementLog_ProductID",
                table: "EGS_SettlementLog",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_SettlementLog_ProductItemID",
                table: "EGS_SettlementLog",
                column: "ProductItemID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_SettlementLog_SettlementModeID",
                table: "EGS_SettlementLog",
                column: "SettlementModeID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_SettlementLog_SettlementTypeID",
                table: "EGS_SettlementLog",
                column: "SettlementTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_SettlementSummary_BankID",
                table: "EGS_SettlementSummary",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_SettlementSummary_MerchantID",
                table: "EGS_SettlementSummary",
                column: "MerchantID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_SettlementSummary_ProductID",
                table: "EGS_SettlementSummary",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_StoreTransaction_InvoiceID",
                table: "EGS_StoreTransaction",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_StoreTransaction_ProductItemID",
                table: "EGS_StoreTransaction",
                column: "ProductItemID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_StoreTransaction_TransactionMethodID",
                table: "EGS_StoreTransaction",
                column: "TransactionMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_StoreTransaction_TransactionTypeID",
                table: "EGS_StoreTransaction",
                column: "TransactionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_SubAgentWallet_UserID",
                table: "EGS_SubAgentWallet",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_SubAgentWallet_WalletID",
                table: "EGS_SubAgentWallet",
                column: "WalletID");

            migrationBuilder.CreateIndex(
                name: "IX_Egs_SubUser_UserID",
                table: "Egs_SubUser",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Egs_SubWallet_WalletID",
                table: "Egs_SubWallet",
                column: "WalletID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_Terminal_ParentTerminalKey",
                table: "EGS_Terminal",
                column: "ParentTerminalKey");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_TicketDetails_TicketID",
                table: "EGS_TicketDetails",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_Tickets_WalletID",
                table: "EGS_Tickets",
                column: "WalletID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_UpgradeAccount_UserID",
                table: "EGS_UpgradeAccount",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_Wallet_UserID",
                table: "EGS_Wallet",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_WalletTransaction_BillerID",
                table: "EGS_WalletTransaction",
                column: "BillerID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_WalletTransaction_InvoiceID",
                table: "EGS_WalletTransaction",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_WalletTransaction_ProductItemID",
                table: "EGS_WalletTransaction",
                column: "ProductItemID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_WalletTransaction_TerminalKey",
                table: "EGS_WalletTransaction",
                column: "TerminalKey");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_WalletTransaction_TransactionID",
                table: "EGS_WalletTransaction",
                column: "TransactionID",
                unique: true,
                filter: "[TransactionID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_WalletTransaction_TransactionMethodID",
                table: "EGS_WalletTransaction",
                column: "TransactionMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_WalletTransaction_TransactionTypeID",
                table: "EGS_WalletTransaction",
                column: "TransactionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_EGS_WalletTransaction_WalletID",
                table: "EGS_WalletTransaction",
                column: "WalletID");

            migrationBuilder.CreateIndex(
                name: "IX_EgsPosRequest_PosTypeID",
                table: "EgsPosRequest",
                column: "PosTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FundWallets_UserID",
                table: "FundWallets",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_FundWallets_WalletID",
                table: "FundWallets",
                column: "WalletID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_AgentMapping_AggregatorID",
                table: "SYS_AgentMapping",
                column: "AggregatorID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_AgentType_TransactionMethodID",
                table: "SYS_AgentType",
                column: "TransactionMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_AgentType_UserID",
                table: "SYS_AgentType",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Aggregator_BankID",
                table: "SYS_Aggregator",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Aggregator_UserID",
                table: "SYS_Aggregator",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_AggregatorType_AggregatorID",
                table: "SYS_AggregatorType",
                column: "AggregatorID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_AggregatorType_TransactionMethodID",
                table: "SYS_AggregatorType",
                column: "TransactionMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Company_GLAccountID",
                table: "SYS_Company",
                column: "GLAccountID",
                unique: true,
                filter: "[GLAccountID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Document_RoleID",
                table: "SYS_Document",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_MerchantMapping_MerchantID",
                table: "SYS_MerchantMapping",
                column: "MerchantID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_ServiceNotification_UserID",
                table: "SYS_ServiceNotification",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Target_TransactionMethodID",
                table: "SYS_Target",
                column: "TransactionMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_UserKYCInfo_BankID",
                table: "SYS_UserKYCInfo",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_UserKYCInfo_IdentificationTypeID",
                table: "SYS_UserKYCInfo",
                column: "IdentificationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_AccountTypeID",
                table: "SYS_Users",
                column: "AccountTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_AgentTypeID",
                table: "SYS_Users",
                column: "AgentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_AggregatorID",
                table: "SYS_Users",
                column: "AggregatorID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_LgaID",
                table: "SYS_Users",
                column: "LgaID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_MerchantID",
                table: "SYS_Users",
                column: "MerchantID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_RoleID",
                table: "SYS_Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_StateID",
                table: "SYS_Users",
                column: "StateID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_TerminalKey",
                table: "SYS_Users",
                column: "TerminalKey");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_TitleID",
                table: "SYS_Users",
                column: "TitleID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_TransactionLimitID",
                table: "SYS_Users",
                column: "TransactionLimitID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_UserTypeID",
                table: "SYS_Users",
                column: "UserTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_WalletID",
                table: "SYS_Users",
                column: "WalletID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_Users_SYS_Aggregator_AggregatorID",
                table: "SYS_Users",
                column: "AggregatorID",
                principalTable: "SYS_Aggregator",
                principalColumn: "AggregatorID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_Users_EGS_Wallet_WalletID",
                table: "SYS_Users",
                column: "WalletID",
                principalTable: "EGS_Wallet",
                principalColumn: "WalletID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_Users_EGS_AccountType_AccountTypeID",
                table: "SYS_Users",
                column: "AccountTypeID",
                principalTable: "EGS_AccountType",
                principalColumn: "AccountTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_Users_SYS_AgentType_AgentTypeID",
                table: "SYS_Users",
                column: "AgentTypeID",
                principalTable: "SYS_AgentType",
                principalColumn: "AgentTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EGS_WalletTransaction_EGS_EgoleWalletTransactions_TransactionID",
                table: "EGS_WalletTransaction",
                column: "TransactionID",
                principalTable: "EGS_EgoleWalletTransactions",
                principalColumn: "TransactionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_Company_EGS_GLAccounts_GLAccountID",
                table: "SYS_Company",
                column: "GLAccountID",
                principalTable: "EGS_GLAccounts",
                principalColumn: "GLAccountID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EGS_AccountType_SYS_Users_CreatedByUserID",
                table: "EGS_AccountType");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_EgoleWalletTransactions_SYS_Users_UserID",
                table: "EGS_EgoleWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_ProductItem_SYS_Users_UserID",
                table: "EGS_ProductItem");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_Wallet_SYS_Users_UserID",
                table: "EGS_Wallet");

            migrationBuilder.DropForeignKey(
                name: "FK_SYS_AgentType_SYS_Users_UserID",
                table: "SYS_AgentType");

            migrationBuilder.DropForeignKey(
                name: "FK_SYS_Aggregator_SYS_Users_UserID",
                table: "SYS_Aggregator");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_EgoleWalletTransactions_SYS_Bank_BankID",
                table: "EGS_EgoleWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_GLAccounts_SYS_Bank_BankID",
                table: "EGS_GLAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_Invoice_SYS_Bank_BankID",
                table: "EGS_Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_Merchant_SYS_Bank_BankID",
                table: "EGS_Merchant");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_EgoleWalletTransactions_EGS_Wallet_WalletID",
                table: "EGS_EgoleWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_WalletTransaction_EGS_Wallet_WalletID",
                table: "EGS_WalletTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_EgoleWalletTransactions_EGS_TransactionMethod_TransactionMethodID",
                table: "EGS_EgoleWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_WalletTransaction_EGS_TransactionMethod_TransactionMethodID",
                table: "EGS_WalletTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_EgoleWalletTransactions_SYS_TransactionType_TransactionTypeID",
                table: "EGS_EgoleWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_WalletTransaction_SYS_TransactionType_TransactionTypeID",
                table: "EGS_WalletTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_EgoleWalletTransactions_EGS_WalletTransaction_WalletTransactionID",
                table: "EGS_EgoleWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_EGS_GLAccounts_SYS_Company_CompanyID",
                table: "EGS_GLAccounts");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EGS_AccountInfo");

            migrationBuilder.DropTable(
                name: "EGS_AccountLookup");

            migrationBuilder.DropTable(
                name: "EGS_AccountManagers");

            migrationBuilder.DropTable(
                name: "EGS_AgentCommission");

            migrationBuilder.DropTable(
                name: "EGS_AggregateSum");

            migrationBuilder.DropTable(
                name: "EGS_AggregatorCommission");

            migrationBuilder.DropTable(
                name: "EGS_AggregatorRequest");

            migrationBuilder.DropTable(
                name: "EGS_AssignAgentTerminal");

            migrationBuilder.DropTable(
                name: "EGS_AuditTrail");

            migrationBuilder.DropTable(
                name: "EGS_AuthFactor");

            migrationBuilder.DropTable(
                name: "EGS_BankBvns");

            migrationBuilder.DropTable(
                name: "EGS_Beneficiary");

            migrationBuilder.DropTable(
                name: "EGS_BettingProviders");

            migrationBuilder.DropTable(
                name: "EGS_BettingUsers");

            migrationBuilder.DropTable(
                name: "EGS_cabletvusers");

            migrationBuilder.DropTable(
                name: "EGS_CardHolder");

            migrationBuilder.DropTable(
                name: "EGS_Channels");

            migrationBuilder.DropTable(
                name: "EGS_Client");

            migrationBuilder.DropTable(
                name: "EGS_Disbursement");

            migrationBuilder.DropTable(
                name: "EGS_DocumentValue");

            migrationBuilder.DropTable(
                name: "EGS_Energymeterusers");

            migrationBuilder.DropTable(
                name: "EGS_InsuranceRemittance");

            migrationBuilder.DropTable(
                name: "EGS_InvoiceMode");

            migrationBuilder.DropTable(
                name: "EGS_Iswpricebands");

            migrationBuilder.DropTable(
                name: "EGS_MerchantType");

            migrationBuilder.DropTable(
                name: "EGS_PeriodicAgents");

            migrationBuilder.DropTable(
                name: "EGS_PeriodicTransactionReport");

            migrationBuilder.DropTable(
                name: "EGS_Pinpads");

            migrationBuilder.DropTable(
                name: "EGS_ProductItemRate");

            migrationBuilder.DropTable(
                name: "EGS_Sales");

            migrationBuilder.DropTable(
                name: "EGS_ServiceType");

            migrationBuilder.DropTable(
                name: "EGS_SettlementBasis");

            migrationBuilder.DropTable(
                name: "EGS_SettlementHistory");

            migrationBuilder.DropTable(
                name: "EGS_SettlementInterval");

            migrationBuilder.DropTable(
                name: "EGS_SettlementLog");

            migrationBuilder.DropTable(
                name: "EGS_SettlementSummary");

            migrationBuilder.DropTable(
                name: "EGS_StoreTransaction");

            migrationBuilder.DropTable(
                name: "EGS_SubAgentWallet");

            migrationBuilder.DropTable(
                name: "Egs_SubUser");

            migrationBuilder.DropTable(
                name: "Egs_SubWallet");

            migrationBuilder.DropTable(
                name: "EGS_TerminalTracking");

            migrationBuilder.DropTable(
                name: "EGS_TicketDetails");

            migrationBuilder.DropTable(
                name: "EGS_TokenValidation");

            migrationBuilder.DropTable(
                name: "EGS_UpgradeAccount");

            migrationBuilder.DropTable(
                name: "EGS_UserDevice");

            migrationBuilder.DropTable(
                name: "EGS_UserNotification");

            migrationBuilder.DropTable(
                name: "EGS_UserToken");

            migrationBuilder.DropTable(
                name: "EGS_UssdTransactions");

            migrationBuilder.DropTable(
                name: "EGS_VASProviderChannels");

            migrationBuilder.DropTable(
                name: "EGS_Vendor");

            migrationBuilder.DropTable(
                name: "EGS_Webhooklogs");

            migrationBuilder.DropTable(
                name: "EgsPosRequest");

            migrationBuilder.DropTable(
                name: "FundWallets");

            migrationBuilder.DropTable(
                name: "SYS_AgentMapping");

            migrationBuilder.DropTable(
                name: "SYS_AggregatorType");

            migrationBuilder.DropTable(
                name: "SYS_MerchantMapping");

            migrationBuilder.DropTable(
                name: "SYS_RequestResponseLog");

            migrationBuilder.DropTable(
                name: "SYS_ServiceNotification");

            migrationBuilder.DropTable(
                name: "SYS_Target");

            migrationBuilder.DropTable(
                name: "SYS_UserContactInfo");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SYS_Document");

            migrationBuilder.DropTable(
                name: "SYS_UserKYCInfo");

            migrationBuilder.DropTable(
                name: "EGS_SettlementMode");

            migrationBuilder.DropTable(
                name: "EGS_SettlementType");

            migrationBuilder.DropTable(
                name: "EGS_Tickets");

            migrationBuilder.DropTable(
                name: "EgsPosType");

            migrationBuilder.DropTable(
                name: "SYS_IdentificationType");

            migrationBuilder.DropTable(
                name: "SYS_Users");

            migrationBuilder.DropTable(
                name: "EGS_AccountType");

            migrationBuilder.DropTable(
                name: "SYS_AgentType");

            migrationBuilder.DropTable(
                name: "SYS_Aggregator");

            migrationBuilder.DropTable(
                name: "SYS_LGA");

            migrationBuilder.DropTable(
                name: "SYS_Role");

            migrationBuilder.DropTable(
                name: "SYS_States");

            migrationBuilder.DropTable(
                name: "SYS_Title");

            migrationBuilder.DropTable(
                name: "SYS_TransactionLimits");

            migrationBuilder.DropTable(
                name: "SYS_UserType");

            migrationBuilder.DropTable(
                name: "SYS_Bank");

            migrationBuilder.DropTable(
                name: "EGS_Wallet");

            migrationBuilder.DropTable(
                name: "EGS_TransactionMethod");

            migrationBuilder.DropTable(
                name: "SYS_TransactionType");

            migrationBuilder.DropTable(
                name: "EGS_WalletTransaction");

            migrationBuilder.DropTable(
                name: "SYS_Billers");

            migrationBuilder.DropTable(
                name: "EGS_Invoice");

            migrationBuilder.DropTable(
                name: "EGS_ProductItem");

            migrationBuilder.DropTable(
                name: "EGS_Terminal");

            migrationBuilder.DropTable(
                name: "EGS_EgoleWalletTransactions");

            migrationBuilder.DropTable(
                name: "EGS_Products");

            migrationBuilder.DropTable(
                name: "EgsParentTerminal");

            migrationBuilder.DropTable(
                name: "EGS_Merchant");

            migrationBuilder.DropTable(
                name: "EGS_ProductCategories");

            migrationBuilder.DropTable(
                name: "SYS_Company");

            migrationBuilder.DropTable(
                name: "EGS_GLAccounts");
        }
    }
}
