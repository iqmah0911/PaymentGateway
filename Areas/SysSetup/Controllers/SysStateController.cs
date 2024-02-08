using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    public class SysStateController : Controller
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
        //private readonly IEmailSender _mailSender;
        #endregion

        public SysStateController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: SysStateController
        public ActionResult Index()
        {
            return View();
        }


        #region "States"
        //Display view for creating up States
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> State()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        //Create new States
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> State(CreateStateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.State.AddSaveAsync(new SysState
            {
                StateID = model.StateID,
                StateName = model.StateName,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "State has been created successfully";
            return View();
        }

        //Display view for editing State
        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateState(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                var state = _unitOfWork.State.Get((int)id);
                var model = new UpdateStateViewModel
                {
                    StateID = state.StateID,
                    StateName = state.StateName
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update State
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateState(UpdateStateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.State.UpdateSaveAsync(new SysState
            {
                StateID = model.StateID,
                StateName = model.StateName
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "State has been updated successfully";
            return View();
        }

        //Display all States
        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> States(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplayStateViewModel> model = new List<DisplayStateViewModel>();
                var states = _unitOfWork.State.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    states = states.Where(state => state.StateName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var state in states)
                {
                    model.Add(new DisplayStateViewModel
                    {
                        StateID = state.StateID,
                        StateName = state.StateName
                    });
                }
                var pager = new Pager(states.Count(), pageIndex);

                var modelR = new HoldDisplayStateViewModel
                {
                    HoldAllStates = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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

        #endregion

    }
}
