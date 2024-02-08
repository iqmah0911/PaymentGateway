using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.Reports.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PaymentGateway21052021.Areas.SysSetup.Models;

namespace PaymentGateway21052021.Areas.Reports.Controllers
{
    [Area("Reports")]
    //[Authorize(Roles = "Agent")]
    [AllowAnonymous]
    public class AgentsController : Controller
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

        public AgentsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            //_mailSender = emailSender;
            _roleManager = roleManager;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> AgentReport(int? id)
        {
            //var userSAD = await SADList();
            //var userSAD = await AWalletList();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            ViewBag.role = user2.Role.RoleID;
            ViewBag.Product = await ProductList();
            ViewBag.ProductItem = await ProductItemList();
            ViewBag.ProductItemCategory = await ProductItemCategory();
            if (user2.Role.RoleID == 2)
            {
                ViewBag.Agents = await ADAgentList(); 
            }
            else
            {
                ViewBag.Agents = await AgentList(); 
            }

           // ViewBag.Agents = await AgentList();

            if (id == 1)
            {
                //return RedirectToAction("AgentReports", new { startDay = DateTime.Now.Day, startMonth = DateTime.Now.Month, startYear = DateTime.Now.Year, endDay = DateTime.Now.Day, endMonth = DateTime.Now.Month, endYear = DateTime.Now.Year });
                return RedirectToAction("AgentReports", new AgentReportModel { StartDate = DateTime.Now, EndDate = DateTime.Now });

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgentReport(AgentReportModel reportModel)
        {
            ViewBag.Product = ProductList();
            ViewBag.ProductItem = await ProductItemList();
            ViewBag.ProductItemCategory = await ProductItemCategory();
            if (ModelState.IsValid)
            {
                // return RedirectToAction("AgentReports", new { startDay = reportModel.StartDate.Day, startMonth = reportModel.StartDate.Month, startYear = reportModel.StartDate.Year, endDay = reportModel.EndDate.Day, endMonth = reportModel.EndDate.Month, endYear = reportModel.EndDate.Year });
                return RedirectToAction("TransReportList", reportModel);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgentReportRec(AgentReportModel reportModel)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            ViewBag.Product = ProductList();
            ViewBag.ProductItem = await ProductItemList();
            ViewBag.ProductItemCategory = await ProductItemCategory();
            reportModel.UserID = user2.UserID.ToString();
            if (ModelState.IsValid)
            {
                // return RedirectToAction("AgentReports", new { startDay = reportModel.StartDate.Day, startMonth = reportModel.StartDate.Month, startYear = reportModel.StartDate.Year, endDay = reportModel.EndDate.Day, endMonth = reportModel.EndDate.Month, endYear = reportModel.EndDate.Year });
                return RedirectToAction("TransReportList", reportModel);

            }
            return View();
        }


        // public async Task<IActionResult> AgentReports(int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
        public async Task<IActionResult> AgentReports(int? pageIndex, AgentReportModel reportModel)
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

            List<AgentRViewModel> model = new List<AgentRViewModel>();
            IEnumerable<EgsInvoice> invoices;
            if (roleName == "Super Admin")
            {

                //invoices = await _unitOfWork.AgentReport.GetInvoicesByDate(startDate, endDate);
                var _invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", user2.UserID.ToString());

                ////var resultlamba = invoices.GroupBy(stu => new { stu.AlternateReferenceNo, stu.PaymentReference,stu.Amount });//.OrderBy(g => g.Key.standard).ThenBy(y => y.Key.age); //.GroupBy(stu => stu.AlternateReferenceNo).OrderBy(stu => stu.Key);

                //var query = invoices.GroupBy(g => new {
                //    g.DateCreated,
                //    g.AlternateReferenceNo,
                //    g.ReferenceNo,
                //    g.PaymentStatus,
                //    }).Select(group => new {
                //        DateCreated = group.Key.DateCreated.Date,
                //        AlternateReferenceNo = group.Key.AlternateReferenceNo,
                //        ReferenceNo = group.Key.ReferenceNo,
                //        PaymentStatus = group.Key.PaymentStatus,
                //        Amount = group.Sum(a => a.Amount),
                //        ItemCount = group.Count(),
                //});


                double baseamounts = 0;

                foreach (var invoice in _invoices)//invoices)
                {
                    //var getProductItem = await _unitOfWork.ProductItem.GetProductItems(invoice.ProductItemID);
                    //var getProduct = await _unitOfWork.Products.GetProductExtension(invoice.ProductItemID);
                    model.Add(new AgentRViewModel
                    {
                        ReferenceNo = invoice.ReferenceNo,
                        AlternateReferenceNo = invoice.CustomerAlternateRef,
                        ServiceNumber = invoice.ServiceNumber,
                        Amount = invoice.Amount,
                        DateCreated = invoice.DateCreated,
                        ProductItemName = invoice.ProductItemName,//getProductItem.ProductItemName,
                        ProductName = invoice.ProductName,//getProduct.Product.ProductName,
                        PaymentDate = invoice.PaymentDate,
                        InvoiceID = invoice.InvoiceID,
                        PaymentStatus = invoice.PaymentStatus
                    });
                    baseamounts += invoice.Amount;
                }

                //var pager = new Pager(invoices.Count(), pageIndex);
                var modelRs = new HoldInvoicesRViewModel
                {
                    HoldAllInvoices = model
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                ViewBag.baseamount = baseamounts;
                return View(modelRs);
            }
            if (roleName == "Merchant")
            {

                //invoices = await _unitOfWork.AgentReport.GetInvoicesByDate(startDate, endDate);
                var _invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", user2.UserID.ToString());

                double baseamounts = 0;

                foreach (var invoice in _invoices)//invoices)
                {
                    model.Add(new AgentRViewModel
                    {
                        ReferenceNo = invoice.ReferenceNo,
                        AlternateReferenceNo = invoice.CustomerAlternateRef,
                        CustomerAlternateRef = invoice.CustomerAlternateRef,
                        ServiceNumber = invoice.ServiceNumber,
                        Amount = invoice.Amount,
                        DateCreated = invoice.DateCreated,
                        ProductItemName = invoice.ProductItemName,//getProductItem.ProductItemName,
                        ProductName = invoice.ProductName,//getProduct.Product.ProductName,
                        PaymentDate = invoice.PaymentDate,
                        InvoiceID = invoice.InvoiceID,
                        PaymentStatus = invoice.PaymentStatus
                    });
                    baseamounts += invoice.Amount;
                }

                //var pager = new Pager(invoices.Count(), pageIndex);
                var modelRs = new HoldInvoicesRViewModel
                {
                    HoldAllInvoices = model
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                ViewBag.baseamount = baseamounts;
                return View(modelRs);
            }

            var u_invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", user2.UserID.ToString());

            //invoices = await _unitOfWork.AgentReport.GetInvoicesByDateRange(startDate, endDate, user2.UserID);
            //invoices = await _unitOfWork.AgentReport.GetInvoicesByID(user2.UserID); 

            double baseamount = 0;

            foreach (var invoice in u_invoices)
            {
                //var getProductItem = await _unitOfWork.ProductItem.GetProductItems(invoice.ProductItemID);
                //var getProduct = await _unitOfWork.Products.GetProductExtension(invoice.ProductItemID);
                model.Add(new AgentRViewModel
                {
                    ReferenceNo = invoice.ReferenceNo,
                    AlternateReferenceNo = invoice.AlternateReferenceNo,
                    ServiceNumber = invoice.ServiceNumber,
                    Amount = invoice.Amount,
                    DateCreated = invoice.DateCreated,
                    ProductItemName = invoice.ProductItemName,//getProductItem.ProductItemName,
                    ProductName = invoice.ProductName,
                    PaymentDate = invoice.PaymentDate,
                    InvoiceID = invoice.InvoiceID,
                    PaymentStatus = invoice.PaymentStatus
                });
                baseamount += invoice.Amount;
            }
            var modelR = new HoldInvoicesRViewModel
            {
                HoldAllInvoices = model
            };
            ViewBag.baseamount = baseamount;
            return View(modelR);
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> TransReportList(int? pageIndex, string searchText, AgentReportModel reportModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;
                List<DisplayInvoiceTrans> model = new List<DisplayInvoiceTrans>();

                int startDay = reportModel.StartDate.Day;
                int startMonth = reportModel.StartDate.Month;
                int startYear = reportModel.StartDate.Year;
                int endDay = reportModel.EndDate.Day;
                int endMonth = reportModel.EndDate.Month;
                int endYear = reportModel.EndDate.Year;
                string _userID = reportModel.UserID;
                var roleName = user2.Role.RoleName;

                //Construct new Date from parameters
                DateTime startDate = new DateTime(startYear, startMonth, startDay);
                DateTime endDate = new DateTime(endYear, endMonth, endDay);

                double baseamounts = 0;
                string ruserid;

                //ViewBags for StartDate
                ViewBag.OfferingDayStart = startDate.Day;
                ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
                ViewBag.OfferingYearStart = startDate.Year;

                //ViewBags for EndDate
                ViewBag.OfferingDayEnd = endDate.Day;
                ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
                ViewBag.OfferingYearEnd = endDate.Year;

                ruserid = user2.UserID.ToString();//reportModel.UserID;

                //Audit Trail 
                var audit_ = new EgsAuditTrail
                {
                    DbAction = "View",
                    DateCreated = DateTime.Now,
                    Page = "Transaction Details Report (AgentReport)",
                    Description = user2.FirstName + "" + user2.LastName + " View Agent Report transaction details between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                    IPAddress = Helpers.General.GetIPAddress(),
                    CreatedBy = user2.UserID,
                    Menu = "Report",
                    Role = user2.Role.RoleName
                };

                await _unitOfWork.AuditTrail.AddAsync(audit_);
                _unitOfWork.Complete();

                if (user2.Role.RoleID == 2 || user2.Role.RoleID == 12)
                {
                    if (ruserid != "0")
                    {

                        var _invoicees = new List<AgentRViewModel>();
                        //var _invoicees = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", ruserid);
                        if (_userID == null || _userID == "0")
                        {
                            _invoicees = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "TRANSACTIONS", "", "");

                        }
                        else
                        {
                            _invoicees = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, _userID, "TRANSACTIONS", _userID);

                        }

                        //var _invoicees = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", ruserid);
                        // var _invoicees = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, _userID, "TRANSACTIONS", user2.UserID.ToString());

                        //Logic for search
                        if (!String.IsNullOrEmpty(searchText))
                        {
                            _invoicees = _invoicees.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                        }



                        foreach (var invoice in _invoicees)
                        {
                            string rstate;
                            var invuser = await _unitOfWork.User.GetSysUsers(invoice.CreatedBy.ToString());
                          if (invuser ==null)
                            {
                                rstate = "Lagos";
                            }
                            else
                            {
                                rstate = invuser.ResidentialState.StateName;
                            }
                            var transactionmethodN = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(Convert.ToInt32(invoice.TransactionMethodID));
                            //var username = await _unitOfWork.User.GetUserByID(invoice.CreatedBy);
                            model.Add(new DisplayInvoiceTrans
                            {
                                ReferenceNo = invoice.TransactionReferenceNo,
                                // AlternateReferenceNo = invoice.AlternateReferenceNo,
                                ServiceNumber = invoice.ServiceNumber,
                                TransactionMethodName = transactionmethodN.TransactionMethod,
                                TransactionDescription = invoice.TransactionDescription,
                                Amount = invoice.Amount,
                                Previous = invoice.Previous,
                                Current = invoice.Current,
                                CreatedByName = invoice.CreatedByName, //username.FirstName + " " + username.LastName,
                                DateCreated = invoice.DateCreated,
                                ProductItemName = invoice.ProductItemName,
                                ProductName = invoice.ProductName,
                                DatePaid = invoice.TransactionDate, //invoice.PaymentDate.ToString(),//.ToString("dd/MMMM/yyyy"),
                                PaymentStatus = invoice.PaymentStatus,
                                Region = rstate
                            });

                            baseamounts += invoice.Amount;

                   
                        }

                        //int pageSizes = 4000;
                        //var pagers = new Pager(_invoicees.Count(), pageIndex);

                        var modelRs = new HoldDisplayInvoiceTrans
                        {
                            HoldRPTInvoiceTrans = model //.Skip((pagers.CurrentPage - 1) * pagers.PageSize).Take(pagers.PageSize),
                            //Pager = pagers
                        };

                        ViewBag.baseamount = baseamounts;
                        return View(modelRs);
                    }
                }

                if (user2.Role.RoleID == 4 && user2.ResidentialState.StateID == 35)
                {
                    var _invoicees = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "37", "TRANSACTIONS", user2.UserID.ToString());

                    //Logic for search
                    if (!String.IsNullOrEmpty(searchText))
                    {
                        _invoicees = _invoicees.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    foreach (var invoice in _invoicees)
                    {
                        var invuser = await _unitOfWork.User.GetSysUsers(invoice.CreatedBy.ToString());

                        var transactionmethodN = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(Convert.ToInt32(invoice.TransactionMethodID));

                        model.Add(new DisplayInvoiceTrans
                        {
                            ReferenceNo = invoice.TransactionReferenceNo,
                            // AlternateReferenceNo = invoice.AlternateReferenceNo,
                            ServiceNumber = invoice.ServiceNumber,
                            TransactionMethodName = transactionmethodN.TransactionMethod,
                            TransactionDescription = invoice.TransactionDescription,
                            Amount = invoice.Amount,
                            Previous = invoice.Previous,
                            Current = invoice.Current,
                            CreatedByName = invoice.CreatedByName, //username.FirstName + " " + username.LastName,
                            DateCreated = invoice.DateCreated,
                            ProductItemName = invoice.ProductItemName,
                            ProductName = invoice.ProductName,
                            DatePaid = invoice.TransactionDate, //invoice.PaymentDate.ToString(),//.ToString("dd/MMMM/yyyy"),
                            PaymentStatus = invoice.PaymentStatus,
                            Region = invuser.ResidentialState.StateName
                        });

                        baseamounts += invoice.Amount;
                    }


                    var modelRs = new HoldDisplayInvoiceTrans
                    {
                        HoldRPTInvoiceTrans = model //.Skip((pagers.CurrentPage - 1) * pagers.PageSize).Take(pagers.PageSize),
                                                    //Pager = pagers
                    };

                    ViewBag.baseamount = baseamounts;
                    return View(modelRs);

                }

                if (user2.Role.RoleID == 13)
                {
                    //ViewBag.role = 13;
                    if (ruserid != "0")
                    {
                        var _invoicees = new List<AgentRViewModel>();
                        //var _invoicees = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", ruserid);
                        if (_userID == null || _userID == "0")
                        {
                            _invoicees = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "TRANSACTIONS", "", "");

                        }
                        else
                        {
                            _invoicees = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, _userID, "TRANSACTIONS", _userID);

                        }

                        //Logic for search
                        if (!String.IsNullOrEmpty(searchText))
                        {
                            _invoicees = _invoicees.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                        }


                        //load up the list of viewmodels 
                        foreach (var invoice in _invoicees)
                        {
                            var invuser = new SysUsers();
                            if (invoice.CreatedBy == 0)
                            {
                                invuser = await _unitOfWork.User.GetSysUsers("1078");
                            }
                            else
                            {
                                invuser = await _unitOfWork.User.GetSysUsers(invoice.CreatedBy.ToString());
                            }

                            var transactionmethodN = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(Convert.ToInt32(invoice.TransactionMethodID));
                            //var username = await _unitOfWork.User.GetUserByID(invoice.CreatedBy);
                            model.Add(new DisplayInvoiceTrans
                            {
                                ReferenceNo = invoice.TransactionReferenceNo,
                                // AlternateReferenceNo = invoice.AlternateReferenceNo,
                                ServiceNumber = invoice.ServiceNumber,
                                TransactionMethodName = transactionmethodN.TransactionMethod,
                                TransactionDescription = invoice.TransactionDescription,
                                Amount = invoice.Amount,
                                Previous = invoice.Previous,
                                Current = invoice.Current,
                                CreatedByName = invoice.CreatedByName, //username.FirstName + " " + username.LastName,
                                DateCreated = invoice.DateCreated,
                                ProductItemName = invoice.ProductItemName,
                                ProductName = invoice.ProductName,
                                DatePaid = invoice.TransactionDate, //invoice.PaymentDate.ToString(),//.ToString("dd/MMMM/yyyy"),
                                PaymentStatus = invoice.PaymentStatus,
                                Region = invuser.ResidentialState.StateName
                            });

                            baseamounts += invoice.Amount;
                        }

                        //int pageSizes = 4000;
                        //var pagers = new Pager(_invoicees.Count(), pageIndex);

                        var modelRs = new HoldDisplayInvoiceTrans
                        {
                            HoldRPTInvoiceTrans = model //.Skip((pagers.CurrentPage - 1) * pagers.PageSize).Take(pagers.PageSize),
                            //Pager = pagers
                        };

                        ViewBag.baseamount = baseamounts;
                        return View(modelRs);
                    }
                }



                if (user2.Role.RoleID == 3)
                {
                    var _invoice = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "TRANSACTIONS", user2.UserID.ToString());
                    //var settledInvoices = await GeneralSqlClient.RPT_AggregatorCommission(startDate, endDate, user2.UserID.ToString(), "DASHBOARD");
                    //user2.UserID.ToString()
                    //Logic for search
                    if (!String.IsNullOrEmpty(searchText))
                    {
                        _invoice = _invoice.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    //load up the list of viewmodels 
                    //foreach (var invoice in _invoice)
                    //{
                    //    //var username = await _unitOfWork.User.GetUserByID(invoice.CreatedBy);
                    //    model.Add(new DisplayInvoiceTrans
                    //    {
                    //        ReferenceNo = invoice.ReferenceNo,
                    //        AlternateReferenceNo = invoice.AlternateReferenceNo,
                    //        ServiceNumber = invoice.ServiceNumber,
                    //        TransactionDescription = invoice.TransactionDescription,
                    //        Amount = invoice.Amount,
                    //        Previous = invoice.Previous,
                    //        Current = invoice.Current,
                    //        CreatedByName = invoice.CreatedByName, //username.FirstName + " " + username.LastName,
                    //        DateCreated = invoice.DateCreated,
                    //        ProductItemName = invoice.ProductItemName,
                    //        ProductName = invoice.ProductName,
                    //        DatePaid = invoice.TransactionDate, //invoice.PaymentDate.ToString(),//.ToString("dd/MMMM/yyyy"),
                    //        PaymentStatus = invoice.PaymentStatus
                    //    });

                    //    baseamounts += invoice.Amount;
                    //}



                    //load up the list of viewmodels 
                    foreach (var invoice in _invoice)
                    {
                        var invuser = await _unitOfWork.User.GetSysUsers(invoice.CreatedBy.ToString());

                        var transactionmethodN = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(Convert.ToInt32(invoice.TransactionMethodID));
                        //var username = await _unitOfWork.User.GetUserByID(invoice.CreatedBy);
                        model.Add(new DisplayInvoiceTrans
                        {
                            ReferenceNo = invoice.TransactionReferenceNo,
                            // AlternateReferenceNo = invoice.AlternateReferenceNo,
                            ServiceNumber = invoice.ServiceNumber,
                            TransactionMethodName = transactionmethodN.TransactionMethod,
                            TransactionDescription = invoice.TransactionDescription,
                            Amount = invoice.Amount,
                            Previous = invoice.Previous,
                            Current = invoice.Current,
                            CreatedByName = invoice.CreatedByName, //username.FirstName + " " + username.LastName,
                            DateCreated = invoice.DateCreated,
                            ProductItemName = invoice.ProductItemName,
                            ProductName = invoice.ProductName,
                            DatePaid = invoice.TransactionDate, //invoice.PaymentDate.ToString(),//.ToString("dd/MMMM/yyyy"),
                            PaymentStatus = invoice.PaymentStatus,
                            Region = invuser.ResidentialState.StateName
                        });

                        baseamounts += invoice.Amount;
                    }


                    //int pageSiz = 4000;
                    //var page = new Pager(_invoice.Count(), pageIndex, pageSiz);

                    var modelRe = new HoldDisplayInvoiceTrans
                    {
                        HoldRPTInvoiceTrans = model //.Skip((page.CurrentPage - 1) * page.PageSize).Take(page.PageSize),
                                                    // Pager = page
                    };

                    ViewBag.baseamount = baseamounts;
                    return View(modelRe);
                }

                if (user2.Role.RoleID == 5)
                {
                    //var _invoice = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", user2.UserID.ToString());
                    var _invoice = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "TRANSACTIONS", user2.UserID.ToString());
                    //user2.UserID.ToString()
                    //Logic for search
                    if (!String.IsNullOrEmpty(searchText))
                    {
                        _invoice = _invoice.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    //load up the list of viewmodels 
                    //foreach (var invoice in _invoice)
                    //{
                    //    //var username = await _unitOfWork.User.GetUserByID(invoice.CreatedBy); 
                    //    model.Add(new DisplayInvoiceTrans
                    //    {
                    //        ReferenceNo = invoice.ReferenceNo,
                    //        AlternateReferenceNo = invoice.AlternateReferenceNo,
                    //        ServiceNumber = invoice.ServiceNumber,
                    //        TransactionDescription = invoice.TransactionDescription,
                    //        Amount = invoice.Amount,
                    //        Previous = invoice.Previous,
                    //        Current = invoice.Current,
                    //        CreatedByName = invoice.CreatedByName, //username.FirstName + " " + username.LastName,
                    //        DateCreated = invoice.DateCreated,
                    //        ProductItemName = invoice.ProductItemName,
                    //        ProductName = invoice.ProductName,
                    //        DatePaid = invoice.TransactionDate, //invoice.PaymentDate.ToString(),//.ToString("dd/MMMM/yyyy"),
                    //        PaymentStatus = invoice.PaymentStatus
                    //    });

                    //    baseamounts += invoice.Amount;
                    //}

                    foreach (var invoice in _invoice)
                    {
                        var invuser = await _unitOfWork.User.GetSysUsers(invoice.CreatedBy.ToString());

                        var transactionmethodN = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(Convert.ToInt32(invoice.TransactionMethodID));
                        //var username = await _unitOfWork.User.GetUserByID(invoice.CreatedBy);
                        model.Add(new DisplayInvoiceTrans
                        {
                            ReferenceNo = invoice.TransactionReferenceNo,
                            // AlternateReferenceNo = invoice.AlternateReferenceNo,
                            ServiceNumber = invoice.ServiceNumber,
                            TransactionMethodName = transactionmethodN.TransactionMethod,
                            TransactionDescription = invoice.TransactionDescription,
                            Amount = invoice.Amount,
                            Previous = invoice.Previous,
                            Current = invoice.Current,
                            CreatedByName = invoice.CreatedByName, //username.FirstName + " " + username.LastName,
                            DateCreated = invoice.DateCreated,
                            ProductItemName = invoice.ProductItemName,
                            ProductName = invoice.ProductName,
                            DatePaid = invoice.TransactionDate, //invoice.PaymentDate.ToString(),//.ToString("dd/MMMM/yyyy"),
                            PaymentStatus = invoice.PaymentStatus,
                            Region = invuser.ResidentialState.StateName
                        });

                        baseamounts += invoice.Amount;
                    }



                    //int pageSiz = 4000;
                    //var page = new Pager(_invoice.Count(), pageIndex, pageSiz);

                    var modelRe = new HoldDisplayInvoiceTrans
                    {
                        HoldRPTInvoiceTrans = model //.Skip((page.CurrentPage - 1) * page.PageSize).Take(page.PageSize),
                        //Pager = page
                    };

                    ViewBag.baseamount = baseamounts;
                    return View(modelRe);
                }

                //var _invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", "");
                var _invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "TRANSACTIONS", user2.UserID.ToString());
                //user2.UserID.ToString()
                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    _invoices = _invoices.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                //foreach (var invoice in _invoices)
                //{
                //    //var username = await _unitOfWork.User.GetUserByID(invoice.CreatedBy);
                //    model.Add(new DisplayInvoiceTrans
                //    {
                //        ReferenceNo = invoice.ReferenceNo,
                //        AlternateReferenceNo = invoice.AlternateReferenceNo,
                //        ServiceNumber = invoice.ServiceNumber,
                //        TransactionDescription = invoice.TransactionDescription,
                //        Amount = invoice.Amount,
                //        Previous = invoice.Previous,
                //        Current = invoice.Current,
                //        CreatedByName = invoice.CreatedByName, //username.FirstName + " " + username.LastName,
                //        DateCreated = invoice.DateCreated,
                //        ProductItemName = invoice.ProductItemName,
                //        ProductName = invoice.ProductName,
                //        DatePaid = invoice.TransactionDate, //invoice.PaymentDate.ToString(),//.ToString("dd/MMMM/yyyy"),
                //        PaymentStatus = invoice.PaymentStatus
                //    }); 
                //    baseamounts += invoice.Amount;
                //}


                foreach (var invoice in _invoices)
                {
                    var invuser = await _unitOfWork.User.GetSysUsers(invoice.CreatedBy.ToString());

                    var transactionmethodN = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(Convert.ToInt32(invoice.TransactionMethodID));
                    //var username = await _unitOfWork.User.GetUserByID(invoice.CreatedBy);
                    model.Add(new DisplayInvoiceTrans
                    {
                        ReferenceNo = invoice.TransactionReferenceNo,
                        // AlternateReferenceNo = invoice.AlternateReferenceNo,
                        ServiceNumber = invoice.ServiceNumber,
                        TransactionMethodName = transactionmethodN.TransactionMethod,
                        TransactionDescription = invoice.TransactionDescription,
                        Amount = invoice.Amount,
                        Previous = invoice.Previous,
                        Current = invoice.Current,
                        CreatedByName = invoice.CreatedByName, //username.FirstName + " " + username.LastName,
                        DateCreated = invoice.DateCreated,
                        ProductItemName = invoice.ProductItemName,
                        ProductName = invoice.ProductName,
                        DatePaid = invoice.TransactionDate, //invoice.PaymentDate.ToString(),//.ToString("dd/MMMM/yyyy"),
                        PaymentStatus = invoice.PaymentStatus,
                        Region = invuser.ResidentialState.StateName
                    });

                    baseamounts += invoice.Amount;
                }





                //int pageSize = 4000;
                //var pager = new Pager(_invoices.Count(), pageIndex, pageSize);

                var modelR = new HoldDisplayInvoiceTrans
                {
                    HoldRPTInvoiceTrans = model //.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };

                ViewBag.baseamount = baseamounts;

                return View(modelR);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }


        public async Task<IActionResult> AgentReportALLREPORT(int? pageIndex, string searchText, AgentReportModel reportModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;
                List<DisplayInvoiceTrans> model = new List<DisplayInvoiceTrans>();

                int startDay = reportModel.StartDate.Day;
                int startMonth = reportModel.StartDate.Month;
                int startYear = reportModel.StartDate.Year;
                int endDay = reportModel.EndDate.Day;
                int endMonth = reportModel.EndDate.Month;
                int endYear = reportModel.EndDate.Year;
                string _userID = reportModel.UserID;
                var roleName = user2.Role.RoleName;

                //Construct new Date from parameters
                DateTime startDate = new DateTime(startYear, startMonth, startDay);
                DateTime endDate = new DateTime(endYear, endMonth, endDay);

                double baseamounts = 0;
                string ruserid;

                //ViewBags for StartDate
                ViewBag.OfferingDayStart = startDate.Day;
                ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
                ViewBag.OfferingYearStart = startDate.Year;

                //ViewBags for EndDate
                ViewBag.OfferingDayEnd = endDate.Day;
                ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
                ViewBag.OfferingYearEnd = endDate.Year;

                ruserid = user2.UserID.ToString();//reportModel.UserID;

                //Audit Trail 
                var audit_ = new EgsAuditTrail
                {
                    DbAction = "View",
                    DateCreated = DateTime.Now,
                    Page = "ALL Transactions Details Report (AgentReport)",
                    Description = user2.FirstName + "" + user2.LastName + " View ALL Report transaction details between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                    IPAddress = Helpers.General.GetIPAddress(),
                    CreatedBy = user2.UserID,
                    Menu = "Report",
                    Role = user2.Role.RoleName
                };

                await _unitOfWork.AuditTrail.AddAsync(audit_);
                _unitOfWork.Complete();


                var _invoicees = new List<AgentRViewModel>();

                {
                    _invoicees = await GeneralSqlClient.RPT_ALLInvoiceTransaction(startDate, endDate, "WALLET ALLREPORT");

                }

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    _invoicees = _invoicees.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }


                //load up the list of viewmodels 
                foreach (var invoice in _invoicees)
                {
                    var invuser = new SysUsers();
                    if (invoice.CreatedBy == 0)
                    {
                        invuser = await _unitOfWork.User.GetSysUsers("1078");
                    }
                    else
                    {
                        invuser = await _unitOfWork.User.GetSysUsers(invoice.CreatedBy.ToString());
                    }


                    //string transmethod="";
                    //if (invoice.TransactionMethodID != 0 )
                    //{
                    var transactionmethodN = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(Convert.ToInt32(invoice.TransactionMethodID));

                    //}
                    //else if (invoice.TransactionMethodID == 0)
                    //{
                    //    transmethod = null;
                    //}

                    //var username = await _unitOfWork.User.GetUserByID(invoice.CreatedBy);
                    model.Add(new DisplayInvoiceTrans
                    {
                        ReferenceNo = invoice.TransactionReferenceNo,
                        // AlternateReferenceNo = invoice.AlternateReferenceNo,
                        ServiceNumber = invoice.ServiceNumber,
                        TransactionMethodName = transactionmethodN.TransactionMethod,
                        TransactionDescription = invoice.TransactionDescription,
                        Amount = invoice.Amount,
                        Previous = invoice.Previous,
                        Current = invoice.Current,
                        CreatedByName = invoice.CreatedByName, //username.FirstName + " " + username.LastName,
                        DateCreated = invoice.DateCreated,
                        ProductItemName = invoice.ProductItemName,
                        ProductName = invoice.ProductName,
                        DatePaid = invoice.TransactionDate, //invoice.PaymentDate.ToString(),//.ToString("dd/MMMM/yyyy"),
                        PaymentStatus = invoice.PaymentStatus,
                        Region = invuser.ResidentialState.StateName
                    });

                    baseamounts += invoice.Amount;
                }


                var modelRs = new HoldDisplayInvoiceTrans
                {
                    HoldRPTInvoiceTrans = model //.Skip((pagers.CurrentPage - 1) * pagers.PageSize).Take(pagers.PageSize),
                                                //Pager = pagers
                };

                ViewBag.baseamount = baseamounts;
                return View(modelRs);

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        public async Task<List<DDListView>> ADAgentList()
        {
            List<DDListView> AgentList = new List<DDListView>();
            var agents = await _unitOfWork.User.GetAllAgents();

            foreach (var agentItems in agents)
            {
                AgentList.Add(new DDListView
                {
                    itemValue = agentItems.UserID,
                    itemName = agentItems.FirstName + " " + agentItems.LastName + " (" +  agentItems.Wallet.WalletAccountNumber + ")" 
                });
            }

            DropDownModelView nAgentlist = new DropDownModelView();
            AgentList.Insert(0, new DDListView { itemValue = 0, itemName = "--Select Agent  --" });
            nAgentlist.items = AgentList;

            return nAgentlist.items;
        }
        public async Task<List<DDListView>> AgentList()
        {
            List<DDListView> AgentList = new List<DDListView>();
            var agents = await _unitOfWork.User.GetAllAgents();

            foreach (var agentItems in agents)
            {
                AgentList.Add(new DDListView
                {
                    itemValue = agentItems.UserID,
                    itemName = agentItems.FirstName + " " + agentItems.LastName + " (" + Helpers.General.Mask(agentItems.Wallet.WalletAccountNumber, 0, 6, 'X') + ")"//+ " (" + agentItems.Wallet.WalletAccountNumber + ")"
                });
            }

            DropDownModelView nAgentlist = new DropDownModelView();
            AgentList.Insert(0, new DDListView { itemValue = 0, itemName = "--Select Agent  --" });
            nAgentlist.items = AgentList;

            return nAgentlist.items;
        }


        public async Task<List<DDListView>> ActiveAgentList()
        {
            List<DDListView> AgentList = new List<DDListView>();
            // var agents = await _unitOfWork.User.GetActiveAgents();
            var agents = await _unitOfWork.User.GetActiveAgentsNew();
            foreach (var agentItems in agents)
            {
                AgentList.Add(new DDListView
                {
                    itemValue = agentItems.UserID,
                    itemName = agentItems.FirstName + " " + agentItems.LastName + " (" + Helpers.General.Mask(agentItems.Wallet.WalletAccountNumber, 0, 6, 'X') + ")"//+ " (" + agentItems.Wallet.WalletAccountNumber + ")"
                });
            }

            DropDownModelView nAgentlist = new DropDownModelView();
            nAgentlist.items = AgentList;

            return nAgentlist.items;
        }

        public async Task<List<DDListView>> UserSubAgentList()
        {
            List<DDListView> SubAgentList = new List<DDListView>();
            var SubAgents = await _unitOfWork.User.GetUserActiveSubAgents();

            foreach (var SubAgentItems in SubAgents)
            {
                SubAgentList.Add(new DDListView
                {
                    itemValue = SubAgentItems.UserID,
                    itemName = SubAgentItems.FirstName + " " + SubAgentItems.LastName + " (" + SubAgentItems.BankAccountCode + ")"
                });
            }

            DropDownModelView nSubAgentlist = new DropDownModelView();
            nSubAgentlist.items = SubAgentList;

            return nSubAgentlist.items;
        }


        public async Task<List<DDListView>> ProductsList()
        {
            List<DDListView> ProductList = new List<DDListView>();

            var products = await _unitOfWork.Products.GetProductListByCategoryID(14); //14--5

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


        //public async Task<List<DDListView>> ProductItemCategoryList()
        //{
        //    List<DDListView> ProductList = new List<DDListView>();

        //    var products = await _unitOfWork.ProductItem.GetProductitemCategory();

        //    foreach (var product in products)
        //    {
        //        ProductList.Add(new DDListView
        //        {
        //            itemValue = product.ProductItemID,
        //            itemName = product.ProductItemCategory
        //        });
        //    }

        //    DropDownModelView nproductlist = new DropDownModelView();
        //    nproductlist.items = ProductList;

        //    return nproductlist.items;
        //}


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
        public async Task<IActionResult> SummaryReport()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            ViewBag.role = user2.Role.RoleID;
            //ViewBag.ProductItemCategoryList = await ProductItemCategoryList();
            ViewBag.ProductItem = await ProductItemList();
            if (user2.Role.RoleID == 2)
            {
                ViewBag.AgentList = await ADAgentList();
                ViewBag.ActiveAgentList = await ADAgentList();
            }
            else
            {
                ViewBag.AgentList = await AgentList();
                ViewBag.ActiveAgentList = await ActiveAgentList();
            }
         
            ViewBag.SubAgentList = await UserSubAgentList();
            ViewBag.ProductList = await ProductsList();
            ViewBag.NProductList = await NasProductsList();        //ProductsList();
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TransactionSummary(AgentReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;
            string walletid;
            if (reportModel.UserID != null)
            {
                var getwallet = await _unitOfWork.Wallet.GetWalletByUserID(Convert.ToInt32(reportModel.UserID));
                walletid = Convert.ToString(getwallet.WalletID);    //reportModel.RPTWalletID;
            }
            else
            {
                walletid = "";
            }

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

            List<AgentRViewModel> model = new List<AgentRViewModel>();
            IEnumerable<EgsInvoice> invoices;
            double baseamounts = 0;
            double baseamountss = 0;


            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "Transaction Summary Report (Agent Report)",
                Description = user2.FirstName + "" + user2.LastName + " View Agent Report transaction summary between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();


            if (roleName == "Super Admin")
            {

                //var sm_Invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", user2.UserID.ToString());
                var sm_Invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "SUMMARY BY REF", user2.UserID.ToString(), walletid);

                //var iquery = sm_Invoices.GroupBy(g => new
                //{
                //    V = g.DateCreated.Date.ToString("d MMM yyy"),
                //    Nam = g.CreatedBy,///.Wallet.User.FirstName + " " + g.Wallet.User.LastName,
                //    g.AlternateReferenceNo,
                //    g.CustomerAlternateRef,
                //    g.ReferenceNo,
                //    g.PaymentStatus,
                //}).Select(group => new
                //{
                //    DateCreated = group.Key.V,
                //    UserID = group.Key.Nam,
                //    AlternateReferenceNo = group.Key.AlternateReferenceNo,
                //    CustomerAlternateRef = group.Key.CustomerAlternateRef,
                //    ReferenceNo = group.Key.ReferenceNo,
                //    PaymentStatus = group.Key.PaymentStatus,
                //    Amount = group.Sum(a => a.Amount),
                //    ItemCount = group.Count(),
                //});

                foreach (var invoice in sm_Invoices)
                {
                    var createdByUserInfo = await _unitOfWork.User.GetSysUsers(invoice.UserID.ToString());
                    string acct = "";
                    //if (createdByUserInfo.BankAccountCode != null)
                    //{
                    //    acct = createdByUserInfo.BankAccountCode;
                    //}
                    //else
                    //{
                    //    acct = createdByUserInfo.Wallet.WalletAccountNumber;
                    //}
                    model.Add(new AgentRViewModel
                    {
                        ReferenceNo = invoice.ReferenceNo,
                        AlternateReferenceNo = invoice.CustomerAlternateRef,
                        //CreatedByName = createdByUserInfo.FirstName + " " + createdByUserInfo.LastName,
                        // BankAcctCode = acct,
                        Amount = invoice.Amount,
                        //TransactionDate = invoice.DateCreated,
                        PaymentStatus = invoice.PaymentStatus,
                        ItemCount = invoice.ItemCount,
                    });
                    baseamounts += invoice.Amount;
                }

                //var pager = new Pager(invoices.Count(), pageIndex);
                var modelRs = new HoldInvoicesRViewModel
                {
                    HoldAllInvoices = model
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                ViewBag.baseamount = baseamounts;
                return View(modelRs);
            }

            if (roleName == "Merchant")
            {
                //var _invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", user2.UserID.ToString());
                var _invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "SUMMARY BY REF", user2.UserID.ToString(), walletid);
                //var iquery = _invoices.GroupBy(g => new
                //{
                //    V = g.DateCreated.Date.ToString("d MMM yyy"),
                //    Nam = g.CreatedBy,///.Wallet.User.FirstName + " " + g.Wallet.User.LastName,
                //    g.AlternateReferenceNo,
                //    g.CustomerAlternateRef,
                //    g.ReferenceNo,
                //    g.PaymentStatus,
                //}).Select(group => new
                //{
                //    DateCreated = group.Key.V,
                //    UserID = group.Key.Nam,
                //    AlternateReferenceNo = group.Key.CustomerAlternateRef,//group.Key.AlternateReferenceNo,
                //    CustomerAlternateRef = group.Key.CustomerAlternateRef,
                //    ReferenceNo = group.Key.ReferenceNo,
                //    PaymentStatus = group.Key.PaymentStatus,
                //    Amount = group.Sum(a => a.Amount),
                //    ItemCount = group.Count(),
                //});
                //baseamounts = 0.00;

                foreach (var invoice in _invoices)
                {
                    //var createdByUserInfo = await _unitOfWork.User.GetSysUsers(invoice.UserID.ToString());
                    //string acct = "";
                    //if (createdByUserInfo.BankAccountCode != null)
                    //{
                    //    acct = createdByUserInfo.BankAccountCode;
                    //}
                    //else
                    //{
                    //    acct = createdByUserInfo.Wallet.WalletAccountNumber;
                    //}

                    model.Add(new AgentRViewModel
                    {
                        ReferenceNo = invoice.ReferenceNo,
                        AlternateReferenceNo = invoice.CustomerAlternateRef,
                        //CreatedByName = createdByUserInfo.FirstName + " " + createdByUserInfo.LastName,
                        //BankAcctCode = acct,
                        Amount = invoice.Amount,
                        //TransactionDate = invoice.DateCreated,
                        PaymentStatus = invoice.PaymentStatus,
                        ItemCount = invoice.ItemCount,
                    });
                    baseamountss += invoice.Amount;
                }

                //var pager = new Pager(invoices.Count(), pageIndex);
                var modelRs = new HoldInvoicesRViewModel
                {
                    HoldAllInvoices = model
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                ViewBag.baseamount = baseamountss;
                return View(modelRs);


            }


            //invoices = await _unitOfWork.AgentReport.GetInvoicesByDateRange(startDate, endDate, user2.UserID);
            //var c_Invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", user2.UserID.ToString());
            var c_Invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "SUMMARY BY REF", user2.UserID.ToString(), walletid);

            //var invquery = c_Invoices.GroupBy(g => new
            //{
            //    V = g.DateCreated.Date.ToString("d MMM yyy"),
            //    Nam = g.CreatedBy,///.Wallet.User.FirstName + " " + g.Wallet.User.LastName,
            //    g.AlternateReferenceNo,
            //    g.CustomerAlternateRef,
            //    g.ReferenceNo,
            //    g.PaymentStatus,
            //}).Select(group => new
            //{
            //    DateCreated = group.Key.V,
            //    UserID = group.Key.Nam,
            //    AlternateReferenceNo = group.Key.AlternateReferenceNo,
            //    CustomerAlternateRef = group.Key.CustomerAlternateRef,
            //    ReferenceNo = group.Key.ReferenceNo,
            //    PaymentStatus = group.Key.PaymentStatus,
            //    Amount = group.Sum(a => a.Amount),
            //    ItemCount = group.Count(),
            //});

            foreach (var invoice in c_Invoices)
            {
                var createdByUserInfo = await _unitOfWork.User.GetSysUsers(invoice.UserID.ToString());
                string acct = "";
                //if (createdByUserInfo.BankAccountCode != null)
                //{
                //    acct = createdByUserInfo.BankAccountCode;
                //}
                //else
                //{
                //    acct = createdByUserInfo.Wallet.WalletAccountNumber;
                //}
                model.Add(new AgentRViewModel
                {
                    ReferenceNo = invoice.ReferenceNo,
                    //CreatedByName = createdByUserInfo.FirstName + " " + createdByUserInfo.LastName,
                    // BankAcctCode = acct,
                    AlternateReferenceNo = invoice.AlternateReferenceNo,
                    Amount = invoice.Amount,
                    //TransactionDate = invoice.DateCreated,
                    PaymentStatus = invoice.PaymentStatus,
                    ItemCount = invoice.ItemCount,

                });
                baseamounts += invoice.Amount;
            }

            var modelR = new HoldInvoicesRViewModel
            {
                HoldAllInvoices = model
            };
            ViewBag.baseamount = baseamounts;
            return View(modelR);
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> TransReportSummary(int? pageIndex, string searchText, AgentReportModel reportModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;
                if (user2.Role.RoleID == 4 && user2.ResidentialState.StateID == 35)
                {
                    return RedirectToAction("MerchantTReportSummary", "Agents", reportModel);
                }

                List<DisplayInvoiceTrans> model = new List<DisplayInvoiceTrans>();

                int startDay = reportModel.StartDate.Day;
                int startMonth = reportModel.StartDate.Month;
                int startYear = reportModel.StartDate.Year;
                int endDay = reportModel.EndDate.Day;
                int endMonth = reportModel.EndDate.Month;
                int endYear = reportModel.EndDate.Year;
                var roleName = user2.Role.RoleName;
                string userwid = reportModel.UserID;
                string walletid;

                if (reportModel.UserID == null)
                {
                    walletid = "";
                }
               else if (reportModel.UserID != "0")
                {
                    var getwallet = await _unitOfWork.Wallet.GetWalletByUserID(Convert.ToInt32(reportModel.UserID));
                    walletid = Convert.ToString(getwallet.WalletID);    //reportModel.RPTWalletID;
                }
                else
                {

                    walletid = "";
                }

                //Construct new Date from parameters
                DateTime startDate = new DateTime(startYear, startMonth, startDay);
                DateTime endDate = new DateTime(endYear, endMonth, endDay);

                double baseamounts = 0;

                //ViewBags for StartDate
                ViewBag.OfferingDayStart = startDate.Day;
                ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
                ViewBag.OfferingYearStart = startDate.Year;

                //ViewBags for EndDate
                ViewBag.OfferingDayEnd = endDate.Day;
                ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
                ViewBag.OfferingYearEnd = endDate.Year; //userwid

                if (user2.Role.RoleID == 2 || user2.Role.RoleID == 12)
                {
                    var _invoics = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "SUMMARY BY PRODUCT CATEGORY", userwid, walletid);

                    //Logic for search
                    if (!String.IsNullOrEmpty(searchText))
                    {
                        _invoics = _invoics.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    }


                    //load up the list of viewmodels 
                    foreach (var invoice in _invoics.Where(rptInv => rptInv.PaymentStatus == true))
                    {
                        model.Add(new DisplayInvoiceTrans
                        {
                            ProductName = invoice.ProductItemCategory,
                            TotalAmt = string.Format("{0:#,0}", invoice.Amount),
                        });

                        baseamounts += invoice.Amount;
                    }

                    int pageSiz = 40;
                    var page = new Pager(_invoics.Count(), pageIndex, pageSiz);

                    var modelRe = new HoldDisplayInvoiceTrans
                    {
                        HoldRPTInvoiceTrans = model.Skip((page.CurrentPage - 1) * page.PageSize).Take(page.PageSize),
                        Pager = page
                    };

                    ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
                    return View(modelRe);
                }

                if (user2.Role.RoleID == 13)
                {
                    var _invoicess = new List<AgentRViewModel>();
                    string guserid = "";
                    if (reportModel.UserID != "0")
                    {
                        guserid = reportModel.UserID;
                        _invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "PRODUCT CATEGORY SUMMARY", guserid, walletid);

                    }
                    else
                    {
                        guserid = "";
                        _invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "PRODUCT CATEGORY SUMMARY");

                    }

                    // Logic for search
                    if (!String.IsNullOrEmpty(searchText))
                    {
                        _invoicess = _invoicess.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    }


                    //load up the list of viewmodels 
                    foreach (var invoice in _invoicess.Where(rptInv => rptInv.PaymentStatus == true))
                    {
                        model.Add(new DisplayInvoiceTrans
                        {
                            ProductName = invoice.ProductItemCategory,
                            TotalAmt = string.Format("{0:#,0}", invoice.Amount),
                        });

                        baseamounts += invoice.Amount;
                    }

                    int pageSizes = 40;
                    var pagers = new Pager(_invoicess.Count(), pageIndex, pageSizes);

                    var modelRs = new HoldDisplayInvoiceTrans
                    {
                        HoldRPTInvoiceTrans = model.Skip((pagers.CurrentPage - 1) * pagers.PageSize).Take(pagers.PageSize),
                        Pager = pagers
                    };

                    ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
                    return View(modelRs);
                }
                if (user2.Role.RoleID == 4 && user2.ResidentialState.StateID==29 )
                {
                    var _invoicess = new List<AgentRViewModel>();
                    string guserid = "";
                   
                        guserid = reportModel.UserID;
                    _invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "34", "PRODUCT CATEGORY SUMMARY", guserid, walletid);

