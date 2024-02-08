using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;





namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    [Authorize(Roles = "Super Admin, Merchant")]
    public class EgsProductCategoryController : Controller
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

        public EgsProductCategoryController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: EgsProductCategory
        public ActionResult Index()
        {
            return View();
        }


        #region "ProductCategories"

        //Display view for creating up ProductCategories
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ProductCategory()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        //Create new ProductCategories
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ProductCategory(EgsProductCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.ProductCategory.AddSaveAsync(new EgsProductCategory
            {
                ProductCategoryID = model.ProductCategoryID,
                ProductCategoryName = model.ProductCategoryName,
                Image = model.Image,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Product Category has been created successfully";
            return View();
        }

        //Display view for editing ProductCategory
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateProductCategory(int? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;

                var ProductCategory = _unitOfWork.ProductCategory.Get((int)id);
                var model = new EgsProductCategoryViewModel
                {
                    ProductCategoryID = ProductCategory.ProductCategoryID,
                    ProductCategoryName = ProductCategory.ProductCategoryName,
                    Image = ProductCategory.Image
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update ProductCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateProductCategory(EgsProductCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            await _unitOfWork.ProductCategory.UpdateSaveAsync(new EgsProductCategory
            {
                ProductCategoryID = model.ProductCategoryID,
                ProductCategoryName = model.ProductCategoryName,
                Image = model.Image
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Product Category has been updated successfully";
            return View();
        }

        //Display all ProductCategories
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ProductCategoriesList(int? pageIndex, string searchText)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;
                List<DisplayProductCategoryViewModel> model = new List<DisplayProductCategoryViewModel>();
                var ProductCategories = _unitOfWork.ProductCategory.GetAll();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    ProductCategories = ProductCategories.Where(ProductCategory => ProductCategory.ProductCategoryName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var ProductCategory in ProductCategories)
                {
                    model.Add(new DisplayProductCategoryViewModel
                    {
                        ProductCategoryID = ProductCategory.ProductCategoryID,
                        ProductCategoryName = ProductCategory.ProductCategoryName,
                        Image = ProductCategory.Image
                    });
                }
                var pager = new Pager(ProductCategories.Count(), pageIndex);

                var modelR = new HoldDisplayProductCategoryViewModel
                {
                    HoldAllProductCategories = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
