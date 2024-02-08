using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.Reports.Models;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Controllers
{
    [Area("Reports")]
    //[Authorize(Roles = "Agent")]
    [AllowAnonymous]
    public class WalletHistoryController : Controller
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

        public WalletHistoryController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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

        [HttpGet]
        public async Task<IActionResult> WalletHistory(string msg)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            ViewBag.role = user2.Role.RoleID;
            // ViewBag.Error = msg;
            ViewBag.roleName = user2.Role.RoleName;
            ViewBag.AgentList = await AgentList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WalletHistory(WalletHistoryModel walletHistoryModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            if (ModelState.IsValid)
            {
                var checkdaterange = (walletHistoryModel.EndDate - walletHistoryModel.StartDate).TotalDays;
                if (checkdaterange > 29)
                {
                    ViewBag.AgentList = await AgentList();
                    ViewBag.role = user2.Role.RoleID;
                    ModelState.Clear();
                    ViewBag.Error = "Wallet Reports can only be spooled for one month";
                    return View();
                }

                return RedirectToAction("WalletHistorys", new {walletid = walletHistoryModel.UserID, startDay = walletHistoryModel.StartDate.Day, startMonth = walletHistoryModel.StartDate.Month, startYear = walletHistoryModel.StartDate.Year, endDay = walletHistoryModel.EndDate.Day, endMonth = walletHistoryModel.EndDate.Month, endYear = walletHistoryModel.EndDate.Year, tnxType = walletHistoryModel.TransactionType });
            }

            ViewBag.AgentList = await AgentList();
            return View();
        }

        public async Task<IActionResult> WalletHistorys(int walletid,int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear, string tnxType)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

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

            List<WalletHViewModel> model = new List<WalletHViewModel>();

            int ID = user2.UserID;
            int walletId;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "WalletHistory",
                Description = user2.FirstName + "" + user2.LastName + " View Wallet History Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (user2.Role.RoleID == 2 || user2.Role.RoleID == 11 || user2.Role.RoleID == 12 || user2.Role.RoleID == 13)
            {
                //var transactions = await _unitOfWork.WalletHistory.GetTransactionsByDateRangeAndUserID(startDate, endDate, ID);
                var transactions = await _unitOfWork.WalletHistory.GetWalletHistoryTransaction(user.UserID, walletid, startDate, endDate, user2.Role.RoleName, tnxType.ToUpper());

                foreach (var transaction in transactions)
                {
                    if (transaction != null)
                    {
                        if (transaction.TransactionMethod == null)
                        {
                            model.Add(new WalletHViewModel
                            {
                                TransactionID = transaction.WalletTransactionID,
                                ReferenceNo = transaction.TransactionReferenceNo,
                                Amount = transaction.Amount,
                                TransactionMethod = "Bill Payment",
                                TransactionType = transaction.TransactionType.TransactionType,
                                PaymentDate = transaction.TransactionDate,
                                WalletAccountNumber = Helpers.General.Mask(transaction.Wallet.WalletAccountNumber, 0, 6, 'X'),//transaction.Wallet.WalletAccountNumber,
                                WalletHolder = transaction.Wallet.User.FirstName + " " + transaction.Wallet.User.LastName,
                            });
                        }
                        else
                        {
                            model.Add(new WalletHViewModel
                            {
                                TransactionID = transaction.WalletTransactionID,
                                ReferenceNo = transaction.TransactionReferenceNo,
                                Amount = transaction.Amount,
                                TransactionMethod = transaction.TransactionMethod.TransactionMethod,
                                TransactionType = transaction.TransactionType.TransactionType,
                                PaymentDate = transaction.TransactionDate,
                                WalletAccountNumber = Helpers.General.Mask(transaction.Wallet.WalletAccountNumber, 0, 6, 'X'),//transaction.Wallet.WalletAccountNumber,
                                WalletHolder = transaction.Wallet.User.FirstName + " " + transaction.Wallet.User.LastName,
                            });
                        }
                    }
                    //baseamount += transaction.Invoices.Amount;
                }
                //var pager = new Pager(transactions.Count(), 1);

                var modelR = new HoldWalletHViewModel
                {
                    HoldAllWalletHistorys = model//.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                //ViewBag.baseamount = baseamount;

                ViewBag.roleName = user2.Role.RoleName;
                ViewBag.AgentList = await AgentList();
                return View(modelR);
            }
            else if (user2.Role.RoleID == 5 || user2.Role.RoleID == 9 || user2.Role.RoleID == 3)
            {
                if (user2.BankAccountCode == null)
                {
                    walletId = user2.Wallet.WalletID;
                }
                else
                {
                    var getwallet = await _unitOfWork.Wallet.GetWalletByAccountNumber(user2.BankAccountCode);
                    walletId = getwallet.WalletID;
                }
                var transactions = await _unitOfWork.WalletHistory.GetTransactionsByDateRangeAndUserID(startDate, endDate, walletId);
                foreach (var transaction in transactions)
                {
                    //var getProductItem = await _unitOfWork.ProductItem.GetProductItems(transaction.Invoices.ProductItemID);
                    //var getProduct = await _unitOfWork.Products.GetProductExtension(transaction.Invoices.ProductItemID);
                    //var tmethod = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(transaction.TransactionMethod.TransactionMethodID);

                    if (transaction != null)
                    {
                        if (transaction.TransactionMethod == null)
                        {

                            model.Add(new WalletHViewModel
                            {
                                TransactionID = transaction.WalletTransactionID,
                                ReferenceNo = transaction.TransactionReferenceNo,
                                Amount = transaction.Amount,
                                //TransactionMethod = transaction.TransactionMethod.TransactionMethod == null ? null : "", 
                                TransactionMethod = "Bill Payment",
                                TransactionType = transaction.TransactionType.TransactionType,
                                PaymentDate = transaction.TransactionDate,
                                //ProductItemName = transaction.ProductItem.ProductItemName == null ? "Payment" : transaction.ProductItem.ProductItemName,//activesystemUser?.Gender ?? null,
                                WalletAccountNumber = Helpers.General.Mask(transaction.Wallet.WalletAccountNumber, 0, 6, 'X'),//transaction.Wallet.WalletAccountNumber,
                                WalletHolder = transaction.Wallet.User.FirstName + " " + transaction.Wallet.User.LastName,
                                //ProductItemName = getProductItem.ProductItemName,
                                //ProductName = getProduct.Product.ProductName,
                                //InvoiceID = transaction.Invoices.InvoiceID,
                                //PaymentStatus = transaction.Invoices.PaymentStatus
                            });
                        }
                        else
                        {
                            model.Add(new WalletHViewModel
                            {
                                TransactionID = transaction.WalletTransactionID,
                                ReferenceNo = transaction.TransactionReferenceNo,
                                Amount = transaction.Amount,
                                //TransactionMethod = transaction.TransactionMethod.TransactionMethod == null ? null : "", 
                                TransactionMethod = transaction.TransactionMethod.TransactionMethod,

                                TransactionType = transaction.TransactionType.TransactionType,
                                PaymentDate = transaction.TransactionDate,
                                //ProductItemName = transaction.ProductItem.ProductItemName == null ? "Payment" : transaction.ProductItem.ProductItemName,//activesystemUser?.Gender ?? null,
                                WalletAccountNumber = Helpers.General.Mask(transaction.Wallet.WalletAccountNumber, 0, 6, 'X'), //transaction.Wallet.WalletAccountNumber,
                                WalletHolder = transaction.Wallet.User.FirstName + " " + transaction.Wallet.User.LastName,
                                //ProductItemName = getProductItem.ProductItemName,
                                //ProductName = getProduct.Product.ProductName,
                                //InvoiceID = transaction.Invoices.InvoiceID,
                                //PaymentStatus = transaction.Invoices.PaymentStatus
                            });
                        }
                    }
                    //baseamount += transaction.Invoices.Amount;
                }
                //var pager = new Pager(transactions.Count(), 1);
                var modelR = new HoldWalletHViewModel
                {
                    HoldAllWalletHistorys = model//.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    //Pager = pager
                };
                //ViewBag.baseamount = baseamount;
                ViewBag.roleName = user2.Role.RoleName;
                ViewBag.AgentList = await AgentList();
                return View(modelR);

            }
            return View("WalletHistory");
        }

        public async Task<List<DDListView>> AgentList()
        {

            List<DDListView> AgentList = new List<DDListView>();
            List<SysUsers> agents = null;
            //if (ViewBag.roleName.ToUpper().Contains("FINANCE") || ViewBag.roleName.ToUpper().Contains("AUDIT AND CONTROL"))
            //{
            //    agents = await _unitOfWork.User.GetSpecialAgents();
            //}
            //else
            {
                agents = await _unitOfWork.User.GetAllAgents();
            }

            foreach (var agentItems in agents)
            {
                {
                    AgentList.Add(new DDListView
                    {
                        itemValue = agentItems.UserID,
                        itemName = agentItems.FirstName + " " + agentItems.LastName + " (" + Helpers.General.Mask(agentItems.Wallet.WalletAccountNumber, 0, 6, 'X') + ")"//" (" + Helpers.General.MaskString(agentItems.Wallet.WalletAccountNumber, 3, "****") + ")"
                    });
                }
            }

            DropDownModelView nAgentlist = new DropDownModelView();

            AgentList.Insert(0, new DDListView { itemValue = 0, itemName = "--Select Agent  --" });

            nAgentlist.items = AgentList;

            return nAgentlist.items;
        }

        public async Task<IActionResult> WalletFundHistory(WalletHistoryModel walletHistoryModel)
        {
            //ViewBag.AgentList = await AgentList();
            int startDay, startMonth, startYear, endDay, endMonth, endYear;
            string tnxType;
            startDay = walletHistoryModel.StartDate.Day;
            startMonth = walletHistoryModel.StartDate.Month;
            startYear = walletHistoryModel.StartDate.Year;
            endDay = walletHistoryModel.EndDate.Day;
            endMonth = walletHistoryModel.EndDate.Month;
            endYear = walletHistoryModel.EndDate.Year;
            tnxType = walletHistoryModel.TransactionType;
            int agID = walletHistoryModel.UserID;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            tnxType = "CREDIT";
            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            var checkdaterange = (endDay - startDay);   //.TotalDays;
            if (checkdaterange > 1)
            {
                string msg = "Wallet Reports can only be spooled for one day";
                return RedirectToAction("WalletHistory");
            }


            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;

            List<WalletHViewModel> model = new List<WalletHViewModel>();

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "WalletFundHistory",
                Description = user2.FirstName + "" + user2.LastName + " View Wallet Fund History Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            int ID = user2.UserID;

            if (user2.Role.RoleID == 11 || user2.Role.RoleID == 12 || user2.Role.RoleID == 2 || user2.Role.RoleID == 13)
            {
                if (agID != 0)
                {
                    var selectedwallet = await _unitOfWork.User.GetUserByID(agID);
                    //var transactions = await _unitOfWork.WalletHistory.GetTransactionsByDateRangeAndUserID(startDate, endDate, ID);
                    var transactions = await _unitOfWork.WalletHistory.GetWalletHistoryTransaction(agID, selectedwallet.Wallet.WalletID, startDate, endDate, "Super Admin", tnxType.ToUpper());

                    foreach (var transaction in transactions)
                    {
                        if (transaction != null)
                        {
                            model.Add(new WalletHViewModel
                            {
                                TransactionID = transaction.WalletTransactionID,
                                ReferenceNo = transaction.TransactionReferenceNo,
                                Amount = transaction.Amount,
                                TransactionMethod = transaction.TransactionMethod.TransactionMethod == null ? null : "",
                                TransactionType = transaction.TransactionType.TransactionType,
                                PaymentDate = transaction.TransactionDate,
                                WalletAccountNumber = Helpers.General.Mask(transaction.Wallet.WalletAccountNumber, 0, 6, 'X'),// Helpers.General.Mask(agentItems.Wallet.WalletAccountNumber, 0, 6, 'X'),//transaction.Wallet.WalletAccountNumber,
                                WalletHolder = transaction.Wallet.User.FirstName + " " + transaction.Wallet.User.LastName,
                                WalletEmail = transaction.Wallet.User.Email
                            });
                        }
                        //baseamount += transaction.Invoices.Amount;
                    }

                    var modelR = new HoldWalletHViewModel
                    {
                        HoldAllWalletHistorys = model
                    };

                    ViewBag.roleName = user2.Role.RoleName;
                    ViewBag.AgentList = await AgentList();
                    return View(modelR);
                }
                else
                {
                    var transactions = await _unitOfWork.WalletHistory.GetWalletHistoryTransaction(agID, 0, startDate, endDate, "Super Admin", tnxType.ToUpper());

                    foreach (var transaction in transactions)
                    {
                        if (transaction != null)
                        {
                            model.Add(new WalletHViewModel
                            {
                                TransactionID = transaction.WalletTransactionID,
                                ReferenceNo = transaction.TransactionReferenceNo,
                                Amount = transaction.Amount,
                                TransactionMethod = transaction.TransactionMethod.TransactionMethod == null ? null : "",
                                TransactionType = transaction.TransactionType.TransactionType,
                                PaymentDate = transaction.TransactionDate,
                                WalletAccountNumber = Helpers.General.Mask(transaction.Wallet.WalletAccountNumber, 0, 6, 'X'),//transaction.Wallet.WalletAccountNumber,
                                WalletHolder = transaction.Wallet.User.FirstName + " " + transaction.Wallet.User.LastName,
                                WalletEmail = transaction.Wallet.User.Email
                            }); ;
                        }
                        //baseamount += transaction.Invoices.Amount;
                    }

                    var modelR = new HoldWalletHViewModel
                    {
                        HoldAllWalletHistorys = model
                    };

                    ViewBag.roleName = user2.Role.RoleName;
                    ViewBag.AgentList = await AgentList();
                    return View(modelR);
                }
            }

            if (agID != 0)
            {
                var selectedwallet = await _unitOfWork.User.GetUserByID(ID);
                //var transactions = await _unitOfWork.WalletHistory.GetTransactionsByDateRangeAndUserID(startDate, endDate, ID);
                var transactions = await _unitOfWork.WalletHistory.GetWalletHistoryTransaction(agID, selectedwallet.Wallet.WalletID, startDate, endDate, user2.Role.RoleName, tnxType.ToUpper());

                foreach (var transaction in transactions)
                {
                    if (transaction != null)
                    {
                        model.Add(new WalletHViewModel
                        {
                            TransactionID = transaction.WalletTransactionID,
                            ReferenceNo = transaction.TransactionReferenceNo,
                            Amount = transaction.Amount,
                            TransactionMethod = transaction.TransactionMethod.TransactionMethod == null ? null : "",
                            TransactionType = transaction.TransactionType.TransactionType,
                            PaymentDate = transaction.TransactionDate,
                            WalletAccountNumber = Helpers.General.Mask(transaction.Wallet.WalletAccountNumber, 0, 6, 'X'),//transaction.Wallet.WalletAccountNumber,
                            WalletHolder = transaction.Wallet.User.FirstName + " " + transaction.Wallet.User.LastName,
                            WalletEmail = transaction.Wallet.User.Email
                        });
                    }
                    //baseamount += transaction.Invoices.Amount;
                }

                var modelR = new HoldWalletHViewModel
                {
                    HoldAllWalletHistorys = model
                };

                ViewBag.roleName = user2.Role.RoleName;
                ViewBag.AgentList = await AgentList();
                return View(modelR);
            }
            else
            {
                var transactions = await _unitOfWork.WalletHistory.GetWalletHistoryTransaction(agID, 0, startDate, endDate, user2.Role.RoleName, tnxType.ToUpper());

                foreach (var transaction in transactions)
                {
                    if (transaction != null)
                    {
                        model.Add(new WalletHViewModel
                        {
                            TransactionID = transaction.WalletTransactionID,
                            ReferenceNo = transaction.TransactionReferenceNo,
                            Amount = transaction.Amount,
                            TransactionMethod = transaction.TransactionMethod.TransactionMethod == null ? null : "",
                            TransactionType = transaction.TransactionType.TransactionType,
                            PaymentDate = transaction.TransactionDate,
                            WalletAccountNumber = Helpers.General.Mask(transaction.Wallet.WalletAccountNumber, 0, 6, 'X'),//transaction.Wallet.WalletAccountNumber,
                            WalletHolder = transaction.Wallet.User.FirstName + " " + transaction.Wallet.User.LastName,
                            WalletEmail = transaction.Wallet.User.Email
                        });
                    }
                    //baseamount += transaction.Invoices.Amount;
                }

                var modelR = new HoldWalletHViewModel
                {
                    HoldAllWalletHistorys = model
                };

                ViewBag.roleName = user2.Role.RoleName;
                ViewBag.AgentList = await AgentList();
                return View(modelR);
            }

            return RedirectToAction("WalletHistory");
        }

        [HttpGet]
        public async Task<IActionResult> WalletActivitySummary()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            ViewBag.role = user2.Role.RoleID;
            ViewBag.roleName = user2.Role.RoleName;
            ViewBag.AgentList = await AgentList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WalletActivitySummary(WalletHistoryModel walletHistoryModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            int startDay, startMonth, startYear, endDay, endMonth, endYear, qUserID;
            string tnxType;
            startDay = walletHistoryModel.StartDate.Day;
            startMonth = walletHistoryModel.StartDate.Month;
            startYear = walletHistoryModel.StartDate.Year;
            endDay = walletHistoryModel.EndDate.Day;
            endMonth = walletHistoryModel.EndDate.Month;
            endYear = walletHistoryModel.EndDate.Year;
            tnxType = walletHistoryModel.TransactionType;
            qUserID = walletHistoryModel.UserID;//user2.UserID;

            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //var checkdaterange = (endDay - startDay);   //.TotalDays;
            //if (checkdaterange > 1)
            //{
            //    string msg = "Wallet Reports can only be spooled for one day";
            //    //return View("WalletHistory", msg);
            //    return RedirectToAction("WalletHistory");
            //}


            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;

            List<WalletHViewModel> model = new List<WalletHViewModel>();
            int ID = user2.UserID;
            int walletId;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "WalletActivitySummary",
                Description = user2.FirstName + "" + user2.LastName + " View Wallet Activity Summary Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (user2.Role.RoleID == 2 || user2.Role.RoleID == 12 || user2.Role.RoleID == 13)
            {
                if (walletHistoryModel.UserID != 0)
                {
                    var selectedwallet = await _unitOfWork.User.GetUserByID(walletHistoryModel.UserID);
                    //Passing value 0 because it's sysAdministrator

                    var transactions = await GeneralSqlClient.RPT_WalletTransaction(startDate, endDate, "", "WALLET SUMMARY", user2.UserID.ToString(), selectedwallet.Wallet.WalletID.ToString());

                    foreach (var transaction in transactions)
                    {
                        model.Add(new WalletHViewModel
                        {
                            TransactionType = transaction.TransactionType,
                            TransactionDate = transaction.TransactionDate,
                            TransactionMethod = transaction.TransactionMethod,
                            WalletHolder = transaction.WalletHolder,
                            Amount = transaction.Amount,
                        });

                        //baseamounts += transaction.Amount;
                    }

                    var modelR = new HoldWalletHViewModel
                    {
                        HoldAllWalletHistorys = model
                    };
                    ViewBag.roleName = user2.Role.RoleName;
                    ViewBag.AgentList = await AgentList();
                    return View(modelR);
                }
                else if (walletHistoryModel.UserID == 0)
                {
                    //Passing value 0 because it's sysAdministrator
                    var transactions = await GeneralSqlClient.RPT_WalletTransaction(startDate, endDate, "", "WALLET SUMMARY", user2.UserID.ToString(), "-1");

                    foreach (var transaction in transactions)
                    {
                        model.Add(new WalletHViewModel
                        {
                            TransactionType = transaction.TransactionType,
                            TransactionDate = transaction.TransactionDate,
                            TransactionMethod = transaction.TransactionMethod,
                            WalletHolder = transaction.WalletHolder,
                            Amount = transaction.Amount,
                        });

                        //baseamounts += transaction.Amount;
                    }

                    var modelR = new HoldWalletHViewModel
                    {
                        HoldAllWalletHistorys = model
                    };
                    ViewBag.roleName = user2.Role.RoleName;
                    ViewBag.AgentList = await AgentList();
                    return View(modelR);
                }
            }
            if (user2.Role.RoleID == 5 || user2.Role.RoleID == 3)
            {
                var checkdaterange = (endDay - startDay);   //.TotalDays;
                if (checkdaterange > 1)
                {
                    string msg = "Wallet Reports can only be spooled for one day";
                    //return View("WalletHistory", msg);
                    return RedirectToAction("WalletHistory");
                }

                if (user2.UserID != 0)
                {
                    var selectedwallet = await _unitOfWork.User.GetUserByID(walletHistoryModel.UserID);
                    var transactions = await GeneralSqlClient.RPT_WalletTransaction(startDate, endDate, "", "WALLET SUMMARY", user2.UserID.ToString(), selectedwallet.Wallet.WalletID.ToString());

                    foreach (var transaction in transactions)
                    {
                        model.Add(new WalletHViewModel
                        {
                            TransactionType = transaction.TransactionType,
                            TransactionDate = transaction.TransactionDate,
                            TransactionMethod = transaction.TransactionMethod,
                            WalletHolder = transaction.WalletHolder,
                            Amount = transaction.Amount,
                        });

                        //baseamounts += transaction.Amount;
                    }

                    var modelR = new HoldWalletHViewModel
                    {
                        HoldAllWalletHistorys = model
                    };
                    ViewBag.roleName = user2.Role.RoleName;
                    ViewBag.AgentList = await AgentList();
                    return View(modelR);
                }
            }

            return View("WalletHistory");
        }

        [HttpGet]
        public async Task<IActionResult> WalletTransferSummary()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            ViewBag.role = user2.Role.RoleID;
            ViewBag.roleName = user2.Role.RoleName;
            ViewBag.AgentList = await AgentList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WalletTransferSummary(WalletHistoryModel walletHistoryModel)
        {
            ViewBag.TransactionType = walletHistoryModel.TransactionType;
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            int startDay, startMonth, startYear, endDay, endMonth, endYear, qUserID;
            string tnxType;
            startDay = walletHistoryModel.StartDate.Day;
            startMonth = walletHistoryModel.StartDate.Month;
            startYear = walletHistoryModel.StartDate.Year;
            endDay = walletHistoryModel.EndDate.Day;
            endMonth = walletHistoryModel.EndDate.Month;
            endYear = walletHistoryModel.EndDate.Year;
            tnxType = walletHistoryModel.TransactionType;
            qUserID = walletHistoryModel.UserID;//user2.UserID;

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

            List<WalletHViewModel> model = new List<WalletHViewModel>();
            int ID = user2.UserID;
            int walletId;

            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "WalletTransferSummary",
                Description = user2.FirstName + "" + user2.LastName + " View Wallet Transfer Summary Report between " + startDate.ToString() + " and " + endDate.ToString() + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();

            if (user2.Role.RoleID == 2 || user2.Role.RoleID == 12 || user2.Role.RoleID == 13)
            {
                if (walletHistoryModel.UserID != 0)
                {
                    var selectedwallet = await _unitOfWork.User.GetUserByID(walletHistoryModel.UserID);
                    //Passing value 0 because it's sysAdministrator

                    var transactions = await GeneralSqlClient.RPT_WalletTransaction(startDate, endDate, "", "WALLET TRANSFER", user2.UserID.ToString(), selectedwallet.Wallet.WalletID.ToString(), walletHistoryModel.TransactionType);

                    if (walletHistoryModel.TransactionType == "CREDIT")
                    {
                        foreach (var transaction in transactions)
                        {
                            model.Add(new WalletHViewModel
                            {
                                TransactionType = transaction.TransactionType,
                                TransactionDate = transaction.TransactionDate,
                                TransactionMethod = transaction.TransactionMethod,
                                WalletHolder = transaction.WalletHolder,
                                Amount = transaction.Amount,
                                Sender = transaction.Sender,
                            });

                            //baseamounts += transaction.Amount;
                        }

                    }
                    else
                    {
                        foreach (var transaction in transactions)
                        {
                            model.Add(new WalletHViewModel
                            {
                                TransactionType = transaction.TransactionType,
                                TransactionDate = transaction.TransactionDate,
                                TransactionMethod = transaction.TransactionMethod,
                                WalletHolder = transaction.WalletHolder,
                                Amount = transaction.Amount,
                                ReceiverName=transaction.ReceiverName,
                                ReceiverBank=transaction.ReceiverBank,
                                ReceiverAccount=transaction.ReceiverAccount
                            });

                            //baseamounts += transaction.Amount;
                        }
                    }

                    var modelR = new HoldWalletHViewModel
                    {
                        HoldAllWalletHistorys = model
                    };
                    ViewBag.roleName = user2.Role.RoleName;
                    ViewBag.AgentList = await AgentList();
                    return View(modelR);
                }
                else if (walletHistoryModel.UserID == 0)
                {
                    //Passing value 0 because it's sysAdministrator
                    var transactions = await GeneralSqlClient.RPT_WalletTransaction(startDate, endDate, "", "WALLET TRANSFER", user2.UserID.ToString(), "-1", walletHistoryModel.TransactionType);

                    if (walletHistoryModel.TransactionType == "CREDIT")
                    {
                        foreach (var transaction in transactions)
                        {
                            model.Add(new WalletHViewModel
                            {
                                TransactionType = transaction.TransactionType,
                                TransactionDate = transaction.TransactionDate,
                                TransactionMethod = transaction.TransactionMethod,
                                WalletHolder = transaction.WalletHolder,
                                Amount = transaction.Amount,
                                Sender = transaction.Sender,
                            });

                            //baseamounts += transaction.Amount;
                        }
                    }
                    else
                    {
                        foreach (var transaction in transactions)
                        {
                            model.Add(new WalletHViewModel
                            {
                                TransactionType = transaction.TransactionType,
                                TransactionDate = transaction.TransactionDate,
                                TransactionMethod = transaction.TransactionMethod,
                                WalletHolder = transaction.WalletHolder,
                                Amount = transaction.Amount,
                                ReceiverName = transaction.ReceiverName,
                                ReceiverBank = transaction.ReceiverBank,
                                ReceiverAccount = transaction.ReceiverAccount
                            });

                            //baseamounts += transaction.Amount;
                        }
                    }

                    var modelR = new HoldWalletHViewModel
                    {
                        HoldAllWalletHistorys = model
                    };
                    ViewBag.roleName = user2.Role.RoleName;
                    ViewBag.AgentList = await AgentList();
                    return View(modelR);
                }
            }

            if (user2.Role.RoleID == 5 || user2.Role.RoleID == 3)
            {
                //var checkdaterange = (endDay - startDay);   //.TotalDays;
                //if (checkdaterange > 1)
                //{
                //    string msg = "Wallet Reports can only be spooled for one day";
                //    //return View("WalletHistory", msg);
                //    return RedirectToAction("WalletHistory");
                //}

                if (user2.UserID != 0)
                {
                    var selectedwallet = await _unitOfWork.User.GetUserByID(walletHistoryModel.UserID);
                    var transactions = await GeneralSqlClient.RPT_WalletTransaction(startDate, endDate, "", "WALLET TRANSFER", user2.UserID.ToString(), selectedwallet.Wallet.WalletID.ToString(), walletHistoryModel.TransactionType);

                    if (walletHistoryModel.TransactionType == "CREDIT")
                    {
                        foreach (var transaction in transactions)
                        {
                            model.Add(new WalletHViewModel
                            {
                                TransactionType = transaction.TransactionType,
                                TransactionDate = transaction.TransactionDate,
                                TransactionMethod = transaction.TransactionMethod,
                                WalletHolder = transaction.WalletHolder,
                                Amount = transaction.Amount,
                                Sender = transaction.Sender,
                            });

                            //baseamounts += transaction.Amount;
                        }

                    }
                    else
                    {
                        foreach (var transaction in transactions)
                        {
                            model.Add(new WalletHViewModel
                            {
                                TransactionType = transaction.TransactionType,
                                TransactionDate = transaction.TransactionDate,
                                TransactionMethod = transaction.TransactionMethod,
                                WalletHolder = transaction.WalletHolder,
                                Amount = transaction.Amount,
                            });

                            //baseamounts += transaction.Amount;
                        }
                    }

                    var modelR = new HoldWalletHViewModel
                    {
                        HoldAllWalletHistorys = model
                    };
                    ViewBag.roleName = user2.Role.RoleName;
                    ViewBag.AgentList = await AgentList();
                    return View(modelR);
                }
            }

            return View("WalletHistory");
        }



        [HttpPost]
        public async Task<IActionResult> WeeklyTransactions(int? pageIndex, WalletHistoryModel walletHistoryModel)
        {
            ViewBag.TransactionType = walletHistoryModel.TransactionType;
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            List<WeeklyRViewModel> model = new List<WeeklyRViewModel>();
            string tnxType; 
            tnxType = walletHistoryModel.TransactionType; 
             
            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "View",
                DateCreated = DateTime.Now,
                Page = "WeeklyTransactionSummary",
                Description = user2.FirstName + "" + user2.LastName + " View Weekly Transaction Summary Report between " + " at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Report",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();
            double baseamounts = 0;
            var _invoicess = new List<WeeklyRViewModel>();
             
            _invoicess = await GeneralSqlClient.RPT_WeeklySummaryTransaction(tnxType);

            //load up the list of viewmodels 
            foreach (var invoice in _invoicess)//.Where(rptInv => rptInv.PaymentStatus == true))
            {
                model.Add(new DisplayWeeklyRViewModel
                {
                    Amount=invoice.Amount,
                    FirstName=invoice.FirstName,
                    MiddleName=invoice.MiddleName,
                    LastName=invoice.LastName,
                    WalletID=invoice.WalletID,
                    TransCount=invoice.TransCount,
                    TransPeriod=invoice.TransPeriod
                });

                baseamounts += invoice.Amount;
            }

            int pageSizess = 40;
            var pagerss = new Pager(_invoicess.Count(), pageIndex, pageSizess);

            var modelRss = new HoldWeeklyRViewModel
            {
                HoldAllInvoices = model,  //.Skip((pagerss.CurrentPage - 1) * pagerss.PageSize).Take(pagerss.PageSize),
                Pager = pagerss
            };

            ViewBag.baseamount = string.Format("{0:#,0}", baseamounts);
            return View(modelRss);
             
        }









        public static string GetMonthInWord(int month)
        {
            var monthNames = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            int getMonth = month;

            return $"{monthNames[getMonth - 1] }";
        }


    }
}
