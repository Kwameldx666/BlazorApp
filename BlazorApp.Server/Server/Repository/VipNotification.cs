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

        public bool SendNotification(string message, string recipientEmail)
        {
            try
            {
                // Извлекаем настройки
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderName = _configuration["EmailSettings:SenderName"];
                var password = _configuration["EmailSettings:Password"];

                if (!int.TryParse(_configuration["EmailSettings:Port"], out int port))
                {
                    Console.WriteLine("Invalid SMTP port in configuration.");
                    return false;
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = "Notification",
                    Body = message,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(recipientEmail);

                using (var smtpClient = new SmtpClient(smtpServer, port))
                {
                    smtpClient.Credentials = new NetworkCredential(senderEmail, password);
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;

                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Regular user notification sent successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending regular user notification: {ex}");
                return false;
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

        public bool SendNotification(string message, string recipientEmail)
        {
            try
            {
                // Извлекаем настройки
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderName = _configuration["EmailSettings:SenderName"];
                var password = _configuration["EmailSettings:Password"];

                if (!int.TryParse(_configuration["EmailSettings:Port"], out int port))
                {
                    Console.WriteLine("Invalid SMTP port in configuration.");
                    return false;
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = "Notification",
                    Body = message,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(recipientEmail);

                using (var smtpClient = new SmtpClient(smtpServer, port))
                {
                    smtpClient.Credentials = new NetworkCredential(senderEmail, password);
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;

                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Regular user notification sent successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending regular user notification: {ex}");
                return false;
            }
        
    }
    }
}
