using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Controllers
{
    [Area("EgsOperations")]
    [Authorize]
    //[Authorize(Roles = "Customer Reconcilation,Super Admin, Administrator")]
    public class EgsWalletController : Controller
    {

        string token = "bcb81916-4f8d-3407-882c-d88d5a342382";
        string walletcred = "Q3dhVDRjSkkwdjl0ZkhMMXFpXzN5Q1dhXzNZYTp1d0Fhb0Q3X1BzZGNWWVVWa3hGUHFMWnVGMFlh";

        #region logger
        private readonly ILogger<dynamic> _logger;
        #endregion

        #region repositories
        private IUnitOfWork _unitOfWork;
        #endregion

        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;
        //private readonly IEmailSender _emailSender;
        private IHttpContextAccessor _accessor;
        
        #region logger
        private readonly IEmailSender _mailSender;
        #endregion

        public EgsWalletController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, /*EmailSender emailSender,*/
            RoleManager<ApplicationRole> roleManager, IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            //_mailSender = emailSender;
            _roleManager = roleManager;
            _accessor = accessor;
        }


        // GET: EgsWalletControler
        public ActionResult Index()
        {
            return View();
        }


        #region "EgsWallet"
        [HttpGet]
        //[Authorize(Roles = "Agent")]
        public async Task<IActionResult> Wallet()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }


        //Create new EgsWallet
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize()]
        public async Task<IActionResult> Wallet(WalletViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.Wallet.AddSaveAsync(new EgsWallet
            {
                WalletID = model.WalletID,
                WalletAccountNumber = model.WalletAccountNumber,
                User = user2,
                DateCreated = DateTime.Now
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Wallet has been created successfully";
            return View();
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateWallet(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                //var wallet = _unitOfWork.Wallet.Get((int)id);
                //var userByWallettId =
                var wallet = await _unitOfWork.Wallet.GetUserByWalletID((int)id);

                var model = new UpdateWalletViewModel
                {
                    WalletID = wallet.WalletID,
                    WalletAccountNumber = wallet.WalletAccountNumber,
                    FirstName = wallet.User.FirstName,
                    LastName = wallet.User.LastName,
                    IsActive = wallet.IsActive
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateWallet(UpdateWalletViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.Wallet.UpdateSaveAsync(new EgsWallet
            {
                WalletID = model.WalletID,
                WalletAccountNumber = model.WalletAccountNumber,

                IsActive = model.IsActive,
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Wallet has been updated successfully";
            return View();
        }



        [HttpGet]
        //[Authorize(Roles = "Agent")]
        public async Task<IActionResult> FundWallet()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            //var user = await _userManager.GetUserAsync(User);
            //var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //var check = await _unitOfWork.Wallet.GetWalletByUserID(user2.UserID);

            //ViewBag.WalletId = check.WalletID;
            ViewBag.TransactionMethodList = await TransactionMethodList();
            return View();
        }


        [HttpPost]
        //[Authorize(Roles = "Agent")]
        public async Task<IActionResult> FundWallet(FundWalletViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var compmerchant = _unitOfWork.Merchant.GetMerchantsById(9); //EgolePay merchantid
            var transactionType = _unitOfWork.TransactionType.Get(1);
            var transactionMethod = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(model.TransactionMethodID);
            string fromAccount = compmerchant.Result.Merchant.AccountNo;
            string fromClientId = compmerchant.Result.Merchant.KYC;//"1000047669", toAccount = "0087611190";
            string fromClient = compmerchant.Result.CompanyName;
            string fromSavingsId = compmerchant.Result.Merchant.MerchantCode;
            string transReference = "EgolePay_Transfer" + Guid.NewGuid().ToString("N").Substring(0, 3);

            var merchant = _unitOfWork.Merchant.GetMerchantsById(8); //merchantid to settlement
            string toAccount = merchant.Result.Merchant.AccountNo;
            string toBank = "999999";//merchant.Result.Merchant.Bank.BankCode;

            string bodyRequest, vfdEnquiryUrl, vfdEnquiryReqParams, IResponse = "";
            VFDResponse aregResponse = new VFDResponse();
            vfdEnquiryUrl = "https://devesb.vfdbank.systems:8263/vfd-wallet/1.1/wallet2/transfer/recipient?";
            vfdEnquiryReqParams = "transfer_type=inter&accountNo=" + toAccount + "&bank=" + toBank + "&wallet-credentials=" + walletcred;

            VFDRecipient reciResponse = new VFDRecipient();

            using (var _client = new HttpClient())
            {
                var Paramsrequest = (vfdEnquiryUrl + vfdEnquiryReqParams, "", "POST");
                IResponse = General.MakeVFDRequest(vfdEnquiryUrl, vfdEnquiryReqParams, "POST", token);

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Paramsrequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    reciResponse = JsonConvert.DeserializeObject<VFDRecipient>(IResponse);

                    if (Convert.ToInt32(aregResponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + aregResponse.message;
                        return View();
                    }
                }

            }

            string toClient = reciResponse.data.name;
            string toKyc = reciResponse.data.status;
            toAccount = reciResponse.data.account.number;
            string hashSignature = General.Get512Hash(fromAccount + toAccount);
            //toBank = reciResponse.data.bank;

            VFDTransferData transBody = new VFDTransferData()
            {
                fromSavingsId = fromSavingsId,
                amount = "100",
                toAccount = toAccount,
                fromBvn = "cout-222222",
                signature = hashSignature,
                fromAccount = fromAccount,
                toBvn = "22277733344",
                remark = "fundWallet Test Other",
                fromClientId = fromClientId,
                fromClient = fromClient,
                toKyc = toKyc,
                reference = transReference,
                toClientId = "",
                toClient = toClient,
                toSession = "999116188817221154270626748777",
                transferType = "inter",
                toBank = toBank,
                toSavingsId = ""
            };
            //On transfer success then update log and summary

            bodyRequest = JsonConvert.SerializeObject(transBody, Formatting.Indented);
            //VFDResponse aregResponse = new VFDResponse();
            vfdEnquiryUrl = "https://devesb.vfdbank.systems:8263/vfd-wallet/1.1/wallet2/transfer";
            vfdEnquiryReqParams = "?wallet-credentials=Q3dhVDRjSkkwdjl0ZkhMMXFpXzN5Q1dhXzNZYTp1d0Fhb0Q3X1BzZGNWWVVWa3hGUHFMWnVGMFlh&source=pool";


            using (var _client = new HttpClient())
            {
                IResponse = General.MakeVFDRequest(vfdEnquiryUrl, vfdEnquiryReqParams, "POST", token, bodyRequest);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    aregResponse = JsonConvert.DeserializeObject<VFDResponse>(IResponse);

                    if (Convert.ToInt32(aregResponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + aregResponse.message;
                        return View();
                    }
                }

            }

            await _unitOfWork.WalletTransaction.AddSaveAsync(new EgsWalletTransaction
            {
                Wallet = user2.Wallet,
                Amount = model.Amount,
                TransactionDate = DateTime.Now,
                TransactionType = transactionType,
                TransactionMethod = transactionMethod

            });

            ModelState.Clear();
            ViewBag.Message = "Wallet has been funded successfully";
            ViewBag.TransactionMethodList = await TransactionMethodList();
            return View();
        }

        //Method to pull Agents

        public async Task<List<DDListView>> AgentList()
        {
            List<DDListView> AgentList = new List<DDListView>();
            var agents = await _unitOfWork.User.GetAllAgents();

            foreach (var agentItems in agents)
            {
                AgentList.Add(new DDListView
                {
                    itemValue = agentItems.UserID,
                    itemName = agentItems.FirstName + " " + agentItems.LastName + " (" + Helpers.General.MaskString(agentItems.Wallet.WalletAccountNumber, 3, "****") + ")"
                });
            }

            DropDownModelView nAgentlist = new DropDownModelView();
            nAgentlist.items = AgentList;

            return nAgentlist.items;
        }


        public async Task<List<DDListView>> CourtevilleAgentList()
        {
            List<DDListView> AgentList = new List<DDListView>();
            //var agents = await _unitOfWork.User.GetAllAgents();
            var agents = await _unitOfWork.User.GetAllCourtevilleAgents();


            foreach (var agentItems in agents)
            {
                AgentList.Add(new DDListView
                {
                    itemValue = agentItems.UserID,
                    itemName = agentItems.FirstName + " " + agentItems.LastName + " (" + Helpers.General.MaskString(agentItems.Wallet.WalletAccountNumber, 3, "****") + ")"
                });
            }

            DropDownModelView nAgentlist = new DropDownModelView();
            nAgentlist.items = AgentList;

            return nAgentlist.items;
        }


        public async Task<List<TMListView>> TransactionMethodList()
        {
            List<TMListView> TransactionMethodList = new List<TMListView>();
            var transMethod = await _unitOfWork.TransactionMethod.GetAllTransactionMethod();

            foreach (var transMethd in transMethod)
            {
                TransactionMethodList.Add(new TMListView
                {
                    itemValue = transMethd.TransactionMethodID,
                    itemName = transMethd.TransactionMethod
                });
            }

            TransMethodModelView nTransactionMethodlist = new TransMethodModelView();
            nTransactionMethodlist.items = TransactionMethodList;

            return nTransactionMethodlist.items;
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin ")]
        //[Authorize(Roles = "Customer Reconcilation,Super Admin, Administrator")]
        public async Task<IActionResult> FundWalletAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            //var user = await _userManager.GetUserAsync(User);
            //var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //var check = await _unitOfWork.Wallet.GetWalletByUserID(user2.UserID);

            if (user2.Role.RoleID == 13)
            { 
                ViewBag.CAgentList = await CourtevilleAgentList();
                ViewBag.role = user2.Role.RoleID;
                return View();
            }
            if (user2.Role.RoleID == 2) { 
                ViewBag.role = user2.Role.RoleID;
            //ViewBag.WalletId = check.WalletID;
            ViewBag.AgentList = await AgentList();
            return View();
            }
            return View("AccessDenied");
        }

            
        [HttpPost]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> FundWalletAdmin(FundWalletAdminViewModel model)
        {

            string ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (!ModelState.IsValid)
            {
                ViewBag.AgentList = await AgentList();
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
            string Baseurl = EgolePayServiceConfig.Url;
            string producturl = Baseurl + "/api/notification/fundwallet";
            var senuser = await _unitOfWork.User.GetSysUsers(model.UserID.ToString());

            var data = new FundWalletNotificationViewModel
            {
                walletAccountnumber = senuser.Wallet.WalletAccountNumber,          //user2.Wallet.WalletAccountNumber,
                TransactionReference = model.TransactionRef,
                Amount = model.Amount,
                TimeStamp = DateTime.Now.ToString(),
                Narration = model.Narration,//"Wallet Refund due to service downtime",
                CreatedBy = user.UserID,
                ipAddress = ipAddress,
            };


            string bodyReqParams = JsonConvert.SerializeObject(data, Formatting.Indented); //model.ReferenceNo;

            var aregResponse = new FundPaymentResponse();

            using (var _client = new HttpClient())
            {
                var Srequest = (producturl, bodyReqParams, "POST");
                string IResponse = General.MakeVFDRequest(producturl, null, "POST", null, bodyReqParams);

                var logResponse = JsonConvert.DeserializeObject<FundPaymentResponse>(IResponse);

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now,
                    Message = logResponse.message,
                    StatusCode = logResponse.status,
                    TransactionType = "AutoReg-Reference-Payment"
                };

                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);
                if (!String.IsNullOrEmpty(IResponse))
                {
                    aregResponse = JsonConvert.DeserializeObject<FundPaymentResponse>(IResponse);

                    if (Convert.ToInt32(aregResponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = aregResponse.message;
                        ViewBag.AgentList = await AgentList();
                        return View(model);
                    }
                }

            }

            ModelState.Clear();
            ViewBag.Message = "Wallet has been funded successfully";
            ViewBag.AgentList = await AgentList();
            return View();
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Wallets(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplayWalletViewModel> model = new List<DisplayWalletViewModel>();
                var wallets = await _unitOfWork.Wallet.GetWallets();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    wallets = wallets.Where(Wallet => Wallet.WalletAccountNumber.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var wallet in wallets)
                {
                    model.Add(new DisplayWalletViewModel
                    {
                        WalletID = wallet.WalletID,
                        WalletAccountNumber = wallet.WalletAccountNumber,
                        FirstName = wallet.User.FirstName,
                        LastName = wallet.User.LastName,
                        IsActive = wallet.IsActive,
                        DateCreated = wallet.DateCreated
                    });
                }
                var pager = new Pager(wallets.Count(), pageIndex);

                var modelR = new HoldDisplayWalletViewModel
                {
                    HoldAllWallet = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };

                return View(modelR);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            {
                return View();
            }
        }
    }
}
#endregion

