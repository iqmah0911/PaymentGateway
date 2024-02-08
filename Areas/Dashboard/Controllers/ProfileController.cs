using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.Dashboard.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentGateway21052021.Areas.SysSetup.Models;
using Newtonsoft.Json;
using System.Net.Http;
using RestSharp;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Text;

namespace PaymentGateway21052021.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    //[Authorize]
    public class ProfileController : Controller
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
        public IHostingEnvironment _hostingEnv { get; } 
        private readonly long MAX_FILE_SIZE = 10 * 1024 * 1024;
        private readonly string[] ACCEPTED_FILE_TYPES = new[] { ".jpeg", ".jpg", ".png", ".pdf" };
        private readonly string[] ACCEPTED_PHOTO_TYPES = new[] { ".jpeg", ".jpg", ".png"};

        #region logger
        private readonly IEmailSender _mailSender;
        #endregion

        public ProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment,/*EmailSender emailSender,*/
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            //_mailSender = emailSender;
            _hostingEnv = hostingEnvironment;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region "Profile"
        //Default View Page for Profile
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> MyProfile(int? pageIndex)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return LocalRedirect("/Identity/Account/Login");
                }
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                bool bisFilled = true, norUser = true;

                DisplayProfileViewModel model = new DisplayProfileViewModel(); 
                //var Products = await _unitOfWork.Products.GetProductList();
                //var Products = _unitOfWork.Products.GetAll();

                //Logic for search
                if (user2.BankAccountCode == null)
                {
                    var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                    ViewBag.pic = profiles.Image;
                    //var profileWalletNumber = _unitOfWork.Wallet.GetAll();

                    ViewBag.UserName = profiles.Email;
                    ViewBag.WalletAccountNumber = profiles.Wallet?.WalletAccountNumber ?? null;
                    ViewBag.FirstName = profiles.FirstName;
                    ViewBag.LastName = profiles.LastName;
                    ViewBag.BusinessAddress = profiles.BusinessAddress;
                    ViewBag.BusinessName = profiles.BusinessName;

                    var roleInfo = await _unitOfWork.UpgradeAccount.GetRequestByUserId(user2.UserID);

                    if (roleInfo != null)
                    {
                        norUser = false;
                        if (profiles.BusinessName == null)
                        {
                            bisFilled = false;

                        }
                        else
                        {
                            bisFilled = true;
                        }
                    }

                    var ggmod = new List<AggregatorCommsnVModel>();
                    if (user2.Role.RoleName == "Aggregator")
                    {
                        var getaggcommsn = await _unitOfWork.AggregatorCommission.GetAggregatorMonthlyCommision(Convert.ToInt32(user2.AggregatorID));
                        //date.AddMonths(12).AddDays(-1);

                        foreach (var aggcmms in getaggcommsn.HoldAllCommsn)
                        {
                            ggmod.Add(new AggregatorCommsnVModel
                            {
                                monthname = aggcmms.monthname,//GetMonthInWord(aggcmms.monthname),
                                //SettlementDate = aggcmms.SettlementDate,
                                Amount = aggcmms.Amount
                            });
                        }

                        var pager = new Pager(getaggcommsn.HoldAllCommsn.Count(), pageIndex);
                        var modelR = new HoldDisplayAggViewModel
                        {
                            HoldAllCommsn = ggmod.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                            Pager = pager
                        };

                        var displayAgg = new DisplayProfileViewModel
                        {
                            UserID = profiles.UserID,
                            RoleID = profiles.Role.RoleID,
                            UserName = profiles.Email,
                            FirstName = profiles.FirstName,
                            LastName = profiles.LastName,
                            Image = profiles.Image,
                            BusinessName = profiles.BusinessName,
                            BusinessAddress = profiles.BusinessAddress,
                            IsFilled = bisFilled,
                            IsGeneral = norUser,
                            DisplayAggViewModel = modelR
                            //ProfileIMG = profiles.
                            //WalletNumber = profiles.Wallet.WalletAccountNumber,
                        };

                        return View(displayAgg);
                    }


                    var displayUser = new DisplayProfileViewModel
                    {
                        UserID = profiles.UserID,
                        RoleID = profiles.Role.RoleID,
                        UserName = profiles.Email,
                        FirstName = profiles.FirstName,
                        LastName = profiles.LastName,
                        Image = profiles.Image,
                        BusinessName = profiles.BusinessName,
                        BusinessAddress = profiles.BusinessAddress,
                        IsFilled = bisFilled,
                        IsGeneral = norUser,
                        //ProfileIMG = profiles.
                        //WalletNumber = profiles.Wallet.WalletAccountNumber,
                    };

                    return View(displayUser);
                }
                else
                {
                    var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                    ViewBag.pic = profiles.Image;
                    //var profileWalletNumber = _unitOfWork.Wallet.GetAll();

                    ViewBag.UserName = profiles.Email;
                    ViewBag.WalletAccountNumber = user2.BankAccountCode;
                    ViewBag.FirstName = profiles.FirstName;
                    ViewBag.LastName = profiles.LastName;
                    ViewBag.BusinessAddress = profiles.BusinessAddress;
                    ViewBag.BusinessName = profiles.BusinessName;

                    var roleInfo = await _unitOfWork.UpgradeAccount.GetRequestByUserId(user2.UserID);

                    if (roleInfo != null)
                    {
                        norUser = false;
                        if (profiles.BusinessName == null)
                        {
                            bisFilled = false;
                        }
                        else
                        {
                            bisFilled = true;
                        }
                    }
                
                    var displayUser = new DisplayProfileViewModel
                    {
                        UserID = profiles.UserID,
                        RoleID = profiles.Role.RoleID,
                        UserName = profiles.Email,
                        FirstName = profiles.FirstName,
                        LastName = profiles.LastName,
                        Image = profiles.Image,
                        BusinessName = profiles.BusinessName,
                        BusinessAddress = profiles.BusinessAddress,
                        IsFilled = bisFilled,
                        IsGeneral = norUser,
                        //ProfileIMG = profiles.
                        //WalletNumber = profiles.Wallet.WalletAccountNumber,
                    };

                    return View(displayUser);
                }
             
               
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }
         
        [HttpPost] 
        public async Task<IActionResult> UpdateInfo(int UserID, string BusinessName, string BusinessAddress)
        {
            var profile = await _unitOfWork.User.GetUserProfile(UserID);
            profile.BusinessName = BusinessName;
            profile.BusinessAddress = BusinessAddress;

            await _unitOfWork.CreateUser.UpdateSaveAsync(profile);
            ViewBag.pic = profile.Image;
            //var profileWalletNumber = _unitOfWork.Wallet.GetAll();

            ViewBag.UserName = profile.Email;
            ViewBag.WalletAccountNumber = profile.Wallet?.WalletAccountNumber ?? null;
            ViewBag.FirstName = profile.FirstName;
            ViewBag.LastName = profile.LastName;
            ViewBag.BusinessAddress = profile.BusinessAddress;
            ViewBag.BusinessName = profile.BusinessName;


            var roleInfo = await _unitOfWork.UpgradeAccount.GetRequestByUserId(UserID);
            bool bisFilled = false, norUser = true;

            if (roleInfo != null)
            {
                norUser = false;
                if (profile.BusinessName == null)
                {
                    bisFilled = false;
                }
                else
                {
                    bisFilled = true;
                }
            }

            var displayUser = new DisplayProfileViewModel
            {
                UserID = profile.UserID,
                UserName = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Image = profile.Image,
                BusinessName = profile.BusinessName,
                BusinessAddress = profile.BusinessAddress,
                IsFilled = bisFilled,
                IsGeneral = norUser,
            };

            return View("MyProfile", displayUser);

        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePic(IFormFile file, int UserID)
        {
            var uploadsFolderPath = Path.Combine(_hostingEnv.WebRootPath, "UploadedPics/ProfilePics");

            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var profile = await _unitOfWork.User.GetUserProfile(UserID);
            //Retrieve current user picture. Will delete later before creating a new one to avoid uneccesary files floating in folder
            string guestCurrentPhotoUrl = profile.Image;

            //==DELETE EXISTING PIC FOR THIS USER BEFORE PROCEEDING====//
            var allUploadFolderFiles = Directory.GetFiles(uploadsFolderPath);
            if (profile.Image != null)
            {
                var theFile = allUploadFolderFiles.Where(fl => fl.Contains(guestCurrentPhotoUrl)).FirstOrDefault();
                //TODO: Delete this theFile later
                if (theFile != null)
                {
                    FileInfo fi = new FileInfo(theFile);
                    fi.Delete();
                }
                //============================================================//
            }

            if (file != null && file.Length > 0 && file.Length < MAX_FILE_SIZE)
            {
                if (ACCEPTED_PHOTO_TYPES.Any(s => s.ToLower() == Path.GetExtension(file.FileName).ToLower()))
                {
                    var firstImgFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var firstImgfilePath = Path.Combine(uploadsFolderPath, firstImgFileName);
                    var fileExtension = Path.GetExtension(firstImgFileName).ToString().ToLower();

                    if (fileExtension.ToString() != ".jpg")
                    {
                        ViewBag.UploadErrMsg = Path.GetExtension(file.FileName).ToString() + " Not allowed... Only jpg is allowed.";
                        var dUser = new DisplayProfileViewModel
                        {
                            UserID = profile.UserID,
                            UserName = profile.Email,
                            FirstName = profile.FirstName,
                            LastName = profile.LastName,
                            Image = profile.Image
                        };

                        return View("MyProfile", dUser);
                    }

                    using (var img = Image.Load(file.OpenReadStream()))
                    {
                        img.Mutate(x => x.Resize(500, 500));
                        //var filestrmRes = new FileStreamResult(memStrm, "image/jpg");
                        var stream = new FileStream(firstImgfilePath, FileMode.Create);
                        try
                        {
                            img.SaveAsJpeg(stream, new JpegEncoder { Quality = 100 });
                        }
                        finally
                        {
                            stream.Dispose();
                        }
                    }

                    profile.Image = firstImgFileName;

                    await _unitOfWork.CreateUser.UpdateSaveAsync(profile);
                    var displayUser = new DisplayProfileViewModel
                    {
                        UserID = profile.UserID,
                        UserName = profile.Email,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        Image = profile.Image
                    };

                    return View("MyProfile", displayUser);
                }
            }

            return View("MyProfile");
        }

        #region
        [HttpPost]
        public async Task<IActionResult> MyProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please update your profile.");
                return View();
            }
            return View();
        }
        #endregion

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpgradeAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            ViewBag.Roles = await UpgradeRoleList();
            ViewBag.IdentificationTypeList = await IdentificationTypeList();
            ViewBag.BankList = await BankList();
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> UpgradeAccount(SysViewModel model, List<IFormFile> files)
        {
            try
            { 

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Please fill in the required fields.");
                    return View();
                }
                ViewBag.Roles = await UpgradeRoleList();
                ViewBag.IdentificationTypeList = await IdentificationTypeList();
                ViewBag.BankList = await BankList();

                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

                var getrole = await _unitOfWork.Role.GetRole(model.RoleID);
                var doclist = await _unitOfWork.Document.GetDocumentByRole(model.RoleID);

                if (files.Count < 3)
                {
                    ViewBag.Error = "All files are required.";
                    return View();
                }
                else if (files.Count > 2)
                {
                    //==DELETE EXISTING FOLDER FOR THIS USER BEFORE PROCEEDING====//
                    var allUploadFolderFiles = Path.Combine(_hostingEnv.WebRootPath, "UploadedDocs/" + getrole.RoleName + "_" + user2.UserID);

                    if (Directory.Exists(allUploadFolderFiles))
                    {
                        var theFile = allUploadFolderFiles;
                        Directory.Delete(theFile, true); 
                        //============================================================//
                    }
                     
                    //combining formfiles and documentlist
                    var filesAnddocuments = files.Zip(doclist, (n, w) => new { file = n, document = w });

                    List<SDocument> docModel = new List<SDocument>();
                    //
                    foreach (var nw in filesAnddocuments)
                    {
                       //checking to allow only jpg and pdf file
                        if (nw.file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                        {
                            ViewBag.Error = "Only .jpg  and .pdf file allowed";
                            return View();
                        }
                         
                        var uploadsFolderPath = Path.Combine(_hostingEnv.WebRootPath, "UploadedDocs/" + getrole.RoleName + "_" + user2.UserID);
                        //==CREATE DIRECTORY IF NOT EXISTING BEFORE PROCEEDING====//
                        if (!Directory.Exists(uploadsFolderPath))
                        {
                            Directory.CreateDirectory(uploadsFolderPath);
                        }

                        if (nw.file != null && nw.file.Length > 0)
                        {
                            if (ACCEPTED_FILE_TYPES.Any(s => s.ToLower() == Path.GetExtension(nw.file.FileName).ToLower()))
                            {
                                var firstFileName = nw.file.FileName;
                                var firstfilePath = Path.Combine(uploadsFolderPath, firstFileName);

                                docModel.Add(new SDocument
                                {
                                    DocumentName = firstFileName,
                                    DocumentPath = firstfilePath,
                                    DateCreated = DateTime.Now,
                                    DocumentID = nw.document.DocumentID

                                });
                               if (nw.file.ContentType == "image/jpeg" || nw.file.ContentType == "image/png")
                                {
                                    using (var img = Image.Load(nw.file.OpenReadStream()))
                                    {
                                        img.Mutate(x => x.Resize(300, 300));

                                        var stream = new FileStream(firstfilePath, FileMode.Create);
                                        try
                                        {
                                            img.SaveAsJpeg(stream, new JpegEncoder { Quality = 100 });
                                        }
                                        finally
                                        {
                                            stream.Dispose();
                                        }
                                    }
                                }
                                else  //else if file is of type PDF
                                {
                                    using (FileStream fs = System.IO.File.Create(firstfilePath))
                                    {
                                        nw.file.CopyTo(fs);
                                    }
                                }      //if file is of type image
                           
                            }
                        }
                    }

                    /////
                    var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                    var clientUrl = APIServiceConfig?.Url;

                    var upgradeResponse = new UpgradeAccountResponse();

                    PostSysViewModel paramsBody = new PostSysViewModel()
                    {
                        UserID = user2.UserID,
                        Email = user2.Email,
                        ////Address = model.Address,
                        BankAccount = model.BankAccount,
                        BankID = model.BankID,
                        ////CompanyName = model.CompanyName,
                        BusinessAddress = model.BusinessAddress,
                        BusinessName = model.BusinessName,
                        IdentificationTypeID = model.IdentificationTypeID,
                        IdentificationValue = model.IdentificationValue,
                        //RoleID = model.RoleID,
                        RoleRequestID = model.RoleID,
                        RoleName = getrole.RoleName,
                        Documents = docModel
                    };

                    var bodyRequest = JsonConvert.SerializeObject(paramsBody, Formatting.Indented);
                     
                    using (var _client = new HttpClient())
                    {
                        var Srequest = (clientUrl + "/api/Account/upgradeAccount", "POST", bodyRequest); 
                        string IResponse = General.MakeVFDRequest(clientUrl + "/api/Account/upgradeAccount", null, "POST", null, bodyRequest);
                         
                         //Save Request and Response
                        var RequestResponseLog = new SysRequestResponseLog
                        {
                            Request = Srequest.ToString(),
                            Response = IResponse,
                            DateCreated = DateTime.Now
                        };
                        await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                        if (!String.IsNullOrEmpty(IResponse))
                        {
                            upgradeResponse = JsonConvert.DeserializeObject<UpgradeAccountResponse>(IResponse);

                            if (Convert.ToInt32(upgradeResponse.StatusCode) >= 1)
                            {
                                ViewBag.Error = "Response " + upgradeResponse.message;
                                return View();
                            }
                        }
                    }


                    ViewBag.Message = upgradeResponse.message;
                    ModelState.Clear();
                    return View();

                }
            }
            catch (Exception ex)
            {
                // General.LogToFile(ex); 
                throw ex;
                //_logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }




        [HttpGet]
        public JsonResult GetDocuments(int roleId)
        {
            if (roleId != 0)
            {
                var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                var clientUrl = APIServiceConfig?.Url;


                using (var _client = new HttpClient())
                {
                    var docResponse = new DocumentsResponse();
                    
                   string IResponse = General.MakeVFDRequest(clientUrl + "/api/Document/role/" + roleId.ToString(), null, "GET");
                     
                    if (!String.IsNullOrEmpty(IResponse))
                    {
                        docResponse = JsonConvert.DeserializeObject<DocumentsResponse>(IResponse);
                        var tt = docResponse.message;
                        if (Convert.ToInt32(docResponse.status) >= 1)
                        {
                            ViewBag.ErrorMessage = "Response " + docResponse.message;
                            return null;
                        }
                        return Json(docResponse.documents);
                    }
                    //docResponse.documents 
                }
            }
            return null;
        }


        public async Task<List<SysRole>> UpgradeRoleList()
        {
            List<SysRole> roleList = new List<SysRole>();

            roleList = await _unitOfWork.Role.UpgradeRoles();

            roleList.Insert(0, new SysRole { RoleID = 0, RoleName = "--Select Role--" });

            return roleList;
        }

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



        public static string GetMonthInWord(int month)
        {
            var monthNames = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            int getMonth = month;

            return $"{monthNames[getMonth - 1] }";
        }




    }
}
#endregion