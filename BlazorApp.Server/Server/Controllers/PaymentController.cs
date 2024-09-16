using Microsoft.AspNetCore.Mvc;
using BlazorApp.Interfaces;
using BlazorApp.Models;
using BlazorApp.Models.Response;
using System;
using System.Threading.Tasks;

using ISession = BlazorApp.Interfaces.ISession;

namespace BlazorApp.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ISession _sessionService;

        public PaymentController(IPaymentGateway paymentGateway, ISession sessionService)
        {
            _paymentGateway = paymentGateway;
            _sessionService = sessionService;
        }

        [HttpGet("totalPrice")]
        public IActionResult Amount()
        {
            var userId = _sessionService.GetUserId();

            // Проверка на валидность пользователя
            if (userId == Guid.Empty)
            {
                return Unauthorized(new { message = "User not logged in." });
            }

            // Получаем общую сумму из paymentGateway
            decimal totalPrice = _paymentGateway.TotalPrice(userId);

            // Возвращаем результат в формате JSON
            return Ok(totalPrice);
        }
        // POST: api/payment/submit
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitPayment([FromBody] PaymentDetails paymentDetails)
        {
            // Получаем идентификатор пользователя из сессии
            Guid userId = _sessionService.GetUserId();

            // Проверка валидности идентификатора пользователя
            if (userId == Guid.Empty)
            {
                return BadRequest(new { message = "User not found. Please log in." });
            }

            // Обработка платежа через шлюз
            PaymentResponse paymentResponse = _paymentGateway.ProcessPayment(userId, paymentDetails);

            // Проверка результата обработки платежа
            if (paymentResponse.Success)
            {
                return Ok(new { success = true, message = "Payment processed successfully." });
            }
            else
            {
                return BadRequest(new { success = false, message = "Payment processing failed. Please try again." });
            }
        }



        // GET: api/payment/confirmation
        [HttpGet("confirmation")]
        public IActionResult GetConfirmation()
        {
            // Возврат подтверждения успешного платежа
            return Ok(new { message = "Payment successfully confirmed." });
        }
    }
}
