using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleBankingSystemAPI.Interfaces.Services;
using System.Net.Mail;
using System.Threading.Tasks;
using WatchDog;

namespace SimpleBankingSystemAPI.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The content of the email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendEmailAsync(string email, string subject, string message)
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

            var mailMessage = new MailMessage(useremail, email, subject, message)
            {
                IsBodyHtml = true
            };

            try
            {
                WatchLogger.Log($"Attempting to send email to {email} with subject {subject}");
                await client.SendMailAsync(mailMessage);
                WatchLogger.Log($"Email sent successfully to {email} with subject {subject}");
            }
            catch (Exception ex)
            {
                WatchLogger.Log($"Failed to send email to {email} with subject {subject}\n{ex}");
                throw;
            }
        }
    }
}
