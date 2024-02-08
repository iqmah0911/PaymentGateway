using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nancy.Json;
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
    public class EgsProductItemRateController : Controller
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


        public EgsProductItemRateController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: EgsProductItemRateController
        public ActionResult Index()
        {
            return View();
        }


        #region "ProductItemRates"

        //Display view for creating up ProductItemRates
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ProductItemRate()
        {
            //var prodList = await _unitOfWork.Products.GetProductList(); 
            //var prodItemList = await _unitOfWork.ProductItem.GetAllProductItems();
            //var itemList = await _unitOfWork.ProductItem.GetItemsByProductID(35);
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            ViewBag.ProductList = await ProductList();
            ViewBag.ProductItemList = await ProductItemList();
            return View();
        }

        //Create new ProductItemRates
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ProductItemRate(EgsProductItemRateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            //var prod = await _unitOfWork.Products.GetProducts(model.ProductID);
            //var productItem_ = await _unitOfWork.ProductItem.GetProductItems(model.ProductItemID);

            //var productItem_ = await _unitOfWork.ProductItem.GetProductItem(model.ProductItemID);

            var productItem = _unitOfWork.ProductItem.Get(model.ProductItemID);

            await _unitOfWork.ProductItemRate.AddSaveAsync(new EgsProductItemRate
            {
                //ProductItemRateID = model.ProductItemRateID,
                ProductItem = productItem,
                AmountRate = model.AmountRate,
                DateCreated = DateTime.Now,
                IsActive = true,
                CreatedBy = user2
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Product Item Rate has been created successfully";
            ViewBag.ProductList = await ProductList();
            ViewBag.ProductItemList = await ProductItemList();
            return View();
        }

        //Display view for editing ProductItemRate
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateProductItemRate(int? id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;

                ViewBag.ProductList = await ProductList();
                ViewBag.ProductItemList = await ProductItemList();
                var ProductItemRate = await _unitOfWork.ProductItemRate.GetProductItemsByProdItemId((int)id);

                var model = new EgsProductItemRateViewModel
                {
                    ProductItemRateID = ProductItemRate.ProductItemRateID,
                    ProductItemID = ProductItemRate.ProductItem.ProductItemID,
                    AmountRate = ProductItemRate.AmountRate,
                    DateCreated = ProductItemRate.DateCreated
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        //Update ProductItemRate
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateProductItemRate(EgsProductItemRateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var productItem_ = await _unitOfWork.ProductItem.GetProductItems(model.ProductItemID);

            await _unitOfWork.ProductItemRate.UpdateSaveAsync(new EgsProductItemRate
            {
                ProductItemRateID = model.ProductItemRateID,
                ProductItem = productItem_,
                AmountRate = model.AmountRate,
                DateCreated = model.DateCreated
            });

            //_unitOfWork.Complete();
            ModelState.Clear();
            ViewBag.Message = "Product Item Rate has been updated successfully";
            ViewBag.ProductList = await ProductList();
            ViewBag.ProductItemList = await ProductItemList();
            return View();
        }

        //Display all ProductItemRates
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ProductItemRates(int? pageIndex, string searchText)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
                ViewBag.pic = profiles.Image;
                List<DisplayProductItemRate> model = new List<DisplayProductItemRate>();
                var ProductItemRates = await _unitOfWork.ProductItemRate.GetAllProductItemRates();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    ProductItemRates = ProductItemRates.Where(ProductItemRate => ProductItemRate.ProductItem.ProductItemName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var ProductItemRate in ProductItemRates)
                {
                    model.Add(new DisplayProductItemRate
                    {
                        ProductItemRateID = ProductItemRate.ProductItemRateID,
                        ProductItem = ProductItemRate.ProductItem.ProductItemName,
                        AmountRate = ProductItemRate.AmountRate,
                        DateCreated = ProductItemRate.DateCreated
                    });
                }
                var pager = new Pager(ProductItemRates.Count(), pageIndex);

                var modelR = new HoldDisplayProductItemRates
                {
                    HoldAllProductItemRates = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
        //public async Task<List<EgsProduct>> ProductList()
        //{
        //    List<EgsProduct> prodList = new List<EgsProduct>();

        //    prodList = await _unitOfWork.Products.GetAllProducts();

        //    prodList.Insert(0, new EgsProduct { ProductID = 0, ProductName = "--Select Product--" });

        //    return prodList;
        //}


        public async Task<List<EgsProduct>> ProductList()  //int? id)
        {
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = APIServiceConfig?.Url;

            List<EgsProduct> productList = new List<EgsProduct>();

            using (var client = new HttpClient())
            {
                //string IResponse = General.MakeRequest(Baseurl + "/api/Product/GetProductByCategoryId/{categoryid}/" + id.ToString(), "", "POST");
                string IResponse = General.MakeRequest(Baseurl + "/api/Product", "", "GET");

                string responseToCaller = string.Empty;

                if (IResponse.ToString() == "No data returned" || IResponse.ToString() == "[]" || IResponse.ToString() == "500")
                {
                    ViewBag.Message = IResponse.ToString();
                    //return View();
                }

                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                productList = jsSerializer.Deserialize<List<EgsProduct>>(IResponse);
            }
            return productList;
        }
         
        //Method to pull ProductItems 
        //public async Task<List<EgsProductItem>> ProductItemList()
        //{
        //    List<EgsProductItem> prodItemsList = new List<EgsProductItem>();

        //    int prdId = 9;

        //    prodItemsList = await _unitOfWork.ProductItem.GetItemsByProductID(prdId);

        //    prodItemsList.Insert(0, new EgsProductItem { ProductItemID = 0, ProductItemName = "--Select Product Item--" });

        //    return prodItemsList;
        //}


        public async Task<List<EgsProductItem>> ProductItemList()  //int? id)
        {
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = APIServiceConfig?.Url;

            List<EgsProductItem> prodItemsList = new List<EgsProductItem>();

            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(Baseurl + "/api/ProductItem", "", "GET");

                string responseToCaller = string.Empty;

                if (IResponse.ToString() == "No data returned" || IResponse.ToString() == "[]" || IResponse.ToString() == "500")
                {
                    ViewBag.Message = IResponse.ToString();
                    //return View();
                }

                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                prodItemsList = jsSerializer.Deserialize<List<EgsProductItem>>(IResponse);
            }
            return prodItemsList;
        }



        [HttpGet]   //   Task<List<EgsProductItem>>
        public async Task<JsonResult> GetProductitems(int ProductId)
        {
            if (ProductId != 0)
            {
                var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

                //string Baseurl = APIServiceConfig?.Url;

                var productendpoint = "/api​/ProductItem​/GetItemsByProduct​/";      //+ ProductId.ToString();
                 
                string Baseurl = $"{APIServiceConfig?.Url}{productendpoint}";

                //string _params = Convert.ToString(ProductId);
               
                //List<EgsProductItem> prodItemsList = new List<EgsProductItem>();

                //using (var client = new HttpClient())   //+ _params 
                //{  //POST
                //    string IResponse = General.MakeRequest(Baseurl , "", "GET");
                //    //string IResponse = General.MakeVFDRequest(Baseurl + "/api​/ProductItem​/GetItemsByProduct​/",ProductId.ToString(), "POST");

                //    string responseToCaller = string.Empty;

                //    if (IResponse.ToString() == "No data returned" || IResponse.ToString() == "[]" || IResponse.ToString() == "500")
                //    {
                //        ViewBag.Message = IResponse.ToString();
                //        //return View();
                //    }

                //    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                //    prodItemsList = jsSerializer.Deserialize<List<EgsProductItem>>(IResponse);
                //}


                string vfdEnquiryUrl = Baseurl;
                string vfdEnquiryReqParams = Convert.ToString(ProductId); ; //+"/"+ getstatecode.StateCode;  
                List<EgsProductItem> prodItemsList = new List<EgsProductItem>();

                using (var _client = new HttpClient())
                {
                    var Srequest = (vfdEnquiryUrl, vfdEnquiryReqParams, "GET");
                    string IResponse = General.MakeVFDRequest(vfdEnquiryUrl, vfdEnquiryReqParams, "GET");

                    //Save Request and Response
                    var RequestResponseLog = new SysRequestResponseLog
                    { 
                    Request = Srequest.ToString(),
                        Response = IResponse,
                        DateCreated = DateTime.Now
                    };
                    await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);




                    if (IResponse.ToString() == "No data returned" || IResponse.ToString() == "[]" || IResponse.ToString() == "500")
                    {
                        ViewBag.Message = IResponse.ToString();
                        //return View();
                    }

                    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                    prodItemsList = jsSerializer.Deserialize<List<EgsProductItem>>(IResponse);
 
                }
                 
                //return prodItemsList;
                //prodItemsList.Insert(0, new EgsProductItem { ProductItemID = 0, ProductItemName = "--Select--" });
                return Json(new SelectList(prodItemsList, "ProductItemID", "ProductItemName"));
            }
            return null;
        }



















        //Method to pull ProductItems 
        //public async Task<List<EgsProductItem>> ProductItemRates()
        //{

        //    string Baseurl = "http://localhost:810/";

        //    List<EgsProductItem> prodItemsList = new List<EgsProductItem>();

        //    using (var client = new HttpClient())
        //    {
        //        string IResponse = General.MakeRequest(Baseurl + "api/ProductItem/", "", "GET");

        //        string responseToCaller = string.Empty;

        //        if (!String.IsNullOrEmpty(IResponse))
        //        {
        //            prodItemsList = JsonConvert.DeserializeObject<List<EgsProductItem>>(IResponse);
        //            responseToCaller = "sent";
        //            prodItemsList.Insert(0, new EgsProductItem { ProductItemID = 0, ProductItemName = "--Select Product Item --" });

        //        }
        //        else
        //        {
        //            responseToCaller = "not sent";
        //            prodItemsList.Insert(0, new EgsProductItem { ProductItemID = 0, ProductItemName = "--N/A Product Item --" });
        //        }

        //    }

        //    return prodItemsList;
        //}

        #endregion

    }

}
