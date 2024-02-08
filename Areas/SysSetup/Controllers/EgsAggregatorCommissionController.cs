using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class EgsAggregatorCommissionController : Controller
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


        public EgsAggregatorCommissionController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _roleManager = roleManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> Settlement()
        {

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            ViewBag.Aggregator = await AggregatorList();
            ViewBag.Products = await _unitOfWork.Products.GetAllProducts();
            ViewBag.Productitems = await _unitOfWork.ProductItem.GetAllProductItems();
            ViewBag.SettlementTypes = await _unitOfWork.SettlementType.GetSettlementTypes();
            ViewBag.SettlementIntervals = await _unitOfWork.SettlementInterval.GetSettlementIntervals();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Settlement(EgsAggregatorCommissionViewModel model)
        {
            ViewBag.ProductID = model.ProductID;
            ViewBag.ProductItemID = model.ProductItemID;
            ViewBag.AggregatorID = model.AggregatorID;
            ViewBag.SettlementIntervalID = model.SettlementIntervalID;
            ViewBag.SettlementTypeID = model.SettlementTypeID;

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();


            var clientUrl = APIServiceConfig?.Url;
            var agcResponse = new AggregatorCommResponse();

            AggregatorCommModel aggComm = new AggregatorCommModel()
            {
                AggregatorID = model.AggregatorID,
                AggregatorCommissionID = 0,//model.AggregatorCommissionID,
                ProductID = model.ProductID,
                ProductItemID = model.ProductItemID,//"PDF",
                SettlementIntervalID = model.SettlementIntervalID,
                SettlementTypeID = model.SettlementTypeID,
                SplittingRate = model.SplittingRate,
                DateCreated = DateTime.Now,
                CreatedBy = user2.UserID,
            };

            var bodyRequest = JsonConvert.SerializeObject(aggComm, Formatting.Indented);

            using (var _client = new HttpClient())
            {
                string IResponse = General.MakeVFDRequest(clientUrl + "/api/AggregatorCommission/add", null, "POST", null, bodyRequest);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    agcResponse = JsonConvert.DeserializeObject<AggregatorCommResponse>(IResponse);

                    if (Convert.ToInt32(agcResponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + agcResponse.message;
                        return View();
                    }
                }

            }

            ViewBag.Aggregator = await AggregatorList();
            ViewBag.Products = await _unitOfWork.Products.GetAllProducts();
            ViewBag.Productitems = await _unitOfWork.ProductItem.GetAllProductItems();
            ViewBag.SettlementTypes = await _unitOfWork.SettlementType.GetSettlementTypes();
            ViewBag.SettlementIntervals = await _unitOfWork.SettlementInterval.GetSettlementIntervals();
            ViewBag.Message = agcResponse.message;
            return View();
        }

        //[HttpGet]
        //public async Task<List<AggregatorsView>> GetAggregator()
        //{
        //    List<AggregatorsView> subList = new List<AggregatorsView>();
        //    subList = await AggregatorList();
        //    return subList;
        //}

        public async Task<List<AggregatorsView>> AggregatorList()
        {
            List<AggregatorsView> agglist = new List<AggregatorsView>();

            var aggregator = await _unitOfWork.Aggregator.GetAllAggregators();

            foreach (var item in aggregator)
            {
                agglist.Add(new AggregatorsView
                {
                    AggregatorID = item.Aggregator.AggregatorID,
                    AggregatorName = item.Aggregator.AggregatorName
                });
            }

            AggregatorList aList = new AggregatorList();
            aList.Aggregator = agglist;

            return aList.Aggregator;
        }

        [HttpGet]
        public async Task<JsonResult> GetProducts(int AggregatorId) //userid
        {
            if (AggregatorId != 0)
            {
                var aggregator = await _unitOfWork.Aggregator.GetAggregatorById(AggregatorId);
                List<EgsProduct> productList = new List<EgsProduct>();
                productList = await _unitOfWork.Products.GetAllProducts();
                productList.Insert(0, new EgsProduct { ProductID = 0, ProductName = "--Select--" });
                return Json(new SelectList(productList, "ProductID", "ProductName"));
            }

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductitems(int ProductId)
        {
            if (ProductId != 0)
            {
                List<EgsProductItem> productitemsSel = new List<EgsProductItem>();
                productitemsSel = await _unitOfWork.ProductItem.GetItemsByProductID(ProductId);
                productitemsSel.Insert(0, new EgsProductItem { ProductItemID = 0, ProductItemName = "--Select--" });
                return Json(new SelectList(productitemsSel, "ProductItemID", "ProductItemName"));
            }
            return null;
        }
    }
}
