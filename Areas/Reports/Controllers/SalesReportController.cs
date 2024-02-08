using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    //[Authorize(Roles = "Agent")]
    [AllowAnonymous]
    public class SalesReportController : Controller
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

        public SalesReportController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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

        public IActionResult SalesReport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SalesReport(SalesReportModel reportModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("SalesReports", new { startDay = reportModel.StartDate.Day, startMonth = reportModel.StartDate.Month, startYear = reportModel.StartDate.Year, endDay = reportModel.EndDate.Day, endMonth = reportModel.EndDate.Month, endYear = reportModel.EndDate.Year });
            }
            return View();
        }

        public async Task<IActionResult> SalesReports(int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
        {
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

            List<SalesRViewModel> model = new List<SalesRViewModel>();
            var sales = await _unitOfWork.Sales.GetSalesByDateRange(startDate, endDate);
            double baseamount = 0;

            foreach (var sale in sales)
            {
                var getWalletInfo = await _unitOfWork.WalletTransaction.GetTransaction(Convert.ToInt32(sale.WalletTransactionID));
                //var getInvoiceInfo = await _unitOfWork.Invoice.GetInvoice(Convert.ToInt32());
                //var getProductInfo = await _unitOfWork.Products.GetProductExtension(sale);

                model.Add(new SalesRViewModel
                {
                    SalesID = sale.SalesID,
                    ReferenceNo = sale.TransactionReferenceNumber,
                    //ServiceNumber = sale.ServiceNumber,
                    Amount = sale.DiscountedAmount,
                    DateCreated = sale.DateCreated,
                    //ProductItemName = getProductItem.ProductItemName,
                    //ProductName = getProduct.Product.ProductName,
                    PaymentDate = sale.SettlementDate,
                    //InvoiceID = invoice.InvoiceID,
                    //PaymentStatus = sale.PaymentStatus

                });

                baseamount += sale.DiscountedAmount;
            }
            var pager = new Pager(sales.Count(), 1);
            var modelR = new HoldSalesRViewModel
            {
                HoldAllSales = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
