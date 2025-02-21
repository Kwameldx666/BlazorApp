using System.Net.Mail;
using System.Net;
using BlazorApp.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BlazorApp.Server.Repository
{
    public class VipNotification : INotification
    {
        private static readonly Lazy<VipNotification> _instance = new(() => new());
        private readonly IConfiguration _configuration;

        private VipNotification()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static VipNotification Instance => _instance.Value;

        public void SendNotification(string message, string recipientEmail)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];
            var password = _configuration["EmailSettings:Password"];

            // VIP-уведомление с уникальной темой и HTML-оформлением
            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = "VIP Notification: Exclusive Update",
                Body = $"<h2>Dear VIP,</h2><p>{message}</p><p>Thank you for being a valued VIP member!</p>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(recipientEmail);

            using (var smtpClient = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = port == 587
            })
            {
                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("VIP notification sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending VIP notification: {ex.Message}");
                }
            }
        }
    }

    public class RegularUserNotification : INotification
    {
        private static readonly Lazy<RegularUserNotification> _instance = new(() => new());
        private readonly IConfiguration _configuration;

        private RegularUserNotification()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static RegularUserNotification Instance => _instance.Value;

        public void SendNotification(string message, string recipientEmail)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];
            var password = _configuration["EmailSettings:Password"];

            // Простое уведомление для обычных пользователей
            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = "Notification",
                Body = message,
                IsBodyHtml = false // Обычный текст
            };

            mailMessage.To.Add(recipientEmail);

            using (var smtpClient = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = port == 587
            })
            {
                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Regular user notification sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending regular user notification: {ex.Message}");
                }
            }
        }
    }
}
