using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.Reports.Models;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Controllers
{
    [Area("Reports")]
    [Authorize(Roles = "Super Admin, Merchant")]
    [AllowAnonymous]
    public class SettlementController : Controller
    {
        #region logger
        private readonly ILogger<dynamic> _logger;
        #endregion

        #region repositories
        private IUnitOfWork _unitOfWork;
        #endregion

        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;

        public SettlementController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork,RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger; 
            _roleManager = roleManager;
        }
          
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SettlementSummary()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetMerchantSysUsers(user.UserID);

            if (user2.Role.RoleName.Equals("Merchant"))
            {
                List<SettlementViewModel> model = new List<SettlementViewModel>();
                var Merchantsettlements = await _unitOfWork.MerchantReport.GetMerchantSettlement(user2.Merchant.MerchantID);
                double baseamount = 0;
                double baseCollection = 0;

                foreach (var settlement in Merchantsettlements)
                {
                    var bankName = await _unitOfWork.Bank.GetBanks(settlement.Bank.BankID);
                    var productName = await _unitOfWork.Products.GetProducts(settlement.Product.ProductID);
                    model.Add(new SettlementViewModel
                    {
                        SettlementSummaryID = settlement.SettlementSummaryID,
                        MerchantID = settlement.Merchant.MerchantID,
                        AccountNo = settlement.AccountNo,
                        AmountSettled = settlement.AmountSettled,
                        DateCreated = settlement.DateCreated,
                        Narration = settlement.Narration,
                        BankID = settlement.Bank .BankID,
                        BankName = bankName.BankName,
                        ProductName = productName.ProductName,
                        TotalCollection=settlement.TotalCollection,
                        PaymentStatus = settlement.IsPaid 
                    });
                    baseamount += settlement.AmountSettled;
                    baseCollection += settlement.TotalCollection;
                }
                var modelR = new HoldSettlementViewModel
                {
                    HoldAllSettlement = model
                   
                };
                ViewBag.baseamount = baseamount;
                ViewBag.baseCollection = baseCollection;
                return View(modelR);

            }else if (user2.Role.RoleName.Equals("Super Admin"))
            {
                return RedirectToAction("SettlementSummaryReport");
            }

            return View();
        }
         
        public  IActionResult SettlementSummaryReport()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SettlementSummaryReport(SettlementReportModel reportModel)
        {
            if (ModelState.IsValid)
            {
             return RedirectToAction("SettlementSummaryReports", new { startDay = reportModel.StartDate.Day, startMonth = reportModel.StartDate.Month, startYear = reportModel.StartDate.Year, endDay = reportModel.EndDate.Day, endMonth = reportModel.EndDate.Month, endYear = reportModel.EndDate.Year });
                
            }
            return View();
        }


         public async Task<IActionResult> SettlementSummaryReports(int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
         {  
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetMerchantSysUsers(user.UserID);
            var roleName = user2.Role.RoleName;

            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.SettlementDayStart = startDate.Day;
            ViewBag.SettlementMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.SettlementYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.SettlementDayEnd = endDate.Day;
            ViewBag.SettlementMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.SettlementYearEnd = endDate.Year;
             
            List<SettlementViewModel> model = new List<SettlementViewModel>();
            var Merchantsettlements = await _unitOfWork.MerchantReport.GetSettlementByDateRange(startDate, endDate);
            double baseamount = 0;
            double baseCollection = 0;

            foreach (var settlement in Merchantsettlements)
            {
                var bankName = await _unitOfWork.Bank.GetBanks(settlement.Bank.BankID);
                var productName = await _unitOfWork.Products.GetProducts(settlement.Product.ProductID); 
                model.Add(new SettlementViewModel
                {
                    SettlementSummaryID = settlement.SettlementSummaryID,
                    MerchantID = settlement.Merchant.MerchantID,
                    AccountNo = settlement.AccountNo,
                    AmountSettled = settlement.AmountSettled,
                    DateCreated = settlement.DateCreated,
                    Narration = settlement.Narration,
                    BankID = settlement.Bank.BankID,
                    BankName = bankName.BankName,
                    ProductName = productName.ProductName,
                    TotalCollection = settlement.TotalCollection,
                    PaymentStatus = settlement.IsPaid
                });
                baseamount += settlement.AmountSettled;
                baseCollection += settlement.TotalCollection;
            }
            var modelR = new HoldSettlementViewModel
            {
                HoldAllSettlement = model 
            };
            ViewBag.baseamount = baseamount;
            ViewBag.baseCollection = baseCollection;
            return View(modelR); 
        }

         

        public async Task<IActionResult> SettlementDetails()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetMerchantSysUsers(user.UserID);

            if (user2.Role.RoleName.Equals("Merchant"))
            {
                List<SettlementDetailsViewModel> model = new List<SettlementDetailsViewModel>();
                var Merchantdetailsettlements = await _unitOfWork.MerchantReport.GetMerchantSettlementDetails(user2.Merchant.MerchantID);
                double baseamount = 0;
                double TotalCollectionamount = 0;

                foreach (var settlement in Merchantdetailsettlements)
                {
                    var productname = await _unitOfWork.Products.GetProducts(settlement.Product.ProductID);
                    var productitemname = await _unitOfWork.ProductItem.GetProductItems(settlement.ProductItem.ProductItemID);
                    var settlementIntervalName = await _unitOfWork.SettlementInterval.GetSettlementIntervalById(settlement.SettlementIntervalID);
                    var settlementTypeName = await _unitOfWork.SettlementType.GetSettlementTypeByID(settlement.SettlementType.SettlementTypeID);
                    var settlementModeName = await _unitOfWork.SettlementMode.GetSettlementModeByID(settlement.SettlementMode.SettlementModeID);
                    var bankName = await _unitOfWork.Bank.GetBanks(settlement.Bank.BankID);

                    model.Add(new SettlementDetailsViewModel
                    {
                        SettlementLogID = settlement.SettlementLogID,
                        //ProductItemID = settlement.ProductItem.ProductItemID,
                        //ProductID = settlement.Product.ProductID,
                        ProductItemName = productitemname.ProductItemName,
                        ProductName = productname.ProductName,
                        Totalcollection = settlement.Totalcollection,
                        //SettlementIntervalID = settlement.SettlementIntervalID,
                        SettlementIntervalName= settlementIntervalName.SettlementIntervalName,
                        //MerchantID = settlement.Merchant.MerchantID,
                        Amount = settlement.Amount,
                        //SettlementTypeID = settlement.SettlementType.SettlementTypeID,
                        SettlementTypeName= settlementTypeName.SettlementType,
                        ExactSettlementDate = settlement.DateCreated,
                        IsPaid  =settlement.IsPaid,
                        MerchantAccountNo = settlement.MerchantAccountNo,
                        //SalesID = settlement.SalesID,
                        SettlementModeName= settlementModeName.SettlementModeName,
                        //SettlementModeID =settlement.SettlementMode.SettlementModeID,
                        //BankID = settlement.Bank.BankID ,
                        BankName= bankName.BankName
                    });
                    baseamount += settlement.Amount;
                    TotalCollectionamount += settlement.Totalcollection;
                }
                var modelR = new HoldSettlementDetailsViewModel
                {
                    HoldAllSettlementDetails = model 
                };
                ViewBag.baseamount = baseamount;
                ViewBag.TotalCollectionamount = TotalCollectionamount;
                return View(modelR); 
            }
            else if (user2.Role.RoleName.Equals("Super Admin"))
            {
                return RedirectToAction("SettlementSummaryDetailsReport");
            }

            return View();
        }


        public async Task<List<DDListViews>> ProductsList()
        {
            List<DDListViews> ProductList = new List<DDListViews>();

            var products = await _unitOfWork.Products.GetProductList();//.GetProductListByCategoryID(14); //14--5

            foreach (var product in products)
            {
                ProductList.Add(new DDListViews
                {
                    itemValue = product.ProductID,
                    itemName = product.ProductName
                });
            }

            DropDownModelViews nproductlist = new DropDownModelViews();
            nproductlist.items = ProductList;

            return nproductlist.items;
        }



        public async Task<IActionResult> SettlementSummaryDetailsReport()
        {
            ViewBag.ProductList = await ProductsList();
            return View();
        }


        [HttpPost]
        public IActionResult SettlementSummaryDetailsReport(SettlementReportModel reportModel)
        {
            if (ModelState.IsValid)
            {
                //return RedirectToAction("SettlementDetailsReports", new { startDay = reportModel.StartDate.Day, startMonth = reportModel.StartDate.Month, startYear = reportModel.StartDate.Year, endDay = reportModel.EndDate.Day, endMonth = reportModel.EndDate.Month, endYear = reportModel.EndDate.Year });
                return RedirectToAction("SettlementTransDetailsReport", new { prd = reportModel.ProductID, startDay = reportModel.StartDate.Day, startMonth = reportModel.StartDate.Month, startYear = reportModel.StartDate.Year, endDay = reportModel.EndDate.Day, endMonth = reportModel.EndDate.Month, endYear = reportModel.EndDate.Year });
            }
            return View();
        }

        public async Task<IActionResult> SettlementTransDetailsReport(int prd, int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetMerchantSysUsers(user.UserID);
            var roleName = user2.Role.RoleName;

            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.SettlementDayStart = startDate.Day;
            ViewBag.SettlementMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.SettlementYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.SettlementDayEnd = endDate.Day;
            ViewBag.SettlementMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.SettlementYearEnd = endDate.Year;

            //var user = await _userManager.GetUserAsync(User);
            var walInv = await _unitOfWork.WalletTransaction.GetBackOfficeTransactions(prd, startDate, endDate);

            List<TransactionsModel> model = new List<TransactionsModel>();

            double baseamount = 0;
            double TotalCollectionamount = 0;
            //var ud = env.Select(d => new
            //{
            //    d.ReportDate.Year,
            //    d.ReportDate.Month,
            //    FormattedDate = d.ReportDate.ToString("yyyy-MMM")
            //})
            //.Distinct()
            //.OrderByDescending(d => d.Year)
            //.ThenByDescending(d => d.Month)
            //.Select(d => d.FormattedDate);
            //List<DisplayInvoiceTrans> model = new List<DisplayInvoiceTrans>();
            foreach (var invoice in walInv.OrderByDescending(d => d.PaymentDate))
            {
                //var invuser = await _unitOfWork.User.GetSysUsers(invoice.u.CreatedBy.ToString());

                //                select I.AlternateReferenceNo AS VendorReference,I.ReferenceNo AS EgoleReference,
                // CONCAT(U.FirstName, ' - ', U.LastName) AS Accountholder, U.Email, (select WalletAccountNumber from EGS_Wallet where WalletID = U.WalletID) As AccountNumber
                //     , I.Amount, I.SurCharge ,ISNULL(I.PhoneNumber, '-') AS PhoneNumber, I.ServiceNumber as Product,
                //FORMAT(i.PaymentDate, 'dd-MMM-yyyy') AS PaymentDate
                // from EGS_Invoice I inner
                // join
                //SYS_Users U on u.UserID = i.CreatedBy
                // where InvoiceID in  
                // (select InvoiceID from EGS_WalletTransaction where ProductID = 49--TransactionMethodID in (20, 22)
                // and Convert(date, TransactionDate)>= Convert(date, '2023-03-13') )
                
                // var transactionmethodN = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(Convert.ToInt32(invoice.TransactionMethod));
                var username = await _unitOfWork.User.GetUserByID(invoice.CreatedBy);
                model.Add(new TransactionsModel
                {
                    WalletReferenceNumber = invoice.WalletReferenceNumber,//EgoleReference
                    TransactionReferenceNo = invoice.TransactionReferenceNo,//VendorReference
                    TransactionType = invoice.TransactionType,
                    //ServiceNumber = invoice.ServiceNumber,
                    TransactionMethod = invoice.TransactionMethod,
                    TransactionDescription = invoice.TransactionDescription,
                    Amount = invoice.Amount,
                    Previous = invoice.Previous,
                    Current = invoice.Current,
                    SurCharge = invoice.SurCharge,
                    CreatedByName = username.FirstName + " " + username.LastName,//Accountholder
                    DateCreated = invoice.DateCreated,//PaymentDate
                    ProductItem = invoice.ProductItem,//Product
                    strPaymentDate = invoice.PaymentDate.ToString("ddd, dd MMM yyy hh:mm tt"),
                    //DatePaid = invoice.TransactionDate, //invoice.PaymentDate.ToString(),//.ToString("dd/MMMM/yyyy"),
                    PaymentStatus = invoice.PaymentStatus,
                    Region = invoice.Region,
                    Email = invoice.Email,
                    AccountNumber = invoice.AccountNumber
                });
                baseamount += invoice.Amount;
            }
            //model = (List<TransactionsModel>)model.OrderByDescending(d => d.PaymentDate)
            //    .ThenByDescending(d => d.PaymentDate);

            var modelR = new HoldTransactionsViewModel
            {
                HoldAllTransactionsDetails = model//.Distinct()
            };
            ViewBag.baseamount = baseamount;
            return View(modelR);

        }


        public async Task<IActionResult> SettlementDetailsReports(int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetMerchantSysUsers(user.UserID);
            var roleName = user2.Role.RoleName;

            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.SettlementDayStart = startDate.Day;
            ViewBag.SettlementMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.SettlementYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.SettlementDayEnd = endDate.Day;
            ViewBag.SettlementMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.SettlementYearEnd = endDate.Year;
              
            List<SettlementDetailsViewModel> model = new List<SettlementDetailsViewModel>();
            var Merchantdetailsettlements = await _unitOfWork.MerchantReport.GetSettlementDetailsByDateRange(startDate, endDate);
            double baseamount = 0;
            double TotalCollectionamount = 0;

            foreach (var settlement in Merchantdetailsettlements)
            {
                var productname = await _unitOfWork.Products.GetProducts(settlement.Product.ProductID);
                var productitemname = await _unitOfWork.ProductItem.GetProductItems(settlement.ProductItem.ProductItemID);
                var settlementIntervalName = await _unitOfWork.SettlementInterval.GetSettlementIntervalById(settlement.SettlementIntervalID);
                var settlementTypeName = await _unitOfWork.SettlementType.GetSettlementTypeByID(settlement.SettlementType.SettlementTypeID);
                var settlementModeName = await _unitOfWork.SettlementMode.GetSettlementModeByID(settlement.SettlementMode.SettlementModeID);
                var bankName = await _unitOfWork.Bank.GetBanks(settlement.Bank.BankID);

                model.Add(new SettlementDetailsViewModel
                {
                    SettlementLogID = settlement.SettlementLogID,
                    //ProductItemID = settlement.ProductItem.ProductItemID,
                    //ProductID = settlement.Product.ProductID,
                    ProductItemName = productitemname.ProductItemName,
                    ProductName = productname.ProductName,
                    Totalcollection = settlement.Totalcollection,
                    //SettlementIntervalID = settlement.SettlementIntervalID,
                    SettlementIntervalName = settlementIntervalName.SettlementIntervalName,
                    //MerchantID = settlement.Merchant.MerchantID,
                    Amount = settlement.Amount,
                    //SettlementTypeID = settlement.SettlementType.SettlementTypeID,
                    SettlementTypeName = settlementTypeName.SettlementType,
                    ExactSettlementDate = settlement.DateCreated,
                    IsPaid = settlement.IsPaid,
                    MerchantAccountNo = settlement.MerchantAccountNo,
                    //SalesID = settlement.SalesID,
                    SettlementModeName = settlementModeName.SettlementModeName,
                    //SettlementModeID =settlement.SettlementMode.SettlementModeID,
                    //BankID = settlement.Bank.BankID ,
                    BankName = bankName.BankName
                }); 
                //model.Add(new SettlementDetailsViewModel
                //{
                //    SettlementLogID = settlement.SettlementLogID,
                //    ProductItemID = settlement.ProductItem.ProductItemID,
                //    ProductID = settlement.Product.ProductID,
                //    Totalcollection = settlement.Totalcollection,
                //    SettlementIntervalID = settlement.SettlementIntervalID,
                //    MerchantID = settlement.Merchant.MerchantID,
                //    Amount = settlement.Amount,
                //    SettlementTypeID = settlement.SettlementType.SettlementTypeID,
                //    ExactSettlementDate = settlement.DateCreated,
                //    IsPaid = settlement.IsPaid,
                //    MerchantAccountNo = settlement.MerchantAccountNo,
                //    SalesID = settlement.SalesID,
                //    SettlementModeID = settlement.SettlementMode.SettlementModeID,
                //    BankID = settlement.Bank.BankID
                //});
                baseamount += settlement.Amount;
                TotalCollectionamount += settlement.Totalcollection;
            }
            var modelR = new HoldSettlementDetailsViewModel
            {
                HoldAllSettlementDetails = model
            };
            ViewBag.baseamount = baseamount;
            ViewBag.TotalCollectionamount = TotalCollectionamount;
            return View(modelR);
             
        }







        public static string GetMonthInWord(int month)
        {
            var monthNames = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            int getMonth = month;

            return $"{monthNames[getMonth - 1] }";
        }


    }
}
