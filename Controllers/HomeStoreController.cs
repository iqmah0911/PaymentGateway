using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.Invoices.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


//using System.Web.Mvc;

namespace PaymentGateway21052021.Controllers
{
    public class HomeStoreController : Controller
    {
        string token = "bcb81916-4f8d-3407-882c-d88d5a342382";
        string walletcred = "Q3dhVDRjSkkwdjl0ZkhMMXFpXzN5Q1dhXzNZYTp1d0Fhb0Q3X1BzZGNWWVVWa3hGUHFMWnVGMFlh";
        #region logger
        private readonly ILogger<dynamic> _logger;
        #endregion

        #region repositories
        private IUnitOfWork _unitOfWork;
        #endregion

        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public HomeStoreController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, IEmailSender emailSender,
            RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;
            _roleManager = roleManager;
        }


        public IActionResult Welcome()
        {
            return Redirect("/Identity/Account/Login");
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var firstdayOfThisWeek = DateTime.Now.FirstDayOfWeek();
            var lastDayOfWeek = DateTime.Now.LastDayOfWeekWithD1();
            var nextDay = DateTime.Now.NextDay();
            var firstDayOfThisMonth = DateTime.Now.FirstDayOfMonth();
            var lastDayOfThisMonth = DateTime.Now.LastDayOfMonth().AddDays(1);

            ViewBag.DateFirstWeek = firstdayOfThisWeek;
            ViewBag.DateLastWeek = lastDayOfWeek;
            ViewBag.DateNextDay = nextDay;
            ViewBag.DateFirstDayOfThisMonth = firstDayOfThisMonth;
            ViewBag.DateFirstDayOfNextMonth = lastDayOfThisMonth;
            ViewBag.ProductCategory = await ProductCategoryList();
            //ViewBag.ProductCategory = await _unitOfWork.ProductCategory.GetAllProductCategory();

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Index(int? pageIndex, string prodCategory, string searchText)
        {
            try
            {
                ViewBag.ProductCategory = await ProductCategoryList();


                return RedirectToAction("GetProductItemRates", "HomeStore", new { prct = prodCategory, vsearch = searchText });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        public string FetchRequest(string paramsval, string paramsval2)
        {

            string _params = "";
            //_params = paramsval;

            //if (paramsval2 != "0")
            //{
            //   _params = string.Empty;
            //   _params = paramsval + paramsval2;
            //}
            _params = paramsval + "/" + paramsval2;

            var productendpoint = "/api/Product/GetProductALike/";
            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            //string Baseurl = EgolePayServiceConfig.Url ;
            string Baseurl = $"{EgolePayServiceConfig?.Url}{productendpoint}";


            string responseToCaller = "";
            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(Baseurl + _params, "", "POST");

                if (!string.IsNullOrEmpty(IResponse))
                {
                    responseToCaller = IResponse;
                }
                else
                {
                    responseToCaller = "Invalid Login Details";
                }

            }
            return responseToCaller;
        }

        public string FetchGetRequest(string paramsval)
        {
            string responseToCaller = "";
            using (var client = new HttpClient())
            {
                string IResponse = General.MakeRequest(paramsval, "", "POST");

                if (!string.IsNullOrEmpty(IResponse))
                {
                    responseToCaller = IResponse;
                }
                else
                {
                    responseToCaller = "Invalid Login Details";
                }

            }
            return responseToCaller;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductItemRates(int? pageIndex, string prct = "", string vsearch = "")
        {
            try
            {
                ViewBag.ProductCategory = await ProductCategoryList();
                List<HomeStoreModel> model = new List<HomeStoreModel>();
                List<HomeStoreModel> productList = new List<HomeStoreModel>();
                //string cateParams = "?prodCategory = " + prodCategory;
                string apiResponse = FetchRequest(vsearch, prct);
                if (!string.IsNullOrEmpty(vsearch))
                {
                    if (apiResponse.ToString() == "No data returned" || apiResponse.ToString() == "[]" || apiResponse.ToString() == "500")
                    {

                    }
                    else
                    {
                        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                        productList = jsSerializer.Deserialize<List<HomeStoreModel>>(apiResponse);
                    }
                }

                //load up the list of viewmodels 
                foreach (var Products_ in productList)
                {
                    model.Add(new HomeStoreModel
                    {
                        ProductID = Products_.ProductID,
                        ProductName = Products_.ProductName,
                        InvoiceModeId = Products_.InvoiceModeId
                    });

                }
                var pager = new Pager(productList.Count(), pageIndex);

                var modelR = new HoldProductsAlike
                {
                    HoldAllProductsAlike = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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

        [HttpPost]
        public async Task<IActionResult> GetProductItemRates(HoldProductsAlike model)
        {
            try
            {
                //ViewBag.ProductCategory = await ProductCategoryList();
                TempData["ProductID"] = model.ProductID;

                if (TempData["ProductID"].ToString() == "" || TempData["ProductID"].ToString() == null)
                {
                    ViewBag.Message = "Product must be selected";
                    return View();
                }
                //
                return RedirectToAction("ProductInfoCollection", "HomeStore", new { id = model.InvoiceModeId, productid = model.ProductID, product = model.ProductName });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            return View();
        }

        public async Task<List<EgsProductCategory>> ProductCategoryList()
        {
            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = EgolePayServiceConfig.Url;


            List<EgsProductCategory> prodCategoryList = new List<EgsProductCategory>();

            using (var client = new HttpClient())
            {
                var Srequest = (Baseurl + "/api/ProductCategory/", "", "GET");
                string IResponse = General.MakeRequest(Baseurl + "/api/ProductCategory/", "", "GET");

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                string responseToCaller = string.Empty;

                if (!String.IsNullOrEmpty(IResponse))
                {
                    prodCategoryList = JsonConvert.DeserializeObject<List<EgsProductCategory>>(IResponse);
                    //responseToCaller = "sent";
                    prodCategoryList.Insert(0, new EgsProductCategory { ProductCategoryID = 0, ProductCategoryName = "-- Product Category(s) --" });
                }
                else
                {
                    //responseToCaller = "not sent";
                    prodCategoryList.Insert(0, new EgsProductCategory { ProductCategoryID = 0, ProductCategoryName = "-- Product Category(s) --" });
                }

            }

            return prodCategoryList;
        }

        [HttpGet]
        public async Task<IActionResult> ProductWallet(int? id, string vals = null)//(int? id, bool? IsFixedAmount,string productName = "")
        {
            if (id == null)
            {
                ViewBag.Message = "You don't have permission to use this page.";
                return View();
            }
            else if (id == 1) //Product information is on egole
            {
                var getProdDetails = await _unitOfWork.ProductItem.GetProductItems(Convert.ToInt32(TempData["ProductItemID"]));

                var getWallet = new OrdersModel
                {
                    ProductModeID = Convert.ToInt32(id),
                    ProductID = getProdDetails.Product.ProductID,
                    ProductName = getProdDetails.Product.ProductName,
                    ProductItemID = Convert.ToInt32(TempData["ProductItemID"]),
                    ProductItemName = getProdDetails.ProductItemName,
                    AmountRate = Convert.ToDouble(TempData["AmountRate"]),
                    Email = Convert.ToString(TempData["Email"]),
                    PhoneNumber = Convert.ToString(TempData["PhoneNumber"]),
                    RegNumber = Convert.ToString(TempData["RegNumber"])
                };
                return View(getWallet);
            }
            else if (id == 2) //load the invoice information from egole with the ref nummber then make payment if still pending
            {
                var getWallet = new OrdersModel
                {
                    ProductModeID = Convert.ToInt32(id),
                    ReferenceNo = Convert.ToString(TempData["ReferenceNo"])
                };
                return View(getWallet);
            }
            else if (id == 3) //get 
            {
                var getWallet = new OrdersModel
                {
                    ProductModeID = Convert.ToInt32(id),
                    ProductID = Convert.ToInt32(vals),
                    Email = Convert.ToString(TempData["Email"]),
                    PhoneNumber = Convert.ToString(TempData["PhoneNumber"]),
                    RegNumber = Convert.ToString(TempData["RegNumber"])
                };
                return View(getWallet);
            }
            else if (id == 4) //get 
            {
                //var getProdDetails = await _unitOfWork.ProductItem.GetProductItems(Convert.ToInt32(TempData["ProductItemID"]));
                var getWallet = new OrdersModel
                {
                    ProductModeID = Convert.ToInt32(id),
                    ProductID = Convert.ToInt32(vals),
                    MeterNo = Convert.ToString(TempData["MeterNo"]),
                    vendType = Convert.ToString(TempData["vendType"]),
                    discoCode = Convert.ToString(TempData["discoCode"]),

                    //ProductID =  getProdDetails.Product.ProductID, 
                    // ProductItemID =  Convert.ToInt32(TempData["ProductItemID"]),
                };
                return View(getWallet);
            }

            //return View(getWallet);
            return null;

        }

        [HttpPost]
        public async Task<IActionResult> ProductWallet(OrdersModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            string invRef = "";
            if (model.ProductModeID == 1)
            {

                var productUser = await _unitOfWork.Products.GetProductExtension(model.ProductItemID);

                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

                var invoiceUser = new EgsInvoice
                {
                    InvoiceID = model.InvoiceID,
                    Amount = model.AmountRate,
                    Product = productUser.Product,
                    ProductItemID = model.ProductItemID,
                    ServiceNumber = model.RegNumber,
                    ReferenceNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8),//General.GetInvoiceRef(Guid.NewGuid().ToString()),//.Substring(0, 5)
                    DateCreated = DateTime.Now,
                    CreatedBy = user2.UserID
                };
                await _unitOfWork.Invoice.AddSaveAsync(invoiceUser);

                ////var product = await _unitOfWork.Products.GetProducts(Convert.ToInt32(model.ProductID));
                //var invUser = await _unitOfWork.Invoice.GetInvoice(Convert.ToInt32(model.InvoiceID));
                //var transTypeUser = _unitOfWork.TransactionType.Get(2); //2 is for a debit transaction type
                //var prodItemUser = await _unitOfWork.ProductItem.GetProductItems(Convert.ToInt32(model.ProductItemID));

                invRef = invoiceUser.ReferenceNo;
                ViewBag.Message = "Invoice has been created successfully";
                ViewBag.ProductItemList = await ProductItemList(Convert.ToInt32(model.ProductID));
                TempData["ProductModeID"] = model.ProductModeID;
                TempData["ProductID"] = model.ProductID;

                //return RedirectToAction("PayWithWallet", "HomeStore", new { ReferenceNo = invRef });
                return RedirectToAction("PayWithWallet", "HomeStore", new { prdid = model.ProductID, phone = model.PhoneNumber, ReferenceNo = invRef });


            }
            else if (model.ProductModeID == 2 || model.ProductModeID == 3)
            {
                TempData["ProductModeID"] = model.ProductModeID;
                TempData["ProductID"] = model.ProductID;
                return RedirectToAction("PayWithWallet", "HomeStore", new { prdid = model.ProductID, ReferenceNo = model.ReferenceNo });
            }
            else if (model.ProductModeID == 4)
            {
                TempData["ProductModeID"] = model.ProductModeID;
                TempData["ProductID"] = model.ProductID;
                if (model.vendType != null && model.MeterNo != null && model.discoCode != null)
                {
                    return RedirectToAction("PayWithEWallet", "HomeStore", model);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PayWithEsWallet(OrdersModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            TempData["ProductModeID"] = model.ProductModeID;
            ViewBag.ProductID = model.ProductID;

            var UCardDataServiceConfig = Startup.StaticConfig.GetSection("UCardDataService").Get<UCardDataService>();

            var productUrl = UCardDataServiceConfig.Url;

            var phone = model.Phone;
            var msisdn = "234" + phone;
            ViewBag.msisdn = msisdn;
            string result = phone.Substring(0, 5);
            string provider;
            var regResponse = new DataRoot();

            if (result.Contains("07025") || result.Contains("07026") || result.Contains("0703") || result.Contains("0704") || result.Contains("0706") || result.Contains("0803") || result.Contains("0806")
             || result.Contains("0810") || result.Contains("0813") || result.Contains("0814") || result.Contains("0816") || result.Contains("0903") || result.Contains("0906"))
            {
                provider = "MTN";

                var clientUrl = productUrl + "msisdn=" + msisdn;

                var client = new RestClient(clientUrl);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                var response = client.Execute(request);
                var amt = Convert.ToString(model.Amount);
                var getcontent = response.Content.ToString();

                regResponse = JsonConvert.DeserializeObject<DataRoot>(getcontent);

                if (Convert.ToInt32(regResponse.code) >= 1)
                {
                    //ViewBag.ErrorMessage = "Response " + regResponse.data.;
                    //return RedirectToAction("OrderInfo", "WalletServices", new { area = "Wallets", id = prdid });
                }

                var allmodel = new DataRoot
                {
                    code = regResponse.code,
                    data = regResponse.data.Where(x => x.Operator == provider && x.topup_price == amt),
                    phone = model.Phone,
                    Amount = Convert.ToInt32(model.Amount),
                    ProductID = model.ProductID,
                    ProductModeID = model.ProductModeID
                };

                return View(allmodel);

            }
            else if (result.Contains("0802") || result.Contains("0808") || result.Contains("0708") || result.Contains("0812") || result.Contains("0701") || result.Contains("0902"))
            {
                provider = "Airtel";
                var clientUrl = productUrl + "msisdn=" + msisdn;

                var client = new RestClient(clientUrl);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                var response = client.Execute(request);
                var amt = Convert.ToString(model.Amount);
                var getcontent = response.Content.ToString();

                regResponse = JsonConvert.DeserializeObject<DataRoot>(getcontent);

                if (Convert.ToInt32(regResponse.code) >= 1)
                {
                    //ViewBag.ErrorMessage = "Response " + regResponse.data.;
                    //return RedirectToAction("OrderInfo", "WalletServices", new { area = "Wallets", id = prdid });
                }

                var allmodel = new DataRoot
                {
                    code = regResponse.code,
                    data = regResponse.data.Where(x => x.Operator == provider && x.topup_price == amt),
                    phone = model.Phone,
                    Amount = Convert.ToInt32(model.Amount),
                    ProductID = model.ProductID,
                    ProductModeID = model.ProductModeID
                };

                return View(allmodel);
            }
            else if (result.Contains("0805") || result.Contains("0807") || result.Contains("0705") || result.Contains("0815") | result.Contains("0811") || result.Contains("0905"))
            {
                provider = "Globacom";
                var clientUrl = productUrl + "msisdn=" + msisdn;

                var client = new RestClient(clientUrl);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                var response = client.Execute(request);
                var amt = Convert.ToString(model.Amount);
                var getcontent = response.Content.ToString();

                regResponse = JsonConvert.DeserializeObject<DataRoot>(getcontent);

                if (Convert.ToInt32(regResponse.code) >= 1)
                {
                    //ViewBag.ErrorMessage = "Response " + regResponse.data.;
                    //return RedirectToAction("OrderInfo", "WalletServices", new { area = "Wallets", id = prdid });
                }

                var allmodel = new DataRoot
                {
                    code = regResponse.code,
                    data = regResponse.data.Where(x => x.Operator == provider && x.topup_price == amt),
                    phone = model.Phone,
                    Amount = Convert.ToInt32(model.Amount),
                    ProductID = model.ProductID,
                    ProductModeID = model.ProductModeID
                };

                return View(allmodel);
            }
            else if (result.Contains("0809") || result.Contains("0818") || result.Contains("0817") || result.Contains("0909"))
            {
                provider = "9mobile";
                var clientUrl = productUrl + "msisdn=" + msisdn;

                var client = new RestClient(clientUrl);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                var response = client.Execute(request);
                var amt = Convert.ToString(model.Amount);
                var getcontent = response.Content.ToString();

                regResponse = JsonConvert.DeserializeObject<DataRoot>(getcontent);

                if (Convert.ToInt32(regResponse.code) >= 1)
                {
                    //ViewBag.ErrorMessage = "Response " + regResponse.data.;
                    //return RedirectToAction("OrderInfo", "WalletServices", new { area = "Wallets", id = prdid });
                }

                var allmodel = new DataRoot
                {
                    code = regResponse.code,
                    data = regResponse.data.Where(x => x.Operator == provider && x.topup_price == amt),
                    phone = model.Phone,
                    Amount = Convert.ToInt32(model.Amount),
                    ProductID = model.ProductID,
                    ProductModeID = model.ProductModeID
                };

                return View(allmodel);
            }

            return View();
        }




        /// <summary>
        /// Airtime Recharge
        /// </summary>
        /// <param name="ref_"></param>
        /// <param name="phone"></param>
        /// <param name="prdid"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> RechargeConfirmation(string ref_, string phone, string prdid)
        {
            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = EgolePayServiceConfig.Url;

            var data = "";
            var aregResponse = new AutoDResponse();
            using (var client = new HttpClient())
            {
                var Srequest = (Baseurl + "/api/Invoice/" + ref_, "", "GET");
                string IResponse = General.MakeRequest(Baseurl + "/api/Invoice/" + ref_, "", "GET");

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    aregResponse = JsonConvert.DeserializeObject<AutoDResponse>(IResponse);

                    if (aregResponse.status == "True")
                    {
                        ViewBag.ErrorMessage = "Response " + aregResponse.message;

                        //return View(ref_);
                        return RedirectToAction("RechargeConfirmation", "HomeStore",
                            new { ref_ = ref_, phone = phone, prdid = prdid });

                    }
                }

                string result = phone.Substring(0, 5);
                string provider = "";

                if (result.Contains("07025") || result.Contains("07026") || result.Contains("0703") || result.Contains("0704") || result.Contains("0706") || result.Contains("0803") || result.Contains("0806")
              || result.Contains("0810") || result.Contains("0813") || result.Contains("0814") || result.Contains("0816") || result.Contains("0903") || result.Contains("0906"))
                {
                    provider = "MTN";
                }
                else if (result.Contains("0802") || result.Contains("0808") || result.Contains("0708") || result.Contains("0812") || result.Contains("0701") || result.Contains("0902"))
                {
                    provider = "Airtel";
                }
                else if (result.Contains("0805") || result.Contains("0807") || result.Contains("0705") || result.Contains("0815") | result.Contains("0811") || result.Contains("0905"))
                {
                    provider = "Globacom";
                }
                else if (result.Contains("0809") || result.Contains("0818") || result.Contains("0817") || result.Contains("0909"))
                {
                    provider = "9mobile";
                }
                var rmodel = new OrdersModel
                {
                    FirstName = aregResponse.firstName,
                    MiddleName = aregResponse.middleName,
                    LastName = aregResponse.lastName,
                    Email = aregResponse.email,
                    Phone = phone,     //aregResponse.phone,
                    CustReference = aregResponse.custReference,
                    Amount = aregResponse.amount,
                    TotalAmount = aregResponse.totalAmount,
                    ReferenceNo = aregResponse.custReference,
                    RegistrationNo = aregResponse.registrationNo,
                    ProductModeID = aregResponse.productModeID,
                    ProductName = provider, //aregResponse.productName,
                    AmountRate = aregResponse.amountRate,
                    ProductID = Convert.ToInt32(prdid)  //Convert.ToInt32(aregResponse.productID)  
                };
                ViewBag.ProductID = Convert.ToInt32(prdid);
                return View(rmodel);

            }
        }


        [HttpPost]
        public async Task<IActionResult> RechargeConfirmation(OrdersModel model)
        {
            if (!string.IsNullOrEmpty(model.ReferenceNo)) //ReferenceNo
            {
                if (model.ProductModeID == 1)  //prdMode
                {
                    var ViewModel = new OrdersModel();

                    var user = await _userManager.GetUserAsync(User);
                    var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

                    //Check the Wallet balance of the user logged in of all TransactionType(1) 
                    var walletBalance = await _unitOfWork.Invoice.GetWalletBalance(user2.Wallet.WalletID);

                    var balanceAmount = walletBalance.Sum(u => u.Amount);

                    //Check if the balance is sufficient enough to carry out the transaction
                    if (balanceAmount > model.Amount)
                    {
                        var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                        string Baseurl = EgolePayServiceConfig.Url;

                        string producturl = Baseurl;
                        var aregResponse = new AirtimeRechargeResponse();
                        var phoneno = model.Phone;
                        double amt = model.TotalAmount;
                        int prdID = model.ProductID;
                        string prdname = model.ProductName;
                        string refNo = model.ReferenceNo;
                        int uid = user2.UserID;

                        using (var client = new HttpClient())
                        {
                            var Srequest = (Baseurl + "/api/Invoice/AirtimeRecharge/" + phoneno + "/" + amt + "/" + prdID + "/" + prdname + "/" + refNo + "/" + uid, "POST");
                            string vfdEnquiryReqParams = "/api/Invoice/AirtimeRecharge/" + phoneno + "/" + amt + "/" + prdID + "/" + prdname + "/" + refNo + "/" + uid;
                            string IResponse = General.MakeVFDRequest(Baseurl, vfdEnquiryReqParams, "POST");

                            //Save Request and Response
                            var logResponse = JsonConvert.DeserializeObject<AirtimeResponse>(IResponse);

                            var RequestResponseLog = new SysRequestResponseLog
                            {
                                Request = Srequest.ToString(),
                                Response = IResponse,
                                DateCreated = DateTime.Now,
                                Message = logResponse.message,
                                StatusCode = logResponse.status,
                                TransactionType = "AirtimeRecharge"
                            };

                            await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                            if (!String.IsNullOrEmpty(IResponse))
                            {

                                //aregResponse = JsonConvert.DeserializeObject<AirtimeRechargeResponse>(IResponse);

                                if (Convert.ToInt32(logResponse.status) != 200)
                                {
                                    ViewBag.ErrorMessage = "Response " + logResponse.message;

                                    return View(model);
                                }
                            }

                            model.CustReference = logResponse.transId;
                            ViewBag.SuccessMessage = "Recharge Successful,Thanks for your Patronage";
                            return RedirectToAction("AirtimeRechargeReceipt", "HomeStore", model);

                        }
                    }
                }
                return View(model);
            }
            else
            {
                ViewBag.Message = "There is no record for this order ";
                return View();
            }
        }


        public async Task<IActionResult> AirtimeRechargeReceipt(OrdersModel model)
        {
            ViewBag.Date = DateTime.Now;
            ViewBag.OrderID = model.CustReference;
            ViewBag.Amount = model.TotalAmount;
            ViewBag.Network = model.ProductName;
            ViewBag.Phone = model.Phone;
            ViewBag.Total = model.TotalAmount;

            return View();
        }


        public int randomNumber()
        {
            var random = new Random();
            int randomNumber = random.Next(1, 999999999) + random.Next(1, 999999999);
            return randomNumber;
        }


        /// <summary>
        /// //DataBundle Recharge
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>


        public async Task<IActionResult> RechargeBundleConfirmation(OrdersModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            TempData["ProductModeID"] = model.ProductModeID;
            ViewBag.ProductID = model.ProductID;

            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
            string Baseurl = EgolePayServiceConfig.Url;

            //var ShagoServiceConfig = Startup.StaticConfig.GetSection("ShagoPaymentService").Get<ShagoPayService>();
            //string productUrl = ShagoServiceConfig.Url;
            //var authKey = ShagoServiceConfig.hashKey;
            //var Email = ShagoServiceConfig.email;
            //var Password = ShagoServiceConfig.password;

            var phone = model.Phone;
            var msisdn = phone; //"234" + ;
            ViewBag.msisdn = msisdn;
            string result = phone.Substring(0, 5);
            string provider;
            var regResponse = new Roots();
            var aregResponse = new AirtimeRechargeResponse();


            using (var client = new HttpClient())
            {
                var Srequest = (Baseurl + "/api/Invoice/GetDataBundle/" + phone + "/" + model.ProductID + "/" + model.ProductModeID, "GET");
                string vfdEnquiryReqParams = "/api/Invoice/GetDataBundle/" + phone + "/" + model.ProductID + "/" + model.ProductModeID;
                string IResponse = General.MakeVFDRequest(Baseurl, vfdEnquiryReqParams, "GET");

                //Save Request and Response
                var logResponse = JsonConvert.DeserializeObject<AutoRegResponse>(IResponse);

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now,
                    Message = logResponse.message,
                    StatusCode = logResponse.status,
                    TransactionType = "AirtimedataBundle"
                };

                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    regResponse = JsonConvert.DeserializeObject<Roots>(IResponse);

                    if (Convert.ToInt32(regResponse.status) != 200)
                    {
                        ViewBag.ErrorMessage = "Response " + regResponse.message;

                        return View(model);
                    }
                }
                var allmodel = new Roots
                {
                    network = regResponse.network,
                    status = regResponse.status,
                    product = regResponse.product.ToList(),
                    phone = regResponse.phone,       //model.Phone,
                    //Amount = Convert.ToInt32(model.Amount),
                    ProductID = regResponse.ProductID,      //model.ProductID,
                    ProductModeID = regResponse.ProductModeID,         //model.ProductModeID
                };
                return View("RechargeBundleVend", allmodel);
                //return RedirectToAction("RechargeBundleVend","HomeStore", allmodel);
            }

        }


        public async Task<IActionResult> RechargeBundleVend(Roots model)//
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> RechargeBundleConfirmation(Roots model)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var walletBalance = await _unitOfWork.Invoice.GetWalletBalance(user2.Wallet.WalletID);

            var balanceAmount = walletBalance.Sum(u => u.Amount);

            //Check if the balance is sufficient enough to carry out the transaction
            if (balanceAmount > model.Amount)
            {
                var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                string Baseurl = EgolePayServiceConfig.Url;

                string producturl = Baseurl;
                var aregResponse = new AirtimeRechargeResponse();
                var phoneno = model.phone;
                string amt = model.Amount.ToString();
                int prdID = model.ProductID;
                string bundle = model.bundle;
                string network = model.network;
                int prdmodeID = model.ProductModeID;
                int uid = user2.UserID;
                string serviceCode = model.serviceCode;

                using (var client = new HttpClient())
                {
                    var Srequest = (Baseurl + "/api/Invoice/DataRecharge/" + serviceCode + "/" + phoneno + "/" + amt + "/" + bundle + "/" + network + "/" + prdID + "/" + prdmodeID + "/" + uid, "POST");
                    string vfdEnquiryReqParams = "/api/Invoice/DataRecharge/" + serviceCode + "/" + phoneno + "/" + amt + "/" + bundle + "/" + network + "/" + prdID + "/" + prdmodeID + "/" + uid;
                    string IResponse = General.MakeVFDRequest(Baseurl, vfdEnquiryReqParams, "POST");

                    //Save Request and Response
                    var logResponse = JsonConvert.DeserializeObject<AirtimeRechargeResponse>(IResponse);

                    var RequestResponseLog = new SysRequestResponseLog
                    {
                        Request = Srequest.ToString(),
                        Response = IResponse,
                        DateCreated = DateTime.Now,
                        Message = logResponse.message,
                        StatusCode = logResponse.status,
                        TransactionType = "AirtimeDataRecharge"
                    };

                    await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                    if (!String.IsNullOrEmpty(IResponse))
                    {
                        aregResponse = JsonConvert.DeserializeObject<AirtimeRechargeResponse>(IResponse);

                        if (Convert.ToInt32(aregResponse.status) != 200)
                        {
                            ViewBag.ErrorMessage = "Response " + aregResponse.message;

                            return View(model);
                        }
                    }
                    var product_ = await _unitOfWork.Products.GetProducts(model.ProductID);

                    var Datamodel = new Roots
                    {
                        request_id = Convert.ToInt32(aregResponse.requestID),
                        bundle = model.bundle,
                        product = model.product,
                        phone = model.phone,
                        ProductID = model.ProductID,
                        network = product_.ProductName,
                        ProductModeID = model.ProductModeID,
                        Amount = model.Amount
                    };

                    ViewBag.SuccessMessage = "Data Bundle Vending successful";
                    return RedirectToAction("DataBundleReceipt", "HomeStore", Datamodel);

                }

            }
            string msg = "Recharge Failed - Insufficient Balance";
            ViewBag.SuccessMessage = msg;
            return View(model);
        }


        public async Task<IActionResult> DataBundleReceipt(Roots model)
        {
            ViewBag.Date = DateTime.Now;
            ViewBag.OrderID = model.request_id;
            ViewBag.Amount = model.Amount;
            ViewBag.Network = model.network;
            ViewBag.bundle = model.bundle;
            ViewBag.Phone = model.phone;
            ViewBag.Total = model.Amount;

            //ViewBag.Message = model.message;
            return View();
        }


        //ELECTRICITY ITEXT IMPLEMENTATION

        public async Task<IActionResult> PayWithEWallet(OrdersModel model)
        {
            //checking user's currently logged in
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            TempData["ProductModeID"] = model.ProductModeID;
            ViewBag.ProductID = model.ProductID;

            var getproduct = await _unitOfWork.Products.GetProductInvoiceModeById(model.ProductID);

            var walletBalance = await _unitOfWork.Invoice.GetWalletBalance(user2.Wallet.WalletID);

            var balanceAmount = walletBalance.Sum(u => u.Amount);

            //Check if the balance is sufficient enough to carry out the transaction
            if (balanceAmount > model.Amount)
            {
                var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                string Baseurl = EgolePayServiceConfig.Url;

                if (model.discoCode == "ibedc" || model.discoCode == "jedc")
                {
                    //IBADAN SOAP IMPLEMENTATION
                    string produrl = Baseurl + "/api/Invoice/IBEDCValidation";
                    var IElecResponse = new EData();
                    var Idata = new ITextValidationmodel
                    {
                        vendType = model.vendType,
                        discoCode = model.discoCode,
                        Amount = model.Amount.ToString(),
                        MeterNo = model.MeterNo,
                        ProductID = model.ProductID
                    };

                    string IbodyReqParams = JsonConvert.SerializeObject(Idata, Formatting.Indented);

                    using (var _client = new HttpClient())
                    {
                        var Srequest = (produrl, IbodyReqParams, "POST");
                        string IResponse = General.MakeVFDRequest(produrl, null, "POST", null, IbodyReqParams);
                        var logResponse = JsonConvert.DeserializeObject<EData>(IResponse);

                        var RequestResponseLog = new SysRequestResponseLog
                        {
                            Request = Srequest.ToString(),
                            Response = IResponse,
                            DateCreated = DateTime.Now,
                            Message = logResponse.message,
                            StatusCode = logResponse.responseCode,
                            TransactionType = "Electricity Validation"
                        };

                        await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);
                        if (!String.IsNullOrEmpty(IResponse))
                        {
                            IElecResponse = JsonConvert.DeserializeObject<EData>(IResponse);

                            if (Convert.ToInt32(IElecResponse.responseCode) >= 1)
                            {
                                ViewBag.ErrorMessage = "Response " + IElecResponse.message;
                                var fmodel = new EData()
                                {
                                    responseCode = IElecResponse.responseCode,
                                    message = IElecResponse.message,
                                };
                                return View(fmodel);
                            }
                        }

                        var rmodel = new EData()
                        {
                            responseCode = IElecResponse.responseCode,
                            message = IElecResponse.message,
                            error = IElecResponse.error,
                            customerId = IElecResponse.customerId,
                            name = IElecResponse.name,
                            meterNumber = IElecResponse.meterNumber,
                            accountNumber = IElecResponse.accountNumber,
                            businessUnit = IElecResponse.businessUnit,
                            businessUnitId = IElecResponse.businessUnitId,
                            undertaking = IElecResponse.undertaking,
                            phone = IElecResponse.phone,
                            address = IElecResponse.address,
                            email = IElecResponse.email,
                            lastTransactionDate = IElecResponse.lastTransactionDate,
                            minimumPurchase = IElecResponse.minimumPurchase,
                            customerArrears = IElecResponse.customerArrears,
                            tariff = IElecResponse.tariff,
                            tariffCode = IElecResponse.tariffCode,
                            description = IElecResponse.description,
                            customerType = IElecResponse.customerType,
                            productCode = IElecResponse.productCode,
                            service = model.discoCode,
                            Amount = model.Amount.ToString(),
                            ProductID = model.ProductID.ToString()
                        };
                        ViewBag.SuccessMessage = "Validation successful";
                        return View(rmodel);
                    }
                }

                string producturl = Baseurl + "/api/Invoice/ElectricityValidation"; 

                var ElecResponse = new EData();
                var data = new ITextValidationmodel
                {
                    discoCode = model.discoCode,                 
                    vendType = model.vendType, 
                    Amount = model.Amount.ToString(),
                    MeterNo = model.MeterNo,
                    ProductID = model.ProductID
                };

                var bodyReqParams = JsonConvert.SerializeObject(data, Formatting.Indented);

                //using (var _client = new HttpClient())
                //{
                //var Srequest = (producturl, bodyReqParams, "POST");
                //string IResponse = General.MakeVFDRequest(producturl, null, "POST", null, bodyReqParams);
                //var logResponse = JsonConvert.DeserializeObject<EData>(IResponse);


                var srequest = new HttpRequestMessage(HttpMethod.Post, "");
                var lognResponse = new EData();
                srequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                srequest.Content = new StringContent(bodyReqParams, Encoding.UTF8);
                srequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(producturl);

                    var responses = await httpClient.SendAsync(srequest);
                    var content = await responses.Content.ReadAsStringAsync();
                    //deserialize response to get token
                    lognResponse = JsonConvert.DeserializeObject<EData>(content);

                     

                    var RequestResponseLog = new SysRequestResponseLog
                    {
                        Request = srequest.ToString(),
                        Response = content,
                        DateCreated = DateTime.Now,
                        Message = lognResponse.message,
                        StatusCode = lognResponse.responseCode,
                        TransactionType = "Electricity Validation"
                    };

                    await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);
                    if (!String.IsNullOrEmpty(content))
                    {
                        ElecResponse = JsonConvert.DeserializeObject<EData>(content);

                        if (Convert.ToInt32(ElecResponse.responseCode) >= 1)
                        {
                            ViewBag.ErrorMessage = "Response " + ElecResponse.message;
                            var fmodel = new EData()
                            {
                                responseCode = ElecResponse.responseCode,
                                message = ElecResponse.message,
                            };
                            return View(fmodel);
                        }
                    }

                    var rmodel = new EData()
                    {
                        responseCode = ElecResponse.responseCode,
                        message = ElecResponse.message,
                        error = ElecResponse.error,
                        customerId = ElecResponse.customerId.Remove(0,6) +"asd"+ Helpers.General.randomNumber(),
                        name = ElecResponse.name,
                        meterNumber = ElecResponse.meterNumber,
                        accountNumber = ElecResponse.accountNumber,
                        businessUnit = ElecResponse.businessUnit,
                        businessUnitId = ElecResponse.businessUnitId,
                        undertaking = ElecResponse.undertaking,
                        phone = ElecResponse.phone,
                        address = ElecResponse.address,
                        email = ElecResponse.email,
                        lastTransactionDate = ElecResponse.lastTransactionDate,
                        minimumPurchase = ElecResponse.minimumPurchase,
                        customerArrears = ElecResponse.customerArrears,
                        tariff = ElecResponse.tariff,
                        tariffCode = ElecResponse.tariffCode,
                        description = ElecResponse.description,
                        customerType = ElecResponse.customerType,
                        productCode = ElecResponse.productCode,
                        service = model.discoCode,
                        Amount = model.Amount.ToString(),
                        ProductID = ElecResponse.ProductID
                    };
                    ViewBag.SuccessMessage = "Validation successful";
                    return View(rmodel);
                }
            }
            ViewBag.ErrorMessage = "Insufficient Balance";
            var famodel = new EData()
            {
                responseCode = "99",
                message = "Insufficient Balance",
            };
            return View(famodel);
        }

        [HttpPost]
        public async Task<IActionResult> PayELectricity(EData model)
        {
            //checking user's currently logged in
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            //TempData["ProductModeID"] = model.ProductModeID;
            //ViewBag.ProductID = model.ProductID;

            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
            string Baseurl = EgolePayServiceConfig.Url;


            if (model.service == "ibedc" || model.service == "jedc")
            {
                string produrl = Baseurl + "/api/Invoice/IBEDCPayment";
                //Pass meterNo,AccNo,Amount
                var IElecResponse = new ITextPaymentResponse();
                var Idata = new IBEDCPaymodel
                {
                    AccountNo = model.accountNumber,
                    Amount = model.Amount,
                    MeterNo = model.meterNumber
                };
                string IbodyReqParams = JsonConvert.SerializeObject(Idata, Formatting.Indented);

                using (var _client = new HttpClient())
                {
                    var Srequest = (produrl, IbodyReqParams, "POST");
                    string IResponse = General.MakeVFDRequest(produrl, null, "POST", null, IbodyReqParams);
                    var logResponse = JsonConvert.DeserializeObject<ITextPaymentResponse>(IResponse);

                    var RequestResponseLog = new SysRequestResponseLog
                    {
                        Request = Srequest.ToString(),
                        Response = IResponse,
                        DateCreated = DateTime.Now,
                        Message = logResponse.message,
                        StatusCode = logResponse.responseCode,
                        TransactionType = "Electricity Payment"
                    };

                    await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);
                    if (!String.IsNullOrEmpty(IResponse))
                    {
                        IElecResponse = JsonConvert.DeserializeObject<ITextPaymentResponse>(IResponse);

                        if (Convert.ToInt32(IElecResponse.responseCode) >= 1)
                        {
                            ViewBag.ErrorMessage = "Response " + IElecResponse.message;
                            var fmodel = new ITextPaymentResponse()
                            {
                                responseCode = IElecResponse.responseCode,
                                message = IElecResponse.message,
                            };
                            return View(fmodel);
                        }
                    }

                    var rmodel = new ITextPaymentResponse()
                    {
                        responseCode = IElecResponse.responseCode,
                        message = IElecResponse.message,
                        error = IElecResponse.error,


                    };
                    ViewBag.SuccessMessage = IElecResponse.message;
                    return View(rmodel);
                }


            }


            string producturl = Baseurl + "/api/Invoice/ElectricityPayment";

            var ElecResponse = new ITexPayData();   //ITextPaymentResponse();
            var data = new ITextPaymentmodel
            {
                customerPhoneNumber = model.phone,
                paymentMethod = "cash",
                service = model.service,
                clientReference = model.customerId,
                pin = "",
                productCode = model.productCode,
                userid = user2.UserID,
                ProductID = Convert.ToInt32(model.ProductID), 
                Amount = Convert.ToInt32(model.Amount), 
                card = "", 
            };
             
            //pass correct data

            string bodyReqParams = JsonConvert.SerializeObject(data, Formatting.Indented);

            var Serequest = (producturl, bodyReqParams, "POST");
            var srequest = new HttpRequestMessage(HttpMethod.Post, "");
            var lognResponse = new EData();
            srequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            srequest.Content = new StringContent(bodyReqParams, Encoding.UTF8);
            srequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(producturl);

                var responses = await httpClient.SendAsync(srequest);
                var content = await responses.Content.ReadAsStringAsync();
                 
                var logResponse = JsonConvert.DeserializeObject<ITexPayData>(content);

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Serequest.ToString(),
                    Response = content,
                    DateCreated = DateTime.Now,
                    Message = logResponse.message,
                    StatusCode = logResponse.responseCode,
                    TransactionType = "Electricity Payment"
                };

                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);
                if (!String.IsNullOrEmpty(content))
                {
                    ElecResponse = JsonConvert.DeserializeObject<ITexPayData>(content);

                    if (Convert.ToInt32(ElecResponse.responseCode) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + ElecResponse.message;
                        var fmodel = new ITextPaymentResponse()
                        {
                            responseCode = ElecResponse.responseCode,
                            message = ElecResponse.message,
                        };
                        return View(fmodel);
                    }
                }

