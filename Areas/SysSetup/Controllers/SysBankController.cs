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

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    [Authorize(Roles = "Super Admin")]
    public class SysBankController : Controller
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

        public SysBankController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: SysBankControler
        public ActionResult Index()
        {
            return View();
        }


        #region "SysBank"
        //Display view for creating up SysBank
        [HttpGet]
        //[Authorize()]
        public async Task<IActionResult> Bank()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }


        //Create new SysBank
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize()]
        public async Task<IActionResult> Bank(SysBankViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.Bank.AddSaveAsync(new SysBank
            {
                BankID = model.BankID,
                BankName = model.BankName,
                BankCode = model.BankCode,
                CreatedBy = user2.UserID,
                DateCreated = DateTime.Now
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Bank has been created successfully";
            return View();
        }

        //Display view for editing SysBank
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateBank(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                //var SysBank = UnitOfWork.SysBank.Get((int)id);
                var SysBank = _unitOfWork.Bank.Get((int)id);
                var model = new SysBankViewModel
                {
                    BankID = SysBank.BankID,
                    BankName = SysBank.BankName
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update SysBank
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateBank(SysBankViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.Bank.UpdateSaveAsync(new SysBank
            {
                BankID = model.BankID,
                BankName = model.BankName,
                BankCode = model.BankCode
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Bank has been updated successfully";
            return View();
        }

        //Display all SysBank
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Banks(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplayBankViewModel> model = new List<DisplayBankViewModel>();
                var SysBank = _unitOfWork.Bank.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    SysBank = SysBank.Where(Bank => Bank.BankName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var Bank in SysBank)
                {
                    model.Add(new DisplayBankViewModel
                    {
                        BankID = Bank.BankID,
                        BankName = Bank.BankName,
                        BankCode = Bank.BankCode,
                        DateCreated = Bank.DateCreated
                    });
                }
                var pager = new Pager(SysBank.Count(), pageIndex);

                var modelR = new HoldDisplayBankViewModel
                {
                    HoldAllBank = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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