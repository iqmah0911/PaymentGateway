using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.Reports.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Reports.Controllers
{
   
        [Area("Reports")]
        //[Authorize(Roles = "Merchant")]
        [AllowAnonymous]
        public class MerchantsController : Controller
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

            public MerchantsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                ILogger<dynamic> logger, IUnitOfWork unitOfWork,
                RoleManager<ApplicationRole> roleManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _unitOfWork = unitOfWork;
                _logger = logger;
                //_mailSender = emailSender;
                _roleManager = roleManager;
            }



            public IActionResult MerchantReport()
            {
                return View();
            }

            [HttpPost]
            public IActionResult MerchantReport(MerchantReportModel reportModel)
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("MerchantReports", new { startDay = reportModel.StartDate.Day, startMonth = reportModel.StartDate.Month, startYear = reportModel.StartDate.Year, endDay = reportModel.EndDate.Day, endMonth = reportModel.EndDate.Month, endYear = reportModel.EndDate.Year });
                }
                return View();
            }


            public async Task<IActionResult> MerchantReports(int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var roleName = user2.Role.RoleName;

                //Construct new Date from parameters
                DateTime startDate = new DateTime(startYear, startMonth, startDay);
                DateTime endDate = new DateTime(endYear, endMonth, endDay);

                //ViewBags for StartDate
                ViewBag.OfferingDayStart = startDate.Day;
                ViewBag.OfferingMonthStart = GetMonthInWord(startDate.Month);
                ViewBag.OfferingYearStart = startDate.Year;

                //ViewBags for EndDate
                ViewBag.OfferingDayEnd = endDate.Day;
                ViewBag.OfferingMonthEnd = GetMonthInWord(endDate.Month);
                ViewBag.OfferingYearEnd = endDate.Year;

                List<MerchantRReportViewModel> model = new List<MerchantRReportViewModel>();
                var invoices = await _unitOfWork.MerchantReport.GetInvoicesByDateRange(startDate, endDate);
                double baseamount = 0;

                foreach (var invoice in invoices)
                {
                    var getProductItem = await _unitOfWork.ProductItem.GetProductItems(invoice.ProductItemID);
                    var getProduct = await _unitOfWork.Products.GetProductExtension(invoice.ProductItemID);
                    model.Add(new MerchantRReportViewModel
                    {
                        ReferenceNo = invoice.ReferenceNo,
                        ServiceNumber = invoice.ServiceNumber,
                        Amount = invoice.Amount,
                        DateCreated = invoice.DateCreated,
                        ProductItemName = getProductItem.ProductItemName,
                        ProductName = getProduct.Product.ProductName,
                        PaymentDate = invoice.PaymentDate,
                        InvoiceID = invoice.InvoiceID,
                        PaymentStatus = invoice.PaymentStatus

                    });

                    baseamount += invoice.Amount;
                }
                var pager = new Pager(invoices.Count(), 1);
                var modelR = new HoldInvoicesViewModel
                {
                    HoldInvoices = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                ViewBag.baseamount = baseamount;
                return View(modelR);

            }






            public static string GetMonthInWord(int month)
            {
                var monthNames = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                int getMonth = month;

                return $"{monthNames[getMonth - 1] }";
            }

        }
    }

