using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using Microsoft.AspNetCore.Http;

namespace PaymentGateway21052021.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<LoginModel> _logger;
        private IHttpContextAccessor _accessor;
        private IUnitOfWork _unitOfWork;


        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            RoleManager<ApplicationRole> roleManager,
            ILogger<LoginModel> logger,
            IHttpContextAccessor accessor)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _accessor = accessor;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            public string strEmail { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public class ProductModel
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public DateTime DateCreated { get; set; }
            public int CreatedBy { get; set; }
            public int ProductCategoryID { get; set; }
            public string ProductCategory { get; set; }

        }

        public async Task OnGetAsync(int? productid, string returnUrl = null, string loginType = "0")
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            //Input.strEmail = Helpers.General.MaskString("1003869277", 3, "****");
            TempData["LoginType"] = loginType;
            TempData["ProdctID"] = productid;

            //TempData["MaskString"] = Helpers.General.MaskString("1003869277", 3, "****");
            //TempData["ProdctI_D"] = TempData["MaskString"].ToString();
            returnUrl = returnUrl ?? Url.Content("~/");
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

          // var getInvoiceDetails = await _unitOfWork.Invoice.GetReferenceNo("639142022701139259");


            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var key = "b14ca5898a4e4133wale2ea2315a1916";
         
         var gret = Helpers.General.DecryptString(key, "1rzpNMn/ckzX5xJYtlhVNg==");


            returnUrl = returnUrl ?? Url.Content("~/");
            string ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email); 
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                ///var checks = await _unitOfWork.User.ApiLogin("07034428605","P@33w0rd");

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    // var activecheck = await _userManager.FindByEmailAsync

                    //Audit Trail 
                    var audit_ = new EgsAuditTrail
                    {
                        DbAction = "Access",
                        DateCreated = DateTime.Now,
                        Page = "Login",
                        Description = user2.FirstName + " " + user2.LastName + " Login into the application at " + DateTime.Now,
                        IPAddress = Helpers.General.GetIPAddress(),
                        CreatedBy = user2.UserID,
                        Menu = "Login",
                        Role = user2.Role.RoleName
                    };

                    await _unitOfWork.AuditTrail.AddAsync(audit_);
                    _unitOfWork.Complete();

                    if (user2.IsActive == false ) //|| user2.IsVerified == false)
                    {
                        ModelState.AddModelError(string.Empty, "User Deactivated.");
                        //Audit Trail 
                        var audit = new EgsAuditTrail
                        {
                            DbAction = "Access",
                            DateCreated = DateTime.Now,
                            Page = "Login",
                            Description = user2.FirstName + " " + user2.LastName + " Attempt to access the application failed, because User Deactivated, access period " + DateTime.Now,
                            IPAddress = Helpers.General.GetIPAddress(),
                            CreatedBy = user2.UserID,
                            Menu = "Login",
                            Role = user2.Role.RoleName
                        };

                        await _unitOfWork.AuditTrail.AddAsync(audit);
                        _unitOfWork.Complete();

                        return Page();
                    }


                    _logger.LogInformation("User logged in.");

                    if ((string)TempData["logintype"] != "0") // Product is on egole
                    {
                        //string paramsVal = TempData["ProdctID"].ToString();
                        //TempData["paramsVal"] = paramsVal;
                        //return RedirectToAction("ProductWallet", "Homestore", new { id = paramsVal });

                        if (TempData["logintype"].ToString() == "3")
                        {
                            string paramsVal = TempData["ProdctID"].ToString();
                            TempData["paramsVal"] = paramsVal;
                            TempData["WalletTrans"] = "True";
                            //return RedirectToAction("ProductWallet", "Homestore", new { id = TempData["logintype"].ToString() });
                            return RedirectToAction("ProductWallet", "Homestore", new { id = TempData["logintype"].ToString(), vals = paramsVal.ToString() });
                        }
                        if (TempData["logintype"].ToString() != "4")
                        {
                            TempData["WalletTrans"] = "True";
                            return RedirectToAction("ProductWallet", "Homestore", new { id = TempData["logintype"].ToString() });

                        }
                        else
                        {
                            string paramsVal = TempData["ProdctID"].ToString();
                            TempData["paramsVal"] = paramsVal;
                            TempData["WalletTrans"] = "True";
                            return RedirectToAction("ProductWallet", "Homestore", new { id = TempData["logintype"].ToString(), vals = paramsVal.ToString() });
                        }
                    }

                    returnUrl = "/Dashboard/Home/Welcome";
                    //returnUrl = returnUrl == "/" ? "/SysSetup/SysTitle/Title" : returnUrl == null ? "/SysSetup/SysTitle/Title" : returnUrl;

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    //Audit Trail 
                    var audit = new EgsAuditTrail
                    {
                        DbAction = "Access (User account locked out)",
                        DateCreated = DateTime.Now,
                        Page = "Login",
                        Description = user2.FirstName + " " + user2.LastName + " Account lockout at " + DateTime.Now,
                        IPAddress = Helpers.General.GetIPAddress(),
                        CreatedBy = user2.UserID,
                        Menu = "Login",
                        Role = user2.Role.RoleName
                    };
                    await _unitOfWork.AuditTrail.AddAsync(audit);
                    _unitOfWork.Complete();
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    //Audit Trail 

                    var audit = new EgsAuditTrail
                    {
                        DbAction = "Access",
                        DateCreated = DateTime.Now,
                        Page = "Login",
                        Description = user2.FirstName + " " + user2.LastName + " Invalid Login attempt " + DateTime.Now,
                        IPAddress = Helpers.General.GetIPAddress(),
                        CreatedBy = user2.UserID,
                        Menu = "Login",
                        Role = user2.Role.RoleName
                    };
                    await _unitOfWork.AuditTrail.AddAsync(audit);
                    _unitOfWork.Complete();

                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


    }
}
