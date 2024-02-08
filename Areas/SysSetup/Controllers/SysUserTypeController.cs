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
    [Authorize(Roles = "Super Admin")]
    public class SysUserTypeController : Controller
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

        public SysUserTypeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: SysUserTypeController
        public ActionResult Index()
        {
            return View();
        }


        #region "UserTypes"
        //Display view for creating up UserTypes
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UserType()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        //Create new UserTypes
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UserType(SysUserTypeVIewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.UserType.AddSaveAsync(new SysUserType
            {
                UserTypeID = model.UserTypeID,
                UserType = model.UserType,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "User Type has been created successfully";
            return View();
        }

        //Display view for editing UserType
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateUserType(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                //var UserType = UnitOfWork.UserType.Get((int)id);
                var UserType = _unitOfWork.UserType.Get((int)id);
                var model = new SysUserTypeVIewModel
                {
                    UserTypeID = UserType.UserTypeID,
                    UserType = UserType.UserType
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update UserType
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateUserType(SysUserTypeVIewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.UserType.UpdateSaveAsync(new SysUserType
            {
                UserTypeID = model.UserTypeID,
                UserType = model.UserType
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "User Type has been updated successfully";
            return View();
        }

        //Display all UserTypes
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UserTypeList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplayUserTypeViewModel> model = new List<DisplayUserTypeViewModel>();
                var UserTypes = _unitOfWork.UserType.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    UserTypes = UserTypes.Where(UserType => UserType.UserType.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var UserType in UserTypes)
                {
                    model.Add(new DisplayUserTypeViewModel
                    {
                        UserTypeID = UserType.UserTypeID,
                        UserType = UserType.UserType,
                        DateCreated = UserType.DateCreated
                    });
                }
                var pager = new Pager(UserTypes.Count(), pageIndex);

                var modelR = new HoldDisplayUserTypeViewModel
                {
                    HoldAllUserTypes = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
