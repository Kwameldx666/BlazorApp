using BlazorApp.Models;
using BlazorApp.Models;
using BlazorApp.Models.Response;

namespace BlazorApp.Interfaces
{
    public interface IReservationPaymentProcessor
    {
        // Processes a payment for a reservation using the provided payment details
        PaymentResponse ProcessPayment(Reservation reservation, PaymentDetails paymentDetails);

        // Refunds a payment for a reservation identified by its reservation ID
        PaymentResponse RefundPayment(Guid reservationId);
    }
}
