using BlazorApp.DbModel;
using BlazorApp.Interfaces;
using BlazorApp.Models;
using BlazorApp.Models.Response;
using BlazorApp.Models.Enums;
using System;
using System.Linq;

namespace BlazorApp.Server.Repository
{
    public class VipReservationPayment : IReservationPaymentProcessor
    {
        private readonly ApplicationDbContext _context;

        public VipReservationPayment(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public PaymentResponse ProcessPayment(Reservation reservation, PaymentDetails paymentDetails, Guid userId)
        {
            if (reservation == null)
                return new PaymentResponse { Success = false, Message = "Reservation cannot be null." };

            if (paymentDetails == null)
                return new PaymentResponse { Success = false, Message = "Payment details are required." };

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return new PaymentResponse { Success = false, Message = "User not found." };

            if (reservation.Status == ReservationStatus.Paid)
                return new PaymentResponse { Success = false, Message = "Reservation is already paid." };

            // VIP получает скидку 10%
            decimal discount = reservation.Amount * 0.10m;
            reservation.Amount -= discount;

            reservation.Status = ReservationStatus.Paid;
            _context.SaveChanges();

            return new PaymentResponse { Success = true, Message = $"VIP Payment successful. Discount applied: {discount:C}" };
        }

        public PaymentResponse RefundPayment(Guid reservationId, Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return new PaymentResponse { Success = false, Message = "User not found." };

            var reservation = _context.Reservations.FirstOrDefault(r => r.TransactionId == reservationId);
            if (reservation == null)
                return new PaymentResponse { Success = false, Message = "Reservation not found." };

            if (reservation.Status != ReservationStatus.Paid)
                return new PaymentResponse { Success = false, Message = "Reservation is not paid, cannot refund." };

            // VIP получает 100% возврат средств
            reservation.Status = ReservationStatus.Canceled;
            _context.SaveChanges();

            return new PaymentResponse { Success = true, Message = "VIP Refund successful. Full amount refunded." };
        }
    }

    public class RegularUserReservationPayment : IReservationPaymentProcessor
    {
        private readonly ApplicationDbContext _context;

        public RegularUserReservationPayment(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public PaymentResponse ProcessPayment(Reservation reservation, PaymentDetails paymentDetails, Guid userId)
        {
            if (reservation == null)
                return new PaymentResponse { Success = false, Message = "Reservation cannot be null." };

            if (paymentDetails == null)
                return new PaymentResponse { Success = false, Message = "Payment details are required." };

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return new PaymentResponse { Success = false, Message = "User not found." };

            if (reservation.Status == ReservationStatus.Paid)
                return new PaymentResponse { Success = false, Message = "Reservation is already paid." };

            // Обычные пользователи платят полную сумму
            reservation.Status = ReservationStatus.Paid;
            _context.SaveChanges();

            return new PaymentResponse { Success = true, Message = "Payment successful." };
        }

        public PaymentResponse RefundPayment(Guid reservationId, Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return new PaymentResponse { Success = false, Message = "User not found." };

            var reservation = _context.Reservations.FirstOrDefault(r => r.TransactionId == reservationId);
            if (reservation == null)
                return new PaymentResponse { Success = false, Message = "Reservation not found." };

            if (reservation.Status != ReservationStatus.Paid)
                return new PaymentResponse { Success = false, Message = "Reservation is not paid, cannot refund." };

            // Обычные пользователи получают только 50% возврата
            decimal refundAmount = reservation.Amount * 0.50m;
            reservation.Amount -= refundAmount;
            reservation.Status = ReservationStatus.Canceled;
            _context.SaveChanges();

            return new PaymentResponse { Success = true, Message = $"Refund successful. 50% refunded: {refundAmount:C}" };
        }
    }
}


