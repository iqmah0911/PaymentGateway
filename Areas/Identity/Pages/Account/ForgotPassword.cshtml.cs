using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentGateway21052021.Areas.Dashboard.Models;
using PaymentGateway21052021.Helpers;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories;
using PaymentGateway21052021.Helpers.Interface;


namespace PaymentGateway21052021.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        //private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IMimeSender _mimeSender;
        private readonly ISendGridmail _sendgridmail;
       private IConfiguration configuration;
             

        private readonly UserManager<ApplicationUser> _userManager;
        private IUnitOfWork _unitOfWork;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender,
            IMimeSender mimeSender, IUnitOfWork unitOfWork,ISendGridmail sendGridmail,IConfiguration iconfig)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _sendgridmail = sendGridmail;
            _mimeSender = mimeSender;
           configuration = iconfig;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                //checking user's currently logged in 
                //var user = await _unitOfWork.User.GetUserByEmail(Input.Email);
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null )
                {
                    ViewData["Error"] = "Email does not exist";

                    return Page();
                    // Don't reveal that the user does not exist or is not confirmed Confirmation
                    //return RedirectToPage("./ForgotPassword");
                }

                string dconn = configuration.GetSection("EImage").GetSection("data").Value;
                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);
 
   
 
                var Message12 = $"<html>" +
                $"<body>"+ 
                $"<h3><p>Hello there,</p></h3>" +
                $"<p>You recently requested a password reset.To complete your request, please <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>click here</a> to set up your new password.</p>" +
                 // $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>." +
                 $"<p>Please ignore if this was not initiated by you.</p>" +
                  
                  $"<p><b>Payment made easy.</b></p>" +
                  //$"<img src='{dconn}'/>" +
                $"</body>" +
                $"</html";


                await _sendgridmail.Execute(Input.Email, "Reset Password",Message12 );

                // await _sendgridmail.Execute(Input.Email, "Reset Password",
                //$"<html>" +
                //$"<body> <h3><p> Your order is successful!</p></h3>" +
                //$"<p> Order Date: 8 / 30 / 2020 </p>" +
                //$"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                //);




                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                //await _mimeSender.Execute("", "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                //Input.Email);



                //            await _sendgridmail.Execute(Input.Email, "Reset Password",
                //$"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");




                ////Api call for mail sending
                //var APIServiceConfig = Startup.StaticConfig.GetSection("EgolePayService").Get<EgolePayService>();
                //var clientUrl = APIServiceConfig?.stageUrl;

                //var upgradeResponse = new UpgradeAccountResponse();

                //PostMailViewModel paramsBody = new PostMailViewModel()
                //{
                //    email = Input.Email,
                //    subject = "Reset Password",
                //    htmlMessage = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                //};

                //var bodyRequest = JsonConvert.SerializeObject(paramsBody, Formatting.Indented);

                //using (var _client = new HttpClient())
                //{
                //    var Srequest = (clientUrl + "/api/Account/ProcessMail", "POST", bodyRequest);
                //    string IResponse = General.MakeVFDRequest(clientUrl + "/api/Account/ProcessMail", null, "POST", null, bodyRequest);

                //    //Save Request and Response
                //    var RequestResponseLog = new SysRequestResponseLog
                //    {
                //        Request = Srequest.ToString(),
                //        Response = IResponse
                //    };
                //    await _unitOfWork.RequestResponseLog.AddSaveAsync(RequestResponseLog);

                //    if (!String.IsNullOrEmpty(IResponse))
                //    {
                //        upgradeResponse = JsonConvert.DeserializeObject<UpgradeAccountResponse>(IResponse);

                //        if (Convert.ToInt32(upgradeResponse.status) >= 1)
                //        {
                //            ViewData["Message"] = "Response " + upgradeResponse.message;
                //            return RedirectToPage("./ForgotPassword");
                //        }
                //        else
                //        { 
                //            ViewData["Message"] = upgradeResponse.message;
                //            return RedirectToPage("./ForgotPasswordConfirmation");
                //        }
                //    }
                //} 
                return RedirectToPage("./ForgotPasswordConfirmation");
               // return RedirectToPage("./ForgotPassword");
            }

            return Page();
        }
    }
}
