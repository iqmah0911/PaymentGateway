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
    //[Authorize(Roles = "Super Admin")]
    public class SysRoleController : Controller
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

        public SysRoleController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: SysRoleController
        public ActionResult Index()
        {
            return View();
        }


        #region "Roles"
        //Display view for creating up Roles
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Role()
        {
            //var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers("18");//user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(18);   //user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        //Create new Roles
        [HttpPost]
       // [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Role(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers("18");    //user.UserID.ToString());

            var newRole = new SysRole
            {
                IsActive = model.IsActive,
                RoleName = model.RoleName,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            };

            await _unitOfWork.Role.AddAsync(newRole);
            _unitOfWork.Complete();
            //await _unitOfWork.CompleteAsync();
            var role = new ApplicationRole
            {
                Name = model.RoleName,
                RoleID = newRole.RoleID
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                ModelState.Clear();
                ViewBag.Message = "Role has been created successfully";
                return View();
            }

            ModelState.Clear();
            ViewBag.Message = "Role creation failed";
            return View();
        }

        //Display view for editing Role
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateRole(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                var role = _unitOfWork.Role.Get((int)id);
                var model = new CreateRoleViewModel
                {
                    RoleID = role.RoleID, 
                     RoleName= role.RoleName
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.Role.UpdateSaveAsync(new SysRole
            {
                RoleName = model.RoleName,
                DateCreated = model.DateCreated
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Role has been updated successfully";
            return View();
        }

        //Display all Roles
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Roles(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplayRoleViewModel> model = new List<DisplayRoleViewModel>();
                var roles = _unitOfWork.Role.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    roles = roles.Where(role => role.RoleName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var role in roles)
                {
                    model.Add(new DisplayRoleViewModel
                    {
                        RoleID = role.RoleID,
                        RoleName = role.RoleName
                    });
                }
                var pager = new Pager(roles.Count(), pageIndex);

                var modelR = new HoldDisplayRoleViewModel
                {
                    HoldAllRoles = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
