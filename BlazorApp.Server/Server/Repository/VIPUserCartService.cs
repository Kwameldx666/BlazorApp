using BlazorApp.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Models
{
    public class VIPUserCartService : ICartService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public VIPUserCartService(ApplicationDbContext dbContext)
        {
            _applicationDbContext = dbContext;
        }

        // Adds an item to the cart for a VIP user and applies a discount
        public async Task<(bool success, string message)> AddItemToCartAsync(Guid userId, CartItem item)
        {
            // Проверка на null для item
            if (item == null)
            {
                return (false, "CartItem cannot be null.");
            }

            // Проверка существования пользователя
            var user = await _applicationDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return (false, "User not found. Cannot add item to cart.");
            }

            // Получаем корзину пользователя
            var cart = await GetCartAsync(userId);

            // Если корзина не найдена, создаем новую
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };

                _applicationDbContext.Cart.Add(cart);
                await _applicationDbContext.SaveChangesAsync();
            }

            // Проверка, что элемент уже не в корзине
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.DishId == item.DishId);
            if (existingItem != null)
            {
                return (false, "This item is already in the cart.");
            }

            // Добавляем элемент в корзину
            item.CartId = cart.CartId;
            item.Cart = cart;
            cart.CartItems.Add(item);

            // Сохраняем изменения
            try
            {
                await _applicationDbContext.SaveChangesAsync();
                return (true, "Item added to cart successfully.");
            }
            catch (DbUpdateException ex)
            {
                // Логирование ошибки
                Console.Error.WriteLine($"Error: {ex.Message}");
                return (false, "An error occurred while saving the cart item to the database.");
            }
        }

        // Removes an item from the cart
        public async Task RemoveItemFromCartAsync(Guid userId, Guid dishId)
        {
            if (dishId == Guid.Empty)
            {
                throw new ArgumentException("Invalid dish ID", nameof(dishId));
            }

            var cart = await GetCartAsync(userId);
            var itemToRemove = cart.CartItems.FirstOrDefault(i => i.DishId == dishId);
            if (itemToRemove != null)
            {
                cart.CartItems.Remove(itemToRemove);
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        // Updates the quantity of an item in the cart and recalculates the total price
        public async Task UpdateItemQuantityAsync(Guid userId, Guid dishId, int quantity)
        {
            if (quantity < 1)
            {
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
            }

            var cart = await GetCartAsync(userId);
            var itemToUpdate = cart.CartItems.FirstOrDefault(i => i.DishId == dishId);
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = quantity;
                itemToUpdate.TotalPrice = ApplyVIPDiscount(itemToUpdate.Price) * quantity; // Recalculate total price
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        // Retrieves the cart for a user, creates a new cart if it doesn't exist
        public async Task<Cart> GetCartAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }

            var cart = await _applicationDbContext.Cart
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // Create a new cart if one does not exist
                cart = new Cart
                {
                    CartId = Guid.NewGuid(),
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };

                _applicationDbContext.Cart.Add(cart);
                await _applicationDbContext.SaveChangesAsync();
            }

            return cart;
        }

        // Calculates the total price of all items in the cart
        public async Task<decimal> CalculateTotalAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }

            var cart = await GetCartAsync(userId);
            return cart.CartItems.Sum(i => i.TotalPrice);
        }

        // Applies a VIP discount to the item price
        private static decimal ApplyVIPDiscount(decimal price)
        {
            // Example VIP discount (10% discount)
            return price * 0.9m;
        }

      
    }
}
