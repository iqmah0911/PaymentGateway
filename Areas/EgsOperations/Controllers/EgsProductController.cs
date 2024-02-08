using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Controllers
{

    [Area("EgsOperations")]
    [Authorize(Roles = "Super Admin, Merchant, General")]
    public class EgsProductController : Controller
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


        public EgsProductController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: EgsProductController
        public ActionResult Index()
        {
            return View();
        }


        #region "Products"

        //Display view for creating up Products
        [HttpGet] 
        public async Task<IActionResult> Product()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString()); 
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            ViewBag.ProductCategory = await ProductCategoryList();
            ViewBag.InvoiceMode = InvoiceModeList();
            ViewBag.Merchant = MerchantList();
            return View();
        }

        //Create new Products
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Product(EgsProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var productCategory = await _unitOfWork.ProductCategory.GetProductCategory(model.ProductCategoryID);

            var getUserMerchant = await _unitOfWork.User.GetMerchantSysUsers(model.UserID);

            var merchantDet = _unitOfWork.Merchant.Get(getUserMerchant.Merchant.MerchantID);

            await _unitOfWork.Products.AddSaveAsync(new EgsProduct
            {
                ProductID = model.ProductID,
                ProductCode = model.ProductCode,
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                Notification = model.Notification,
                Image = model.Image,
                Verification = model.Verification,
                IsFixedAmount = model.IsFixedAmount,
                IsActive = model.IsActive,
                ProductCategories = productCategory,
                CreatedBy = user2.UserID,
                DateCreated = DateTime.Now,
                InvoiceModeID = model.InvoiceModeID,
                Merchant = merchantDet
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Product  has been created successfully";
            ViewBag.ProductCategory = await ProductCategoryList();
            ViewBag.InvoiceMode = InvoiceModeList();
            ViewBag.Merchant = MerchantList();
            return View();
        }

        //Display view for editing Product
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateProduct(int? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;
                ViewBag.ProductCategory = await ProductCategoryList();

                var Product = await _unitOfWork.Products.GetProductCategoryById((int)id);

                var model = new EgsProductViewModel
                {
                    ProductID = Product.ProductID,
                    ProductName = Product.ProductName,
                    ProductDescription = Product.ProductDescription,
                    Image = Product.Image,
                    Notification = Product.Notification,
                    Verification = Product.Verification,
                    IsFixedAmount = Product.IsFixedAmount,
                    ProductCategoryID = Product.ProductCategories.ProductCategoryID,
                    DateCreated = Product.DateCreated
                };
                ViewBag.ProductCategory = await ProductCategoryList();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateProduct(EgsProductViewModel model)
        {
            ViewBag.ProductCategory = await ProductCategoryList();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var productCategory = await _unitOfWork.ProductCategory.GetProductCategory(model.ProductCategoryID);

            await _unitOfWork.Products.UpdateSaveAsync(new EgsProduct
            {
                ProductID = model.ProductID,
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                Image = model.Image,
                Notification = model.Notification,
                Verification = model.Verification,
                IsFixedAmount = model.IsFixedAmount,
                ProductCategories = productCategory
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Product  has been updated successfully";
            ViewBag.ProductCategory = await ProductCategoryList();
            return View();
        }

        //Display all Products
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ProductList(int? pageIndex, string searchText)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;

                List<DisplayProduct> model = new List<DisplayProduct>();
                var Products = await _unitOfWork.Products.GetProductList();
                //var Products = _unitOfWork.Products.GetAll();
                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    Products = Products.Where(Product => Product.ProductName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var Product in Products)
                {
                    model.Add(new DisplayProduct
                    {
                        ProductID = Product.ProductID,
                        ProductName = Product.ProductName,
                        ProductDescription = Product.ProductDescription,
                        Image = Product.Image,
                        Notification = Product.Notification,
                        Verification = Product.Verification,
                        IsFixedAmount = Product.IsFixedAmount,
                        ProductCategory = Product.ProductCategories.ProductCategoryName,
                        DateCreated = Product.DateCreated
                    });
                }
                var pager = new Pager(Products.Count(), pageIndex);

                var modelR = new HoldDisplayProduct
                {
                    HoldAllProduct = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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

        //Method to pull Products
        public async Task<List<EgsProductCategory>> ProductCategoryList()
        {
             
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            var Baseurl = APIServiceConfig?.Url;

            List<EgsProductCategory> prodCategoryList = new List<EgsProductCategory>();

            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(Baseurl + "/api/ProductCategory", "", "GET");

                string responseToCaller = string.Empty;

                if (!String.IsNullOrEmpty(IResponse))
                {
                    prodCategoryList = JsonConvert.DeserializeObject<List<EgsProductCategory>>(IResponse);
                    //responseToCaller = "sent";
                    prodCategoryList.Insert(0, new EgsProductCategory { ProductCategoryID = 0, ProductCategoryName = "--Select Product Category --" });
                }
                else
                {
                    //responseToCaller = "not sent";
                    prodCategoryList.Insert(0, new EgsProductCategory { ProductCategoryID = 0, ProductCategoryName = "--N/A Product Category --" });
                }

            }

            return prodCategoryList;
        }

        public List<EgsInvoiceMode> InvoiceModeList()
        {
            List<EgsInvoiceMode> invoiceModeList = new List<EgsInvoiceMode>();

            invoiceModeList = _unitOfWork.InvoiceMode.GetAllForDropdown();

            invoiceModeList.Insert(0, new EgsInvoiceMode { InvoiceModeID = 0, InvoiceModeType = "--Select State--" });

            return invoiceModeList;
        }

        public List<SysUsers> MerchantList()
        {
            List<SysUsers> merchantList = new List<SysUsers>();

            merchantList = _unitOfWork.Merchant.GetAllMerchants();

            merchantList.Insert(0, new SysUsers { UserID = 0, CompanyName = "--Select Merchant--" });

            return merchantList;
        }
        #endregion


    }

}