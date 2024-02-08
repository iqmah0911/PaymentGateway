using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.EgsOperations.Controllers
{
    [Area("EgsOperations")]
    public class EgsTransferController : Controller
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
        //private readonly IEmailSender _mailSender;
        #endregion
        public EgsTransferController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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

        public async Task<IActionResult> TransferView()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //getting image for user layout
            var profiles = await _unitOfWork.User.GetUserProfile(user2.UserID);
            ViewBag.pic = profiles.Image;

            ViewBag.TransMethod = await _unitOfWork.TransactionMethod.GetAllTransactionMethod();
            ViewBag.TransType =   _unitOfWork.TransactionType.GetAll();
            ViewBag.BankCodes = await _unitOfWork.Bank.GetAllBanks();


            return View();
        }


    }
}
