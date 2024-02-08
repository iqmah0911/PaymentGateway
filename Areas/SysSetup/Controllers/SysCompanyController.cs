using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    [Authorize(Roles = "Super Admin")]
    public class SysCompanyController : Controller
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

        public SysCompanyController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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

        // GET: SysCompanyControler
        public ActionResult Index()
        {
            return View();
        }

        #region "SysCompany"
        //Display view for creating up Company
        [HttpGet]
        //[Authorize()]
        public async Task<IActionResult> Company()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            //ViewBag.GLAccountList = await GLAccountList();
            return View();
        }

        //Create new Company
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize()]
        public async Task<IActionResult> Company(SysCompanyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //var glAccount =  _unitOfWork.GLAccount.Get(model.GLAAccountID);

            await _unitOfWork.Company.AddSaveAsync(new SysCompany
            {
                CompanyID = model.CompanyID,
                CompanyName = model.CompanyName,
                CompanyAddress = model.CompanyAddress,
                //GLAAccount = glAccount,
                CreatedBy = user2.UserID,
                DateCreated = DateTime.Now
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Company has been created successfully";
            return View();
        }

        //Display view for editing Company
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateCompany(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                //ViewBag.GLAccountList = await GLAccountList();
                //var SysBank = UnitOfWork.SysBank.Get((int)id);
                var company = _unitOfWork.Company.Get((int)id);
                //var glAccount = await _unitOfWork.Company.GetCompanyDetail(id);
                var model = new SysCompanyViewModel
                {
                    CompanyID = company.CompanyID,
                    CompanyName = company.CompanyName,
                    CompanyAddress = company.CompanyAddress,
                    //GLAccountName = glAccount.GLAAccount.GLAccountCode,
                    DateCreated = company.DateCreated
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update Company
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateCompany(SysCompanyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            //var glAccount = _unitOfWork.GLAccount.Get(model.GLAAccountID);
            
            await _unitOfWork.Company.UpdateSaveAsync(new SysCompany
            {
                CompanyID = model.CompanyID,
                CompanyName = model.CompanyName,
                CompanyAddress = model.CompanyAddress,
                //GLAAccount = glAccount

            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Bank has been updated successfully";
            return View();
        }

        //Display all Companies
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> CompanyList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                //ViewBag.GLAccountList = await GLAccountList();
                List<DisplayCompanyViewModel> model = new List<DisplayCompanyViewModel>();
                var companies = _unitOfWork.Company.GetAll();
                //var Products = _unitOfWork.Products.GetAll();
                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    companies = companies.Where(Company => Company.CompanyName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var company in companies)
                {
                    model.Add(new DisplayCompanyViewModel
                    {
                        CompanyID = company.CompanyID,
                        CompanyName = company.CompanyName,
                        CompanyAddress = company.CompanyAddress,
                        //GLAccountName = company.GLAAccount.GLAccountCode,
                        DateCreated = company.DateCreated
                    });
                }
                var pager = new Pager(companies.Count(), pageIndex);

                var modelR = new HoldDisplayCompanyViewModel
                {
                    HoldAllCompanies = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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

        //Method to pull GLAccount
        //public async Task<List<EgsGLAccount>> GLAccountList()
        //{
        //    List<EgsGLAccount> GLAccountList = new List<EgsGLAccount>();

        //    GLAccountList = await _unitOfWork.GLAccount.GetAllGLAccounts();

        //    GLAccountList.Insert(0, new EgsGLAccount { GLAccountID = 0, GLAccountCode = "--Select GLAccount--" });

        //    return GLAccountList;
        //}
    }
}
#endregion