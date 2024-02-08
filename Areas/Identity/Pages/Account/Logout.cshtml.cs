using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;

namespace PaymentGateway21052021.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        #region repositories
        private IUnitOfWork _unitOfWork;
        #endregion

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<LoginModel> _logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<LoginModel> logger, 
            IUnitOfWork unitOfWork)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;

        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    //get the identity of the user
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    //get the system user details
                    var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

                    //_logger.LogInformation("User logged out.");

                    if (returnUrl != null)
                    {
                        await _signInManager.SignOutAsync();
                        return LocalRedirect("/Identity/Account/Login");
                        //return LocalRedirect(returnUrl);

                        //var result = LogLoginOutActivity(user2);

                        //if (result)
                        //{
                        //    //return LocalRedirect("/Identity/Account/Login");
                        //    return LocalRedirect(returnUrl);
                        //}
                    }
                    else
                    {
                        //return RedirectToPage("./Login");
                        await _signInManager.SignOutAsync();
                        return LocalRedirect("/Identity/Account/Login");
                    }
                }

                return LocalRedirect("/Identity/Account/Login");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}