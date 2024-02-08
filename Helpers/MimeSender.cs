using MimeKit;
using PaymentGateway21052021.Helpers.Interface;
using System;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace PaymentGateway21052021.Helpers
{
    public class MimeSender : IMimeSender
    {//void
        public async Task Execute(string apiKey, string subject, string msg, string email, string attachment = null, string CopyMail = null)
        {
            try
            {
                var message = new MimeMessage();
                var bodyBuilder = new BodyBuilder();
                //var _deploy = JsonConvert.DeserializeObject<IDeploySettings>(Configuration["DeploySettings"]) ;
                // from
                message.From.Add(new MailboxAddress("hello@egolepay", "hello@egolepay.com" /*"taslim.adiatu@courtevillegroup.com"*/));
                // to
                message.To.Add(new MailboxAddress(email, email));
                // reply to
                //message.ReplyTo.Add(new MailboxAddress("reply_name", "reply_email@example.com"));

                message.Subject = subject;
                bodyBuilder.HtmlBody = msg;

                if (!string.IsNullOrEmpty(attachment))
                {
                    // We may also want to attach a calendar event for Monica's party...
                    bodyBuilder.Attachments.Add(attachment);
                }
                if (!string.IsNullOrEmpty(CopyMail))
                {
                    message.Cc.Add(new MailboxAddress(CopyMail, CopyMail));
                }
                 
                message.Body = bodyBuilder.ToMessageBody();
                 
                var client = new SmtpClient();

               client.ServerCertificateValidationCallback = (s, c, h, e) => true;
               //client.Connect("cloud.hostbility.com", 26, false);
               client.Connect("smtp.sendgrid.net", 465, true); 
                //client.Connect("MAIL_SERVER", 465, SecureSocketOptions.SslOnConnect);
                //client.Authenticate("no-reply@egolepay.com", "P@33word@");
                client.Authenticate("apikey", "AdminSG.YpIdCqS5RQOd7mXqf3ygag.2XRKWPi3_agKITDqce1JM9f_E4fRbY3oxfPeeJ9wlBE123");
                client.Send(message);
                client.Disconnect(true); 
            }
            catch (Exception ex)
            {

            }
            //return Task.FromResult();
        }

    }
}
