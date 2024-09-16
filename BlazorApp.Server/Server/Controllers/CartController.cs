using Microsoft.AspNetCore.Mvc;
using BlazorApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Interfaces;
using BlazorApp.Factory;
using BlazorApp.DbModel;
using BlazorApp.Models.Enums;
using ISession = BlazorApp.Interfaces.ISession;
using BlazorApp.Shared.Models;
using BlazorApp.Shared;
namespace BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ISession _sessionService;
        private readonly IUser _userService;
        private readonly ApplicationDbContext _applicationDbContext;


        public CartController(ISession sessionService, IUser userService, ApplicationDbContext applicationDbContext)
        {
            _sessionService = sessionService;
            _userService = userService;
            _applicationDbContext = applicationDbContext;

        }

        // POST: api/Cart/Add
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CartItemRequest request)
        {
            // Проверка корректности входных данных
            if (request == null || request.DishId == Guid.Empty || string.IsNullOrWhiteSpace(request.DishName) || request.DishPrice <= 0 || request.Quantity <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid input data." });
            }

            var userID = _sessionService.GetUserId();

            // Проверка наличия пользователя
            if (userID == Guid.Empty)
            {
                return BadRequest(new { success = false, message = "User not found." });
            }

            // Создание объекта CartItem
            var cartItem = new CartItem
            {
                UserId = userID,
                Name = request.DishName,
                Price = request.DishPrice,
                DishId = request.DishId,
                Quantity = request.Quantity,
                TotalPrice = request.DishPrice * request.Quantity
            };

            try
            {
                // Проверка существования пользователя в базе данных
                var user = await _userService.GetOneUserByIdAsync(userID);
                if (user == null)
                {
                    return BadRequest(new { success = false, message = "User not found." });
                }

                // Выбор подходящей корзины в зависимости от роли пользователя
                CartFactory cartFactory = user.User.Roles == Role.VIP
                    ? new VipFactoryCart(_applicationDbContext)
                    : new RegularUserCart(_applicationDbContext);

                var userCart = cartFactory.CreateCart();

                // Добавление товара в корзину
                var result = await userCart.AddItemToCartAsync(userID, cartItem);

                // Возврат результата в зависимости от успеха операции
                if (result.success)
                {
                    return Ok(new { success = true, message = result.message });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.message });
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки и возврат статуса 500
                Console.Error.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while adding items to the cart." });
            }
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<ActionResult> Cart()
        {
            try
            {
                var userID = _sessionService.GetUserId();
                if (userID == Guid.Empty)
                {
                    return Unauthorized(new { message = "User not found. Please log in." });
                }

                var user = await _userService.GetOneUserByIdAsync(userID);
                if (user == null)
                {
                    return Unauthorized(new { message = "User not found. Please log in." });
                }

                CartFactory cartFactory = user.User.Roles == Role.VIP
                    ? new VipFactoryCart(_applicationDbContext)
                    : new RegularUserCart(_applicationDbContext);

                var cart = cartFactory.CreateCart();
                Cart userCart = await cart.GetCartAsync(userID);
                var cartDto = ConvertToDto(userCart);

                return Ok(cartDto);


            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}", stackTrace = ex.StackTrace });

            }
        }
        private CartDto ConvertToDto(Cart cart)
        {
            return new CartDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Total = cart.Total,
                CartItems = cart.CartItems.Select(item => new CartItemDto
                {
                    DishId = item.DishId,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice
                }).ToList()
            };
        }

        // POST: api/Cart/Remove
        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] Guid dishId)
        {
            try
            {
                var userID = _sessionService.GetUserId();
                if (userID == Guid.Empty)
                {
                    return Unauthorized(new { message = "User not found. Please log in." });
                }

                var user = await _userService.GetOneUserByIdAsync(userID);

                CartFactory cartFactory = user.User.Roles == Role.VIP
                    ? new VipFactoryCart(_applicationDbContext)
                    : new RegularUserCart(_applicationDbContext);

                var cart = cartFactory.CreateCart();
                await cart.RemoveItemFromCartAsync(userID, dishId);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while removing the item." });
            }
        }

        // POST: api/Cart/UpdateQuantity
        [HttpPost("update-quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            if (request.Quantity <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid quantity." });
            }

            try
            {
                var userID = _sessionService.GetUserId();
                if (userID == Guid.Empty)
                {
                    return Unauthorized(new { message = "User not found. Please log in." });
                }

                var user = await _userService.GetOneUserByIdAsync(userID);

                CartFactory cartFactory = user.User.Roles == Role.VIP
                    ? new VipFactoryCart(_applicationDbContext)
                    : new RegularUserCart(_applicationDbContext);

                var cart = cartFactory.CreateCart();
                await cart.UpdateItemQuantityAsync(userID, request.DishId, request.Quantity);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while updating the item quantity." });
            }
        }
    }

   
}
