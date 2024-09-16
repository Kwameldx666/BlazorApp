using Microsoft.AspNetCore.Mvc;
using BlazorApp.Interfaces;
using BlazorApp.Models;
using BlazorApp.Models.Response;
using System;
using System.Threading.Tasks;
using ISession = BlazorApp.Interfaces.ISession;
using BlazorApp.Shared.Models;
using AutoMapper;

namespace BlazorApp.Controllers
{
    [Route("api/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservation _reservationService;
        private readonly ISession _sessionService;
        private readonly IMapper _mapper;

        public ReservationController(IReservation reservationService, ISession sessionService,IMapper mapper)
        {
            _reservationService = reservationService;
            _sessionService = sessionService;
            _mapper = mapper;
        }

        // GET: api/reservation/book
        [HttpGet("book")]
        public IActionResult Book()
        {
            var userId = _sessionService.GetUserId();

            // Check if the user ID is valid
            if (userId == Guid.Empty)
            {
                return Unauthorized(new { message = "User not logged in." });
            }

            // Return a successful response indicating the booking view can be accessed
            return Ok(new { message = "User is authenticated and can access booking." });
        }

        // POST: api/reservation/submit
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitReservation([FromBody] ReservationDto reservationData)
        {
            var reservation = _mapper.Map<Reservation>(reservationData);
            var userId = _sessionService.GetUserId();

            // Check if the user ID is valid
            if (userId == Guid.Empty)
            {
                return BadRequest(new { message = "User not found. Please log in." });
            }

            // Process the reservation
            ReservationResponse reservationResponse =  _reservationService.CreateReservation(reservation, userId);

            // Check the result of the reservation processing
            if (reservationResponse.Status)
            {
                return Ok(new { success = true, message = "Reservation successfully created." });
            }
            else
            {
                return BadRequest(new { success = false, message = reservationResponse.Message });
            }
        }

        // GET: api/reservation/confirmation
        [HttpGet("confirmation")]
        public IActionResult GetConfirmation()
        {
            // Return a confirmation response
            return Ok(new { message = "Reservation successfully confirmed." });
        }
    }
}
