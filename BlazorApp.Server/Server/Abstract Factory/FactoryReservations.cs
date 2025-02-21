using BlazorApp.DbModel;
using BlazorApp.Interfaces;
using BlazorApp.Repository;
using BlazorApp.Server.Repository;

namespace BlazorApp.Server.Abstract_Factory
{
    public class RegularUserReservationsFactory: IRepositoryInterfacesReservation
    {
        private readonly ApplicationDbContext _context;
        private readonly INotification _notification;

        public RegularUserReservationsFactory(ApplicationDbContext context, INotification notification)
        {
            _context = context;
            _notification = notification;
        }
        public IReservation CreateReservation()
        {
           return new RegularUserReservation(_context,_notification);
        }

        public INotification CreateNotification() => RegularUserNotification.Instance;

        public IReservationPaymentProcessor CreatePaymentReservation() => new RegularUserReservationPayment(_context);

    } 
    public class VipReservationsFactory: IRepositoryInterfacesReservation
    {
        private readonly ApplicationDbContext _context;
        private readonly INotification _notification;

        public VipReservationsFactory(ApplicationDbContext context, INotification notification)
        {
            _context = context;
            _notification = notification;
        }
        public IReservation CreateReservation()
        {
           return new VipReservation(_context,_notification);
        }

        public INotification CreateNotification() => VipNotification.Instance;

        public IReservationPaymentProcessor CreatePaymentReservation() => new VipReservationPayment(_context);

    }
}
