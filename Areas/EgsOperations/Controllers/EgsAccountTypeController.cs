using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Areas.SysSetup.Models;
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
    public class EgsAccountTypeController : Controller
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

        #region logger
        private readonly IEmailSender _mailSender;
        #endregion

        public EgsAccountTypeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: EgsAccountTypeController
        public ActionResult Index()
        {
            return View();
        }


        #region "AccountType"
        //Display view for creating Account Type
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AccountType()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        //Create new Account Type
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AccountType(CreateAccountTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.AccountType.AddSaveAsync(new EgsAccountType
            {
                AccountTypeID = model.AccountTypeID,
                AccountTypeName = model.AccountTypeName,
                DateCreated = DateTime.Now,
                CreatedBy = user2
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Account Type has been created successfully";
            return View();
        }

        //Display view for editing Account Type
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateAccountType(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                var AccountType = _unitOfWork.AccountType.Get((int)id);
                var model = new EgsAccountTypeViewModel
                {
                    AccountTypeID = AccountType.AccountTypeID,
                    AccountTypeName = AccountType.AccountTypeName
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update Account Type
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateAccountType(UpdateAccountTypeViewModel model)
        { 
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.AccountType.UpdateSaveAsync(new EgsAccountType
            {
                AccountTypeID = model.AccountTypeID,
                AccountTypeName = model.AccountTypeName
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Account Type has been updated successfully";
            return View();
        }

        //Display all Account Type
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AccountTypeList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplayAccountTypeViewModel> model = new List<DisplayAccountTypeViewModel>();
                var accounttypes = _unitOfWork.AccountType.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    accounttypes = accounttypes.Where(Accounttype => Accounttype.AccountTypeName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var account in accounttypes)
                {
                    model.Add(new DisplayAccountTypeViewModel
                    {
                        AccountTypeID = account.AccountTypeID,
                        AccountTypeName = account.AccountTypeName,
                        DateCreated = account.DateCreated
                    });
                }
                var pager = new Pager(accounttypes.Count(), pageIndex);

                var modelR = new HoldDisplayAccountTypeViewModel
                {
                    HoldAllAccountType = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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

        #endregion

    }
}