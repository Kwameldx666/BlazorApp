using BlazorApp.DbModel;
using BlazorApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RegularUserCartService : ICartService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public RegularUserCartService(ApplicationDbContext dbContext)
    {
        _applicationDbContext = dbContext;
    }

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
            return (true, "This item is already in the cart.");
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



    public async Task RemoveItemFromCartAsync(Guid userId, Guid dishId)
    {
        var cart = await GetCartAsync(userId);
        if (cart != null)
        {
            // Find the item to remove and remove it from the cart
            var itemToRemove = cart.CartItems.FirstOrDefault(i => i.DishId == dishId);
            if (itemToRemove != null)
            {
                cart.CartItems.Remove(itemToRemove);
                await _applicationDbContext.SaveChangesAsync();
            }
        }
    }

    public async Task UpdateItemQuantityAsync(Guid userId, Guid dishId, int quantity)
    {
        try
        {
            var cart = await GetCartAsync(userId);
            if (cart != null)
            {
                // Find the item to update and set the new quantity
                var itemToUpdate = cart.CartItems.FirstOrDefault(i => i.DishId == dishId);
                if (itemToUpdate != null)
                {
                    itemToUpdate.Quantity = quantity;
                    itemToUpdate.TotalPrice = itemToUpdate.Price * quantity;
                    await _applicationDbContext.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception (use a logging framework in a real application)
            Console.Error.WriteLine($"Error updating item quantity: {ex.Message}");
        }
    }

    public async Task<Cart> GetCartAsync(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty)
            {
                // Create a new cart if the user ID is empty
                return new Cart
                {
                    CartId = Guid.NewGuid(),
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
            }

            // Retrieve the cart from the database, including its items
            var cart = await _applicationDbContext.Cart
                                        .Include(c => c.CartItems)
                                        .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // Create a new cart if none is found
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
        catch (Exception ex)
        {
            // Log the exception (use a logging framework in a real application)
            Console.Error.WriteLine($"Error retrieving cart: {ex.Message}");

            // Return a new cart as a fallback
            return new Cart
            {
                CartId = Guid.NewGuid(),
                UserId = userId,
                CartItems = new List<CartItem>()
            };
        }
    }

    public async Task<decimal> CalculateTotalAsync(Guid userId)
    {
        var cart = await GetCartAsync(userId);
        // Calculate the total price of the items in the cart
        return cart.CartItems.Sum(i => i.TotalPrice);
    }
}
