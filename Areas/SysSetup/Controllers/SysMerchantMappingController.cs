using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.SysSetup.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    //[Authorize(Roles = "Super Admin")]
    public class SysMerchantMappingController : Controller
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

        public SysMerchantMappingController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: SysMerchantMapping
        public ActionResult Index()
        {
            return View();
        }


        #region "MerchantMapping"

        //Display view for creating up MerchantMapping
        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> MerchantMapping()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            ViewBag.Merchant = GetMerchant();//MerchantList();
            return View();
        }

        public List<SysUsers> MerchantList()
        {

            List<SysUsers> merchantList = new List<SysUsers>();

            merchantList = _unitOfWork.Merchant.GetAllMerchants();

            merchantList.Insert(0, new SysUsers { UserID = 0, CompanyName = "--Select Merchant--" });

            return merchantList;
        }

        [HttpGet]
        public List<MerchantsView> GetMerchant()
        {
            List<MerchantsView> subList = new List<MerchantsView>();
            subList = SubMerchantList();
            return subList;
        }

        [HttpGet]
        public async Task<JsonResult> GetSubMerchant(int merchantId) //to load a checklist item
        {
            if (merchantId != 0)
            {
                List<MerchantsView> subList = new List<MerchantsView>();
                subList = SubMerchantList();
                return Json(new SelectList(subList, "MerchantID", "MerchantName"));
            }

            return null;
        }

        public List<MerchantsView> SubMerchantList()
        {
            List<MerchantsView> merlist = new List<MerchantsView>();

            var merchants = _unitOfWork.Merchant.GetAllMerchants();

            foreach (var item in merchants)
            {
                merlist.Add(new MerchantsView
                {
                    MerchantID =item.Merchant.MerchantID,
                    MerchantName = item.CompanyName,
                });
            }

            MerchantList mList = new MerchantList();
            mList.subMerchant = merlist;

            return mList.subMerchant;
        }

        //Create new Merchant
        [HttpPost]
        // [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> MerchantMapping(SysMerchantMappingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            //merchant details
            var getMerchant = await _unitOfWork.Merchant.GetMerchantsById(model.MerchantID);

            //submerchant details
            var getUserSubMerchant = await _unitOfWork.Merchant.GetMerchantsById(model.SubMerchantID);

            await _unitOfWork.MerchantMapping.AddSaveAsync(new SysMerchantMapping
            {
                MerchantMappingID = model.MerchantMappingID,
                Merchant = getMerchant.Merchant,
                SubMerchantID = getUserSubMerchant.Merchant.MerchantID,
                CreatedBy = user2.UserID,
                DateCreated = DateTime.Now,
            });

            //await _unitOfWork.CompleteAsync();
            ModelState.Clear();
            ViewBag.Message = "Sub Merchant  has been mapped successfully.";
            ViewBag.Merchant = GetMerchant();
            return View();

        }
        #endregion

        //Display all MerchantMappingList
        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> MerchantMappingList(int? pageIndex, string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<DisplayMerchantMappingViewModel> model = new List<DisplayMerchantMappingViewModel>();
                var merchantmappingdetails = await _unitOfWork.MerchantMapping.FetchAllSubMerchants();
                var mersdetails = await _unitOfWork.MerchantMapping.FetchMappedSubMerchants();

                 //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    mersdetails = mersdetails.Where(mer => mer.Merchant.MerchantCode.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var MerchantMapping in merchantmappingdetails.ToList())
                {
                    var pMerchantInfo = _unitOfWork.Merchant.GetMerchantsById(MerchantMapping.Merchant.MerchantID);
                    var subMerchantInfo = _unitOfWork.Merchant.GetMerchantsById(MerchantMapping.SubMerchantID);

                    model.Add(new DisplayMerchantMappingViewModel
                    {
                        MerchantMappingID = MerchantMapping.MerchantMappingID,
                        MerchantName = pMerchantInfo.Result.CompanyName,
                        SubMerchantName = subMerchantInfo.Result.CompanyName,
                    });
                }
                var pager = new Pager(merchantmappingdetails.Count(), pageIndex);

                var modelR = new HoldDisplayMerchantMappingViewModel
                {
                    HoldAllMerchantMapping = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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

    }

}
