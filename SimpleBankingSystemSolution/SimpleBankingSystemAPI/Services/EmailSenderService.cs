using Microsoft.Extensions.Configuration;
using SimpleBankingSystemAPI.Interfaces.Services;
using System.Net.Mail;

namespace SimpleBankingSystemAPI.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("Email");
            var useremail = emailSettings["mail"];
            var password = emailSettings["password"];

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential(useremail, password),
                EnableSsl = true,
            };

            // Use the retrieved email and password to send the email

            return client.SendMailAsync(
                               new MailMessage(useremail, email, subject, message)
                               {
                                   IsBodyHtml = true
                               }
                                        );
        }
    }
}
