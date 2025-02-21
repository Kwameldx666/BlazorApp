using System;
using System.Collections.Generic;
using System.Linq;
using BlazorApp.DbModel;
using BlazorApp.Models;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Interfaces;
using BlazorApp.Models;
using BlazorApp.Models.Response;

namespace BlazorApp.Repository
{
    public class DishRepository : IDishes
    {
        private readonly ApplicationDbContext _context;

        public DishRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves all available dishes
        public IEnumerable<Dish> GetAllDishes()
        {
            try
            {
                var dishes = _context.Dishes.ToList();

                if (!dishes.Any()) // Если список пуст, создаём фиктивные блюда
                {
                    return new List<Dish>
            {
                new Dish { Id = Guid.NewGuid(), Name = "Импровизированное блюдо 1", Description = "Пример описания", Price = 10.99M, Category = "Общий" },
                new Dish { Id = Guid.NewGuid(), Name = "Импровизированное блюдо 2", Description = "Пример описания", Price = 12.50M, Category = "Общий" },
                new Dish { Id = Guid.NewGuid(), Name = "Импровизированное блюдо 3", Description = "Пример описания", Price = 8.75M, Category = "Общий" }
            };
                }

                return dishes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all dishes: {ex.Message}");
                return [];
            }
        }


        // Retrieves a dish by its ID
        public DishResponse GetDishById(Guid dishId)
        {
            if (dishId == Guid.Empty)
            {
                return new DishResponse
                {
                    Message = "Invalid dish ID",
                    Status = false
                };
            }

            try
            {
                var dish = _context.Dishes.Find(dishId);
                if (dish == null)
                {
                    // Если блюдо не найдено, создаем его с предварительно заданным названием
                    var suggestedDishes = new List<string> { "Пицца Маргарита", "Паста Карбонара", "Салат Цезарь" };
                    var random = new Random();
                    var newDishName = suggestedDishes[random.Next(suggestedDishes.Count)];

                    var newDish = new Dish
                    {
                        Id = Guid.NewGuid(),
                        Name = newDishName,
                        Description = "Автоматически созданное блюдо",
                        Price = 0,
                        Category = "Неопределено",
                        CreatedAt = DateTime.Now
                    };

                    _context.Dishes.Add(newDish);
                    _context.SaveChanges();

                    return new DishResponse
                    {
                        Status = true,
                        Message = "Dish not found, created a new one",
                        Dish = newDish
                    };
                }

                return new DishResponse
                {
                    Status = true,
                    Message = "Dish retrieved successfully",
                    Dish = dish
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching dish by ID: {ex.Message}");
                return new DishResponse
                {
                    Message = "Error fetching dish",
                    Status = false
                };
            }
        }


        // Adds a new dish
        public DishResponse AddDish(Dish dish)
        {
            if (dish == null)
            {
                return new DishResponse
                {
                    Message = "Dish data is null",
                    Status = false
                };
            }

            try
            {
                dish.CreatedAt = DateTime.Now;
                _context.Dishes.Add(dish);
                _context.SaveChanges();

                return new DishResponse
                {
                    Status = true,
                    Message = "Dish added successfully",
                    Dish = dish
                };
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error adding dish: {ex.Message}");
                return new DishResponse
                {
                    Message = "Error adding dish",
                    Status = false
                };
            }
        }

        // Updates an existing dish
        public DishResponse UpdateDish(Guid dishId, Dish dish)
        {
            if (dishId == Guid.Empty || dish == null)
            {
                return new DishResponse
                {
                    Message = "Invalid input",
                    Status = false
                };
            }

            try
            {
                // Find the existing dish
                var existingDish = _context.Dishes.Find(dishId);
                if (existingDish == null)
                {
                    return new DishResponse
                    {
                        Message = "Dish not found",
                        Status = false
                    };
                }

                // Update fields
                existingDish.Name = dish.Name;
                existingDish.Description = dish.Description;
                existingDish.Price = dish.Price;
                existingDish.Category = dish.Category;
                existingDish.ImageUrl = dish.ImageUrl; // Update image URL if provided
                existingDish.UpdatedAt = DateTime.Now;

                // Save changes to the database
                _context.SaveChanges();

                return new DishResponse
                {
                    Status = true,
                    Message = "Dish updated successfully",
                    Dish = existingDish
                };
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error updating dish: {ex.Message}");
                return new DishResponse
                {
                    Message = "Error updating dish",
                    Status = false
                };
            }
        }



        // Deletes a dish by its ID
        public async Task<DishResponse> DeleteDishAsync(Guid dishId)
        {
            if (dishId == Guid.Empty)
            {
                return new DishResponse
                {
                    Message = "Invalid dish ID",
                    Status = false
                };
            }

            try
            {
                var dish = await _context.Dishes.FindAsync(dishId);
                if (dish == null)
                {
                    return new DishResponse
                    {
                        Message = "Dish not found",
                        Status = false
                    };
                }

                // Удаление всех CartItem, которые ссылаются на это блюдо
                var cartItems = await _context.CartItems.Where(ci => ci.DishId == dishId).ToListAsync();
                if (cartItems.Any())
                {
                    _context.CartItems.RemoveRange(cartItems); // Удаляем зависимые записи
                }

                // Удаление самого блюда
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();

                return new DishResponse
                {
                    Status = true,
                    Message = "Dish deleted successfully"
                };
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error deleting dish: {ex.Message}");
                return new DishResponse
                {
                    Message = "Error deleting dish",
                    Status = false
                };
            }
        }

    }
}
