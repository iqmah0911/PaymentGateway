using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.Dashboard.Models;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Helpers.Interface;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")] 
    [Authorize(Roles = "Super Admin")]
    public class UpgradeRequestController : Controller
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
        public IHostingEnvironment _hostingEnv { get; }


        public UpgradeRequestController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment, IMimeSender mimeSender,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            //_mailSender = emailSender;
            _hostingEnv = hostingEnvironment;
            _roleManager = roleManager;
            _mimeSender = mimeSender;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AllRequestList(int? pageIndex, string searchText)
        {
            try
            {
                string fNames = string.Empty;


                var key = "b14ca5898a4e4133wale2ea2315a1916";

                //getusertoupdate.BVN = Helpers.General.EncryptString(key, items.BVN);

                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                //getting image for user layout
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;

                List<UpgradeAccountViewModel> model = new List<UpgradeAccountViewModel>();
                List<CustDocument> docModel = new List<CustDocument>();
                var getRequests = await _unitOfWork.UpgradeAccount.GetAllRequests();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    getRequests = getRequests.Where(u => u.User.CompanyName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                               || u.User.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                u.User.PhoneNumber.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                u.User.BusinessName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                u.User.ResidentialState.StateName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 

                foreach (var getrequest in getRequests)
                {
                    var getuserkyc = await _unitOfWork.UserKYCInfo.GetUserKYCInfoByUserID(getrequest.User.UserID);
                    var uName = await _unitOfWork.User.GetSysUsers(getrequest.User.UserID.ToString());
                    var rName = await _unitOfWork.Role.GetRole(getrequest.RoleRequestID);

                    if (getuserkyc != null)
                    {
                        var getdocs = await _unitOfWork.DocumentValue.GetDocumentsbykycID(getuserkyc.UserKYCID);
                        foreach (var docum in getdocs)
                        {
                            docModel.Add(new CustDocument
                            {
                                DocumentID = docum.Document.DocumentID,
                                DocumentName = docum.Document.DocumentName,
                                DocumentPath = docum.DocPath,
                                DateCreated = docum.DateCreated,
                                UserKYCID = docum.KycInfo.UserKYCID
                            });
                        }
                    }

                    if (uName.CompanyName == null || uName.CompanyName == "")
                    {
                        fNames = uName.FirstName + " " + uName.MiddleName + " " + uName.LastName;
                    }
                    else
                    {
                        fNames = uName.CompanyName;
                    }

                    model.Add(new UpgradeAccountViewModel
                    {
                        UpgradeAccountID = getrequest.UpgradeAccountID,
                        RoleRequestId = getrequest.RoleRequestID,
                        UserID = uName.UserID,
                        FullNames = fNames,
                        Email = uName.Email,
                        PhoneNumber = uName.PhoneNumber,
                        Address = uName.Address,
                        Gender = uName.Gender,
                        KYC = Convert.ToString(uName.KYCID),
                        BusinessName = uName.BusinessName,
                        BusinessAddress = uName.BusinessAddress,
                        DateCreated = Convert.ToDateTime(getrequest.DateCreated), //uName.DateCreated,
                        RoleID = uName.Role.RoleID,
                        Role = uName.Role.RoleName,
                        RoleRequestName = rName.RoleName,
                        StateID = uName.ResidentialState.StateID,
                        StateName = uName.ResidentialState.StateName,
                        CompanyName = uName.CompanyName,
                        BVN = uName.BVN,
                        //DOB =  Convert.ToDateTime(uName.DateOfBirth),
                        documents = docModel

                    });
                }
                var pager = new Pager(getRequests.Count(), pageIndex);
                var modelR = new HoldUpgradeAccountViewModel
                {
                    HoldAllRequests = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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

        public async Task<IActionResult> AllRequestList_Old(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            List<UpgradeAccountViewModel> model = new List<UpgradeAccountViewModel>();
            var getRequests = await _unitOfWork.UpgradeAccount.GetAllRequests();

            ////Logic for search
            if (!String.IsNullOrEmpty(searchText))
            {
                getRequests = getRequests.Where(u => u.User.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            //load up the list of viewmodels 
            foreach (var getrequest in getRequests)
            { 
                var uName = await _unitOfWork.User.GetSysUsers(getrequest.User.UserID.ToString());
                var rName = await _unitOfWork.Role.GetRole(getrequest.RoleRequestID);

                model.Add(new UpgradeAccountViewModel
                {
                    UpgradeAccountID = getrequest.UpgradeAccountID,
                    User = uName, //getrequest.User,
                    RoleRequestId = getrequest.RoleRequestID,
                    UserName = uName.Email,
                    RoleRequestName = rName.RoleName,
                    DateCreated = getrequest.DateCreated
                });
            }

            var pager = new Pager(getRequests.Count(), pageIndex);
            var modelR = new HoldUpgradeAccountViewModel
            {
                HoldAllRequests = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
 
            return View(modelR);
        }

        public async Task<IActionResult> ProcessRequest(int? id)
        {
            try
            {
                string fNames = string.Empty;

                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                //getting image for user layout
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;

                UpgradeAccountViewModel model = new UpgradeAccountViewModel();
                List<CustDocument> docModel = new List<CustDocument>();

                if (id == 0)
                {
                    return RedirectToAction("AllRequestList");
                }
                var getrequest = await _unitOfWork.UpgradeAccount.GetRequestById(Convert.ToInt32(id));
                if (getrequest == null)
                {
                    return RedirectToAction("AllRequestList");
                }
                 
               //load up the list of viewmodels 

                    var getuserkyc = await _unitOfWork.UserKYCInfo.GetUserKYCInfoByUserID(getrequest.User.UserID);
                    var uName = await _unitOfWork.User.GetSysUsers(getrequest.User.UserID.ToString());
                    var rName = await _unitOfWork.Role.GetRole(getrequest.RoleRequestID);

                    if (getuserkyc != null)
                    {
                        var getdocs = await _unitOfWork.DocumentValue.GetDocumentsbykycID(getuserkyc.UserKYCID);
                        foreach (var docum in getdocs)
                        {
                            docModel.Add(new CustDocument
                            {
                                DocumentID = docum.Document.DocumentID,
                                DocumentName = docum.Document.DocumentName,
                                DocumentPath = docum.DocPath,
                                DateCreated = docum.DateCreated,
                                UserKYCID = docum.KycInfo.UserKYCID
                            });
                        }
                    }

                    if (uName.CompanyName == null || uName.CompanyName == "")
                    {
                        fNames = uName.FirstName + " " + uName.MiddleName + " " + uName.LastName;
                    }
                    else
                    {
                        fNames = uName.CompanyName;
                    }

                    model = new UpgradeAccountViewModel
                    {
                        UpgradeAccountID = getrequest.UpgradeAccountID,
                        RoleRequestId = getrequest.RoleRequestID,
                        UserID = uName.UserID,
                        FullNames = fNames,
                        Email = uName.Email,
                       // PhoneNumber = uName.PhoneNumber,
                        Address = uName.Address,
                        Gender = uName.Gender,
                        BusinessName = uName.BusinessName,
                        BusinessAddress = uName.BusinessAddress,
                        KYC = Convert.ToString(uName.KYCID),
                        DateCreated = Convert.ToDateTime(getrequest.DateCreated),//uName.DateCreated,
                        RoleID = uName.Role.RoleID,
                        Role = uName.Role.RoleName,
                        RoleRequestName = rName.RoleName,
                        StateID = uName.ResidentialState.StateID,
                        StateName = uName.ResidentialState.StateName,
                        CompanyName = uName.CompanyName,
                        BVN = uName.BVN,
                        //DOB = Convert.ToDateTime(uName.DateOfBirth),
                        documents = docModel
                    };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessRequest(UpgradeAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                ViewBag.message = "Please fill in the required fields.";
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
            var clientUrl = APIServiceConfig?.Url;

            var upgradeResponse = new UpgradeAccountResponse(); 
            PostUpgradeViewModel paramsBody = new PostUpgradeViewModel()
            {
                UpgradeAccountID = model.UpgradeAccountID,
                Email = model.Email,
                RoleRequestID = model.RoleRequestId,
                IsProcessed = model.IsProcessed,
                Comment = model.Comment,
                ProcessedBy = user2.UserID,
                DateProcessed = DateTime.Now,
                DateCreated = model.DateCreated,
                CreatedBy = user2.UserID

            };

            var bodyRequest = JsonConvert.SerializeObject(paramsBody, Formatting.Indented);

            using (var _client = new HttpClient())
            {
                string IResponse = General.MakeVFDRequest(clientUrl + "/api/Account/ProcessRequest", null, "POST", null, bodyRequest);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    upgradeResponse = JsonConvert.DeserializeObject<UpgradeAccountResponse>(IResponse);

                    var declineduser = await _unitOfWork.User.GetUserByEmail(model.Email);
                    if (Convert.ToInt32(upgradeResponse.StatusCode) >= 1)
                    {
                        var Message12 = "Dear " + declineduser.FirstName + " " + declineduser.LastName + ",<br/><br/>" +
                                                  "Kindly note,Your Request to be upgraded on EgolePay has been declined, comment:" +model.Comment +"<br/><br/>"+
                                                  "" + "<br/>";
                        await _mimeSender.Execute("", "Upgrade Request Declined", Message12, model.Email);

                        ViewBag.ErrorMessage = "Response " + upgradeResponse.message;
                        //return View("ProcessRequest", new { id = model.UpgradeAccountID });

                        return RedirectToAction("AllRequestList", "UpgradeRequest");
                    }
                }
            }
            ViewBag.Message = upgradeResponse.message;

            //return View(model);
            return RedirectToAction("AllRequestList", "UpgradeRequest");
        }

         
        public FileResult DownloadFile(string FileName)
        {
            //string path = Path.Combine(this._hostingEnv.WebRootPath) + FileName;
            string path = Path.Combine(FileName);

            var fileextension = Path.GetExtension(FileName).ToLower();


            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", FileName);

        }

















        public async Task<IActionResult> ProcessRequest_OLD(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            if (id == 0)
            {
                return RedirectToAction("AllRequestList");
            }
            var aggdata = await _unitOfWork.UpgradeAccount.GetRequestById(Convert.ToInt32(id));
            if (aggdata == null)
            {
                return RedirectToAction("AllRequestList");
            }

            var getkyc = await _unitOfWork.UserKYCInfo.GetUserKYCInfoByUserID(aggdata.User.UserID);
            var userdocs = await _unitOfWork.DocumentValue.GetDocumentsbykycID(getkyc.UserKYCID);
            var uName = await _unitOfWork.User.GetSysUsers(aggdata.User.UserID.ToString());
            var rName = await _unitOfWork.Role.GetRole(aggdata.RoleRequestID);

            List<CustDocument> sdoc = new List<CustDocument>();

            foreach (var doc in userdocs)
            {
                sdoc.Add(new CustDocument
                {
                    DocumentName = doc.Document.DocumentName,
                    DocumentPath = doc.DocPath,
                    DateCreated = doc.DateCreated,
                    DocumentID = doc.Document.DocumentID
                });
            };

            var aggmodel = new UpgradeAccountViewModel
            {
                UpgradeAccountID = aggdata.UpgradeAccountID,
                UserName = uName.Email,
                Email = uName.Email,
                //------
                Address = uName.Address,
                Company = uName.CompanyName,
                FirstName = uName.FirstName,
                LastName = uName.LastName,
                MiddleName = uName.MiddleName,

                //BankID = 
                //BankAccount//
                RoleRequestName = rName.RoleName,
                User = uName,
                RoleRequestId = aggdata.RoleRequestID,
                DateCreated = aggdata.DateCreated,
                documents = sdoc
            };

            return View(aggmodel);
        }


        [HttpPost]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpgradeAccount(UpgradeAccountViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Please fill in the required fields.");
                    return View();
                }


                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

                var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                var clientUrl = APIServiceConfig?.Url;

                var processaggregatorResponse = new UpgradeAccountResponse();


                //var bodyRequest = JsonConvert.SerializeObject(paramsBody, Formatting.Indented);

                //using (var _client = new HttpClient())
                //{
                //    string IResponse = General.MakeVFDRequest(clientUrl + "/api/Account/upgradeAccount", null, "POST", null, bodyRequest);

                //    if (!String.IsNullOrEmpty(IResponse))
                //    {
                //        upgradeResponse = JsonConvert.DeserializeObject<UpgradeAccountResponse>(IResponse);

                //        if (Convert.ToInt32(upgradeResponse.status) >= 1)
                //        {
                //            ViewBag.ErrorMessage = "Response " + upgradeResponse.message;
                //            return View();
                //        }
                //    }
                //}
                //ViewBag.Message = upgradeResponse.message;


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
