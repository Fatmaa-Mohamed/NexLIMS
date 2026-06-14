using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var smtpHost = _config["Email:Host"];
            var smtpPort = int.Parse(_config["Email:Port"]);
            var smtpUser = _config["Email:Username"];
            
            var smtpPass = _config["Email:Password"];
            var smtpFrom = _config["Email:From"]; // ← add this

            if (string.IsNullOrEmpty(smtpHost)) throw new Exception("Email:Host is null");
            if (string.IsNullOrEmpty(smtpUser)) throw new Exception("Email:Username is null");
            if (string.IsNullOrEmpty(smtpPass)) throw new Exception("Email:Password is null");


            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var mail = new MailMessage(smtpFrom, toEmail, subject, body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mail);
        

    }
    }
}
