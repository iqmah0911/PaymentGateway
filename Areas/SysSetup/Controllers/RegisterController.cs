using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.Dashboard.Models;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Helpers.Interface;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    [Authorize(Roles = "Super Admin, Merchant, Aggregator, Agent")]
    [AllowAnonymous]
    public class RegisterController : Controller
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
        private readonly ISendGridmail _sendgridmail;

        #region logger
        private readonly IEmailSender _mailSender;
        #endregion

        public RegisterController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, IEmailSender emailSender, IMimeSender mimeSender, ISendGridmail sendGridmail,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _mimeSender = mimeSender;
            _sendgridmail = sendGridmail;
        }


        // GET: RegisterController
        public ActionResult Index()
        {
            return View();
        }


        //Default View Page for System User
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> RegisterUser(string message = null)
        {
            //var user = await _userManager.GetUserAsync(User);
            //var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            //ViewBag.pic = profiles.Image;
            var glaccinuse = _unitOfWork.GLAccount.GetGLAccount(1);


            ViewBag.ListofState = await StateList();
            ViewBag.Message = message;

            return View();
        }


        //Create a new System User
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> RegisterUser(CreateUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ListofState = await StateList();
                    ViewBag.ErrorMessage = "Please fill in the required fields";
                    TempData["message"] = "Please fill in the required fields";
                    return View();
                }
                if (model.StateID == 0)
                {
                    ViewBag.ListofState = await StateList();
                    ViewBag.ErrorMessage = "Please fill in the required fields";
                    TempData["message"] = "Please fill in the required fields";

                    return View();
                }
                //Check for BVN
                var BVNExists = await _unitOfWork.User.GetExistingUserBVN(model.BVN);
                if (BVNExists != null)
                {
                    ViewBag.ListofState = await StateList();
                    ViewBag.ErrorMessage = "BVN already exist on the System,Account not created.";
                    TempData["message"] = "BVN already exist on the System,Account not created.";

                    return View();
                }

                if (model.ReferralCode != null)
                {
                    //Get bank Wallet Details
                    var getwalletdetails = await _unitOfWork.Wallet.GetWalletByAccountNumber(model.ReferralCode);
                    if (getwalletdetails == null)
                    {
                        ViewBag.ListofState = await StateList();
                        ViewBag.ErrorMessage = "Referral Code doesn't exist,Account not created.";
                        TempData["message"] = "Referral Code doesn't exist,Account not created."; 


                        return View();
                    }
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
                            BankAccountCode = model.ReferralCode,
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
                                TempData["message"] = "User has been registered successfully";

                                ModelState.Clear();
                                ViewBag.ListofState = await StateList();
                                return View();
                            }
                        }
                        else
                        {
                            ViewBag.ListofState = await StateList();
                            ViewBag.ErrorMessage = "-" + "User Details not created successfully, kindly reattempt!";
                            TempData["message"] = "-" + "User Details not created successfully, kindly reattempt!";
                            return View();
                        }
                    }
                }

                //ViewBag.ListofState = StateList(); 
                var systemUserViewModel = new CreateUserViewModel();

                var State = _unitOfWork.State.Get(model.StateID);

                //var cbsRole = _unitOfWork.Role.Get(Convert.ToInt32(model.RoleID));
                var role = await _roleManager.Roles.Where(r => r.Name == "General").SingleOrDefaultAsync();

                var roleDet = _unitOfWork.Role.Get((int)role.RoleID);

                var userType = _unitOfWork.UserType.Get(1);

                var usernameExists = await _unitOfWork.User.GetUserByEmailAndPhoneNumber(model.Email.Trim(), model.PhoneNumber);

                if (usernameExists == null)
                {
                    var VFDServiceConfig = Startup.StaticConfig.GetSection("VFDService").Get<VFDService>();
                    //Call VFD endpoint to create wallet for the new user at VFD endpoint 
                    var dob = Convert.ToDateTime(model.DOB).ToString("dd-MMM-yyyy");
                    var clientUrl = $"{VFDServiceConfig?.Url}?bvn={model.BVN}&wallet-credentials={VFDServiceConfig?.walletCredentials}&dateOfBirth={dob}";
                    var client = new RestClient(clientUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", "Bearer " + VFDServiceConfig.token);
                    request.AddHeader("Accept", "application/json");
                    var response = client.Execute(request);

                    //Getting the response from VFD endpoint
                    var content = response.Content;

                    if (content == "")
                    {
                        ViewBag.ListofState = await StateList();
                        ViewBag.ErrorMessage = "NIBBS is currently not available ";
                        TempData["message"] = "NIBBS is currently not available ";
                         
                        return View();
                    }

                    VVFDResponseViewModel getVVFDResponse = JsonConvert.DeserializeObject<VVFDResponseViewModel>(content);

                    var getStatus = getVVFDResponse.status;

                    if (getVVFDResponse.status.ToString() != "00")
                    {
                        ViewBag.ListofState = await StateList();
                        ViewBag.ErrorMessage = getVVFDResponse.message.ToString();
                        TempData["message"] = getVVFDResponse.message.ToString();
                         
                        return View();
                    }

                    //Successful creation of VFD Wallet Account - if true, then proceed to create Egole User Details and Wallet details
                    if (getStatus == "00")
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
                            ResidentialState = State,
                            IsActive = true,
                            IsVerified = false,
                            DateCreated = DateTime.Now,
                            Role = roleDet,
                            UserType = userType,
                            BVN = model.BVN,
                            DateOfBirth = model.DOB.ToString()
                        };

                        await _unitOfWork.CreateUser.AddAsync(sysUser);
                        _unitOfWork.Complete();
                        var systemUser = new ApplicationUser { UserName = sysUser.Email, Email = sysUser.Email, UserID = sysUser.UserID };
                        var password = Guid.NewGuid().ToString("N").Substring(0, 8) + "ep@0X1";
                        var result = await _userManager.CreateAsync(systemUser, password);

                        //Checking if Identity details have been created
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User created a new account with password.");
                            // code for adding user to role
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

                                //Creating a Wallet on EgolePay platform for the new User just created
                                var wallet = new EgsWallet
                                {
                                    WalletAccountNumber = getVVFDResponse.data.accountNo,
                                    DateCreated = DateTime.Now,
                                    IsActive = true,
                                    User = sysUser,
                                    BVN = model.BVN
                                };
                                await _unitOfWork.Wallet.AddSaveAsync(wallet);

                                sysUser.Wallet = wallet;

                                _unitOfWork.CreateUser.UpdateUser(sysUser);

                                _unitOfWork.Complete();


                                // await _mimeSender.Execute("", "Confirm your email", Message12, sysUser.Email);
                                await _sendgridmail.Execute(sysUser.Email, "Confirm your email", Message12);



                                ViewBag.Message = "User has been registered successfully";
                                TempData["message"] = "User has been registered successfully";


                                ModelState.Clear();


                            }
                        }

                        //Creating a Wallet on EgolePay platform for the new User just created
                        //var wallet = new EgsWallet
                        //{
                        //    WalletAccountNumber = getVVFDResponse.data.accountNo,
                        //    DateCreated = DateTime.Now,
                        //    IsActive = true,
                        //    User = sysUser,
                        //    BVN = model.BVN
                        //};
                        //await _unitOfWork.Wallet.AddSaveAsync(wallet);

                        //sysUser.Wallet = wallet;

                        //_unitOfWork.CreateUser.UpdateUser(sysUser);

                        //_unitOfWork.Complete();

                        //ModelState.Clear();
                    }
                    //Unsuccessful creation of VFD Wallet Account, cannot proceed to Egole User details and Wallet Details
                    else
                    {
                        ViewBag.ErrorMessage = getVVFDResponse.message + " " + "-" + "User Details not created successfully, kindly reattempt!";
                        TempData["message"] = getVVFDResponse.message + " " + "-" + "User Details not created successfully, kindly reattempt!";

                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Email or Phone Number already exist on the System";
                    TempData["message"] = "Email or Phone Number already exist on the System";

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



        // RegisterCorporate



        public async Task<IActionResult> RegisterCorporate(CreateUserViewModel model)
       {
            try
            {
                //Check for BVN
                var BVNExists = await _unitOfWork.User.GetExistingUserBVN(model.BVN);
                if (BVNExists != null)
                {
                    ViewBag.ListofState = await StateList();
                    ViewBag.ErrorMessage = "BVN already exist on the System,Account not created.";
                    TempData["message"] = "BVN already exist on the System,Account not created.";
                    return RedirectToAction("RegisterUser", "Register");
                }

                if (model.rcNumber != null || model.companyName != null || model.BVN != null || model.incorporationDate != null)
                {
                    var systemUserViewModel = new CreateUserViewModel();

                    var State = _unitOfWork.State.Get(model.StateID);

                    //var cbsRole = _unitOfWork.Role.Get(Convert.ToInt32(model.RoleID));
                    var role = await _roleManager.Roles.Where(r => r.Name == "General").SingleOrDefaultAsync();

                    var roleDet = _unitOfWork.Role.Get((int)role.RoleID);

                    var userType = _unitOfWork.UserType.Get(1);

                    var usernameExists = await _unitOfWork.User.GetUserByEmailAndPhoneNumber(model.Email.Trim(), model.PhoneNumber);

                    if (usernameExists == null)
                    {

                        var VFDServiceConfig = Startup.StaticConfig.GetSection("VFDCoporateService").Get<VFDService>();
                        //Call VFD endpoint to create wallet for the new user at VFD endpoint 
                        var dob = Convert.ToDateTime(model.DOB).ToString("dd-MMM-yyyy");
                        var vfddate = Convert.ToDateTime(model.incorporationDate).ToString("dd MMMM yyyy");
                        var clientUrl = $"{VFDServiceConfig?.Url}corporateclient/create?wallet-credentials={VFDServiceConfig?.walletCredentials}";

                        var phonestring = model.PhoneNumber.Substring(model.PhoneNumber.Length-4);

                        var postdata = new CorporateRequest
                        {
                            rcNumber = model.rcNumber +phonestring,
                            companyName = model.companyName,
                            incorporationDate = vfddate,
                            BVN = model.BVN
                        };

                        var inputs = JsonConvert.SerializeObject(postdata);

                        
                        {
                           
                            var client = new HttpClient();
                            var request = new HttpRequestMessage(HttpMethod.Post, clientUrl);

                            // Add Bearer token to header
                            var token = VFDServiceConfig.token;
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            // Add request body
                            var body = inputs;
                            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                            // Send request
                            var response = client.SendAsync(request).Result;
                            var responseContent = response.Content.ReadAsStringAsync().Result;

                           

                            coporateres getVVFDResponse = JsonConvert.DeserializeObject<coporateres>(responseContent);


                            var getStatus = getVVFDResponse.status;

                            if (getVVFDResponse.status.ToString() != "00")
                            {
                                ViewBag.ListofState = await StateList();
                                ViewBag.CErrorMessage = getVVFDResponse.message.ToString();

                                TempData["message"] = getVVFDResponse.message.ToString();

                                return RedirectToAction("RegisterUser", "Register");
                            }

                            //Successful creation of VFD Wallet Account - if true, then proceed to create Egole User Details and Wallet details
                            if (getStatus == "00")
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
                                    ResidentialState = State,
                                    IsActive = true,
                                    IsVerified = false,
                                    DateCreated = DateTime.Now,
                                    Role = roleDet,
                                    UserType = userType,
                                    BVN = model.BVN,
                                    DateOfBirth = model.DOB.ToString(),
                                    RCNumber=model.rcNumber,
                                    CompanyName=model.CompanyName,
                                    BusinessName="CORPORATE ACCOUNTS"
                                };

                                await _unitOfWork.CreateUser.AddAsync(sysUser);
                                _unitOfWork.Complete();
                                var systemUser = new ApplicationUser { UserName = sysUser.Email, Email = sysUser.Email, UserID = sysUser.UserID };
                                var password = Guid.NewGuid().ToString("N").Substring(0, 8) + "ep@0X1";
                                var result = await _userManager.CreateAsync(systemUser, password);

                                //Checking if Identity details have been created
                                if (result.Succeeded)
                                {
                                    _logger.LogInformation("User created a new account with password.");
                                    // code for adding user to role
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
                                                             "-AccountName -"+getVVFDResponse.data.accountName + "<br/><br/>" +
                                                             "AccountNumber: " + getVVFDResponse.data.accountNo + "<br/><br/>" +
                                                            "Please login to confirm your email<br/>" +
                                                            "EgolePay Team.<br/>" +
                                                            "https://egolepay.com" + "<br/>";
                                        var Message2 = $"Your password is {password}";

                                        //Creating a Wallet on EgolePay platform for the new User just created
                                        var wallet = new EgsWallet
                                        {
                                            WalletAccountNumber = getVVFDResponse.data.accountNo,
                                            DateCreated = DateTime.Now,
                                            IsActive = true,
                                            User = sysUser,
                                            BVN = model.BVN
                                        };
                                        await _unitOfWork.Wallet.AddSaveAsync(wallet);

                                        sysUser.Wallet = wallet;

                                        _unitOfWork.CreateUser.UpdateUser(sysUser);

                                        _unitOfWork.Complete();

                                        await _sendgridmail.Execute(sysUser.Email, "Confirm your email", Message12);

                                        ViewBag.Message = "User has been registered successfully";
                                        TempData["message"] = getVVFDResponse.message.ToString();
                                       /// ModelState.Clear();
                                    }
                                }
                            }
                            else
                            {
                                TempData["message"] = getVVFDResponse.message + " " + "-" + "User Details not created successfully, kindly reattempt!";
                                ViewBag.ErrorMessage = getVVFDResponse.message + " " + "-" + "User Details not created successfully, kindly reattempt!";
                            }

                        }

                        //var client = new RestClient(clientUrl);
                        //var request = new RestRequest(Method.POST);
                        //request.AddHeader("Authorization", "Bearer " + VFDServiceConfig.token);
                        //request.AddHeader("Accept", "application/json");
                        //request.c = new StringContent(inputs, Encoding.UTF8);
                        //var response = client.Execute(request);

                        //Getting the response from VFD endpoint
                        // var content = response.Content;

                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Email or Phone Number already exist on the System";
                    }

                }
                ViewBag.ListofState = await StateList(); 

                return RedirectToAction("RegisterUser", "Register");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }

            return View();
        }














        ////Create a new System User
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////[Authorize(Roles = "Super Admin")]
        //public async Task<IActionResult> RegisterUser(CreateUserViewModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            ViewBag.ListofState = await StateList();
        //            ViewBag.ErrorMessage = "Please fill in the required fields";
        //            return View(); 
        //        }
        //        if (model.StateID== 0 )
        //        {
        //            ViewBag.ListofState = await StateList();
        //            ViewBag.ErrorMessage = "Please fill in the required fields";
        //            return View();
        //        }
        //        //Check for BVN
        //        var BVNExists = await _unitOfWork.User.GetExistingUserBVN(model.BVN);
        //        if (BVNExists != null)
        //        {
        //            ViewBag.ListofState = await StateList();
        //            ViewBag.ErrorMessage = "BVN already exist on the System,Account not created.";
        //            return View();
        //        }

        //        //var key = "b14ca5898asngi3bbce2ea2315a1916";

        //        if (model.ReferralCode != null)
        //        {
        //            //Get bank Wallet Details
        //            var getwalletdetails = await _unitOfWork.Wallet.GetWalletByAccountNumber(model.ReferralCode);
        //            if (getwalletdetails == null)
        //            {
        //                ViewBag.ListofState = await StateList();
        //                ViewBag.ErrorMessage = "Referral Code doesn't exist,Account not created.";
        //                return View();
        //            }
        //            //Create sub agent tied to bank
        //            var BUserViewModel = new CreateUserViewModel(); 
        //            var Bstate = _unitOfWork.State.Get(model.StateID);  
        //            var Brole = await _roleManager.Roles.Where(r => r.Name == "SubAgent").SingleOrDefaultAsync(); 
        //            var BroleDet = _unitOfWork.Role.Get((int)Brole.RoleID); 
        //            var BuserType = _unitOfWork.UserType.Get(1);
        //            var subagentsExists = await _unitOfWork.User.GetUserByEmailAndPhoneNumber(model.Email.Trim(), model.PhoneNumber);
        //            if (subagentsExists == null)
        //            {  
        //                    //Create user details on EgolePay platform
        //                    var sysUser = new SysUsers
        //                    {
        //                        FirstName = model.FirstName,
        //                        LastName = model.LastName,
        //                        MiddleName = model.MiddleName,
        //                        Gender = model.Gender,
        //                        //PhoneNumber = model.PhoneNumber,
        //                        Email = model.Email,
        //                        ResidentialState = Bstate,
        //                        IsActive = true,
        //                        IsVerified = false,
        //                        DateCreated = DateTime.Now,
        //                        //Wallet =getwalletdetails.User.Wallet,
        //                        BankAccountCode = model.ReferralCode,
        //                        Role = BroleDet,
        //                        UserType = BuserType,
        //                        //BVN = model.BVN,
        //                        //DateOfBirth = model.DOB.ToString()
        //                        PhoneNumber = model.PhoneNumber,
        //                        BVN = model.BVN,
        //                        DateOfBirth = model.DOB.ToString()
        //                    };

        //                    await _unitOfWork.CreateUser.AddAsync(sysUser);
        //                    _unitOfWork.Complete();
        //                    var systemUser = new ApplicationUser { UserName = sysUser.Email, Email = sysUser.Email, UserID = sysUser.UserID };
        //                    var password = Guid.NewGuid().ToString("N").Substring(0, 8) + "ep@0X1";
        //                    var Bresult = await _userManager.CreateAsync(systemUser, password);

        //                    //Checking if Identity details have been created
        //                    if (Bresult.Succeeded)
        //                    {
        //                        _logger.LogInformation("User created a new account with password.");
        //                        // code for adding user to role
        //                        var userAddToRoleResult = await _userManager.AddToRoleAsync(systemUser, "SubAgent");

        //                        if (userAddToRoleResult.Succeeded)
        //                        {
        //                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(systemUser);
        //                            var Message = $"Please login to your  account at <a href='#'>clicking here</a>. Your password is {password}";
        //                            var Message12 = "Dear " + sysUser.FirstName + " " + sysUser.LastName + ",<br/><br/>" +
        //                                                "Congratulations!" + "<br/><br/>" +
        //                                                "You have just been created on our platform as user with the username " + sysUser.Email + ".<br/><br/>" +
        //                                                "Find below your <b>Username</b> and <b>Password</b>;" +
        //                                                "<br/>" +
        //                                                "- Username: " + sysUser.Email + "<br/>" +
        //                                                "- Password: " + password + "<br/><br/>" +
        //                                                "Please login to confirm your email<br/>" +
        //                                                "EgolePay Team.<br/>" +
        //                                                "https://egolepay.com" + "<br/>";
        //                            var Message2 = $"Your password is {password}";

        //                       // await _mimeSender.Execute("", "Confirm your email", Message12, sysUser.Email);
        //                        await _sendgridmail.Execute(sysUser.Email, "Confirm your email", Message12);


        //                        ViewBag.Message = "User has been registered successfully"; 
        //                            ModelState.Clear();
        //                        ViewBag.ListofState = await StateList();
        //                        return View();
        //                    } 
        //                }  else
        //                {
        //                    ViewBag.ListofState = await StateList();
        //                    ViewBag.ErrorMessage = "-" + "User Details not created successfully, kindly reattempt!";
        //                    return View();
        //                } 
        //            } 
        //        }

        //        //ViewBag.ListofState = StateList(); 
        //        var systemUserViewModel = new CreateUserViewModel();

        //        var State = _unitOfWork.State.Get(model.StateID);

        //        //var cbsRole = _unitOfWork.Role.Get(Convert.ToInt32(model.RoleID));
        //        var role = await _roleManager.Roles.Where(r => r.Name == "General").SingleOrDefaultAsync();

        //        var roleDet = _unitOfWork.Role.Get((int)role.RoleID);

        //        var userType = _unitOfWork.UserType.Get(1);

        //        var usernameExists = await _unitOfWork.User.GetUserByEmailAndPhoneNumber(model.Email.Trim(), model.PhoneNumber);

        //        if (usernameExists == null)
        //        {
        //            var VFDServiceConfig = Startup.StaticConfig.GetSection("VFDService").Get<VFDService>();
        //            //Call VFD endpoint to create wallet for the new user at VFD endpoint 
        //            var dob = Convert.ToDateTime(model.DOB).ToString("dd-MMM-yyyy");
        //            var clientUrl = $"{VFDServiceConfig?.Url}?bvn={model.BVN}&wallet-credentials={VFDServiceConfig?.walletCredentials}&dateOfBirth={dob}";
        //            var client = new RestClient(clientUrl);
        //            var request = new RestRequest(Method.POST);
        //            request.AddHeader("Authorization", "Bearer " + VFDServiceConfig.token);
        //            request.AddHeader("Accept", "application/json");
        //            var response = client.Execute(request);

        //            //Getting the response from VFD endpoint
        //            var content = response.Content;

        //            VVFDResponseViewModel getVVFDResponse = JsonConvert.DeserializeObject<VVFDResponseViewModel>(content);

        //            var getStatus = getVVFDResponse.status;

        //            if (getVVFDResponse.status.ToString() != "00")
        //            {
        //                ViewBag.ListofState = await StateList();
        //                ViewBag.ErrorMessage = getVVFDResponse.message.ToString();
        //                return View();
        //            }

        //            //Successful creation of VFD Wallet Account - if true, then proceed to create Egole User Details and Wallet details
        //            if (getStatus == "00")
        //            {
        //                ViewBag.ListofState = await StateList();
        //                //Create user details on EgolePay platform
        //                var sysUser = new SysUsers
        //                {
        //                    FirstName = model.FirstName,
        //                    LastName = model.LastName,
        //                    MiddleName = model.MiddleName,
        //                    Gender = model.Gender,
        //                    Email = model.Email,
        //                    ResidentialState = State,
        //                    IsActive = true,
        //                    IsVerified = false,
        //                    DateCreated = DateTime.Now,
        //                    Role = roleDet,
        //                    UserType = userType,
        //                    PhoneNumber = model.PhoneNumber,
        //                    BVN = model.BVN,
        //                    DateOfBirth = model.DOB.ToString()
        //                };

        //                await _unitOfWork.CreateUser.AddAsync(sysUser);
        //                _unitOfWork.Complete();
        //                var systemUser = new ApplicationUser { UserName = sysUser.Email, Email = sysUser.Email, UserID = sysUser.UserID };
        //                var password = Guid.NewGuid().ToString("N").Substring(0, 8) + "ep@0X1";
        //                var result = await _userManager.CreateAsync(systemUser, password);

        //                //ViewBag.ListofState = await StateList();
        //                //Checking if Identity details have been created
        //                if (result.Succeeded)
        //                {
        //                    _logger.LogInformation("User created a new account with password.");
        //                    // code for adding user to role
        //                    var userAddToRoleResult = await _userManager.AddToRoleAsync(systemUser, "General");

        //                    if (userAddToRoleResult.Succeeded)
        //                    {
        //                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(systemUser);
        //                        var Message = $"Please login to your  account at <a href='#'>clicking here</a>. Your password is {password}";
        //                        var Message12 = "Dear " + sysUser.FirstName + " " + sysUser.LastName + ",<br/><br/>" +
        //                                            "Congratulations!" + "<br/><br/>" +
        //                                            "You have just been created on our platform as user with the username " + sysUser.Email + ".<br/><br/>" +
        //                                            "Find below your <b>Username</b> and <b>Password</b>;" +
        //                                            "<br/>" +
        //                                            "- Username: " + sysUser.Email + "<br/>" +
        //                                            "- Password: " + password + "<br/><br/>" +
        //                                            "Please login to confirm your email<br/>" +
        //                                            "EgolePay Team.<br/>" +
        //                                            "https://egolepay.com" + "<br/>";
        //                        var Message2 = $"Your password is {password}";

        //                        //Creating a Wallet on EgolePay platform for the new User just created
        //                        var wallet = new EgsWallet
        //                        {
        //                            WalletAccountNumber = getVVFDResponse.data.accountNo,
        //                            DateCreated = DateTime.Now,
        //                            IsActive = true,
        //                            User = sysUser,
        //                            BVN = model.BVN
        //                        };
        //                        await _unitOfWork.Wallet.AddSaveAsync(wallet);

        //                        sysUser.Wallet = wallet;

        //                        _unitOfWork.CreateUser.UpdateUser(sysUser);

        //                        _unitOfWork.Complete();

        //                        //await _emailSender.SendEmailAsync(sysUser.Email, "Confirm your email", Message12);

        //                        ///await _mimeSender.Execute("", "Confirm your email", Message12, sysUser.Email);

        //                        await _sendgridmail.Execute(sysUser.Email, "Confirm your email", Message12);

        //                        ViewBag.Message = "User has been registered successfully";

        //                        //ViewBag.ListofState = await StateList();

        //                        ModelState.Clear();


        //                    }
        //                }

        //                //Creating a Wallet on EgolePay platform for the new User just created
        //                //var wallet = new EgsWallet
        //                //{
        //                //    WalletAccountNumber = getVVFDResponse.data.accountNo,
        //                //    DateCreated = DateTime.Now,
        //                //    IsActive = true,
        //                //    User = sysUser,
        //                //    BVN = model.BVN
        //                //};
        //                //await _unitOfWork.Wallet.AddSaveAsync(wallet);

        //                //sysUser.Wallet = wallet;

        //                //_unitOfWork.CreateUser.UpdateUser(sysUser);

        //                //_unitOfWork.Complete();

        //                //ModelState.Clear();
        //            }
        //            //Unsuccessful creation of VFD Wallet Account, cannot proceed to Egole User details and Wallet Details
        //            else
        //            {
        //                ViewBag.ErrorMessage = getVVFDResponse.message + " " + "-" +  "User Details not created successfully, kindly reattempt!";
        //            }

        //        }
        //        else
        //        {
        //            ViewBag.ErrorMessage = "Email or Phone Number already exist on the System";
        //        }


        //        ViewBag.ListofState = await StateList();

        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.Message + ex.StackTrace);
        //    }

        //    return View();
        //}



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





