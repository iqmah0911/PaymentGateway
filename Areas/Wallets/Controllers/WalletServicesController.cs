using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using PaymentGateway21052021.Helpers;
using System.Net.Http;
using Nancy.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Repositories;
using RestSharp;
using PaymentGateway21052021.Areas.Invoices.Models;
using PaymentGateway21052021.Areas.Wallets.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway21052021.Areas.Wallets.Controllers
{
    [Area("Wallets")]
    [Authorize]
    public class WalletServicesController : Controller
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

        public WalletServicesController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<ActionResult> Mobile(int? id)
        //{
        //    ViewBag.Products = await ProductsList(id);
        //    return View();
        //}

        public async Task<ActionResult> OrderWallet()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Mobile(int? id)//(int? id, bool? IsFixedAmount,string productName = "")
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            if (TempData["ErrorViewBag"] != null)
            {
                ViewBag.Message = TempData["ErrorViewBag"];
            }

            ViewBag.Products = await ProductByCategoryList(Convert.ToInt32(id));
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Mobile(WalletServices model)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            ViewBag.LoginUser = user2.Email;

            
            return RedirectToAction("OrderInfo", "WalletServices", new { id = model.ProductID });

        }

        public async Task<List<SysSetup.Models.DDListView>> ProductItemList(int ProductID)
        {
            var prodItem = await _unitOfWork.ProductItem.GetItemsByProductID(ProductID);
            List<SysSetup.Models.DDListView> prodItemList = new List<SysSetup.Models.DDListView>();


            var ltrprodItem = prodItem.Where(p => p.ProductItemCode != "ENUTransFees");

            foreach (var prodItems in ltrprodItem)
            {
                prodItemList.Add(new SysSetup.Models.DDListView
                {
                    strItemValue = prodItems.ProductItemCode,
                    itemName = prodItems.ProductItemCode,
                });
            }

            SysSetup.Models.DropDownModelView nprodItemlist = new SysSetup.Models.DropDownModelView();
            nprodItemlist.items = prodItemList;

            return nprodItemlist.items;
        }

        public async Task<IActionResult> OrderInfo(int? id)//(int? id, bool? IsFixedAmount,string productName = "")
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            ViewBag.LoginUser = user2.Email;


            if (id == null || id == 0)
            {
                ViewBag.Message = "You don't have permission to use this page.";
                return View();
            } 

            var productDetails = await _unitOfWork.Products.GetProducts(Convert.ToInt32(id));

            var getstate = await _unitOfWork.Merchant.GetMerchantsById(productDetails.Merchant.MerchantID);

            if (productDetails.ProductID==49)
            {
                ViewBag.ProductItemList = await ProductItemList(productDetails.ProductID);
                var qprodItem = await _unitOfWork.ProductItem.GetProductItem(productDetails.ProductID);

                var qproductMode = new WalletServices
                {
                    ProductModeID = productDetails.InvoiceModeID,
                    ProductID = Convert.ToInt32(id),
                    LoginUser = user2.Email,
                    ProductItemID = qprodItem.ProductItemID,
                    UserState = user2.ResidentialState.StateName,
                };

                return View(qproductMode);
            }




            if (getstate.ResidentialState.StateID== user2.ResidentialState.StateID )
            {
                ViewBag.ProductItemList = await ProductItemList(productDetails.ProductID);
                var crprodItem = await _unitOfWork.ProductItem.GetProductItem(productDetails.ProductID);

                var crproductMode = new WalletServices
                {
                    ProductModeID = productDetails.InvoiceModeID,
                    ProductID = Convert.ToInt32(id),
                    LoginUser = user2.Email,
                    ProductItemID = crprodItem.ProductItemID,
                    UserState = user2.ResidentialState.StateName,
                };

                return View(crproductMode);
            }


            if (user2.ResidentialState.StateID != getstate.ResidentialState.StateID)
            {
                ViewBag.Message = "You are not allowed to make payment in the state: " + getstate.ResidentialState.StateName;
                TempData["ErrorViewBag"] = ViewBag.Message;
                return RedirectToAction("Mobile","WalletServices",new { id = productDetails.ProductCategories.ProductCategoryID });

            }

            ViewBag.ProductItemList = await ProductItemList(productDetails.ProductID);
            var prodItem = await _unitOfWork.ProductItem.GetProductItem(productDetails.ProductID);

            var productMode = new WalletServices
            {
                ProductModeID = productDetails.InvoiceModeID,
                ProductID = Convert.ToInt32(id),
                LoginUser = user2.Email,
                ProductItemID= prodItem.ProductItemID,
                UserState = user2.ResidentialState.StateName,
            };

            return View(productMode); 
        }

        [HttpPost]
        public async Task<IActionResult> OrderInfo(OrdersModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }
            EgsAuditTrail audit_;
            string invRef = "";
            if (model.ProductModeID == 1)
            {
                var productUser = await _unitOfWork.Products.GetProductExtension(model.ProductItemID);

                //var productUser = await _unitOfWork.ProductItem.GetItemsByProductID(model.ProductItemID);
                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());


                var invoiceUser = new EgsInvoice
                {
                    InvoiceID = model.InvoiceID,
                    Amount = model.AmountRate,
                    Product = productUser.Product,
                    ProductItemID = model.ProductItemID,
                    ServiceNumber = model.RegNumber,
                    ReferenceNo =  Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8)+ Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8),
                    DateCreated = DateTime.Now,
                    CreatedBy = user2.UserID
                };
                await _unitOfWork.Invoice.AddSaveAsync(invoiceUser);

                invRef = invoiceUser.ReferenceNo;
                ViewBag.Message = "Invoice has been created successfully";
                ViewBag.ProductItemList = await ProductItemList(Convert.ToInt32(model.ProductID));
                TempData["ProductModeID"] = model.ProductModeID;
                TempData["ProductID"] = model.ProductID;

                //Audit Trail 
                audit_ = new EgsAuditTrail
                {
                    DbAction = "INSERT",
                    DateCreated = DateTime.Now,
                    Page = "Order Info",
                    Description = user2.FirstName + " " + user2.LastName + " Successfully created Invoice with ReferenceNo "+ invRef +" at "+ DateTime.Now,
                    IPAddress = Helpers.General.GetIPAddress(),
                    CreatedBy = user2.UserID,
                    Menu = "Pay Bills",
                    Role = user2.Role.RoleName
                };

                await _unitOfWork.AuditTrail.AddAsync(audit_);
                _unitOfWork.Complete();

                return RedirectToAction("RechargeConfirmation", "HomeStore", new { ref_ = invRef, phone =model.PhoneNumber,prdid=model.ProductID });

                //return RedirectToAction("PayWithWallet", "HomeStore", new { prdid = model.ProductID,phone= model.PhoneNumber, ReferenceNo = invRef, prdMode = model.ProductModeID});
            }
            else if (model.ProductModeID == 2 )
            {
                TempData["ProductModeID"] = model.ProductModeID;
                TempData["ProductID"] = model.ProductID;

                return RedirectToAction("PayCableWallet", "HomeStore", model);
            }
            else if (model.ProductModeID == 3)
            {
                TempData["ProductModeID"] = model.ProductModeID;
                TempData["ProductID"] = model.ProductID;

                return RedirectToAction("PayWithWallet", "HomeStore", new { prdid = model.ProductID, prdItem = model.ProductItemCode, ReferenceNo = model.ReferenceNo, uEmail= model.LoginUser });
            }
            else if ( model.ProductModeID == 4)
            {
                TempData["ProductModeID"] = model.ProductModeID;
                TempData["ProductID"] = model.ProductID;

                //return RedirectToAction("ProductWallet", "HomeStore", new {id=model.ProductModeID, prdid = model.ProductID });
                return RedirectToAction("PayWithEWallet", "HomeStore", model);
            }
            else if (model.ProductModeID == 5)
            {
                TempData["ProductModeID"] = model.ProductModeID;
                TempData["ProductID"] = model.ProductID;
                //PayWithEsWallet
                //return RedirectToAction("ProductWallet", "HomeStore", new {id=model.ProductModeID, prdid = model.ProductID });
                return RedirectToAction("RechargeBundleConfirmation", "HomeStore", model);
            }
            else if (model.ProductModeID == 6)
            {
                var productUser = await _unitOfWork.Products.GetProductExtension(model.ProductItemID);

                ViewBag.ProductItemList = await ProductItemList(Convert.ToInt32(model.ProductID));
                TempData["ProductModeID"] = model.ProductModeID;
                TempData["ProductID"] = model.ProductID;
                TempData["ProductItemID"] = model.ProductItemID;

                return RedirectToAction("PayWaecWallet", "HomeStore", model);

            }

            return View();

        }


        public async Task<List<EgsProductCategory>> ProductCategoryList()
        {
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = APIServiceConfig?.Url;

            List<EgsProductCategory> prodCategoryList = new List<EgsProductCategory>();

            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(Baseurl + "api/ProductCategory/", "", "GET");

                string responseToCaller = string.Empty;

                if (IResponse.ToString() == "No data returned" || IResponse.ToString() == "[]" || IResponse.ToString() == "500")
                {
                    ViewBag.Message = IResponse.ToString();
                    //return View();
                }

                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                prodCategoryList = jsSerializer.Deserialize<List<EgsProductCategory>>(IResponse);
            }

            return prodCategoryList;
        }

        public async Task<List<EgsProduct>> ProductByCategoryList(int? id)
        {
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = APIServiceConfig?.Url;

            List<EgsProduct> productList = new List<EgsProduct>();

            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(Baseurl + "/api/Product/GetProductByCategoryId/" + id.ToString(), "", "POST");

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

        public async Task<List<EgsProduct>> ProductsList(int? id)
        {
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = APIServiceConfig?.Url;

            List<EgsProduct> ItemsList = new List<EgsProduct>();

            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(Baseurl + "/api/Product/GetProductByCategoryId/" + id.ToString(), "", "POST");

                string responseToCaller = string.Empty;

                if (IResponse.ToString() == "No data returned" || IResponse.ToString() == "[]" || IResponse.ToString() == "500")
                {
                    ItemsList.Insert(0, new EgsProduct { ProductID = 0, ProductName = "--Select Product--" });
                }
                else
                {
                    //JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                    ItemsList = JsonConvert.DeserializeObject<List<EgsProduct>>(IResponse);
                    //ItemsList = jsSerializer.Deserialize<List<EgsProduct>>(IResponse);
                    ItemsList.Insert(0, new EgsProduct { ProductID = 0, ProductName = "--Select Product--" });
                }
            }

        //    var ViewModel = new PayWithWalletViewModel();
        //    ERegInvoicesViewModel testR = GetInvoiceInfo(ViewModel.ReferenceNo);
        //    var paymentNotification = new ProductNotificationResponse
        //    {
        //        MerchantId = 23,
        //        MerchantCode = "AKWA",
        //        cusName = "",
        //        cusPhoneNo = "",
        //        cusEmail = testR.email,
        //        Product = ViewModel.ProductName,
        //        ProductCode = "ProductCode",
        //        ProductItemLists
        //        UniqueReferenceNo = ViewModel.ReferenceNo,
        //        TransactionAmount = ViewModel.Amount.ToString(),
        //        DateCreated = ViewModel.TransactionDate.ToString()
        //        //Forming objects for the Pay Line Item
                
        //        //ProductItemLists.Add(new ProductsItemModel
        //        //{
        //        //    //transactionType = fetchAllTranc.OptServiceName,
        //        //    transactionType = serviceName,
        //        //    amountInNaira = fetchAllTrancc.Amount,
        //        //    paymentItemCode = fetchAllTrancc.StateServiceRate.ServiceRateCode
        //        //},
        //};


            return ItemsList;
        }

        public async Task<List<EgsProductItem>> ProductItemList(int? id)
        {
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = APIServiceConfig?.Url;

            List<EgsProductItem> ItemsList = new List<EgsProductItem>();

            using (var client = new HttpClient())
            {     //POST
                string IResponse = General.MakeRequest(Baseurl + "/api/ProductItem/GetItemsByProduct/" + id.ToString(), "", "GET");

                string responseToCaller = string.Empty;

                if (!String.IsNullOrEmpty(IResponse))
                {
                    ItemsList = JsonConvert.DeserializeObject<List<EgsProductItem>>(IResponse);
                    //responseToCaller = "sent";
                    ItemsList.Insert(0, new EgsProductItem { ProductItemID = 0, ProductItemName = "--Select Product Item --" });

                }
                else
                {
                    //responseToCaller = "not sent";
                    ItemsList.Insert(0, new EgsProductItem { ProductItemID = 0, ProductItemName = "--N/A Product Item --" });
                }

            }

            return ItemsList;
        }


        //public async ERegInvoicesViewModel GetInvoiceInfo(string ref_)  //  ERegInvoicesViewModel model
        //{
        //    var refDetail = await _unitOfWork.Invoice.GetReferenceNo(ref_);

        //    var getproduct = await _unitOfWork.Products.GetProducts(refDetail.Product.ProductID);

        //    string productUrl = getproduct.Verification;


        //    var clientUrl = productUrl + ref_;

        //    var client = new RestClient(clientUrl);
        //    var request = new RestRequest(Method.GET);
        //    request.AddHeader("Accept", "application/json");
        //    var response = client.Execute(request);

        //    //getting the response string

        //    var getcontent = response.Content.ToString();

        //    //Replacing 1st header container
        //    string jsonstring = getcontent.Replace("\"customerInformationResponse\":{", "");

        //    //Replacing 2nd header container for product items
        //    jsonstring = jsonstring.Replace("\"paymentItems\":", "");

        //    //Removing extra curly braces at the end of string
        //    jsonstring = jsonstring.Remove(jsonstring.Length - 1, 1);
        //    //Removing curly braces at the end and beginnining of items 

        //    int bracesStart = jsonstring.IndexOf("items") - 2;

        //    jsonstring = jsonstring.Remove(bracesStart, 1);

        //    int bracesEnd = jsonstring.IndexOf("]") + 1;

        //    jsonstring = jsonstring.Remove(bracesEnd, 1);


        //    ERegInvoicesViewModel ereg = JsonConvert.DeserializeObject<ERegInvoicesViewModel>(jsonstring);

        //    var allmodel = new ERegInvoicesViewModel
        //    {
        //        status = ereg.status,
        //        message = ereg.message,
        //        custReference = ereg.custReference,
        //        customerReferenceAlternate = ereg.customerReferenceAlternate,
        //        firstName = ereg.firstName,
        //        lastName = ereg.lastName,
        //        otherName = ereg.otherName,
        //        email = ereg.email,
        //        phone = ereg.phone,
        //        thirdPartyCode = ereg.thirdPartyCode,
        //        agentName = ereg.agentName,
        //        branchName = ereg.branchName,
        //        registrationNo = ereg.registrationNo,
        //        items = ereg.items,
        //        totalAmount = ereg.totalAmount

        //    };

        //    return allmodel;
        //}


        public ActionResult GetRates(string selection)
        {
            // do your server-side processing and get your data
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = APIServiceConfig?.Url;

            var data = "";
            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(Baseurl + "/api/ProductItemRate/GetItemRatesByProductItem/" + selection.ToString(), "", "GET");

                string responseToCaller = string.Empty;

                if (!String.IsNullOrEmpty(IResponse))
                {
                    data = IResponse; //JsonConvert.SerializeObject(IResponse);//JsonSerializer.Deserialize(json, typeof(List<Person>))
                }
                else
                {
                    //data = "";
                }

            }
            return Json(data);
        }






        




    }
}
