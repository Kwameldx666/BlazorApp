using BlazorApp.Models;
using BlazorApp.Models.Response;

public interface IPaymentGateway
{
    // Processes a payment based on user ID and payment details
    PaymentResponse ProcessPayment(Guid userId, PaymentDetails paymentDetails);

    // Processes a refund for a specific transaction
    bool ProcessRefund(string transactionId);
    decimal TotalPrice(Guid userId);
}
