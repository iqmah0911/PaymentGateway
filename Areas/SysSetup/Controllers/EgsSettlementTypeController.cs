
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using PaymentGateway21052021.Repositories.SysSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers   
{
    [Area("SysSetup")]
    //[Authorize(Roles = "Super Admin")]
    public class EgsSettlementTypeController : Controller
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

        public EgsSettlementTypeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: EgsSettlementTypeController
        public ActionResult Index()
        {
            return View();
        }


        #region "EgsSettlementType"
        //Display view for creating up EgsSettlementType
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SettlementTypeAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }


        //Create new EgsSettlementType
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SettlementType(EgsSettlementTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.SettlementType.AddSaveAsync(new EgsSettlementType
            {
                SettlementTypeID = model.SettlementTypeID,
                SettlementType = model.SettlementType,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "SettlementType has been created successfully";
            return View();
        }

        //Display view for editing SysBank
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateEgsSettlementType(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                //var EgsSettlementType = UnitOfWork.EgsSettlementType.Get((int)id);
                var EgsSettlementType = _unitOfWork.SettlementType.Get((int)id);
                var model = new EgsSettlementTypeViewModel
                {
                    SettlementTypeID = EgsSettlementType.SettlementTypeID,
                    SettlementType = EgsSettlementType.SettlementType
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update EgsSettlementType
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateSettlementType(EgsSettlementTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.SettlementType.UpdateSaveAsync(new EgsSettlementType
            {
                SettlementTypeID = model.SettlementTypeID,
                SettlementType = model.SettlementType
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "SettlementType has been updated successfully";
            return View();
        }

        //Display all SettlementType
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SettlementTypeList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplaySettlementTypeViewModel> model = new List<DisplaySettlementTypeViewModel>();
                var EgsSettlementType = _unitOfWork.SettlementType.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    EgsSettlementType = EgsSettlementType.Where(SettlementType => SettlementType.SettlementType.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var SettlementType in EgsSettlementType)
                {
                    model.Add(new DisplaySettlementTypeViewModel
                    {
                        SettlementTypeID = SettlementType.SettlementTypeID,
                        SettlementType = SettlementType.SettlementType,
                        DateCreated = SettlementType.DateCreated
                    });
                }
                var pager = new Pager(EgsSettlementType.Count(), pageIndex);

                var modelR = new HoldDisplaySettlementTypeViewModel
                {
                    HoldAllSettlementType = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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