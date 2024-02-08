using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PaymentGateway21052021.Areas.SysSetup.Models.SysTransactionLimitViewModel;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    //[Authorize(Roles = "Super Admin")]
    public class SysTransactionLimitController : Controller
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

        public SysTransactionLimitController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: EgsProductCategory
        public ActionResult Index()
        {
            return View();
        }


        #region "TransactionLimits"
        //Display view for creating up TransactionLimits
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> TransactionLimit()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        //Create new TransactionLimits
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> TransactionLimit(SysTransactionLimitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.TransactionLimit.AddSaveAsync(new SysTransactionLimit
            {
                TransactionLimitID = model.TransactionLimitID,
                TransactionLimitName = model.TransactionLimitName,
                TransactionLimit = model.TransactionLimit,
                TransactionBalance = model.TransactionBalance,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Transaction Limit has been created successfully";
            return View();
        }

        //Display view for editing TransactionLimit
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateTransactionLimit(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                //var TransactionLimit = UnitOfWork.TransactionLimit.Get((int)id);
                var TransactionLimit = await _unitOfWork.TransactionLimit.GetTransactionLimit((int)id);
                var model = new SysTransactionLimitViewModel
                {
                    TransactionLimitID = TransactionLimit.TransactionLimitID,
                    TransactionLimitName = TransactionLimit.TransactionLimitName,
                    TransactionLimit = TransactionLimit.TransactionLimit,
                    TransactionBalance = TransactionLimit.TransactionBalance,
                    DateCreated = TransactionLimit.DateCreated
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update TransactionLimit
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateTransactionLimit(SysTransactionLimitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.TransactionLimit.UpdateSaveAsync(new SysTransactionLimit
            {
                TransactionLimitID = model.TransactionLimitID,
                TransactionLimitName = model.TransactionLimitName,
                TransactionLimit = model.TransactionLimit,
                TransactionBalance = model.TransactionBalance
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Transaction Limit has been updated successfully";
            return View();
        }

        //Display all TransactionLimits
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> TransactionLimitList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplayTransactionLimitViewModel> model = new List<DisplayTransactionLimitViewModel>();
                var TransactionLimits = await _unitOfWork.TransactionLimit.GetAllTransactionLimit();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    TransactionLimits = TransactionLimits.Where(TransactionLimit => TransactionLimit.TransactionLimitName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var TransactionLimit in TransactionLimits)
                {
                    model.Add(new DisplayTransactionLimitViewModel
                    {
                        TransactionLimitID = TransactionLimit.TransactionLimitID,
                        TransactionLimitName = TransactionLimit.TransactionLimitName,
                        TransactionLimit = TransactionLimit.TransactionLimit,
                        TransactionBalance = TransactionLimit.TransactionBalance,
                        DateCreated = TransactionLimit.DateCreated
                    });
                }
                var pager = new Pager(TransactionLimits.Count(), pageIndex);

                var modelR = new HoldDisplayTransactionLimitViewModel
                {
                    HoldAllTransactionLimits = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