//var VFDOnboardingeConfig = Startup.StaticConfig.GetSection("VFDOnboardingService").Get<VFDService>();

//var onboardingUrl = $"{VFDOnboardingeConfig?.devUrl}wallet-credentials={VFDOnboardingeConfig?.walletCredentials}";
//var wname = model.FirstName + " " + model.LastName;
//var trname = wname.Substring(0, 4);

//var onboarddata = new CorporateOnboardingRequest
//{
//   username= "egolepay",
//   walletName= wname,
//   webhookUrl="",
//   shortName= trname,
//   implementation="Pool"
//};

//var winputs = JsonConvert.SerializeObject(onboarddata);

//var client = new RestClient(onboardingUrl);
//var wrequest = new RestRequest(Method.POST);
//wrequest.AddHeader("Authorization", "Bearer " + VFDOnboardingeConfig.token);
//wrequest.AddHeader("Accept", "application/json");
//wrequest.AddJsonBody(winputs);
//var response = client.Execute(wrequest);

//var wrequest = new HttpRequestMessage(HttpMethod.Post, "");

// wrequest.Headers.Add("Authorization", "Bearer " + VFDOnboardingeConfig.token);
// wrequest.Headers.Add("Accept", "application/json");
//        wrequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
// wrequest.Content = new StringContent(winputs, Encoding.UTF8);
//using (HttpClient httpClient = new HttpClient())
//{
//    httpClient.BaseAddress = new Uri(onboardingUrl);

//var responses = await httpClient.SendAsync(wrequest);

//var ncontent =  response.Content.ToString();
//  VFDOnboardingResponse

// var onboardingResponse = JsonConvert.DeserializeObject<VFDOnboardingResponse>(ncontent);

// if (onboardingResponse.status !=  "00")

//}


