using System.Net;
using System.Net.Mail;
using BlazorApp.Interfaces;

namespace BlazorApp.Repository
{

    public class NotificationService : INotification
    {
        // Singleton instance
        private static readonly Lazy<NotificationService> _instance =
            new(() => new());

        private readonly IConfiguration _configuration;

        // Private constructor for Singleton pattern
        private NotificationService()
        {
            // Load configuration
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        // Public property to access the Singleton instance
        public static NotificationService Instance => _instance.Value;

        // Method to send notifications
        public bool SendNotification(string messageSend, string email)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderName = _configuration["EmailSettings:SenderName"];
                var appPassword = _configuration["EmailSettings:Password"]; // Пароль приложения!

                if (!int.TryParse(_configuration["EmailSettings:Port"], out int port))
                {
                    Console.WriteLine("Invalid SMTP port in configuration.");
                    return false;
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = "Notification",
                    Body = messageSend,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(email);

                using (var smtpClient = new SmtpClient(smtpServer, port))
                {
                    smtpClient.Credentials = new NetworkCredential(senderEmail, appPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;

                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully.");
                    return true;
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP Error: {ex.StatusCode} - {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex}");
                return false;
            }
        }
        }
}


