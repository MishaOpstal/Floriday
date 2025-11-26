using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LeafBidAPI.Services
{
    public class EmailSender : IEmailSender<Models.User>
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendConfirmationLinkAsync(Models.User user, string email, string confirmationLink)
        {
            return Execute(_emailSettings.Subject, $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>link</a>", email);
        }

        public Task SendPasswordResetLinkAsync(Models.User user, string email, string resetLink)
        {
            return Execute(_emailSettings.Subject, $"Please reset your password by clicking this link: <a href='{resetLink}'>link</a>", email);
        }

        public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(subject, htmlMessage, email);
        }

        public async Task Execute(string subject, string message, string email)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email) ? _emailSettings.ToEmail : email;
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, "LeafBid")
                };
                mail.To.Add(new MailAddress(toEmail));
                mail.Subject = "LeafBid - " + subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (System.Exception ex)
            {
                // TODO: Log this exception
                throw ex;
            }
        }
    }

    public class DummyEmailSender : IEmailSender<Models.User>
    {
        public Task SendConfirmationLinkAsync(Models.User user, string email, string confirmationLink)
        {
            return Task.CompletedTask;
        }

        public Task SendPasswordResetLinkAsync(Models.User user, string email, string resetLink)
        {
            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            return Task.CompletedTask;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }

    public class EmailSettings
    {
        public string PrimaryDomain { get; set; }
        public int PrimaryPort { get; set; }
        public string UsernameEmail { get; set; }
        public string UsernamePassword { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
    }
}