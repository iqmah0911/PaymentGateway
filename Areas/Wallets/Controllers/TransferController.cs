using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.Wallets.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Areas.Wallets.Controllers
{
    [Area("Wallets")]
    //[Authorize(Roles = "Agent")] 
    [Authorize(Roles = "Aggregator")]
    [AllowAnonymous]
    public class TransferController : Controller
    {
        #region repositories
        private IUnitOfWork _unitOfWork;
        #endregion
        private readonly ILogger<dynamic> _logger;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;

        public TransferController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<dynamic> logger, IUnitOfWork unitOfWork,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _roleManager = roleManager;
        }



        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Pincreation()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Pincreation(PinViewModel pinViewModel)
        {
            if (ModelState.IsValid)
            {
                var key = "b14ca5898a4e4133bbce2ea2315a1916";

              

                var user = await _userManager.GetUserAsync(User);
                var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
                var _checkuserpinexist = await _unitOfWork.UserToken.GetPin(user2.UserID);

                if (_checkuserpinexist != null)
                {
                    ViewBag.ErrorMessage = "Pin previously created";
                    return View();
                }

                if (pinViewModel.ConfirmPin.Length != 4 && pinViewModel.Pin.Length != 4)
                {
                    ViewBag.ErrorMessage = "Invalid Pin,Kindly Retry";
                    return View();
                }

 
                var pin = Helpers.General.EncryptString(key, pinViewModel.Pin);
               // var pin = Helpers.General.DecryptString(key, pinViewModel.Pin);


                await _unitOfWork.UserToken.AddSaveAsync(new EgsUserToken
                {
                    UserID = user2.UserID,
                    Pin = pin,//pinViewModel.Pin,
                    AuthFactor = 1
                });
                ViewBag.Message = "Pin successfully created";
                ModelState.Clear();
                return View();
            }

            return View();
        }


        public async Task<IActionResult> Transfer(string msg)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            ViewBag.ErrorMessage = msg;
            ViewBag.Beneficiary = await BeneficiaryList(user2.UserID);
            ViewBag.ListofBanks = await BankList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(TransferViewModel model)
        {

            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            ViewBag.ListofBanks = await BankList();
            var _checkuserpinexist = await _unitOfWork.UserToken.GetPin(user2.UserID);
            if (_checkuserpinexist == null)
            {
                ViewBag.ErrorMessage = "Invalid Operation,Transfer Pin not created";
                string errmsg = "Invalid Operation,Transfer Pin not created";
                ViewBag.Beneficiary = await BeneficiaryList(user2.UserID);
                ViewBag.ListofBanks = await BankList();
                return View("Transfer", model);
            }

            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            //var decryptedString = Helpers.General.DecryptString(key, encryptedString);
            var pin = Helpers.General.EncryptString(key, model.transferpin);
            if (_checkuserpinexist.Pin != pin)
            {
                ViewBag.ErrorMessage = "Invalid PIN";
                string errmsg_ = "Invalid PIN";
                ViewBag.Beneficiary = await BeneficiaryList(user2.UserID);
                ViewBag.ListofBanks = await BankList();
                return View("Transfer", model);
            }
            if (_checkuserpinexist.Pin != pin && _checkuserpinexist.UserID != user2.UserID)
            {
                ViewBag.ErrorMessage = "Invalid PIN";
                string errmsg_ = "Invalid PIN";
                ViewBag.Beneficiary = await BeneficiaryList(user2.UserID);
                ViewBag.ListofBanks = await BankList();
                return View("Transfer", model); 
            }

            if (model.transAmount < 1)
            {
                ViewBag.ErrorMessage = "Invalid Amount Entered, Amount must be greater than 1.";
                string mssg = "Invalid Amount Entered, Amount must be greater than 1.";
                ViewBag.Beneficiary = await BeneficiaryList(user2.UserID);
                ViewBag.ListofBanks = await BankList();
                return View("Transfer", model);
            }

            if (user2.Role.RoleID == 3 && _checkuserpinexist.Pin == pin && _checkuserpinexist.UserID == user2.UserID)
            {
                var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                var clientUrl = APIServiceConfig?.Url;

                double plustransfee = model.transAmount + 20;

                const double limit = 300000.00;
                if (plustransfee >= limit)
                {
                    ViewBag.ErrorMessage = "Maximum Transfer Limit of #300,000 Exceeded";
                    string msg= "Maximum Transfer Limit of #300,000 Exceeded";
                    ViewBag.Beneficiary = await BeneficiaryList(user2.UserID);
                    ViewBag.ListofBanks = await BankList();
                    return View("Transfer", model); 
                }
                //if (model.BeneficiaryID == 0)
                //{

                //}  
                NTransferViewModel paramsBody = new NTransferViewModel()
                {
                    walletId = user2.Wallet.WalletID,
                    toAccount = model.toAccount,
                    toBank = model.toBank,
                    transAmount = plustransfee,
                    transferType = "inter",
                    Narration = model.Narration,
                    Receiver=model.Receiver
                };


                var bodyRequest = JsonConvert.SerializeObject(paramsBody, Formatting.Indented);
                var servResponse = new TransfersResponse();
                using (var _client = new HttpClient())
                {
                    var Srequest = (clientUrl + "/api/WalletTransactions/Transfer", "POST", bodyRequest);
                    string IResponse = General.MakeVFDRequest(clientUrl + "/api/WalletTransactions/Transfer", null, "POST", null, bodyRequest);

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
                        servResponse = JsonConvert.DeserializeObject<TransfersResponse>(IResponse);

                        if (Convert.ToInt32(servResponse.status) >= 1)
                        {
                            ViewBag.ErrorMessage = "Response " + servResponse.message;
                            return View();
                        }
                    }


                    //ViewBag.Successful = "Transfer successful";
                    //ViewBag.Message = 
                    TransfersReceiptVM TMODEL = new TransfersReceiptVM();
                    TMODEL.OrderID = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8) + "-" + DateTime.Now.ToString("MMMM dd");
                    TMODEL.Date = DateTime.Now.ToString("dddd, dd MMMM yyyy");
                    TMODEL.Amount = model.transAmount;
                    TMODEL.Fee = Convert.ToString(20);
                    //TMODEL.message= "Response " + servResponse.message;
                    TMODEL.Total = Convert.ToString(plustransfee);

                    // ModelState.Clear();
                    //return View(TMODEL);
                    return RedirectToAction("TransferReceipt", "Transfer", TMODEL);
                } 
            }

            ViewBag.Message = "Transfer Not Successful,Kindly retry ";
            string vmsg = "Transfer Not Successful,Kindly retry ";
            //ModelState.Clear();
            ViewBag.Beneficiary = await BeneficiaryList(user2.UserID);
            ViewBag.ListofBanks = await BankList();
            return View("Transfer", model);
        }


        public async Task<IActionResult> TransferReceipt(TransfersReceiptVM model)
        {

            ViewBag.Date = model.Date;
            ViewBag.OrderID = model.OrderID;
            ViewBag.Amount = model.Amount;
            ViewBag.Fee = model.Fee;
            ViewBag.Total = model.Total;

            ViewBag.Message = model.message;
            return View();
        }


        [HttpGet]
        public async Task<JsonResult> Verify(string BankCode, string AccNumber)
        {
            if (BankCode != null && AccNumber != null)
            {
                var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                var clientUrl = APIServiceConfig?.Url;
                //VerificationViewModel verifyresponse = new VerificationViewModel();
                VFDRecipient reciResponse = new VFDRecipient();


                using (var client = new HttpClient())
                {
                    var Srequest = (clientUrl + "/api/WalletTransactions/ReceiverVerification/?BankCode=" + BankCode + "&&Accountnumber=" + AccNumber, "", "GET");
                    string IResponse = General.MakeRequest(clientUrl + "/api/WalletTransactions/ReceiverVerification/?BankCode=" + BankCode + "&&Accountnumber=" + AccNumber, "", "GET");

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
                        reciResponse = JsonConvert.DeserializeObject<VFDRecipient>(IResponse);

                        if (Convert.ToInt32(reciResponse.status) >= 1)
                        {
                            var response = reciResponse.message;
                            ViewBag.ErrorMessage = response;
                            //return Json(response);
                        }
                        if (reciResponse.data != null)
                        {
                            var data = new
                            {
                                Name = reciResponse.data.name
                            };
                            return Json(data);
                        }
                        else if (reciResponse.data == null)
                        {
                            var data = new
                            {
                                Name = reciResponse.message
                            };
                            return Json(data);
                        }
                    }
                }

            }
            ViewBag.ErrorMessage = "Invalid Bank or Account";
            return null;
        }

        [HttpGet]
        public async Task<JsonResult> WalletVerify(string accnumber)
        {
            if (accnumber != null)
            {
                var getwalletdet = await _unitOfWork.Wallet.GetWalletByAccountNumber(accnumber);
                if (getwalletdet != null)
                {
                    var getuserdet = await _unitOfWork.User.GetSysUsers(Convert.ToString(getwalletdet.User.UserID));


                    if (getuserdet != null)
                    {
                        var data = new
                        {
                            Name = getuserdet.FirstName+" "+getuserdet.MiddleName+" "+getuserdet.LastName 
                        };
                        return Json(data);
                    }
                    else if (getuserdet == null)
                    {
                        var data = new
                        {
                            Name = "Invalid Wallet Account"
                        };
                        return Json(data);
                    }
                }

            }
            ViewBag.ErrorMessage = "Invalid Wallet Account";
            return null;
        }



        [HttpGet]
        public async Task<IActionResult> WalletToWallet()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WalletToWallet(WalletToWalletViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());
            //ViewBag.ListofBanks = await BankList();

            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            //var decryptedString = Helpers.General.DecryptString(key, encryptedString);
            var pin = Helpers.General.EncryptString(key, model.transferpin);

            var _checkuserpinexist = await _unitOfWork.UserToken.GetPin(user2.UserID);
            if (_checkuserpinexist == null)
            {
                ViewBag.ErrorMessage = "Invalid Operation,Transfer Pin not created";
                return View();
            }
            if (_checkuserpinexist.Pin != pin && _checkuserpinexist.UserID != user2.UserID)
            {
                ViewBag.ErrorMessage = "Invalid PIN";
                return View();
            }

            if (model.TransferAmount < 1)
            {
                ViewBag.ErrorMessage = "Invalid Amount Entered, Amount must be greater than 1.";
                ViewBag.Beneficiary = await BeneficiaryList(user2.UserID);
                ViewBag.ListofBanks = await BankList();
                return View();
            }


            if (user2.Role.RoleID == 3 && _checkuserpinexist.Pin == pin && _checkuserpinexist.UserID == user2.UserID)
            { 
                double plustransfee = model.TransferAmount;

                //Get wallet balance  
                //var getwallet = await _unitOfWork.Wallet.GetWalletByAccountNumber(model.WalletAccountNumber);
                //var walletBalance = await _unitOfWork.Invoice.GetWalletBalance(user2.Wallet.WalletID); 
                //double balanceAmount = walletBalance.Sum(u => u.Amount);

                var walletBalance = await _unitOfWork.Wallet.GetWalletBalanceById(user2.Wallet.WalletID);
                double balanceAmount = walletBalance.WalletBalance;

                if (balanceAmount >= plustransfee)
                {
                    var Walletinfo = await _unitOfWork.Wallet.GetUserByWalletID(user2.Wallet.WalletID);
                    var AgentInfo = await _unitOfWork.User.GetSysUsers(Convert.ToString(Walletinfo.User.UserID));
                    double wallettransfercharges = 50;
                    string fromrole = AgentInfo.Role.RoleName;

                    //Deduct from wallet including  transaction fee
                    var DtransactionType = _unitOfWork.TransactionType.Get(2);
                    //generate reference Number
                    var TransReferenceNo = "EgolePay_" + fromrole + "_Transfer" + Guid.NewGuid().ToString("N").Substring(0, 4);   //Guid.NewGuid().ToString().Replace("-", "").Substring(0, 9);
                    var BankchargesTransNo = "Bank Charges on Transfer of" + wallettransfercharges;
                    var walletReferenceNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 9);
                    var wallettransfermethod = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(15);

                    //Creating a record in WalletTransaction of TransactionType(2) - Debit
                    var walletTransaction = new EgsWalletTransaction
                    {
                        Wallet = Walletinfo,
                        //IPAddress = PaymentGateway_V1.1.Helpers.General.GetIPAddressLocal(),
                        TransactionType = DtransactionType,
                        Amount = model.TransferAmount * (-1),
                        TransactionReferenceNo = TransReferenceNo,
                        TransactionDate = DateTime.Now,
                        CreatedBy = Walletinfo.User.UserID,
                        TransactionMethod = wallettransfermethod,
                        WalletReferenceNumber = walletReferenceNo,
                        Previous = balanceAmount,
                        Current = balanceAmount - model.TransferAmount,
                        TransactionStatus = "Completed",
                    };
                    await _unitOfWork.WalletTransaction.AddSaveAsync(walletTransaction);

                    ////Update WalletBalance of the sender
                    walletBalance.User = user2;
                    walletBalance.OpeningBalance = balanceAmount;
                    walletBalance.ClosingBalance = (balanceAmount - model.TransferAmount);
                    walletBalance.WalletBalance = (balanceAmount - model.TransferAmount);
                    _unitOfWork.Wallet.Update(walletBalance);
                    _unitOfWork.Complete();


                    //Log transaction in egole
                    var vfbank = _unitOfWork.Bank.GetBanks(6);
                   // var tranmethod = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(15);
                      
                    var getreceiver = await _unitOfWork.Wallet.GetWalletByAccountNumber(model.WalletAccountNumber);
                    var receiverwalletinfo = await _unitOfWork.Wallet.GetUserByWalletID(getreceiver.WalletID);


                    ////Get Credit transaction method and type
                    var transactionmethod = await _unitOfWork.TransactionMethod.GetTransactionMehodByID(15);
                    var Ctransactiontype = _unitOfWork.TransactionType.Get(1);
                    var Dtransactiontype = _unitOfWork.TransactionType.Get(2);

                    var egoleuser = await _unitOfWork.User.GetUserByEmail("Egolepay@courtevillegroup.com");
                    var Egolewalletinfo = await _unitOfWork.Wallet.GetUserByWalletID(egoleuser.Wallet.WalletID);
                    // credit  Egole transactions log table with transaction fee

                    var sysUserToUpdate = await _unitOfWork.User.GetSysUsers(egoleuser.UserID.ToString());
                    var ReceiverToUpdate = await _unitOfWork.User.GetSysUsers(getreceiver.User.UserID.ToString());

                  //Crediting receiver with  amount less bank charges
                    var erwalletTransaction = new EgsWalletTransaction
                    {
                        Wallet = receiverwalletinfo,
                        //IPAddress = Helpers.General.GetIPAddressLocal(),
                        TransactionType = Ctransactiontype,
                        Amount = model.TransferAmount + (wallettransfercharges * (-1)),
                        TransactionReferenceNo = TransReferenceNo,
                        TransactionDate = DateTime.Now,
                        CreatedBy = ReceiverToUpdate.UserID,//receiverwalletinfo.User.UserID,
                        TransactionMethod = wallettransfermethod,
                        WalletReferenceNumber = walletReferenceNo,
                        Previous = balanceAmount,
                        Current = (balanceAmount + model.TransferAmount) + (wallettransfercharges * (-1)),
                        TransactionStatus = "Completed",
                    };
                    await _unitOfWork.WalletTransaction.AddSaveAsync(erwalletTransaction);

                    var receiverwalletBalance = await _unitOfWork.Wallet.GetWalletBalanceById(ReceiverToUpdate.Wallet.WalletID);
                    double rwalletbalance = receiverwalletBalance.WalletBalance;
                    //Update WalletBalance of the receiver
                    //receiverwalletinfo.User = ReceiverToUpdate;
                    receiverwalletBalance.OpeningBalance = rwalletbalance;
                    receiverwalletBalance.ClosingBalance = (rwalletbalance + model.TransferAmount) + (wallettransfercharges * (-1));
                    receiverwalletBalance.WalletBalance = (rwalletbalance + model.TransferAmount) + (wallettransfercharges * (-1));
                     
                    _unitOfWork.Wallet.Update(receiverwalletBalance);
                    _unitOfWork.Complete();
  

                    //Credit egole bank charges
                    var fwalletTransaction = new EgsWalletTransaction
                    {
                        Wallet = Egolewalletinfo,
                        // IPAddress = Helpers.General.GetIPAddressLocal(),
                        TransactionType = Ctransactiontype,
                        Amount = wallettransfercharges ,
                        TransactionReferenceNo = BankchargesTransNo,
                        TransactionDate = DateTime.Now,
                        CreatedBy = egoleuser.UserID, //receiverwalletinfo.User.UserID,
                        TransactionMethod = wallettransfermethod,
                        WalletReferenceNumber = walletReferenceNo,
                        TransactionStatus = "Completed"
                        
                    };
                    await _unitOfWork.WalletTransaction.AddSaveAsync(fwalletTransaction);


                    //--------------------------------------------------------------------
                    //..................Logging in egolewallettransaction
                    //Sender Debit
                    var senderwalletTransaction = new EgsEgoleWalletTransactions
                    {
                        User = AgentInfo,
                        Wallet = Walletinfo,
                        TransferAmount = (model.TransferAmount * -1),
                        TransactionType = Dtransactiontype,
                        TransactionMethod = transactionmethod,
                        Bank = vfbank.Result,
                        Status = "Completed",
                        DateCreated = DateTime.Now,
                        Narration = model.Narration,
                        ReceiverAccount=model.WalletAccountNumber,
                        ReceiverBank="Wallet-To-Wallet",
                        ReceiverName=model.Receiver
                    };
                    // await _unitOfWork.EgoleWalletTransactions.AddSaveAsync(senderwalletTransaction);
                    _unitOfWork.EgoleWalletTransactions.Add(senderwalletTransaction);
                    _unitOfWork.Complete();

                    //Receivers Credit
                    var recwalletTransaction = new EgsEgoleWalletTransactions
                    {
                        User = ReceiverToUpdate,
                        Wallet = receiverwalletinfo,
                        TransferAmount = model.TransferAmount,
                        TransactionType = Ctransactiontype,
                        TransactionMethod = transactionmethod,
                        Bank = vfbank.Result,
                        Status = "Completed",
                        DateCreated = DateTime.Now,
                        Narration = model.Narration,
                        ReceiverAccount = model.WalletAccountNumber,
                        ReceiverBank = "Wallet-To-Wallet",
                        ReceiverName = model.Receiver
                    };
                    await _unitOfWork.EgoleWalletTransactions.AddSaveAsync(recwalletTransaction);


                    //Receiver Charges Debit 
                    var rechargeswalletTransaction = new EgsEgoleWalletTransactions
                    {
                        User = ReceiverToUpdate,
                        Wallet = receiverwalletinfo,
                        TransferAmount = wallettransfercharges * (-1),
                        TransactionType = Dtransactiontype,
                        TransactionMethod = transactionmethod,
                        Bank = vfbank.Result,
                        Status = "Completed",
                        DateCreated = DateTime.Now,
                        Narration = model.Narration,
                        ReceiverAccount = model.WalletAccountNumber,
                        ReceiverBank = "Wallet-To-Wallet",
                        ReceiverName = model.Receiver
                    };
                    await _unitOfWork.EgoleWalletTransactions.AddSaveAsync(rechargeswalletTransaction);
                     
                    //Egole Credit Charges
                    var egolwalletTransaction = new EgsEgoleWalletTransactions
                    {
                        User = sysUserToUpdate,  
                        Wallet = Egolewalletinfo,
                        TransferAmount = wallettransfercharges,
                        TransactionType = Ctransactiontype,
                        TransactionMethod = transactionmethod,
                        Bank = vfbank.Result,
                        Status = "Completed",
                        DateCreated = DateTime.Now,
                        Narration=model.Narration,
                        ReceiverAccount = model.WalletAccountNumber,
                        ReceiverBank = "Wallet-To-Wallet",
                        ReceiverName = model.Receiver
                    };
                    await _unitOfWork.EgoleWalletTransactions.AddSaveAsync(egolwalletTransaction);


                    TransfersReceiptVM TMODEL = new TransfersReceiptVM();
                    TMODEL.OrderID = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8) + "-" + DateTime.Now.ToString("MMMM dd");
                    TMODEL.Date = DateTime.Now.ToString("dddd, dd MMMM yyyy");
                    TMODEL.Amount = model.TransferAmount;
                    TMODEL.Fee = Convert.ToString(0); 
                    TMODEL.Total = Convert.ToString(plustransfee);
                     
                    return RedirectToAction("TransferWReceipt", "Transfer", TMODEL);

                } 
            }
            ViewBag.Message = "Transfer Not Successful,Kindly retry ";
            ModelState.Clear();
            return View(); 
        }

         

        public async Task<IActionResult> TransferWReceipt(TransfersReceiptVM model)
        {

            ViewBag.Date = model.Date;
            ViewBag.OrderID = model.OrderID;
            ViewBag.Amount = model.Amount;
            ViewBag.Fee = model.Fee;
            ViewBag.Total = model.Total;

            ViewBag.Message = model.message;
            return View();
        }



        public async Task<IActionResult> PinChange()
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            return View();
        }



        public async Task<List<SysBank>> BankList()
        {
            List<SysBank> bankList = new List<SysBank>();

            bankList = await _unitOfWork.Bank.GetAllBanks();       //GetAllForDropdown();

            bankList.Insert(0, new SysBank { BankCode = "", BankName = "Select Bank" });

            return bankList;
        }

        public async Task<List<EgsBeneficiary>> BeneficiaryList(int userid)
        {
            List<EgsBeneficiary> BeneficiaryList = new List<EgsBeneficiary>();

           var Beneficiarys = await _unitOfWork.Beneficiary.GetBeneficiaryByUserID(userid);       //GetAllForDropdown();

            foreach (var Items in Beneficiarys)
            {
                BeneficiaryList.Add(new EgsBeneficiary
                {
                    BeneficaryID = Items.BeneficaryID,
                    BeneficiaryName = Items.BeneficiaryName +  " (" + Items.BeneficiaryBankAccount + ")"
                });
            }
             
            BeneficiaryList.Insert(0, new EgsBeneficiary { BeneficaryID = 0, BeneficiaryName = "Select Beneficiary" });

            //BeneficiaryList = Beneficiarys;

            return BeneficiaryList;
             
        }



        public async Task<JsonResult> InsertBeneficiary([FromBody] TransferViewModel transferModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var user2 = await _unitOfWork.User.GetSysUsers(user.UserID.ToString());

            var checkbeneficiaryexist = await _unitOfWork.Beneficiary.GetBeneficiaryByAcct(transferModel.toAccount);

            if (checkbeneficiaryexist != null)
            {

                return Json("Beneficiary already exists");
            }

            if (checkbeneficiaryexist == null)
            {
                var getbank = await _unitOfWork.Bank.GetBanksByCode(transferModel.toBank);
                await _unitOfWork.Beneficiary.AddSaveAsync(new EgsBeneficiary
                {
                    Bank = getbank,
                    BeneficiaryBankAccount = transferModel.toAccount,
                    BeneficiaryName = transferModel.Receiver,
                    userId = user2.UserID,
                    Datecreated = DateTime.Now
                });

                return Json("Beneficiary has been saved successfully");
            }
            return Json("Beneficiary Insert Failed");
        }

        [HttpGet]
        public async Task<JsonResult> VerifyBeneficiary(int benfid)
        {
            if (benfid != 0)
            {
                var getbeneficiary = await _unitOfWork.Beneficiary.GetBeneficiaryByBenfID(benfid);
                var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                var clientUrl = APIServiceConfig?.Url;
                //VerificationViewModel verifyresponse = new VerificationViewModel();
                VFDRecipient reciResponse = new VFDRecipient();


                using (var client = new HttpClient())
                {
                    var Srequest = (clientUrl + "/api/WalletTransactions/ReceiverVerification/?BankCode=" + getbeneficiary.Bank.BankCode + "&&Accountnumber=" + getbeneficiary.BeneficiaryBankAccount, "", "GET");
                    string IResponse = General.MakeRequest(clientUrl + "/api/WalletTransactions/ReceiverVerification/?BankCode=" + getbeneficiary.Bank.BankCode + "&&Accountnumber=" + getbeneficiary.BeneficiaryBankAccount, "", "GET");

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
                        reciResponse = JsonConvert.DeserializeObject<VFDRecipient>(IResponse);

                        if (Convert.ToInt32(reciResponse.status) >= 1)
                        {
                            var response = reciResponse.message;
                            ViewBag.ErrorMessage = response;
                            //return Json(response);
                        }
                        if (reciResponse.data != null)
                        {
                            var data = new
                            {
                                Name = reciResponse.data.name
                            };
                            return Json(data);
                        }
                        else if (reciResponse.data == null)
                        {
                            var data = new
                            {
                                Name = reciResponse.message
                            };
                            return Json(data);
                        }
                    }
                }

            }
            ViewBag.ErrorMessage = "Invalid Beneficiary ";
            return null;
        }




    }
}
