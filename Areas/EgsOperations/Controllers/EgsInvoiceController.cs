using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Controllers
{
    [Area("EgsOperations")]
    [Authorize(Roles = "Agent, Aggregator")]
    public class EgsInvoiceController : Controller
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

        public EgsInvoiceController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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
    


        // GET: EgsWalletControler
        //public ActionResult Index()
        //{
        //    return View();
        //}


//        #region "EgsInvoice"
//        [HttpGet]
//        //[Authorize()]
//        public IActionResult Invoice()
//        {
//            return View();
//        }


//        //Create new EgsInvoice
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        //[Authorize()]
//        public async Task<IActionResult> Invoice(InvoiceViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                ModelState.AddModelError("", "Please fill in the required fields.");
//                return View();
//            }

//            var user = await _userManager.GetUserAsync(User);
//            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

//            await _unitOfWork.Invoice.AddSaveAsync(new EgsInvoice
//            {
//                InvoiceID = model.InvoiceID, 
//                ReferenceNo = model.ReferenceNo,
//                Amount = model.Amount, 
//                PaymentDate = model.PaymentDate, 
//                PaymentStatus = model.PaymentStatus,
//                //Bank = model.Bank.B,
//                //WalletID = model.WalletID,
//                //WalletAccountNumber = model.WalletAccountNumber,
//                ////User = user2.,
//                DateCreated = DateTime.Now
//            });

//            //await _unitOfWork.CompleteAsync();
//            ModelState.Clear();
//            ViewBag.Message = "Invoice has been created successfully";
//            return View();
//        }

//        [HttpGet]
//        //[Authorize(Roles = "Super Admin")]
//        public IActionResult UpdateInvoice(int? id)
//        {
//            try
//            {
//                var invoice = _unitOfWork.Invoice.Get((int)id);
//                var model = new UpdateInvoiceViewModel
//                {
//                    //WalletID = wallet.WalletID,
//                    //WalletAccountNumber = wallet.WalletAccountNumber
//                };
//                return View(model);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogInformation(ex.Message + ex.StackTrace);
//            }
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        //[Authorize(Roles = "Super Admin")]
//        public async Task<IActionResult> UpdateInvoice(UpdateInvoiceViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                ModelState.AddModelError("", "Please fill in the required fields.");
//                return View();
//            }

//            await _unitOfWork.Invoice.UpdateSaveAsync(new EgsInvoice
//            {
//                //WalletID = model.WalletID,
//                //WalletAccountNumber = model.WalletAccountNumber,
//            });

//            //_unitOfWork.Complete();
//            ModelState.Clear();
//            ViewBag.Message = "Wallet has been updated successfully";
//            return View();
//        }

//        [HttpGet]
//        //[Authorize(Roles = "Super Admin")]
//        public IActionResult Invoices(int? pageIndex, string searchText)
//        {
//            try
//            {
//                List<DisplayInvoiceViewModel> model = new List<DisplayInvoiceViewModel>();
//                var invoices = _unitOfWork.Invoice.GetAll();

//                //Logic for search
//                if (!String.IsNullOrEmpty(searchText))
//                {
//                    invoices = invoices.Where(Invoice => Invoice.ReferenceNo.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
//                }

//                //load up the list of viewmodels 
//                foreach (var invoice in invoices)
//                {
//                    model.Add(new DisplayInvoiceViewModel
//                    {
//                        //WalletID = wallet.WalletID,
//                        //WalletAccountNumber = wallet.WalletAccountNumber,
//                        //DateCreated = wallet.DateCreated
//                    });
//                }
//                var pager = new Pager(invoices.Count(), pageIndex);

//                var modelR = new HoldDisplayInvoiceViewModel
//                {
//                    HoldAllInvoice = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
//                    Pager = pager
//                };

//                return View(modelR);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogInformation(ex.Message + ex.StackTrace);
//            }
//            {
//                return View();
//            }
//        }
//    }
//}
//#endregion

