﻿using Microsoft.AspNetCore.Authorization;
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
    public class SysTransactionTypeController : Controller
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

        public SysTransactionTypeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: SysTransactionTypeController
        public ActionResult Index()
        {
            return View();
        }


        #region "SysTransactionType"
        //Display view for creating up SysTransactionType
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> TransactionType()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }


        //Create new SysTransactionType
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> TransactionType(SysTransactionTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.TransactionType.AddSaveAsync(new SysTransactionType
            {
                TransactionTypeID = model.TransactionTypeID,
                TransactionType = model.TransactionType,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "TransactionType has been created successfully";
            return View();
        }

        //Display view for editing SysBank
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateTransactionType(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                //var SysTransactionType = UnitOfWork.SysTransactionType.Get((int)id);
                var transactionType = _unitOfWork.TransactionType.Get((int)id);
                var model = new SysTransactionTypeViewModel
                {
                    TransactionTypeID = transactionType.TransactionTypeID,
                    TransactionType = transactionType.TransactionType
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update SysTransactionType
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateTransactionType(SysTransactionTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.TransactionType.UpdateSaveAsync(new SysTransactionType
            {
                TransactionTypeID = model.TransactionTypeID,
                TransactionType = model.TransactionType
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Transaction Type has been updated successfully";
            return View();
        }

        //Display all TransactionType
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> TransactionTypeList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplayTransactionTypeViewModel> model = new List<DisplayTransactionTypeViewModel>();
                var SysTransactionType = _unitOfWork.TransactionType.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    SysTransactionType = SysTransactionType.Where(TransactionType => TransactionType.TransactionType.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var TransactionType in SysTransactionType)
                {
                    model.Add(new DisplayTransactionTypeViewModel
                    {
                        TransactionTypeID = TransactionType.TransactionTypeID,
                        TransactionType = TransactionType.TransactionType,
                        DateCreated = TransactionType.DateCreated
                    });
                }
                var pager = new Pager(SysTransactionType.Count(), pageIndex);

                var modelR = new HoldDisplayTransactionTypeViewModel
                {
                    HoldAllTransactionType = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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