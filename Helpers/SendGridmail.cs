using System;
using System.Threading.Tasks;
using PaymentGateway21052021.Helpers.Interface;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PaymentGateway21052021.Helpers
{
    public class SendGridmail: ISendGridmail
    {
        
        private string MESSAGEFOOTER = $"<br/><br/><span style='font-size:10px;color:#999;'>Copyright © {DateTime.Now.Year} EgolePay, All rights reserved.</span>";
         
        public async Task Execute(string email, string subject, string htmlMessage)
        {
            try
            {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("hello@egolepay.com", "EGOLEPAY");

                var to = new EmailAddress(email, email);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage + MESSAGEFOOTER);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                string exception = ex.ToString();
            }
        }
    }
}