                var rmodel = new ITextPaymentResponse()
                {
                    responseCode = ElecResponse.responseCode,
                    message = ElecResponse.message,
                    error = ElecResponse.error, 
                    transactionDate = ElecResponse.transactionDate,
                    transactionReference = ElecResponse.transactionReference, 
                    name = ElecResponse.name,
                    token = ElecResponse.token,
                    address = ElecResponse.address,
                    type = ElecResponse.type,
                    units = ElecResponse.units,
                    sequence = ElecResponse.sequence,
                    reference = ElecResponse.reference, 
                    clientReference = ElecResponse.clientReference,
                    amount = ElecResponse.amount, 

                };
                ViewBag.SuccessMessage = ElecResponse.message;
                return View(rmodel);
            }
        }


        /// <summary>
        /// CABLE TV
        /// </summary>
        /// <returns></returns>


        public async Task<IActionResult> PayCableWallet(OrdersModel model)
        {
            //checking user's currently logged in
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
            string Baseurl = EgolePayServiceConfig.Url;

            string produrl = Baseurl + "/api/Invoice/CableTVVerification";

            var aregResponse = new CableNResponse();

            string prdID = model.ProductID.ToString();
            int prdmodeID = model.ProductModeID;
            int uid = user2.UserID;
            var phone = model.Phone;
            var email = model.Email;

            using (var client = new HttpClient())
            {
                //"CableTVVerification/{AccountNo}/{service}/{productID}
                var Srequest = (Baseurl + "/api/Invoice/CableTVVerification/" + model.AccountNo + "/" + model.ServiceType + "/" + prdID, "POST");
                string vfdEnquiryReqParams = "/api/Invoice/CableTVVerification/" + model.AccountNo + "/" + model.ServiceType + "/" + prdID;
                string IResponse = General.MakeVFDRequest(Baseurl, vfdEnquiryReqParams, "POST");
                 
                //Save Request and Response
                aregResponse = JsonConvert.DeserializeObject<CableNResponse>(IResponse);

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now,
                    Message = aregResponse.errorMessage,
                    StatusCode = aregResponse.status.ToString(),
                    TransactionType = "Cable Verification"
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);


                if (!String.IsNullOrEmpty(IResponse))
                {
                    if (Convert.ToInt32(aregResponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + aregResponse.errorMessage;
                        var fmodel = new NCableDatum()
                        {
                            status = aregResponse.errorMessage
                        };
                        return View(fmodel);
                    }
                }
            }
            var smodel = new NCableDatum();
            var bregResponse = new NCableDatum();
            using (var client = new HttpClient())
            {
                var drequest = (Baseurl + "/api/Invoice/CableTVBouquets/" + model.ServiceType, "POST");
                string vfdEnquiryReqParams = "/api/Invoice/CableTVBouquets/" + model.ServiceType;
                string bResponse = General.MakeVFDRequest(Baseurl, vfdEnquiryReqParams, "POST");

                bregResponse = JsonConvert.DeserializeObject<NCableDatum>(bResponse);
                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = drequest.ToString(),
                    Response = bResponse,
                    DateCreated = DateTime.Now,
                    Message = bregResponse.message,
                    StatusCode = bregResponse.status.ToString(),
                    TransactionType = "Cable TvBouquet"
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                if (!String.IsNullOrEmpty(bResponse))
                {
                    if (Convert.ToInt32(bregResponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + bregResponse.message;
                        var fmodel = new NCableDatum()
                        {
                            status = bregResponse.status,
                            message = bregResponse.message
                        };
                        return View(fmodel);
                    }
                }
                ViewBag.Message = "Response " + bregResponse.message;
                long transid = General.randomNumber();
                smodel = new NCableDatum()
                {
                    status = bregResponse.status,
                    message = bregResponse.message,
                    availablePricingOptions = bregResponse.availablePricingOptions,
                    code = bregResponse.code,
                    data = bregResponse.data,
                    description = bregResponse.description,
                    name = aregResponse.name,
                    msisdn = phone,
                    email = email,
                    service_type = model.ServiceType,
                    account_ID =aregResponse.accountNumber,  // model.AccountNo,
                    trans_id = Convert.ToString(transid),
                    prdID = model.ProductID.ToString()
                };
            }
            return View(smodel);
        }

        public async Task<IActionResult> CablePayment(NCableDatum model)
        {
            //checking user's currently logged in
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
            string Baseurl = EgolePayServiceConfig.Url;

            string produrl = Baseurl + "/api/Invoice/CablePayment/";

            var cregResponse = new UCabletRootReceipt();   //CapayResponse();
            var data = new NCardCablePayInput
            {
                accountName = model.name,
                amount = Convert.ToInt32(model.amount), 
                ProductID = Convert.ToInt32(model.prdID),
                userid = user2.UserID.ToString(),
                addOnCode="",
                narration="",
                subscriberId=model.account_ID,
                productCode=model.code,
                providerCode=model.service_type,
                quantity=1,
                transactionId=model.trans_id,
                transactionPin=""

            };

            string bodyReqParams = JsonConvert.SerializeObject(data, Formatting.Indented); //model.ReferenceNo;

            using (var _client = new HttpClient())
            {
                var Srequest = (produrl, bodyReqParams, "POST");
                string IResponse = General.MakeVFDRequest(produrl, null, "POST", null, bodyReqParams);
                // cregResponse = JsonConvert.DeserializeObject<CapayResponse>(IResponse);
                cregResponse = JsonConvert.DeserializeObject<UCabletRootReceipt>(IResponse);

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now,
                    Message = cregResponse.message,
                    StatusCode = cregResponse.statusCode,
                    TransactionType = "Cable Payment"
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    if (Convert.ToInt32(cregResponse.statusCode) >= 1 || cregResponse.statusCode==null)
                    {
                        ViewBag.ErrorMessage = "Response " + cregResponse.message;
                        var fmodel = new UCabletRootReceipt()  //NCableDatum()
                        {
                            statusCode = cregResponse.statusCode,
                            message = cregResponse.message,
                            transactionId= cregResponse.transactionId,
                            subscriberId=cregResponse.subscriberId,
                            accountName=cregResponse.accountName,
                            amount=cregResponse.amount,
                            narration=cregResponse.narration,
                            providerCode=cregResponse.providerCode
                        };
                        return View(fmodel); 
                    }
                }
            }
            var cmodel = new UCabletRootReceipt() //NCableDatum()
            {
                statusCode = cregResponse.statusCode, 
                message = cregResponse.message,
                transactionId = cregResponse.transactionId,
                subscriberId = cregResponse.subscriberId,
                accountName = cregResponse.accountName,
                amount = cregResponse.amount,
                narration = cregResponse.narration,
                providerCode = cregResponse.providerCode
            };
            //return View(cmodel);
            return RedirectToAction("PrintCableReceipt", cmodel);
        }

        public IActionResult PrintCableReceipt(UCabletRootReceipt model)
        {
            ViewBag.Date = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            ViewBag.TotalAmount = model.amount;
            ViewBag.Amount = model.amount;
            ViewBag.AmountRate = 0;
            ViewBag.providerCode = model.providerCode;
            ViewBag.narration = model.narration;
            ViewBag.accountName = model.accountName;
            ViewBag.subscriberId = model.subscriberId;
            ViewBag.transactionId = model.transactionId;
            ViewBag.message = model.message;
            ViewBag.statusCode = model.statusCode;

            return View();
        }

        //EDUCATION JAMB/WAEC

        public async Task<IActionResult> PayWaecWallet(OrdersModel model)
        {
            //checking user's currently logged in
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
            string Baseurl = EgolePayServiceConfig.Url;

            string produrl = Baseurl + "/api/Invoice/WaecLookUp";

            var aregResponse = new SShagoWaecRoot();


            var request = new HttpRequestMessage(HttpMethod.Post, "");

            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(produrl); 

                var responses = await httpClient.SendAsync(request);
                var content = await responses.Content.ReadAsStringAsync();
               
                //Save Request and Response
                aregResponse = JsonConvert.DeserializeObject<SShagoWaecRoot>(content);

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = produrl.ToString(),
                    Response = content,
                    DateCreated = DateTime.Now,
                    Message = aregResponse.message,
                    StatusCode = aregResponse.status.ToString(),
                    TransactionType = "WAEC Verification"
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                int pamount=0;int quantity = 0;
                 
                if (!String.IsNullOrEmpty(content))
                {
                    if (Convert.ToInt32(aregResponse.status) == 200)
                    {
                        foreach  (var item in aregResponse.product)
                        {
                            pamount = item.price;
                            quantity = item.availableCount;
                        }

                        ViewBag.ErrorMessage = "Response " + aregResponse.message;
                        var fmodel = new OrdersModel()
                        { 
                            StatusMessage = aregResponse.message,
                            Amount = (model.NumberofPins * pamount),
                            QuantityAvailable= quantity.ToString(),
                            //ServiceNumber = aregResponse.type,
                            ProductID=model.ProductID,
                            NumberofPins= model.NumberofPins,
                            ProductName = aregResponse.type,

                        };
                        return View(fmodel);
                    } 
                }
                ViewBag.ErrorMessage = "Response " + aregResponse.message;
                var failedmodel = new OrdersModel()
                {
                    StatusMessage = aregResponse.message,
                    //Amount = (model.NumberofPins * pamount),
                    QuantityAvailable = quantity.ToString(),
                    ProductName = model.ProductName,
                     ProductID = model.ProductID
                };
                return View(failedmodel); 
            } 
           /// return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PaywithWaecWallet(OrdersModel model)
        {
            //checking user's currently logged in
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
            string Baseurl = EgolePayServiceConfig.Url;

            string produrl = Baseurl;  
            var aregResponse = new SPinWResponse();
 

            using (var client = new HttpClient())
            {
                var Srequest = (produrl + model.NumberofPins + "/" + model.Amount + "/" + model.ProductID + "/" + user2.UserID, "POST");
                string vfdEnquiryReqParams = "/api/Invoice/WaecPurchase/" + model.NumberofPins + "/" + model.Amount + "/" + model.ProductID + "/" + user2.UserID;

                string IResponse = General.MakeVFDRequest(Baseurl, vfdEnquiryReqParams, "POST");

                //Save Request and Response
                aregResponse = JsonConvert.DeserializeObject<SPinWResponse>(IResponse);

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now,
                    Message = aregResponse.message,
                    StatusCode = aregResponse.status.ToString(),
                    TransactionType = "WAEC PIN Purchase"
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                int pamount = 0; int quantity = 0;

                if (!String.IsNullOrEmpty(IResponse))
                {
                    if (Convert.ToInt32(aregResponse.status) == 200)
                    { 

                        ViewBag.ErrorMessage = "Response " + aregResponse.message;
                        var fmodel = new SPinWResponse()
                        {
                        serial=aregResponse.serial,
                        status=aregResponse.status,
                        amount=aregResponse.amount,
                        date=aregResponse.date,
                        expirydat=aregResponse.expirydat,
                        message=aregResponse.message,
                        pin=aregResponse.pin,
                         transId = aregResponse.transId
                        };
                        // return View(fmodel);
                        return RedirectToAction("PrintWaecReceipt", fmodel);
                    }
                }
                ViewBag.ErrorMessage = "Response " + aregResponse.message;
                var failedmodel = new SPinWResponse()
                {
                    status = aregResponse.status,
                    message = aregResponse.message,
                };
                ViewBag.Message = aregResponse.message;
                return View(failedmodel);
            } 
        }

        public IActionResult PrintWaecReceipt(SPinWResponse model)
        {
            ViewBag.Date = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");//
            ViewBag.TotalAmount = model.amount;
            ViewBag.Amount = model.amount;
            ViewBag.AmountRate = 0;
            ViewBag.serial = model.serial;//
            ViewBag.status = model.status;
            ViewBag.message = model.message;//
            ViewBag.Pin = model.pin;
            ViewBag.transactionId = model.transId; //

            return View();
        }

         
        public IActionResult Reprint()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Reprint(ReprintModel model)
        {

            if (model.refno == null)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                ViewBag.Error = "Please fill in the required fields.";
                return View(model);
            }
            var refcheck = await _unitOfWork.Invoice.GetInvoicesReference(model.refno);

            if (refcheck == null)
            {
                ViewBag.Error = "Reference Number doesn't exist.";
                return View(model);
            }

            double totalamount = 0;
            double rate = 0;
            string transdate = "";
            string transtime = "";
            string createdby = ""; string cemail = ""; string cname = ""; string regNo = "";

            foreach (var item in refcheck)
            {
                if (item.ServiceNumber.Contains("FEE") || item.ServiceNumber.Contains("OYCH01"))
                {
                    rate = item.Amount;
                }
                else
                {
                    totalamount += item.Amount;
                    transdate = item.PaymentDate.ToString("dddd, dd MMMM yyyy");
                    transtime = item.PaymentDate.ToString("HH:mm");
                    createdby = item.CreatedBy.ToString();
                    cname = item.CustomerName;
                    cemail = item.CustomerEmail;
                    regNo = item.RegistrationNo;
                }

            }
            var generatedBy = await _unitOfWork.User.GetSysUsers(createdby);

            var rmodel = new ReprintModel()
            {
                TotalAmount = (totalamount + rate),
                Amount = totalamount,
                AmountRate = rate,
                refno = model.refno,
                Name = cname,
                RegistrationNo = regNo,
                Email = cemail,
                GeneratedBy = generatedBy.FirstName + " " + generatedBy.LastName,
                TransactionDate = transdate,
                TransactionTime = transtime
            };



            return RedirectToAction("PrintReceipt", "HomeStore", rmodel);
            //return View(rmodel);
        }

        public IActionResult PrintReceipt(ReprintModel model)
        {
            ViewBag.TotalAmount = model.TotalAmount;
            ViewBag.Amount = model.Amount;
            ViewBag.AmountRate = model.AmountRate;
            ViewBag.refno = model.refno;
            ViewBag.Name = model.Name;
            ViewBag.RegistrationNo = model.RegistrationNo;
            ViewBag.Email = model.Email;
            ViewBag.GeneratedBy = model.GeneratedBy;
            ViewBag.TransactionDate = model.TransactionDate;
            ViewBag.TransactionTime = model.TransactionTime;
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ProductInfoCollection(int? id, int? productid, string productName = "")
        {
            //ViewBag.Message = "This the value " + IsFixedAmount.ToString();
            //ViewBag.Products = await _unitOfWork.Products.GetAllProducts();
            TempData["ProductID"] = productid.ToString();
            ViewBag.ProductItemList = await ProductItemList(Convert.ToInt32(TempData["ProductID"].ToString()));
            //TempData["ProductID"] = id.ToString();
            TempData["InvoiceModeId"] = id.ToString();
            TempData["WalletTrans"] = "False";

            ViewBag.productid = productid;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductInfoCollection(OrderDetailsModel model, string card, string wallet)
        {
            //ViewBag.ProductItemList = await ProductItemList(Convert.ToInt32(TempData["ProductID"].ToString()));

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }
            //Convert.ToInt32(TempData["ProductID"].ToString())
            //var product = await _unitOfWork.Products.GetProducts(Convert.ToInt32(model.ProductID)); //ProductID was null hence the new method below

            TempData["WalletTrans"] = "False";

            if (wallet != null)
            {
                string ProductModeID = "";

                TempData["ProductModeID"] = model.ProductModeID;
                ProductModeID = model.ProductModeID.ToString();
                TempData["RefID"] = model.ProductItemID;
                TempData["ProductItemID"] = model.ProductItemID;
                TempData["Email"] = model.Email;
                TempData["PhoneNumber"] = model.PhoneNumber;
                TempData["RegNumber"] = model.RegNumber;
                TempData["AmountRate"] = model.AmountRate;
                TempData["ReferenceNo"] = model.ReferenceNo;
                //
                TempData["ProductID"] = model.ProductID;

                return Redirect("~/Identity/Account/Login?loginType=" + model.ProductModeID.ToString() + "&productid=" + model.ProductID.ToString());
                //return Redirect("~/Identity/Account/Login?=productid" + model.ProductID + "loginType=" + model.ProductModeID.ToString());
                //return Redirect("~/Identity/Account/Login?=productid" + model.ProductID + "loginType=" + model.ProductModeID.ToString());
                //return Redirect("~/Identity/Account/Login/productid" + model.ProductID + "loginType=" + model.ProductModeID.ToString());

            }

            if (card != null)
            {
                //Checking if the visitor user already exist 
                var checkExistingUser = await _unitOfWork.User.CheckExistingUser(model.PhoneNumber, model.Email);

                //If user exists
                if (checkExistingUser)
                {
                    //Using the ProductItemID to fetch the product details
                    var productexistUser = await _unitOfWork.Products.GetProductExtension(model.ProductItemID);

                    var getExistingUserDetails = await _unitOfWork.User.GetExistingUser(model.Email, model.PhoneNumber);

                    var invoiceExistUser = new EgsInvoice
                    {
                        InvoiceID = model.InvoiceID,
                        Amount = model.AmountRate,
                        Product = productexistUser.Product,
                        ProductItemID = model.ProductItemID,
                        ReferenceNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8),//General.GetInvoiceRef(Guid.NewGuid().ToString()),//.Substring(0, 5)
                        DateCreated = DateTime.Now,
                        CreatedBy = getExistingUserDetails.UserID,
                        ServiceNumber = model.RegNumber
                    };
                    await _unitOfWork.Invoice.AddSaveAsync(invoiceExistUser);

                    //var product = await _unitOfWork.Products.GetProducts(Convert.ToInt32(model.ProductID));
                    var invExistUser = await _unitOfWork.Invoice.GetInvoice(Convert.ToInt32(model.InvoiceID));
                    var transTypeExistUser = _unitOfWork.TransactionType.Get(2); //2 is for a debit transaction type
                    var prodItemExistUser = await _unitOfWork.ProductItem.GetProductItems(Convert.ToInt32(model.ProductItemID));


                    ViewBag.Message = "Invoice has been created successfully";
                    ViewBag.ProductItemList = await ProductItemList(Convert.ToInt32(model.ProductID));
                    //return View();
                    //TempData["ProductItemName"] = model.ProductItemName;
                    //TempData["ProductItemID"] = model.ProductItemID;
                    //TempData["ProductName"] = model.ProductName;
                    //TempData["ProductName"] = productexistUser.Product.ProductID;
                    //TempData["ProductID"] = model.ProductID;
                    //TempData["ProductID"] = productexistUser.Product.ProductName;
                    //TempData["RegNumber"] = model.RegNumber;
                    //TempData["Rate"] = model.AmountRate;
                    //TempData["invRefNo"] = invoiceExistUser.ReferenceNo;
                    //TempData["Email"] = model.Email;
                    //TempData["PhoneNumber"] = model.PhoneNumber;
                    //TempData["InvoiceID"] = invoiceExistUser.InvoiceID.ToString();
                    //return RedirectToAction("ConfirmDetails", "HomeStore", new {prct = model.ProductName});
                    //return RedirectToAction("ConfirmDetails", "HomeStore", new { prct = product.Product.ProductName });
                    return RedirectToAction("ConfirmDetails", "HomeStore", new { prct = invoiceExistUser.ReferenceNo });
                }

                #region "If the User does not exist"

                //Set up a user account for the user attempting to buy without logging in
                //Get the role for General
                var role = await _roleManager.Roles.Where(r => r.Id == "26d4e7a8-225c-4739-8230-663706d6cabc").SingleOrDefaultAsync();

                var roleDet = _unitOfWork.Role.Get((int)role.RoleID);

                //Getting the Account Type for Individual
                var userType = _unitOfWork.UserType.Get(1);

                var sysUser = new SysUsers
                {
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Role = roleDet,
                    UserType = userType,
                    IsActive = false,
                    IsVerified = false,
                    DateCreated = DateTime.Now,
                    IsSignUp = false,

                };
                await _unitOfWork.CreateUser.AddAsync(sysUser);
                _unitOfWork.Complete();

                //Using the ProductItemID to fetch the product details
                var product = await _unitOfWork.Products.GetProductExtension(model.ProductItemID);

                var invoice = new EgsInvoice
                {
                    InvoiceID = model.InvoiceID,
                    Amount = model.AmountRate,
                    Product = product.Product,
                    ProductItemID = model.ProductItemID,
                    ReferenceNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8),//General.GetInvoiceRef(Guid.NewGuid().ToString()),//.Substring(0, 5)
                    DateCreated = DateTime.Now,
                    CreatedBy = sysUser.UserID,
                    ServiceNumber = model.RegNumber
                };
                await _unitOfWork.Invoice.AddSaveAsync(invoice);

                //var product = await _unitOfWork.Products.GetProducts(Convert.ToInt32(model.ProductID));
                var inv = await _unitOfWork.Invoice.GetInvoice(Convert.ToInt32(model.InvoiceID));
                var transType = _unitOfWork.TransactionType.Get(2); //2 is for a debit transaction type
                var prodItem = await _unitOfWork.ProductItem.GetProductItems(Convert.ToInt32(model.ProductItemID));

                //var strTransaction = new EgsStoreTransaction
                //{
                //    Invoices = inv,
                //    Amount = model.AmountRate,
                //    ProductItem = prodItem,
                //    TransactionType = transType,
                //    TransactionReferenceNumber = model.ReferenceNo,
                //    TransactionDate = DateTime.Now,
                //    RegistrationNumber = model.RegNumber,
                //    PhoneEmail = model.PhoneEmail
                //};

                //await _unitOfWork.StoreTransaction.AddSaveAsync(strTransaction);


                ViewBag.Message = "Invoice has been created successfully";
                ViewBag.ProductItemList = await ProductItemList(Convert.ToInt32(model.ProductID));
                //return View();
                //TempData["ProductItemName"] = model.ProductItemName;
                //TempData["ProductItemID"] = model.ProductItemID;
                //TempData["ProductName"] = model.ProductName;
                //TempData["ProductName"] = product.Product.ProductID;
                //TempData["ProductID"] = model.ProductID;
                //TempData["ProductID"] = product.Product.ProductName;
                //TempData["RegNumber"] = model.RegNumber;
                //TempData["Rate"] = model.AmountRate;
                //TempData["invRefNo"] = invoice.ReferenceNo;
                //TempData["Email"] = model.Email;
                //TempData["PhoneNumber"] = model.PhoneNumber;
                //TempData["InvoiceID"] = invoice.InvoiceID.ToString();
                //return RedirectToAction("ConfirmDetails", "HomeStore", new {prct = model.ProductName});
                //return RedirectToAction("ConfirmDetails", "HomeStore", new { prct = product.Product.ProductName });
                return RedirectToAction("ConfirmDetails", "HomeStore", new { prct = invoice.ReferenceNo });

                #endregion
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InvoiceVerification(OrderDetailsModel model, string wallet)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                //return View();
                return RedirectToAction("ProductInfoCollection", "HomeStore");
            }

            if (wallet != null)
            {
                TempData["ProductItemID"] = model.ProductItemID;
                TempData["RegNumber"] = model.RegNumber;
                TempData["AmountRate"] = model.AmountRate;
                TempData["Email"] = model.Email;
                TempData["PhoneNumber"] = model.PhoneNumber;
                return Redirect("~/Identity/Account/Login?loginType=" + model.ProductModeID.ToString());
            }

            // do your server-side processing and get your data

            if (model.ReferenceNo != null)
            {
                return RedirectToAction("InvoiceConfirmation", "HomeStore", new { ref_ = model.ReferenceNo });

            }
            ModelState.AddModelError("", "Please fill in the required fields.");

            return RedirectToAction("ProductInfoCollection", "HomeStore");
        }

        [HttpPost]
        public IActionResult InvoicePVerification(OrderDetailsModel model, string wallet)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return RedirectToAction("ProductInfoCollection", "HomeStore");
            }

            if (wallet != null)
            {
                string ProductModeID = "";

                TempData["ProductModeID"] = model.ProductModeID;
                ProductModeID = model.ProductModeID.ToString();
                TempData["ProductItemID"] = model.ProductItemID;
                TempData["RegNumber"] = model.RegNumber;
                TempData["AmountRate"] = model.AmountRate;
                TempData["Email"] = model.Email;
                TempData["PhoneNumber"] = model.PhoneNumber;
                TempData["ProductID"] = model.ProductID;
                //return Redirect("~/Identity/Account/Login?loginType=3");
                return Redirect("~/Identity/Account/Login?loginType=" + model.ProductModeID.ToString() + "&productid=" + model.ProductID.ToString());
            }

            if (model.ReferenceNo != null)
            {
                return RedirectToAction("InvoicePConfirmation", "HomeStore", new { ref_ = model.ReferenceNo });
            }

            return RedirectToAction("ProductInfoCollection", "HomeStore");

        }


        [HttpGet]
        public async Task<IActionResult> InvoicePConfirmation(string ref_)
        {

            var refDetail = await _unitOfWork.Invoice.GetReferenceNo(ref_);

            var getproduct = await _unitOfWork.Products.GetProducts(refDetail.Product.ProductID);

            string productUrl = getproduct.Verification;

            var clientUrl = productUrl + ref_;
            var aregResponse = new AutoRegResponse();

            var client = new RestClient(clientUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            var response = client.Execute(request);

            //getting the response string 
            var getcontent = response.Content.ToString();

            //Save Request and Response
            var RequestResponseLog = new SysRequestResponseLog
            {
                Request = request.ToString(),
                Response = getcontent,
                DateCreated = DateTime.Now
            };
            await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

            aregResponse = JsonConvert.DeserializeObject<AutoRegResponse>(getcontent);

            var gettransactionfee = await _unitOfWork.Products.GetTransactionfeebyProductCode("TRF001");
            var productitem = await _unitOfWork.ProductItem.GetProductItem(gettransactionfee.ProductID);
            var getproductrate = await _unitOfWork.ProductItemRate.GetItemRateByProductItemID(productitem.ProductItemID);

            var allmodel = new ERegInvoicesViewModel
            {
                status = Convert.ToInt32(aregResponse.status),
                message = aregResponse.message,
                custReference = aregResponse.customerInformationResponse.custReference,
                customerReferenceAlternate = aregResponse.customerInformationResponse.customerReferenceAlternate,
                firstName = aregResponse.customerInformationResponse.firstName,
                lastName = aregResponse.customerInformationResponse.lastName,
                otherName = aregResponse.customerInformationResponse.otherName,
                email = aregResponse.customerInformationResponse.email,
                phone = aregResponse.customerInformationResponse.phone,
                thirdPartyCode = aregResponse.customerInformationResponse.thirdPartyCode,
                agentName = aregResponse.customerInformationResponse.agentName,
                branchName = aregResponse.customerInformationResponse.branchName,
                registrationNo = aregResponse.customerInformationResponse.registrationNo,
                //items = aregResponse.customerInformationResponse.paymentItems.items,
                totalAmount = Convert.ToDouble(aregResponse.customerInformationResponse.totalAmount),
                TransactionFee = getproductrate.AmountRate

            };

            return View(allmodel);
        }

        public async Task<IActionResult> BuyPowerPayment(EBuyPowerPayment model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return RedirectToAction("ProductInfoCollection", "HomeStore");
            }

            ViewBag.ProductID = model.ProductID;
            var getproduct = await _unitOfWork.Products.GetProductInvoiceModeById(model.ProductID);
            var productbaseurl = getproduct.ActionUrl;

            var authKey = getproduct.APIKey;
            var Baseurl = productbaseurl;
            //passing model to postdat
            var postdata = new EBuyPowerPaymentPostAPI
            {
                meter = model.meterNo,
                disco = model.discoCode,
                vendType = model.vendType,
                orderId = model.orderId,
                paymentType = model.paymentType,
                phone = model.phone,
                email = model.email,
                name = model.name,
                amount = model.amount
            };

            //Serializing postdata
            var inputs = JsonConvert.SerializeObject(postdata);
            var request = new HttpRequestMessage(HttpMethod.Post, "");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(inputs, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authKey);

                var responses = await httpClient.SendAsync(request);
                var content = await responses.Content.ReadAsStringAsync();
                //responses.EnsureSuccessStatusCode();
                //Save Request and Response
                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = request.ToString(),
                    Response = content,
                    DateCreated = DateTime.Now
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);


                if (responses.IsSuccessStatusCode == true)
                {
                    //
                    var ViewModel = new OrdersModel();

                    //int prdModeId = model.ProductModeID;

                    //checking user's currently logged in
                    var user = await _userManager.GetUserAsync(User);
                    var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

                    //Check the Wallet balance of the user logged in of all TransactionType(1) 
                    var walletBalance = await _unitOfWork.Invoice.GetWalletBalance(user2.Wallet.WalletID);

                    //Getting the accrued balance of all transaction type(1) - Credit returned
                    //var balanceAmount = 2000000;//
                    var balanceAmount = walletBalance.Sum(u => u.Amount);

                    //Check if the balance is sufficient enough to carry out the transaction
                    if (balanceAmount > model.amount)
                    {
                        var genReferenceNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
                        var genTransReferenceNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
                        var genSalesReferenceNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);

                        var getproductitem = await _unitOfWork.ProductItem.GetProductItem(model.ProductID);
                        var productUser = await _unitOfWork.Products.GetProductExtension(getproductitem.ProductItemID);
                        var gettransactionfee = await _unitOfWork.Products.GetTransactionfeebyProductCode("TRF001");
                        var productitem = await _unitOfWork.ProductItem.GetProductItem(gettransactionfee.ProductID);
                        var getproductrate = await _unitOfWork.ProductItemRate.GetItemRateByProductItemID(productitem.ProductItemID);

                        var _productBasisInterval = await _unitOfWork.SettlementBasis.GetSettlementBasisByProductID(model.ProductID);
                        var _interval = await _unitOfWork.SettlementInterval.GetSettlementIntervalById(_productBasisInterval.SettlementIntervalID);
                        DateTime settlementDate = new DateTime();

                        if (_interval.SettlementIntervalName == "Instant")
                        {
                            var nextDay = DateTime.Now.AddMinutes(10);
                            settlementDate = nextDay;
                        }
                        else if (_interval.SettlementIntervalName == "Daily")
                        {
                            var nextDay = DateTime.Now.NextDay();
                            settlementDate = nextDay;
                        }
                        else if (_interval.SettlementIntervalName == "Weekly")
                        {
                            var firstdayOfThisWeek = DateTime.Now.FirstDayOfWeek();
                            var lastDayOfWeek = DateTime.Now.LastDayOfWeekWithD1();
                            settlementDate = lastDayOfWeek;
                        }
                        else if (_interval.SettlementIntervalName == "Monthly")
                        {
                            var firstDayOfThisMonth = DateTime.Now.FirstDayOfMonth();
                            var firstDayOfNextMonth = DateTime.Now.LastDayOfMonth().AddDays(1);
                            settlementDate = firstDayOfNextMonth;
                        }

                        var invoiceUser = new EgsInvoice
                        {
                            //InvoiceID = model.InvoiceID,
                            Amount = model.amount,
                            Product = productUser.Product,
                            ProductItemID = getproductitem.ProductItemID,
                            //ServiceNumber = model.RegNumber,
                            PaymentStatus = true,
                            PaymentDate = DateTime.Now,
                            //ServiceNumber = productUser.Product.ProductCode,
                            ReferenceNo = genReferenceNo,
                            DateCreated = DateTime.Now,
                            CreatedBy = user2.UserID
                        };
                        await _unitOfWork.Invoice.AddSaveAsync(invoiceUser);

                        var TransactionFeeinvoice = new EgsInvoice
                        {
                            Amount = getproductrate.AmountRate,
                            ReferenceNo = genReferenceNo,
                            Product = gettransactionfee,
                            ProductItemID = productitem.ProductItemID,
                            //AlternateReferenceNo = ,
                            DateCreated = DateTime.Now,
                            CreatedBy = user2.UserID,
                            ServiceNumber = gettransactionfee.ProductCode
                        };
                        await _unitOfWork.Invoice.AddAsync(TransactionFeeinvoice);

                        ///_unitOfWork.Complete();

                        var getInvoiceDetails = await _unitOfWork.Invoice.GetReferenceNo(genReferenceNo);
                        var transactionType = _unitOfWork.TransactionType.Get(2);

                        //Creating a record in WalletTransaction of TransactionType(2) - Debit
                        var walletTransaction = new EgsWalletTransaction
                        {
                            Wallet = user2.Wallet,
                            //ProductItem = _unitOfWork.ProductItem.Find(x => x.ProductItemID == getInvoiceDetails.ProductItemID).FirstOrDefault(),
                            TransactionType = transactionType,
                            Amount = getInvoiceDetails.Amount * (-1),
                            WalletReferenceNumber = genTransReferenceNo,
                            TransactionReferenceNo = genReferenceNo,
                            Invoices = _unitOfWork.Invoice.Find(x => x.InvoiceID == getInvoiceDetails.InvoiceID).FirstOrDefault(),
                            TransactionDate = DateTime.Now
                        };
                        await _unitOfWork.WalletTransaction.AddSaveAsync(walletTransaction);
                        //Creating a record in Sales 
                        var sales = new EgsSales
                        {
                            WalletTransactionID = walletTransaction.WalletTransactionID,
                            IsSettled = false,
                            DiscountedAmount = getInvoiceDetails.Amount,
                            TransactionReferenceNumber = genReferenceNo,
                            SalesReferenceNumber = genSalesReferenceNo,
                            SettlementDate = settlementDate,
                            CreatedBy = user2.UserID,
                            DateCreated = DateTime.Now
                        };
                        await _unitOfWork.Sales.AddSaveAsync(sales);


                        var walletproductitems = _unitOfWork.ProductItem.Find(x => x.ProductItemID == productitem.ProductItemID).FirstOrDefault();


                        var secTransaction = new EgsWalletTransaction
                        {
                            Wallet = user2.Wallet,
                            ProductItem = walletproductitems,
                            TransactionType = transactionType,
                            Amount = getproductrate.AmountRate * (-1),
                            WalletReferenceNumber = genTransReferenceNo,
                            TransactionReferenceNo = genReferenceNo,
                            Invoices = TransactionFeeinvoice,
                            TransactionDate = DateTime.Now
                        };
                        await _unitOfWork.WalletTransaction.AddSaveAsync(secTransaction);


                        var _sales = new EgsSales
                        {
                            WalletTransactionID = secTransaction.WalletTransactionID,
                            IsSettled = false,
                            DiscountedAmount = getproductrate.AmountRate,
                            TransactionReferenceNumber = genReferenceNo,
                            SalesReferenceNumber = genSalesReferenceNo,
                            CreatedBy = user2.UserID,
                            SettlementDate = settlementDate,
                            DateCreated = DateTime.Now,
                            ProductItem = walletproductitems,
                            Product = gettransactionfee
                        };
                        await _unitOfWork.Sales.AddSaveAsync(_sales);

                        //invoice.PaymentStatus = true;
                        //invoice.PaymentReference = genReferenceNo;
                        //invoice.PaymentDate = DateTime.Now;
                        //_unitOfWork.Invoice.UpdateInvoice(invoice);

                        _unitOfWork.Complete();


                        return RedirectToAction("BuyPowerTransaction", "HomeStore", new { orderId = postdata.orderId, prdid = model.ProductID });
                    }

                }
                else if (responses.IsSuccessStatusCode != true)
                {
                    ModelState.AddModelError("", responses.ReasonPhrase);
                    return RedirectToAction("ProductInfoCollection", "HomeStore");

                    //ViewBag.error 
                }
            }

            return View("BuyPowerPayment", model);
        }


        public async Task<IActionResult> BuyPowerTransaction(string orderid, int prdid)
        {
            ViewBag.OrderID = orderid;
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            if (orderid != null)
            {
                var getproduct = await _unitOfWork.Products.GetProductInvoiceModeById(prdid);
                var productbaseurl = getproduct.Verification;
                var Baseurl = productbaseurl;

                //var authKey = _configuration.GetValue<string>("AuthKey");
                var authKey = getproduct.APIKey;
                //var Baseurl = _configuration.GetSection("BuyPower:Query").Value;

                var clientUrl = (Baseurl + orderid);

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authKey);
                string json = await client.GetStringAsync(clientUrl);

                //Save Request and Response
                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = clientUrl.ToString(),
                    Response = json,
                    DateCreated = DateTime.Now
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                Root ereg = JsonConvert.DeserializeObject<Root>(json);

                ViewBag.id = ereg.result.data.id;
                ViewBag.orderid = ereg.result.data.orderId;
                ViewBag.receiptNo = ereg.result.data.receiptNo;
                ViewBag.disco = ereg.result.data.disco;
                ViewBag.vendRef = ereg.result.data.vendRef;
                ViewBag.vendTime = ereg.result.data.vendTime;
                ViewBag.vendAmount = ereg.result.data.vendAmount;
                ViewBag.units = ereg.result.data.units;

                ViewBag.totalAmountPaid = ereg.result.data.totalAmountPaid;
                ViewBag.token = ereg.result.data.token;
                ViewBag.debtAmount = ereg.result.data.debtAmount;
                ViewBag.debtRemaining = ereg.result.data.debtRemaining;
                ViewBag.amountGenerated = ereg.result.data.amountGenerated;
                // ViewBag.responseCode = ereg.result.data.responseCode;
                ViewBag.responseMessage = ereg.result.data.responseMessage;
                ViewBag.tax = ereg.result.data.tax;
                //"" ./uii/logos/EgolePay.png
                string msg = "<p>Dear " + user2.FirstName + ",<br/>Thank you for shopping on EgolePay<br/>" +
                    "Kindly find below your transaction receipt for about the product(s) your ordered!</p><br/>" +
                "<div style='box-shadow: 0 0 1in -0.25in rgba(0, 0, 0, 0.5);padding: 2mm; margin:0 auto;width: 44mm;background: #FFF;'>" +
                 "<div>" +
               "<div> <img src ='@Url.Content('~/uii/logos/EgolePay.png')'/> </div>" +
                "<div style='text-align:center'><h2>EGOLEPAY</h2></div> " +
                "</div> " +
                "<div>" +
                "<div style='display: block;;margin-left: 0;'><h3>RECEIPT DETAILS</h3>" +
                "<p><b>Order ID:</b>" + ereg.result.data.orderId + "</p> " +
                "<p><b>ReceiptNo:</b>" + ereg.result.data.receiptNo + "</p>" +
                "<p><b>Disco:</b>" + ereg.result.data.disco + "</p>" +
                "<p><b>VendRef:</b>" + ereg.result.data.vendRef + "</p>" +
                "<p><b>VendTime:</b>" + ereg.result.data.vendTime + "</p>" +
                "<p><b>VendAmount:</b> <span>&#8358;</span>" + ereg.result.data.vendAmount + "</p>" +
                "<p><b>Units:</b>" + ereg.result.data.units + "</p>" +
                "<p><b>TotalAmountPaid:</b> <span>&#8358;</span>" + ereg.result.data.totalAmountPaid + "</p>" +
                "<p><b>Token:</b>" + ereg.result.data.token + "</p>" +
                "<p><b>DebtAmount:</b>" + ereg.result.data.debtAmount + "</p>" +
                "<p><b>debtRemaining:</b>" + ereg.result.data.debtRemaining + "</p>" +
                "<p><b>Status:</b>" + ereg.result.data.responseMessage + "</p>" + "</div></div></div>" +
                 "<br /><strong>Thank you for your business!</a></b>";

                //Charles.obika@courtevillegroup.com-- user2.Email   //<img src='@Url.Content('~/uii/logos/EgolePay.png')' alt='' style='width:60px; height:60px'/>
                await _emailSender.SendEmailAsync(user2.Email, "EGOLEPAY RECEIPT", msg);


                return View();
            }
            ViewBag.Success = "Payment Successful,See transaction details below";
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> PayWithWallet(int? prdid, string prdItem, string phone, string ReferenceNo, string prdMode, string uEmail)
        {
            EgsAuditTrail audit_;
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var uemail = user2.Email;
            if (!string.IsNullOrEmpty(ReferenceNo))
            {
                OrdersModel model = new OrdersModel();

                var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                string Baseurl = EgolePayServiceConfig.Url;

                var referNo = ReferenceNo.Trim().TrimEnd().TrimStart();
                var productID = prdid;
                var productItemCode = prdItem;
                //string vfdEnquiryReqParams = ReferenceNo;
                var aregResponse = new AutoDResponse();
                //?prditemcode=
                using (var client = new HttpClient())
                {
                    if (productItemCode == null)
                    {
                        var Srequest = (Baseurl + "/api/Invoice/ProcessInvoiceR/" + referNo + "/" + productID + "/" + uemail, "GET");//+ "/" + uEmail    + "/" + uEmail
                        string vfdEnquiryReqParams = "/api/Invoice/ProcessInvoiceR/" + referNo + "/" + productID + "/" + uemail;
                        string IResponse = General.MakeVFDRequest(Baseurl, vfdEnquiryReqParams, "GET");
                        //General.MakeRequest(Baseurl + "/api/Invoice/ProcessInvoiceR/" + referNo + "/" + productID , uemail, "GET");

                        //Save Request and Response
                        var logResponse = JsonConvert.DeserializeObject<AutoRegResponse>(IResponse);

                        var RequestResponseLog = new SysRequestResponseLog
                        {
                            Request = Srequest.ToString(),
                            Response = IResponse,
                            DateCreated = DateTime.Now,
                            Message = logResponse.message,
                            StatusCode = logResponse.status,
                            TransactionType = "InvoicePaymentRetry"
                        };

                        await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                        if (!String.IsNullOrEmpty(IResponse))
                        {
                            //if (IResponse == "500")
                            //{
                            //    ViewBag.ErrorMessage = "Invalid Server Response, try again "; 
                            //    return View(model);
                            //}
                            aregResponse = JsonConvert.DeserializeObject<AutoDResponse>(IResponse);

                            if (Convert.ToInt32(aregResponse.status) >= 1)
                            {
                                ViewBag.ErrorMessage = "Response " + aregResponse.message;

                                //Audit Trail 
                                audit_ = new EgsAuditTrail
                                {
                                    DbAction = "QUERY",
                                    DateCreated = DateTime.Now,
                                    Page = "PayWithWallet",
                                    Description = user2.FirstName + " " + user2.LastName + " Reference query returned " + aregResponse.message + " at " + DateTime.Now,
                                    IPAddress = Helpers.General.GetIPAddress(),
                                    CreatedBy = user2.UserID,
                                    Menu = "Pay Bills",
                                    Role = user2.Role.RoleName
                                };

                                await _unitOfWork.AuditTrail.AddAsync(audit_);
                                _unitOfWork.Complete();

                                return View(model);
                                //return RedirectToAction("OrderInfo","WalletServices",new { area = "Wallets", id =prdid });
                            }
                        }
                        var rmodel = new OrdersModel
                        {
                            FirstName = aregResponse.firstName,
                            MiddleName = aregResponse.middleName,
                            LastName = aregResponse.lastName,
                            Email = aregResponse.email,
                            Phone = aregResponse.phone,
                            CustReference = aregResponse.custReference,
                            Amount = aregResponse.amount,
                            TotalAmount = aregResponse.totalAmount,
                            ReferenceNo = aregResponse.custReference,
                            RegistrationNo = aregResponse.registrationNo,
                            ProductModeID = aregResponse.productModeID,
                            ProductName = aregResponse.productName,
                            ProductItemCode = aregResponse.productItemCode,
                            AmountRate = aregResponse.amountRate,
                            ProductID = Convert.ToInt32(prdid),
                            GeneratedBy = user2.FirstName + " " + user2.LastName,
                        };
                        ViewBag.ProductID = Convert.ToInt32(prdid);
                        ViewBag.StateID = user2.ResidentialState.StateID;

                        //Audit Trail 
                        audit_ = new EgsAuditTrail
                        {
                            DbAction = "QUERY",
                            DateCreated = DateTime.Now,
                            Page = "PayWithWallet",
                            Description = user2.FirstName + " " + user2.LastName + " Reference query returned success at " + DateTime.Now,
                            IPAddress = Helpers.General.GetIPAddress(),
                            CreatedBy = user2.UserID,
                            Menu = "Pay Bills",
                            Role = user2.Role.RoleName
                        };

                        await _unitOfWork.AuditTrail.AddAsync(audit_);
                        _unitOfWork.Complete();

                        return View(rmodel);
                    }
                    else if (productItemCode != null)
                    {
                        var Srequest = (Baseurl + "/api/Invoice/ProcessInvoiceR/" + referNo + "/" + productID + "/" + uemail + "?prditemcode=" + productItemCode, "GET");//+ "/" + uEmail    + "/" + uEmail
                        string vfdEnquiryReqParams = "/api/Invoice/ProcessInvoiceR/" + referNo + "/" + productID + "/" + uemail + "?prditemcode=" + productItemCode;
                        string IResponse = General.MakeVFDRequest(Baseurl, vfdEnquiryReqParams, "GET");
                        //General.MakeRequest(Baseurl + "/api/Invoice/ProcessInvoiceR/" + referNo + "/" + productID , uemail, "GET");

                        //Save Request and Response
                        var logResponse = JsonConvert.DeserializeObject<AutoRegResponse>(IResponse);

                        var RequestResponseLog = new SysRequestResponseLog
                        {
                            Request = Srequest.ToString(),
                            Response = IResponse,
                            DateCreated = DateTime.Now,
                            Message = logResponse.message,
                            StatusCode = logResponse.status,
                            TransactionType = "InvoicePaymentRetry"
                        };

                        await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                        if (!String.IsNullOrEmpty(IResponse))
                        {
                            //if (IResponse == "500")
                            //{
                            //    ViewBag.ErrorMessage = "Invalid Server Response, try again "; 
                            //    return View(model);
                            //}
                            aregResponse = JsonConvert.DeserializeObject<AutoDResponse>(IResponse);

                            if (Convert.ToInt32(aregResponse.status) >= 1)
                            {
                                ViewBag.ErrorMessage = "Response " + aregResponse.message;

                                //Audit Trail 
                                audit_ = new EgsAuditTrail
                                {
                                    DbAction = "QUERY",
                                    DateCreated = DateTime.Now,
                                    Page = "PayWithWallet",
                                    Description = user2.FirstName + " " + user2.LastName + " Reference query returned " + aregResponse.message + " at " + DateTime.Now,
                                    IPAddress = Helpers.General.GetIPAddress(),
                                    CreatedBy = user2.UserID,
                                    Menu = "Pay Bills",
                                    Role = user2.Role.RoleName
                                };

                                await _unitOfWork.AuditTrail.AddAsync(audit_);
                                _unitOfWork.Complete();

                                return View(model);
                                //return RedirectToAction("OrderInfo","WalletServices",new { area = "Wallets", id =prdid });
                            }
                        }

                        var rmodel = new OrdersModel
                        {
                            FirstName = aregResponse.firstName,
                            MiddleName = aregResponse.middleName,
                            LastName = aregResponse.lastName,
                            Email = aregResponse.email,
                            Phone = aregResponse.phone,
                            CustReference = aregResponse.custReference,
                            Amount = aregResponse.amount,
                            TotalAmount = aregResponse.totalAmount,
                            ReferenceNo = aregResponse.custReference,
                            RegistrationNo = aregResponse.registrationNo,
                            ProductModeID = aregResponse.productModeID,
                            ProductName = aregResponse.productName,
                            ProductItemCode = productItemCode,
                            AmountRate = aregResponse.amountRate,
                            ProductID = Convert.ToInt32(prdid),
                            GeneratedBy = user2.FirstName + " " + user2.LastName,
                        };

                        ViewBag.ProductItemCode = productItemCode;
                        ViewBag.ProductID = Convert.ToInt32(prdid);
                        ViewBag.StateID = user2.ResidentialState.StateID;

                        //Audit Trail 
                        audit_ = new EgsAuditTrail
                        {
                            DbAction = "QUERY",
                            DateCreated = DateTime.Now,
                            Page = "PayWithWallet",
                            Description = user2.FirstName + " " + user2.LastName + " Reference query returned success at " + DateTime.Now,
                            IPAddress = Helpers.General.GetIPAddress(),
                            CreatedBy = user2.UserID,
                            Menu = "Pay Bills",
                            Role = user2.Role.RoleName
                        };

                        await _unitOfWork.AuditTrail.AddAsync(audit_);
                        _unitOfWork.Complete();

                        return View(rmodel);
                    }

                    /***This section was commented out by emmanuel contact if needed to be added to
                    else if (productItemCode != null)
                    {
                        var Srequest = (Baseurl + "/api/Invoice/ProcessInvoiceR/" + referNo + "/" + productID + "?prditemcode=" + productItemCode, "", "GET");
                        string IResponse = General.MakeRequest(Baseurl + "/api/Invoice/ProcessInvoiceR/" + referNo + "/" + productID + "?prditemcode=" + productItemCode, "", "GET");
                        //var Srequest = (Baseurl + "/api/Invoice/ProcessInvoiceR/" + referNo + "/" + productItemCode, "", "GET");
                        //string IResponse = General.MakeRequest(Baseurl + "/api/Invoice/ProcessInvoiceR/" + referNo + "/"+ productItemCode, "", "GET");

                        //Save Request and Response
                        var RequestResponseLog = new SysRequestResponseLog
                        {
                            Request = Srequest.ToString(),
                            Response = IResponse,
                            DateCreated = DateTime.Now
                        };
                        await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);
                        if (!String.IsNullOrEmpty(IResponse))
                        {
                            //if (IResponse == "500")
                            //{
                            //    ViewBag.ErrorMessage = "Invalid Server Response, try again ";
                            //    return View(model);
                            //}
                            aregResponse = JsonConvert.DeserializeObject<AutoDResponse>(IResponse);

                            if (Convert.ToInt32(aregResponse.status) >= 1)
                            {
                                ViewBag.ErrorMessage = "Response " + aregResponse.message;

                                return View(model);
                            }
                        }
                        var rmodel = new OrdersModel
                        {
                            FirstName = aregResponse.firstName,
                            MiddleName = aregResponse.middleName,
                            LastName = aregResponse.lastName,
                            Email = aregResponse.email,
                            Phone = aregResponse.phone,
                            CustReference = aregResponse.custReference,
                            Amount = aregResponse.amount,
                            TotalAmount = aregResponse.totalAmount,
                            ReferenceNo = aregResponse.custReference,
                            RegistrationNo = aregResponse.registrationNo,
                            ProductModeID = aregResponse.productModeID,
                            ProductName = aregResponse.productName,
                            AmountRate = aregResponse.amountRate,
                            ProductID = aregResponse.productID,
                            ProductItemID = aregResponse.productItemID,
                            ProductItemCode = aregResponse.productItemCode,
                            GeneratedBy = user2.FirstName + " " + user2.LastName,
                        };
                        ViewBag.ProductID = aregResponse.productID;
                        ViewBag.ProductItemID = aregResponse.productItemID;
                        ViewBag.productItemCode = aregResponse.productItemCode;
                        ViewBag.StateID = user2.ResidentialState.StateID;

                        //Audit Trail 
                        audit_ = new EgsAuditTrail
                        {
                            DbAction = "QUERY",
                            DateCreated = DateTime.Now,
                            Page = "PayWithWallet",
                            Description = user2.FirstName + " " + user2.LastName + " Reference query with product itemcode "+ productItemCode + " returned success at " + DateTime.Now,
                            IPAddress = Helpers.General.GetIPAddress(),
                            CreatedBy = user2.UserID,
                            Menu = "Pay Bills",
                            Role = user2.Role.RoleName
                        };

                        await _unitOfWork.AuditTrail.AddAsync(audit_);
                        _unitOfWork.Complete();

                        return View(rmodel);
                    }***/
                }
                return View(model);

            }
            else
            {
                ViewBag.Message = "There is no record for this order ";
                //Audit Trail 
                audit_ = new EgsAuditTrail
                {
                    DbAction = "QUERY",
                    DateCreated = DateTime.Now,
                    Page = "PayWithWallet",
                    Description = user2.FirstName + " " + user2.LastName + " Reference query returned no record for this Reference at " + DateTime.Now,
                    IPAddress = Helpers.General.GetIPAddress(),
                    CreatedBy = user2.UserID,
                    Menu = "Pay Bills",
                    Role = user2.Role.RoleName
                };

                await _unitOfWork.AuditTrail.AddAsync(audit_);
                _unitOfWork.Complete();

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PayWithWallet(OrdersModel model) //   model InvoicePayModel
        {
            //var ViewModel = new OrdersModel();
            //var transactionType = _unitOfWork.TransactionType.Get(2);
            EgsAuditTrail audit_;
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            //int prdModeId = model.ProductModeID; 
            if (model.ReferenceNo != null && model.ProductID != 0 && model.TotalAmount > 0)
            {
                var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                string Baseurl = EgolePayServiceConfig.Url;
                //string producturl = Baseurl + "/api/Invoice/PayInvoiceR";
                string producturl = Baseurl + "/api/Invoice/PaymentInvoiceR";
                //var cResponse = new CustomerInformationResponse();
                var aregResponse = new AutoDResponse();

                var data = new InvoicePayModel
                {
                    TotalAmount = model.TotalAmount,
                    refno = model.ReferenceNo,
                    productID = model.ProductID,
                    ProductItemCode = model.ProductItemCode,
                    ProductItemID = model.ProductItemID,
                    Email = user2.Email,
                    IPAddress = Helpers.General.GetIPAddress(),
                };

                string bodyReqParams = JsonConvert.SerializeObject(data, Formatting.Indented); //model.ReferenceNo;

                using (var _client = new HttpClient())
                {
                    var Srequest = (producturl, bodyReqParams, "POST");
                    string IResponse = General.MakeVFDRequest(producturl, null, "POST", null, bodyReqParams);
                    var logResponse = JsonConvert.DeserializeObject<AutoRegResponse>(IResponse);

                    var RequestResponseLog = new SysRequestResponseLog
                    {
                        Request = Srequest.ToString(),
                        Response = IResponse,
                        DateCreated = DateTime.Now,
                        Message = logResponse.message,
                        StatusCode = logResponse.status,
                        TransactionType = "AutoReg-Reference-Payment"
                    };

                    await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);
                    if (!String.IsNullOrEmpty(IResponse))
                    {
                        aregResponse = JsonConvert.DeserializeObject<AutoDResponse>(IResponse);

                        if (Convert.ToInt32(aregResponse.status) >= 1)
                        {
                            ViewBag.ErrorMessage = "Response " + aregResponse.message;

                            //Audit Trail 
                            audit_ = new EgsAuditTrail
                            {
                                DbAction = "QUERY",
                                DateCreated = DateTime.Now,
                                Page = "PayWithWallet",
                                Description = user2.FirstName + " " + user2.LastName + " AutoReg-Reference-Payment returned " + aregResponse.message + " at " + DateTime.Now,
                                IPAddress = Helpers.General.GetIPAddress(),
                                CreatedBy = user2.UserID,
                                Menu = "Pay Bills",
                                Role = user2.Role.RoleName
                            };

                            await _unitOfWork.AuditTrail.AddAsync(audit_);
                            _unitOfWork.Complete();

                            return View(model);
                        }
                    }

                    var rmodel = new OrdersModel
                    {
                        FirstName = aregResponse.firstName,
                        MiddleName = aregResponse.middleName,
                        LastName = aregResponse.lastName,
                        Email = aregResponse.email,
                        Phone = aregResponse.phone,
                        CustReference = aregResponse.custReference,
                        Amount = aregResponse.amount,
                        TotalAmount = aregResponse.totalAmount,
                        ReferenceNo = aregResponse.custReference,
                        RegistrationNo = aregResponse.registrationNo,
                        ProductModeID = aregResponse.productModeID,
                        ProductName = aregResponse.productName,
                        AmountRate = aregResponse.amountRate,
                        ProductID = aregResponse.productID,
                        ProductItemID = aregResponse.productItemID,
                        ProductItemCode = aregResponse.productItemCode,
                        Message = Convert.ToInt32(aregResponse.message),
                        GeneratedBy = user2.FirstName + " " + user2.LastName,

                    };
                    if (rmodel.Message <= 1)
                    {

                        //Audit Trail 
                        audit_ = new EgsAuditTrail
                        {
                            DbAction = "INSERT",
                            DateCreated = DateTime.Now,
                            Page = "PayWithWallet",
                            Description = user2.FirstName + " " + user2.LastName + " Wallet payment is successful at " + DateTime.Now,
                            IPAddress = Helpers.General.GetIPAddress(),
                            CreatedBy = user2.UserID,
                            Menu = "Pay Bills",
                            Role = user2.Role.RoleName
                        };

                        await _unitOfWork.AuditTrail.AddAsync(audit_);
                        _unitOfWork.Complete();

                        ViewBag.SuccessMessage = "Wallet payment is successful";
                        ViewBag.Paid = "Wallet payment is successful,Print";
                        return View(rmodel);
                    }
                }
                return View(model);
            }
            else
            {
                //ViewBag.ErrorMessage = "You do not have sufficient fund to perform this transaction.";
                ViewBag.ErrorMessage = "Invalid Request.";
                return View(model);
                //return RedirectToAction("PayWithWallet", new {prd=model.ProductID,phone = model.Phone,ReferenceNo =model.ReferenceNo });  //(int ? prdid, string phone, string ReferenceNo)
            }
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmDetails(string prct)  //id is the productid//ConfirmDetails(int? id, string invRefNo="", string prodtName = "", string rate = "", string FullDetails = "", string NumReg = "") //id is the productid
        {
            if (!string.IsNullOrEmpty(prct))
            {
                //ViewBag.Message = "This the value " + invRefNo.ToString();
                var getInvoiceDetails = await _unitOfWork.Invoice.GetReferenceNo(prct);
                var getUser = await _unitOfWork.User.GetUserByInvoice(getInvoiceDetails.CreatedBy);
                var model = new HomeStoreModel
                {
                    ReferenceNo = getInvoiceDetails.ReferenceNo,
                    ProductName = getInvoiceDetails.Product.ProductName,
                    PhoneNumber = getUser.PhoneNumber,
                    Email = getUser.Email,
                    RegNumber = getInvoiceDetails.ServiceNumber,
                    AmountRate = getInvoiceDetails.Amount,
                    InvoiceID = getInvoiceDetails.InvoiceID

                };

                return View(model);

            }
            else
            {
                ViewBag.Message = "There is no record for this order ";
                return View();
            }
            //ViewBag.ProductItemList = await ProductItemList(id);
        }


        public async Task<List<EgsProductItem>> ProductItemList(int? id)
        {
            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = EgolePayServiceConfig.Url;

            List<EgsProductItem> ItemsList = new List<EgsProductItem>();

            using (var client = new HttpClient())
            {
                var Srequest = (Baseurl + "/api/ProductItem/GetItemsByProduct/" + id.ToString(), "", "GET");
                string IResponse = General.MakeRequest(Baseurl + "/api/ProductItem/GetItemsByProduct/" + id, "", "GET"); //POST

                string responseToCaller = string.Empty;
                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse,
                    DateCreated = DateTime.Now
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

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

        public async Task<ActionResult> GetRates(string selection)
        {
            var EgolePayServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            string Baseurl = EgolePayServiceConfig.Url;

            var data = "";
            using (var client = new HttpClient())
            {
                var Srequest = (Baseurl + "/api/ProductItemRate/GetItemRatesByProductItem/" + selection.ToString(), "", "GET");
                string IResponse = General.MakeRequest(Baseurl + "/api/ProductItemRate/GetItemRatesByProductItem/" + selection.ToString(), "", "GET");

                var RequestResponseLog = new SysRequestResponseLog
                {
                    Request = Srequest.ToString(),
                    Response = IResponse
                };
                await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                //string responseToCaller = string.Empty;

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


        public ActionResult BuyPowerReceipt(EBuyPowerQuery regInvoice)
        {
            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            using (var doc = new Document(pdf))
            {
                //Image img = new Image(ImageDataFactory.Create("@Url.Content('~/uii/logos/EgolePay.png')"))
                //.SetTextAlignment(TextAlignment.LEFT);
                //doc.Add(img); 

                Paragraph header = new Paragraph("EgolePay Receipt")
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetFontSize(20);
                doc.Add(header);
                LineSeparator ls = new LineSeparator(new SolidLine());
                doc.Add(ls);
                Paragraph receiptNo = new Paragraph("ReceiptNo: " + Convert.ToString(regInvoice.receiptNo))
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFontSize(14);
                doc.Add(receiptNo);
                //receiptNo.SetPaddingBottom(72f);

                receiptNo.SetMultipliedLeading(60f);


                Paragraph OrderId = new Paragraph("OrderID: " + regInvoice.orderId)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFontSize(12);
                doc.Add(OrderId);

                Paragraph Message = new Paragraph("Status: " + regInvoice.responseMessage)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12);
                doc.Add(Message);

                Paragraph Token = new Paragraph("Token: " + regInvoice.token)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(12);
                doc.Add(Token);

                Paragraph Disco = new Paragraph("Disco: " + regInvoice.disco)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(12);
                doc.Add(Disco);

                Paragraph amountGenerated = new Paragraph("AmountGenerated: " + Convert.ToString(regInvoice.amountGenerated))
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(12);
                doc.Add(amountGenerated);

                Paragraph totalAmountPaid = new Paragraph("TotalAmountPaid: " + Convert.ToString(regInvoice.totalAmountPaid))
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(12);
                doc.Add(totalAmountPaid);

                Paragraph units = new Paragraph(Convert.ToString("Units: " + regInvoice.units))
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(12);
                doc.Add(units);

                Paragraph tax = new Paragraph(Convert.ToString("Tax: " + regInvoice.tax))
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12);
                doc.Add(tax);

                Paragraph vendAmount = new Paragraph("VendAmount: " + Convert.ToString(regInvoice.vendAmount))
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12);
                doc.Add(vendAmount);

                Paragraph vendRef = new Paragraph("VendRef: " + Convert.ToString(regInvoice.vendRef))
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12);
                doc.Add(vendRef);

                Paragraph vendTime = new Paragraph("VendTime: " + Convert.ToString(regInvoice.vendTime))
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12);
                doc.Add(vendTime);

                doc.Close();
                doc.Flush();
                pdfBytes = stream.ToArray();
            }
            return new FileContentResult(pdfBytes, "application/pdf");
        }

        public ActionResult AutoRegReceipt(OrdersModel model)
        {
            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            using (var doc = new Document(pdf))
            {
                //Image img = new Image(ImageDataFactory.Create(@"\uii\logos\EgolePay.png"))
                //.SetTextAlignment(TextAlignment.LEFT);
                //doc.Add(img);
                Paragraph header = new Paragraph("EgolePay Receipt")
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetFontSize(20);
                doc.Add(header);
                LineSeparator ls = new LineSeparator(new SolidLine());
                doc.Add(ls);
                Paragraph receiptNo = new Paragraph("ReferenceNo: " + Convert.ToString(model.CustReference))
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFontSize(12);
                doc.Add(receiptNo);
                //receiptNo.SetPaddingBottom(72f);

                receiptNo.SetMultipliedLeading(60f);

                //+" " +model.MiddleName+" "+model.LastName
                Paragraph Name = new Paragraph("Name: " + model.FirstName)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFontSize(12);
                doc.Add(Name);

                Paragraph RegistrationNo = new Paragraph("RegistrationNo: " + model.RegNumber)
           .SetTextAlignment(TextAlignment.LEFT)
           .SetFontSize(12);
                doc.Add(RegistrationNo);

                //    Paragraph PhoneNumber = new Paragraph("PhoneNumber: " + model.Phone)
                //.SetTextAlignment(TextAlignment.LEFT)
                //.SetFontSize(12);
                //    doc.Add(PhoneNumber);

                Paragraph Email = new Paragraph("Email: " + model.Email)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12);
                doc.Add(Email);

                // Paragraph BranchName = new Paragraph("BranchName: " + model.BranchName)
                //.SetTextAlignment(TextAlignment.LEFT)
                //.SetFontSize(12);
                // doc.Add(BranchName);



                //foreach (var item in model.ItemList)
                //{
                //    Paragraph ProductItemName = new Paragraph("ProductItem Name: " + item.ProductItemName)
                //    .SetTextAlignment(TextAlignment.LEFT)
                //    .SetFontSize(12);
                //    doc.Add(ProductItemName);

                //    Paragraph ProductItemCode = new Paragraph("ProductItem Code: " + item.ProductItemCode)
                //   .SetTextAlignment(TextAlignment.LEFT)
                //   .SetFontSize(12);
                //    doc.Add(ProductItemCode);

                //}
                Paragraph ItemsAmount = new Paragraph("Amount Paid: " + model.Amount)
            .SetTextAlignment(TextAlignment.LEFT)
             .SetFontSize(12);
                doc.Add(ItemsAmount);

                Paragraph AmountRate = new Paragraph("Service Charge: " + model.AmountRate)
               .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12);
                doc.Add(AmountRate);



                Paragraph TotalAmount = new Paragraph(Convert.ToString("TotalAmount: " + model.TotalAmount))
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(12);
                doc.Add(TotalAmount);
                Paragraph AgentName = new Paragraph("GeneratedBy: " + model.GeneratedBy)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(12);
                doc.Add(AgentName);
                Paragraph TransDate = new Paragraph("TransactionDate: " + DateTime.Now.ToString("dddd, dd MMMM yyyy"))
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(12);
                doc.Add(TransDate);
                Paragraph TransTime = new Paragraph("TransactionTime: " + DateTime.Now.ToString("HH:mm"))
             .SetTextAlignment(TextAlignment.LEFT)
             .SetFontSize(12);
                doc.Add(TransTime);

                doc.Close();
                doc.Flush();
                pdfBytes = stream.ToArray();
            }
            return new FileContentResult(pdfBytes, "application/pdf");
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Fees()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

    }
}
