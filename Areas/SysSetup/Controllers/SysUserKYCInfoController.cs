using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    //[Authorize("Super Admin")]
    [Authorize(Roles = "Super Admin")]

    public class SysUserKYCInfoController : Controller
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
        //private readonly IEmailSender _mailSender;
        #endregion

        public SysUserKYCInfoController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: SysUserKYCInfoController
        public ActionResult Index()
        {
            return View();
        }


        #region "UserKYCInfo"
        //Display view for creating up UserKYCInfo
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UserKYCInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            ViewBag.IdentificationTypeList = await IdentificationTypeList();
            return View();
        }

        //Create new UserKYCInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UserKYCInfo(CreateUserKYCInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var identificationType = await _unitOfWork.IdentificationType.GetIdentificationType(model.IdentificationTypeID);

            await _unitOfWork.UserKYCInfo.AddSaveAsync(new SysUserKycInfo
            {
                UserKYCID = model.UserKYCInfoID,
                IdentificationType = identificationType,
                IdentificationValue = model.IdentificationValue, 
                BankAccount = model.BankAccount, 
                DateCreated = DateTime.Now,
                Createdby = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.IdentificationTypeList = await IdentificationTypeList();
            ViewBag.Message = "User KYC Info has been created successfully";
            return View();
        }

        //Display view for editing UserKYCInfo
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateUserKYCInfo(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                ViewBag.IdentificationTypeList = await IdentificationTypeList();
                var userkyc = _unitOfWork.UserKYCInfo.Get((int)id);
                var identificationType = await _unitOfWork.UserKYCInfo.GetIdentificationTypeById((int)id);
                var model = new UpdateUserKYCInfoViewModel
                { 
                    UserKYCInfoID = userkyc.UserKYCID, 
                    IdentificationType = identificationType.IdentificationType.IdentificationTypeName,
                    IdentificationValue = userkyc.IdentificationValue
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update UserKYCInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateUserKYCInfo(UpdateUserKYCInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var identificationType = await _unitOfWork.IdentificationType.GetIdentificationType(model.IdentificationTypeID);
            
            await _unitOfWork.UserKYCInfo.UpdateSaveAsync(new SysUserKycInfo
            {
                UserKYCID = model.UserKYCInfoID,
                IdentificationType = identificationType,
                IdentificationValue = model.IdentificationValue, 
                DateCreated = model.DateCreated
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.IdentificationTypeList = await IdentificationTypeList();
            ViewBag.Message = "UserKYCInfo has been updated successfully";
            return View();
        }

        //Display all UserKYCInfos
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UserKYCInfos(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                ViewBag.IdentificationTypeList = await IdentificationTypeList();
                List<DisplayUserKYCInfoViewModel> model = new List<DisplayUserKYCInfoViewModel>();
                var userkycs = _unitOfWork.UserKYCInfo.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    userkycs = userkycs.Where(userkyc => userkyc.IdentificationType.IdentificationTypeName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var userkyc in userkycs)
                {
                    model.Add(new DisplayUserKYCInfoViewModel
                    {
                        UserKYCInfoID = userkyc.UserKYCID, 
                        IdentificationType = userkyc.IdentificationType.IdentificationTypeName, 
                        IdentificationValue = userkyc.IdentificationValue
                    });
                }
                var pager = new Pager(userkycs.Count(), pageIndex);

                var modelR = new HoldDisplayUserKYCInfoViewModel
                {
                    HoldAllUserKYCInfo = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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

        public async Task<List<SysIdentificationType>> IdentificationTypeList()
        {
            List<SysIdentificationType> identificationTypeList = new List<SysIdentificationType>();

            identificationTypeList = await _unitOfWork.IdentificationType.GetAllIdentificationType();

            identificationTypeList.Insert(0, new SysIdentificationType { IdentificationTypeID = 0, IdentificationTypeName = "--Select Identification Type--" });

            return identificationTypeList;
        }

        #endregion

    }
}
