using BlazorApp.Models;

namespace BlazorApp.Models.Response
{
    public class ReservationResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public Reservation Reservation { get; set; }
    }
}
