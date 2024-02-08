 
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
    public class SysAggregatorController : Controller
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

        public SysAggregatorController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: SysAggregatorControler
        public ActionResult Index()
        {
            return View();
        }


        #region "SysAggregator"
        //Display view for creating up SysAggregator
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Aggregator()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }


        //Create new SysAggregator
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Aggregator(SysAggregatorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.Aggregator.AddSaveAsync(new SysAggregator
            {
                AggregatorID = model.AggregatorID,
                AggregatorName = model.AggregatorName,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "SysAggregator has been created successfully";
            return View();
        }

        //Display view for editing SysAggregator
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateSysAggregator(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                //var SysAggregator = UnitOfWork.SysAggregator.Get((int)id);
                var SysAggregator = _unitOfWork.Aggregator.Get((int)id);
                var model = new SysAggregatorViewModel
                {
                    AggregatorID = SysAggregator.AggregatorID,
                    AggregatorName = SysAggregator.AggregatorName
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update SysAggregator
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateAggregator(SysAggregatorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.Aggregator.UpdateSaveAsync(new SysAggregator
            {
                AggregatorName = model.AggregatorName,
                DateCreated = model.DateCreated
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "SysAggregator has been updated successfully";
            return View();
        }

        //Display all SysAggregator
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> SysAggregators(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplaySysAggregatorViewModel> model = new List<DisplaySysAggregatorViewModel>();
                var SysAggregator = _unitOfWork.Aggregator.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    SysAggregator = SysAggregator.Where(Aggregator => Aggregator.AggregatorName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var Aggregator in SysAggregator)
                {
                    model.Add(new DisplaySysAggregatorViewModel
                    {
                        AggregatorID = Aggregator.AggregatorID,
                        AggregatorName = Aggregator.AggregatorName
                    });
                }
                var pager = new Pager(SysAggregator.Count(), pageIndex);

                var modelR = new HoldDisplaySysAggregatorViewModel
                {
                    HoldAllAggregator = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
    }
}
#endregion