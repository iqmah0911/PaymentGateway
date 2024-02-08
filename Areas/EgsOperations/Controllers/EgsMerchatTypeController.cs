using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("EgsOperations")]
    //[Authorize]
    public class EgsMerchantTypeController : Controller
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

        public EgsMerchantTypeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: EgsMerchantTypeController
        public ActionResult Index()
        {
            return View();
        }


        #region "MerchantTypes"
        //Display view for creating up MerchantTypes
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> MerchantType()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        //Create new MerchantTypes MerchantType
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> MerchantType(EgsMerchantTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.MerchantType.AddSaveAsync(new EgsMerchantType
            {
                MerchantTypeID = model.MerchantTypeID,
                MerchantType = model.MerchantTypeName,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "User Type has been created successfully";
            return View();
        }

        //Display view for editing MerchantType
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateMerchantType(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                //var MerchantType = UnitOfWork.MerchantType.Get((int)id);
                var MerchantType = _unitOfWork.MerchantType.Get((int)id);
                var model = new EgsMerchantTypeViewModel
                {
                    MerchantTypeID = MerchantType.MerchantTypeID,
                    MerchantTypeName = MerchantType.MerchantType
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update MerchantType
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateMerchantType(EgsMerchantTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.MerchantType.UpdateSaveAsync(new EgsMerchantType
            {
                MerchantTypeID = model.MerchantTypeID,
                MerchantType = model.MerchantTypeName
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "User Type has been updated successfully";
            return View();
        }

        //Display all MerchantTypes
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> MerchantTypeList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<EgsMerchantTypeViewModel> model = new List<EgsMerchantTypeViewModel>();
                var MerchantTypes = _unitOfWork.MerchantType.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    MerchantTypes = MerchantTypes.Where(MerchantType => MerchantType.MerchantType.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var MerchantType in MerchantTypes)
                {
                    model.Add(new EgsMerchantTypeViewModel
                    {
                        MerchantTypeID = MerchantType.MerchantTypeID,
                        MerchantTypeName = MerchantType.MerchantType,
                        DateCreated = MerchantType.DateCreated
                    });
                }
                var pager = new Pager(MerchantTypes.Count(), pageIndex);

                var modelR = new HoldDisplayMerchantTypeViewModel
                {
                    HoldAllMerchantType = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
