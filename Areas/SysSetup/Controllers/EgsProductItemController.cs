using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    [Authorize(Roles = "Super Admin, Merchant")]
    public class EgsProductItemController : Controller
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


        public EgsProductItemController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: EgsProductItemController
        public ActionResult Index()
        {
            return View();
        }


        #region "ProductItems"

        //Display view for creating up ProductItems
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ProductItem()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            ViewBag.ProductList = await ProductList();
            return View();
        }

        //Create new ProductItems
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ProductItem(EgsProductItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var product = await _unitOfWork.Products.GetProducts(model.ProductID);

            await _unitOfWork.ProductItem.AddSaveAsync(new EgsProductItem
            {
                ProductItemID = model.ProductItemID,
                ProductItemName = model.ProductItemName,
                Product = product,
                CreatedBy = user2.UserID,
                DateCreated = DateTime.Now
            }) ;

            var settlementExist = await _unitOfWork.SettlementBasis.GetSettlementBasisByProductItemID(model.ProductID, product.Merchant.MerchantID, model.ProductItemID);
            
            if(settlementExist == null)
            {
                await _unitOfWork.SettlementBasis.AddSaveAsync(new EgsSettlementBasis
                {
                    ProductItemID = model.ProductItemID,
                    ProductID = model.ProductID,
                    MerchantID = product.Merchant.MerchantID,
                    SettlementTypeID = 1,//Fixed settlementType,
                    SettlementIntervalID = 1,//Daily Interval Type,
                    SplittingRate = 10,//Default of #10 / 10 naira
                    CreatedBy = user2.UserID,
                    DateCreated = DateTime.Now
                });

            }

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Product Item has been created successfully";
            ViewBag.ProductList = await ProductList();
            return View();
        }

        //Display view for editing ProductItem
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateProductItem(int? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;

                ViewBag.ProductList = await ProductList(); //this allow the selection of the dropdown value
                var ProductItem = await _unitOfWork.ProductItem.GetProductItemsByProdId((int)id);

                var model = new EgsProductItemViewModel
                {
                    ProductItemID = ProductItem.ProductItemID,
                    ProductItemName = ProductItem.ProductItemName,
                    ProductID = ProductItem.Product.ProductID
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update ProductItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateProductItem(EgsProductItemViewModel model)
        {
            ViewBag.ProductList = await ProductList();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var product = await _unitOfWork.Products.GetProducts(model.ProductID);

            await _unitOfWork.ProductItem.UpdateSaveAsync(new EgsProductItem
            {
                ProductItemID = model.ProductItemID,
                ProductItemName = model.ProductItemName,
                Product = product
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Product Item has been updated successfully";
            ViewBag.ProductList = await ProductList();
            return View();
        }

        ////Delete ProductItem
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////[Authorize(Roles = "Super Admin")]
        //public IActionResult DeleteProductItem(int id)
        //{
        //    _unitOfWork.ProductItem.Remove(new EgsProductItem
        //    {
        //        ProductItemID = id
        //    });

        //    ModelState.Clear();
        //    ViewBag.Message = "Product Item has been deleted successfully";
        //    return View();
        //}


        // GET: Instructors/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prdItem = _unitOfWork.ProductItem.Get((int)id);

            if (prdItem == null)
            {
                return NotFound();
            }

            return View(prdItem);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var instructor = await _context.Instructors.FindAsync(id);
            //_context.Instructors.Remove(instructor);
            //await _context.SaveChangesAsync();

            _unitOfWork.ProductItem.Remove(new EgsProductItem
            {
                ProductItemID = id
            });
            
            //ModelState.Clear();
            _unitOfWork.Complete();
            ViewBag.Message = "Product Item has been deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        //Display all ProductItems
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ProductItemList(int? pageIndex, string searchText)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;
                List<DisplayProductItem> model = new List<DisplayProductItem>();
                var ProductItems = await _unitOfWork.ProductItem.GetAllProductItems();
                //var Products = _unitOfWork.Products.GetAll();
                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    ProductItems = ProductItems.Where(ProductItem => ProductItem.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var ProductItem in ProductItems)
                {
                    model.Add(new DisplayProductItem
                    {
                        ProductItemID = ProductItem.ProductItemID,
                        ProductItemName = ProductItem.ProductItemName,
                        ProductName = ProductItem.Product.ProductName,
                        DateCreated = ProductItem.DateCreated
                    });
                }
                var pager = new Pager(ProductItems.Count(), pageIndex);

                var modelR = new HoldDisplayProductItems
                {
                    HoldAllProductItems = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
        public async Task<List<EgsProduct>> ProductList()
        { 
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            var Baseurl = APIServiceConfig?.Url;

            List<EgsProduct> productsList = new List<EgsProduct>();

            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(Baseurl + "/api/Product/", "", "GET");

                string responseToCaller = string.Empty;

                if (!String.IsNullOrEmpty(IResponse))
                {
                    productsList = JsonConvert.DeserializeObject<List<EgsProduct>>(IResponse);
                    responseToCaller = "sent";
                    productsList.Insert(0, new EgsProduct { ProductID = 0, ProductName = "--Select Product --" });

                }
                else
                {
                    responseToCaller = "not sent";
                    productsList.Insert(0, new EgsProduct { ProductID = 0, ProductName = "--N/A Product --" });
                }

            }

            //List<EgsProduct> productsList = new List<EgsProduct>();

            //productsList = await _unitOfWork.Products.GetAllProducts();


            //productsList.Insert(0, new EgsProduct { ProductID = 0, ProductName = "--Select Product--" });

            return productsList;
        }

        //Method to pull ProductCategories
        public async Task<List<EgsProductCategory>> ProductCategoryList()
        {
             

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            var Baseurl = APIServiceConfig?.Url;


            List<EgsProductCategory> prodCategoryList = new List<EgsProductCategory>();

            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(Baseurl + "/api/ProductCategory/", "", "GET");

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
                    prodCategoryList.Insert(0, new EgsProductCategory { ProductCategoryID = 0, ProductCategoryName = "--Select Product Category --" });
                }

            }

            return prodCategoryList;
        }

        #endregion

    }
}
