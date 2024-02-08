using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.Invoices.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Invoices.Controllers
{
    [Area("Invoices")]
    //[Authorize(Roles = "Super Admin")]
    public class UnpaidInvoicesController : Controller
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

        public UnpaidInvoicesController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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

        // GET: UnpaidInvoicesController
        public ActionResult Index()
        {
            return View();
        }


        #region "UnpaidInvoices"
        // GET: UnpaidInvoicesController/Details/5
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<ActionResult> InvoiceList(int? pageIndex, string searchText)
        { 
            try
            {

                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                //getting image for user layout
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;

                List<InvoicesViewModel> model = new List<InvoicesViewModel>();
                var getInvoices = await _unitOfWork.Invoice.GetUnPaidInvoice(user2.UserID);

                ////Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    getInvoices = getInvoices.Where(u => u.ReferenceNo.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var getInvoice in getInvoices)
                {
                    model.Add(new InvoicesViewModel
                    {
                        ReferenceNo = getInvoice.ReferenceNo,
                        ServiceNumber = getInvoice.ServiceNumber,
                        Amount = getInvoice.Amount,
                        DateCreated = getInvoice.DateCreated
                    });
                }

                var pager = new Pager(getInvoices.Count(), pageIndex);
                var modelR = new HoldInvoicesViewModel
                {
                    HoldAllInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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

        #endregion

    }
}
