using BlazorApp.Models;

namespace BlazorApp.Interfaces
{
    public interface IReservationNotifier
    {
        // Sends a confirmation notification for a reservation
        void SendReservationConfirmation(Reservation reservation);

        // Sends a cancellation notification for a reservation
        void SendReservationCancellation(Reservation reservation);
    }
}
