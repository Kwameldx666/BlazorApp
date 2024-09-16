using BlazorApp.Models;
using BlazorApp.Models.Response;

namespace BlazorApp.Interfaces
{
    public interface IDishes
    {
        // Retrieve all available dishes
        IEnumerable<Dish> GetAllDishes();

        // Retrieve a dish by its identifier
        DishResponse GetDishById(Guid dishId);

        // Add a new dish
        DishResponse AddDish(Dish dish);

        // Update an existing dish
        DishResponse UpdateDish(Guid dishId, Dish dish);

        // Delete a dish by its identifier
        Task<DishResponse> DeleteDishAsync(Guid dishId);


    }
}
