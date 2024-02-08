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
   // [Authorize(Roles = "Super Admin")]
    public class SysLgaController : Controller
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

        public SysLgaController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: SysLgaController
        public ActionResult Index()
        {
            return View();
        }


        #region "Lga"
        //Display view for creating up LGA
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Lga()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            ViewBag.StateList = await StateList();
            return View();
        }


        //Create new Lga
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Lga(SysLgaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.LGA.AddSaveAsync(new SysLga
            {
                LgaID = model.LgaID,
                LgaName = model.LgaName,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "LGA has been created successfully";
            ViewBag.StateList = await StateList();
            return View();
        }

        //Display view for editing Lga
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public  async Task<IActionResult>UpdateLga(int? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;
                ViewBag.StateList = await StateList();
                //var LGA = UnitOfWork.LGA.Get((int)id);
                var LGA = _unitOfWork.LGA.Get((int)id);
                var model = new SysLgaViewModel
                {
                    LgaID = LGA.LgaID,
                    LgaName = LGA.LgaName
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update LGA
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateLga(SysLgaViewModel model)
        {
            ViewBag.StateList = await StateList();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.LGA.UpdateSaveAsync(new SysLga
            {
                LgaID = model.LgaID,
                LgaName = model.LgaName
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.StateList = await StateList();
            ViewBag.Message = "LGA has been updated successfully";
            return View();
        }

        //Display all LGAS
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Lgas(int? pageIndex, string searchText)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;
                List<DisplayLgaViewModel> model = new List<DisplayLgaViewModel>();
                var LGA = _unitOfWork.LGA.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    LGA = LGA.Where(Lga => Lga.LgaName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var lga in LGA)
                {
                    model.Add(new DisplayLgaViewModel
                    {
                        LgaID = lga.LgaID,
                        LgaName = lga.LgaName
                    });
                }
                var pager = new Pager(LGA.Count(), pageIndex);

                var modelR = new HoldDisplayLgaViewModel
                {
                    HoldAllLga = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
        //Method to pull Products 
        public async Task<List<SysState>> StateList()
        {
            List<SysState> StateList = new List<SysState>();

            StateList = await _unitOfWork.State.GetAllStates();

            StateList.Insert(0, new SysState { StateID = 0, StateName = "--Select State--" });

            return StateList;
        }

    }
}
#endregion