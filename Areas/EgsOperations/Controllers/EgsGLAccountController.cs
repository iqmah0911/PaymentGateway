using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Controllers
{
    [Area("EgsOperations")]
    [Authorize(Roles = "Super Admin")]
    public class EgsGLAccountController : Controller
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

        public EgsGLAccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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

        #region "GLAccounts"

        //Display view for creating up GLAccounts
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GLAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            ViewBag.CompanyList = await CompanyList();
            ViewBag.BankList = await BankList();
            return View();
        }

        //Create new GLAccounts
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GLAccount(EgsGLAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var company = _unitOfWork.Company.Get(model.CompanyID);
            var bank = _unitOfWork.Bank.Get(model.BankID);

            await _unitOfWork.GLAccount.AddSaveAsync(new EgsGLAccount
            {
                GLAccountID = model.GLAccountID,
                GLAccountCode = model.GLAccountCode,
                Company = company,
                Bank = bank,
                BankAccount =model.BankAccount,
                CreatedBy = user2.UserID,
                DateCreated = DateTime.Now
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "GLAccount has been created successfully";
            ViewBag.CompanyList = await CompanyList();
            ViewBag.BankList = await BankList();
            return View();
        }

        //Display view for editing GLAccount
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateGLAccount(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                ViewBag.CompanyList = await CompanyList();
                ViewBag.BankList = await BankList();
                var glAccount = await _unitOfWork.GLAccount.GetGLAccountByCompanyID((int)id);
                var bank = await _unitOfWork.Bank.GetBanks((int)id);

                var model = new EgsGLAccountViewModel
                {
                    GLAccountID = glAccount.GLAccountID,
                    GLAccountCode = glAccount.GLAccountCode,
                    Company = glAccount.Company.CompanyName,
                    Bank = bank.BankName,
                    BankAccount = glAccount.BankAccount,
                    DateCreated = glAccount.DateCreated
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update GLAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateGLAccount(EgsGLAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var company = await _unitOfWork.Company.GetCompanyDetail(model.CompanyID);
            var bank = await _unitOfWork.Bank.GetBanks(model.BankID);

            await _unitOfWork.GLAccount.UpdateSaveAsync(new EgsGLAccount
            {
                GLAccountID = model.GLAccountID,
                GLAccountCode = model.GLAccountCode,
                Company = company,
                Bank = bank,
                BankAccount = model.BankAccount
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "GLAccount has been updated successfully";
            ViewBag.CompanyList = await CompanyList();
            ViewBag.BankList = await BankList();
            return View();
        }

        //Display all GLAccounts
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GLAccountList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                ViewBag.CompanyList = await CompanyList();
                ViewBag.BankList = await BankList();
                List<DisplayGLAccountViewModel> model = new List<DisplayGLAccountViewModel>();
                var glAccounts = await _unitOfWork.GLAccount.GetAllGLAccounts();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    glAccounts = glAccounts.Where(glAccount => glAccount.Company.CompanyName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    glAccounts = glAccounts.Where(glAccount => glAccount.Bank.BankName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var glAccount in glAccounts)
                {
                    model.Add(new DisplayGLAccountViewModel
                    {
                        GLAccountID = glAccount.GLAccountID,
                        GLAccountCode = glAccount.GLAccountCode,
                        Company = glAccount.Company.CompanyName,
                        Bank = glAccount.Bank.BankName,
                        DateCreated = glAccount.DateCreated
                    });
                }
                var pager = new Pager(glAccounts.Count(), pageIndex);

                var modelR = new HoldDisplayGLAccountViewModel
                {
                    HoldAllGLAccounts = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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

        //Method to pull Company 
        public async Task<List<SysCompany>> CompanyList()
        {
            List<SysCompany> companyList = new List<SysCompany>();

            companyList = await _unitOfWork.Company.GetCompanys();

            companyList.Insert(0, new SysCompany { CompanyID = 0, CompanyName = "--Select Company--" });

            return companyList;
        }

        //Method to pull Bank 
        public async Task<List<SysBank>> BankList()
        {
            List<SysBank> bankList = new List<SysBank>();

            bankList = await _unitOfWork.Bank.GetAllBanks();

            bankList.Insert(0, new SysBank { BankID = 0, BankName = "--Select Bank--" });

            return bankList;
        }
    }
}
#endregion