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
    public class SysTitleController : Controller
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

        public SysTitleController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            //_mailSender = emailSender;
            _roleManager = roleManager;
        }


        // GET: SysTitleController
        public ActionResult Index()
        {
            return View();
        }


        #region "Titles"
        //Display view for creating up Titles
        [HttpGet]
        public async Task<IActionResult> Title()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        //Create new Titles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Title(SysTitleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.Title.AddSaveAsync(new SysTitle
            {
                TitleID = model.TitleID,
                TitleName = model.TitleName,
                DateCreated = DateTime.Now,
                CreatedBy = user.UserID
            });

            //await unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Title has been created successfully";
            return View();
        }

        //Display view for editing Title
        [HttpGet]
        public async Task<IActionResult> UpdateTitle(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                var title = _unitOfWork.Title.Get((int)id);
                var model = new SysTitleViewModel
                {
                    TitleID = title.TitleID,
                    TitleName = title.TitleName
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update Title
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTitle(SysTitleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.Title.UpdateSaveAsync(new SysTitle
            {
                TitleID = model.TitleID,
                TitleName = model.TitleName
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Title has been updated successfully";
            return View();
        }


        //// GET: /Edit/5
        //public ActionResult Edit(int id)
        //{
        //    var eg = _unitOfWork.Title.Get(id);
        //    SysTitleViewModel ecg = new SysTitleViewModel()
        //    {
        //        TitleID = eg.TitleID,
        //        TitleName = eg.TitleName
        //    };

        //    return View(ecg);
        //}

        //// POST: Title/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(UpdateTitleViewModel editTitle)
        //{
        //    try
        //    {
        //        var user = await _userManager.GetUserAsync(User);
        //        var eg = _unitOfWork.Title.Get(editTitle.TitleID);
        //        eg.TitleName = editTitle.TitleName;
        //        // TODO: Add update logic here
        //        _unitOfWork.Title.Update(eg);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //Display all Titles
        [HttpGet]
        public async Task<IActionResult> Titles(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplayTitleViewModel> model = new List<DisplayTitleViewModel>();
                var titles = _unitOfWork.Title.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    titles = titles.Where(state => state.TitleName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var title in titles)
                {
                    model.Add(new DisplayTitleViewModel
                    {
                        TitleID = title.TitleID,
                        TitleName = title.TitleName
                    });
                }
                var pager = new Pager(titles.Count(), pageIndex);

                var modelR = new HoldDisplayTitleViewModel
                {
                    HoldAllTitles = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
