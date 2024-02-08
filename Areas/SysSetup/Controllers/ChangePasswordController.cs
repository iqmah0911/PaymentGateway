using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers.Interface;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    [Authorize]
    public class ChangePasswordController : Controller
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
        private readonly IMimeSender _mimeSender;


        public ChangePasswordController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMimeSender mimeSender,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, IEmailSender emailSender,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _mimeSender = mimeSender;
        }

        public IActionResult Index()
        {
            return View();
        }



        //Default View Page for System User
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        //Create a new System User
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Please fill in the required fields.");
                }

                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var systemUserViewModel = new ChangePasswordViewModel();

                var usernameExists = await _unitOfWork.User.GetUserByChangePassword(user2);


                if (usernameExists != null)
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {
                        foreach (var error in changePasswordResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        ViewBag.ErrorMessage = "Incorrect Password";
                        return View();
                    }

                    await _signInManager.RefreshSignInAsync(user);

                    //await _emailSender.SendEmailAsync(user2.Email,
                    // "Password Change",
                    // $"Please be informed that your password has been changed and your new password is " + model.NewPassword +"<br/> Regards");

            await _mimeSender.Execute("", "Password Change",$"Please be informed that your password has been changed and your new password is " + model.NewPassword + "<br/> Regards",
              user2.Email);

                    ViewBag.Message = "Password has been changed successfully";

                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "Username or Email already exist";
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }

            return View();
        }




    }
}
