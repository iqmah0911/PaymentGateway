using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
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
    public class SysUserController : Controller
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

        public SysUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: SysUserController
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return Redirect("/Identity/Account/Manage/ChangePassword");
        }
        


        #region "Users"
        //Default View Page for System User
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SystemUser(string message = null)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString()); 
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            ViewBag.ListofState = StateList();
            ViewBag.ListofRole = await RoleList();
            ViewBag.Message = message;

            return View();
        }

        //Create a new System User
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SystemUser(CreateUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Please fill in the required fields.");
                }

                var systemUserViewModel = new CreateUserViewModel();

                var State = _unitOfWork.State.Get(model.StateID);
                var role = _unitOfWork.Role.Get(model.RoleID);
          
                var userType = _unitOfWork.UserType.Get(1);

                var usernameExists = await _unitOfWork.User.GetUserByEmail(model.Email.Trim());

                if (usernameExists == null)
                {
                    //Handling Users Created by Admin under Company ID -12

                    var sysUser = new SysUsers
                    {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            MiddleName = model.MiddleName,
                            Gender = model.Gender,
                            PhoneNumber = model.PhoneNumber,
                            Email = model.Email,
                            CompanyName = model.CompanyName,
                            Address = model.Address, 
                            ResidentialState = State,
                            IsActive =  true, //false,
                            IsVerified = false,
                            DateCreated = DateTime.Now,
                            Role = role,
                            UserType = userType
                    };

                        await _unitOfWork.CreateUser.AddAsync(sysUser);
                        _unitOfWork.Complete();
                        //Search Role

                        var systemUser = new ApplicationUser { UserName = sysUser.Email, Email = sysUser.Email, UserID = sysUser.UserID };
                        var password = "A" + Guid.NewGuid().ToString("N") + "@01";
                        var result = await _userManager.CreateAsync(systemUser, password);

                   if (result.Succeeded)
                   {
                            _logger.LogInformation("User created a new account with password.");
                        // code for adding user to role
                        //var userAddToRoleResult = await _userManager.AddToRoleAsync(systemUser, role.Name);
                        var userAddToRoleResult = await _userManager.AddToRoleAsync(systemUser, "General");

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

                            ViewBag.Message = "System User has been created successfully";
                              
                        }
                    }
                    if (role.RoleName == "Aggregator" || role.RoleName == "Merchant")
                    {
                    }
                    else { 
                   //Creating a Wallet for the new User
                    var wallet = new EgsWallet
                    {
                        WalletAccountNumber = model.PhoneNumber,
                        DateCreated = DateTime.Now,
                        IsActive = true,
                        User = sysUser
                    };
                    await _unitOfWork.Wallet.AddSaveAsync(wallet);
                    
                    sysUser.Wallet = wallet;

                    }
                    _unitOfWork.CreateUser.UpdateUser(sysUser);

                    _unitOfWork.Complete();

                   ModelState.Clear();
                    
                }
                else
                {
                    ViewBag.ErrorMessage = "Username or Email already exist";
                }


                ViewBag.ListofState = StateList();
                ViewBag.ListofRole = await RoleList();
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
                ViewBag.TransactionLimit = await TransactionLimitList();
                ViewBag.AccountType = await AccountTypeList();
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
        public async Task<IActionResult> UpdateSystemUser (UpdateUserViewModel model)
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
                IsVerified =model.IsVerified,
                Email = model.Email,
                DateOfBirth = model.DOB,
                Image = model.Image
            });

            ModelState.Clear();
            ViewBag.Message = "User has been updated successfully";
            ViewBag.State = StateList();
            ViewBag.TransactionLimit = await TransactionLimitList();
            ViewBag.AccountType = await AccountTypeList();
            return View();
        }

        //Display all System Users
        [HttpGet]
        [Authorize(Roles = "Super Admin, Operation Officer")]
        public async Task<IActionResult> SystemUsers(int? pageIndex, string searchText)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;

                List<SystemUserViewModel> model = new List<SystemUserViewModel>();
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
                    model.Add(new SystemUserViewModel
                    {
                        UserID = activesystemUser.UserID,
                        FirstName = activesystemUser?.FirstName + " " +activesystemUser?.LastName ?? null,
                        LastName = activesystemUser?.LastName ?? null,
                        MiddleName = activesystemUser?.MiddleName ?? null,
                        StateName = activesystemUser?.ResidentialState?.StateName,
                        Gender = activesystemUser?.Gender ?? null,
                        PhoneNumber = activesystemUser?.PhoneNumber ?? null,
                        Email = activesystemUser?.Email ?? null,
                        Address = activesystemUser?.Address ?? null,
                        IsActive = activesystemUser.IsActive
                        
                        // = systemUser.LoginActive,
                        //IsActive = (bool)activesystemUser.IsActive
                    });
                }

                var pager = new Pager(activeSystemUsers.Count(), pageIndex);
                var modelR = new HoldSystemUserViewModel
                {
                    HoldAllSystemUsers = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
        public List<SysState> StateList()
        {
            List<SysState> statesList = new List<SysState>();

            statesList = _unitOfWork.State.GetAllForDropdown();

            statesList.Insert(0, new SysState { StateID = 0, StateName = "--Select State--" });

            return statesList;
        }

        //public async Task<List<SysRole>> RoleList()
        //{
        //    List<SysRole> rolesList = new List<SysRole>();

        //    rolesList = await _unitOfWork.Role.GetAllRoles();

        //    rolesList.Insert(0, new SysRole { RoleID = 0, RoleName = "--Select Role--" });

        //    return rolesList;
        //}

        public async Task<List<SysIdentificationType>> IdentificationTypeList()
        {
            List<SysIdentificationType> identificationTypeList = new List<SysIdentificationType>();

            identificationTypeList = await _unitOfWork.IdentificationType.GetAllIdentificationType();

            identificationTypeList.Insert(0, new SysIdentificationType { IdentificationTypeID = 0, IdentificationTypeName = "--Select Identification Type--" });

            return identificationTypeList;
        }

        public async Task<List<SysBank>> BankList()
        {
            List<SysBank> bankList = new List<SysBank>();

            bankList = await _unitOfWork.Bank.GetAllBanks();

            bankList.Insert(0, new SysBank { BankID = 0, BankName = "--Select Bank--" });

            return bankList;
        }

        public async Task<List<SysRole>> RoleList()
        {
            List<SysRole> roleList = new List<SysRole>();

            roleList =await _unitOfWork.Role.GetAllRoles();

            roleList.Insert(0, new SysRole { RoleID = 0, RoleName = "--Select Role--" });

            return roleList;
        }

        public async Task<List<EgsAccountType>> AccountTypeList()
        {
            List<EgsAccountType> acctTypList = new List<EgsAccountType>();

            acctTypList = await _unitOfWork.AccountType.GetAllAccountTypes();

            acctTypList.Insert(0, new EgsAccountType { AccountTypeID = 0, AccountTypeName = "--Select Account Type--" });

            return acctTypList;
        }

        //public async Task<List<SysAggregator>> AggregatorList()
        //{
        //    List<SysAggregator> aggList = new List<SysAggregator>();

        //    aggList = await _unitOfWork.Aggregator.GetAllAggregators();

        //    aggList.Insert(0, new SysAggregator { AggregatorID = 0, AggregatorName = "--Select Aggregator--" });

        //    return aggList;
        //}

        public async Task<List<SysTransactionLimit>> TransactionLimitList()
        {
            List<SysTransactionLimit> transLimitList = new List<SysTransactionLimit>();

            transLimitList = await _unitOfWork.TransactionLimit.GetAllTransactionLimit();

            transLimitList.Insert(0, new SysTransactionLimit { TransactionLimitID = 0, TransactionLimitName = "--Select Transaction Limit--" });

            return transLimitList;
        }

        //public async Task<List<EgsMerchant>> MerchantList()
        //{
        //    List<EgsMerchant> merchantList = new List<EgsMerchant>();

        //    merchantList = await _unitOfWork.Merchant.GetAllForDropdown();

        //    merchantList.Insert(0, new EgsMerchant { MerchantID = 0, MerchantCode = "--Select Merchant--" });

        //    return merchantList;
        //}



        //public List<SysStates> StateList()
        //{
        //    List<SysStates> statesList = new List<SysStates>();

        //    statesList = _unitOfWork.States.GetAllForDropdown();

        //    statesList.Insert(0, new SysStates { StateId = 0, StateName = "--Select State--" });

        //    return statesList;
        //}
    }
}
