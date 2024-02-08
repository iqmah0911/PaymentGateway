using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PaymentGateway21052021.Areas.EgsOperations.Controllers
{

    [Area("EgsOperations")]
    [Authorize(Roles = "Super Admin")]
    public class EgsAccountInfoController : Controller
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
        //private readonly IEmailSender _emailSender;


        public EgsAccountInfoController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, /*EmailSender emailSender,*/
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            //_mailSender = emailSender;
            _roleManager = roleManager;
        }


        // GET: EgsAccountInfoController
        public ActionResult Index()
        {
            return View();
        }


        #region "AccountInfos"


        //Display view for creating up AccountInfos
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AccountInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            ViewBag.BankList = await BankList();
            return View();
        }

        //Create new AccountInfos
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AccountInfo(EgsAccountInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var bank = await _unitOfWork.Bank.GetBanks(model.BankID);

            await _unitOfWork.AccountInfo.AddSaveAsync(new EgsAccountsInfo
            {
                AccountsInfoID = model.AccountInfoID,
                AccountNumber = model.AccountNumber,
                Bank = bank,
                CreatedBy = user2.UserID,
                DateCreated = DateTime.Now
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Bank Account Info. has been created successfully";
            ViewBag.BankList = await BankList();
            return View();
        }


        //Display view for editing AccountInfo
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateAccountInfo(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                ViewBag.BankList = await BankList(); //this allow the selection of the dropdown value
                var AccountInfo = await _unitOfWork.AccountInfo.GetAccountInfosWithBankId((int)id);

                var model = new EgsAccountInfoViewModel
                {
                    AccountInfoID = AccountInfo.AccountsInfoID,
                    AccountNumber = AccountInfo.AccountNumber,
                    BankID = AccountInfo.Bank.BankID
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update AccountInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateAccountInfo(EgsAccountInfoViewModel model)
        {
            ViewBag.BankList = await BankList();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var bankId = await _unitOfWork.Bank.GetBanks(model.BankID);

            await _unitOfWork.AccountInfo.UpdateSaveAsync(new EgsAccountsInfo
            {
                AccountNumber = model.AccountNumber,
                Bank = bankId
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Bank Account Info. has been updated successfully";
            ViewBag.BankList = await BankList();
            return View();
        }

        //Display all AccountInfos
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AccountInfoList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplayAccountInfoViewModel> model = new List<DisplayAccountInfoViewModel>();
                var AccountInfos = await _unitOfWork.AccountInfo.GetAllAccountInfos();
                //var Products = _unitOfWork.Products.GetAll();
                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    AccountInfos = AccountInfos.Where(AccountInfo => AccountInfo.AccountNumber.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var AccountInfo in AccountInfos)
                {
                    model.Add(new DisplayAccountInfoViewModel
                    {
                        AccountInfoID = AccountInfo.AccountsInfoID,
                        AccountNumber = AccountInfo.AccountNumber,
                        BankName = AccountInfo.Bank.BankName,
                        DateCreated = AccountInfo.DateCreated
                    });
                }
                var pager = new Pager(AccountInfos.Count(), pageIndex);

                var modelR = new HoldDisplayAccountInfoViewModel
                {
                    HoldAllAccountInfo = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };

                return View(modelR);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Method to pull Products 
        public async Task<List<SysBank>> BankList()
        {
            List<SysBank> BanksList = new List<SysBank>();

            BanksList = await _unitOfWork.Bank.GetAllBanks();


            BanksList.Insert(0, new SysBank { BankID = 0, BankName = "--Select Bank--" });

            return BanksList;
        }


        #endregion

    }
}
