using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Helpers.Interface;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    [Authorize]
    public class SysMerchantController : Controller
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
        private readonly IMimeSender _mimeSender;
        #endregion

        public SysMerchantController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, IMimeSender mimeSender, /*EmailSender emailSender,*/
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mimeSender = mimeSender;
            //_mailSender = emailSender;
            _roleManager = roleManager;
        }


        // GET: SysUserController
        public ActionResult Index()
        {
            return View();
        }

        //public IActionResult ChangePassword()
        //{
        //    return Redirect("/Identity/Account/Manage/ChangePassword");
        //}


        #region "Users"
        //Default View Page for System User
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SystemMerchant(string message = null)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString()); 
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            ViewBag.ListofState = StateList();
            ViewBag.Message = message;
             
            return View();
        }

        //Create a new System User
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SystemMerchant(CreateUserViewModel model)
        {
            try
            {
                if (model.CompanyName== null && model.MerchantCode== null && model.Address==null)//!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Please fill in the required fields.");
                    ViewBag.ListofState = StateList();
                    return View();
                }

                var systemUserViewModel = new CreateUserViewModel();

                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

                var MerchantType = _unitOfWork.MerchantType.Get(1);
                //Creating into the merchant table
                var Merchant = new EgsMerchant
                {
                    MerchantType = MerchantType.MerchantTypeID,
                    AccountNo = model.AccountNo,
                    Address = model.Address,
                    KYC = model.KYC,
                    PhoneNumber = model.PhoneNumber,
                    DateCreated = DateTime.Now,
                    CreatedBy = user2.UserID,
                    MerchantCode = model.MerchantCode

                };
                await _unitOfWork.Merchant.AddSaveAsync(Merchant);

                //Create Wallet for Merchant
                //var VFDServiceConfig = Startup.StaticConfig.GetSection("VFDService").Get<VFDService>();
                ////Call VFD endpoint to create wallet for the new user at VFD endpoint 
                //var dob = Convert.ToDateTime(model.DOB).ToString("dd-MMM-yyyy");
                //var clientUrl = $"{VFDServiceConfig?.Url}?bvn={model.BVN}&wallet-credentials={VFDServiceConfig?.walletCredentials}&dateOfBirth={dob}";
                //var client = new RestClient(clientUrl);
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("Authorization", "Bearer " + VFDServiceConfig.token);
                //request.AddHeader("Accept", "application/json");
                //var response = client.Execute(request);

                ////Getting the response from VFD endpoint
                //var content = response.Content;

                //VVFDResponseViewModel getVVFDResponse = JsonConvert.DeserializeObject<VVFDResponseViewModel>(content);

                //var getStatus = getVVFDResponse.status;
                 
                //-----------------


                var State = _unitOfWork.State.Get(model.StateID);

                //Merchant role
                var role = await _roleManager.Roles.Where(r => r.Id == "e1122e80-8793-4a27-93ba-2696b811290e").SingleOrDefaultAsync();

                var roleDet = _unitOfWork.Role.Get((int)role.RoleID);

                var userType = _unitOfWork.UserType.Get(1);

                //var role = cbsRole == null ? null : await _roleManager.Roles.Where(r => r.Name.ToUpper() == cbsRole.RoleName.ToUpper()).SingleOrDefaultAsync();

                var usernameExists = await _unitOfWork.User.GetUserByEmail(model.Email.Trim());

                //var emailExists = await _unitOfWork.SystemUser.GetUserByEmail(model.Email.Trim());

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
                        //AccountNo = model.AccountNo,
                        //KYC = model.KYC,
                        ResidentialState = State,
                        IsActive = true,
                        IsVerified = false,
                        DateCreated = DateTime.Now,
                        Role = roleDet,
                        UserType = userType,
                        Merchant = Merchant
                    };

                    await _unitOfWork.CreateUser.AddAsync(sysUser);
                    _unitOfWork.Complete();
                    //Search Role

                    ViewBag.ListofState = StateList();

                    var systemUser = new ApplicationUser { UserName = sysUser.Email, Email = sysUser.Email, UserID = sysUser.UserID };
                    var password = "A" + Guid.NewGuid().ToString("N") + "@01";
                    var result = await _userManager.CreateAsync(systemUser, password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");
                        // code for adding user to role
                        //var userAddToRoleResult = await _userManager.AddToRoleAsync(systemUser, role.Name);
                        var userAddToRoleResult = await _userManager.AddToRoleAsync(systemUser, "Merchant");

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
                                                "https://backoffice.biyaego.com" + "<br/>";
                            var Message2 = $"Your password is {password}";
                            
                            //sending the email to the new user
                           // await _mailSender.SendEmailAsync(sysUser.Email, "Confirm your email", Message12);
                            await _mimeSender.Execute("", "Confirm your email", Message12,sysUser.Email);

                            ViewBag.Message = "System User has been created successfully";
                            //await _signInManager.SignInAsync(user, isPersistent: false);
                            //return RedirectToAction(nameof(this.SystemUser),new { message=Message });
                            //return RedirectToAction(nameof(this.SystemUser));
                        }

                        ViewBag.ListofState = StateList();
                    } 
                }
                else
                {
                    ViewBag.ErrorMessage = "SubMerchant or Email already exist";
                }


                ViewBag.ListofState = StateList();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }

            return View();
        }

        //Display all System Users
        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SystemUsers(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<SystemUserViewModel> model = new List<SystemUserViewModel>();
                var activeSystemUsers = await _unitOfWork.User.GetMerchants();

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
                        FirstName = activesystemUser?.FirstName ?? null,
                        LastName = activesystemUser?.LastName ?? null,
                        MiddleName = activesystemUser?.MiddleName ?? null,
                        StateName = activesystemUser?.ResidentialState?.StateName,
                        Gender = activesystemUser?.Gender ?? null,
                        PhoneNumber = activesystemUser?.PhoneNumber ?? null,
                        Email = activesystemUser?.Email ?? null,
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

       // Display view for editing System User
        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateMerchant(int? id)
        {
            ViewBag.ListofState = StateList();
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            try
            {
                
                var UserMer = await _unitOfWork.User.GetMerchantsById((int)id);

                var model = new UpdateUserViewModel
                {
                    UserID= UserMer.UserID,
                    FirstName = UserMer.FirstName,
                    MiddleName = UserMer.MiddleName,
                    LastName = UserMer.LastName,
                    Email =UserMer.Email,
                    Address = UserMer.Address,
                    PhoneNumber =UserMer.PhoneNumber,
                    StateID = UserMer.ResidentialState.StateID,
                    CompanyName= UserMer.CompanyName,
                    IsActive = UserMer.IsActive,
                    MerchantCode = UserMer.Merchant.MerchantCode,
                    MerchantID= UserMer.Merchant.MerchantID
                               
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateMerchant(UpdateUserViewModel model)
        {

            ViewBag.ListofState = StateList();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }
            var State = _unitOfWork.State.Get(model.StateID);

             await _unitOfWork.User.UpdateUser(new SysUsers
            {
                 UserID =model.UserID,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Address = model.Address,
                ResidentialState = State,
                PhoneNumber = model.PhoneNumber,
                CompanyName = model.CompanyName,
                IsActive = model.IsActive,
                Email = model.Email,
           
            });

            await _unitOfWork.Merchant.UpdateSaveAsync(new EgsMerchant
            {
                MerchantID = model.MerchantID,
                AccountNo = model.AccountNo,
                MerchantCode = model.MerchantCode,
                PhoneNumber = model.PhoneNumber,
                Address= model.Address
               
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Merchant has been updated successfully"; 
            return View();
        }

        //Update System User
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin, Operation Officer")]
        //public async Task<IActionResult> UpdateSystemUser(SystemUserViewModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            ModelState.AddModelError("", "Please fill in the required fields.");
        //            return View();
        //        }

        //        var user = await _userManager.GetUserAsync(User);
        //        var user2 = await _unitOfWork.Users.GetSysUsers(user.UserID.ToString());

        //        var company = _unitOfWork.Company.Get(model.CompanyID);

        //        var newRole = await _roleManager.Roles.Where(r => r.Id == model.Role).SingleOrDefaultAsync();
        //        var oldRole = await _roleManager.Roles.Where(role => role.Id == model.OldRole).SingleOrDefaultAsync();

        //        var sysUserToUpdate = await _unitOfWork.Users.GetSysUsers(model.UserID.ToString());
        //        var emailExist = await _unitOfWork.Users.UserExists(model.Email);

        //        if (sysUserToUpdate != null)
        //        {
        //            var userToUpdate = await _userManager.Users
        //                                .Where(u => u.UserID == sysUserToUpdate.UserId)
        //                                .SingleOrDefaultAsync();
        //            if (company.CompanyId == 12 && user2.Role.RoleId == 49)
        //            {
        //                if (userToUpdate != null)
        //                {
        //                    sysUserToUpdate.Role = _unitOfWork.SysRoles.Get(Convert.ToInt32(newRole.RoleID));
        //                    sysUserToUpdate.UserId = model.UserID;
        //                    sysUserToUpdate.Username = model.UserName;
        //                    sysUserToUpdate.Firstname = model.FirstName;
        //                    sysUserToUpdate.Middlename = model.MiddleName;
        //                    sysUserToUpdate.Lastname = model.LastName;
        //                    sysUserToUpdate.Gender = model.Gender;
        //                    sysUserToUpdate.Email = model.Email;
        //                    //sysUserToUpdate.IsActive = model.IsActive;
        //                    sysUserToUpdate.Mobile = model.Mobile;
        //                    sysUserToUpdate.Company = company;

        //                    userToUpdate.Email = model.Email;
        //                    userToUpdate.NormalizedEmail = model.Email.ToUpper();

        //                    var resultUserUpdate = await _userManager.UpdateAsync(user);

        //                    if (resultUserUpdate.Succeeded)
        //                    {
        //                        await _unitOfWork.SystemUser.UpdateSaveAsync(sysUserToUpdate);

        //                        var removeOldRoleresult = await _userManager.RemoveFromRoleAsync(userToUpdate, oldRole.Name);

        //                        if (removeOldRoleresult.Succeeded)
        //                        {
        //                            var result = await _userManager.AddToRoleAsync(userToUpdate, newRole.Name);

        //                            if (result.Succeeded)
        //                            {
        //                                // _unitOfWork.Complete();
        //                                ModelState.Clear();

        //                                //var user = await _userManager.GetUserAsync(User);
        //                                //var user2 = await _unitOfWork.Users.GetSysUsers(user.UserID.ToString());

        //                                var Message12 = "Dear " + sysUserToUpdate.Firstname + " " + sysUserToUpdate.Lastname + ",<br/><br/>" +
        //                                          "Your details on the AutoReg platform has been updated " + ".<br/><br/>" +
        //                                          "Please login to confirm your email<br/>" +
        //                                          "AutoReg Team.<br/>" +
        //                                          "https://vreg.autoreglive.com | http://autoreg.com" + "<br/>";
        //                                // _logger.LogInformation(Message2);
        //                                await _mailSender.SendEmailAsync(sysUserToUpdate.Email, "AutoReg Update", Message12);

        //                                if (User.IsInRole("Operation Officer"))
        //                                {
        //                                    ViewBag.ListofCompany = CompanyListCustom();
        //                                }
        //                                else
        //                                {
        //                                    ViewBag.ListofCompany = CompanyList();
        //                                }

        //                                ViewBag.ListofState = StateList();
        //                                ViewBag.ListofBranch = BranchList();
        //                                ViewBag.Message = "";
        //                                if (user2.Company.CompanyId == 12 && User.IsInRole("Operation Officer"))
        //                                {
        //                                    //if (User.IsInRole("Operation Officer"))
        //                                    //{
        //                                    ViewBag.Roles = _roleManager.Roles.Where(a => a.RoleID == 3053
        //                                || a.RoleID == 48 || a.RoleID == 3054 || a.RoleID == 47 || a.RoleID == 3 || a.RoleID == 5 || a.RoleID == 4
        //                                || a.RoleID == 2046 || a.RoleID == 2049 || a.RoleID == 3046 || a.RoleID == 50 || a.RoleID == 45).ToList();
        //                                    //}
        //                                }
        //                                else
        //                                {
        //                                    ViewBag.Roles = _roleManager.Roles.ToList();
        //                                }

        //                                ViewBag.Message = "System User has been updated successfully";

        //                                return View();
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    sysUserToUpdate.Role = _unitOfWork.SysRoles.Get(Convert.ToInt32(newRole.RoleID));
        //                    sysUserToUpdate.UserId = model.UserID;
        //                    sysUserToUpdate.Username = model.UserName;
        //                    sysUserToUpdate.Firstname = model.FirstName;
        //                    sysUserToUpdate.Middlename = model.MiddleName;
        //                    sysUserToUpdate.Lastname = model.LastName;
        //                    sysUserToUpdate.Gender = model.Gender;
        //                    sysUserToUpdate.Email = model.Email;
        //                    //sysUserToUpdate.IsActive = model.IsActive;
        //                    sysUserToUpdate.Mobile = model.Mobile;
        //                    sysUserToUpdate.Company = company;

        //                    await _unitOfWork.SystemUser.UpdateSaveAsync(sysUserToUpdate);
        //                    // _unitOfWork.Complete();
        //                    ModelState.Clear();

        //                    //var user = await _userManager.GetUserAsync(User);
        //                    //var user2 = await _unitOfWork.Users.GetSysUsers(user.UserID.ToString());

        //                    var Message12 = "Dear " + sysUserToUpdate.Firstname + " " + sysUserToUpdate.Lastname + ",<br/><br/>" +
        //                                "Your details on the AutoReg platform has been updated " + ".<br/><br/>" +
        //                                "Please login to confirm your email<br/>" +
        //                                "AutoReg Team.<br/>" +
        //                                "https://vreg.autoreglive.com | http://autoreg.com" + "<br/>";
        //                    // _logger.LogInformation(Message2);
        //                    await _mailSender.SendEmailAsync(sysUserToUpdate.Email, "AutoReg Update", Message12);

        //                    if (User.IsInRole("Operation Officer"))
        //                    {
        //                        ViewBag.ListofCompany = CompanyListCustom();
        //                    }
        //                    else
        //                    {
        //                        ViewBag.ListofCompany = CompanyList();
        //                    }

        //                    ViewBag.ListofState = StateList();
        //                    ViewBag.ListofBranch = BranchList();
        //                    ViewBag.Message = "";
        //                    if (user2.Company.CompanyId == 12 && User.IsInRole("Operation Officer"))
        //                    {
        //                        //if (User.IsInRole("Operation Officer"))
        //                        //{
        //                        ViewBag.Roles = _roleManager.Roles.Where(a => a.RoleID == 3053
        //                    || a.RoleID == 48 || a.RoleID == 3054 || a.RoleID == 47 || a.RoleID == 3 || a.RoleID == 5 || a.RoleID == 4
        //                    || a.RoleID == 2046 || a.RoleID == 2049 || a.RoleID == 3046 || a.RoleID == 50 || a.RoleID == 45).ToList();
        //                        //}
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Roles = _roleManager.Roles.ToList();
        //                    }

        //                    ViewBag.Message = "System User has been updated successfully";

        //                    return View();
        //                }
        //            }
        //            else
        //            {
        //                if (userToUpdate != null)
        //                {
        //                    sysUserToUpdate.Role = _unitOfWork.SysRoles.Get(Convert.ToInt32(newRole.RoleID));
        //                    sysUserToUpdate.UserId = model.UserID;
        //                    sysUserToUpdate.Username = model.UserName;
        //                    sysUserToUpdate.Firstname = model.FirstName;
        //                    sysUserToUpdate.Middlename = model.MiddleName;
        //                    sysUserToUpdate.Lastname = model.LastName;
        //                    sysUserToUpdate.Email = model.Email;
        //                    sysUserToUpdate.Gender = model.Gender;
        //                    sysUserToUpdate.IsActive = model.IsActive;
        //                    sysUserToUpdate.Mobile = model.Mobile;
        //                    sysUserToUpdate.Company = company;
        //                    user.Email = model.Email;
        //                    user.NormalizedEmail = model.Email.ToUpper();

        //                    var resultUserUpdate = await _userManager.UpdateAsync(user);

        //                    if (resultUserUpdate.Succeeded)
        //                    {
        //                        await _unitOfWork.SystemUser.UpdateSaveAsync(sysUserToUpdate);

        //                        var removeOldRoleresult = await _userManager.RemoveFromRoleAsync(userToUpdate, oldRole.Name);

        //                        if (removeOldRoleresult.Succeeded)
        //                        {
        //                            var result = await _userManager.AddToRoleAsync(userToUpdate, newRole.Name);

        //                            if (result.Succeeded)
        //                            {
        //                                // _unitOfWork.Complete();
        //                                ModelState.Clear();

        //                                //var user = await _userManager.GetUserAsync(User);
        //                                //var user2 = await _unitOfWork.Users.GetSysUsers(user.UserID.ToString());

        //                                var Message12 = "Dear " + sysUserToUpdate.Firstname + " " + sysUserToUpdate.Lastname + ",<br/><br/>" +
        //                                          "Your details on the AutoReg platform has been updated " + ".<br/><br/>" +
        //                                          "Please login to confirm your email<br/>" +
        //                                          "AutoReg Team.<br/>" +
        //                                          "https://vreg.autoreglive.com | http://autoreg.com" + "<br/>";
        //                                // _logger.LogInformation(Message2);
        //                                await _mailSender.SendEmailAsync(sysUserToUpdate.Email, "AutoReg Update", Message12);

        //                                if (User.IsInRole("Operation Officer"))
        //                                {
        //                                    ViewBag.ListofCompany = CompanyListCustom();
        //                                }
        //                                else
        //                                {
        //                                    ViewBag.ListofCompany = CompanyList();
        //                                }

        //                                ViewBag.ListofState = StateList();
        //                                ViewBag.ListofBranch = BranchList();
        //                                ViewBag.Message = "";
        //                                if (user2.Company.CompanyId == 12 && User.IsInRole("Operation Officer"))
        //                                {
        //                                    //if (User.IsInRole("Operation Officer"))
        //                                    //{
        //                                    ViewBag.Roles = _roleManager.Roles.Where(a => a.RoleID == 3053
        //                                || a.RoleID == 48 || a.RoleID == 3054 || a.RoleID == 47 || a.RoleID == 3 || a.RoleID == 5 || a.RoleID == 4
        //                                || a.RoleID == 2046 || a.RoleID == 2049 || a.RoleID == 3046 || a.RoleID == 50 || a.RoleID == 45).ToList();
        //                                    //}
        //                                }
        //                                else
        //                                {
        //                                    ViewBag.Roles = _roleManager.Roles.ToList();
        //                                }

        //                                ViewBag.Message = "System User has been updated successfully";

        //                                return View();
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    sysUserToUpdate.Role = _unitOfWork.SysRoles.Get(Convert.ToInt32(newRole.RoleID));
        //                    sysUserToUpdate.UserId = model.UserID;
        //                    sysUserToUpdate.Username = model.UserName;
        //                    sysUserToUpdate.Firstname = model.FirstName;
        //                    sysUserToUpdate.Middlename = model.MiddleName;
        //                    sysUserToUpdate.Lastname = model.LastName;
        //                    sysUserToUpdate.Email = model.Email;
        //                    sysUserToUpdate.Gender = model.Gender;
        //                    sysUserToUpdate.IsActive = model.IsActive;
        //                    sysUserToUpdate.Mobile = model.Mobile;
        //                    sysUserToUpdate.Company = company;

        //                    await _unitOfWork.SystemUser.UpdateSaveAsync(sysUserToUpdate);

        //                    // _unitOfWork.Complete();
        //                    ModelState.Clear();

        //                    //var user = await _userManager.GetUserAsync(User);
        //                    //var user2 = await _unitOfWork.Users.GetSysUsers(user.UserID.ToString());

        //                    var Message12 = "Dear " + sysUserToUpdate.Firstname + " " + sysUserToUpdate.Lastname + ",<br/><br/>" +
        //                                "Your details on the AutoReg platform has been updated " + ".<br/><br/>" +
        //                                "Please login to confirm your email<br/>" +
        //                                "AutoReg Team.<br/>" +
        //                                "https://vreg.autoreglive.com | http://autoreg.com" + "<br/>";
        //                    // _logger.LogInformation(Message2);
        //                    await _mailSender.SendEmailAsync(sysUserToUpdate.Email, "AutoReg Update", Message12);

        //                    if (User.IsInRole("Operation Officer"))
        //                    {
        //                        ViewBag.ListofCompany = CompanyListCustom();
        //                    }
        //                    else
        //                    {
        //                        ViewBag.ListofCompany = CompanyList();
        //                    }

        //                    ViewBag.ListofState = StateList();
        //                    ViewBag.ListofBranch = BranchList();
        //                    ViewBag.Message = "";
        //                    if (user2.Company.CompanyId == 12 && User.IsInRole("Operation Officer"))
        //                    {
        //                        //if (User.IsInRole("Operation Officer"))
        //                        //{
        //                        ViewBag.Roles = _roleManager.Roles.Where(a => a.RoleID == 3053
        //                    || a.RoleID == 48 || a.RoleID == 3054 || a.RoleID == 47 || a.RoleID == 3 || a.RoleID == 5 || a.RoleID == 4
        //                    || a.RoleID == 2046 || a.RoleID == 2049 || a.RoleID == 3046 || a.RoleID == 50 || a.RoleID == 45).ToList();
        //                        //}
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Roles = _roleManager.Roles.ToList();
        //                    }

        //                    ViewBag.Message = "System User has been updated successfully";

        //                    return View();
        //                }
        //            }
        //        }

        //        ModelState.Clear();

        //        ViewBag.ListofCompany = CompanyList();
        //        ViewBag.ListofState = StateList();
        //        ViewBag.ListofBranch = BranchList();
        //        ViewBag.Roles = _roleManager.Roles.ToList();

        //        ViewBag.Message = "";
        //        ViewBag.ErrorMessage = "System User update unsuccessfully";

        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.Message + ex.StackTrace);
        //    }

        //    return View();
        //}



        #endregion

        //Method to pull States 
        public List<SysState> StateList()
        {
            List<SysState> statesList = new List<SysState>();

            statesList = _unitOfWork.State.GetAllForDropdown();

            statesList.Insert(0, new SysState { StateID = 0, StateName = "--Select State--" });

            return statesList;
        }

        //public List<SysStates> StateList()
        //{
        //    List<SysStates> statesList = new List<SysStates>();

        //    statesList = _unitOfWork.States.GetAllForDropdown();

        //    statesList.Insert(0, new SysStates { StateId = 0, StateName = "--Select State--" });

        //    return statesList;
        //}
    }
}
