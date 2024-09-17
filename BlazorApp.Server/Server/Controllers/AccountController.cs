using BlazorApp.DbModel;
using BlazorApp.Interfaces;
using BlazorApp.Models.Enums;
using BlazorApp.Models;
using Microsoft.AspNetCore.Mvc;
using ISession = BlazorApp.Interfaces.ISession;
using AutoMapper;
using BlazorApp.Shared.Models;

namespace BlazorApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IDishes _dishes;
        private readonly ISession _sessionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICartService _cartService;
        private readonly IUser _userService;
        private readonly IReservation _reservation;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        // Constructor to initialize dependencies
        public AccountController(IMapper mapper, IDishes dishes, IReservation reservation, ISession sessionService, IHttpContextAccessor httpContextAccessor, ICartService cartService, IUser userService, ApplicationDbContext dbContext)
        {
            _reservation = reservation;
            _dishes = dishes;
            _dbContext = dbContext;
            _sessionService = sessionService;
            _httpContextAccessor = httpContextAccessor;
            _cartService = cartService;
            _userService = userService;
            _mapper = mapper;
        }

        // GET: /api/account/profile
        [HttpGet("getReservation")]
        public async Task<IActionResult> GetAllReservationsAsync()
        {
            try
            {
                // Получаем идентификатор пользователя из сессии
                var userId = _sessionService.GetUserId();

                // Проверка, если пользователь не найден
                if (userId == Guid.Empty)
                {
                    // Возвращаем 401 статус, что означает "не авторизован"
                    return Unauthorized(new { Message = "User not authenticated. Please login." });
                }

                // Получаем информацию о пользователе
                var user = await _userService.GetOneUserByIdAsync(userId);

                // Проверяем статус пользователя
                if (user?.Status == true)
                {
                    // Если метод GetAllReservations асинхронный, нужно использовать await
                    var reservHistory =  _reservation.GetAllReservations(userId);

                    return Ok(reservHistory);
                }

                // Возвращаем ошибку, если пользователь не найден или не активен
                return BadRequest(new { Message = "User not found or inactive" });
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                _logger.LogError(ex, "An error occurred while fetching reservations.");
                // Возвращаем ответ с кодом 500
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }



        // GET: /api/account/contacts
        [HttpGet("contacts")]
        public async Task<IActionResult> GetContacts()
        {
            var userId = _sessionService.GetUserId();
            var user = await _userService.GetOneUserByIdAsync(userId);

            if (user?.Status == true)
            {
                return Ok(user.User);
            }

            return BadRequest(new { Message = "User not found or inactive" });
        }

        // GET: /api/account/discounts
        [HttpGet("discounts")]
        public IActionResult GetDiscounts()
        {
            // Placeholder for discounts data
            return Ok(new { Discounts = "Discount data placeholder" });
        }

        // GET: /api/address
        [HttpGet("address")]
        public async Task<IActionResult> Getaddress()
        {
            var userId = _sessionService.GetUserId();
            var addressResponse = await _userService.GetOneAddressByUserIdAsync(userId);

            if (addressResponse.Success)
            {
                return Ok(addressResponse.DeliveryAddress);
            }

            return BadRequest(new { Message = "Address not found" });
        }

        // POST: /api/account/updatecontacts
        [HttpPost("updateContacts")]
        public async Task<IActionResult> UpdateContacts([FromBody] UserDataDto data)
        {
           var userData =  _mapper.Map<UserData>(data);
            var userId = _sessionService.GetUserId();
            userData.Id = userId;

            var updateResponse = await _userService.UpdateUser(userData);

            if (!updateResponse.Status)
            {
                return BadRequest();
            }

            return Ok(new { Message = "User updated successfully.", User = updateResponse.User });
        }

        // POST: /api/account/updateaddress
        [HttpPost("updateAddress")]
        public async Task<IActionResult> UpdateAddress([FromBody] DeliveryAddress data)
        {
            var userId = _sessionService.GetUserId();
            var userResponse = await _userService.GetOneUserByIdAsync(userId);

            if (!userResponse.Status)
            {
                return BadRequest(new { Message = userResponse.Message });
            }

            var updateResponse = await _userService.UpdateAddress(userResponse.User, data);

            if (!updateResponse.Status)
            {
                return BadRequest(new { Message = updateResponse.Message });
            }

            return Ok(new { Message = "Address updated successfully.", User = updateResponse.User });
        }

        // POST: /api/account/forgotpassword
        [HttpPost("reset-password")]
        public IActionResult ForgotPassword([FromBody] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { Message = "Please enter a valid email address." });
            }

            var resetResponse = _userService.ChangeUserPassword(email);

            if (resetResponse.Status)
            {
                return Ok(new { Message = $"A new password has been sent to {email}." });
            }

            return BadRequest(new { Message = resetResponse.Message ?? "Failed to reset password. Please try again." });
        }

        // POST: /api/account/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _sessionService.UserLogout();
            return Ok(new { Message = "Logout successful" });
        }
    }
}
