using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.Reports.Models;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Controllers
{
    [Area("Reports")]
    [AllowAnonymous]
    public class AggregatorController : Controller
    {
        #region repositories
        private IUnitOfWork _unitOfWork;
        #endregion
        private readonly ILogger<dynamic> _logger;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;
        public AggregatorController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _roleManager = roleManager;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}


        public async Task<List<AgguserListVM>> UsersList()
        {

            List<SysUsers> userList = new List<SysUsers>();

            userList = await _unitOfWork.User.GetLSysUserByRole(3);

            List<AgguserListVM> muList = new List<AgguserListVM>();
            foreach (var agent in userList)
            {
                muList.Add(new AgguserListVM()
                {
                    AggregatorID = Convert.ToInt32(agent.AggregatorID),
                    //Email = agent.Agent.Email,
                    Name = agent.FirstName + " " + agent.LastName + " (" + Helpers.General.Mask(agent.Wallet.WalletAccountNumber, 0, 6, 'X') + ")"//+ "(" + Helpers.General.MaskString(agent.Wallet.WalletAccountNumber, 3, "****") + ")"
                }); ;

                //mList.Insert(0, new AggregatorAgentListVM { UserId = 0, Email = "--Select Agent  --" });
            }

            muList.Insert(0, new AgguserListVM { AggregatorID = 0, Name = "--Select Name --" });

            return muList;

        }

        public async Task<List<DDListView>> NasProductsList()
        {
            List<DDListView> ProductList = new List<DDListView>();

            //var products = await _unitOfWork.Products.GetProductListByCategoryID(14); //14--5

            var products = await _unitOfWork.Products.GetProductNasarawaByCategoryID();

            foreach (var product in products)
            {
                ProductList.Add(new DDListView
                {
                    itemValue = product.ProductID,
                    itemName = product.ProductName
                });
            }

            DropDownModelView nproductlist = new DropDownModelView();
            nproductlist.items = ProductList;

            return nproductlist.items;
        }



        [HttpGet]
        public async Task<IActionResult> AggregatorReport()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());


            if (user2.Role.RoleID == 2 || user2.Role.RoleID == 12 || user2.Role.RoleID == 13)
            {
                ViewBag.role = user2.Role.RoleID;
                ViewBag.NProductList = await NasProductsList();
                ViewBag.aggregators = await UsersList();

                return View();
            }

            ViewBag.aggregators = await UsersList();
           
            ViewBag.role = user2.Role.RoleID;
            List<EgsAggregatorRequest> aggList = new List<EgsAggregatorRequest>();
            aggList = await _unitOfWork.AggregatorRequest.GetProcessedAggregatorRequest(Convert.ToInt32(user2.AggregatorID));


            //GetAggregatorFromTblById(int Id)
            var getAggagents = await _unitOfWork.AggregatorRequest.GetAggregatorFromTblById(Convert.ToInt32(user2.AggregatorID));

            List<AggregatorAgentListVM> mList = new List<AggregatorAgentListVM>();
            foreach (var agent in aggList)
            {
                mList.Add(new AggregatorAgentListVM()
                {
                    UserId = agent.Agent.UserID,
                    //Email = agent.Agent.Email,
                    Name = agent.Agent.FirstName + " " + agent.Agent.LastName
                });

                //mList.Insert(0, new AggregatorAgentListVM { UserId = 0, Email = "--Select Agent  --" });
            }

            mList.Add(new AggregatorAgentListVM()
            {
                UserId = getAggagents.User.UserID,
                Name = getAggagents.User.FirstName + " " + getAggagents.User.LastName
            });

            ViewBag.Agents = mList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AggregatorReport(AggregatorReportModel reportModel)
        {
            if (ModelState.IsValid)
            {
                // return RedirectToAction("AgentReports", new { startDay = reportModel.StartDate.Day, startMonth = reportModel.StartDate.Month, startYear = reportModel.StartDate.Year, endDay = reportModel.EndDate.Day, endMonth = reportModel.EndDate.Month, endYear = reportModel.EndDate.Year });
                return RedirectToAction("AggregatorReports", reportModel);

            }
            return View();
        }

        public async Task<IActionResult> AggregatorReports(int? pageIndex, AggregatorReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var roleName = user2.Role.RoleName;

            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;
            int AgentId = Convert.ToInt32(reportModel.UserID);
            var agenuser = await _unitOfWork.User.GetSysUsers(reportModel.UserID);
            ViewBag.agenuser = agenuser.FirstName + " " + agenuser.LastName;

            List<AggregatorRViewModel> model = new List<AggregatorRViewModel>();
            IEnumerable<EgsSales> sales;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "AggregatorReports",
                Description = user2.FirstName + "" + user2.LastName + " View Aggregator Sales Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (roleName == "Aggregator")
            {
                //var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
                //ViewBag.aggcode = aggcode.AggregatorCode;
                sales = await _unitOfWork.AggregatorReport.GetSalesByDateRange(startDate, endDate, AgentId);


                //int AgentId = Convert.ToInt32(reportModel.UserID);

                //var getAggagents = await _unitOfWork.AggregatorRequest.GetProcessedAggregatorRequest(Convert.ToInt32(user2.AggregatorID));
                //List<string> IDList = new List<string>();
                //foreach (var getag in getAggagents)
                //{ 
                //    IDList.Add(getag.Agent.UserID.ToString());
                //    IDList.Add(user2.UserID.ToString()); 
                //} 


                //sales = await _unitOfWork.AggregatorReport.GetAllSalesByDateRange(startDate, endDate, IDList);


                double baseamounts = 0;
                double comisionamount = 0;
                foreach (var sale in sales)
                {
                    var getProductItem = await _unitOfWork.ProductItem.GetProductItems(sale.ProductItem.ProductItemID);
                    //var getProduct = await _unitOfWork.Products.GetProductExtension(sale.Product.ProductID);
                    //var commsn = (0.1 * sale.DiscountedAmount);
                    model.Add(new AggregatorRViewModel
                    {
                        WalletTransactionID = Convert.ToInt32(sale.WalletTransactionID),
                        DiscountedAmount = sale.DiscountedAmount,
                        TransactionReferenceNumber = sale.TransactionReferenceNumber,
                        //CreatedBy = sale.CreatedBy,
                        DateCreated = sale.DateCreated,
                        SettlementDate = sale.SettlementDate,
                        ProductItemName = sale.ProductItem.ProductItemName, //getProductItem.ProductItemName,
                        ProductName = sale.Product.ProductName,
                        Commission = getProductItem.CommisionAmount //sale.CommisionAmount //commsn
                    });
                    baseamounts += sale.DiscountedAmount;
                    comisionamount += getProductItem.CommisionAmount;//commsn;
                }

                var modelRs = new HoldSalesWViewModel
                {
                    HoldWAllSales = model
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                ViewBag.baseamount = baseamounts;
                ViewBag.camount = comisionamount;
                return View(modelRs);
            }
            return RedirectToAction("AggregatorReport");
        }


        public async Task<IActionResult> AggregatorAllAgents(int? pageIndex, AggregatorReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var roleName = user2.Role.RoleName;
            //var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
            //ViewBag.aggcode = aggcode.AggregatorCode;
            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;

            List<AggregatorRViewModel> model = new List<AggregatorRViewModel>();
            IEnumerable<EgsSales> sales;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "AggregatorAllAgents",
                Description = user2.FirstName + "" + user2.LastName + " View Commission By Aggregator Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (roleName == "Aggregator")
            {
                var getAggagents = await _unitOfWork.AggregatorRequest.GetProcessedAggregatorRequest(Convert.ToInt32(user2.AggregatorID));
                List<string> IDList = new List<string>();
                var idList = string.Empty;
                foreach (var getag in getAggagents)
                {
                    IDList.Add(getag.Agent.UserID.ToString()) /*+',')*/;
                    idList = idList + getag.Agent.UserID.ToString() + ',';
                }
                IDList.Add(user2.UserID.ToString());
                idList = idList + user2.UserID.ToString();

                var invoices = await GeneralSqlClient.RPT_AggregatorCommission(startDate, endDate, idList, "Report By Aggregator Commission");

                double baseamounts = 0;
                double comisionamount = 0;
                var iquery = invoices.Where(inv => inv.Amount > 0).GroupBy(g => new
                {
                    V = g.AlternateReferenceNo,        //g.Product.ProductName,
                    Nam = g.WalletInfo,//g.ProductItemID, 
                    invRef = g.AlternateReferenceNo,
                    payRef = g.PaymentReference,
                    payDate = g.PaymentDate.Date,
                    prcat = g.ProductItemCategory,
                    pritem = g.productItemName,
                    comAmt = g.Commisionamount,
                }).Select(group => new
                {
                    invRef = group.Key.V,
                    acctInfo = group.Key.Nam,
                    prcat = group.Key.prcat,
                    pritem = group.Key.pritem,
                    AlternateReferenceNo = group.Key.invRef,
                    ReferenceNo = group.Key.payRef,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.InvAmount),
                    ComAmount = group.Key.comAmt,
                    ItemCount = group.Count(),
                });

                foreach (var invoice in iquery)
                {
                    var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
                    model.Add(new AggregatorRViewModel
                    {
                        WalletInfo = invoice.acctInfo,
                        ItemCategory = invoice.prcat,
                        ProductItemName = invoice.pritem,
                        InvRefNum = invoice.AlternateReferenceNo,
                        TransCount = invoice.ItemCount,
                        PayRefNum = invoice.ReferenceNo,
                        Amount = invoice.Amount, //(invoice.ItemCount * 200),
                        ComAmount = invoice.ComAmount,//ItemCatCom.CommisionAmount,
                        DateCreated = invoice.PayDate,
                        //commission = (0.1 * (invoice.Amount - (invoice.ItemCount * 200)))
                    });
                    baseamounts += invoice.Amount;
                    comisionamount += invoice.ComAmount;
                }

                var modelRs = new HoldSalesWViewModel
                {
                    HoldWAllSales = model
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                ViewBag.baseamount = baseamounts.ToString("N", CultureInfo.InvariantCulture);
                ViewBag.camount = comisionamount.ToString("N", CultureInfo.InvariantCulture);

                return View(modelRs);
            }

            return RedirectToAction("AggregatorReport");
        }



        //public async Task<IActionResult> AggregatorAllAdmin(int? pageIndex, AggregatorReportModel reportModel)
        //{
        //    int startDay = reportModel.StartDate.Day;
        //    int startMonth = reportModel.StartDate.Month;
        //    int startYear = reportModel.StartDate.Year;
        //    int endDay = reportModel.EndDate.Day;
        //    int endMonth = reportModel.EndDate.Month;
        //    int endYear = reportModel.EndDate.Year;

        //    var user = await _userManager.GetUserAsync(User);
        //    var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
        //    var roleName = user2.Role.RoleName;
        //    //var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
        //    //ViewBag.aggcode = aggcode.AggregatorCode;
        //    //Construct new Date from parameters
        //    DateTime startDate = new DateTime(startYear, startMonth, startDay);
        //    DateTime endDate = new DateTime(endYear, endMonth, endDay);

        //    //ViewBags for StartDate
        //    ViewBag.OfferingDayStart = startDate.Day;
        //    ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
        //    ViewBag.OfferingYearStart = startDate.Year;

        //    //ViewBags for EndDate
        //    ViewBag.OfferingDayEnd = endDate.Day;
        //    ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
        //    ViewBag.OfferingYearEnd = endDate.Year;

        //    List<AggregatorRViewModel> model = new List<AggregatorRViewModel>();
        //    List<SettlementRViewModel> bmodel = new List<SettlementRViewModel>();
        //    IEnumerable<EgsSales> sales;
        //    if (roleName == "Super Admin")
        //    { 
        //        if (Convert.ToInt32(reportModel.AggID) != 0)
        //        {

        //        }
        //            var getagguser = await _unitOfWork.Aggregator.GetAggregatorById(Convert.ToInt32(reportModel.AggID));
        //        ViewBag.aggname = getagguser.FirstName + " " + getagguser.LastName;
        //        var getAggagents = await _unitOfWork.AggregatorRequest.GetProcessedAggregatorRequest(Convert.ToInt32(reportModel.AggID));
        //        List<string> IDList = new List<string>();
        //        var idList = string.Empty;
        //        foreach (var getag in getAggagents)
        //        {
        //            IDList.Add(getag.Agent.UserID.ToString()) /*+',')*/;
        //            idList = idList + getag.Agent.UserID.ToString() + ',';
        //        }

        //        IDList.Add(getagguser.UserID.ToString());
        //        idList = idList + getagguser.UserID.ToString();

        //        var invoices = await GeneralSqlClient.RPT_AggregatorCommission(startDate, endDate, idList, "Report By Aggregator Commission");

        //        double baseamounts = 0;
        //        double comisionamount = 0;
        //        var iquery = invoices.Where(inv => inv.Amount > 0).GroupBy(g => new
        //        {
        //            V = g.AlternateReferenceNo,        //g.Product.ProductName,
        //            Nam = g.WalletInfo,//g.ProductItemID, 
        //            invRef = g.AlternateReferenceNo,
        //            payRef = g.PaymentReference,
        //            payDate = g.PaymentDate.Date,
        //            prcat = g.ProductItemCategory,
        //            pritem = g.productItemName,
        //            comAmt = g.Commisionamount,
        //        }).Select(group => new
        //        {
        //            invRef = group.Key.V,
        //            acctInfo = group.Key.Nam,
        //            prcat = group.Key.prcat,
        //            pritem = group.Key.pritem,
        //            AlternateReferenceNo = group.Key.invRef,
        //            ReferenceNo = group.Key.payRef,
        //            PayDate = group.Key.payDate,
        //            Amount = group.Sum(a => a.InvAmount),
        //            ComAmount = group.Key.comAmt,
        //            ItemCount = group.Count(),
        //        });

        //        foreach (var invoice in iquery)
        //        {
        //            //var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
        //            model.Add(new AggregatorRViewModel
        //            {
        //                WalletInfo = invoice.acctInfo,
        //                ItemCategory = invoice.prcat,
        //                ProductItemName = invoice.pritem,
        //                InvRefNum = invoice.AlternateReferenceNo,
        //                TransCount = invoice.ItemCount,
        //                PayRefNum = invoice.ReferenceNo,
        //                Amount = invoice.Amount, //(invoice.ItemCount * 200),
        //                ComAmount = invoice.ComAmount,//ItemCatCom.CommisionAmount,
        //                DateCreated = invoice.PayDate,
        //                //commission = (0.1 * (invoice.Amount - (invoice.ItemCount * 200)))
        //            });
        //            baseamounts += invoice.Amount;
        //            comisionamount += invoice.ComAmount;
        //        }

        //        var modelRs = new HoldSalesWViewModel
        //        {
        //            HoldWAllSales = model
        //            //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
        //            //Pager = pager
        //        };
        //        ViewBag.baseamount = baseamounts.ToString("N", CultureInfo.InvariantCulture);
        //        ViewBag.camount = comisionamount.ToString("N", CultureInfo.InvariantCulture);

        //        return View(modelRs);
        //    }

        //    return RedirectToAction("AggregatorReport");
        //}


        public async Task<IActionResult> AggregatorAllAdmin(int? pageIndex, AggregatorReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var roleName = user2.Role.RoleName;
            //var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
            //ViewBag.aggcode = aggcode.AggregatorCode;
            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;

            string idbusiness = reportModel.AggID;

            List<AggregatorRViewModel> model = new List<AggregatorRViewModel>();
            List<SettlementRViewModel> bmodel = new List<SettlementRViewModel>();
            IEnumerable<EgsSales> sales;

            double baseamounts = 0;
            double comisionamount = 0;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "AdminCommissionReport",
                Description = user2.FirstName + "" + user2.LastName + " View Aggregator Commission Summary Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (roleName == "Super Admin" || roleName == "Operations" || roleName == "Customer Reconcilation")
            {
                if (Convert.ToInt32(reportModel.AggID) == 0)
                {
                    idbusiness = "-1";
                    var invoicees = await GeneralSqlClient.RPT_Transaction(startDate, endDate, "", "SUMMARY BY AGGREGATOR COMMISSION", user2.UserID.ToString(), idbusiness);

                    foreach (var invoice in invoicees)
                    {
                        model.Add(new AggregatorRViewModel
                        {
                            Business = invoice.Outlet,
                            Commission = invoice.commission,
                            Period = invoice.CommissionPeriod
                        });
                        comisionamount += invoice.commission;
                    }

                    var modelRes = new HoldSalesWViewModel
                    {
                        HoldWAllSales = model
                    };
                    //ViewBag.baseamount = baseamounts.ToString("N", CultureInfo.InvariantCulture);
                    ViewBag.camount = comisionamount.ToString("N", CultureInfo.InvariantCulture);

                    return View(modelRes);
                }
 
                var invoices = await GeneralSqlClient.RPT_Transaction(startDate, endDate, "", "SUMMARY BY AGGREGATOR COMMISSION", "", idbusiness);
                 
                foreach (var invoice in invoices)
                {
                    //var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
                    model.Add(new AggregatorRViewModel
                    {
                        Business = invoice.Outlet,
                        Commission = invoice.commission,
                        Period = invoice.CommissionPeriod
                        //commission = (0.1 * (invoice.Amount - (invoice.ItemCount * 200)))
                    });
                    //baseamounts += invoice.Amount;
                    comisionamount += invoice.commission;
                }

                var modelRs = new HoldSalesWViewModel
                {
                    HoldWAllSales = model
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                //ViewBag.baseamount = baseamounts.ToString("N", CultureInfo.InvariantCulture);
                ViewBag.camount = comisionamount.ToString("N", CultureInfo.InvariantCulture);

                return View(modelRs);
            }

            if (roleName == "Aggregator")
            { 
                    idbusiness = user2.AggregatorID.ToString();
                    var invoicees = await GeneralSqlClient.RPT_Transaction(startDate, endDate, "", "SUMMARY BY AGGREGATOR COMMISSION","" , idbusiness);

                    foreach (var invoice in invoicees)
                    {
                        model.Add(new AggregatorRViewModel
                        {
                            Business = invoice.Outlet,
                            Commission = invoice.commission,
                            Period = invoice.CommissionPeriod
                        });
                        comisionamount += invoice.commission;
                    }

                    var modelRes = new HoldSalesWViewModel
                    {
                        HoldWAllSales = model
                    };
                    //ViewBag.baseamount = baseamounts.ToString("N", CultureInfo.InvariantCulture);
                    ViewBag.camount = comisionamount.ToString("N", CultureInfo.InvariantCulture);

                    return View(modelRes); 
            }


            return RedirectToAction("AggregatorReport");
        }



        [HttpGet]
        public async Task<IActionResult> AggregatorSummaryReport()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
            //ViewBag.aggcode = aggcode.AggregatorCode;
            List<EgsAggregatorRequest> aggList = new List<EgsAggregatorRequest>();

            aggList = await _unitOfWork.AggregatorRequest.GetProcessedAggregatorRequest(Convert.ToInt32(user2.AggregatorID));
            if (aggList.Count == 0)
            {
                List<AggregatorAgentListVM> mList = new List<AggregatorAgentListVM>();
                mList.Add(new AggregatorAgentListVM()
                {
                    UserId = 0,
                    Email = ""
                });
                //mList.Insert(0, new AggregatorAgentListVM { UserId = 0, Email = "" });
                ViewBag.Agents = mList;
                return View();
            }

            foreach (var agent in aggList)
            {
                List<AggregatorAgentListVM> mList = new List<AggregatorAgentListVM>();
                mList.Add(new AggregatorAgentListVM()
                {
                    UserId = agent.Agent.UserID,
                    Email = agent.Agent.Email
                });
                mList.Insert(0, new AggregatorAgentListVM { UserId = 0, Email = "--Select Agent  --" });
                ViewBag.Agents = mList;
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AgentswalletBalance()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            List<EgsAggregatorRequest> aggList = new List<EgsAggregatorRequest>();
            aggList = await _unitOfWork.AggregatorRequest.GetProcessedAggregatorRequest(Convert.ToInt32(user2.AggregatorID));

            var getAggagents = await _unitOfWork.AggregatorRequest.GetAggregatorFromTblById(Convert.ToInt32(user2.AggregatorID));

            List<AggregatorAgentsbalanceVM> mList = new List<AggregatorAgentsbalanceVM>();
            foreach (var agent in aggList)
            {
                // var agenbalance = await _unitOfWork.Invoice.GetWalletBalance(agent.Agent.Wallet.WalletID);
                var agenbalance = await _unitOfWork.Invoice.GetNWalletBalance(agent.Agent.Wallet.WalletID);
                mList.Add(new AggregatorAgentsbalanceVM()
                {
                    UserId = agent.Agent.UserID,
                    Email = agent.Agent.Email,
                    Name = agent.Agent.FirstName + " " + agent.Agent.LastName,
                    Balance = agenbalance.WalletBalance
                });

            }

            var modelR = new HoldAggregatorAgentsbalanceVM
            {
                HoldAllBalance = mList
            };
            return View(modelR);

        }


        public async Task<IActionResult> AdminCommissionReport(AgentReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var roleName = user2.Role.RoleName;

            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;

            List<SettlementRViewModel> model = new List<SettlementRViewModel>();
            LSettlementRViewModel model_ = new LSettlementRViewModel();

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "AdminCommissionReport",
                Description = user2.FirstName + "" + user2.LastName + " View Reference Summary Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (roleName == "Super Admin")
            {
                var invoices = await GeneralSqlClient.RPT_Transaction(startDate, endDate, reportModel.ProductID, "Report By Reference Summary");
                var iquery = invoices.GroupBy(g => new
                {
                    V = g.ProductName,
                    winfo = g.WalletInfo,
                    prcat = g.ProductItemCategory,
                    invref = g.ReferenceNumber,
                    pyref = g.PaymentReference,
                }).Select(group => new
                {
                    pName = group.Key.V,
                    walinfo = group.Key.winfo,
                    prcat = group.Key.prcat,
                    invref = group.Key.invref,
                    pyref = group.Key.pyref,
                    Amount = group.Sum(a => a.Amount),
                    ItemCount = group.Count(),
                });

                foreach (var invoice in iquery)
                {

                    model.Add(new SettlementRViewModel
                    {
                        ProductName = invoice.pName,
                        WalletInfo = invoice.walinfo,
                        ReferenceNumber = invoice.invref,
                        PaymentReference = invoice.pyref,
                        ProductItemCategory = invoice.prcat,
                        ItemCount = invoice.ItemCount,
                        transactioncvalue = invoice.Amount,

                    });

                }

                //var pager = new Pager(invoices.Count(), pageIndex);
                var modelRs = new HoldSettlementRViewModel
                {
                    HoldAllSettlement = model
                };

                return View(modelRs);
            }

            return View();
        }


        public async Task<IActionResult> AllAggregatorSummary(AgentReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var roleName = user2.Role.RoleName;
            //var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
            //ViewBag.aggcode = aggcode.AggregatorCode;
            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;

            List<AggregatorRViewModel> model = new List<AggregatorRViewModel>();
            List<SettlementRViewModel> bmodel = new List<SettlementRViewModel>();
            IEnumerable<EgsSales> sales;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "AllAggregatorSummary",
                Description = user2.FirstName + "" + user2.LastName + " View Aggregator Commission Summary Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (roleName == "Super Admin")
            {

                var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "SUMMARY BY AGGREGATOR COMMISSION", "", "");


                foreach (var invo in invoices_)
                {
                    //var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
                    bmodel.Add(new SettlementRViewModel
                    {
                        Outlet = invo.Outlet,
                        Period = invo.Period,
                        AgentsCounts = invo.AgentsCounts,
                        commission = invo.commission,
                        Aggregator = invo.Aggregator



                        //Outlet = invo.Outlet,
                        //Agent = invo.Agent,
                        //ItemCount = invo.ItemCount,
                        //commission = invo.commission,
                        //Aggregator = invo.Aggregator,

                    });
                }

                var modelRs_ = new HoldSettlementRViewModel        //HoldSalesWViewModel
                {
                    HoldAllSettlement = bmodel
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                return View(modelRs_);
            }
            if (roleName == "Aggregator")
            {
             var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY ITEM", "", user2.AggregatorID.ToString());


                foreach (var invo in invoices_)
                { 
                    bmodel.Add(new SettlementRViewModel
                    {
                        Outlet = invo.Outlet,
                        //Agent = invo.Agent,
                        AgentsCounts = invo.AgentsCounts,
                        commission = invo.commission,
                        Aggregator = invo.Aggregator,
                        ItemCount = invo.ItemCount,
                        Item = invo.Item, 
                        CommissionPeriod = invo.CommissionPeriod

                    });
                }

                var modelRs_ = new HoldSettlementRViewModel         
                {
                    HoldAllSettlement = bmodel 
                };
                return View(modelRs_);
            }
            return RedirectToAction("AggregatorReport");
        }



        public async Task<IActionResult> AllAggregatorsSummary(AggregatorReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var roleName = user2.Role.RoleName;
            //var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
            //ViewBag.aggcode = aggcode.AggregatorCode;
            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;

            string idbusiness = reportModel.AggID;
            List<AggregatorRViewModel> model = new List<AggregatorRViewModel>();
            List<SettlementRViewModel> bmodel = new List<SettlementRViewModel>();
            IEnumerable<EgsSales> sales;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "AllAggregatorsSummary",
                Description = user2.FirstName + "" + user2.LastName + " View Aggregator Commission Summary by Item Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (roleName == "Super Admin"|| roleName == "Operations" || roleName == "Customer Reconcilation")
            {
                var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY ITEM", "", idbusiness);

                foreach (var invo in invoices_)
                {
                    //var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
                    bmodel.Add(new SettlementRViewModel
                    {
                        Outlet = invo.Outlet,
                        Period = invo.Period,
                        ItemCount = invo.ItemCount,
                        Item = invo.Item,
                        AgentsCounts = invo.AgentsCounts,
                        commission = invo.commission,
                        //Aggregator = invo.Aggregator
                        CommissionPeriod = invo.CommissionPeriod
                    });
                }

                var modelRs_ = new HoldSettlementRViewModel        //HoldSalesWViewModel
                {
                    HoldAllSettlement = bmodel
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                return View(modelRs_);
            }


            if (Convert.ToInt32(reportModel.AggID) == 0)
            {
                idbusiness = "-1";
                var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY ITEM", user2.UserID.ToString(), idbusiness);

                foreach (var invo in invoices_)
                {
                    //var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
                    bmodel.Add(new SettlementRViewModel
                    {
                        Outlet = invo.Outlet,
                        Period = invo.Period,
                        ItemCount = invo.ItemCount,
                        Item = invo.Item,
                        AgentsCounts = invo.AgentsCounts,
                        commission = invo.commission,
                        //Aggregator = invo.Aggregator
                        CommissionPeriod = invo.CommissionPeriod
                    });
                }

                var modelRs_ = new HoldSettlementRViewModel        //HoldSalesWViewModel
                {
                    HoldAllSettlement = bmodel
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                return View(modelRs_);
            }

            return RedirectToAction("AggregatorReport");
        }



        public async Task<IActionResult> AggregatorAgentsSummary(AggregatorReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var roleName = user2.Role.RoleName;
            //var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
            //ViewBag.aggcode = aggcode.AggregatorCode;
            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;

            List<AggregatorRViewModel> model = new List<AggregatorRViewModel>();
            List<SettlementRViewModel> bmodel = new List<SettlementRViewModel>();
            IEnumerable<EgsSales> sales;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "AggregatorAgentsSummary",
                Description = user2.FirstName + "" + user2.LastName + " View Aggregator Commission Summary by Agent Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();


            if (roleName == "Super Admin")
            {
                var useragg = ""; var usersid = "";
                if (useragg != null)
                {
                    useragg = reportModel.AggID;
                    var agguserid = await _unitOfWork.Aggregator.GetAggregatorById(Convert.ToInt32(useragg));
                    usersid = Convert.ToString(agguserid.UserID);
                }                  //Convert.ToString(user2.UserID);
                var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY AGENT ", usersid, useragg);

                //var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY AGENT ","","");
                foreach (var invo in invoices_)
                {
                    //var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
                    bmodel.Add(new SettlementRViewModel
                    {
                        Outlet = invo.Outlet,
                        Agent = invo.Agent,
                        ItemCount = invo.ItemCount,
                        commission = invo.commission,
                        Aggregator = invo.Aggregator,
                    });
                }

                var modelRs_ = new HoldSettlementRViewModel        //HoldSalesWViewModel
                {
                    HoldAllSettlement = bmodel
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                return View(modelRs_);
            }
            if (roleName == "Aggregator")
            {
                //    var useragg = Convert.ToString(user2.AggregatorID);
                //    var usersid = Convert.ToString(user2.UserID);
                //    var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY AGENT ", usersid, useragg);

                //    foreach (var invo in invoices_)
                //    {
                //        //var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
                //        bmodel.Add(new SettlementRViewModel
                //        {
                //            Outlet = invo.Outlet,
                //            Agent=invo.Agent,
                //            //Period = invo.Period,
                //            //AgentsCounts = invo.AgentsCounts,
                //            ItemCount=invo.ItemCount,
                //            commission = invo.commission,
                //            Aggregator = invo.Aggregator,
                //        });
                //    }

                //    var modelRs_ = new HoldSettlementRViewModel        //HoldSalesWViewModel
                //    {
                //        HoldAllSettlement = bmodel
                //        //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                //        //Pager = pager
                //    };
                //    return View(modelRs_);
                //}
                //return RedirectToAction("AggregatorReport"); 

                List<string> IDList = new List<string>();
                var useridList = string.Empty;

                var getaggid = await _unitOfWork.User.GetUserByID(user2.UserID);
                int useragg = Convert.ToInt32(getaggid.AggregatorID);
                var agguserid = await _unitOfWork.AggregatorRequest.GetAgentstiedtoAggregator(useragg);
                var aggregatoruser = await _unitOfWork.Aggregator.GetAggregatorById(useragg);

                foreach (var _item in agguserid)
                {
                    useridList = useridList + _item.Agent.UserID + ',';
                }
                useridList = useridList + (Convert.ToString(aggregatoruser.UserID));

                var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY AGENT", "", useridList);

                foreach (var invo in invoices_)
                {
                    bmodel.Add(new SettlementRViewModel
                    {
                        Outlet = invo.Outlet,
                        Agent = invo.Agent,
                        ItemCount = invo.ItemCount,
                        commission = invo.commission,
                        CommissionPeriod = invo.CommissionPeriod,
                        Aggregator = invo.Aggregator,
                    });
                }

                var modelRs_ = new HoldSettlementRViewModel
                {
                    HoldAllSettlement = bmodel
                };
                return View(modelRs_); 
            }
            return RedirectToAction("AggregatorReport");
        }



        public async Task<IActionResult> AggregatorsAgentsSummary(AggregatorReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var roleName = user2.Role.RoleName;
            //var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
            //ViewBag.aggcode = aggcode.AggregatorCode;
            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;
            string idbusiness = reportModel.AggID;

            if (Convert.ToInt32(reportModel.AggID) == 0)
            {
                idbusiness = "-1";
            }

            List<AggregatorRViewModel> model = new List<AggregatorRViewModel>();
            List<SettlementRViewModel> bmodel = new List<SettlementRViewModel>();
            IEnumerable<EgsSales> sales;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "AggregatorsAgentsSummary",
                Description = user2.FirstName + "" + user2.LastName + " View Aggregator Commission Summary by Agent Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (roleName == "Super Admin"|| roleName == "Operations" || roleName == "Customer Reconcilation")
            {
                List<string> IDList = new List<string>();
                var useridList = string.Empty;
                int useragg = Convert.ToInt32(idbusiness);
                var agguserid = await _unitOfWork.AggregatorRequest.GetAgentstiedtoAggregator(useragg);
                var aggregatoruser = await _unitOfWork.Aggregator.GetAggregatorById(useragg);

                if (Convert.ToInt32(reportModel.AggID) == 0)
                {
                    useridList = "-1";
                    var invoicees_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY AGENT", user2.UserID.ToString(), useridList);

                    foreach (var invo in invoicees_)
                    {
                        //var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
                        bmodel.Add(new SettlementRViewModel
                        {
                            Outlet = invo.Outlet,
                            Agent = invo.Agent,
                            ItemCount = invo.ItemCount,
                            commission = invo.commission,
                            CommissionPeriod = invo.CommissionPeriod,
                            Aggregator = invo.Aggregator,
                        });
                    }

                    var modelRes_ = new HoldSettlementRViewModel        //HoldSalesWViewModel
                    {
                        HoldAllSettlement = bmodel
                        //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                        //Pager = pager
                    };
                    return View(modelRes_);
                }


                foreach (var _item in agguserid)
                {
                    useridList = useridList + _item.Agent.UserID + ',';
                }
                useridList = useridList + (Convert.ToString(aggregatoruser.UserID));

                var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY AGENT", "", useridList);

                foreach (var invo in invoices_)
                {
                    //var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
                    bmodel.Add(new SettlementRViewModel
                    {
                        Outlet = invo.Outlet,
                        Agent = invo.Agent,
                        ItemCount = invo.ItemCount,
                        commission = invo.commission,
                        CommissionPeriod = invo.CommissionPeriod,
                        Aggregator = invo.Aggregator,
                    });
                }

                var modelRs_ = new HoldSettlementRViewModel        //HoldSalesWViewModel
                {
                    HoldAllSettlement = bmodel
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                return View(modelRs_);
            }
            if (roleName == "Aggregator")
            {
                var useragg = Convert.ToString(user2.AggregatorID);
                var usersid = Convert.ToString(user2.UserID);
                var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY AGENT ", usersid, useragg);
                //var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY AGENT ");
                foreach (var invo in invoices_)
                {
                    //var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
                    bmodel.Add(new SettlementRViewModel
                    {
                        Outlet = invo.Outlet,
                        Agent = invo.Agent,
                        //Period = invo.Period,
                        //AgentsCounts = invo.AgentsCounts,
                        ItemCount = invo.ItemCount,
                        commission = invo.commission,
                        Aggregator = invo.Aggregator,
                    });
                }

                var modelRs_ = new HoldSettlementRViewModel        //HoldSalesWViewModel
                {
                    HoldAllSettlement = bmodel
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                return View(modelRs_);
            }
            return RedirectToAction("AggregatorReport");
        }



        public async Task<IActionResult> AggregatorsAgentWithItem(AggregatorReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var roleName = user2.Role.RoleName;
            //var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
            //ViewBag.aggcode = aggcode.AggregatorCode;
            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;
            string idbusiness = reportModel.AggID;

            if (Convert.ToInt32(reportModel.AggID) == 0)
            {
                idbusiness = "-1";
            }

            List<AggregatorRViewModel> model = new List<AggregatorRViewModel>();
            List<SettlementRViewModel> bmodel = new List<SettlementRViewModel>();
            IEnumerable<EgsSales> sales;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "AggregatorsAgentWithItem",
                Description = user2.FirstName + "" + user2.LastName + " View Aggregator Commission Summary by Agent With Item Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (roleName == "Super Admin"|| roleName == "Operations" || roleName == "Customer Reconcilation")
            {
                List<string> IDList = new List<string>();
                var useridList = string.Empty;

                if (Convert.ToInt32(reportModel.AggID) == 0)
                {
                    useridList = "-1";

                    var invoicese_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY AGENT WITH ITEM", user2.UserID.ToString(), useridList);

                    foreach (var invo in invoicese_)
                    {
                        bmodel.Add(new SettlementRViewModel
                        {
                            Outlet = invo.Outlet,
                            Aggregator = invo.Aggregator,
                            Agent = invo.Agent,
                            ItemCount = invo.ItemCount,
                            Item = invo.Item,
                            commission = invo.commission,
                            CommissionPeriod = invo.CommissionPeriod,
                        });
                    }

                    var modelRes_ = new HoldSettlementRViewModel
                    {
                        HoldAllSettlement = bmodel
                    };
                    return View(modelRes_);
                }
                int useragg = Convert.ToInt32(idbusiness);
                var agguserid = await _unitOfWork.AggregatorRequest.GetAgentstiedtoAggregator(useragg);
                var aggregatoruser = await _unitOfWork.Aggregator.GetAggregatorById(useragg);

                foreach (var _item in agguserid)
                {
                    useridList = useridList + _item.Agent.UserID + ',';
                }
                useridList = useridList + (Convert.ToString(aggregatoruser.UserID));

                var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY AGENT WITH ITEM", user2.UserID.ToString(), useridList);

                foreach (var invo in invoices_)
                {
                    bmodel.Add(new SettlementRViewModel
                    {
                        Outlet = invo.Outlet,
                        Aggregator = invo.Aggregator,
                        Agent = invo.Agent,
                        ItemCount = invo.ItemCount,
                        Item = invo.Item,
                        commission = invo.commission,
                        CommissionPeriod = invo.CommissionPeriod,

                    });
                }

                var modelRs_ = new HoldSettlementRViewModel
                {
                    HoldAllSettlement = bmodel
                };
                return View(modelRs_);
            }
            if (roleName == "Aggregator")
            {

                  List<string> IDList = new List<string>();
                var useridList = string.Empty;
                 
                int useragg = Convert.ToInt32(user2.AggregatorID);
                var agguserid = await _unitOfWork.AggregatorRequest.GetAgentstiedtoAggregator(useragg);
                var aggregatoruser = await _unitOfWork.Aggregator.GetAggregatorById(useragg);

                foreach (var _item in agguserid)
                {
                    useridList = useridList + _item.Agent.UserID + ',';
                }
                useridList = useridList + (Convert.ToString(aggregatoruser.UserID));

                 

                //var useragg = Convert.ToString(user2.AggregatorID);
                //var usersid = Convert.ToString(user2.UserID);
                var invoices_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "AGGREGATOR COMMISSION SUMMARY BY AGENT WITH ITEM", "", useridList);

                foreach (var invo in invoices_)
                {
                    //var ItemCatCom = await _unitOfWork.ProductItem.GetProductItemCategoriesByCat(invoice.prcat);
                    bmodel.Add(new SettlementRViewModel
                    {
                        Outlet = invo.Outlet,
                        Agent = invo.Agent,
                        //Period = invo.Period,
                        //AgentsCounts = invo.AgentsCounts,
                        Item = invo.Item,
                        CommissionPeriod = invo.CommissionPeriod,
                        ItemCount = invo.ItemCount,
                        commission = invo.commission,
                        Aggregator = invo.Aggregator,
                    });
                }

                var modelRs_ = new HoldSettlementRViewModel
                {
                    HoldAllSettlement = bmodel
                };
                return View(modelRs_);
            }
            return RedirectToAction("AggregatorReport");
        }

         
        public async Task<IActionResult> walletBalance()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());


            if (user2.Role.RoleID == 2 ||user2.Role.RoleID == 13 || user2.Role.RoleID == 12)
            {
                ViewBag.role = user2.Role.RoleID; 
                  
                return View();
            }
            return View();
        }

        public async Task<IActionResult> WalletBalanceReport(AggregatorReportModel reportModel)
        {

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            List<SysUsers> aggList = new List<SysUsers>();

            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            aggList = await _unitOfWork.User.GetActiveAgents();
             
            List<SettlementRViewModel> mList = new List<SettlementRViewModel>();

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "WalletBalanceReport",
                Description = user2.FirstName + "" + user2.LastName + " View Wallet Balance Report details between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            var invoicese_ = await GeneralSqlClient.RPT_Transaction(startDate, endDate, " ", "WALLET BALANCE", "", "");

            foreach (var invo in invoicese_)
            {
                mList.Add(new SettlementRViewModel
                {
                    //UserId = invo.UserID,
                    Email = invo.Email,
                    Company = invo.Company,
                    Name = invo.Agent + " (" + Helpers.General.MaskString(invo.AccountNumber, 3, "****") + ")",//invo.Agent,
                    LastName =invo.LastName,
                    Balance = invo.Balance
                });
            }





            //foreach (var agent in aggList)
            //{
            //    var agenbalance = await _unitOfWork.Invoice.GetWalletBalance(agent.Wallet.WalletID,reportModel.EndDate);
            //    mList.Add(new AggregatorAgentsbalanceVM()
            //    {
            //        UserId = agent.UserID,
            //        Email = agent.Email,
            //        Company=agent.CompanyName,
            //        Name = agent.FirstName + " " + agent.LastName,
            //        Balance = agenbalance.Sum(s => s.Amount)
            //    });

            //}

            var modelR = new HoldSettlementRViewModel        //HoldAggregatorAgentsbalanceVM
            {
                //HoldAllBalance = mList
                HoldAllSettlement=mList
            };
            return View(modelR); 
        }

        public async Task<IActionResult> OpeningBalanceReport(AggregatorReportModel reportModel)
        {

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            List<SysUsers> aggList = new List<SysUsers>();

            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);
             
            List<OpeningWalletViewModel> mList = new List<OpeningWalletViewModel>();

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "WalletBalanceReport",
                Description = user2.FirstName + "" + user2.LastName + " View Opening/closing Balance Report details between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            var invoicese_ = await GeneralSqlClient.RPT_ATransaction(endDate, endDate, "","OPENING CLOSING BALANCE","","");

            foreach (var invo in invoicese_)
            {
                mList.Add(new OpeningWalletViewModel
                {
                    BusinessName=invo.BusinessName,
                    Agent=invo.Agent,
                    OpeningBalance=invo.OpeningBalance,
                    ClosingBalance=invo.ClosingBalance,
                    WalletBalance=invo.WalletBalance
                });
            }
             
            var modelR = new HoldOpeningWalletViewModel        //HoldAggregatorAgentsbalanceVM
            {
                //HoldAllBalance = mList
                HoldAllSettlement = mList
            };
            return View(modelR);
        }


        //public async Task<IActionResult> AggregatorAllAgents()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

        //    List<EgsAggregatorRequest> aggList = new List<EgsAggregatorRequest>();
        //    aggList = await _unitOfWork.AggregatorRequest.GetProcessedAggregatorRequest(Convert.ToInt32(user2.AggregatorID));

        //    foreach (var agent in aggList)
        //    {
        //        List<AggregatorAgentListVM> mList = new List<AggregatorAgentListVM>();
        //        mList.Add(new AggregatorAgentListVM()
        //        {
        //            UserId = agent.Agent.UserID,
        //            Email = agent.Agent.Email
        //        });
        //        mList.Insert(0, new AggregatorAgentListVM { UserId = 0, Email = "--Select Agent  --" });
        //        ViewBag.Agents = mList;




        //    }


        //    return View( );
        //}










        public static string GetMonthInWord(int month)
        {
            var monthNames = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            int getMonth = month;

            return $"{monthNames[getMonth - 1] }";
        }

















    }
}
