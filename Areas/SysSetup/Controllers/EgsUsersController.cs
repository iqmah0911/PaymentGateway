using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Helpers.Interface;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    [Authorize]
    public class EgsUsersController : Controller
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
        private readonly IMimeSender _mimeSender;
        #region logger
        private readonly IEmailSender _mailSender;
        #endregion

        public EgsUsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, IMimeSender mimeSender,/*EmailSender emailSender,*/
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            //_mailSender = emailSender;
            _mimeSender = mimeSender;
            _roleManager = roleManager;
        }


        // GET: EgsUsersController
        public ActionResult Index()
        {
            return View();
        }


        #region "Users"
        //Default View Page for System User
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SystemUser(string message = null)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            ViewBag.ListofState = StateList();
            ViewBag.Message = message;

            return View();
        }

        //Create a new System User
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SystemUser(CreateUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ListofState = await StateList();
                    ViewBag.ErrorMessage = "Please fill in the required fields";
                    return View();
                }
                if (model.StateID == 0)
                {
                    ViewBag.ListofState = await StateList();
                    ViewBag.ErrorMessage = "Please fill in the required fields";
                    return View();
                }

                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

                //Get bank Wallet Details
                var getwalletdetails = await _unitOfWork.Wallet.GetWalletByUserID(user2.Wallet.WalletID);
                var ReferralCode = getwalletdetails.WalletAccountNumber;

                //Create sub agent tied to bank
                var BUserViewModel = new CreateUserViewModel();
                var Bstate = _unitOfWork.State.Get(model.StateID);
                var Brole = await _roleManager.Roles.Where(r => r.Name == "SubAgent").SingleOrDefaultAsync();
                var BroleDet = _unitOfWork.Role.Get((int)Brole.RoleID);
                var BuserType = _unitOfWork.UserType.Get(1);
                var subagentsExists = await _unitOfWork.User.GetUserByEmailAndPhoneNumber(model.Email.Trim(), model.PhoneNumber);

                if (subagentsExists == null)
                {
                    //Create user details on EgolePay platform
                    var sysUser = new SysUsers
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        MiddleName = model.MiddleName,
                        Gender = model.Gender,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        ResidentialState = Bstate,
                        IsActive = true,
                        IsVerified = false,
                        DateCreated = DateTime.Now,
                        //Wallet =getwalletdetails.User.Wallet,
                        BankAccountCode = ReferralCode,
                        Role = BroleDet,
                        UserType = BuserType,
                        BVN = model.BVN,
                        DateOfBirth = model.DOB.ToString()
                    };

                    await _unitOfWork.CreateUser.AddAsync(sysUser);
                    _unitOfWork.Complete();
                    var systemUser = new ApplicationUser { UserName = sysUser.Email, Email = sysUser.Email, UserID = sysUser.UserID };
                    var password = Guid.NewGuid().ToString("N").Substring(0, 8) + "ep@0X1";
                    var Bresult = await _userManager.CreateAsync(systemUser, password);

                    //Checking if Identity details have been created
                    if (Bresult.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");
                        // code for adding user to role
                        var userAddToRoleResult = await _userManager.AddToRoleAsync(systemUser, "SubAgent");

                        if (userAddToRoleResult.Succeeded)
                        {
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(systemUser);
                            var Message = $"Please login to your  account at <a href='#'>clicking here</a>. Your password is {password}";
                            var Message12 = "Dear " + sysUser.FirstName + " " + sysUser.LastName + ",<br/><br/>" +
                                                "Congratulations!" + "<br/><br/>" +
                                                "You have just been created on our platform as user with the username " + sysUser.Email + ".<br/><br/>" +
                                                "Find below your <b>Username</b> and <b>Password</b>;" +
                                                "<br/>" +
                                                "- Username: " + sysUser.Email + "<br/>" +
                                                "- Password: " + password + "<br/><br/>" +
                                                "Please login to confirm your email<br/>" +
                                                "EgolePay Team.<br/>" +
                                                "https://egolepay.com" + "<br/>";
                            var Message2 = $"Your password is {password}";

                            await _mimeSender.Execute("", "Confirm your email", Message12, sysUser.Email);

                            ViewBag.Message = "User has been registered successfully";
                            ModelState.Clear();
                            ViewBag.ListofState = await StateList();
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.ListofState = await StateList();
                        ViewBag.ErrorMessage = "-" + "User Details not created successfully, kindly reattempt!";
                        return View();
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Email or Phone Number already exist on the System";
                }

                ViewBag.ListofState = await StateList();

                return View();

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }

            return View();
        }

        //Update a system user GET method
        [HttpGet]
        public async Task<IActionResult> UpdateSystemUser(int id)
        {
            try
            {
                var userr = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(userr.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;

                ViewBag.State = StateList();
                //var userMer = await _unitOfWork.User.GetMerchantsById((int)id);
                var user = await _unitOfWork.User.GetUserByStateID((int)id);

                var model = new UpdateUserViewModel
                {
                    UserID = user.UserID,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    StateID = user.ResidentialState.StateID,
                    CompanyName = user.CompanyName,
                    IsActive = user.IsActive,
                    IsVerified = user.IsVerified,
                    DOB = user.DateOfBirth,
                    Gender = user.Gender,
                    BVN = user.BVN,
                    MerchantID = user.Merchant.MerchantID,
                    Image = user.Image

                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Updating User POST method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSystemUser(UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var acctType = await _unitOfWork.AccountType.GetAccountTypeByID(model.AccountTypeID);
            var State = _unitOfWork.State.Get(model.StateID);
            var transLimit = await _unitOfWork.TransactionLimit.GetTransactionLimit(model.TransactionLimitID);

            await _unitOfWork.User.UpdateUser(new SysUsers
            {
                UserID = model.UserID,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Address = model.Address,
                ResidentialState = State,
                PhoneNumber = model.PhoneNumber,
                CompanyName = model.CompanyName,
                TransactionLimit = transLimit,
                AccountType = acctType,
                IsActive = model.IsActive,
                IsVerified = model.IsVerified,
                Email = model.Email,
                DateOfBirth = model.DOB,
                Image = model.Image
            });

            ModelState.Clear();
            ViewBag.Message = "User has been updated successfully";
            ViewBag.State = StateList();
            return View();
        }

        //Display all System Users
        [HttpGet]
        [Authorize(Roles = "Super Admin, Operation Officer")]
        public async Task<IActionResult> OperationUsers(int? pageIndex, string searchText)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;

                List<SysCustomerViewModel> model = new List<SysCustomerViewModel>();
                var activeSystemUsers = await _unitOfWork.User.GetAllActiveUsers();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    activeSystemUsers = activeSystemUsers
                        .Where(u => (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                                    (!string.IsNullOrEmpty(u.PhoneNumber) && u.PhoneNumber.Contains(searchText, StringComparison.OrdinalIgnoreCase))).ToList();
                }

                foreach (var activesystemUser in activeSystemUsers)
                {
                    model.Add(new SysCustomerViewModel
                    {
                        UserID = activesystemUser.UserID,
                        FirstName = activesystemUser?.FirstName + " " + activesystemUser?.LastName ?? null,
                        LastName = activesystemUser?.LastName ?? null,
                        MiddleName = activesystemUser?.MiddleName ?? null,
                        StateName = activesystemUser?.ResidentialState?.StateName,
                        Gender = activesystemUser?.Gender ?? null,
                        PhoneNumber = activesystemUser?.PhoneNumber ?? null,
                        Email = activesystemUser?.Email ?? null,
                        Address = activesystemUser?.Address ?? null
                        //IsActive = activesystemUser.IsActive

                        // = systemUser.LoginActive,
                        //IsActive = (bool)activesystemUser.IsActive
                    });
                }

                var pager = new Pager(activeSystemUsers.Count(), pageIndex);
                var modelR = new HoldCustomerViewModel
                {
                    HoldAllCustomers = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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

        //Method to pull States 
        public async Task<List<SysState>> StateList()
        {
            List<SysState> statesList = new List<SysState>();

            statesList = await _unitOfWork.State.GetAllStates();       //GetAllForDropdown();

            statesList.Insert(0, new SysState { StateID = 0, StateName = "Select State" });

            return statesList;
        }


    }


}
