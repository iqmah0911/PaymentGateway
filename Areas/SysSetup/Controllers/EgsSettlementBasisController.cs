using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class EgsSettlementBasisController : Controller
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


        public EgsSettlementBasisController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            ViewBag.Merchant = GetMerchant();
            ViewBag.Productitems = await _unitOfWork.ProductItem.GetAllProductItems();
            ViewBag.SettlementTypes = await _unitOfWork.SettlementType.GetSettlementTypes();
            ViewBag.SettlementIntervals = await _unitOfWork.SettlementInterval.GetSettlementIntervals();
            ViewBag.Products = await _unitOfWork.Products.GetAllProducts();
           
            return View();
        }
        [HttpGet]
        public List<MerchantsView> GetMerchant()
        {
            List<MerchantsView> subList = new List<MerchantsView>();
            subList = MerchantList();
            return subList;
        }
        public List<MerchantsView> MerchantList()
        {
            List<MerchantsView> merlist = new List<MerchantsView>();

            var merchants = _unitOfWork.Merchant.GetAllMerchants();

            foreach (var item in merchants)
            {
                merlist.Add(new MerchantsView
                {
                    MerchantID = item.Merchant.MerchantID,
                    MerchantName = item.CompanyName,
                });
            }

            MerchantList mList = new MerchantList();
            mList.subMerchant = merlist;

            return mList.subMerchant;
        }

        [HttpGet]
        public async Task<JsonResult> GetProducts(int MerchantId) //userid
        {
         if (MerchantId != 0)
            {
                var merchant = await _unitOfWork.Merchant.GetMerchantsById(MerchantId);
                List<EgsProduct> productList = new List<EgsProduct>();
                productList = await _unitOfWork.Products.GetProductsByMerchantId(merchant.Merchant.MerchantID);
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

        [HttpPost]
        public async Task<IActionResult> SettlementMerchantMapping(int? pageIndex, EgsSettlementBasisViewModel model_)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                ViewBag.ProductID = model_.ProductID;
                ViewBag.ProductItemID = model_.ProductItemID;
                ViewBag.MerchantID = model_.MerchantID;
                ViewBag.SettlementIntervalID = model_.SettlementIntervalID;
                ViewBag.SettlementTypeID = model_.SettlementTypeID;

                List<DisplaySettlementBasisViewModel> merMapping = new List<DisplaySettlementBasisViewModel>();

                var merchantmappingdetails = await _unitOfWork.MerchantMapping.FetchSubMerchantByMerchantID(model_.MerchantID);

                //load up the list of viewmodels 
                foreach (var merchantMapping in merchantmappingdetails)
                {
                    var submerchant = _unitOfWork.Merchant.GetMerchantsById(merchantMapping.SubMerchantID);// .getmer.FetchSubMerchantMappingByMerchantID(merchantMapping.SubMerchantID);
                    
                    merMapping.Add(new DisplaySettlementBasisViewModel
                    {
                       SubMerchantID = merchantMapping.SubMerchantID,
                       SubMerchantName = submerchant.Result.CompanyName,
                       MerchantID = model_.MerchantID,
                    });
                }

                var merchantinfo = _unitOfWork.Merchant.GetMerchantsById(model_.MerchantID);// .getmer.FetchSubMerchantMappingByMerchantID(merchantMapping.SubMerchantID);

                merMapping.Add(new DisplaySettlementBasisViewModel
                {
                    SubMerchantID = model_.MerchantID,
                    SubMerchantName = merchantinfo.Result.CompanyName,
                });

                var pager = new Pager(merMapping.Count(), pageIndex);

                var modelR = new HoldDisplaySettlementBasisViewModel
                {
                    HoldAllSettlementBasis = merMapping.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    
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
        public async Task<IActionResult> SettlementMerchantMappings(EgsSettlementBasisViewModel model)
        {
            //List<EgsSettlementBasisViewModel> dataModel = new List<EgsSettlementBasisViewModel>();
            //var Dmodel = model;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            await _unitOfWork.SettlementBasis.AddSaveAsync(new EgsSettlementBasis
            {
                ProductItemID = model.ProductItemID,
                ProductID = model.ProductID,
                MerchantID = model.SubMerchantID,
                SettlementTypeID = model.SettlementTypeID,
                SettlementIntervalID = model.SettlementIntervalID,
                SplittingRate = model.SplittingRate,
                CreatedBy = user2.UserID,
                DateCreated = DateTime.Now
            });

            //foreach (var item in model.HoldSettlementBasis)
            //{
            //    await _unitOfWork.SettlementBasis.AddSaveAsync(new EgsSettlementBasis
            //    {
            //        ProductItemID = item.ProductItemID,
            //        ProductID = item.ProductID,
            //        MerchantID = item.SubMerchantID,
            //        SettlementTypeID = item.SettlementTypeID,
            //        SettlementIntervalID = item.SettlementIntervalID,
            //        SplittingRate = item.SplittingRate,
            //        CreatedBy = user2.UserID,
            //        DateCreated = DateTime.Now
            //    });

            //}
            //    //await _unitOfWork.CompleteAsync();
            //    //ModelState.Clear();
            ViewBag.Message = "Settlement Basis has been saved successfully";

            return View("SettlementMerchantMapping"); 
        }

          
        public async Task<JsonResult> InsertSettlements([FromBody] IEnumerable<SettlementBasisViewModel> settlementModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            //Loop and insert records.
            foreach (SettlementBasisViewModel item in settlementModel)
            {
                await _unitOfWork.SettlementBasis.AddSaveAsync(new EgsSettlementBasis
                {
                    ProductItemID = item.ProductItemID,
                    ProductID = item.ProductID,
                    MerchantID = item.SubMerchantID,
                    SettlementTypeID = item.SettlementTypeID,
                    SettlementIntervalID = item.SettlementIntervalID,
                    SplittingRate = item.SplittingRate,
                    CreatedBy = user2.UserID,
                    DateCreated = DateTime.Now
                });
            }
            //ModelState.Clear();
           return Json("Settlement has been inserted successfully");
            
        }































        //[HttpPost]
        //public async Task<IActionResult> SettlementMerchantMappingTest(int? pageIndex, EgsSettlementBasisViewModel model_)
        //{
        //    try
        //    {
        //        ViewBag.ProductID = model_.ProductID;
        //        ViewBag.ProductItemID = model_.ProductItemID;
        //        ViewBag.MerchantID = model_.MerchantID;
        //        ViewBag.SettlementIntervalID = model_.SettlementIntervalID;
        //        ViewBag.SettlementTypeID = model_.SettlementTypeID;

        //        List<DisplaySettlementBasisViewModel> merMapping = new List<DisplaySettlementBasisViewModel>();

        //        var merchantmappingdetails = await _unitOfWork.MerchantMapping.FetchSubMerchantByMerchantID(model_.MerchantID);

        //        //load up the list of viewmodels 
        //        foreach (var merchantMapping in merchantmappingdetails)
        //        {
        //            var submerchant = _unitOfWork.Merchant.GetMerchantsById(merchantMapping.SubMerchantID);// .getmer.FetchSubMerchantMappingByMerchantID(merchantMapping.SubMerchantID);

        //            merMapping.Add(new DisplaySettlementBasisViewModel
        //            {
        //                SubMerchantID = merchantMapping.SubMerchantID,
        //                SubMerchantName = submerchant.Result.CompanyName,
        //                MerchantID = model_.MerchantID,
        //            });
        //        }

        //        var merchantinfo = _unitOfWork.Merchant.GetMerchantsById(model_.MerchantID);// .getmer.FetchSubMerchantMappingByMerchantID(merchantMapping.SubMerchantID);

        //        merMapping.Add(new DisplaySettlementBasisViewModel
        //        {
        //            SubMerchantID = model_.MerchantID,
        //            SubMerchantName = merchantinfo.Result.CompanyName,
        //        });

        //        var pager = new Pager(merMapping.Count(), pageIndex);

        //        var modelR = new HoldDisplaySettlementBasisViewModel
        //        {
        //            HoldAllSettlementBasis = merMapping.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),

        //            Pager = pager
        //        };

        //        return View(modelR);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.Message + ex.StackTrace);
        //    }
        //    return View();
        //}


        //[HttpPost]
        //public async Task<IActionResult> SettlementMerchantMappingRS(DisplaySettlementBasisViewModel model)
        //{
        //    //List<EgsSettlementBasisViewModel> dataModel = new List<EgsSettlementBasisViewModel>();
        //    //var Dmodel = model;

        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("", "Please fill in the required fields.");
        //        return View();
        //    }
        //    var user = await _userManager.GetUserAsync(User);
        //    var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

        //    //await _unitOfWork.SettlementBasis.AddSaveAsync(new EgsSettlementBasis
        //    //{
        //    //    ProductItemID = model.ProductItemID,
        //    //    ProductID = model.ProductID,
        //    //    MerchantID = model.SubMerchantID,
        //    //    SettlementTypeID = model.SettlementTypeID,
        //    //    SettlementIntervalID = model.SettlementIntervalID,
        //    //    SplittingRate = model.SplittingRate,
        //    //    CreatedBy = user2.UserID,
        //    //    DateCreated = DateTime.Now
        //    //});

        //    foreach (var item in model.Items)
        //    {
        //        await _unitOfWork.SettlementBasis.AddSaveAsync(new EgsSettlementBasis
        //        {
        //            ProductItemID = item.ProductItemID,
        //            ProductID = item.ProductID,
        //            MerchantID = item.SubMerchantID,
        //            SettlementTypeID = item.SettlementTypeID,
        //            SettlementIntervalID = item.SettlementIntervalID,
        //            SplittingRate = item.SplittingRate,
        //            CreatedBy = user2.UserID,
        //            DateCreated = DateTime.Now
        //        });

        //    }
        //    //    //await _unitOfWork.CompleteAsync();
        //    //    //ModelState.Clear();
        //    ViewBag.Message = "Settlement Basis has been saved successfully";

        //    return View("SettlementMerchantMappingRS");

        //}





    }
}
