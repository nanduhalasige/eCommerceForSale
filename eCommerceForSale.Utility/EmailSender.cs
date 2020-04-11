using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceForSale.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions _emailOptions;

        public EmailSender(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(_emailOptions.SendGridKey, subject, htmlMessage, email);
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage, string toName = "End user")
        {
            return Execute(_emailOptions.SendGridKey, subject, htmlMessage, email, toName);
        }

        private Task Execute(string sendGridKey, string subject, string message, string email, string toName = "End user")
        {
            if (!(_emailOptions.EmailFrom.ToLower().Contains("@gmail.com") || _emailOptions.EmailFrom.ToLower().Contains("@yahoo.com")))
            {
                var client = new SendGridClient(sendGridKey);
                var from = new EmailAddress(_emailOptions.EmailFrom, _emailOptions.FromName);
                var to = new EmailAddress(email, toName);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", message);
                return client.SendEmailAsync(msg);
            }
            else
            {
                var fromAddress = new MailAddress(_emailOptions.EmailFrom, _emailOptions.FromName);
                var toAddress = new MailAddress(email, toName);
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(fromAddress.Address, _emailOptions.EmailPassword)
                };
                using (var msg = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                })
                    smtp.Send(msg);
                return Task.CompletedTask;
            }
        }
    }
}