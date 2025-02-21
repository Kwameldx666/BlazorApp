using BlazorApp.Interfaces;

namespace BlazorApp.Server.Repository
{
    public interface IRepositoryInterfacesReservation
    {
        public  IReservation CreateReservation();
        public INotification CreateNotification();
        public  IReservationPaymentProcessor CreatePaymentReservation();
    }
}
