using Microsoft.AspNetCore.Authorization;
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
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Controllers
{
    [Area("EgsOperations")]
    //[Authorize(Roles = "Agent")]
    public class EgsAggregatorRequestController : Controller
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
        private readonly IEmailSender _mailSender;
        #endregion

        public EgsAggregatorRequestController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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

  

        [HttpGet]
        public async Task<IActionResult> AggregatorRequest(string searchText)
        {
            var user = await _userManager.GetUserAsync(User);
            
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            try
            {
                List<AggregatorViewModel> modelR = new List<AggregatorViewModel>();
                if (searchText == null)
                {
                    //var listOfAggregators = new List<AggregatorViewModel>();
                    //var Aggregatorinfo = await _unitOfWork.AggregatorRequest.GetAggregatorByCode("egole");

                    //listOfAggregators.Add(new AggregatorViewModel
                    //{
                    //    AggregatorID = Aggregatorinfo.AggregatorID,
                    //    AggregatorCode = Aggregatorinfo.AggregatorCode,
                    //    FirstName = Aggregatorinfo.User.FirstName,
                    //    LastName = Aggregatorinfo.User.LastName,
                    //    Company = Aggregatorinfo.User.CompanyName,
                    //    Email = Aggregatorinfo.User.Email,
                    //    PhoneNumber = Aggregatorinfo.PhoneNumber,
                    //    Bank = Aggregatorinfo.Bank.BankName,
                    //    AccountNumber = Aggregatorinfo.AccountNo,
                    //    RoleID = Aggregatorinfo.User.Role.RoleID,
                    //    RoleName = Aggregatorinfo.User.Role.RoleName
                    //});
                    //var vmodel = new HoldAggregatorsDisplayViewModel
                    //{
                    //    HoldsAggregatorInfo = listOfAggregators
                    //};

                    return View();
                }

                var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

                var clientUrl = APIServiceConfig?.Url;
                var paramsRequest = searchText;
                var servResponse = new AggregatorRequestResponse();

                using (var _client = new HttpClient())
                {
                    var Srequest = (clientUrl + "/api/Aggregator/", paramsRequest, "GET");
                    string IResponse = General.MakeVFDRequest(clientUrl + "/api/Aggregator/", paramsRequest, "GET");
                    
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
                        servResponse = JsonConvert.DeserializeObject<AggregatorRequestResponse>(IResponse);

                        if (Convert.ToInt32(servResponse.status) >= 1)
                        {
                            ViewBag.ErrorMessage = "Response " + servResponse.message;
                            return View();
                            //return RedirectToAction("OrderInfo", "WalletServices", new { area = "Wallets", id = prdid });
                        }
                    }

                }

                List<AggregatorViewModel> aggreInfo = new List<AggregatorViewModel>();
                aggreInfo = servResponse.aggregators;

                foreach (var aggre in aggreInfo)
                {
                    modelR.Add(new AggregatorViewModel
                    {
                        AggregatorID = aggre.AggregatorID,
                        AggregatorCode = aggre.AggregatorCode,
                        FirstName = aggre.FirstName,
                        LastName = aggre.LastName,
                        Email = aggre.Email,
                        PhoneNumber = aggre.PhoneNumber,
                        Company = aggre.Company,
                        RoleID = aggre.RoleID,
                        RoleName = aggre.RoleName,
                        AccountNumber = aggre.AccountNumber,
                        Bank = aggre.Bank 
                    });
                    ViewBag.AggregatorID = aggre.AggregatorID;  
                }
                  
                var model = new HoldAggregatorsDisplayViewModel
                {
                    HoldsAggregatorInfo = modelR
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + ex.StackTrace);
            }
            {
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> AggregatorRequest(HoldAggregatorsDisplayViewModel model )      
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var checkagentmapped = await _unitOfWork.AggregatorRequest.GetUserTiedtoAggregator(user2.UserID);

            if (checkagentmapped == null)
            {

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            var clientUrl = APIServiceConfig?.Url;
            var servResponse = new DocumentsResponse();

            RequestAggregatorParams paramsBody = new RequestAggregatorParams()
            {
                //RequestID = 0,
                AgentID = user2.UserID,
                AggregatorID = model.aggID,         //model.AggregatorID,
                DateCreated = DateTime.Now,
            };


            var bodyRequest = JsonConvert.SerializeObject(paramsBody, Formatting.Indented);

            using (var _client = new HttpClient())
            {
                var Srequest = (clientUrl + "/api/Aggregator/add", "POST", bodyRequest);
                string IResponse = General.MakeVFDRequest(clientUrl + "/api/Aggregator/add", null, "POST", null, bodyRequest);
                 
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
                    servResponse = JsonConvert.DeserializeObject<DocumentsResponse>(IResponse);

                    if (Convert.ToInt32(servResponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + servResponse.message;
                        return View();
                    }
                }

            }
             
            ViewBag.Role = await _unitOfWork.Role.UpgradeRoles();
            ViewBag.Message = servResponse.message;
                 
            return View();
          }
            ViewBag.Role = await _unitOfWork.Role.UpgradeRoles();
            ViewBag.Message = "Agent already tied to an aggregator";
            return View();

        }






        public IActionResult Index()
        {
            return View();
        }

    }
}
