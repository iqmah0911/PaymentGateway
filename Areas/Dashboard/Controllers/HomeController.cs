using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.Dashboard.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    public class HomeController : Controller
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
        private readonly IConfiguration _configuration;
        //private readonly IEmailSender _emailSender;

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, /*EmailSender emailSender,*/
            RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            //_mailSender = emailSender;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        // GET: HomeController
        public async Task<ActionResult> Welcome()
        { 
            string dashStatus = "";
            double allTransValue = 0.00;

            //checking user's currently logged in
            var user = await _userManager.GetUserAsync(User); 
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            var roleName = user2.Role.RoleName;
            var _DateNow = DateTime.Now;
            var MyNewDateValue = _DateNow;//.AddDays(-7);

            //user2.AggregatorID = 3012;

            //if (roleName == "Aggregator")
            //{
            //    var getAggagents = await _unitOfWork.AggregatorRequest.GetAggregatorAgents(Convert.ToInt32(user2.AggregatorID));
            //    List<string> IDList = new List<string>();
            //    var idList = string.Empty;
            //    foreach (var getag in getAggagents)
            //    {
            //        IDList.Add(getag.Agent.UserID.ToString()) /*+',')*/;
            //        idList = idList + getag.Agent.UserID.ToString() + ',';
            //    }
            //    IDList.Add(user2.UserID.ToString());
            //    idList = idList + user2.UserID.ToString();

            //    DateTime MyDateTime = new DateTime();
            //    //MyDateTime = DateTime.Now;
            //    var invoices = await GeneralSqlClient.RPT_AggregatorCommission(MyDateTime, DateTime.Now, idList, "Aggregator Commission");
            //    var settledInvoices = await GeneralSqlClient.RPT_AggregatorCommission(MyDateTime, DateTime.Now, user2.AggregatorID.ToString(), "Aggregator Commission Paid");
            //    //var commissum = invoices.Sum(u => u.Commisionamount);
            //    var commissum = invoices.Sum(u => u.Commisionamount);
            //    var commissettledsum = settledInvoices.Sum(u => u.Commisionamount);
            //    ViewBag.Commissionamount = commissum.ToString("N", CultureInfo.InvariantCulture);
            //    ViewBag.CommissionPaid = commissettledsum.ToString("N", CultureInfo.InvariantCulture);
                
            //    List<AgentsDashData> agentsmodel = new List<AgentsDashData>();
            //    double baseamounts = 0.00;
            //    var iquery = invoices.GroupBy(g => new//.Where(p => p.IsAggregatorSettled == false).GroupBy(g => new
            //    {
            //        //payDate = g.PaymentDate.Date,
            //        prcat = g.AccountInfo,
            //    }).Select(group => new
            //    {
            //        prcat = group.Key.prcat,
            //        //PayDate = group.Key.payDate,
            //        Amount = group.Sum(a => a.Commisionamount),
            //        ItemCount = group.Count(),
            //    });

            //    foreach (var invoice in iquery.OrderByDescending(x => x.Amount).Take(10))
            //    {
            //        agentsmodel.Add(new AgentsDashData
            //        {
            //            AccountInfo = invoice.prcat,
            //            TotalAmount = invoice.Amount,
            //        });
            //        baseamounts += invoice.Amount;
            //    }

            //    var transs = new DashboardViewModel
            //    {
            //        RoleName = roleName,
            //        RoleID = user2.Role.RoleID,
            //        UnPaidCommisionValue = commissum.ToString("N", CultureInfo.InvariantCulture),//string.Format("{0:#,0}", commissum.ToString()),
            //        PaidCommisionValue = commissettledsum.ToString("N", CultureInfo.InvariantCulture),//string.Format("{0:#,0}", commissettledsum.ToString()),
            //        AgentsList = agentsmodel
            //    };

            //    //var transs = new DashboardViewModel
            //    //{
            //    //    RoleName = roleName,
            //    //    RoleID = user2.Role.RoleID,
            //    //    //HoldAllTransactionHistory = transmodel 
            //    //};
            //    return View(transs);
            //}

            //if (roleName == "Customer")
            //{

            //    int ID = user2.UserID;
            //    var transactions = await _unitOfWork.WalletHistory.GetTransactionsByIDAndValue(ID, 5);

            //    List<TransactionHistory> transmodel = new List<TransactionHistory>();

            //    foreach (var transaction in transactions)
            //    {

            //        transmodel.Add(new TransactionHistory
            //        {
            //            TransactionID = transaction.WalletTransactionID,
            //            TransactionType = transaction.TransactionType.TransactionType,
            //            Amount = transaction.Amount
            //        });

            //    }


            //    var transs = new DashboardViewModel
            //    {
            //        RoleName = roleName,
            //        RoleID = user2.Role.RoleID,
            //        HoldAllTransactionHistory = transmodel

            //    };

            //    return View(transs);
            //}
            //if (roleName == "Agent")
            //{
            //    var invoices = await GeneralSqlClient.RPT_Transaction(MyNewDateValue, MyNewDateValue, "", "DASHBOARD", user2.UserID.ToString(),"");

            //    int allTransCount = invoices.Count();//await _unitOfWork.WalletHistory.GetWalletTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "ALL");
            //    allTransValue = invoices.Sum(u => u.Amount);
            //    var compTransCount = invoices.Where(p => p.PaymentStatus == true).Count(); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "COMPLETED");
            //    var unCompTransCount = invoices.Where(p => p.PaymentStatus == false).Count(); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "UNCOMPLETED");

            //    //var allTransCount = await _unitOfWork.Invoice.GetTransaction(user.UserID, MyNewDateValue, MyNewDateValue, roleName, "ALL");
            //    //allTransValue = allTransCount.Sum(u => u.Amount);
            //    //var compTransCount = await _unitOfWork.Invoice.GetTransaction(user.UserID, MyNewDateValue, MyNewDateValue, roleName, "COMPLETED");
            //    //var unCompTransCount = await _unitOfWork.Invoice.GetTransaction(user.UserID, MyNewDateValue, MyNewDateValue, roleName, "UNCOMPLETED");

            //    var allProducts = await _unitOfWork.Invoice.GetTransaction(user.UserID, MyNewDateValue, MyNewDateValue, roleName, "ALL");

            //    var trans = new DashboardViewModel
            //    {
            //        RoleName = roleName,
            //        RoleID = user2.Role.RoleID,
            //        TransactionCount = allTransCount,
            //        TransactionValue = allTransValue.ToString(),
            //        CompletedTransactionCount = compTransCount,
            //        UnCompletedTransactionCount = unCompTransCount,
            //        //ProductList = (ProductData)(IEnumerable<ProductDashData>)prdmodel
            //        //ProductList = prdmodel
            //    };

            //    return View(trans);
            //}
            //if (roleName == "SubAgent")
            //{
            //    var allTransCount = await _unitOfWork.Invoice.GetTransaction(user.UserID, MyNewDateValue, MyNewDateValue, roleName, "ALL");
            //    allTransValue = allTransCount.Sum(u => u.Amount);
            //    var compTransCount = await _unitOfWork.Invoice.GetTransaction(user.UserID, MyNewDateValue, MyNewDateValue, roleName, "COMPLETED");
            //    var unCompTransCount = await _unitOfWork.Invoice.GetTransaction(user.UserID, MyNewDateValue, MyNewDateValue, roleName, "UNCOMPLETED");

            //    var allProducts = await _unitOfWork.Invoice.GetTransaction(user.UserID, MyNewDateValue, MyNewDateValue, roleName, "ALL");

            //    var trans = new DashboardViewModel
            //    {
            //        RoleName = roleName,
            //        RoleID = user2.Role.RoleID,
            //        TransactionCount = allTransCount.Count(),
            //        TransactionValue = allTransValue.ToString(),
            //        CompletedTransactionCount = compTransCount.Count(),
            //        UnCompletedTransactionCount = unCompTransCount.Count(),
            //        //ProductList = (ProductData)(IEnumerable<ProductDashData>)prdmodel
            //        //ProductList = prdmodel
            //    };

            //    return View(trans);
            //}

            if (roleName == "Merchant")
            {
                var merchantInfo = await _unitOfWork.User.GetMerchantsById(user2.UserID);
                //Cost recovery
                var invoices = await GeneralSqlClient.RPT_Transaction(MyNewDateValue, MyNewDateValue, "", "DASHBOARD", user2.UserID.ToString(),"");


                int allTransCount = 0;// = invoices.Count();//await _unitOfWork.WalletHistory.GetWalletTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "ALL");
                allTransValue = invoices.Sum(u => u.Amount);
                //var compTrans = invoices.Where(p => p.PaymentStatus == true).Count(); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "COMPLETED");
                int unCompTransCount = 0; //= invoices.Where(p => p.PaymentStatus == false).Count(); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "UNCOMPLETED");
                //string.Format("{0:#,0}", totalTransfer)
                
                var _iqueryAll = invoices.GroupBy(g => new
                {
                    payDate = g.PaymentDate.Date,
                    prcat = g.PaymentReference,
                    altRef = g.AlternateReferenceNo,
                    prname=g.ProductName,
                }).Select(group => new
                {
                    prname = group.Key.prname,
                    prcat = group.Key.prcat,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.Amount),
                    RefCount = group.Select(refs => refs.AlternateReferenceNo).Distinct().Count(),
                });
                var compTransCount = 0;

                foreach (var invoice in _iqueryAll)
                {
                    allTransCount += invoice.RefCount;
                    //allTransValue += invoice.Amount;
                }

                var _iqueryPaid = invoices.Where(p => p.PaymentStatus == true).GroupBy(g => new
                {
                    payDate = g.PaymentDate.Date,
                    prcat = g.PaymentReference,
                    altRef = g.AlternateReferenceNo,
                    prname = g.ProductName,
                }).Select(group => new
                {
                    prname = group.Key.prname,
                    prcat = group.Key.prcat,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.Amount),
                    RefCount = group.Select(refs => refs.AlternateReferenceNo).Distinct().Count(),
                });
                compTransCount = 0;
                
                foreach (var invoice in _iqueryPaid)
                {
                    compTransCount += invoice.RefCount;
                }

                var _iqueryUnPaid = invoices.Where(p => p.PaymentStatus == false).GroupBy(g => new
                {
                    payDate = g.PaymentDate.Date,
                    prcat = g.PaymentReference,
                    altRef = g.AlternateReferenceNo,
                }).Select(group => new
                {
                    prcat = group.Key.prcat,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.Amount),
                    RefCount = group.Select(refs => refs.AlternateReferenceNo).Distinct().Count(),
                });
                unCompTransCount = 0;

                foreach (var invoice in _iqueryUnPaid)
                {
                    unCompTransCount += invoice.RefCount;
                }

                List<ProductDashData> prodmodel = new List<ProductDashData>();
                double baseamounts = 0.00;
                var iquery = invoices.Where(p => p.PaymentStatus == true).GroupBy(g => new
                {
                    prname = g.ProductName,
                    payDate = g.PaymentDate.Date,
                    prcat = g.ProductItemCategory,
                }).Select(group => new
                {
                    prname = group.Key.prname,
                    prcat = group.Key.prcat,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.Amount),
                    ItemCount = group.Count(),
                });

                if (user2.ResidentialState.StateID == 35)
                {
                    foreach (var invoice in iquery.OrderByDescending(x => x.Amount).Take(5))
                    {
                        prodmodel.Add(new ProductDashData
                        {
                            ProductName = invoice.prname,   
                            TotalAmount = invoice.Amount,
                        });
                        baseamounts += invoice.Amount;
                    }

                    var transsc = new DashboardViewModel
                    {
                        RoleName = roleName,
                        RoleID = user2.Role.RoleID,
                        TransactionCount = allTransCount,
                        TransactionValue = string.Format("{0:#,0}", allTransValue.ToString()),//,
                        CompletedTransactionCount = compTransCount,
                        UnCompletedTransactionCount = unCompTransCount,
                        ProductList = prodmodel
                    };

                    return View(transsc);
                }

                foreach (var invoice in iquery.OrderByDescending(x => x.Amount).Take(3))
                {
                    prodmodel.Add(new ProductDashData
                    {
                        ProductName = invoice.prcat,   // prname = group.Key.prname,
                        TotalAmount = invoice.Amount,
                    });
                    baseamounts += invoice.Amount;
                }

                var transs = new DashboardViewModel
                {
                    RoleName = roleName,
                    RoleID = user2.Role.RoleID,
                    TransactionCount = allTransCount,
                    TransactionValue = string.Format("{0:#,0}", allTransValue.ToString()),//,
                    CompletedTransactionCount = compTransCount,
                    UnCompletedTransactionCount = unCompTransCount,
                    ProductList = prodmodel
                };

                return View(transs);
            }
            if (roleName == "Super Admin")
            {
                var egoleuser = await _unitOfWork.User.GetUserByEmail("Egolepay@courtevillegroup.com");
                var merchantCount = await _unitOfWork.User.GetMerchantUsers();//GetMerchants()
                var agentCount = await _unitOfWork.User.GetAllAgents();
                var dMerchantCount = await _unitOfWork.User.GetMerchantsToday(MyNewDateValue, MyNewDateValue);//GetMerchants()
                var dAgentCount = await _unitOfWork.User.GetAgentsToday(MyNewDateValue, MyNewDateValue);
                var actAgentCount = await _unitOfWork.User.GetActiveAgents();// Active Agents
                var inActAgentCount = await _unitOfWork.User.GetInActiveAgents();// InActive Agents
                var actMerchantCount = await _unitOfWork.User.GetActiveMerchants();// Active Merchants
                var inActMerchantCount = await _unitOfWork.User.GetInActiveMerchants();// InActive 
                var upgraderequest = await _unitOfWork.UpgradeAccount.GetAllRequests();
                ViewBag.RequestsCount = upgraderequest.Count();

                var invoices = await GeneralSqlClient.RPT_Transaction(MyNewDateValue, MyNewDateValue, "", "DASHBOARD", user2.UserID.ToString(),"");
                var banks = await GeneralSqlClient.RPT_Transaction(MyNewDateValue, MyNewDateValue, "", "BANK TRANSACTIONS", user2.UserID.ToString(),"");

                int allTransCount = invoices.Count();//await _unitOfWork.WalletHistory.GetWalletTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "ALL");
                allTransValue = invoices.Sum(u => u.Amount);
                //var compTransCount = invoices.Where(p => p.PaymentStatus == true).Count(); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "COMPLETED");
                //var unCompTransCount = invoices.Where(p => p.PaymentStatus == false).Count(); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "UNCOMPLETED");

                var totalTransfer = banks.Where(p => p.TransactionTypeID == 1 && p.TransactionMethodID == 2).Sum(u => u.Amount); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "COMPLETED");
                var totalWalBal = banks.Where(p => p.TransactionTypeID == 1 && (p.TransactionMethodID != 2 || p.TransactionMethodID != 12) && p.WalletID != egoleuser.Wallet.WalletID).Sum(u => u.Amount); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "UNCOMPLETED");
                var totalBalCharge = banks.Where(p => p.TransactionTypeID == 1 && p.TransactionMethodID == 10).Sum(u => u.Amount); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "UNCOMPLETED");

                var _iqueryAll = invoices.GroupBy(g => new
                {
                    payDate = g.PaymentDate.Date,
                    prcat = g.PaymentReference,
                    altRef = g.AlternateReferenceNo,
                }).Select(group => new
                {
                    prcat = group.Key.prcat,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.Amount),
                    RefCount = group.Select(refs => refs.AlternateReferenceNo).Distinct().Count(),
                });
                int compTransCount = 0, unCompTransCount = 0;

                foreach (var invoice in _iqueryAll)
                {
                    allTransCount += invoice.RefCount;
                    //allTransValue += invoice.Amount;
                }

                var _iqueryPaid = invoices.Where(p => p.PaymentStatus == true).GroupBy(g => new
                {
                    payDate = g.PaymentDate.Date,
                    prcat = g.PaymentReference,
                    altRef = g.AlternateReferenceNo,
                }).Select(group => new
                {
                    prcat = group.Key.prcat,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.Amount),
                    RefCount = group.Select(refs => refs.AlternateReferenceNo).Distinct().Count(),
                });
                compTransCount = 0;

                foreach (var invoice in _iqueryPaid)
                {
                    compTransCount += invoice.RefCount;
                }

                var _iqueryUnPaid = invoices.Where(p => p.PaymentStatus == false).GroupBy(g => new
                {
                    payDate = g.PaymentDate.Date,
                    prcat = g.PaymentReference,
                    altRef = g.AlternateReferenceNo,
                }).Select(group => new
                {
                    prcat = group.Key.prcat,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.Amount),
                    RefCount = group.Select(refs => refs.AlternateReferenceNo).Distinct().Count(),
                });
                unCompTransCount = 0;

                foreach (var invoice in _iqueryUnPaid)
                {
                    unCompTransCount += invoice.RefCount;
                }

                //var iquery = invoices.GroupBy(g => new
                //{
                //    payDate = g.PaymentDate.Date,
                //    prcat = g.PaymentReference,
                //}).Select(group => new
                //{
                //    prcat = group.Key.prcat,
                //    PayDate = group.Key.payDate,
                //    Amount = group.Sum(a => a.Amount),
                //    RefCount = group.Count(),
                //});
                //compTransCount = 0;

                //foreach (var invoice in iquery)
                //{
                //    compTransCount += invoice.RefCount;
                //}


                ViewBag.allTrans = allTransCount;
                ViewBag.completedTrans = compTransCount;

                List<ProductDashData> prodmodel = new List<ProductDashData>();

                var transs = new DashboardViewModel
                {
                    TotalAgentsCount = agentCount.Count(),
                    TotalMerchantCount = merchantCount.Count(),
                    dAgentsCount = dAgentCount.Count(),
                    dMerchantCount = dMerchantCount.Count(),
                    ActiveAgentCount = actAgentCount.Count(),
                    InActiveAgentCount = inActAgentCount.Count(),
                    ActiveMerchantCount = actMerchantCount.Count(),
                    InActiveMerchantCount = inActMerchantCount.Count(),
                    RoleName = roleName,
                    RoleID = user2.Role.RoleID,
                    TotalTransfer = string.Format("{0:#,0}", totalTransfer),     //allTransCount.Count(),
                    TotalWalletBal = string.Format("{0:#,0}", totalWalBal),
                    TotalBankCharges = string.Format("{0:#,0}", totalBalCharge),
                    CompletedTransactionCount = compTransCount, //compTransCount.Count(),
                    UnCompletedTransactionCount = unCompTransCount,//unCompTransCount.Count(),
                    //ProductList = (ProductData)(IEnumerable<ProductDashData>)prdmodel
                    ProductList = prodmodel
                };

                return View(transs);
            } 
            if (roleName == "Customer Reconcilation")
            {
                var egoleuser = await _unitOfWork.User.GetUserByEmail("Egolepay@courtevillegroup.com");
                var merchantCount = await _unitOfWork.User.GetMerchantUsers();//GetMerchants()
                var agentCount = await _unitOfWork.User.GetAllAgents();
                var dMerchantCount = await _unitOfWork.User.GetMerchantsToday(MyNewDateValue, MyNewDateValue);//GetMerchants()
                var dAgentCount = await _unitOfWork.User.GetAgentsToday(MyNewDateValue, MyNewDateValue);
                var actAgentCount = await _unitOfWork.User.GetActiveAgents();// Active Agents
                var inActAgentCount = await _unitOfWork.User.GetInActiveAgents();// InActive Agents
                var actMerchantCount = await _unitOfWork.User.GetActiveMerchants();// Active Merchants
                var inActMerchantCount = await _unitOfWork.User.GetInActiveMerchants();// InActive 
                var upgraderequest = await _unitOfWork.UpgradeAccount.GetAllRequests();
                ViewBag.RequestsCount = upgraderequest.Count();

                var invoices = await GeneralSqlClient.RPT_Transaction(MyNewDateValue, MyNewDateValue, "", "DASHBOARD", "18", "");
                var banks = await GeneralSqlClient.RPT_Transaction(MyNewDateValue, MyNewDateValue, "", "BANK TRANSACTIONS", "18", "");

                int allTransCount = invoices.Count();//await _unitOfWork.WalletHistory.GetWalletTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "ALL");
                allTransValue = invoices.Sum(u => u.Amount);
                //var compTransCount = invoices.Where(p => p.PaymentStatus == true).Count(); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "COMPLETED");
                //var unCompTransCount = invoices.Where(p => p.PaymentStatus == false).Count(); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "UNCOMPLETED");

                var totalTransfer = banks.Where(p => p.TransactionTypeID == 1 && p.TransactionMethodID == 2).Sum(u => u.Amount); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "COMPLETED");
                var totalWalBal = banks.Where(p => p.TransactionTypeID == 1 && (p.TransactionMethodID != 2 || p.TransactionMethodID != 12) && p.WalletID != egoleuser.Wallet.WalletID).Sum(u => u.Amount); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "UNCOMPLETED");
                var totalBalCharge = banks.Where(p => p.TransactionTypeID == 1 && p.TransactionMethodID == 10).Sum(u => u.Amount); //await _unitOfWork.Invoice.GetTransaction(merchantInfo.Merchant.MerchantID, MyNewDateValue, MyNewDateValue, roleName, "UNCOMPLETED");

                var _iqueryAll = invoices.GroupBy(g => new
                {
                    payDate = g.PaymentDate.Date,
                    prcat = g.PaymentReference,
                    altRef = g.AlternateReferenceNo,
                }).Select(group => new
                {
                    prcat = group.Key.prcat,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.Amount),
                    RefCount = group.Select(refs => refs.AlternateReferenceNo).Distinct().Count(),
                });
                int compTransCount = 0, unCompTransCount = 0;

                foreach (var invoice in _iqueryAll)
                {
                    allTransCount += invoice.RefCount;
                    //allTransValue += invoice.Amount;
                }

                var _iqueryPaid = invoices.Where(p => p.PaymentStatus == true).GroupBy(g => new
                {
                    payDate = g.PaymentDate.Date,
                    prcat = g.PaymentReference,
                    altRef = g.AlternateReferenceNo,
                }).Select(group => new
                {
                    prcat = group.Key.prcat,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.Amount),
                    RefCount = group.Select(refs => refs.AlternateReferenceNo).Distinct().Count(),
                });
                compTransCount = 0;

                foreach (var invoice in _iqueryPaid)
                {
                    compTransCount += invoice.RefCount;
                }

                var _iqueryUnPaid = invoices.Where(p => p.PaymentStatus == false).GroupBy(g => new
                {
                    payDate = g.PaymentDate.Date,
                    prcat = g.PaymentReference,
                    altRef = g.AlternateReferenceNo,
                }).Select(group => new
                {
                    prcat = group.Key.prcat,
                    PayDate = group.Key.payDate,
                    Amount = group.Sum(a => a.Amount),
                    RefCount = group.Select(refs => refs.AlternateReferenceNo).Distinct().Count(),
                });
                unCompTransCount = 0;

                foreach (var invoice in _iqueryUnPaid)
                {
                    unCompTransCount += invoice.RefCount;
                }
  
                ViewBag.allTrans = allTransCount;
                ViewBag.completedTrans = compTransCount;

                List<ProductDashData> prodmodel = new List<ProductDashData>();

                var transs = new DashboardViewModel
                { 
                    RoleName = roleName,
                    RoleID = user2.Role.RoleID,
                    TotalTransfer = string.Format("{0:#,0}", totalTransfer),     
                    TotalWalletBal = string.Format("{0:#,0}", totalWalBal),
                    TotalBankCharges = string.Format("{0:#,0}", totalBalCharge),
                    CompletedTransactionCount = compTransCount,  
                    UnCompletedTransactionCount = unCompTransCount, 
                    ProductList = prodmodel
                };

                return View(transs);
            }
            if (roleName == "Customer Care")
            {

                //int ID = user2.UserID;
                //var transactions = await _unitOfWork.WalletHistory.GetTransactionsByIDAndValue(ID, 5);

                //List<TransactionHistory> transmodel = new List<TransactionHistory>();

                //foreach (var transaction in transactions)
                //{

                //    transmodel.Add(new TransactionHistory
                //    {
                //        TransactionID = transaction.WalletTransactionID,
                //        TransactionType = transaction.TransactionType.TransactionType,
                //        Amount = transaction.Amount
                //    });

                //}


                var transs = new DashboardViewModel
                {
                    RoleName = roleName,
                    RoleID = user2.Role.RoleID,
                   // HoldAllTransactionHistory = transmodel

                };

                return View(transs);
            }
            else
            {
                var trans = new DashboardViewModel
                {
                    RoleName = roleName,
                    RoleID = user2.Role.RoleID,
                };
                return View(trans);
            }
        }

        //public IActionResult Line()
        //{
        //    Random rnd = new Random();
        //    var lstModel = new List<ChartReporttData>();
        //    lstModel.Add(new ChartReporttData
        //    {
        //        Day = "Sunday",
        //        Amount = 10
        //    });
        //    lstModel.Add(new ChartReporttData
        //    {
        //        Day = "Monday",
        //        Amount = 9
        //    });
        //    lstModel.Add(new ChartReporttData
        //    {
        //        Day = "Tuesday",
        //        Amount = 12
        //    });
        //    lstModel.Add(new ChartReporttData
        //    {
        //        Day = "Wednesday",
        //        Amount = 10
        //    });
        //    lstModel.Add(new ChartReporttData
        //    {
        //        Day = "Thursday",
        //        Amount = 10
        //    });
        //    lstModel.Add(new ChartReporttData
        //    {
        //        Day = "Friday",
        //        Amount = 10
        //    });
        //    lstModel.Add(new ChartReporttData
        //    {
        //        Day = "Saturday",
        //        Amount = 10
        //    });
        //    return View(lstModel);
        //}

         

         
        public async Task<ActionResult> Purchase()
        {
            //checking user's currently logged in
            var user = await _userManager.GetUserAsync(User); 
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            ViewBag.ProductCategory = await ProductCategoryList();
            return View();
        }

        public ActionResult Menu(int? id, string menu = null)
        {
            return LocalRedirect("~/Wallets/WalletServices/Mobile/" + id.ToString());
        }

        public async Task<List<EgsProductCategory>> ProductCategoryList()
        {
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = APIServiceConfig?.Url;

            List<EgsProductCategory> prodCategoryList = new List<EgsProductCategory>();

            using (var client = new HttpClient())
            {
                var Srequest = (Baseurl + "/api/ProductCategory/", "", "GET");
                string IResponse = General.MakeRequest(Baseurl + "/api/ProductCategory/", "", "GET");

                //Save Request and Response
                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                string responseToCaller = string.Empty;

                if (IResponse.ToString() == "No data returned" || IResponse.ToString() == "[]" || IResponse.ToString() == "500")
                {
                    ViewBag.Message = IResponse.ToString();
                    //return View();
                }

                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                prodCategoryList = jsSerializer.Deserialize<List<EgsProductCategory>>(IResponse);
            }

            return prodCategoryList;
        }

        public async Task<List<EgsProduct>> ProductList(int? id)
        {
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = APIServiceConfig?.Url;


            List<EgsProduct> productList = new List<EgsProduct>();

            using (var client = new HttpClient())
            {
                var Srequest = (Baseurl + "/api/Product/GetProductByCategoryId/{categoryid}/" + id.ToString(), "", "POST");
                string IResponse = General.MakeRequest(Baseurl + "/api/Product/GetProductByCategoryId/{categoryid}/" + id.ToString(), "", "POST");

                //Save Request and Response
                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                string responseToCaller = string.Empty;

                if (IResponse.ToString() == "No data returned" || IResponse.ToString() == "[]" || IResponse.ToString() == "500")
                {
                    ViewBag.Message = IResponse.ToString();
                    //return View();
                }

                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                productList = jsSerializer.Deserialize<List<EgsProduct>>(IResponse);
            }

            return productList;
        }

        public async Task<JsonResult> PushCommissionToWallet()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var uemail = user2.Email;
            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
            string Baseurl = EgolePayServiceConfig.Url, IPAddress;
            IPAddress = Helpers.General.GetIPAddressLocal();
            var postResponse = new CommissionPushResponse();

            DateTime MyDateTime = new DateTime();
            //MyDateTime = null;
            
            using (var client = new HttpClient())
            {
                var Srequest = (Baseurl + "/api/AggregatorCommission/AggregatorPaymentPush?aggrId=" + user2.AggregatorID + "&walletId=" + user2.Wallet.WalletID + "&CreatedBy=" + user.UserID.ToString() + "&IpAddress=" + IPAddress, "POST");//+ "/" + uEmail    + "/" + uEmail
                string vfdEnquiryReqParams = "/api/AggregatorCommission/AggregatorPaymentPush?aggrId=" + user2.AggregatorID + "&walletId=" + user2.Wallet.WalletID + "&CreatedBy=" + user.UserID.ToString() + "&IpAddress=" + IPAddress;
                string IResponse = General.MakeVFDRequest(Baseurl, vfdEnquiryReqParams, "POST");
                //AggregatorPaymentPush?aggrId=4&walletId=4&CreatedBy=55&IpAddress=123
                //Save Request and Response
                var logResponse = JsonConvert.DeserializeObject<CommissionPushResponse>(IResponse);

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now,
                    Message = logResponse.Message,
                    StatusCode = logResponse.StatusCode.ToString(),
                    TransactionType = "AggregatorCommisionSelfPush"
                };

                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    postResponse = JsonConvert.DeserializeObject<CommissionPushResponse>(IResponse);
                }

            }

            var data = new
            {
                StatusCode = postResponse.StatusCode,
                Message = postResponse.Message,
                DateCreated = DateTime.Now,
            };

            if(postResponse.StatusCode < 1)
            {
                //var settledInvoices = await GeneralSqlClient.RPT_AggregatorCommission(MyDateTime, MyDateTime, user2.AggregatorID.ToString(), "Aggregator Commission Paid");
                //var commissettledsum = settledInvoices.Sum(u => u.Commisionamount);
                //ViewBag.CommissionPaid = commissettledsum.ToString("N", CultureInfo.InvariantCulture);
                return Json(postResponse.Message);
            }
            else
            {
                return Json(postResponse.Message);
            }

        }

    }
}
