using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Helpers
{
    public class EmailSender : IEmailSender
    {

        private const string MAILSERVERHOST = "smtp.sendgrid.net"; //"smtp.gmail.com"; // "cloud.hostbility.com"; //mail.egolepay.com  ;
        private const int MAILSERVERPORT = 25; // 587((for unencrypted/TLS connections));//465	(for SSL connections)  
        private const string MAILSERVERUSERNAME = "apikey";    //"hello@egolepay.com";   //
        private const string MAILSERVERPASSWORD = "AdminSG.YpIdCqS5RQOd7mXqf3ygag.2XRKWPi3_agKITDqce1JM9f_E4fRbY3oxfPeeJ9wlBE123";  ///"P@33word@";  //4
        private const string DISPLAYNAME = "EgolePay";
        private string MESSAGEFOOTER = $"<br/><br/><span style='font-size:10px;color:#999;'>Copyright © {DateTime.Now.Year} EgolePay, All rights reserved.</span>";

      
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                MailAddress AdOwnerAddress = new MailAddress(email.Trim());

                using (SmtpClient smtpClient = new SmtpClient(MAILSERVERHOST, MAILSERVERPORT))
                {

                    smtpClient.UseDefaultCredentials = false; //false;

                    smtpClient.Credentials = new System.Net.NetworkCredential(MAILSERVERUSERNAME, MAILSERVERPASSWORD);
                    smtpClient.EnableSsl = false;//true;
                    smtpClient.Timeout = 20000;

                    using (MailMessage msg = new MailMessage())
                    {
                        msg.From = new MailAddress(MAILSERVERUSERNAME, DISPLAYNAME);
                        msg.IsBodyHtml = true;
                        msg.To.Add(AdOwnerAddress);
                        msg.Subject = subject;
                        msg.Body = htmlMessage + MESSAGEFOOTER;

                        await smtpClient.SendMailAsync(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                string exception = ex.ToString();
            }
        }


         
    }
}
