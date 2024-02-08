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
using static PaymentGateway21052021.Areas.SysSetup.Models.EgsSettlementHistoryViewModel;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
   // [Authorize(Roles = "Super Admin")]
    public class EgsSettlementHistoryController : Controller
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
        private readonly IEmailSender _emailSender;

        public EgsSettlementHistoryController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, /*EmailSender emailSender,*/
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
         //   _mailSender = emailSender;
            _roleManager = roleManager;
        }


      //  GET: EgsProductCategory
        public ActionResult Index()
        {
            return View();
        }


        #region "SettlementHistory"
        //Display view for creating up SettlementHistory
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SettlementHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        //Create new SettlementHistory
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SettlementHistory(EgsSettlementHistoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.SettlementHistory.AddSaveAsync(new EgsSettlementHistory
            {
                SettlementHistoryID = model.SettlementHistoryID,
                SettlementAmount = model.SettlementAmount,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Settlement has been created successfully";
            return View();
        }

        //Display view for editing SettlementHistory
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateSettlementHistory(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                //var SettlementHistory = UnitOfWork.SettlementHistory.Get((int)id);
                var SettlementHistory = await _unitOfWork.SettlementHistory.GetSettlementHistoryByID((int)id);
                var model = new EgsSettlementHistoryViewModel
                {
                    SettlementHistoryID = SettlementHistory.SettlementHistoryID,
                    SettlementAmount = SettlementHistory.SettlementAmount,
                    //SettlementHistory = SettlementHistory.SettlementHistory,
                    //TransactionBalance = SettlementHistory.TransactionBalance,
                    DateCreated = SettlementHistory.DateCreated
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update SettlementHistory
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateSettlementHistory(EgsSettlementHistoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.SettlementHistory.UpdateSaveAsync(new EgsSettlementHistory
            {
                SettlementHistoryID = model.SettlementHistoryID,
                SettlementAmount = model.SettlementAmount,
                //SettlementHistory = model.SettlementHistory,
                //TransactionBalance = model.TransactionBalance
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Settlement Limit has been updated successfully";
            return View();
        }

        //Display all SettlementHistory
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SettlementHistoryList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplaySettlementHistoryViewModel> model = new List<DisplaySettlementHistoryViewModel>();
                var SettlementHistory = await _unitOfWork.SettlementHistory.GetSettlementHistory(); //GetSettlementHistoryByID();

                //Logic for search
                //if (!String.IsNullOrEmpty(searchText))
                //{
                //    SettlementHistory = SettlementHistory.Where(_SettlementHistory =>  _SettlementHistory.SettlementAmount.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                //}
                
                ////load up the list of viewmodels 
                //foreach (var SettlementAmount in SettlementHistory)
                //{
                //    model.Add(new DisplaySettlementHistoryViewModel
                //    {
                //        SettlementHistoryID = SettlementHistory.SettlementHistoryID,
                //        SettlementAmount = SettlementHistory.SettlementAmount,
                //        //SettlementHistory = SettlementHistory.SettlementHistory,
                //        //TransactionBalance = SettlementHistory.TransactionBalance,
                //        DateCreated = SettlementHistory.DateCreated
                //    });
                //}
                var pager = new Pager(SettlementHistory.Count(), pageIndex);

                var modelR = new HoldDisplaySettlementHistoryViewModel
                {
                    HoldAllSettlementHistory = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
