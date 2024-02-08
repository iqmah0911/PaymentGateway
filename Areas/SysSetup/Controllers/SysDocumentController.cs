using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    [Authorize(Roles = "Super Admin")]
    public class SysDocumentController : Controller
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

        public SysDocumentController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Document()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            List<DocumentType> docuTypeList = new List<DocumentType>();  
            ViewBag.Role = await _unitOfWork.Role.UpgradeRoles();
            //ViewBag.UserType = _unitOfWork.UserType.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Document(DocumentsViewModel model)
        { 
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();


            var clientUrl = APIServiceConfig?.Url;
            var docResponse = new DocumentsResponse();

            DocumentParamsModel paramsBody = new DocumentParamsModel()
            {
                UserTypeID = model.RoleID,
                DocumentID = 0,//model.documentID,
                DocumentName = model.documentName,
                DocumentType = model.documentType,//"PDF",
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID,
            };

            var bodyRequest = JsonConvert.SerializeObject(paramsBody, Formatting.Indented);

            using (var _client = new HttpClient())
            {
                string IResponse = General.MakeVFDRequest(clientUrl + "/api/Document/add", null, "POST", null, bodyRequest);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    docResponse = JsonConvert.DeserializeObject<DocumentsResponse>(IResponse);

                    if (Convert.ToInt32(docResponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + docResponse.message;
                        return View();
                    }
                }

            }

            ViewBag.Role = await _unitOfWork.Role.UpgradeRoles();
            ViewBag.Message = docResponse.message;
            return View();
        }


        public async Task<IActionResult> Documents(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DocumentsViewModel> model = new List<DocumentsViewModel>();

                var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

                var clientUrl = APIServiceConfig?.Url;
                var docResponse = new DocumentsResponse();

                using (var _client = new HttpClient())
                {
                    //var Srequest = (clientUrl, vfdEnquiryReqParams, "GET");
                    string IResponse = General.MakeVFDRequest(clientUrl + "/api/Document", null, "GET");

                    if (!String.IsNullOrEmpty(IResponse))
                    {
                        docResponse = JsonConvert.DeserializeObject<DocumentsResponse>(IResponse);

                        if (Convert.ToInt32(docResponse.status) >= 1)
                        {
                            ViewBag.ErrorMessage = "Response " + docResponse.message;
                            return View();
                            //return RedirectToAction("OrderInfo", "WalletServices", new { area = "Wallets", id = prdid });
                        }
                    }
                }

                var SysDocument = docResponse.documents;

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    SysDocument = SysDocument.Where(Document => Document.documentName.Contains(searchText, StringComparison.OrdinalIgnoreCase) || Document.RoleName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var Document in SysDocument)
                {
                    model.Add(new DocumentsViewModel
                    {
                        documentID = Document.documentID,
                        documentName = Document.documentName,
                        documentType = Document.documentType,
                        RoleName = Document.RoleName,
                        dateCreated = Document.dateCreated
                    });
                }

                var pager = new Pager(SysDocument.Count(), pageIndex);

                var modelR = new HoldDocumentDisplayViewModel
                {
                    HoldAllDocument = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };

                return View(modelR);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateDocument(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                
                var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

                var clientUrl = APIServiceConfig?.Url;
                var docResponse = new DocumentsResponse();

                using (var _client = new HttpClient())
                {
                    //var Srequest = (clientUrl, vfdEnquiryReqParams, "GET");
                    string IResponse = General.MakeVFDRequest(clientUrl + "/api/Document/docrole/" + Convert.ToInt32(id).ToString(), null, "GET");

                    if (!String.IsNullOrEmpty(IResponse))
                    {
                        docResponse = JsonConvert.DeserializeObject<DocumentsResponse>(IResponse);

                        if (Convert.ToInt32(docResponse.status) >= 1)
                        {
                            ViewBag.ErrorMessage = "Response " + docResponse.message;
                            return View();
                        }
                    }
                }

                //var SysDocument = docResponse.document;
                var docuument = new DocumentsViewModel
                {
                    documentID = docResponse.document.documentID,
                    documentName = docResponse.document.documentName,
                    documentType = docResponse.document.documentType,
                    RoleName = docResponse.document.RoleName
                };

                ViewBag.Role = await RoleList();
                return View(docuument);
                //return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDocument(DocumentsViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();


            var clientUrl = APIServiceConfig?.Url;
            var docResponse = new DocumentsResponse();


            DocumentParamsModel paramsBody = new DocumentParamsModel()
            {
                //UserTypeID = model.RoleID,
                DocumentID = model.documentID,
                DocumentName = model.documentName,
                DocumentType = model.documentType,//"PDF",
                //RoleName = model.RoleName
                RoleID = model.RoleID
            };

            var bodyRequest = JsonConvert.SerializeObject(paramsBody, Formatting.Indented);

            using (var _client = new HttpClient())
            {
                string IResponse = General.MakeVFDRequest(clientUrl + "/api/Document/update", null, "POST", null, bodyRequest);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    docResponse = JsonConvert.DeserializeObject<DocumentsResponse>(IResponse);

                    if (Convert.ToInt32(docResponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + docResponse.message;
                        return View();
                    }
                }

            }

            ViewBag.Role = await RoleList();
            ViewBag.Message = docResponse.message;
            return View();
        }

        public async Task<List<RoleView>> RoleList()
        {
            List<RoleView> rollist = new List<RoleView>();

            var role = await _unitOfWork.Role.UpgradeRoles();

            foreach (var item in role)
            {
                rollist.Add(new RoleView
                {
                    RoleID = item.RoleID,
                    RoleName = item.RoleName
                });
            }

            RoleList aList = new RoleList();
            aList.Role = rollist;

            return aList.Role;
        }
    }
}
