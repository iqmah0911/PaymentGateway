﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
   // [Authorize(Roles = "Super Admin")]
    public class EgsSettlementIntervalController : Controller
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

        public EgsSettlementIntervalController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: EgsSettlementInterval
        public ActionResult Index()
        {
            return View();
        }


        #region "SettlementInterval"

        //Display view for creating up SettlementInterval
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SettlementInterval()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        //Create new SettlementInterval
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SettlementInterval(EgsSettlementIntervalViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.SettlementInterval.AddSaveAsync(new EgsSettlementInterval
            {
                SettlementIntervalID = model.SettlementIntervalID,
                SettlementIntervalName = model.SettlementIntervalName,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Settlement Interval has been created successfully";
            return View();
        }

        //Display view for editing SettlementInterval
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateSettlementInterval(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                var SettlementInterval = _unitOfWork.SettlementInterval.Get((int)id);
                var model = new EgsSettlementIntervalViewModel
                {
                    SettlementIntervalID = SettlementInterval.SettlementIntervalID,
                    SettlementIntervalName = SettlementInterval.SettlementIntervalName
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update SettlementInterval
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateSettlementInterval(EgsSettlementIntervalViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.SettlementInterval.UpdateSaveAsync(new EgsSettlementInterval
            {
                SettlementIntervalID = model.SettlementIntervalID,
                SettlementIntervalName = model.SettlementIntervalName
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Settlement Interval has been updated successfully";
            return View();
        }

        //Display all SettlementInterval
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SettlementIntervalList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplaySettlementIntervalViewModel> model = new List<DisplaySettlementIntervalViewModel>();
                var SettlementInterval = _unitOfWork.SettlementInterval.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    SettlementInterval = SettlementInterval.Where(SettlementIntervalLike => SettlementIntervalLike.SettlementIntervalName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var settlementInterval in SettlementInterval)
                {
                    model.Add(new DisplaySettlementIntervalViewModel
                    {
                        SettlementIntervalID = settlementInterval.SettlementIntervalID,
                        SettlementIntervalName = settlementInterval.SettlementIntervalName
                    });
                }
                var pager = new Pager(SettlementInterval.Count(), pageIndex);

                var modelR = new HoldDisplaySettlementIntervalViewModel
                {
                    HoldAllSettlementInterval = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
