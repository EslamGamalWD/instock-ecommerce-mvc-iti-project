using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Helpers.Account
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSetting mailSetting;

        public EmailSender(IOptions<MailSetting> mailSetting)
        {
            this.mailSetting=mailSetting.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            MailMessage message = new MailMessage()
            {
                From = new MailAddress(mailSetting.Email!,mailSetting.DisplayName),
                Body = htmlMessage,
                Subject = subject,
                IsBodyHtml = true
            };
            message.To.Add(email);
            SmtpClient smtpClient = new(mailSetting.Host)
            {

                Port =mailSetting.Port,
                Credentials = new NetworkCredential(mailSetting.Email, mailSetting.Password),
                EnableSsl=true
            };
            await smtpClient.SendMailAsync(message);
            smtpClient.Dispose();
        }
    }
}
