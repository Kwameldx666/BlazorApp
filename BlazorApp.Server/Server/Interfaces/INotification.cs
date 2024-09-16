using BlazorApp.Models.Response;

namespace BlazorApp.Interfaces
{
    public interface INotification
    {
        // Sends a notification with a message to the specified recipient via email
        void SendNotification(string message, string recipientEmail);
    }
}
