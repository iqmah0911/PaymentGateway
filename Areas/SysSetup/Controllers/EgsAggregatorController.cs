using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Helpers.Interface;
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
    //[Authorize(Roles = "Aggregator")]
    public class EgsAggregatorController : Controller
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
        private readonly IMimeSender _mimeSender;
        public IHostingEnvironment _hostingEnv { get; }

        #region logger
        private readonly IEmailSender _mailSender;
        #endregion

        public EgsAggregatorController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment, IMimeSender mimeSender,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mimeSender = mimeSender;
            _hostingEnv = hostingEnvironment;
            _roleManager = roleManager;
        }
          
        public IActionResult Index()
        {
            return View();
        }
         
        public async Task<IActionResult> AgentRequestList()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString()); 
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
            ViewBag.aggcode = aggcode.AggregatorCode;

            //List<AgentRequestViewModel> model = new List<AgentRequestViewModel>();
            var model = new List<AgentRequestViewModel>();
            var aresponse = new PAgentResponse();

            var aggregator = await _unitOfWork.Aggregator.GetAggregatorByUserId(user2.UserID);
            
            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            var Baseurl = APIServiceConfig?.Url;

            var paramas = aggregator.AggregatorID;

            using (var client = new HttpClient())
            { 
                string IResponse = General.MakeVFDRequest(Baseurl + "/api/RequestAggregator/pending/" , Convert.ToString(paramas),"GET");
               
                if (!String.IsNullOrEmpty(IResponse))
                {
                    aresponse = JsonConvert.DeserializeObject<PAgentResponse>(IResponse);

                    if (Convert.ToInt32(aresponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + aresponse.message;
                        return View();
                    }
                }
            }

            if(aresponse.aggregatorrequest.Count == 0)
            {
                // return View();
                var modeld = new HoldAgentRequestViewModel
                {
                    HoldAllRequests = model

                };
                return View(modeld);
            }

            //var getRequests = await _unitOfWork.AggregatorRequest.GetAggregatorRequestByAggregatorID(aggregator.AggregatorID);
           
            //load up the list of viewmodels --- getRequests
            foreach (var getrequest in aresponse.aggregatorrequest)
            {
                //var uName = await _unitOfWork.User.GetSysUsers(getrequest.Agent.UserID.ToString());
                var uName = await _unitOfWork.User.GetSysUsers(getrequest.agentID.ToString());
                 
                model.Add(new AgentRequestViewModel
                {
                    RequestID = getrequest.requestID,
                    //UserID = getrequest.Agent.UserID, 
                    UserID = getrequest.agentID,
                    UserName = uName.FirstName+" "+uName.MiddleName+" "+uName.LastName,
                    //AggregatorId = getrequest.Aggregator.AggregatorID,
                    AggregatorId = getrequest.aggregatorID,
                    IsProcessed = getrequest.isProcessed,
                    DateCreated = getrequest.dateCreated
                });
            }

            var modelR = new HoldAgentRequestViewModel
            {
                HoldAllRequests = model

            };
            return View(modelR); 
        }
         
        public async Task<IActionResult> ProcessAgentRequest(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
            ViewBag.aggcode = aggcode.AggregatorCode;
            if (id == 0)
            {
                return RedirectToAction("AgentRequestList");
            }
            var agentdata = await _unitOfWork.AggregatorRequest.GetAggregatorRequestByReqId(Convert.ToInt32(id));
            if (agentdata == null)
            {
                return RedirectToAction("AgentRequestList");
            }
            var uName = await _unitOfWork.User.GetSysUsers(agentdata.Agent.UserID.ToString());

            var aggmodel = new AgentRequestViewModel
            {
                RequestID = agentdata.RequestID,
                UserName = uName.FirstName+" "+ uName.LastName,
                AggregatorId = agentdata.Aggregator.AggregatorID, 
                AggregatorName = agentdata.Aggregator.AggregatorName,
                DateCreated = agentdata.DateCreated,
                UserID = agentdata.Agent.UserID,
                
            };
              
            return View(aggmodel);
        }


        [HttpPost]
        public async Task<IActionResult> ProcessAgentRequest(AgentRequestViewModel model)
        {
            if (!ModelState.IsValid)
            { 
                ViewBag.Message = "Please fill in the required fields.";
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
            var clientUrl = APIServiceConfig?.Url;

            var agentResponse = new PostAgentResponse();
             
            PostAgentRequestViewModel paramsBody = new PostAgentRequestViewModel()
            {
                RequestID = model.RequestID,
               AggregatorID =model.AggregatorId,
                 UserID=model.UserID, 
                IsProcessed = model.IsProcessed,
                Comment = model.Comment,
                ApprovedBy = user2.UserID,
                DateApproved = DateTime.Now,
                DateCreated = model.DateCreated ,
                RejectedBy=model.RejectedBy
            };

            var bodyRequest = JsonConvert.SerializeObject(paramsBody, Formatting.Indented);

            using (var _client = new HttpClient())
            {
                string IResponse = General.MakeVFDRequest(clientUrl + "/api/RequestAggregator/ProcessPendingRequest", null, "POST", null, bodyRequest);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    agentResponse = JsonConvert.DeserializeObject<PostAgentResponse>(IResponse);

                    if (Convert.ToInt32(agentResponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + agentResponse.message;
                        return View();
                    }
                }
            }

            ViewBag.Message = agentResponse.message;
             
            return RedirectToAction("AgentRequestList", "EgsAggregator");
             
        }

         
        public async Task<IActionResult> ProcessedAgentsList()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;
            var aggcode = await _unitOfWork.Aggregator.GetAggregatorByUserId(user.UserID);
            ViewBag.aggcode = aggcode.AggregatorCode;
            var model = new List<AgentRequestViewModel>(); 
            var aresponse = new PAgentResponse(); 
            var aggregator = await _unitOfWork.Aggregator.GetAggregatorByUserId(user2.UserID);

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            var Baseurl = APIServiceConfig?.Url;

            var paramas = aggregator.AggregatorID;

            using (var client = new HttpClient())
            {
                string IResponse = General.MakeVFDRequest(Baseurl + "/api/RequestAggregator/allprocessedrequestforaggregator/", Convert.ToString(paramas), "GET");

                if (!String.IsNullOrEmpty(IResponse))
                {
                    aresponse = JsonConvert.DeserializeObject<PAgentResponse>(IResponse);

                    if (Convert.ToInt32(aresponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + aresponse.message;
                        return View();
                    }
                }
            }
              
            //foreach (var agent in processedagents)
            foreach (var agent in aresponse.aggregatorrequest)
            {
                //var uName = await _unitOfWork.User.GetSysUsers(agent.Agent.UserID.ToString());
                var uName = await _unitOfWork.User.GetSysUsers(agent.agentID.ToString());
                model.Add(new AgentRequestViewModel
                {
                    RequestID = agent.requestID,
                    //UserID = getrequest.Agent.UserID, 
                    UserID = agent.agentID,
                    UserName = uName.FirstName + " " + uName.LastName, 
                    //AggregatorId = getrequest.Aggregator.AggregatorID,
                    AggregatorId = agent.aggregatorID,
                    IsProcessed = agent.isProcessed,
                    DateCreated = agent.dateCreated,
                    DateApproved= agent.dateApproved,
                    AggregatorName = user2.FirstName+" "+user2.LastName
                });
                ViewBag.AggregatorName = user2.FirstName + " " + user2.LastName;
            }
            var modelR = new HoldAgentRequestViewModel
            {
                HoldAllRequests = model

            };

            return View(modelR);
        }


        public async Task<IActionResult> RemoveAgent(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ProcessedAgentsList");
            }

            var processedagent = await _unitOfWork.AggregatorRequest.GetProcessedRequestByReqId(id);

            if (processedagent != null)
            { 
               await _unitOfWork.AggregatorRequest.RemoveRequest(processedagent);
                 
                ViewBag.message = "Agent Removed Successfully";
            }

            return RedirectToAction("ProcessedAgentsList", "EgsAggregator"); 
        }

        public async Task<IActionResult> AcknowledgedAgentRequestList()
        {

            var requestInfo = await _unitOfWork.AggregatorRequest.GetAggregatorRequestAcknowledged();

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            //List<AgentRequestViewModel> model = new List<AgentRequestViewModel>();
            var model = new List<AgentRequestViewModel>();
            var aresponse = new PAgentResponse();

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();

            var Baseurl = APIServiceConfig?.Url;

            using (var client = new HttpClient())
            {
                string IResponse = General.MakeVFDRequest(Baseurl + "/api/RequestAggregator/AllAcknowledgedAgentToAggregatorRequest/", null, "GET");

                if (!String.IsNullOrEmpty(IResponse))
                {
                    aresponse = JsonConvert.DeserializeObject<PAgentResponse>(IResponse);

                    if (Convert.ToInt32(aresponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + aresponse.message;
                        return View();
                    }
                }
            }

            if (aresponse.aggregatorrequest.Count == 0)
            {
                // return View();
                var modeld = new HoldAgentRequestViewModel
                {
                    HoldAllRequests = model

                };
                return View(modeld);
            }

            //load up the list of viewmodels --- getRequests
            foreach (var getrequest in aresponse.aggregatorrequest)
            {
                //var uName = await _unitOfWork.User.GetSysUsers(getrequest.Agent.UserID.ToString());
                var uName = await _unitOfWork.User.GetSysUsers(getrequest.agentID.ToString());
                var aggregator = await _unitOfWork.Aggregator.GetAggregatorFromTblById(getrequest.aggregatorID);


                model.Add(new AgentRequestViewModel
                {
                    RequestID = getrequest.requestID,
                    //UserID = getrequest.Agent.UserID, 
                    UserID = getrequest.agentID,
                    UserName = uName.FirstName + " " + uName.MiddleName + " " + uName.LastName,
                    AggregatorName = aggregator.AggregatorName,
                    //AggregatorId = getrequest.Aggregator.AggregatorID,
                    AggregatorId = getrequest.aggregatorID,
                    IsProcessed = getrequest.isProcessed,
                    DateApproved = getrequest.dateApproved,
                    DateCreated = getrequest.dateCreated,
                    AgentCompany = uName.CompanyName,
                    AggregatorCompany = aggregator.User.CompanyName,
                });
            }

            var modelR = new HoldAgentRequestViewModel
            {
                HoldAllRequests = model

            };
            return View(modelR);
        }


        public async Task<IActionResult> ApproveAgentRequest(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            if (id == 0)
            {
                return RedirectToAction("AcknowledgedAgentRequestList");
            }

            var agentdata = await _unitOfWork.AggregatorRequest.GetAggregatorRequestAcknowledgedByReqId(Convert.ToInt32(id));

            if (agentdata == null)
            {
                return RedirectToAction("AcknowledgedAgentRequestList");
            }
            var uName = await _unitOfWork.User.GetSysUsers(agentdata.Agent.UserID.ToString());
            var aggregator = await _unitOfWork.Aggregator.GetAggregatorFromTblById(agentdata.Aggregator.AggregatorID);

            var aggmodel = new AgentRequestViewModel
            {
                RequestID = agentdata.RequestID,
                UserName = uName.FirstName + " " + uName.LastName,
                AggregatorId = agentdata.Aggregator.AggregatorID,
                AggregatorName = agentdata.Aggregator.AggregatorName,
                DateCreated = agentdata.DateCreated,
                ApprovedBy = agentdata.ApprovedBy,
                DateApproved = agentdata.DateApproved,
                AgentCompany = uName.CompanyName,
                AggregatorCompany = aggregator.User.CompanyName,
                //ApprovedBy = agentdata.Aggregator.AggregatorName,
                UserID = agentdata.Agent.UserID,

            };

            return View(aggmodel);
        }


        [HttpPost]
        public async Task<IActionResult> ApproveAgentRequest(AgentRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please fill in the required fields.";
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
            var clientUrl = APIServiceConfig?.Url;

            var agentResponse = new PostAgentResponse();

            if (model.IsProcessed == false)
            {
                model.RejectedBy2 = user2.UserID;
                model.DateRejectedBy2 = DateTime.Now;
            }
            else
            {
                model.ApprovedBy2 = user2.UserID;
                model.DateApprovedBy2 = DateTime.Now;
            }

            PostAgentRequestViewModel paramsBody = new PostAgentRequestViewModel()
            {
                RequestID = model.RequestID,
                AggregatorID = model.AggregatorId,
                UserID = model.UserID,
                IsProcessed = model.IsProcessed,
                Comment = model.Comment,
                ApprovedBy = model.ApprovedBy,
                DateApproved = model.DateApproved,
                DateCreated = model.DateCreated,
                RejectedBy = model.RejectedBy,
                ApprovedBy2 = model.ApprovedBy2,
                DateApprovedBy2 = model.DateApprovedBy2,
                RejectedBy2 = model.RejectedBy2,
                DateRejectedBy2 = model.DateRejectedBy2,
            };

            var bodyRequest = JsonConvert.SerializeObject(paramsBody, Formatting.Indented);

            using (var _client = new HttpClient())
            {
                string IResponse = General.MakeVFDRequest(clientUrl + "/api/RequestAggregator/ApprovePendingRequest", null, "POST", null, bodyRequest);

                if (!String.IsNullOrEmpty(IResponse))
                {
                    agentResponse = JsonConvert.DeserializeObject<PostAgentResponse>(IResponse);

                    if (Convert.ToInt32(agentResponse.status) >= 1)
                    {
                        ViewBag.ErrorMessage = "Response " + agentResponse.message;

                        var declinedagent = await _unitOfWork.User.GetUserByID(model.UserID);
                        var declineduser = await _unitOfWork.Aggregator.GetAggregatorFromTblById(model.AggregatorId);

                        var Message12 = "Dear " + declineduser.User.FirstName + " " + declineduser.User.LastName + ",<br/><br/>" +
                                                      "Kindly note,Your Request to Map Teller / Agent " + declinedagent.FirstName + " " + declinedagent.LastName + " on EgolePay has been declined, reason:" + model.Comment + "<br/><br/>" +
                                                      "" + "<br/>";
                        await _mimeSender.Execute("", "Agent Mapping Approval Declined", Message12, declineduser.User.Email);

                        return View();
                    }
                }
            }

            ViewBag.Message = agentResponse.message;

            return View();//RedirectToAction("AcknowledgedAgentRequestList", "EgsAggregator");

        }






    }
}