                    //_invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "34", "SUMMARY BY PRODUCT CATEGORY", "", walletid);


                    // Logic for search
                    if (!String.IsNullOrEmpty(searchText))
                    {
                        _invoicess = _invoicess.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    }


                    //load up the list of viewmodels 
                    foreach (var invoice in _invoicess.Where(rptInv => rptInv.PaymentStatus == true))
                    {
                        model.Add(new DisplayInvoiceTrans
                        {
                            ProductName = invoice.ProductItemCategory,
                            TotalAmt = string.Format("{0:#,0}", invoice.Amount),
                        });

                        baseamounts += invoice.Amount;
                    }

                    int pageSizes = 40;
                    var pagers = new Pager(_invoicess.Count(), pageIndex, pageSizes);

                    var modelRs = new HoldDisplayInvoiceTrans
                    {
                        HoldRPTInvoiceTrans = model.Skip((pagers.CurrentPage - 1) * pagers.PageSize).Take(pagers.PageSize),
                        Pager = pagers
                    };

                    ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
                    return View(modelRs);
                }

                if (user2.Role.RoleID == 4 && user2.ResidentialState.StateID == 34)
                {
                    var _invoicess = new List<AgentRViewModel>();
                    string guserid = "";

                    guserid = reportModel.UserID;
                    _invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "43", "OSUN PRODUCT CATEGORY SUMMARY", guserid, walletid);

                    //_invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "34", "SUMMARY BY PRODUCT CATEGORY", "", walletid);


                    // Logic for search
                    if (!String.IsNullOrEmpty(searchText))
                    {
                        _invoicess = _invoicess.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    }


                    //load up the list of viewmodels 
                    foreach (var invoice in _invoicess.Where(rptInv => rptInv.PaymentStatus == true))
                    {
                        model.Add(new DisplayInvoiceTrans
                        {
                            ProductName = invoice.ProductItemCategory,
                            TotalAmt = string.Format("{0:#,0}", invoice.Amount),
                        });

                        baseamounts += invoice.Amount;
                    }

                    int pageSizes = 40;
                    var pagers = new Pager(_invoicess.Count(), pageIndex, pageSizes);

                    var modelRs = new HoldDisplayInvoiceTrans
                    {
                        HoldRPTInvoiceTrans = model.Skip((pagers.CurrentPage - 1) * pagers.PageSize).Take(pagers.PageSize),
                        Pager = pagers
                    };

                    ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
                    return View(modelRs);
                }






                //var _invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "SUMMARY BY PRODUCT CATEGORY", user2.UserID.ToString(), walletid);
                var _invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "SUMMARY BY PRODUCT CATEGORY", "", walletid);


                //Logic for search
                //if (!String.IsNullOrEmpty(searchText))
                //{
                //    _invoices = _invoices.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                //}


                //load up the list of viewmodels 
                foreach (var invoice in _invoices.Where(rptInv => rptInv.PaymentStatus == true))
                {
                    model.Add(new DisplayInvoiceTrans
                    {
                        ProductName = invoice.ProductItemCategory,
                        TotalAmt = string.Format("{0:#,0}", invoice.Amount),
                    });

                    baseamounts += invoice.Amount;
                }

                int pageSize = 40;
                var pager = new Pager(_invoices.Count(), pageIndex, pageSize);

                var modelR = new HoldDisplayInvoiceTrans
                {
                    HoldRPTInvoiceTrans = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };

                ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
                return View(modelR);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }

            //try
            //{
            //    var user = await _userManager.GetUserAsync(User);
            //    var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //    var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            //    ViewBag.pic = profiles.Image;
            //    List<DisplayInvoiceTrans> model = new List<DisplayInvoiceTrans>();

            //    int startDay = reportModel.StartDate.Day;
            //    int startMonth = reportModel.StartDate.Month;
            //    int startYear = reportModel.StartDate.Year;
            //    int endDay = reportModel.EndDate.Day;
            //    int endMonth = reportModel.EndDate.Month;
            //    int endYear = reportModel.EndDate.Year;
            //    var roleName = user2.Role.RoleName;

            //    //Construct new Date from parameters
            //    DateTime startDate = new DateTime(startYear, startMonth, startDay);
            //    DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //    double baseamounts = 0;

            //    //ViewBags for StartDate
            //    ViewBag.OfferingDayStart = startDate.Day;
            //    ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            //    ViewBag.OfferingYearStart = startDate.Year;

            //    //ViewBags for EndDate
            //    ViewBag.OfferingDayEnd = endDate.Day;
            //    ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            //    ViewBag.OfferingYearEnd = endDate.Year;

            //    var _invoices = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "DASHBOARD", user2.UserID.ToString());


            //    var iquery = _invoices.Where(p => p.PaymentStatus == true).GroupBy(g => new
            //    {
            //        payDate = g.PaymentDate.Date,
            //        prcat = g.ProductItemCategory,
            //    }).Select(group => new
            //    {
            //        prcat = group.Key.prcat,
            //        PayDate = group.Key.payDate,
            //        Amount = group.Sum(a => a.Amount),
            //        ItemCount = group.Count(),
            //    });

            //    //Logic for search
            //    if (!String.IsNullOrEmpty(searchText))
            //    {
            //        iquery = iquery.Where(rptInv => rptInv.prcat.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
            //    }

            //    foreach (var invoice in iquery.OrderByDescending(x => x.Amount).Take(5))
            //    {
            //        model.Add(new DisplayInvoiceTrans
            //        {
            //            ProductName = invoice.prcat,
            //            TotalAmt = string.Format("{0:#,0}", invoice.Amount.ToString()),
            //        });
            //        baseamounts += invoice.Amount;
            //    }

            //    int pageSize = 40;
            //    var pager = new Pager(iquery.Count(), pageIndex, pageSize);

            //    var modelR = new HoldDisplayInvoiceTrans
            //    {
            //        HoldRPTInvoiceTrans = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
            //        Pager = pager
            //    };

            //    ViewBag.baseamount = baseamounts;
            //    return View(modelR);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogInformation(ex.Message + ex.StackTrace);
            //}
            return View();
        }


        [HttpGet] 
        public async Task<IActionResult> MerchantTReportSummary(int? pageIndex, string searchText, AgentReportModel reportModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            List<DisplayInvoiceTrans> model = new List<DisplayInvoiceTrans>();

            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;
            var roleName = user2.Role.RoleName;
            string userwid = reportModel.UserID;


            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            double baseamounts = 0;

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year; //userwid
            ViewBag.role = user2.Role.RoleID;


            var _invoicess = new List<AgentRViewModel>();


            _invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "-4", "OYO CATEGORY SUMMARY");
 

            //load up the list of viewmodels 
            foreach (var invoice in _invoicess)//.Where(rptInv => rptInv.PaymentStatus == true))
            {
                model.Add(new DisplayInvoiceTrans
                {
                    ServiceNumber = invoice.ServiceNumber,
                    ItemCount = invoice.ItemCount,
                    ProductItemCategory = invoice.ProductItemCategory,
                    TotalAmt = string.Format("{0:#,0}", invoice.Amount),
                });

                baseamounts += invoice.Amount;
            }

            int pageSizess = 40;
            var pagerss = new Pager(_invoicess.Count(), pageIndex, pageSizess);

            var modelRss = new HoldDisplayInvoiceTrans
            {
                HoldRPTInvoiceTrans = model,  //.Skip((pagerss.CurrentPage - 1) * pagerss.PageSize).Take(pagerss.PageSize),
                Pager = pagerss
            };

            ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
            return View(modelRss);
        }





        [HttpGet]
        public async Task<IActionResult> NasReportSummary(int? pageIndex, string searchText, AgentReportModel reportModel)
        {

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            List<DisplayInvoiceTrans> model = new List<DisplayInvoiceTrans>();

            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;
            var roleName = user2.Role.RoleName;
            string userwid = reportModel.UserID;
            //string walletid;
            //if (reportModel.UserID != "0")
            //{
            //    var getwallet = await _unitOfWork.Wallet.GetWalletByUserID(Convert.ToInt32(reportModel.UserID));
            //    walletid = Convert.ToString(getwallet.WalletID);    //reportModel.RPTWalletID;
            //}
            //else
            //{
            //    walletid = "";
            //}

            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            double baseamounts = 0;

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year; //userwid
            ViewBag.RoleID = user2.Role.RoleID;

            var _invoicess = new List<AgentRViewModel>();

            if (user2.Role.RoleID == 2)
            {
                _invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "-1", "NASARAWA CATEGORY SUMMARY");
            }
            else

            {
                _invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "NASARAWA CATEGORY SUMMARY");

            }

            // Logic for search
            if (!String.IsNullOrEmpty(searchText))
            {
                _invoicess = _invoicess.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            //load up the list of viewmodels 
            foreach (var invoice in _invoicess.Where(rptInv => rptInv.PaymentStatus == true))
            {
                model.Add(new DisplayInvoiceTrans
                {
                    ProductName = invoice.ProductItemCategory,
                    TotalAmt = string.Format("{0:#,0}", invoice.Amount),
                });

                baseamounts += invoice.Amount;
            }

            int pageSizes = 40;
            var pagers = new Pager(_invoicess.Count(), pageIndex, pageSizes);

            var modelRs = new HoldDisplayInvoiceTrans
            {
                HoldRPTInvoiceTrans = model.Skip((pagers.CurrentPage - 1) * pagers.PageSize).Take(pagers.PageSize),
                Pager = pagers
            };

            ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
            return View(modelRs);
        }


        [HttpGet]
        public async Task<IActionResult> OyoReportSummary(int? pageIndex, string searchText, AgentReportModel reportModel)
        {

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            List<DisplayInvoiceTrans> model = new List<DisplayInvoiceTrans>();

            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;
            var roleName = user2.Role.RoleName;
            string userwid = reportModel.UserID;


            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            double baseamounts = 0;

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year; //userwid
            ViewBag.role = user2.Role.RoleID;


            var _invoicess = new List<AgentRViewModel>();
              
            _invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "", "OYO CATEGORY SUMMARY");
             
            // Logic for search
            if (!String.IsNullOrEmpty(searchText))
            {
                _invoicess = _invoicess.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            //load up the list of viewmodels 
            foreach (var invoice in _invoicess.Where(rptInv => rptInv.PaymentStatus == true))
            {
                model.Add(new DisplayInvoiceTrans
                {
                    ProductName = invoice.ProductItemCategory,
                    TotalAmt = string.Format("{0:#,0}", invoice.Amount),
                });

                baseamounts += invoice.Amount;
            }

            int pageSizes = 40;
            var pagers = new Pager(_invoicess.Count(), pageIndex, pageSizes);

            var modelRs = new HoldDisplayInvoiceTrans
            {
                HoldRPTInvoiceTrans = model.Skip((pagers.CurrentPage - 1) * pagers.PageSize).Take(pagers.PageSize),
                Pager = pagers
            };

            ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
            return View(modelRs);
            
        }

        [HttpGet]
        public async Task<IActionResult> AdminOyoReportSummary(int? pageIndex, string searchText, AgentReportModel reportModel)
        { 
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            List<DisplayInvoiceTrans> model = new List<DisplayInvoiceTrans>();

            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;
            var roleName = user2.Role.RoleName;
            string userwid = reportModel.UserID;


            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            double baseamounts = 0;

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year; //userwid
            ViewBag.role = user2.Role.RoleID;


            var _invoicess = new List<AgentRViewModel>();

           
                _invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "-1", "OYO CATEGORY SUMMARY");

                //if (!String.IsNullOrEmpty(searchText))
                //{
                //    _invoicess = _invoicess.Where(rptInv => rptInv.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.CustomerAlternateRef.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.PaymentReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                //}


                //load up the list of viewmodels 
                foreach (var invoice in _invoicess)  //.Where(rptInv => rptInv.PaymentStatus == true))
                {
                    model.Add(new DisplayInvoiceTrans
                    {
                        ServiceNumber = invoice.ServiceNumber,
                        ItemCount = invoice.ItemCount,
                        ProductItemCategory = invoice.ProductItemCategory,
                        TotalAmt = string.Format("{0:#,0}", invoice.Amount),
                    });

                    baseamounts += invoice.Amount;
                }

                int pageSizess = 40;
                var pagerss = new Pager(_invoicess.Count(), pageIndex, pageSizess);

                var modelRss = new HoldDisplayInvoiceTrans
                {
                    HoldRPTInvoiceTrans = model,  //.Skip((pagerss.CurrentPage - 1) * pagerss.PageSize).Take(pagerss.PageSize),
                    Pager = pagerss
                };
                
                ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
                return View(modelRss); 
        }


        // AdminSokotoReportSummary

        public async Task<IActionResult> AdminSokotoReportSummary(int? pageIndex, string searchText, AgentReportModel reportModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            List<DisplayInvoiceTrans> model = new List<DisplayInvoiceTrans>();

            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;
            var roleName = user2.Role.RoleName;
            string userwid = reportModel.UserID;


            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            double baseamounts = 0;
            double basecommissionamount = 0;
            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year; //userwid
            ViewBag.role = user2.Role.RoleID;


            var _invoicess = new List<AgentRViewModel>();


            _invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, "-1", "SOKOTO CATEGORY SUMMARY");

          
            //load up the list of viewmodels 
            foreach (var invoice in _invoicess)  //.Where(rptInv => rptInv.PaymentStatus == true))
            {
                model.Add(new DisplayInvoiceTrans
                {
                    ServiceNumber = invoice.ServiceNumber,
                    ProductItemCode=invoice.ProductItemCode, 
                    ItemCount = invoice.ItemCount,
                    ProductItemCategory = invoice.ProductItemCategory,
                    TotalAmt = string.Format("{0:#,0}", invoice.Amount),
                    CommissionAmount=   invoice.CommissionAmount
                });

                baseamounts += invoice.Amount;
                basecommissionamount += invoice.CommissionAmount;
            }

            int pageSizess = 40;
            var pagerss = new Pager(_invoicess.Count(), pageIndex, pageSizess);

            var modelRss = new HoldDisplayInvoiceTrans
            {
                HoldRPTInvoiceTrans = model,  //.Skip((pagerss.CurrentPage - 1) * pagerss.PageSize).Take(pagerss.PageSize),
                Pager = pagerss
            };

            ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
            ViewBag.commissionamount = string.Format("{0:#,0}", basecommissionamount);
            return View(modelRss);
        }




        [HttpGet]
        public async Task<IActionResult> AdminOsunReportSummary(int? pageIndex, string searchText, AgentReportModel reportModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            List<DisplayInvoiceTrans> model = new List<DisplayInvoiceTrans>();

            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;
            var roleName = user2.Role.RoleName;
            string userwid = reportModel.UserID;


            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            double baseamounts = 0;

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year; //userwid
            ViewBag.role = user2.Role.RoleID;


            var _invoicess = new List<AgentRViewModel>();


            _invoicess = await GeneralSqlClient.RPT_InvoiceTransaction(startDate, endDate, " ", "OSUN CATEGORY SUMMARY");

          
            //load up the list of viewmodels 
            foreach (var invoice in _invoicess)   
            {
                model.Add(new DisplayInvoiceTrans
                {
                    ServiceNumber = invoice.ServiceNumber,
                    ItemCount = invoice.ItemCount,
                    ProductItemCategory = invoice.ProductItemCategory,
                    TotalAmt = string.Format("{0:#,0}", invoice.Amount),
                });

                baseamounts += invoice.Amount;
            }

            int pageSizess = 40;
            var pagerss = new Pager(_invoicess.Count(), pageIndex, pageSizess);

            var modelRss = new HoldDisplayInvoiceTrans
            {
                HoldRPTInvoiceTrans = model,   
                Pager = pagerss
            };

            ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
            return View(modelRss);
        }
























        [HttpPost]
        public async Task<IActionResult> SettlementProductSummary(AgentReportModel reportModel)
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

            // List<AgentRViewModel> model = new List<AgentRViewModel>();
            List<SettlementRViewModel> model = new List<SettlementRViewModel>();
            LSettlementRViewModel model_ = new LSettlementRViewModel();

            // List<InvoiceReportResponse> model = new List<InvoiceReportResponse>();
            //LReportresponse model_ = new LReportresponse(); 


            //IEnumerable<EgsInvoice> invoices;
            double baseamounts = 0;
            double baseexpectedvalue = 0;
            double basesettlementValue = 0;
            double basecommission = 0;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "Settlement Product Summary DeReport",
                Description = user2.FirstName + "" + user2.LastName + " View Agent Report transaction details between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (roleName == "Super Admin")
            {
                //invoices = await _unitOfWork.AgentReport.GetInvoicesByProductID(startDate, endDate, Convert.ToInt32(reportModel.ProductID));
                var invoices = await GeneralSqlClient.RPT_Transaction(startDate, endDate, reportModel.ProductID, "SUMMARY BY PRODUCT", user2.UserID.ToString(), "");
                var iquery = invoices.GroupBy(g => new
                {
                    V = g.ProductName,        //g.Product.ProductName,
                    //Nam = g.ProductItemCategory,//g.ProductItemID, 
                    //date=g.PaymentDate.Date,
                    //pritem=g.ProductItemName,
                    prcat = g.ProductItemCategory,
                    winfo = g.WalletInfo,
                }).Select(group => new
                {
                    pName = group.Key.V,
                    prcat = group.Key.prcat,
                    winfo = group.Key.winfo,
                    Amount = group.Sum(a => a.Amount),
                    ItemCount = group.Count(),
                });

                foreach (var invoice in iquery)
                {
                    //var itemcategory = await _unitOfWork.ProductItem.GetProductItem(invoice.ItemID);
                    //var itemcategory = await _unitOfWork.ProductItem.GetSingleProductByItem(invoice.ItemID);


                    model.Add(new SettlementRViewModel
                    {
                        //InvoiceID = invoice.in,
                        ProductName = invoice.pName,
                        WalletInfo = invoice.winfo,
                        ProductItemCategory = invoice.prcat, //itemcategory.ProductItemCategory,
                        ItemCount = invoice.ItemCount,
                        transactioncvalue = invoice.Amount,
                        Expectedvalue = (invoice.ItemCount * 200),
                        settlementValue = (invoice.Amount - (invoice.ItemCount * 200)),
                        commission = (0.1 * (invoice.Amount - (invoice.ItemCount * 200)))
                    });
                    baseamounts += invoice.Amount;
                    baseexpectedvalue += (invoice.ItemCount * 200);
                    basesettlementValue += (invoice.Amount - (invoice.ItemCount * 200));
                    basecommission += (0.1 * (invoice.Amount - (invoice.ItemCount * 200)));
                }

                //var pager = new Pager(invoices.Count(), pageIndex);
                var modelRs = new HoldSettlementRViewModel
                {
                    HoldAllSettlement = model
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                ViewBag.baseamount = baseamounts;
                ViewBag.baseexpectedvalue = baseexpectedvalue;
                ViewBag.basesettlementValue = basesettlementValue;
                ViewBag.basecommission = basecommission;

                return View(modelRs);
            }

            //return View();
            return RedirectToAction("SummaryReport", "Agents");
        }



        [HttpPost]
        public async Task<IActionResult> SettlementsProductSummary(AgentReportModel reportModel)
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

            // List<AgentRViewModel> model = new List<AgentRViewModel>();
            List<SettlementRViewModel> model = new List<SettlementRViewModel>();
            LSettlementRViewModel model_ = new LSettlementRViewModel();

            // List<InvoiceReportResponse> model = new List<InvoiceReportResponse>();
            //LReportresponse model_ = new LReportresponse(); 
            var prodstate = await _unitOfWork.Products.GetProducts(Convert.ToInt32(reportModel.ProductID));
            ViewBag.productName = prodstate.ProductName;

            //IEnumerable<EgsInvoice> invoices;
            double baseamounts = 0;
            //double baseexpectedvalue = 0;
            //double basesettlementValue = 0;
            double basecommission = 0;

            if (roleName == "Super Admin" || roleName == "Operations" || roleName == "Customer Reconcilation")
            {
                //invoices = await _unitOfWork.AgentReport.GetInvoicesByProductID(startDate, endDate, Convert.ToInt32(reportModel.ProductID));, user2.UserID.ToString()
                var invoices = await GeneralSqlClient.RPT_Transaction(startDate, endDate, reportModel.ProductID, "Report By Reference Summary", user.UserID.ToString(), "");
                var iquery = invoices.GroupBy(g => new
                {
                    // V = g.ProductName,
                    // prcat = g.ProductItemCategory,
                    Agentinfo = g.Agent,
                    AcctNumber = g.AccountNumber,
                    transcount = g.TransCount,
                    //Altref = g.AlternateReferenceNo,
                    //Amount=g.Amount,
                    commision = g.commission,
                    CommisionPeriod = g.CommissionPeriod
                }).Select(group => new
                {
                    winfo = group.Key.Agentinfo,
                    AcctNumber = group.Key.AcctNumber,
                    transCount = group.Key.transcount,
                    Commision = group.Key.commision,
                    CommisionPeriod = group.Key.CommisionPeriod
                });

                foreach (var invoice in iquery)
                {
                    model.Add(new SettlementRViewModel
                    {
                        WalletInfo = invoice.winfo,
                        AccountNumber = invoice.AcctNumber,
                        TransCount = invoice.transCount,
                        CommissionPeriod = invoice.CommisionPeriod,
                        commission = invoice.Commision
                    });
                    //baseamounts += invoice.Amount;
                    //basecommission += (invoice.ItemCount * 50);
                    basecommission += (invoice.Commision);
                }

                //var pager = new Pager(invoices.Count(), pageIndex);
                var modelRs = new HoldSettlementRViewModel
                {
                    HoldAllSettlement = model
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                //ViewBag.baseamount = baseamounts;
                //ViewBag.baseexpectedvalue = baseexpectedvalue;
                //ViewBag.basesettlementValue = basesettlementValue;
                ViewBag.basecommission = basecommission;

                return View(modelRs);
            }

            //return View();
            return RedirectToAction("SummaryReport", "Agents");
        }

        [HttpPost]
        public async Task<IActionResult> TransactionSummaryByProduct(AgentReportModel reportModel, int? pageIndex)
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

            // List<AgentRViewModel> model = new List<AgentRViewModel>();
            List<SettlementRViewModel> model = new List<SettlementRViewModel>();
            LSettlementRViewModel model_ = new LSettlementRViewModel();

            // List<InvoiceReportResponse> model = new List<InvoiceReportResponse>();
            //LReportresponse model_ = new LReportresponse(); 


            //IEnumerable<EgsInvoice> invoices;
            double baseamounts = 0;
            if (reportModel.ProductID == null)
            {
                reportModel.ProductID = Convert.ToString("0");
            }

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "Transaction Summary By Product",
                Description = user2.FirstName + "" + user2.LastName + " View Transaction Summary By Product Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();


            if (roleName == "Super Admin" || roleName == "Operations")
            {
                //invoices = await _unitOfWork.AgentReport.GetInvoicesByProductID(startDate, endDate, Convert.ToInt32(reportModel.ProductID));
                var invoices = await GeneralSqlClient.RPT_Transaction(startDate, endDate, reportModel.ProductID, "SUMMARY BY PRODUCT", user2.UserID.ToString(), "");
                var iquery = invoices.GroupBy(g => new
                {
                    V = g.ProductName,
                    prcat = g.ProductItemCategory,
                }).Select(group => new
                {
                    pName = group.Key.V,
                    prcat = group.Key.prcat,
                    Amount = group.Sum(a => a.Amount),
                    ItemCount = group.Count(),
                });


                //load up the list of viewmodels 
                foreach (var invoice in iquery)
                {
                    model.Add(new SettlementRViewModel
                    {
                        ProductName = invoice.pName,
                        formattedAmount = string.Format("{0:#,0}", invoice.Amount),     //string.Format("{0:#,0}", invoice.Amount),
                    });

                    baseamounts += invoice.Amount;
                }
                ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
                int pageSize = 40;
                var pager = new Pager(iquery.Count(), pageIndex, pageSize);

                var modelRs = new HoldSettlementRViewModel
                {
                    HoldAllSettlement = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                return View(modelRs);
            }

            //return View();
            return RedirectToAction("SummaryReport", "Agents");
        }

        [HttpPost]
        public async Task<IActionResult> AgentsPerReference(AgentReportModel reportModel)
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

            // List<AgentRViewModel> model = new List<AgentRViewModel>();
            List<SettlementRViewModel> model = new List<SettlementRViewModel>();
            LSettlementRViewModel model_ = new LSettlementRViewModel();

            // List<InvoiceReportResponse> model = new List<InvoiceReportResponse>();
            //LReportresponse model_ = new LReportresponse(); 


            //IEnumerable<EgsInvoice> invoices;
            double baseamounts = 0;
            double baseexpectedvalue = 0;
            double basesettlementValue = 0;
            double basecommission = 0;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "Transaction Details Report (Per Reference)",
                Description = user2.FirstName + "" + user2.LastName + " View Agent Transaction Details Report (Per Reference) between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (roleName == "Super Admin")
            {
                //invoices = await _unitOfWork.AgentReport.GetInvoicesByProductID(startDate, endDate, Convert.ToInt32(reportModel.ProductID));
                var invoices = await GeneralSqlClient.RPT_AgentsRef(startDate, endDate, reportModel.UserID, reportModel.ProductID);
                var iquery = invoices.GroupBy(g => new
                {
                    V = g.ProductName,        //g.Product.ProductName,
                    //Nam = g.ProductItemCategory,//g.ProductItemID, 
                    //date=g.PaymentDate.Date,
                    //pritem=g.ProductItemName,
                    winfo = g.WalletInfo,
                    prcat = g.ProductItemCategory,
                    invref = g.ReferenceNumber,
                    pyref = g.PaymentReference,
                    //g.ReferenceNo,
                    //g.PaymentStatus,
                }).Select(group => new
                {
                    pName = group.Key.V,
                    walinfo = group.Key.winfo,
                    //ItemID = group.Key.Nam,
                    //pdate=group.Key.date,
                    //pritemname=group.Key.pritem,
                    prcat = group.Key.prcat,
                    invref = group.Key.invref,
                    pyref = group.Key.pyref,
                    //AlternateReferenceNo = group.Key.AlternateReferenceNo,
                    //ReferenceNo = group.Key.ReferenceNo,
                    //PaymentStatus = group.Key.PaymentStatus,
                    Amount = group.Sum(a => a.Amount),
                    ItemCount = group.Count(),
                });

                foreach (var invoice in iquery)
                {
                    //var itemcategory = await _unitOfWork.ProductItem.GetProductItem(invoice.ItemID);
                    //var itemcategory = await _unitOfWork.ProductItem.GetSingleProductByItem(invoice.ItemID);


                    model.Add(new SettlementRViewModel
                    {
                        //InvoiceID = invoice.in,
                        ProductName = invoice.pName,
                        WalletInfo = invoice.walinfo,
                        ReferenceNumber = invoice.invref,
                        PaymentReference = invoice.pyref,
                        //PaymentDate = invoice.pdate,
                        //ProductItemName = invoice.pritemname,         //itemcategory.ProductItemName,
                        ProductItemCategory = invoice.prcat,       //itemcategory.ProductItemCategory,
                        ItemCount = invoice.ItemCount,
                        transactioncvalue = invoice.Amount,
                    });
                    baseamounts += invoice.Amount;
                }

                //var pager = new Pager(invoices.Count(), pageIndex);
                var modelRs = new HoldSettlementRViewModel
                {
                    HoldAllSettlement = model
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                ViewBag.baseamount = baseamounts;

                return View(modelRs);
            }

            //return View();
            return RedirectToAction("SummaryReport", "Agents");
        }

        public async Task<IActionResult> AgentReportRef(AgentReportModel reportModel)
        {
            if (reportModel.ReferenceNumber == null)
            {
                ViewBag.Message = "Reference Number cannot be null";
                // return RedirectToAction("AgentReport");
                return View(reportModel);
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var roleName = user2.Role.RoleName;
            ViewBag.Reference = reportModel.ReferenceNumber;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "Transaction Details Report (Agent Report by Ref)",
                Description = user2.FirstName + "" + user2.LastName + " Viewed Agent Report by Reference Number " + reportModel.ReferenceNumber + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();


            List<AgentRViewModel> model = new List<AgentRViewModel>();
            IEnumerable<EgsInvoice> invoices;
            if (roleName == "Super Admin")
            {
                invoices = await _unitOfWork.AgentReport.GetInvoicesByReferenceNo(reportModel.ReferenceNumber);
                double baseamount = 0;

                foreach (var invoice in invoices)
                {
                    var getProductItem = await _unitOfWork.ProductItem.GetProductItems(invoice.ProductItemID);
                    var getProduct = await _unitOfWork.Products.GetProductExtension(invoice.ProductItemID);
                    var username = await _unitOfWork.User.GetAgentById(invoice.CreatedBy);
                    model.Add(new AgentRViewModel
                    {
                        ReferenceNo = invoice.ReferenceNo,
                        ServiceNumber = invoice.ServiceNumber,
                        CreatedByName = username.FirstName + " " + username.LastName,
                        Amount = invoice.Amount,
                        DateCreated = invoice.DateCreated,
                        ProductItemName = getProductItem.ProductItemName,
                        ProductName = getProduct.Product.ProductName,
                        PaymentDate = invoice.PaymentDate,
                        InvoiceID = invoice.InvoiceID,
                        PaymentStatus = invoice.PaymentStatus,
                        PaymentReference = invoice.PaymentReference
                    }); ; ;
                    baseamount += invoice.Amount;
                }
                var modelR = new HoldInvoicesRViewModel
                {
                    HoldAllInvoices = model
                };
                ViewBag.baseamount = baseamount;
                return View(modelR);
            }
            if (roleName == "Customer Care" || roleName == "Aggregator" || roleName == "Operations" || roleName == "Customer Reconcilation" || roleName == "Merchant" || roleName == "Agent")
            {
                invoices = await _unitOfWork.AgentReport.GetInvoicesByReferenceNo(reportModel.ReferenceNumber);
                double baseamount = 0;

                foreach (var invoice in invoices)
                {
                    var getProductItem = await _unitOfWork.ProductItem.GetProductItems(invoice.ProductItemID);
                    var getProduct = await _unitOfWork.Products.GetProductExtension(invoice.ProductItemID);
                    var username = await _unitOfWork.User.GetAgentById(invoice.CreatedBy);

                    model.Add(new AgentRViewModel
                    {
                        ReferenceNo = invoice.ReferenceNo,
                        ServiceNumber = invoice.ServiceNumber,
                        CreatedByName = username.FirstName + " " + username.LastName,
                        Amount = invoice.Amount,
                        DateCreated = invoice.DateCreated,
                        ProductItemName = getProductItem.ProductItemName,
                        ProductName = getProduct.Product.ProductName,
                        PaymentDate = invoice.PaymentDate,
                        InvoiceID = invoice.InvoiceID,
                        PaymentStatus = invoice.PaymentStatus,
                        PaymentReference = invoice.PaymentReference
                    });
                    baseamount += invoice.Amount;
                }
                var modelR = new HoldInvoicesRViewModel
                {
                    HoldAllInvoices = model
                };
                ViewBag.baseamount = baseamount;
                return View(modelR);
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AgentProductRefCount(int? pageIndex, string searchText, AgentReportModel reportModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;
                List<DisplayAgentNasModel> model = new List<DisplayAgentNasModel>();

                int startDay = reportModel.StartDate.Day;
                int startMonth = reportModel.StartDate.Month;
                int startYear = reportModel.StartDate.Year;
                int endDay = reportModel.EndDate.Day;
                int endMonth = reportModel.EndDate.Month;
                int endYear = reportModel.EndDate.Year;
                string _userID = reportModel.UserID;
                string _productid = reportModel.ProductID;
                var roleName = user2.Role.RoleName;

                //Construct new Date from parameters
                DateTime startDate = new DateTime(startYear, startMonth, startDay);
                DateTime endDate = new DateTime(endYear, endMonth, endDay);

                double baseamounts = 0;
                string ruserid;

                //ViewBags for StartDate
                ViewBag.OfferingDayStart = startDate.Day;
                ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
                ViewBag.OfferingYearStart = startDate.Year;

                //ViewBags for EndDate
                ViewBag.OfferingDayEnd = endDate.Day;
                ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
                ViewBag.OfferingYearEnd = endDate.Year;

                ruserid = user2.UserID.ToString();

                //Audit Trail 
                var audit_ = new EgsAuditTrail
                {
                    DbAction = "View",
                    DateCreated = DateTime.Now,
                    Page = "Transaction Details Report (AgentReport)",
                    Description = user2.FirstName + "" + user2.LastName + " View Agent Nasarawa/Oyo Report transaction details between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                    IPAddress = Helpers.General.GetIPAddress(),
                    CreatedBy = user2.UserID,
                    Menu = "Report",
                    Role = user2.Role.RoleName
                };

                await _unitOfWork.AuditTrail.AddAsync(audit_);
                _unitOfWork.Complete();

                var _invoices = await GeneralSqlClient.RPT_STransaction(startDate, endDate, _productid, "PRODUCT TRANSACTIONS COUNT");

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    _invoices = _invoices.Where(rptInv => rptInv.Agent.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.Agent.Contains(searchText, StringComparison.OrdinalIgnoreCase) || rptInv.BusinessName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }


                foreach (var invoice in _invoices)
                {
                    model.Add(new DisplayAgentNasModel
                    {
                        BusinessName = invoice.BusinessName,
                        Agent = invoice.Agent,
                        ItemCount = invoice.ItemCount,
                        Period = invoice.Period
                    });

                    // baseamounts += invoice.Amount;
                }

                var modelR = new HoldAgentNasModel
                {
                    HoldAllInvoices = model
                    //Pager = pager
                };

                //ViewBag.baseamount = baseamounts;

                return View(modelR);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }







        //[HttpGet]
        //public async Task<IActionResult> AdminCommissionReport(AgentReportModel reportModel)
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

        //    List<SettlementRViewModel> model = new List<SettlementRViewModel>();
        //    LSettlementRViewModel model_ = new LSettlementRViewModel();

        //    if (roleName == "Super Admin")
        //    {
        //        var invoices = await GeneralSqlClient.RPT_Transaction(startDate, endDate, reportModel.ProductID, "Report By Reference Summary");
        //         var iquery = invoices.GroupBy(g => new
        //        {
        //            V = g.ProductName,      
        //            winfo = g.WalletInfo,
        //            prcat = g.ProductItemCategory,
        //            invref = g.ReferenceNumber,
        //            pyref = g.PaymentReference, 
        //        }).Select(group => new
        //        {
        //            pName = group.Key.V,
        //            walinfo = group.Key.winfo, 
        //            prcat = group.Key.prcat,
        //            invref = group.Key.invref,
        //            pyref = group.Key.pyref, 
        //            Amount = group.Sum(a => a.Amount),
        //            ItemCount = group.Count(),
        //        });

        //        foreach (var invoice in iquery)
        //        { 

        //            model.Add(new SettlementRViewModel
        //            { 
        //                ProductName = invoice.pName,
        //                WalletInfo = invoice.walinfo,
        //                ReferenceNumber = invoice.invref,
        //                PaymentReference = invoice.pyref, 
        //                ProductItemCategory = invoice.prcat,  
        //                ItemCount = invoice.ItemCount,
        //                transactioncvalue = invoice.Amount,

        //            }); 

        //        }

        //        //var pager = new Pager(invoices.Count(), pageIndex);
        //        var modelRs = new HoldSettlementRViewModel
        //        {
        //            HoldAllSettlement = model 
        //        }; 

        //        return View(modelRs);
        //    }





        //    return View();
        //}












        //[HttpPost]
        //public async Task<IActionResult> AllAggregatorAdmin(AgentReportModel reportModel)
        //{


        //    return View();
        //}

        public async Task<IActionResult> ProductReport(int? pageIndex, AgentReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;
            int prdId = Convert.ToInt32(reportModel.ProductID);

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

            List<AgentRViewModel> model = new List<AgentRViewModel>();
            //IEnumerable<EgsInvoice> invoices;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "Product Report ",
                Description = user2.FirstName + "" + user2.LastName + " View Product Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (roleName == "Super Admin")
            {
                //invoices = await _unitOfWork.AgentReport.GetInvoicesByDateProduct(startDate, endDate, prdId);


                var invoices = await GeneralSqlClient.RPT_Transaction(startDate, endDate, prdId.ToString(), "Report By Product Item", "", "");

                double baseamounts = 0;

                foreach (var invoice in invoices)
                {
                    var getProductItem = await _unitOfWork.ProductItem.GetProductItems(invoice.ProductItemID);
                    var getProduct = await _unitOfWork.Products.GetProductExtension(invoice.ProductItemID);
                    model.Add(new AgentRViewModel
                    {
                        ReferenceNo = invoice.ReferenceNumber,  //.ReferenceNo,
                        ServiceNumber = invoice.ServiceNumber,     //.ServiceNumber,
                        Amount = invoice.Amount,
                        DateCreated = invoice.DateCreated,
                        ProductItemName = getProductItem.ProductItemName,
                        ProductName = invoice.ProductName,   //.Product.ProductName,
                        PaymentDate = invoice.PaymentDate,
                        InvoiceID = invoice.InvoiceID,
                        PaymentStatus = invoice.PaymentStatus
                    });
                    baseamounts += invoice.Amount;
                }

                var pager_ = new Pager(invoices.Count(), pageIndex);
                var modelRs = new HoldInvoicesRViewModel
                {
                    HoldAllInvoices = model.Skip((pager_.CurrentPage - 1) * pager_.PageSize).Take(pager_.PageSize),
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager_
                };
                ViewBag.baseamount = baseamounts;
                return View(modelRs);
            }
            var invoicess = await GeneralSqlClient.RPT_AgentsRef(startDate, endDate, reportModel.UserID, reportModel.ProductID);
            //invoices = await _unitOfWork.AgentReport.GetInvoicesByProductDateRange(startDate, endDate, prdId, user2.UserID);
            //invoices = await _unitOfWork.AgentReport.GetInvoicesByID(user2.UserID); 

            double baseamount = 0;

            foreach (var invoice in invoicess)
            {
                var getProductItem = await _unitOfWork.ProductItem.GetProductItems(invoice.ProductItemID);
                var getProduct = await _unitOfWork.Products.GetProductExtension(invoice.ProductItemID);
                model.Add(new AgentRViewModel
                {
                    ReferenceNo = invoice.ReferenceNumber,     //.ReferenceNo,
                    ServiceNumber = invoice.ServiceNumber,
                    Amount = invoice.Amount,
                    DateCreated = invoice.DateCreated,
                    ProductItemName = getProductItem.ProductItemName,
                    ProductName = getProduct.Product.ProductName,
                    PaymentDate = invoice.PaymentDate,
                    InvoiceID = invoice.InvoiceID,
                    PaymentStatus = invoice.PaymentStatus
                });
                baseamount += invoice.Amount;
            }
            var pager = new Pager(invoicess.Count(), pageIndex);
            var modelR = new HoldInvoicesRViewModel
            {
                HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
            ViewBag.baseamount = baseamount;
            return View(modelR);

        }

        public async Task<IActionResult> RPTProcessingTransaction(int? pageIndex, AgentReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;
            var prdt = await _unitOfWork.ProductItem.GetSingleProductByItem(Convert.ToInt32(reportModel.ProductItemID));
            int prdId = Convert.ToInt32(prdt.Product.ProductID);

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

            List<AgentRViewModel> model = new List<AgentRViewModel>();
            //IEnumerable<EgsInvoice> invoices;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "Processing Transaction Report ",
                Description = user2.FirstName + "" + user2.LastName + " View Processing Transaction Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();
            if (roleName == "Super Admin")
            {
                //invoices = await _unitOfWork.AgentReport.GetInvoicesByDateProductItem(startDate, endDate, prdId);
                var invoices = await GeneralSqlClient.RPT_Transacactionfee(startDate, endDate, "", prdId.ToString());

                double baseamounts = 0;

                foreach (var invoice in invoices)
                {
                    var getProductItem = await _unitOfWork.ProductItem.GetProductItems(invoice.ProductItemID);
                    var getProduct = await _unitOfWork.Products.GetProductExtension(invoice.ProductItemID);
                    model.Add(new AgentRViewModel
                    {
                        Amount = invoice.Amount,
                        ProductItemName = invoice.ProductItemName,
                        ProductName = invoice.ProductName,
                        ItemCount = invoice.ItemCount

                    });
                    baseamounts += invoice.Amount;
                }

                //var pager = new Pager(invoices.Count(), pageIndex);
                var modelRs = new HoldInvoicesRViewModel
                {
                    HoldAllInvoices = model
                    //HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                ViewBag.baseamount = baseamounts;
                return View(modelRs);
            }

            //invoices = await _unitOfWork.AgentReport.GetInvoicesByProductDateRange(startDate, endDate, prdId, user2.UserID);
            ////invoices = await _unitOfWork.AgentReport.GetInvoicesByID(user2.UserID); 

            //double baseamount = 0;

            //foreach (var invoice in invoices)
            //{
            //    var getProductItem = await _unitOfWork.ProductItem.GetProductItems(invoice.ProductItemID);
            //    var getProduct = await _unitOfWork.Products.GetProductExtension(invoice.ProductItemID);
            //    model.Add(new AgentRViewModel
            //    {
            //        ReferenceNo = invoice.ReferenceNo,
            //        ServiceNumber = invoice.ServiceNumber,
            //        Amount = invoice.Amount,
            //        DateCreated = invoice.DateCreated,
            //        ProductItemName = getProductItem.ProductItemName,
            //        ProductName = getProduct.Product.ProductName,
            //        PaymentDate = invoice.PaymentDate,
            //        InvoiceID = invoice.InvoiceID,
            //        PaymentStatus = invoice.PaymentStatus
            //    });
            //    baseamount += invoice.Amount;
            //}
            //var modelR = new HoldInvoicesRViewModel
            //{
            //    HoldAllInvoices = model
            //};
            //ViewBag.baseamount = baseamount;
            //return View(modelR);
            return View("SummaryReport");
        }

        public async Task<IActionResult> ItemCategoryTransaction(int? pageIndex, AgentReportModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;
            var prdItmCate = reportModel.ProductItemCategory;
            //var prdt = await _unitOfWork.ProductItem.GetSingleProductByItem(Convert.ToInt32(reportModel.ProductItemID));
            //int prdId = Convert.ToInt32(prdt.ProductItemID);

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

            List<InvoiceReportResponse> model = new List<InvoiceReportResponse>();
            LReportresponse model_ = new LReportresponse();
            IEnumerable<EgsInvoice> invoices;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "Item Category Transaction Report ",
                Description = user2.FirstName + "" + user2.LastName + " View Item Category Transaction Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            if (roleName == "Super Admin")
            {
                var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

                string Baseurl = EgolePayServiceConfig.Url;
                string sdate = startDate.Year + "-" + startDate.Month + "-" + startDate.Day;
                string edate = endDate.Year + "-" + endDate.Month + "-" + endDate.Day;
                var rtpModel = await GeneralSqlClient.RPT_InvoiceTrans(startDate, endDate, prdItmCate);
                double baseamounts = 0;
                if (rtpModel != null)
                {
                    foreach (var items in rtpModel)
                    {
                        model.Add(new InvoiceReportResponse
                        {
                            PaymentReference = items.PaymentReference,
                            WalletInfo = items.WalletInfo,
                            Business = items.Business,
                            AlternateReferenceNo = items.AlternateReferenceNo,
                            ServiceNumber = items.ServiceNumber,
                            ProductItemCategory = items.ProductItemCategory,
                            PaymentDate = items.PaymentDate,
                            DateCreated = items.DateCreated,
                            Amount = items.Amount,
                        });
                        baseamounts += items.Amount;
                    }
                    var pager = new Pager(rtpModel.Count(), pageIndex);
                    var modelRs = new HoldRPTInvoiceViewModel
                    {
                        HoldRPTInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                        Pager = pager
                    };

                    ViewBag.baseamount = baseamounts;
                    return View(modelRs);
                }
                else
                {
                    return View();
                }

                //using (var client = new HttpClient())
                //{
                //    //if (prdItmCate == null || prdItmCate == "")
                //    //{
                //        var Srequest = (Baseurl + "/api/Invoice/Report/"+prdItmCate+"/"+sdate+"/"+edate,"GET");  //+ "/" + uEmail    + "/" + uEmail
                //        string vfdEnquiryReqParams = "/api/Invoice/Report/"+prdItmCate+"/"+sdate+"/"+edate;
                //        string IResponse = General.MakeVFDRequest(Baseurl, vfdEnquiryReqParams, "GET");

                //        //Save Request and Response
                //        var logResponse = JsonConvert.DeserializeObject<LReportresponse>(IResponse);
                //        model_ = JsonConvert.DeserializeObject<LReportresponse>(IResponse);

                //        var RequestResponseLog = new SysRequestResponseLog
                //        {
                //            Request = Srequest.ToString(),
                //            Response = IResponse,
                //            DateCreated = DateTime.Now,
                //            //Message = logResponse.Message,
                //            //StatusCode = logResponse.Status,
                //            TransactionType = "Invoice Report"
                //        };

                //        await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                //        if (!String.IsNullOrEmpty(IResponse))
                //        {
                //            ////if (IResponse == "500")
                //            ////{
                //            ////    ViewBag.ErrorMessage = "Invalid Server Response, try again "; 
                //            ////    return View(model);
                //            ////}

                //            //if (Convert.ToInt32(logResponse.Status) >= 1)
                //            //{
                //            //    ViewBag.ErrorMessage = "Response " + logResponse.Message;

                //            //    return View(model);
                //            //    //return RedirectToAction("OrderInfo","WalletServices",new { area = "Wallets", id =prdid });
                //            //}
                //        }

                //        foreach (var invoice in model_.invreportreponse)
                //        {
                //            model.Add(new InvoiceReportResponse
                //            {
                //                WalletInfo = invoice.WalletInfo,
                //                Business = invoice.Business,
                //                AlternateReferenceNo = invoice.AlternateReferenceNo,
                //                ServiceNumber = invoice.ServiceNumber,
                //                Amount = invoice.Amount,
                //                DateCreated = invoice.DateCreated,
                //                PaymentDate = invoice.PaymentDate,
                //            });
                //            baseamounts += invoice.Amount;
                //        }

                //        var modelRs = new HoldRPTInvoiceViewModel
                //        {
                //            HoldRPTInvoices = model
                //        };

                //        ViewBag.baseamount = baseamounts;
                //        return View(modelRs);
                //    //}
                //}
            }

            return View();
        }

        public static string GetMonthInWord(int month)
        {
            var monthNames = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            int getMonth = month;

            return $"{monthNames[getMonth - 1] }";
        }

        public async Task<List<EgsProduct>> ProductList()
        {

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            var Baseurl = APIServiceConfig?.Url;

            List<EgsProduct> prodList = new List<EgsProduct>();

            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(Baseurl + "/api/Product", "", "GET");

                string responseToCaller = string.Empty;

                if (!String.IsNullOrEmpty(IResponse))
                {
                    prodList = JsonConvert.DeserializeObject<List<EgsProduct>>(IResponse);
                    //responseToCaller = "sent";
                    prodList.Insert(0, new EgsProduct { ProductID = 0, ProductName = "--Select Product --" });
                }
                else
                {
                    //responseToCaller = "not sent";
                    prodList.Insert(0, new EgsProduct { ProductID = 0, ProductName = "--N/A Product --" });
                }

            }

            return prodList;
        }

        public async Task<List<EgsProductItem>> ProductItemList()
        {
            //var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            //var Baseurl = APIServiceConfig?.Url;

            List<EgsProductItem> prodList = new List<EgsProductItem>();

            prodList = await _unitOfWork.ProductItem.GetProductProcessingItems();

            prodList.Insert(0, new EgsProductItem { ProductItemID = 0, ProductItemName = "--Select Product Item --" });

            return prodList;

        }

        public async Task<List<EgsProductItem>> ProductItemCategory()
        {

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            var Baseurl = APIServiceConfig?.Url;

            List<EgsProductItem> prodList = new List<EgsProductItem>();
            List<EgsProductItem> bookList = new List<EgsProductItem>();

            prodList = await _unitOfWork.ProductItem.GetProductItemCategories();
            //yourTable.GroupBy(x => x.TableFieldColumn).Select(x => x.FirstOrDefault());
            bookList = prodList.GroupBy(book => book.ProductItemCategory).Select(x => x.First()).ToList();

            bookList.Insert(0, new EgsProductItem { ProductItemCategory = "--Select Item Category --" });


            //using (var client = new HttpClient())
            //{
            //    string IResponse = General.MakeRequest(Baseurl + "/api/ProductItem", "", "GET");

            //    string responseToCaller = string.Empty;

            //    if (!String.IsNullOrEmpty(IResponse))
            //    {
            //        prodList = JsonConvert.DeserializeObject<List<EgsProductItemViewModel>>(IResponse);
            //        //responseToCaller = "sent";
            //        prodList.Insert(0, new EgsProductItemViewModel {ProductItemCategory = "--Select Item Category --" });
            //    }
            //    else
            //    {
            //        //responseToCaller = "not sent";
            //        prodList.Insert(0, new EgsProductItemViewModel { ProductItemCategory = "--N/A Item Category --" });
            //    }

            //}

            return bookList;
        }


        public async Task<List<SysUsers>> SADList()
        {

            List<SysUsers> userList = new List<SysUsers>();

            userList = await _unitOfWork.User.GetActiveAgents();

            foreach (var items in userList)
            {
                var key = "b14ca5898a4e4133wale2ea2315a1916";

                var getusertoupdate = await _unitOfWork.User.GetSysUsers(items.UserID.ToString());

                getusertoupdate.BVN = Helpers.General.EncryptString(key, items.BVN);
                getusertoupdate.PhoneNumber = Helpers.General.EncryptString(key, items.PhoneNumber);
                getusertoupdate.DateOfBirth = Helpers.General.EncryptString(key, items.DateOfBirth);

                _unitOfWork.CreateUser.UpdateUser(getusertoupdate);
                _unitOfWork.Complete();

            }


            return userList;

        }



        public async Task<List<EgsWallet>> AWalletList()
        {

            List<EgsWallet> wList = new List<EgsWallet>();

            wList = await _unitOfWork.Wallet.GetWallets();

            foreach (var items in wList)
            {
                var key = "b14ca5898a4e4133wale2ea2315a1916";  

                var getwallettoupdate = await _unitOfWork.Wallet.GetWallet(items.WalletID);

                getwallettoupdate.BVN = Helpers.General.EncryptString(key, items.BVN);

                _unitOfWork.Wallet.Update(getwallettoupdate);
                _unitOfWork.Complete();

            }


            return wList;

        }




    }
}
