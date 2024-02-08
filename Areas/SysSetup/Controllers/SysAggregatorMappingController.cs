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
    [Authorize]
    //[Authorize(Roles = "Super Admin")]
    public class SysAggregatorMappingController : Controller
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

        public SysAggregatorMappingController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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


        // GET: SysAggregatorMapping
        public ActionResult Index()
        {
            return View();
        }

         
        //Display view for creating up MerchantMapping
        [HttpGet] 
        public async Task<IActionResult> AggregatorMapping()
        {
            ViewBag.Aggregator = await AggregatorList();
            ViewBag.Agent = await AgentList();
            return View();
        }

        ////method to pull all the aggregators
        //public List<SysUsers> AggregatorList()
        //{

        //    List<SysUsers> aggregatorList = new List<SysUsers>();

        //    aggregatorList = _unitOfWork.Aggregator.GetAllAggregators();

        //    aggregatorList.Insert(0, new SysUsers { UserID = 0, FirstName = "--Select Aggregator--" });

        //    return aggregatorList;
        //}

        [HttpGet]
        public async Task<List<AgentsView>> GetAgent()
        {
            List<AgentsView> agentList = new List<AgentsView>();
            agentList = await AgentList();
            return agentList;
        }

        [HttpGet]
        public async Task<List<AggregatorsView>> GetAggregator()
        {
            List<AggregatorsView> aggregatorList = new List<AggregatorsView>();
            aggregatorList = await AggregatorList();
            return aggregatorList;
        }

        [HttpGet]
        public async Task<JsonResult> GetAgents(int AggregatorId) //to load a checklist item
        {
            if (AggregatorId != 0)
            {
                List<AgentsView> agentList = new List<AgentsView>();
                agentList = await AgentList();
                return Json(new SelectList(agentList, "AgentID", "AgentName"));
            }

            return null;
        }

        //Method to pull all the agents
        public async Task<List<AgentsView>> AgentList()
        {
            List<AgentsView> agenlist = new List<AgentsView>();

            var agents = await _unitOfWork.User.GetAllAgents();

            foreach (var item in agents)
            {
                agenlist.Add(new AgentsView
                {
                    AgentID = item.UserID,
                    AgentName = item.FirstName +""+ item.LastName,
                });
            }

            AgentList aList = new AgentList();
            aList.Agent = agenlist;

            return aList.Agent;
        }

        //Method to pull all the aggregators
        public async Task<List<AggregatorsView>> AggregatorList()
        {
            List<AggregatorsView> agglist = new List<AggregatorsView>();

            var aggregators = await _unitOfWork.Aggregator.GetAllAggregators();

            foreach (var item in aggregators)
            {
                agglist.Add(new AggregatorsView
                {
                    AggregatorID = item.Aggregator.AggregatorID,
                    AggregatorName = item.FirstName + " " + item.LastName,
                });
            }

            AggregatorList aList = new AggregatorList();
            aList.Aggregator = agglist;

            return aList.Aggregator;
        }

        //Create new Aggregator Mapping
        [HttpPost] 
        public async Task<IActionResult> AggregatorMapping(SysAggregatorMappingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill in the required fields.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            //Aggregator details
            var getAggregator = await _unitOfWork.Aggregator.GetAggregatorById(model.AggregatorID);

            //Agent details
            var getAgent = await _unitOfWork.Aggregator.GetAgentById(model.AgentID);

            await _unitOfWork.AgentMapping.AddSaveAsync(new SysAgentMapping
            {
                AgentMappingID = model.AggregatorMappingID,
                //Aggregator = getAggregator.Aggregator,
                AggregatorID = model.AggregatorID,
                AgentID = getAgent.UserID,
                CreatedBy = user2.UserID,
                DateCreated = DateTime.Now,
            });

           
            ModelState.Clear();
            ViewBag.Message = "Agent has been mapped successfully.";
            ViewBag.Aggregator = await AggregatorList();
            ViewBag.Agent = await AgentList();
            return View();

        }
   

        [HttpGet] 
        public async Task<IActionResult> AggregatorMappingList(int? pageIndex, string searchText)
        {
            try
            {
                List<DisplayAggregatorMappingViewModel> model = new List<DisplayAggregatorMappingViewModel>();
                var agentmappingdetails = await _unitOfWork.AgentMapping.FetchAllAgents();
                var agendetails = await _unitOfWork.AgentMapping.FetchMappedAgents();

                //Logic for search
                if (!String.IsNullOrEmpty(searchText))
                {
                    agendetails = agendetails.Where(agg => agg.Aggregator.AggregatorCode.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                //load up the list of viewmodels 
                foreach (var AgentMapping in agentmappingdetails.ToList())
                {
                    var AggregatorInfo = _unitOfWork.Aggregator.GetAggregatorById(AgentMapping.Aggregator.AggregatorID);
                    var AgentInfo = _unitOfWork.Aggregator.GetAgentById(AgentMapping.AgentID);

                    model.Add(new DisplayAggregatorMappingViewModel
                    {
                        AggregatorMappingID = AgentMapping.AgentMappingID,
                        AggregatorName = AggregatorInfo.Result.FirstName + "" + AggregatorInfo.Result.LastName,
                        AgentName = AgentInfo.Result.FirstName +" " + AgentInfo.Result.LastName,
                    });
                }
                var pager = new Pager(agentmappingdetails.Count(), pageIndex);

                var modelR = new HoldDisplayAggregatorMappingViewModel
                {
                    HoldAllAggregatorMapping = model.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
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
