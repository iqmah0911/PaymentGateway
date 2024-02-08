using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;

namespace PaymentGateway21052021.Areas.SysSetup.Controllers
{
    [Area("SysSetup")]
    [AllowAnonymous]
    public class InvoiceReversalController : Controller
    {

        private readonly ILogger<dynamic> _logger;
 

        #region repositories
        private IUnitOfWork _unitOfWork;
        #endregion
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;

        public InvoiceReversalController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork,
            RoleManager<ApplicationRole> roleManager)
        {
             _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            //_mailSender = emailSender;
            _roleManager = roleManager;
        }
         

        public async Task<IActionResult> InvoiceReversal()
        {
            ViewBag.Users = await AgentList();
            return View();
        }

         
        public async Task<List<RDDListView>> AgentList()
        {
            List<RDDListView> AgentList = new List<RDDListView>();
            var agents = await _unitOfWork.User.GetAllAgents();

            foreach (var agentItems in agents)
            {
                AgentList.Add(new RDDListView
                {
                    itemValue = agentItems.Wallet.WalletAccountNumber,
                    itemName = agentItems.FirstName + " " + agentItems.LastName + " (" + agentItems.Wallet.WalletAccountNumber + ")"
                });
            }

            RDropDownModelView nAgentlist = new RDropDownModelView();
            nAgentlist.items = AgentList;

            return nAgentlist.items;
        }


        [HttpPost]
        public async Task<IActionResult> InvoiceReversal(InvoiceReversal invoiceReversal)
        {
            ViewBag.Users = await AgentList();

            string walletacctnumber = invoiceReversal.WalletAccountNumber;
            string refno = invoiceReversal.ReferenceNumber; 
           
             var retpayment = await  GeneralSqlClient.RPT_InvoiceReversal(walletacctnumber, refno );

 
                return View();
        }









    }
}
