using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
namespace eTickets.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> LoadEmailTemplateAsync(string templatePath, string name, string link)
        {
            string content = await File.ReadAllTextAsync(templatePath);
            content = content.Replace("{{name}}", name);
            content = content.Replace("{{link}}", link);
            return content;
        }
        public async Task SendEmailAsync(string toemail, string subject, string htmlContent)
        {
            var emailMesaage = new MimeMessage();
            emailMesaage.From.Add(new MailboxAddress("Admin", _config["Smtp:SenderEmail"]));
            emailMesaage.To.Add(new MailboxAddress("", toemail));
            emailMesaage.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlContent };

            emailMesaage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_config["Smtp:Server"], int.Parse(_config["Smtp:Port"]), SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_config["Smtp:SenderEmail"], _config["Smtp:SenderPassword"]);
                    await client.SendAsync(emailMesaage);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
