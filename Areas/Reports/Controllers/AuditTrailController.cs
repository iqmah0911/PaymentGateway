using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using PaymentGateway21052021.Areas.Reports.Models;

namespace PaymentGateway21052021.Areas.Reports.Controllers
{
    [Area("Reports")] 
    [AllowAnonymous]
    public class AuditTrailController : Controller
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

        public AuditTrailController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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

        [HttpGet]
        public async Task<IActionResult> AuditReport()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            ViewBag.role = user2.Role.RoleID;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AuditTrailReport(int? pageIndex, AuditTrailViewModel reportModel)
        {
            int startDay = reportModel.StartDate.Day;
            int startMonth = reportModel.StartDate.Month;
            int startYear = reportModel.StartDate.Year;
            int endDay = reportModel.EndDate.Day;
            int endMonth = reportModel.EndDate.Month;
            int endYear = reportModel.EndDate.Year;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var roleName = user2.Role.RoleName;

            //Construct new Date from parameters
            DateTime startDate = new DateTime(startYear, startMonth, startDay);
            DateTime endDate = new DateTime(endYear, endMonth, endDay);

            //ViewBags for StartDate
            ViewBag.OfferingDayStart = startDate.Day;
            ViewBag.OfferingMonthStart = General.GetMonthInWord(startDate.Month);
            ViewBag.OfferingYearStart = startDate.Year;

            //ViewBags for EndDate
            ViewBag.OfferingDayEnd = endDate.Day;
            ViewBag.OfferingMonthEnd = General.GetMonthInWord(endDate.Month);
            ViewBag.OfferingYearEnd = endDate.Year;

            List<AuditTrailViewModel> model = new List<AuditTrailViewModel>();
            
            //Audit Trail 
            var audit_ = new EgsAuditTrail
            {
                DbAction = "Access",
                DateCreated = DateTime.Now,
                Page = "AuditTrail Report",
                Description = user2.FirstName + " " + user2.LastName + " accessed audit trail report into the application at " + DateTime.Now,
                IPAddress = Helpers.General.GetIPAddress(),
                CreatedBy = user2.UserID,
                Menu = "Audit",
                Role = user2.Role.RoleName
            };

            await _unitOfWork.AuditTrail.AddAsync(audit_);
            _unitOfWork.Complete();


            var auditTrails = await GeneralSqlClient.RPT_AuditTrail(startDate, endDate, "", "AUDITTRAIL", user2.UserID.ToString());

            foreach (var aTrail in auditTrails)
            {
                model.Add(new AuditTrailViewModel
                {
                    Role = aTrail.Role,
                    Menu = aTrail.Menu,
                    Page = aTrail.Page,
                    DbAction = aTrail.DbAction,
                    Description = aTrail.Description,
                    IPAddress = aTrail.IPAddress,
                    DateCreated = aTrail.DateCreated
                });

            }
            var modelR = new HoldDisplayAuditTrail
            {
                HoldRPTAuditTrail = model
            };
            return View(modelR);

        }

      


    }
}
