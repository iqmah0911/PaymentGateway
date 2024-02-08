using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//using static PaymentGateway21052021.Areas.SysSetup.Models.SysCustomerViewModel;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    public class SysCustomerController : Controller
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

        public SysCustomerController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> CustomerDetailsView(int? pageIndex, string searchText)
        {
            try
            {
                string fNames = string.Empty;

                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                //getting image for user layout
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;

                List<SysCustomerViewModel> model = new List<SysCustomerViewModel>();
                List<CustDocument> docModel = new List<CustDocument>();
                var getUsers = await _unitOfWork.User.GetAllActiveUsers();
                 
                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    getUsers = getUsers.Where(u => u.CompanyName.Contains(searchText, StringComparison.OrdinalIgnoreCase) 
                               || u.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                u.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                u.MiddleName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                u.LastName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                foreach (var item in getUsers.Where(x=>x.Role.RoleID !=2))
                {
                    var getuserkyc = await _unitOfWork.UserKYCInfo.GetUserKYCInfoByUserID(item.UserID);
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

                    if (item.CompanyName == null || item.CompanyName == "")
                    {
                        fNames = item.FirstName + " " + item.MiddleName + " " + item.LastName;
                    }
                    else
                    {
                        fNames = item.CompanyName;
                    }

                    model.Add(new SysCustomerViewModel
                    {
                        UserID = item.UserID,
                        ////FirstName = item.FirstName,
                        ////MiddleName = item.MiddleName,
                        ////LastName = item.LastName,
                        FullNames = fNames,
                        Email = item.Email,
                        PhoneNumber = item.PhoneNumber,
                        Address = item.Address,
                        Gender = item.Gender,
                        KYC = Convert.ToString(item.KYCID),
                        DateCreated = item.DateCreated,
                        RoleID = item.Role.RoleID,
                        Role = item.Role.RoleName,
                        StateID = item.ResidentialState.StateID,
                        StateName = item.ResidentialState.StateName,
                        //LgaID = item.Lga.LgaID,
                        //LgaName = item.Lga.LgaName,
                        CompanyName = item.CompanyName,
                        BVN = item.BVN,
                        DOB = Convert.ToDateTime(item.DateOfBirth),
                        documents = docModel

                    });
                }
                var pager = new Pager(getUsers.Count(), pageIndex);
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
         

        public async Task<IActionResult> ViewCustomer(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            string fNames = string.Empty;
            List<CustDocument> docModel = new List<CustDocument>();
            var customerdata = await _unitOfWork.User.GetSysUsers(id.ToString());
            var getcustomerkyc = await _unitOfWork.UserKYCInfo.GetUserKYCInfoByUserID(customerdata.UserID);

            if (getcustomerkyc != null)
            {
                var getdocs = await _unitOfWork.DocumentValue.GetDocumentsbykycID(getcustomerkyc.UserKYCID);
                if (getdocs != null)
                {
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
            }
            if (customerdata.CompanyName == null || customerdata.CompanyName == "")
            {
                fNames = customerdata.FirstName + " " + customerdata.MiddleName + " " + customerdata.LastName;
            }
            else
            {
                fNames = customerdata.CompanyName;
            }

             var custdata = new SysCustomerViewModel
            {
                UserID = customerdata.UserID,
                //FirstName = customerdata.FirstName,
                //MiddleName = customerdata.MiddleName,
                //LastName = customerdata.LastName,
                FullNames = fNames,
                Email = customerdata.Email,
                PhoneNumber = customerdata.PhoneNumber,
                Address = customerdata.Address,
                Gender = customerdata.Gender,
                KYC = Convert.ToString(customerdata.KYCID),
                DateCreated = customerdata.DateCreated,
                RoleID = customerdata.Role.RoleID,
                Role = customerdata.Role.RoleName,
                StateID = customerdata.ResidentialState.StateID,
                StateName = customerdata.ResidentialState.StateName, 
                CompanyName = customerdata.CompanyName,
                BVN = customerdata.BVN,
                DOB = Convert.ToDateTime(customerdata.DateOfBirth),
                documents = docModel 
            };
             
            return View(custdata);
        }


        public FileResult DownloadFile(string FileName)
        {
            //string path = Path.Combine(this._hostingEnv.WebRootPath) + FileName;
            string path = Path.Combine(FileName);

            //var fileextension = Path.GetExtension(FileName).ToLower();


            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", FileName);

        }





    }
}
