using System;
using System.Linq;
using BlazorApp.DbModel;
using BlazorApp.Models;
using Microsoft.AspNetCore.Http;
using BlazorApp.Interfaces;
using BlazorApp.Models;
using BlazorApp.Models.Response;
using BlazorApp.DbModel;
using Microsoft.EntityFrameworkCore;

public class PaymentProcessor : IPaymentGateway
{
    private readonly ApplicationDbContext _context;
    private readonly INotification _notification;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PaymentProcessor(ApplicationDbContext context, INotification notification, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _notification = notification;
        _httpContextAccessor = httpContextAccessor;
    }
    public decimal TotalPrice(Guid userId)
    {
        var userCart =  _context.Cart
       .Include(c => c.CartItems) 
       .FirstOrDefault(c => c.UserId == userId);

        if (userCart == null) 
        {
            throw new Exception("The cart wasn't found");
        }
 
        return userCart.Total;
    }
    public PaymentResponse ProcessPayment(Guid userId, PaymentDetails paymentDetails)
    {
        try
        {
            // Retrieve the user by their ID
            var user = _context.Users.Find(userId);

            // Check if the user exists
            if (user == null)
            {
                return new PaymentResponse { Message = "User not found." };
            }

            // Validate payment details
            if (paymentDetails == null || string.IsNullOrEmpty(paymentDetails.CardNumber) ||
                string.IsNullOrEmpty(paymentDetails.CardHolderName) || paymentDetails.ExpiryDate == default ||
                string.IsNullOrEmpty(paymentDetails.CVV) || paymentDetails.TotalPrice <= 0)
            {
                return new PaymentResponse { Message = "Invalid payment details." };
            }

            // Simulate payment processing logic
            var cart = _context.Cart.SingleOrDefault(c => c.UserId == userId);

            // If the cart exists, remove it from the database
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }

            // Generate a unique transaction ID
            var transactionId = Guid.NewGuid().ToString();

            // Save the payment details in the database
            var paymentRecord = new PaymentRecord
            {
                Id = Guid.NewGuid(),
                CardNumber = paymentDetails.CardNumber, // Ensure this is stored securely
                CardHolderName = paymentDetails.CardHolderName,
                ExpiryDate = paymentDetails.ExpiryDate,
                CVV = paymentDetails.CVV, // Ensure this is stored securely
                Amount = paymentDetails.TotalPrice,
                Currency = paymentDetails.Currency,
                TransactionId = transactionId,
                DateProcessed = DateTime.UtcNow
            };

            // Add the payment record to the database and save changes
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.PaymentRecords.Add(paymentRecord);
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw; 
                }
            }

            // Send a notification to the user about the payment
            var message = $"Dear {user.Username}, your payment of {paymentDetails.TotalPrice} {paymentDetails.Currency} has been processed successfully. Transaction ID: {transactionId}.";
            _notification.SendNotification(message, user.Email);

            // Return a successful payment response
            return new PaymentResponse
            {
                Success = true,
                TransactionId = transactionId,
                Message = "Payment processed successfully."
            };
        }
        catch (Exception ex)
        {
    

            // Return a failure response with the error message
            return new PaymentResponse
            {
                Success = false,
                Message = $"Payment processing failed: {ex.Message}"
            };
        }
    }



    public bool ProcessRefund(string transactionId)
    {
        try
        {
            var userIdString = _httpContextAccessor.HttpContext.Session.GetString("UserId");

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                throw new Exception("Invalid User ID.");
            }

            var user = _context.Users.Find(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Simulate refund logic
            var paymentRecord = _context.PaymentRecords.FirstOrDefault(pr => pr.TransactionId == transactionId);
            if (paymentRecord == null)
            {
                throw new Exception("Transaction not found.");
            }

            // Update the record to indicate the refund
            paymentRecord.IsRefunded = true;
            paymentRecord.DateRefunded = DateTime.UtcNow;

            _context.SaveChanges();

            // Send notification to the user
            var message = $"Dear {user.Username}, your refund for transaction ID {transactionId} has been processed.";
            _notification.SendNotification(message, user.Email);

            return true;
        }
        catch (Exception ex)
        {
            // Log exception (consider using a logging framework)
            Console.WriteLine($"Error processing refund: {ex.Message}");
            return false;
        }
    }


 
}
